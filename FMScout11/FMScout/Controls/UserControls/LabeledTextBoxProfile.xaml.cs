using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FMScout.CustomControls;

namespace FMScout.UserControls
{
	public partial class LabeledTextBoxProfile : UserControl
	{
		public LabeledTextBoxProfile()
		{
			this.InitializeComponent();
		}
		
   	 	public Label Label
        {
            get { return this.MyLabel; }
            set { this.MyLabel = value; }
        }

        public TextBox TextBox
        {
            get { return this.MyTextBox; }
            set { this.MyTextBox = value; }
        }		
	}
}