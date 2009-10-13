using System;
using System.IO;
using System.Net;

namespace Bracket.Hosting.SystemWeb
{
    public class HttpListenerRackServer : IDisposable
    {
        private SimpleWebServer _server;
        private string[] _listeningUris;
        private Rack _rack;

        public event EventHandler<HttpRequestErrorEventArgs> RequestProcessingError = (sender, args) => { };

        public HttpListenerRackServer(int port):this("http://+:" + port + "/")
        {
        }

        public HttpListenerRackServer(params string[] listeningUris)
        {
            _listeningUris = listeningUris;
        }

        public void Start(RubyEnvironment rubyEnvironment)
        {
             _rack = new Rack(rubyEnvironment);
            _server = new SimpleWebServer(_listeningUris);
            _server.RequestHandlingError += RequestProcessingError;
            _server.IncomingRequest += ProcessRequest;
            _server.Start();
        }

        public void Start()
        {
            Start(new RubyEnvironment());
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
            if (_server != null)
                _server.Dispose();
        }

        private void ProcessRequest(object sender, HttpRequestEventArgs e)
        {
            try
            {
                var request = new RackRequest();
                var response = new RackResponse(e.RequestContext.Response.OutputStream);

                PrepareRackRequest(e.RequestContext.Request, request);
                _rack.HandleRequest(request, response);

                response.Status = response.Status;
                foreach (var header in response.Headers)
                    if (header.Key.ToLower() != "content-length") //Cannot set directly
                        e.RequestContext.Response.Headers.Add(header.Key, header.Value);

                
            }
            catch (HttpListenerException)
            {
                return;
            }
            catch (Exception ex)
            {
                RequestProcessingError(this, new HttpRequestErrorEventArgs(e.RequestContext,ex));
                var writer = new StreamWriter(e.RequestContext.Response.OutputStream);
                writer.Write(ex.ToString());
                writer.Flush();
                
            }
        }

        private static void PrepareRackRequest(HttpListenerRequest source,RackRequest target)
        {
            target.RequestMethod = source.HttpMethod;
            target.ApplicationPath = String.Empty;
            //Currently not supporting virtual directories / applications
            target.ResourcePath = source.Url.LocalPath;
            target.QueryString = source.Url.Query;
            target.ServerName = source.Url.Host;
            target.ServerPort = source.Url.Port;
            target.UrlScheme = source.Url.Scheme.ToLower();
            target.Body = new StreamReader(source.InputStream).ReadToEnd();
            target.RemoteAddress = source.Url.Host;
            target.ServerProtocol = source.Headers["SERVER_PROTOCOL"];
            target.RequestPath = source.Url.AbsolutePath;
            target.RemoteAddress = source.RemoteEndPoint.Address.ToString();
            target.HttpVersion = source.ProtocolVersion.ToString();
            target.RequestUri = source.Url.ToString();
            target.GatewayInterface = source.Headers["GATEWAY_INTERFACE"];
            target.ContentType = source.Headers["CONTENT_TYPE"];
            target.ContentLength = Convert.ToInt32(source.ContentLength64);
            target.AddHeaders(source.Headers);

        }
    }
}