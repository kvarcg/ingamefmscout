using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using FMScout.ViewModel;
using FMScout.ControlContext;

namespace FMScout.View
{
	public partial class WindowDonate : Window
	{
        private GlobalFuncs globalFuncs = null;
        private DonateWindowViewModel vm = null;
        
        public WindowDonate()
		{
			this.InitializeComponent();

            globalFuncs = Globals.getGlobalFuncs();

            setDataContext();

            this.ButtonClose.Click += new System.Windows.RoutedEventHandler(ButtonClose_Click);
			this.MouseDown += new System.Windows.Input.MouseButtonEventHandler(WindowDonate_MouseDown);
			this.ButtonDonate.Click += new System.Windows.RoutedEventHandler(ButtonDonate_Click);
			this.ButtonNoDonation.Click += new System.Windows.RoutedEventHandler(ButtonNoDonation_Click);
        }

        private void setDataContext()
        {
            vm = new DonateWindowViewModel();

            ImageButtonContext close = new ImageButtonContext();
            close.ImageSource = TryFindResource("close") as ImageSource;
            ImageTextButtonContext ok = new ImageTextButtonContext();
            ok.ImageSource = TryFindResource("yes") as ImageSource;
            ImageTextButtonContext cancel = new ImageTextButtonContext();
            cancel.ImageSource = TryFindResource("cancel") as ImageSource;

            vm.close = close;
            vm.ok = ok;
            vm.cancel = cancel;

            vm.header = new LabeledHeaderContext();

            setControlValues();
            setLocalization();

            this.DataContext = vm;
        }

        public void setControlValues()
        {

        }

        public void setLocalization()
        {
            ObservableCollection<string> WindowDonateLabels = globalFuncs.localization.WindowDonateLabels;
            int index = -1;
            vm.header.Header = WindowDonateLabels[++index];
            vm.ok.TextBlockText = WindowDonateLabels[++index];
            vm.cancel.TextBlockText = WindowDonateLabels[++index];
            String str = Environment.NewLine + globalFuncs.localization.WindowDonateLabels[++index] + Environment.NewLine + Environment.NewLine +
            globalFuncs.localization.WindowDonateLabels[++index] + Environment.NewLine + Environment.NewLine +
            globalFuncs.localization.WindowDonateLabels[++index];
			
			setText(str);
        }

		public void setText(String str)
		{
            this.flowDocument.Blocks.Clear(); 
            Paragraph p1 = new Paragraph();
			p1.TextAlignment = TextAlignment.Center;
            p1.Inlines.Add(new Run(str)); 
			this.flowDocument.Blocks.Add(p1);
		}
		
		private void ButtonDonate_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=1641572");
            globalFuncs.closeWindow(this);
		}

		private void ButtonNoDonation_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            globalFuncs.closeWindow(this);
		}

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            globalFuncs.closeWindow(this);
        }

        private void WindowDonate_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
	}
}