namespace Bracket.Samples.AppAutomation
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
            this.btnDoSomething = new System.Windows.Forms.Button();
            this.btnDoSomethingElse = new System.Windows.Forms.Button();
            this.txtScript = new System.Windows.Forms.TextBox();
            this.cboEvents = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTypeHere = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtScriptOutput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDoSomething
            // 
            this.btnDoSomething.Location = new System.Drawing.Point(12, 12);
            this.btnDoSomething.Name = "btnDoSomething";
            this.btnDoSomething.Size = new System.Drawing.Size(132, 23);
            this.btnDoSomething.TabIndex = 0;
            this.btnDoSomething.Text = "Do Something";
            this.btnDoSomething.UseVisualStyleBackColor = true;
            this.btnDoSomething.Click += new System.EventHandler(this.btnDoSomething_Click);
            // 
            // btnDoSomethingElse
            // 
            this.btnDoSomethingElse.Location = new System.Drawing.Point(12, 41);
            this.btnDoSomethingElse.Name = "btnDoSomethingElse";
            this.btnDoSomethingElse.Size = new System.Drawing.Size(132, 23);
            this.btnDoSomethingElse.TabIndex = 0;
            this.btnDoSomethingElse.Text = "Do Something Else";
            this.btnDoSomethingElse.UseVisualStyleBackColor = true;
            this.btnDoSomethingElse.Click += new System.EventHandler(this.btnDoSomethingElse_Click);
            // 
            // txtScript
            // 
            this.txtScript.Location = new System.Drawing.Point(266, 65);
            this.txtScript.Multiline = true;
            this.txtScript.Name = "txtScript";
            this.txtScript.Size = new System.Drawing.Size(365, 363);
            this.txtScript.TabIndex = 1;
            this.txtScript.Validated += new System.EventHandler(this.txtScript_Validated);
            // 
            // cboEvents
            // 
            this.cboEvents.FormattingEnabled = true;
            this.cboEvents.Items.AddRange(new object[] {
            "btnDoSomething_Click",
            "btnDoSomethingElse_Click",
            "txtTypeHere_Validating",
            "txtTypeHere_Validated"});
            this.cboEvents.Location = new System.Drawing.Point(304, 9);
            this.cboEvents.Name = "cboEvents";
            this.cboEvents.Size = new System.Drawing.Size(327, 21);
            this.cboEvents.TabIndex = 2;
            this.cboEvents.SelectedValueChanged += new System.EventHandler(this.cboEvents_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(263, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Event";
            // 
            // txtTypeHere
            // 
            this.txtTypeHere.Location = new System.Drawing.Point(12, 101);
            this.txtTypeHere.Name = "txtTypeHere";
            this.txtTypeHere.Size = new System.Drawing.Size(248, 20);
            this.txtTypeHere.TabIndex = 4;
            this.txtTypeHere.Validated += new System.EventHandler(this.txtTypeHere_Validated);
            this.txtTypeHere.Validating += new System.ComponentModel.CancelEventHandler(this.txtTypeHere_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Type Here";
            // 
            // txtScriptOutput
            // 
            this.txtScriptOutput.Location = new System.Drawing.Point(12, 154);
            this.txtScriptOutput.Multiline = true;
            this.txtScriptOutput.Name = "txtScriptOutput";
            this.txtScriptOutput.Size = new System.Drawing.Size(248, 274);
            this.txtScriptOutput.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Script Output";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(265, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ruby Script";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 440);
            this.Controls.Add(this.txtScriptOutput);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTypeHere);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboEvents);
            this.Controls.Add(this.txtScript);
            this.Controls.Add(this.btnDoSomethingElse);
            this.Controls.Add(this.btnDoSomething);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDoSomething;
        private System.Windows.Forms.Button btnDoSomethingElse;
        private System.Windows.Forms.TextBox txtScript;
        private System.Windows.Forms.ComboBox cboEvents;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTypeHere;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtScriptOutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}