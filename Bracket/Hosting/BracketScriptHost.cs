using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Bracket.Hosting
{
    public class BracketScriptHost : ScriptHost
    {
        private readonly BracketPlatformAdaptationLayer _platformAdaptationLayer = new BracketPlatformAdaptationLayer();

        public override PlatformAdaptationLayer PlatformAdaptationLayer
        {
            get
            {
                return _platformAdaptationLayer;
            }
        }

    }
}
