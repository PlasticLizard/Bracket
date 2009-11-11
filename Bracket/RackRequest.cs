using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using IronRuby.Builtins;
using IronRuby.Runtime;
using IronRuby.StandardLibrary.StringIO;

namespace Bracket
{
    public class RackRequest
    {
        private readonly Dictionary<object, object> _envVariables = new Dictionary<object, object>();

        public RackRequest()
        {
            RackVersion(1, 0);
            IsMultithreaded = true;
            IsMultiProcess = false;
            IsRunOnce = false;
            RemoteAddress = "127.0.0.1";
           
        }

        public string RequestMethod
        {
            get { return (string) Get(RackEnvironmentVariables.RequestMethod); }
            set
            {
                Add(RackEnvironmentVariables.RequestMethod, value);
            }
        }

        public string ApplicationPath
        {
            get { return (string) Get(RackEnvironmentVariables.ScriptName); }
            set
            {
                string scriptName = value;
                scriptName = scriptName ?? String.Empty;
                if (scriptName.Length > 0 && !scriptName.StartsWith("/"))
                {
                    scriptName = scriptName + "/" + scriptName;
                }

                if (scriptName == "/")
                    scriptName = String.Empty;

                Add(RackEnvironmentVariables.ScriptName, scriptName);
            }
        }

        public string ResourcePath
        {
            get { return (string) Get(RackEnvironmentVariables.PathInfo); }
            set
            {
                string resourcePath = value ?? String.Empty;
                if (resourcePath == String.Empty || !resourcePath.StartsWith("/"))
                {
                    resourcePath = "/" + resourcePath;
                }
                Add(RackEnvironmentVariables.PathInfo, resourcePath);
            }
        }

        public string QueryString
        {
            get { return (string) Get(RackEnvironmentVariables.QueryString); }
            set { Add(RackEnvironmentVariables.QueryString, value ?? String.Empty);}
        }

        public string ServerName
        {
            get { return (string) Get(RackEnvironmentVariables.ServerName); }
            set { Add(RackEnvironmentVariables.ServerName, value);}
        }

        public int ServerPort
        {
            get { return (int) Get(RackEnvironmentVariables.ServerPort); }
            set { Add(RackEnvironmentVariables.ServerPort, value);}
        }

        public RackRequest AddHeader(string headerName, string headerValue)
        {
            string prefix = RackEnvironmentVariables.GenericHeaderPrefix;
            if (String.Equals(headerName, "Content-Length", StringComparison.CurrentCultureIgnoreCase) ||
                String.Equals(headerName, "Content-Type", StringComparison.CurrentCultureIgnoreCase))
                prefix = String.Empty;
            headerName = headerName.ToUpper().Replace("-", "_");
            return Add(prefix + headerName, headerValue);
        }

        public RackRequest AddHeaders(NameValueCollection headers)
        {
            foreach (string headerKey in headers.AllKeys)
            {
                if (String.IsNullOrEmpty(headerKey))
                    continue;

                AddHeader(headerKey, headers[headerKey]);
            }
            return this;
        }

        public RackRequest RackVersion(int major, int minor)
        {
            return Add(RackEnvironmentVariables.RackVersion, new RubyArray(new[] {major, minor}));
        }

        public string UrlScheme
        {
            get { return (string) Get(RackEnvironmentVariables.RackUrlScheme); }
            set { AddHeader(RackEnvironmentVariables.RackUrlScheme, value); }
        }

        public string Body
        {
            get { return (Get(RackEnvironmentVariables.RackInput).ToString()); }
            set { Add(RackEnvironmentVariables.RackInput, value); }
        }

        public bool IsMultithreaded
        {
            get { return (bool) Get(RackEnvironmentVariables.RackMultithreaded); }
            set {Add(RackEnvironmentVariables.RackMultithreaded, value);}
        }

        public bool IsMultiProcess
        {
            get { return (bool) Get(RackEnvironmentVariables.RackMultiprocess); }
            set {Add(RackEnvironmentVariables.RackMultiprocess, value);}
        }

