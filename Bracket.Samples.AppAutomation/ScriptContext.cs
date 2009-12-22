using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bracket.Samples.AppAutomation
{
    public class ScriptContext
    {
        public string EventName { get; set; }
        public object Sender { get; set; }
        public EventArgs Args { get; set; }
        public Form Host { get; set; }

        public void Alert(string message)
        {
            MessageBox.Show(message);
        }
    }
}
