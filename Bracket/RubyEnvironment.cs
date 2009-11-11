using System;
using System.Configuration;
using System.IO;

namespace Bracket
{
    public class RubyEnvironment
    {
        public static string DefaultGemPath = "";
        public const string ApplicationRootConfigurationKey = "ApplicationRootPath";
        public const string GemPathConfigurationKey = "GemPath";

        public string ApplicationRootPath { get; set; }
        public string GemPath { get; set; }
        public RubyEngine Engine { get; set; }

        private bool _configured;

        public RubyEnvironment():this(null)
        {
        }

        public RubyEnvironment(Action<RubyEnvironment> configure)
        {
            Engine = new RubyEngine();
            ApplicationRootPath = AppDomain.CurrentDomain.BaseDirectory;
            string configured = ConfigurationManager.AppSettings[ApplicationRootConfigurationKey];
            if (!String.IsNullOrEmpty(configured))
            {
                if (Path.IsPathRooted(configured) == false)
                    configured = Path.Combine(ApplicationRootPath, configured);

                if (Directory.Exists(configured))
                    ApplicationRootPath = configured;
            }
                

            GemPath = DefaultGemPath;
            configured = ConfigurationManager.AppSettings[GemPathConfigurationKey];
            if (!String.IsNullOrEmpty(configured) && Directory.Exists(configured))
                GemPath = configured;

            if (configure != null)
                configure(this);

            Initialize();
        }

        protected void Initialize()
        {
            if (_configured) return;
            lock(this)
            {
                if (_configured)
                    return;

                if (!String.IsNullOrEmpty(GemPath))
                {
                    Engine.AddLoadPath(GemPath);
                }
                
                if (!String.IsNullOrEmpty(ApplicationRootPath))
                    Engine.AddLoadPath(ApplicationRootPath);

                Engine.Require("rubygems");

                _configured = true;
                
            }
        }
    }
}