        public bool IsRunOnce
        {
            get { return (bool)Get(RackEnvironmentVariables.RackRunOnce);}
            set { Add(RackEnvironmentVariables.RackRunOnce, value); }
        }

        public string Host
        {
            get { return (string) Get(RackEnvironmentVariables.RemoteHost); }
            set { Add(RackEnvironmentVariables.RemoteHost, value); }
        }

        public string ServerProtocol
        {
            get { return (string) Get(RackEnvironmentVariables.ServerProtocol); }
            set { Add(RackEnvironmentVariables.ServerProtocol, value); }
        }

        public string RequestPath
        {
            get { return (string) Get(RackEnvironmentVariables.RequestPath); }
            set { Add(RackEnvironmentVariables.RequestPath, value);}
        }

        public string ServerSoftware
        {
            get { return (string) Get(RackEnvironmentVariables.ServerSoftware); }
            set { Add(RackEnvironmentVariables.ServerSoftware, value);}
        }

        public string RemoteAddress
        {
            get { return (string) Get(RackEnvironmentVariables.RemoteAddress); }
            set { Add(RackEnvironmentVariables.RemoteAddress, value);}
        }

        public string HttpVersion
        {
            get { return (string) Get(RackEnvironmentVariables.HttpVersion); }
            set { Add(RackEnvironmentVariables.HttpVersion, value);}
        }

        public string RequestUri
        {
            get { return (string) Get(RackEnvironmentVariables.RequestUri); }
            set { Add(RackEnvironmentVariables.RequestUri, value);}
        }

        public string GatewayInterface
        {
            get { return (string) Get(RackEnvironmentVariables.GatewayInterface); }
            set { Add(RackEnvironmentVariables.GatewayInterface, value);}
        }

        public string ContentType
        {
            get { return (string) Get(RackEnvironmentVariables.ContentType); }
            set { Add(RackEnvironmentVariables.ContentType, value); }
        }

        public int ContentLength
        {
            get { return Convert.ToInt32(Get(RackEnvironmentVariables.ContentLength)); }
            set { Add(RackEnvironmentVariables.ContentLength, value.ToString()); }
        }

        public RackRequest Add(string variableKey, object variableValue)
        {
            if (variableValue is string)
                variableValue = MutableString.CreateAscii((string) variableValue);
            _envVariables[MutableString.CreateAscii(variableKey)] = variableValue;
            return this;
        }

        public RackRequest Remove(string variableKey)
        {
            if (_envVariables.ContainsKey(variableKey))
                _envVariables.Remove(variableKey);
            return this;
        }

        public object Get(string variableKey)
        {
            if (_envVariables.ContainsKey(MutableString.CreateAscii(variableKey)))
                return _envVariables[MutableString.CreateAscii(variableKey)];
            return null;
        }

        public bool Contains(string variableKey)
        {
            return _envVariables.ContainsKey(MutableString.CreateAscii(variableKey));
        }

        private void Validate()
        {
            //TODO
        }

        private void Prepare(RubyContext context)
        {
            if (!Contains(RackEnvironmentVariables.HttpVersion) && Contains(RackEnvironmentVariables.ServerProtocol))
                Add(RackEnvironmentVariables.HttpVersion, Get(RackEnvironmentVariables.ServerProtocol));

            Remove(RackEnvironmentVariables.Disallowed.HttpContentLength);
            Remove(RackEnvironmentVariables.Disallowed.HttpContentType);

            if (Get(RackEnvironmentVariables.ContentType) == null ||
                Get(RackEnvironmentVariables.ContentType).ToString() == String.Empty)
                Remove(RackEnvironmentVariables.ContentType);

            if (!Contains(RackEnvironmentVariables.RackErrors))
                Add(RackEnvironmentVariables.RackErrors, context.StandardErrorOutput);

            string body = Body ?? string.Empty;

            Remove(RackEnvironmentVariables.RackInput);
            Add(RackEnvironmentVariables.RackInput,
                new StringIO(MutableString.CreateMutable(body, RubyEncoding.Default), IOMode.ReadOnly));

            
        }

        public Hash ToRubyHash(RubyContext context)
        {
            Prepare(context);
            Validate();

            return new Hash(_envVariables);
        }
    }
}