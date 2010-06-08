using System;
using System.ComponentModel;
using System.Windows.Forms;
using Bracket.Samples.AppAutomation.Properties;
using Microsoft.Scripting.Hosting;

namespace Bracket.Samples.AppAutomation
{
    public partial class MainForm : Form
    {

        private RubyEnvironment _scriptHost;
        private string _curScriptName;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _scriptHost = new RubyEnvironment();
            cboEvents.SelectedIndex = 0;
        }

        private void btnDoSomething_Click(object sender, EventArgs e)
        {
            HandleEvent("btnDoSomething_Click",sender,e);
        }

        private void btnDoSomethingElse_Click(object sender, EventArgs e)
        {
            HandleEvent("btnDoSomethingElse_Click",sender,e);
        }

        private void txtTypeHere_Validated(object sender, EventArgs e)
        {
            HandleEvent("txtTypeHere_Validated",sender,e);
        }

        private void txtTypeHere_Validating(object sender, CancelEventArgs e)
        {
            HandleEvent("txtTypeHere_Validating", sender, e);
        }

        private void HandleEvent(string eventName, object sender, EventArgs args)
        {
            var script = Settings.Default[eventName + "_Handler"] as string;
            if (string.IsNullOrEmpty(script))
                return;

            var ctx = new ScriptContext {EventName = eventName, Sender = sender, Args = args, Host = this};
            ScriptScope scope = _scriptHost.Engine.CreateScope();
            scope.SetVariable("context", ctx);
            object output;
            try
            {
                output = _scriptHost.Engine.Execute(script, scope);
            }
            catch (Exception ex)
            {
                output = ex;
            }
            
            txtScriptOutput.AppendText("=>" 
                + (output ?? "<null>")
                + Environment.NewLine);
            
        }

        private void cboEvents_SelectedValueChanged(object sender, EventArgs e)
        {
            txtScript.Clear();
            if (!String.IsNullOrEmpty(cboEvents.Text))
            {
                _curScriptName = cboEvents.Text.Trim();

                var script = Settings.Default[_curScriptName + "_Handler"] as string;

                txtScript.Text = script;
            }
        }

        private void txtScript_Validated(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_curScriptName) == false)
            {
                Settings.Default[_curScriptName + "_Handler"] = txtScript.Text;
                Settings.Default.Save();
            }
        }

    }
}
