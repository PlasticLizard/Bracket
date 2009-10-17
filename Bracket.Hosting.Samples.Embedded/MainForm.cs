using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using Bracket.Events;
using Bracket.Hosting.HttpServer;
using Bracket.Hosting.Kayak;
using Bracket.Hosting.SystemWeb;
using HttpServer;

namespace Bracket.Hosting.Samples.Embedded
{
    public partial class MainForm : Form
    {
        private DefaultRackServer _bracketServer;
        private HttpListenerRackServer _frameworkServer;
        private KayakRackServer _kayakServer;

        private readonly TextBoxLogWriter _logWriter;

        public MainForm()
        {
            InitializeComponent();
            _logWriter = new TextBoxLogWriter(txtOutput);
            //BracketEvent.LogAllEvents = true;
            //BracketEvent.Event += BracketEvent_Event;
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            _logWriter.Write(this, LogPrio.Info, "Starting " + SelectedFrameworkName + " on port 9876...");
            string appName = radRackApp.Checked ? "RackApp" : radSinatraApp.Checked ?  "Sinatra" : "Rails";

           if(radBracket.Checked)
           {
               _bracketServer = new DefaultRackServer(9876, IPAddress.Any, _logWriter);
               _bracketServer.Start(new RubyEnvironment((env) => env.ApplicationRootPath = appName));
           }
           else if (radFramework.Checked)
           {
               _frameworkServer = new HttpListenerRackServer(9876);
               _frameworkServer.Start(new RubyEnvironment((env) => env.ApplicationRootPath = appName));
           }
           else if (radKayak.Checked)
           {
               _kayakServer = new KayakRackServer(9876);
               _kayakServer.Start(new RubyEnvironment((env) => env.ApplicationRootPath = appName));
           }

            _logWriter.Write(this, LogPrio.Info, SelectedFrameworkName + " Started!");

            btnBrowserNavigate.PerformClick();

            grpServerLib.Enabled = false;
            grpApplicationType.Enabled = false;
            btnStartServer.Enabled = false;
            btnStopServer.Enabled = true;

        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            webBrowser.Navigate("");

            _logWriter.Write(this,LogPrio.Info,"Stopping " + SelectedFrameworkName + "...");
            
            if (radBracket.Checked)
            {
                if (_bracketServer != null)
                    _bracketServer.Dispose();
                _bracketServer = null;
            }
            else if (radFramework.Checked)
            {
                if (_frameworkServer != null)
                    _frameworkServer.Dispose();
                _frameworkServer = null;
            }
            else if (radKayak.Checked)
            {
                if (_kayakServer != null)
                    _kayakServer.Dispose();
                _kayakServer = null;
            }

            _logWriter.Write(this, LogPrio.Info,SelectedFrameworkName + " Stopped.");

            grpServerLib.Enabled = true;
            grpApplicationType.Enabled = true;
            btnStartServer.Enabled = true;
            btnStopServer.Enabled = false;

        }

        private string SelectedFrameworkName
        {
            get
            {
                string frameworkName = radBracket.Checked
                                      ? "Bracket.Hosting.Server"
                                      : radFramework.Checked ? "Bracket.Hosing.System.Web" : "Kayak";
                return frameworkName;
            }
        }

        private void btnBrowserNavigate_Click(object sender, EventArgs e)
        {
            if (webBrowser.Url == new Uri(txtUrl.Text))
                webBrowser.Refresh(WebBrowserRefreshOption.Completely);
            else
                webBrowser.Navigate(this.txtUrl.Text);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (_bracketServer != null)
                _bracketServer.Dispose();
            if (_frameworkServer != null)
                _frameworkServer.Dispose();
            if(_kayakServer != null)
                _kayakServer.Dispose();

        }

        private void btnOpenInBrowser_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUrl.Text))
            Process.Start(txtUrl.Text);
        }

        void BracketEvent_Event(object sender, BracketEvent e)
        {
            _logWriter.Write(this, LogPrio.Info, e.ToString());
        }
    }
}
