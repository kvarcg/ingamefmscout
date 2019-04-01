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
using FMScout.UserControls;

namespace FMScout.View.PreferencesWindow
{
	public partial class TeamColumnsTabItem : UserControl
	{
		public TeamColumnsTabItem()
		{
			this.InitializeComponent();

            this.ButtonColumnsDefault.Click += new RoutedEventHandler(ButtonColumnsDefault_Click);
            this.ButtonColumnsSelectAll.Click += new RoutedEventHandler(ButtonColumnsSelectAll_Click);
            this.ButtonColumnsClear.Click += new RoutedEventHandler(ButtonColumnsClear_Click);
        }

        public void ButtonColumnsDefault_Click(object sender, RoutedEventArgs e)
        {
            WrapPanel wrap = this.WrapPanelColumns;
            Settings s = GlobalSettings.getSettings();
            PreferencesSettings curSettings = s.curPreferencesSettings;
            for (int i = 0; i < wrap.Children.Count; ++i)
                ((CheckBox)wrap.Children[i]).IsChecked = false;
            for (int i = 0; i < curSettings.teamColumns.Count; ++i)
                ((CheckBox)wrap.Children[curSettings.teamColumns[i]]).IsChecked = true;
        }

        public void ButtonColumnsSelectAll_Click(object sender, RoutedEventArgs e)
        {
            WrapPanel wrap = this.WrapPanelColumns;
            for (int i = 0; i < wrap.Children.Count; ++i)
                ((CheckBox)wrap.Children[i]).IsChecked = true;
        }

        public void ButtonColumnsClear_Click(object sender, RoutedEventArgs e)
        {
            WrapPanel wrap = this.WrapPanelColumns;
            for (int i = 0; i < wrap.Children.Count; ++i)
                ((CheckBox)wrap.Children[i]).IsChecked = false;
        }
	}
}