using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Bracket.Events
{
   public class FlatFileEventLogger : IBracketEventLogger, IDisposable
   {
       public const string DefaultLogFileName = "bracket_events.log";
       private readonly object _queueLock = new object();
       private readonly string _logFileName;
       private readonly Queue<BracketEvent> _logEntryQueue = new Queue<BracketEvent>();
       private readonly ManualResetEvent _queueWait = new ManualResetEvent(false);
       private Thread _worker;
       private bool _stopRequested;

       public FlatFileEventLogger():this(DefaultLogFileName)
       {
       }

       public FlatFileEventLogger(string logFileName)
       {
           _logFileName = logFileName;
           StartLogging();
       }

       ~FlatFileEventLogger()
       {
           Dispose();
       }

       private void StartLogging()
       {
           if (_worker != null)
               return;

           BracketEvent.Event += LogEvent;
           _worker = new Thread(WriteLogEntries) {IsBackground = true};
           _worker.Start();
       }

       private void StopLogging()
       {
          if (_worker == null)
              return;

           BracketEvent.Event-=LogEvent;
           _stopRequested = true;
           _queueWait.Set();
           //Let's give it a chance to finish its dirty business.
           _worker.Join(TimeSpan.FromMilliseconds(500));
           _worker = null;
           
       }

       private void WriteLogEntries(object state)
       {
           while (!_stopRequested)
           {
               _queueWait.WaitOne();

               StreamWriter writer;
               using (writer = File.AppendText(_logFileName))
               {
                   BracketEvent nextEvent;
                   while ((nextEvent = GetNextEvent()) != null)
                   {
                       writer.WriteLine(nextEvent.ToString());
                   }
                   writer.Flush();
               }
           }
       }

       public void LogEvent(object sender, BracketEvent e)
       {
           lock (_queueLock)
           {
               _logEntryQueue.Enqueue(e);
               _queueWait.Set();
           }
       }

       private BracketEvent GetNextEvent()
       {
           lock(_queueLock)
           {
               if (_logEntryQueue.Count < 1)
                   return null;

               if (_logEntryQueue.Count == 1)
                   _queueWait.Reset();

               return _logEntryQueue.Dequeue();
           }
       }


       public void Dispose()
       {
           StopLogging();
       }
   }
}
