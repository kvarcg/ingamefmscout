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

namespace FMScout.View
{
	public partial class TeamProfile : UserControl
	{
		public TeamProfile()
		{
			this.InitializeComponent();

            this.ButtonListPlayers.Visibility = Visibility.Collapsed;
            this.ButtonListStaff.Visibility = Visibility.Collapsed;
            this.ButtonHealTeam.Visibility = Visibility.Collapsed;
            this.ComboBoxListPlayers.Visibility = Visibility.Collapsed;
            this.ButtonListPlayers.Click += new RoutedEventHandler(ButtonListPlayers_Click);
            this.ButtonListStaff.Click += new RoutedEventHandler(ButtonListStaff_Click);
            this.ButtonHealTeam.Click += new RoutedEventHandler(ButtonHealTeam_Click);
            this.totalwage.Visibility = Visibility.Collapsed;
            this.usedwage.Visibility = Visibility.Collapsed;
        }

        public void ButtonListPlayers_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ButtonListStaff_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ButtonHealTeam_Click(object sender, RoutedEventArgs e)
        {

        }
	}
}