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
using FMScout.ViewModel;

namespace FMScout.View
{
	public partial class PlayerProfile : UserControl
	{
        private GlobalFuncs globalFuncs = null;

		public PlayerProfile()
		{
			this.InitializeComponent();
            globalFuncs = Globals.getGlobalFuncs();

			this.Shortlist.Checked += new System.Windows.RoutedEventHandler(ShortlistCheckBox_Checked);
			this.Shortlist.Unchecked += new System.Windows.RoutedEventHandler(ShortlistCheckBox_Unchecked);
            this.ViewTechnicalAttributes.IsChecked = true;
            this.ViewGoalkeepingAttributes.IsChecked = false;
			this.ViewTechnicalAttributes.Checked += new System.Windows.RoutedEventHandler(ViewTechnicalAttributesCheckBox_Checked);
			this.ViewTechnicalAttributes.Unchecked += new System.Windows.RoutedEventHandler(ViewTechnicalAttributesCheckBox_Unchecked);
			this.ViewGoalkeepingAttributes.Checked += new System.Windows.RoutedEventHandler(ViewGoalkeepingAttributesCheckBox_Checked);
			this.ViewGoalkeepingAttributes.Unchecked += new System.Windows.RoutedEventHandler(ViewGoalkeepingAttributesCheckBox_Unchecked);
            this.GoalkeepingAttributes.Opacity = 0;
            this.teamsquad.Visibility = Visibility.Collapsed;
            this.squadno.Visibility = Visibility.Collapsed;
        }

		private void ShortlistCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
		{
            this.Shortlist.Content = globalFuncs.localization.ProfileGenericLabels[ScoutLocalization.PG_REMOVEFROMSHORTLIST];
            ProfilePlayerViewModel vm = (ProfilePlayerViewModel)this.DataContext;
            WindowMain windowMain = Globals.getGlobalFuncs().windowMain;
            windowMain.Shortlist.addToShortlist(vm.ID);
		}

		private void ShortlistCheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
		{
            if (this.DataContext == null) return;
            this.Shortlist.Content = globalFuncs.localization.ProfileGenericLabels[ScoutLocalization.PG_ADDTOSHORTLIST];
            ProfilePlayerViewModel vm = (ProfilePlayerViewModel)this.DataContext;
            WindowMain windowMain = Globals.getGlobalFuncs().windowMain;
            windowMain.Shortlist.removeFromShortlist(vm.ID);
		}

        private bool checking = false;
        private bool comingFromEvent = false;
		private void ViewTechnicalAttributesCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
		{
            if (comingFromEvent)
            {
                comingFromEvent = false;
                return;
            }
            checking = true;
			this.ViewGoalkeepingAttributes.IsChecked = false;
            checking = false;
		}

		private void ViewTechnicalAttributesCheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
		{
            if (checking)
                Globals.getGlobalFuncs().FlipElementVisibility(this.TechnicalAttributes, this.GoalkeepingAttributes, 0.2);
            else
            {
                comingFromEvent = true;
                this.ViewTechnicalAttributes.IsChecked = true;
            }
        }

		private void ViewGoalkeepingAttributesCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (comingFromEvent)
            {
                comingFromEvent = false;
                return;
            }
            checking = true;
            this.ViewTechnicalAttributes.IsChecked = false;
            checking = false;
        }

		private void ViewGoalkeepingAttributesCheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
		{
            if (checking)
                Globals.getGlobalFuncs().FlipElementVisibility(this.GoalkeepingAttributes, this.TechnicalAttributes, 0.2);
            else
            {
                comingFromEvent = true;
                this.ViewGoalkeepingAttributes.IsChecked = true;
            }
        }
	}
}