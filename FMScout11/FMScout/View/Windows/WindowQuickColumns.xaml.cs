using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;
using FMScout.ViewModel;
using FMScout.ControlContext;

namespace FMScout.View
{
    public partial class WindowQuickColumns : Window
	{
        private Settings settings = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;
        private QuickColumnsWindowViewModel vm = null;
        private List<int> defaultColumns;
        private bool settingSettings = false;

        public WindowQuickColumns()
		{
			this.InitializeComponent();

            settings = GlobalSettings.getSettings();
            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs();

            setDataContext();

            this.ButtonDefault.Click += new RoutedEventHandler(ButtonDefault_Click); 
            this.ButtonClose.Click += new RoutedEventHandler(ButtonClose_Click);
            //this.MouseDown += new System.Windows.Input.MouseButtonEventHandler(WindowInfo_MouseDown);

            defaultColumns = new List<int>();
		}

        private void setDataContext()
        {
            vm = new QuickColumnsWindowViewModel();

            ImageButtonContext close = new ImageButtonContext();
            close.ImageSource = TryFindResource("close") as ImageSource;
            ImageButtonContext def = new ImageButtonContext();
            def.ImageSource = TryFindResource("default") as ImageSource;
            ImageButtonContext ok = new ImageButtonContext();
            ok.ImageSource = TryFindResource("yes") as ImageSource;

            vm.close = close;
            vm.def = def;
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
            PreferencesSettings s = GlobalSettings.getSettings().curPreferencesSettings;
            ObservableCollection<string> WindowCustomizeColumnsLabels = globalFuncs.localization.WindowCustomizeColumnsLabels;
            int index = -1;
            vm.header.Header = WindowCustomizeColumnsLabels[++index];
        }

        public void setColumns(ObservableCollection<DataGridColumn> gridColumns)
        {
            WrapPanel wrapPanel = this.WrapPanelColumns;
            defaultColumns.Clear();
            for (int i = 0; i < gridColumns.Count - 1; ++i)
            {
                CheckBox item = new CheckBox();
                item.Style = App.Current.Resources["CheckBox"] as Style;
                item.Content = gridColumns[i].Header;
                item.Margin = new Thickness(0, 0, 2, 2);
                if (gridColumns[i].Visibility == Visibility.Visible)
                {
                    defaultColumns.Add(i);
                    item.IsChecked = true;
                }
                item.Checked += new RoutedEventHandler(checkListItem_Checked);
                item.Unchecked += new RoutedEventHandler(checkListItem_Unchecked);
                wrapPanel.Children.Add(item);
            }
        }

        private void ButtonDefault_Click(object sender, RoutedEventArgs e)
        {
            settingSettings = true;
            WrapPanel wrapPanel = this.WrapPanelColumns;
            for (int i = 0; i < wrapPanel.Children.Count; ++i)
            {
                CheckBox item = (CheckBox)wrapPanel.Children[i];
                item.IsChecked = false;
            }

            for (int i = 0; i < wrapPanel.Children.Count; ++i)
            {
                CheckBox item = (CheckBox)wrapPanel.Children[i];
                if (defaultColumns.Contains(i))
                    item.IsChecked = true;
            }
            settingSettings = false;
        }

        private void checkListItem_Checked(object sender, RoutedEventArgs e)
        {
            if (!settings.usingCustomSettings && !settingSettings)
            {
                settings.addCustomSetting();
                settings.preferencesSettings[0].isCurrent = true;
            }
        }

        private void checkListItem_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!settings.usingCustomSettings && !settingSettings)
            {
                settings.addCustomSetting();
                settings.preferencesSettings[0].isCurrent = true;
            }
        }

        public void setPosition(ref Button b)
        {
            Point currentPoint = b.PointToScreen(new Point(b.ActualWidth, b.ActualHeight));
            this.Left = currentPoint.X - this.ActualWidth;
            this.Top = currentPoint.Y - (b.Height * 0.5);
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