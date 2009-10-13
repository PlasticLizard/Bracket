using System;
using System.Collections.Generic;
using System.IO;

namespace Bracket
{
    public class RackResponse
    {
        public RackResponse():this(new MemoryStream())
        {}

        public RackResponse(Stream outputStream)
        {
            Headers = new Dictionary<string, string>();
            Output = new StreamWriter(outputStream);
        }
        
        public int Status { get; set; }
        public Stream OutputStream { get { return Output.BaseStream; } }

        public void AddHeader(string key,string value)
        {
            Headers.Add(key, value);
        }

        public IDictionary<string, string> Headers { get; private set; }
        public void Write(string line)
        {
            Output.WriteLine(line);
        }

        private StreamWriter Output { get; set; }
        
        public void Flush()
        {
            Output.Flush();
        }
    }
}