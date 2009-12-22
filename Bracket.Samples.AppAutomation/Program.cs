﻿using System;
using System.Windows.Forms;
using Bracket.Samples.AppAutomation;

namespace Bracket.Hosting.Samples.Embedded
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
