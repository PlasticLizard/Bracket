using System;

namespace Bracket
{
    public static class RackEnvironmentVariables
    {
        //Please see the Rack Specification for complete details:
        //http://rack.rubyforge.org/doc/SPEC.html

        /*Rack Env. Variables*/

        /// <summary>
        /// REQUEST_METHOD:	The HTTP request method, such as “GET” or 
        //  "POST”. This cannot ever be an empty string, and so is always
        //  required.
        //  The REQUEST_METHOD must be a valid token.
        /// </summary>
        public const string RequestMethod = "REQUEST_METHOD";

        /// <summary>
        ///  SCRIPT_NAME:	The initial portion of the request URL’s “path” 
        //   that corresponds to the application object, so that the 
        //   application knows its virtual “location”. This may be an empty
        //   string, if the application corresponds to the “root” of the 
        //   server.
        //   The SCRIPT_NAME, if non-empty, must start with /
        //   SCRIPT_NAME never should be /, but instead be empty.
        /// </summary>
        public const string ScriptName = "SCRIPT_NAME";

        /// <summary>
        ///  PATH_INFO:	The remainder of the request URL’s “path”, 
        //   designating the virtual “location” of the request’s target 
        //   within the application. This may be an empty string, if the 
        //   request URL targets the application root and does not have a 
        //   trailing slash. This value may be percent-encoded when I 
        //   originating from a URL.
        // The PATH_INFO, if non-empty, must start with /
        // PATH_INFO should be / if SCRIPT_NAME is empty. 
        /// </summary>
        public const string PathInfo = "PATH_INFO";

        /// <summary>
        /// QUERY_STRING:	The portion of the request URL that follows the
        ///   ?, if any. May be empty, but is always required!
        /// </summary>
        public const string QueryString = "QUERY_STRING";

        /// <summary>
        ///  SERVER_NAME, SERVER_PORT:	When combined with SCRIPT_NAME and
        //   PATH_INFO, these variables can be used to complete the URL. 
        //   Note, however, that HTTP_HOST, if present, should be used in
        //   preference to SERVER_NAME for reconstructing the request URL. 
        //   SERVER_NAME and SERVER_PORT can never be empty strings, and so
        //   are always required.
        /// </summary>
        public const string ServerName = "SERVER_NAME";

        /// <summary>
        ///  SERVER_NAME, SERVER_PORT:	When combined with SCRIPT_NAME and
        ///   PATH_INFO, these variables can be used to complete the URL. 
        ///   Note, however, that HTTP_HOST, if present, should be used in
        ///   preference to SERVER_NAME for reconstructing the request URL. 
        ///   SERVER_NAME and SERVER_PORT can never be empty strings, and so
        ///   are always required.
        /// </summary>
        public const string ServerPort = "SERVER_PORT";

        public const string RemoteHost = "REMOTE_HOST";
        public const string ServerProtocol = "SERVER_PROTOCOL";
        public const string RequestPath = "REQUEST_PATH";
        public const string ServerSoftware = "SERVER_SOFTWARE";
        public const string RemoteAddress = "REMOTE_ADDR";
        public const string HttpVersion = "HTTP_VERSION";
        public const string RequestUri = "REQUEST_URI";
        public const string GatewayInterface = "GATEWAY_INTERFACE";
        public const string ContentType = "CONTENT_TYPE";
        public const string ContentLength = "CONTENT_LENGTH";

        /// <summary>
        ///  HTTP_ Variables:	Variables corresponding to the client-supplied 
        //   HTTP request headers (i.e., variables whose names begin with 
        //   HTTP_). The presence or absence of these variables should 
        //   correspond with the presence or absence of the appropriate 
        //   HTTP header in the request.
        // The CONTENT_LENGTH, if given, must consist of digits only.
        /// </summary>
        public const string GenericHeaderPrefix = "HTTP_";

