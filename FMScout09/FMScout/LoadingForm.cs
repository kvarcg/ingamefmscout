using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FMScout
{
    public partial class LoadingForm : Form
    {
        private MainForm mainForm = null;

        public LoadingForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.progressBar1.Visible = false;
        }

        public void startLoading()
        {
            this.Show(this.mainForm);
            this.loadingLabel.Text = "\r\nLoading FM 2009...Please wait";
            Application.DoEvents();
        }

        public void setLoading(string str)
        {
            this.loadingLabel.Text = str;
        }

        public void finishLoading(bool res)
        {
            this.mainForm.scoutLoaded = res;
            this.mainForm.Enabled = true;
            this.Hide();
        }

        public void startWorker()
        {
            this.progressBar1.Visible = true;
            this.loadingLabel.Text = "FM " + this.mainForm.fm.GetCurrentFMVersion() + " Loaded\r\n\r\nCalculating Positional Ratings";
            backgroundWorker1.RunWorkerAsync();
        }

        internal void reportProgress(float progress)
        {
            this.backgroundWorker1.ReportProgress((int)progress);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.mainForm.calculatePR();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar1.Visible = false;
            finishLoading(true);
        }
    }
}
