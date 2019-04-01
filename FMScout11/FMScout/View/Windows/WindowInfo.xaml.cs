using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using FMScout.ViewModel;
using FMScout.ControlContext;

namespace FMScout.View
{
    public partial class WindowInfo : Window
    {
        private Context context = null;
        private GlobalFuncs globalFuncs = null;
        private InfoWindowViewModel vm = null;

        public WindowInfo()
        {
            InitializeComponent();

            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs();

            setDataContext();

            this.ButtonClose.Click += new RoutedEventHandler(ButtonClose_Click);
            this.MouseDown += new System.Windows.Input.MouseButtonEventHandler(WindowInfo_MouseDown);
        }

        private void setDataContext()
        {
            vm = new InfoWindowViewModel();

            ImageButtonContext close = new ImageButtonContext();
            close.ImageSource = TryFindResource("close") as ImageSource;

            vm.close = close;

            vm.header = new LabeledHeaderContext();
            vm.gamesettings = new LabeledHeaderContext();
            vm.gamestatus = new LabeledHeaderContext();
            vm.gamestatusinfo = new LabeledHeaderContext();
            vm.gameversion = new LabeledHeaderContext();
            vm.gameversioninfo = new LabeledHeaderContext();
            vm.players = new LabeledHeaderContext();
            vm.playersinfo = new LabeledHeaderContext();
            vm.staff = new LabeledHeaderContext();
            vm.staffinfo = new LabeledHeaderContext();
            vm.teams = new LabeledHeaderContext();
            vm.teamsinfo = new LabeledHeaderContext();
            vm.scoutsettings = new LabeledHeaderContext();
            vm.settings = new LabeledHeaderContext();
            vm.settingsinfo = new LabeledHeaderContext();
            vm.language = new LabeledHeaderContext();
            vm.languageinfo = new LabeledHeaderContext();
            vm.theme = new LabeledHeaderContext();
            vm.themeinfo = new LabeledHeaderContext();
            vm.currency = new LabeledHeaderContext();
            vm.currencyinfo = new LabeledHeaderContext();
            vm.wage = new LabeledHeaderContext();
            vm.wageinfo = new LabeledHeaderContext();
            vm.heightdistance = new LabeledHeaderContext();
            vm.heightdistanceinfo = new LabeledHeaderContext();
            vm.weight = new LabeledHeaderContext();
            vm.weightinfo = new LabeledHeaderContext();
            vm.editorsettings = new LabeledHeaderContext();
            vm.editing = new LabeledHeaderContext();
            vm.editinginfo = new LabeledHeaderContext();

            setControlValues();
            setLocalization();

            this.DataContext = vm;
        }

        public void setControlValues()
        {
            if (!globalFuncs.isDebug)
                vm.gameversioninfo.Header = context.fm.MetaData.CurrentVersion;
            if (!globalFuncs.isDebug)
                vm.playersinfo.Header = context.fm.Players.Count().ToString();
            if (!globalFuncs.isDebug)
                vm.staffinfo.Header = context.fm.Staff.Count().ToString();
            vm.teamsinfo.Header = globalFuncs.allClubs.Count.ToString();
        }

        public void setLocalization()
        {
            PreferencesSettings s = GlobalSettings.getSettings().curPreferencesSettings;
            ScoutLocalization localization = globalFuncs.localization;
            ObservableCollection<string> WindowInfoLabels = globalFuncs.localization.WindowInfoLabels;
            int index = -1;
            vm.header.Header = WindowInfoLabels[++index];
            vm.gamesettings.Header = WindowInfoLabels[++index];
            vm.gamestatus.Header = WindowInfoLabels[++index];
            vm.gamestatusinfo.Header = WindowInfoLabels[++index];
            vm.gameversion.Header = WindowInfoLabels[++index];
            vm.players.Header = WindowInfoLabels[++index];
            vm.staff.Header = WindowInfoLabels[++index];
            vm.teams.Header = WindowInfoLabels[++index];
            vm.scoutsettings.Header = WindowInfoLabels[++index];
            vm.settings.Header = WindowInfoLabels[++index];
            vm.settingsinfo.Header = s.name;
            vm.language.Header = WindowInfoLabels[++index];
            vm.languageinfo.Header = s.language;
            vm.theme.Header = WindowInfoLabels[++index];
            vm.themeinfo.Header = s.theme;
            vm.currency.Header = WindowInfoLabels[++index];
            vm.currencyinfo.Header = localization.currencies[localization.currenciesNative.IndexOf(s.currency)];
            vm.wage.Header = WindowInfoLabels[++index];
            vm.wageinfo.Header = localization.wages[localization.wagesNative.IndexOf(s.wage)];
            vm.heightdistance.Header = WindowInfoLabels[++index];
            vm.heightdistanceinfo.Header = localization.heights[localization.heightsNative.IndexOf(s.height)];
            vm.weight.Header = WindowInfoLabels[++index];
            vm.weightinfo.Header = localization.weights[localization.weightsNative.IndexOf(s.weight)];
            vm.editorsettings.Header = WindowInfoLabels[++index];
            vm.editing.Header = WindowInfoLabels[++index];
            vm.editinginfo.Header = localization.YesNo[localization.editing.IndexOf(s.editing)];
        }

        public void setPosition(ref Button b)
        {
            Point currentPoint = b.PointToScreen(new Point(b.ActualWidth, b.ActualHeight));
            this.Left = currentPoint.X - this.ActualWidth;
            this.Top = currentPoint.Y - b.Height - this.ActualHeight;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            globalFuncs.closeWindow(this);
        }

        private void WindowInfo_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
