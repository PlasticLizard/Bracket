using System;
using System.Globalization;
using System.Net;
using System.Threading;

namespace Bracket.Hosting.SystemWeb
{
    public class SimpleWebServer : IDisposable
    {
        public event EventHandler<HttpRequestEventArgs> IncomingRequest = (sender, args) => { };
        public event EventHandler<HttpRequestErrorEventArgs> RequestHandlingError = (sender, args) => { };

        public enum State
        {
            Stopped,
            Stopping,
            Starting,
            Started
        }

        private Thread _connectionManagerThread;
        private bool _disposed;
        private readonly HttpListener _listener;
        private long _runState = (long)State.Stopped;

        public State RunState
        {
            get
            {
                return (State)Interlocked.Read(ref _runState);
            }
        }

        public virtual Guid UniqueId { get; private set; }


        public SimpleWebServer(params string[] listenerUris)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("The HttpListener class is not supported on this operating system.");
            }
            if (listenerUris == null || listenerUris.Length < 1)
            {
                throw new ArgumentNullException("listenerUris");
            }
            UniqueId = Guid.NewGuid();
            _listener = new HttpListener();
            Array.ForEach(listenerUris,prefixUri=> _listener.Prefixes.Add(prefixUri));
        }

        ~SimpleWebServer()
        {
            Dispose(false);
        }

        private void ConnectionManagerThreadStart()
        {
            Interlocked.Exchange(ref _runState, (long)State.Starting);
            try
            {
                if (!_listener.IsListening)
                {
                    _listener.Start();
                }
                if (_listener.IsListening)
                {
                    Interlocked.Exchange(ref _runState, (long)State.Started);
                }

                try
                {
                    while (RunState == State.Started)
                    {
                        HttpListenerContext context = _listener.GetContext();
                        RaiseIncomingRequest(context);
                    }
                }
                catch (HttpListenerException)
                {
                    // This will occur when the listener gets shut down.
                    // Just swallow it and move on.
                }
            }
            finally
            {
                Interlocked.Exchange(ref _runState, (long)State.Stopped);
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                if (RunState != State.Stopped)
                {
                    Stop();
                }
                if (_connectionManagerThread != null)
                {
                    _connectionManagerThread.Abort();
                    _connectionManagerThread = null;
                }
            }
            _disposed = true;
        }

        private void RaiseIncomingRequest(HttpListenerContext context)
        {
            var e = new HttpRequestEventArgs(context);

            if (IncomingRequest != null)
            {
                Action async = () =>
                                   {
                                       try
                                       {
                                           IncomingRequest(this, e);
                                           context.Response.Close();
                                       }
                                       catch (Exception ex)
                                       {
                                           RequestHandlingError(this, new HttpRequestErrorEventArgs(context, ex));
                                       }
                                   };
                async.BeginInvoke(null, null);
            }

        }

        public virtual void Start()
        {
            if (_connectionManagerThread == null || _connectionManagerThread.ThreadState == ThreadState.Stopped)
            {
                _connectionManagerThread = new Thread(ConnectionManagerThreadStart)
                                               {
                                                   Name =
                                                       String.Format(CultureInfo.InvariantCulture,
                                                                     "ConnectionManager_{0}", UniqueId)
                                               };
            }
            else if (_connectionManagerThread.ThreadState == ThreadState.Running)
            {
                throw new ThreadStateException("The request handling process is already running.");
            }

            if (_connectionManagerThread.ThreadState != ThreadState.Unstarted)
            {
                throw new ThreadStateException("The request handling process was not properly initialized so it could not be started.");
            }
            _connectionManagerThread.Start();

            long waitTime = DateTime.Now.Ticks + TimeSpan.TicksPerSecond * 10;
            while (RunState != State.Started)
            {
                Thread.Sleep(100);
                if (DateTime.Now.Ticks > waitTime)
                {
                    throw new TimeoutException("Unable to start the request handling process.");
                }
            }
        }

        public virtual void Stop()
        {
            // Setting the runstate to something other than "started" and
            // stopping the listener should abort the AddIncomingRequestToQueue
            // method and allow the ConnectionManagerThreadStart sequence to
            // end, which sets the RunState to Stopped.
            Interlocked.Exchange(ref _runState, (long)State.Stopping);
            if (_listener.IsListening)
            {
                _listener.Stop();
            }
            long waitTime = DateTime.Now.Ticks + TimeSpan.TicksPerSecond * 10;
            while (RunState != State.Stopped)
            {
                Thread.Sleep(100);
                if (DateTime.Now.Ticks > waitTime)
                {
                    throw new TimeoutException("Unable to stop the web server process.");
                }
            }

            _connectionManagerThread = null;
        }
    }

    public class HttpRequestEventArgs : EventArgs
    {
        public HttpListenerContext RequestContext { get; private set; }

        public HttpRequestEventArgs(HttpListenerContext requestContext)
        {
            RequestContext = requestContext;
        }
    }

    public class HttpRequestErrorEventArgs : HttpRequestEventArgs
    {
        public Exception Error { get; private set; }
        public HttpRequestErrorEventArgs(HttpListenerContext requestContext,Exception error):base(requestContext)
        {
            Error = error;
        }
    }
}