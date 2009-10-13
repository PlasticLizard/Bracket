using System;
using System.IO;
using System.Web;

namespace Bracket.Hosting.Iis
{
    public class IISHttpHandler : IHttpHandler
    {
        private Rack _rack;

        public IISHttpHandler(Rack rack)
        {
            _rack = rack;
        }

        public void ProcessRequest(HttpContext context)
        {
            var request = new RackRequest();
            var response = new RackResponse(context.Response.OutputStream);

            PrepareRackRequest(request, context.Request);

            _rack.HandleRequest(request, response);


            context.Response.StatusCode = response.Status;
            foreach (var header in response.Headers)
                context.Response.AppendHeader(header.Key, header.Value);
        }

        public bool IsReusable
        {
            get { return true; }
        }

        private static void PrepareRackRequest(RackRequest target,HttpRequest source)
        {
            target.RequestMethod = source.HttpMethod;
            target.ApplicationPath = source.ApplicationPath;
            target.ResourcePath = source.Path.Substring(
                source.Path.IndexOf(source.ApplicationPath) +
                source.ApplicationPath.Length);
            target.QueryString = source.Url.Query;
            target.ServerName = source.Url.Host;
            target.ServerPort = source.Url.Port;
            target.UrlScheme = source.Url.Scheme.ToLower();
            target.Body = new StreamReader(source.InputStream).ReadToEnd();
            target.RemoteAddress = source.Url.Host;
            target.ServerProtocol = source.Headers["SERVER_PROTOCOL"];
            target.RequestPath = source.Url.AbsolutePath;
            //TODO:This is carried over from the original IronRuby version - I'm pretty sure it isn't correct
            target.RemoteAddress = "127.0.0.1";
            target.HttpVersion = source.Headers["SERVER_PROTOCOL"];
            target.RequestUri = source.Url.ToString();
            target.GatewayInterface = source.Headers["GATEWAY_INTERFACE"];
            target.ContentType = source.Headers["CONTENT_TYPE"];
            target.ContentLength = Convert.ToInt32(source.ContentLength);
            target.AddHeaders(source.Headers);

        }
    }
}