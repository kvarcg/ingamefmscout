using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace FMScout
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            this.label1.Text = "Immuner's FMScout09 " + FMScout.MainForm.CurrentVersion;
            this.label2.Text = "\r\nReal-Time scouting utility for FM 2009.\r\n\r\nSpecial thanks goes to DrBernhard at the SIGames forums for his excellent work on the FM2009 Ingame/Scout FrameWork.";
            this.label3.Text = "Contact Details";
            this.linkLabel1.Text = "Kostas Vardis";
            this.linkLabel2.Text = "FMScout09 Homepage";
            this.linkLabel3.Text = "Email me";
            this.linkLabel4.Text = "Bug Report";
            this.label4.Text = "CopyRight Kostas Vardis 2008";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.kostasvardis.com");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            homepage();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            email();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bugReport();
        }

        internal void bugReport()
        {
            System.Diagnostics.Process.Start("mailto:kvardis@hotmail.com?subject=Ingame FMScout09 " + FMScout.MainForm.CurrentVersion + " Bug Report");
        }

        internal void email()
        {
            System.Diagnostics.Process.Start("mailto:kvardis@hotmail.com?subject=Ingame FMScout09 " + FMScout.MainForm.CurrentVersion);
        }

        internal void homepage()
        {
            System.Diagnostics.Process.Start("http://www.kostasvardis.com/portfolio.php5?id=4");
        }
    }
}