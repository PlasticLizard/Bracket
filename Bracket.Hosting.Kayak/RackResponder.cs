using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Kayak;

namespace Bracket.Hosting.Kayak
{
    public class RackResponder : IKayakResponder
    {
        private KayakRackServer  _server;
        public RackResponder(KayakRackServer server)
        {
            _server = server;
        }

        public bool RespondToContext(KayakContext context)
        {
            var rackRequest = context.Request.ToRackRequest();
         
            var rackResponse = new RackResponse(context.Response.GetDirectOutputStream());

            //this will write to the body. We may want to set headers first, so may need some thought.
            _server.Rack.HandleRequest(rackRequest, rackResponse);

            context.Response.StatusCode = rackResponse.Status;
            foreach (var header in rackResponse.Headers)
                context.Response.Headers.Add(header.Key, header.Value);

            return true;
        }
    }
}
