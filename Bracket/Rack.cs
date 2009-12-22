using System.Collections.Generic;
using System.IO;
using System.Text;
using IronRuby.Builtins;
using Microsoft.Scripting.Hosting;

namespace Bracket
{
    public class Rack
    {
        private readonly object _rackApplication;

        public Rack() : this(new RubyEnvironment()) { }

        public Rack(RubyEnvironment environment)
        {
            Ruby = environment;
            _rackApplication = Rackup();
        }

        public RubyEnvironment Ruby { get; private set; }

        public void HandleRequest(RackRequest rackRequest, RackResponse response)
        {
            ScriptScope requestScope;
            //TODO:Probably not necessary
            lock (this)
                requestScope = Ruby.Engine.ScriptEngine.CreateScope();

            Hash envHash = rackRequest.ToRubyHash(Ruby.Engine.Context);
            var rubyResponse = Ruby.Engine.ExecuteMethod<RubyArray>(_rackApplication, "call", envHash);

            try
            {
                response.Status = (int)rubyResponse[0];
                if (rubyResponse[1] != null && rubyResponse[1] is Hash)
                {
                    var headers = (Hash)rubyResponse[1];
                    foreach (KeyValuePair<object, object> header in headers)
                    {
                        foreach (string value in header.Value.ToString().Split('\n'))
                        {
                            response.AddHeader(header.Key.ToString(), value);
                        }
                    }
                }

                requestScope.SetVariable("__body__", rubyResponse[2]);
                requestScope.SetVariable("__response__", response);
                Ruby.Engine.Execute("__body__.each { |part| __response__.write part  }", requestScope);

                response.Flush();

            }
            finally
            {
                Ruby.Engine.Execute("__body__.close if __body__.respond_to?(:close)", requestScope);
            }

        }

        private object Rackup()
        {
            Ruby.Engine.Require("rubygems");
            Ruby.Engine.Require("rack");
            var fullPath = Ruby.Engine.FindFile("config.ru");
            if (fullPath != null)
            {
                var content = File.ReadAllText(fullPath, Encoding.UTF8);
                return Ruby.Engine.Execute("Rack::Builder.new { " + content + "}.to_app");
            }
            return null;
        }
    }
}