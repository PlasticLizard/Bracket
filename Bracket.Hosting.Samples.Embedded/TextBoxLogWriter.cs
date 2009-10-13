using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HttpServer;

namespace Bracket.Hosting.Samples.Embedded
{
    public class TextBoxLogWriter : ILogWriter
    {
        private readonly TextBox _textBox;

        public TextBoxLogWriter(TextBox textBox)
        {
            _textBox = textBox;
        }

        public void Write(object source, LogPrio priority, string message)
        {
            if (!_textBox.InvokeRequired && !_textBox.IsDisposed)
            {
                _textBox.AppendText(String.Format("{0}:{1} on {2}" + Environment.NewLine, DateTime.Now, message, source ?? "<null>"));
            }
            else
            {
                Action<object, LogPrio, string> marshal = Write;
                _textBox.BeginInvoke(marshal, source, priority, message);
            }
        }
    }
}
