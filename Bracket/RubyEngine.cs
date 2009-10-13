using System.IO;
using IronRuby;
using IronRuby.Runtime;
using Microsoft.Scripting.Hosting;

namespace Bracket
{
    public class RubyEngine
    {
        public ScriptEngine ScriptEngine { get; private set; }
        private readonly ScriptScope _globalScope;

        public RubyEngine()
        {
            ScriptEngine = Ruby.CreateEngine();
            _globalScope = ScriptEngine.CreateScope();
        }

        public RubyContext Context
        {
            get
            {
                return Ruby.GetExecutionContext(ScriptEngine);
            }
        }

        public object Require(string file)
        {
            return Require(file, null);
        }

        public object Require(string file, string rackVersion)
        {
            var command = "";
            if (rackVersion != null)
            {
                command = string.Format("gem '{0}', '{1}';", file, rackVersion);
            }
            command += string.Format("require '{0}'", file);
            return Execute(command);
        }

        public object ExecuteFile(string fileName)
        {
            return ScriptEngine.CreateScriptSourceFromString(FindFile(fileName)).Execute(_globalScope);
        }

        public object Execute(string code)
        {
            return Execute(code, _globalScope);
        }

        public object Execute(string code, ScriptScope aScope)
        {
            return ScriptEngine.CreateScriptSourceFromString(code).Execute(aScope);
        }

        public T Execute<T>(string code)
        {
            return Execute<T>(code, _globalScope);
        }

        public T Execute<T>(string code, ScriptScope aScope)
        {
            return (T)ScriptEngine.CreateScriptSourceFromString(code).Execute(aScope);
        }

        public T ExecuteMethod<T>(object instance, string methodName, params object[] args)
        {
            return (T)ScriptEngine.Operations.InvokeMember(instance, methodName, args);
        }

        public object AddLoadPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            path = path.Replace("\\", "/");
            return ScriptEngine.Execute(string.Format(@"$LOAD_PATH.unshift '{0}'",@path));
        }

        public void SetConstant(string name, object value)
        {
            ScriptEngine.Runtime.Globals.SetVariable(name, value);
        }

        public string FindFile(string file)
        {
            foreach (var path in ScriptEngine.GetSearchPaths())
            {
                var fullPath = TryGetFullPath(path, file);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return null;
        }

        private static string TryGetFullPath(string/*!*/ dir, string/*!*/ file)
        {
            try
            {
                return Path.GetFullPath(Path.Combine(dir, file));
            }
            catch
            {
                return null;
            }
        }
    
    }
}