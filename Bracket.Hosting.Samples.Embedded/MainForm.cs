using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using Bracket.Events;
using Bracket.Hosting.Azure.ServiceBus;
using Bracket.Hosting.HttpServer;
using Bracket.Hosting.Kayak;
using Bracket.Hosting.Samples.Embedded.Properties;
using Bracket.Hosting.SystemWeb;
using HttpServer;
using Microsoft.ServiceBus;

namespace Bracket.Hosting.Samples.Embedded
{
    public partial class MainForm : Form
    {
        private DefaultRackServer _bracketServer;
        private HttpListenerRackServer _frameworkServer;
        private KayakRackServer _kayakServer;
        private RackServiceHost _azureServer;

        private readonly TextBoxLogWriter _logWriter;

        public MainForm()
        {
            InitializeComponent();
            _logWriter = new TextBoxLogWriter(txtOutput);

            txtSlnName.Text = Settings.Default.AzureServiceBusSolutionName;
            txtSlnPassword.Text = Settings.Default.AzureServiceBusSolutionPassword;
            txtUrlNamespace.Text = Settings.Default.AzureServiceBusUrlNamespace;
            chkUseSsl.Checked = Settings.Default.AzureServiceBusUseSSL;

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
           else if (radAzure.Checked)
           {
               _azureServer = new RackServiceHost(txtSlnName.Text, txtSlnPassword.Text, txtUrlNamespace.Text,
                                                  chkUseSsl.Checked);
               _azureServer.Start(new RubyEnvironment((env) => env.ApplicationRootPath = appName));

               txtUrl.Text = ServiceBusEnvironment.CreateServiceUri(chkUseSsl.Checked ? "https" : "http",
                                                                    txtSlnName.Text, txtUrlNamespace.Text).ToString();
           }

            _logWriter.Write(this, LogPrio.Info, SelectedFrameworkName + " Started!");

            btnBrowserNavigate.PerformClick();

            grpServerLib.Enabled = false;
            grpApplicationType.Enabled = false;
            btnStartServer.Enabled = false;
            btnStopServer.Enabled = true;
            grpAzureSettings.Enabled = false;

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
            else if (radAzure.Checked)
            {
                if(_azureServer != null)
                    _azureServer.Dispose();
                _azureServer = null;
                grpAzureSettings.Enabled = true;
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
            if (_azureServer != null)
                _azureServer.Dispose();

            Settings.Default.AzureServiceBusSolutionName = txtSlnName.Text;
            Settings.Default.AzureServiceBusSolutionPassword = txtSlnPassword.Text;
            Settings.Default.AzureServiceBusUrlNamespace = txtUrlNamespace.Text;
            Settings.Default.AzureServiceBusUseSSL = chkUseSsl.Checked;
            Settings.Default.Save();

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

        private void radAzure_CheckedChanged(object sender, EventArgs e)
        {
            grpAzureSettings.Enabled = radAzure.Checked;
        }
    }
}
