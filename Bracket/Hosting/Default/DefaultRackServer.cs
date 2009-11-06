using System;
using System.Net;
using HttpServer;
using HttpServer.HttpModules;


namespace Bracket.Hosting.Default
{
    public class DefaultRackServer : IDisposable
    {
        private HttpServer.HttpServer _server;
        private Rack _rack;
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        private readonly ILogWriter _logWriter;

        public DefaultRackServer(int port):this(port,IPAddress.Any)
        {}

        public DefaultRackServer(int port,IPAddress ipAddress):this(port,ipAddress,new NullLogWriter())
        {}

        public DefaultRackServer(int port,IPAddress ipAddress,ILogWriter logWriter)
        {
            _server = new HttpServer.HttpServer(logWriter);
            _ipAddress = ipAddress;
            _port = port;
            _logWriter = logWriter;
        }

        public void Start()
        {
            Start(25);
        }

        /// <summary>
        /// Starts listening on the configured IPAddress and port for requests.
        /// </summary>
        ///The Maximum number of queued connections the underlying TCP listener will maintain
        /// <param name="tcpBacklog"></param>
        public void Start(int tcpBacklog)
        {
            Start(null, tcpBacklog);
        }

        public void Start(RubyEnvironment rubyEnvironment)
        {
            Start(rubyEnvironment,25);
        }

        public void Start(RubyEnvironment rubyEnvironment, int tcpBacklog)
        {
            _rack = rubyEnvironment == null ? new Rack() : new Rack(rubyEnvironment);
            _server.Add(new RackRequestHandler(_logWriter, _rack));
            _server.Start(_ipAddress, _port);
        }

        public void Stop()
        {
            if (_server != null)
                _server.Stop();
            _rack = null;
        }


        public void Dispose()
        {
            Stop();
            _server = null;
        }
    }
}