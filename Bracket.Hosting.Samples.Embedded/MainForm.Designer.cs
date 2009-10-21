namespace Bracket.Hosting.Samples.Embedded
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpServerLib = new System.Windows.Forms.GroupBox();
            this.radAzure = new System.Windows.Forms.RadioButton();
            this.radKayak = new System.Windows.Forms.RadioButton();
            this.radFramework = new System.Windows.Forms.RadioButton();
            this.radBracket = new System.Windows.Forms.RadioButton();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnBrowserNavigate = new System.Windows.Forms.Button();
            this.btnOpenInBrowser = new System.Windows.Forms.Button();
            this.grpOutput = new System.Windows.Forms.GroupBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.grpApplicationType = new System.Windows.Forms.GroupBox();
            this.radRails = new System.Windows.Forms.RadioButton();
            this.radSinatraApp = new System.Windows.Forms.RadioButton();
            this.radRackApp = new System.Windows.Forms.RadioButton();
            this.grpAzureSettings = new System.Windows.Forms.GroupBox();
            this.txtSlnName = new System.Windows.Forms.TextBox();
            this.txtSlnPassword = new System.Windows.Forms.TextBox();
            this.txtUrlNamespace = new System.Windows.Forms.TextBox();
            this.chkUseSsl = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grpServerLib.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpOutput.SuspendLayout();
            this.grpApplicationType.SuspendLayout();
            this.grpAzureSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpServerLib
            // 
            this.grpServerLib.Controls.Add(this.radAzure);
            this.grpServerLib.Controls.Add(this.radKayak);
            this.grpServerLib.Controls.Add(this.radFramework);
            this.grpServerLib.Controls.Add(this.radBracket);
            this.grpServerLib.Location = new System.Drawing.Point(12, 12);
            this.grpServerLib.Name = "grpServerLib";
            this.grpServerLib.Size = new System.Drawing.Size(218, 120);
            this.grpServerLib.TabIndex = 0;
            this.grpServerLib.TabStop = false;
            this.grpServerLib.Text = "Hosting Library";
            // 
            // radAzure
            // 
            this.radAzure.AutoSize = true;
            this.radAzure.Location = new System.Drawing.Point(6, 88);
            this.radAzure.Name = "radAzure";
            this.radAzure.Size = new System.Drawing.Size(112, 17);
            this.radAzure.TabIndex = 0;
            this.radAzure.Text = "Azure Service Bus";
            this.radAzure.UseVisualStyleBackColor = true;
            this.radAzure.CheckedChanged += new System.EventHandler(this.radAzure_CheckedChanged);
            // 
            // radKayak
            // 
            this.radKayak.AutoSize = true;
            this.radKayak.Location = new System.Drawing.Point(6, 65);
            this.radKayak.Name = "radKayak";
            this.radKayak.Size = new System.Drawing.Size(55, 17);
            this.radKayak.TabIndex = 0;
            this.radKayak.Text = "Kayak";
            this.radKayak.UseVisualStyleBackColor = true;
            // 
            // radFramework
            // 
            this.radFramework.AutoSize = true;
            this.radFramework.Location = new System.Drawing.Point(6, 42);
            this.radFramework.Name = "radFramework";
            this.radFramework.Size = new System.Drawing.Size(164, 17);
            this.radFramework.TabIndex = 0;
            this.radFramework.Text = "Bracket.Hosting.System.Web";
            this.radFramework.UseVisualStyleBackColor = true;
            // 
            // radBracket
            // 
            this.radBracket.AutoSize = true;
            this.radBracket.Checked = true;
            this.radBracket.Location = new System.Drawing.Point(6, 19);
            this.radBracket.Name = "radBracket";
            this.radBracket.Size = new System.Drawing.Size(135, 17);
            this.radBracket.TabIndex = 0;
            this.radBracket.TabStop = true;
            this.radBracket.Text = "Bracket.Hosting.Server";
            this.radBracket.UseVisualStyleBackColor = true;
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(12, 235);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(218, 23);
            this.btnStartServer.TabIndex = 1;
            this.btnStartServer.Text = "Rackup!";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.webBrowser);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Location = new System.Drawing.Point(236, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(708, 366);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Web Browser";
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(3, 36);
            this.webBrowser.Margin = new System.Windows.Forms.Padding(5);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(702, 327);
            this.webBrowser.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtUrl);
            this.panel1.Controls.Add(this.btnBrowserNavigate);
            this.panel1.Controls.Add(this.btnOpenInBrowser);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(702, 20);
            this.panel1.TabIndex = 1;
            // 
            // txtUrl
            // 
            this.txtUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUrl.Location = new System.Drawing.Point(0, 0);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(531, 20);
            this.txtUrl.TabIndex = 0;
            this.txtUrl.Text = "http://localhost:9876";
            // 
            // btnBrowserNavigate
            // 
            this.btnBrowserNavigate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBrowserNavigate.Location = new System.Drawing.Point(531, 0);
            this.btnBrowserNavigate.Name = "btnBrowserNavigate";
            this.btnBrowserNavigate.Size = new System.Drawing.Size(75, 20);
            this.btnBrowserNavigate.TabIndex = 1;
            this.btnBrowserNavigate.Text = "Go";
            this.btnBrowserNavigate.UseVisualStyleBackColor = true;
            this.btnBrowserNavigate.Click += new System.EventHandler(this.btnBrowserNavigate_Click);
            // 
            // btnOpenInBrowser
            // 
            this.btnOpenInBrowser.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOpenInBrowser.Location = new System.Drawing.Point(606, 0);
            this.btnOpenInBrowser.Name = "btnOpenInBrowser";
            this.btnOpenInBrowser.Size = new System.Drawing.Size(96, 20);
            this.btnOpenInBrowser.TabIndex = 2;
            this.btnOpenInBrowser.Text = "Launch Browser";
            this.btnOpenInBrowser.UseVisualStyleBackColor = true;
            this.btnOpenInBrowser.Click += new System.EventHandler(this.btnOpenInBrowser_Click);
            // 
            // grpOutput
            // 
            this.grpOutput.Controls.Add(this.txtOutput);
            this.grpOutput.Location = new System.Drawing.Point(236, 382);
            this.grpOutput.Name = "grpOutput";
            this.grpOutput.Size = new System.Drawing.Size(708, 158);
            this.grpOutput.TabIndex = 3;
            this.grpOutput.TabStop = false;
            this.grpOutput.Text = "Output";
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Location = new System.Drawing.Point(3, 16);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(702, 139);
            this.txtOutput.TabIndex = 0;
            // 
            // btnStopServer
            // 
            this.btnStopServer.Enabled = false;
            this.btnStopServer.Location = new System.Drawing.Point(12, 264);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(218, 23);
            this.btnStopServer.TabIndex = 1;
            this.btnStopServer.Text = "Stop Server";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // grpApplicationType
            // 
            this.grpApplicationType.Controls.Add(this.radRails);
            this.grpApplicationType.Controls.Add(this.radSinatraApp);
            this.grpApplicationType.Controls.Add(this.radRackApp);
            this.grpApplicationType.Location = new System.Drawing.Point(12, 138);
            this.grpApplicationType.Name = "grpApplicationType";
            this.grpApplicationType.Size = new System.Drawing.Size(218, 91);
            this.grpApplicationType.TabIndex = 4;
            this.grpApplicationType.TabStop = false;
            this.grpApplicationType.Text = "Rack Flavor";
            // 
            // radRails
            // 
            this.radRails.AutoSize = true;
            this.radRails.Location = new System.Drawing.Point(6, 65);
            this.radRails.Name = "radRails";
            this.radRails.Size = new System.Drawing.Size(48, 17);
            this.radRails.TabIndex = 0;
            this.radRails.Text = "Rails";
            this.radRails.UseVisualStyleBackColor = true;
            // 
            // radSinatraApp
            // 
            this.radSinatraApp.AutoSize = true;
            this.radSinatraApp.Location = new System.Drawing.Point(6, 42);
            this.radSinatraApp.Name = "radSinatraApp";
            this.radSinatraApp.Size = new System.Drawing.Size(113, 17);
            this.radSinatraApp.TabIndex = 0;
            this.radSinatraApp.Text = "Sinatra Application";
            this.radSinatraApp.UseVisualStyleBackColor = true;
            // 
            // radRackApp
            // 
            this.radRackApp.AutoSize = true;
            this.radRackApp.Checked = true;
            this.radRackApp.Location = new System.Drawing.Point(6, 19);
            this.radRackApp.Name = "radRackApp";
            this.radRackApp.Size = new System.Drawing.Size(146, 17);
            this.radRackApp.TabIndex = 0;
            this.radRackApp.TabStop = true;
            this.radRackApp.Text = "Generic Rack Application";
            this.radRackApp.UseVisualStyleBackColor = true;
            // 
            // grpAzureSettings
            // 
            this.grpAzureSettings.Controls.Add(this.txtUrlNamespace);
            this.grpAzureSettings.Controls.Add(this.label3);
            this.grpAzureSettings.Controls.Add(this.label2);
            this.grpAzureSettings.Controls.Add(this.label1);
            this.grpAzureSettings.Controls.Add(this.chkUseSsl);
            this.grpAzureSettings.Controls.Add(this.txtSlnPassword);
            this.grpAzureSettings.Controls.Add(this.txtSlnName);
            this.grpAzureSettings.Enabled = false;
            this.grpAzureSettings.Location = new System.Drawing.Point(12, 382);
            this.grpAzureSettings.Name = "grpAzureSettings";
            this.grpAzureSettings.Size = new System.Drawing.Size(218, 155);
            this.grpAzureSettings.TabIndex = 5;
            this.grpAzureSettings.TabStop = false;
            this.grpAzureSettings.Text = "Azure Service Bus Settings";
            // 
            // txtSlnName
            // 
            this.txtSlnName.Location = new System.Drawing.Point(84, 19);
            this.txtSlnName.Name = "txtSlnName";
            this.txtSlnName.Size = new System.Drawing.Size(128, 20);
            this.txtSlnName.TabIndex = 0;
            this.txtSlnName.Text = "MyServiceBus";
            // 
            // txtSlnPassword
            // 
            this.txtSlnPassword.Location = new System.Drawing.Point(84, 45);
            this.txtSlnPassword.Name = "txtSlnPassword";
            this.txtSlnPassword.Size = new System.Drawing.Size(128, 20);
            this.txtSlnPassword.TabIndex = 0;
            this.txtSlnPassword.Text = "MyPassword";
            // 
            // txtUrlNamespace
            // 
            this.txtUrlNamespace.Location = new System.Drawing.Point(84, 71);
            this.txtUrlNamespace.Name = "txtUrlNamespace";
            this.txtUrlNamespace.Size = new System.Drawing.Size(128, 20);
            this.txtUrlNamespace.TabIndex = 0;
            this.txtUrlNamespace.Text = "Bracket";
            // 
            // chkUseSsl
            // 
            this.chkUseSsl.AutoSize = true;
            this.chkUseSsl.Location = new System.Drawing.Point(84, 97);
            this.chkUseSsl.Name = "chkUseSsl";
            this.chkUseSsl.Size = new System.Drawing.Size(68, 17);
            this.chkUseSsl.TabIndex = 1;
            this.chkUseSsl.Text = "Use SSL";
            this.chkUseSsl.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Solution Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Solution Pwd:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Service Path:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 552);
            this.Controls.Add(this.grpAzureSettings);
            this.Controls.Add(this.grpApplicationType);
            this.Controls.Add(this.grpOutput);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnStopServer);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.grpServerLib);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.grpServerLib.ResumeLayout(false);
            this.grpServerLib.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpOutput.ResumeLayout(false);
            this.grpOutput.PerformLayout();
            this.grpApplicationType.ResumeLayout(false);
            this.grpApplicationType.PerformLayout();
            this.grpAzureSettings.ResumeLayout(false);
            this.grpAzureSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpServerLib;
        private System.Windows.Forms.RadioButton radKayak;
        private System.Windows.Forms.RadioButton radFramework;
        private System.Windows.Forms.RadioButton radBracket;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.GroupBox grpOutput;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnBrowserNavigate;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnStopServer;
        private System.Windows.Forms.GroupBox grpApplicationType;
        private System.Windows.Forms.RadioButton radSinatraApp;
        private System.Windows.Forms.RadioButton radRackApp;
        private System.Windows.Forms.RadioButton radRails;
        private System.Windows.Forms.Button btnOpenInBrowser;
        private System.Windows.Forms.RadioButton radAzure;
        private System.Windows.Forms.GroupBox grpAzureSettings;
        private System.Windows.Forms.CheckBox chkUseSsl;
        private System.Windows.Forms.TextBox txtUrlNamespace;
        private System.Windows.Forms.TextBox txtSlnPassword;
        private System.Windows.Forms.TextBox txtSlnName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}

