using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bracket.Events
{
    public enum BracketEventType
    {
        AtomicOperation,
        BeginOperation,
        CompleteOperation,
        Error,
        Trace
    }

    public enum BracketEvents
    {
        Info,
        Exception,
        OpenOutputFile,
        OpenInputFile
    }

    public class BracketEvent : EventArgs
    {
        private static readonly IBracketEventLogger Logger = new FlatFileEventLogger();

        public static event EventHandler<BracketEvent> Event = (sender, args) =>
                                                                   {
                                                                       if(LogAllEvents)Logger.LogEvent(sender,args);
                                                                   };

        //This is not threadsafe, but the consequences of race conditions
        //are trivial (maybe an entry gets logged just after this is set to false, or an entry gets
        //missed that was submitted just prior to setting to true, either way, no big deal.
        //Logger.LogEvent *is* threadsafe.
        public static bool LogAllEvents;

        public static void PublishEvent(object eventSource,BracketEvent e)
        {
           Event(eventSource, e);
        }

        public static void PublishEvent(object eventSource,BracketEventType eventType,BracketEvents eventName)
        {
            PublishEvent(eventSource, eventType, eventName, null);
        }

        public static void PublishEvent(object eventSource,BracketEventType eventType,BracketEvents eventName,string eventDescription)
        {
            Event(eventSource, new BracketEvent { EventType = eventType, EventName = eventName,EventDescription = eventDescription });
        }

        public static void PublishError(object eventSource,Exception error)
        {
            Event(eventSource,
                  new BracketEvent
                      {
                          EventType = BracketEventType.Error,
                          Error = error,
                          EventDescription = error.ToString(),
                          EventName = BracketEvents.Exception
                      });
        }

        public BracketEvent()
        {
            EventTime = DateTime.Now;
        }

        public DateTime EventTime { get; set; }
        public BracketEvents EventName { get; set; }
        public BracketEventType EventType { get; set; }
        public string EventDescription { get; set; }
        public Exception Error { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1} : {2}, {3}", EventTime, EventType, EventName,
                                 EventDescription ?? EventName.ToString());
        }
    }
}