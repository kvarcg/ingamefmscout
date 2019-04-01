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
    public partial class DonationForm : Form
    {
        public DonationForm()
        {
            InitializeComponent();
            this.label1.Text = "Ingame FMScout09 Donation";
            this.label2.Text = "This tool is and always will be provided to you as a free utility to make your game more enjoyable.\r\nPlus, the framework (that loads the game) which this tool is based on was not created by me, so I would find it unethical to take credit for someone else's work.\r\n\r\nHowever, loads of effort has been put into it so, if you do like this and would like to encourage this project you can do it with a small donation or with a simple thanks :)\r\n\r\nMay the FM force be with you, Kostas.";
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=1641572");
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
