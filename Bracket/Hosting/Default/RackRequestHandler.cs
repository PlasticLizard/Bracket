using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HttpServer;
using HttpServer.HttpModules;
using HttpServer.Sessions;

namespace Bracket.Hosting.Default
{
    public class RackRequestHandler : HttpModule
    {
        private readonly ILogWriter _log;
        private readonly Rack _rack;

        public RackRequestHandler(ILogWriter log, Rack rack)
        {
            _log = log;
            _rack = rack;
        }

        public override bool Process(IHttpRequest request, IHttpResponse response, IHttpSession session)
        {
           _log.Write(this, LogPrio.Trace, String.Format("Begin handling Rack request for {0}", request.Uri));

            var rackRequest = request.ToRackRequest();
            var rackResponse = new RackResponse(response.Body);

            //this will write to the body. We may want to set headers first, so may need some thought.
            _rack.HandleRequest(rackRequest, rackResponse);

            response.Status = (HttpStatusCode)rackResponse.Status;
            foreach (var header in rackResponse.Headers)
                response.AddHeader(header.Key, header.Value);

            _log.Write(this, LogPrio.Trace, String.Format("Finished handling Rack request for {0}", request.Uri));

            return true;
        }
    }
}