        public static class Disallowed
        {
            /// <summary>
            ///  The environment 
            /// must not contain the keys HTTP_CONTENT_TYPE or HTTP_CONTENT_LENGTH 
            /// (use the versions without HTTP_). The CGI keys (named without a period)
            /// must have String values.
            /// </summary>
            public const string HttpContentLength = "HTTP_CONTENT_LENGTH";
            /// <summary>
            ///  The environment 
            /// must not contain the keys HTTP_CONTENT_TYPE or HTTP_CONTENT_LENGTH 
            /// (use the versions without HTTP_). The CGI keys (named without a period)
            /// must have String values.
            /// </summary>
            public const string HttpContentType = "HTTP_CONTENT_TYPE";
        }

        /*Rack Specific Variables*/

        
        /// <summary>
        /// rack.version:	The Array [1,0], representing this version of 
        /// Rack.
        /// </summary>
        public const string RackVersion = "rack.version";

        /// <summary>
        ///rack.url_scheme:	http or https, depending on the request URL.
        /// </summary>
        public const string RackUrlScheme = "rack.url_scheme";

        /// <summary>
        ///rack.input: The input stream is an IO-like object which contains
        /// the raw HTTP POST data. If it is a file then it must be opened 
        /// in binary mode. The input stream must respond to gets, each, 
        /// read and rewind.
        ///
        /// gets must be called without arguments and return a string, or 
        ///   nil on EOF.
        /// read behaves like IO#read. Its signature is 
        ///   read([length, [buffer]]). If given, length must be an 
        ///   non-negative Integer (>= 0) or nil, and buffer must be a String
        ///   and may not be nil. If length is given and not nil, then this 
        ///   method reads at most length bytes from the input stream. If 
        ///   length is not given or nil, then this method reads all data 
        ///   until EOF. When EOF is reached, this method returns nil if 
        ///   length is given and not nil, or “” if length is not given or 
        ///   is nil. If buffer is given, then the read data will be placed 
        ///   into buffer instead of a newly created String object.
        /// each must be called without arguments and only yield Strings.
        /// rewind must be called without arguments. It rewinds the input
        ///   stream back to the beginning. It must not raise Errno::ESPIPE:
        ///   that is, it may not be a pipe or a socket. Therefore, handler
        ///   developers must buffer the input data into some rewindable
        ///   object if the underlying input stream is not rewindable.
        /// close must never be called on the input stream.
        /// </summary>
        public const string RackInput = "rack.input";

        /// <summary>
        /// rack.errors:The error stream must respond to puts, write and flush.
        ///
        /// puts must be called with a single argument that responds to to_s.
        /// write must be called with a single argument that is a String.
        /// flush must be called without arguments and must be called in 
        /// order to make the error appear for sure.
        /// close must never be called on the error stream.
        /// </summary>
        public const string RackErrors = "rack.errors";

        /// <summary>
        ///  rack.multithread:	true if the application object may be simultaneously
        //   invoked by another thread in the same process, false otherwise.
        /// </summary>
        public const string RackMultithreaded = "rack.multithreaded";

        /// <summary>
        /// rack.multiprocess:	true if an equivalent application object may be
        /// simultaneously invoked by another process, false otherwise.
        /// </summary>
        public const string RackMultiprocess = "rack.multiprocess";

        /// <summary>
        ///   rack.run_once: true if the server expects (but does not guarantee!)
        ///   that the application will only be invoked this one time during the 
        ///   life of its containing process. Normally, this will only be true for
        ///   a server based on CGI (or something similar).
        /// </summary>
        public const string RackRunOnce = "rack.run_once";
        
        /// <summary>
        ///  rack.session:	A hash like interface for storing request session data. 
        ///  The store must implement: 
        ///   store(key, value) (aliased as []=); 
        ///   fetch(key, default = nil) (aliased as []); 
        ///   delete(key); 
        ///   clear;
        /// </summary>
        public const string RackSession = "rack.session";

       

        /// <summary>
        /// The server or the application can store their own data in the
        /// environment, too. The keys must contain at least one dot, and 
        /// should be prefixed uniquely. The prefix rack. is reserved for 
        /// use with the Rack core distribution and other accepted 
        /// specifications and must not be used otherwise. 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CustomVariableName(string prefix,string name)
        {
            if (prefix == "rack")
                throw new ArgumentException(
                    "The 'rack' prefix is reserved for use with the Rack core distribution and other accepted specifications.");
            return String.Format("{0}.{1}", prefix, name);
        }
    
    }
}