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
	public partial class WindowAbout : Window
	{
        private GlobalFuncs globalFuncs = null;
        private AboutWindowViewModel vm = null;
        
        public WindowAbout()
		{
			this.InitializeComponent();

            globalFuncs = Globals.getGlobalFuncs();

            setDataContext();

            this.ButtonClose.Click += new System.Windows.RoutedEventHandler(ButtonClose_Click);
            this.MouseDown += new System.Windows.Input.MouseButtonEventHandler(WindowAbout_MouseDown);
            this.ButtonOK.Click += new System.Windows.RoutedEventHandler(ButtonOK_Click);
		}

        private void setDataContext()
        {
            vm = new AboutWindowViewModel();

            ImageButtonContext close = new ImageButtonContext();
            close.ImageSource = TryFindResource("close") as ImageSource;
            ImageTextButtonContext ok = new ImageTextButtonContext();
            ok.ImageSource = TryFindResource("yes") as ImageSource;

            vm.close = close;
            vm.ok = ok;

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
            ObservableCollection<string> WindowAboutLabels = globalFuncs.localization.WindowAboutLabels;
            int index = -1;
            vm.header.Header = WindowAboutLabels[++index] + " " + globalFuncs.CurrentVersion;
            vm.ok.TextBlockText = WindowAboutLabels[++index];

            String str = Environment.NewLine + WindowAboutLabels[++index] + Environment.NewLine + Environment.NewLine +
              WindowAboutLabels[++index] + Environment.NewLine + Environment.NewLine +
              WindowAboutLabels[++index] + Environment.NewLine + Environment.NewLine +
              WindowAboutLabels[++index] + Environment.NewLine + Environment.NewLine +
              WindowAboutLabels[++index] + Environment.NewLine +
              WindowAboutLabels[++index] + Environment.NewLine +
              WindowAboutLabels[++index] + Environment.NewLine;

            setText(str);
        }

		public void setText(String str)
		{
            ObservableCollection<string> WindowAboutLabels = globalFuncs.localization.WindowAboutLabels;
            this.flowDocument.Blocks.Clear();
			Paragraph p1 = new Paragraph();
			p1.TextAlignment = TextAlignment.Center;
            p1.Inlines.Add(new Run(str));
            Hyperlink h1 = new Hyperlink(new Run(Environment.NewLine + "Kostas Vardis"));
            h1.IsEnabled = true;
			h1.NavigateUri = new Uri("http://www.kostasvardis.com");
			h1.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(RequestNavigate);
            Hyperlink h2 = new Hyperlink(new Run(Environment.NewLine + WindowAboutLabels[9]));
            h2.IsEnabled = true; 
            h2.NavigateUri = new Uri("http://www.fmassistant.com");
			h2.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(RequestNavigate);
            Hyperlink h3 = new Hyperlink(new Run(Environment.NewLine + WindowAboutLabels[10]));
            h3.IsEnabled = true; 
            h3.NavigateUri = new Uri("mailto:kvardis@hotmail.com?subject=" + globalFuncs.ApplicationTitle + " " + globalFuncs.CurrentVersion);
			h3.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(RequestNavigate);
            Hyperlink h4 = new Hyperlink(new Run(Environment.NewLine + WindowAboutLabels[11]));
            h4.IsEnabled = true;
            h4.NavigateUri = new Uri("mailto:kvardis@hotmail.com?subject=" + globalFuncs.ApplicationTitle + " " +globalFuncs.CurrentVersion + " Bug Report");
			h4.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(RequestNavigate);
			p1.Inlines.Add(h1);
			p1.Inlines.Add(h2);
			p1.Inlines.Add(h3);
			p1.Inlines.Add(h4);
            p1.Inlines.Add(new Run(Environment.NewLine + Environment.NewLine + "CopyRight © Kostas Vardis 2010-2011" + Environment.NewLine));
			this.flowDocument.Blocks.Add(p1);
		}

        private void RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            globalFuncs.closeWindow(this);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            globalFuncs.closeWindow(this);
        }

        private void WindowAbout_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
	}
}