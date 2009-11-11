using System;
using System.IO;
using HttpServer;

namespace Bracket.Hosting.Default
{
    public static class HttpServerHelper
    {
        public static RackRequest ToRackRequest(this IHttpRequest source)
        {
            //Workaround for HttpServer position being at EOF
            if (source.Body.CanSeek)
                source.Body.Position = 0;

            var target = new RackRequest();
            target.RequestMethod = source.Method;
            target.ApplicationPath = String.Empty; //Currently not supporting multiple applications per server instance
            target.ResourcePath = source.Uri.LocalPath;
            target.QueryString = source.QueryString.ToString(true);
            target.ServerName = source.Uri.Host;
            target.ServerPort = source.Uri.Port;
            target.UrlScheme = source.Uri.Scheme.ToLower();
            target.Body = new StreamReader(source.Body).ReadToEnd();
            target.RemoteAddress = source.Uri.Host;
            target.ServerProtocol = source.Headers["SERVER_PROTOCOL"];
            target.RequestPath = source.UriPath;
            target.RemoteAddress = source.RemoteEndPoint.Address.ToString();
            target.HttpVersion = source.HttpVersion;
            target.RequestUri = source.Uri.ToString();
            target.GatewayInterface = source.Headers["GATEWAY_INTERFACE"];
            target.ContentType = source.Headers["CONTENT_TYPE"];
            target.ContentLength = source.ContentLength;
            target.AddHeaders(source.Headers);
            return target;
        }
    }
}