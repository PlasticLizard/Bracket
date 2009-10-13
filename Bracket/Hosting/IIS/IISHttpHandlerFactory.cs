using System.Web;

namespace Bracket.Hosting.Iis
{
    public class IISHttpHandlerFactory : IHttpHandlerFactory
    {
        private Rack _rack = new Rack();

        public IISHttpHandlerFactory()
        {
            _rack = new Rack();
        }

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            return new IISHttpHandler(_rack);   
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}