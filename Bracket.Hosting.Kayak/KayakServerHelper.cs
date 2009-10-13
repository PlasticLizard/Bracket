using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kayak;

namespace Bracket.Hosting.Kayak
{
    public static class KayakServerHelper
    {
        public static RackRequest ToRackRequest(this KayakRequest source)
        {
            var uri = new Uri("http://" + source.Headers["Host"] + source.RequestUri);

            var target = new RackRequest();
            target.RequestMethod = source.Method;
            target.ApplicationPath = String.Empty; //Currently not supporting multiple applications per server instance
            target.ResourcePath = uri.LocalPath;
            target.QueryString = uri.Query;
            target.ServerName = uri.Host;
            target.ServerPort = uri.Port;
            target.UrlScheme = uri.Scheme.ToLower();
            if (source.ContentLength > 0)
                target.Body = new StreamReader(source.InputStream).ReadToEnd();
            target.RemoteAddress = uri.Host;
            target.ServerProtocol = source.Headers["SERVER_PROTOCOL"];
            target.RequestPath = source.Path;
            target.RemoteAddress = "127.0.0.1";
            target.HttpVersion = source.HttpVersion;
            target.RequestUri = uri.ToString();
            target.GatewayInterface = source.Headers["GATEWAY_INTERFACE"];
            target.ContentType = source.Headers["CONTENT_TYPE"];
            target.ContentLength = Convert.ToInt32(source.ContentLength);
            foreach (var header in source.Headers)
                target.AddHeader(header.Name, header.Value);
            return target;
        }
    }
}
