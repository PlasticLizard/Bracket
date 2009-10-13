using System;
using System.Net;
using Kayak;

namespace Bracket.Hosting.Kayak
{
    public class KayakRackServer : IDisposable
    {
        private KayakServer _server;
        private Rack _rack;

        public KayakRackServer(int port):this(port,IPAddress.Any)
        {
        }

        public KayakRackServer(int port,IPAddress ipAddress)
        {
            _server = new KayakServer {ListenEP = new IPEndPoint(ipAddress, port)};
            _server.Responders.Add(new RackResponder(this));
        }

        public Rack Rack { get { return _rack;}}

        public void Start()
        {
            Start(new RubyEnvironment());
        }

        public void Start(RubyEnvironment rubyEnvironment)
        {
            _rack = new Rack(rubyEnvironment);
            _server.Start();
        }

        public void Stop()
        {
            _server.Stop();
            _rack = null;
        }

        public void Dispose()
        {
            if (_server != null)
                _server.Stop();
            _server = null;
        }
    }
}
