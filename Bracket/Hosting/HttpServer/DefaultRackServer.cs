using System;
using System.Net;
using HttpServer;
using HttpListener=HttpServer.HttpListener;

namespace Bracket.Hosting.HttpServer
{
    public class DefaultRackServer : IDisposable
    {
        private HttpListener _server;
        private Rack _rack;

        public DefaultRackServer(int port):this(port,IPAddress.Any)
        {}

        public DefaultRackServer(int port,IPAddress ipAddress):this(port,ipAddress,new NullLogWriter())
        {}

        public DefaultRackServer(int port,IPAddress ipAddress,ILogWriter logWriter)
        {
            _server = HttpListener.Create(logWriter, ipAddress, port);
            _server.AddHandler(null,null,null,true,ProcessRequest);
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
            if (rubyEnvironment == null)
                _rack = new Rack();
            else
            {
                _rack = new Rack(rubyEnvironment);
            }

            _server.Start(tcpBacklog);
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

        private void ProcessRequest(IHttpClientContext context, IHttpRequest request, IHttpResponse response)
        {
            _server.LogWriter.Write(this,LogPrio.Trace,String.Format("Begin handling Rack request for {0}",request.Uri));
            
            var rackRequest = request.ToRackRequest();
            var rackResponse = new RackResponse(response.Body);

            //this will write to the body. We may want to set headers first, so may need some thought.
            _rack.HandleRequest(rackRequest, rackResponse);

            response.Status = (HttpStatusCode)rackResponse.Status;
            foreach (var header in rackResponse.Headers)
                response.AddHeader(header.Key, header.Value);

            _server.LogWriter.Write(this, LogPrio.Trace, String.Format("Finished handling Rack request for {0}", request.Uri));
        }
    }
}