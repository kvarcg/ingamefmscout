using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using FMScout.ViewModel;
using Young3.FMSearch.Core.Entities.InGame;
using FMScout.ControlContext;

namespace FMScout.View
{
    public enum WindowProfileType
    {
        Player = 0,
        Staff = 1,
        Team = 2,
        None = 3
    };
    
    public partial class WindowProfile : Window
	{
        private WindowProfileType type = WindowProfileType.None;
		private Settings settings = null;
        private GlobalFuncs globalFuncs = null;
        private ProfileWindowViewModel vm = null;
        ObservableCollection<ProfileViewModel> data = new ObservableCollection<ProfileViewModel>();

        public WindowProfile(WindowProfileType type)
        {
            this.type = type;
            this.InitializeComponent();

            this.ButtonClose.Click += new RoutedEventHandler(ButtonClose_Click);
            this.MouseDown += new System.Windows.Input.MouseButtonEventHandler(WindowPreferences_MouseDown);
            this.Closed += new System.EventHandler(Window_Closed);

            settings = GlobalSettings.getSettings();
            globalFuncs = Globals.getGlobalFuncs();

            setDataContext();

            this._data.Items.Clear(); 
            this._data.ItemsSource = data;
            this._data.SelectionChanged += new SelectionChangedEventHandler(_data_SelectionChanged);
        }

		private void setDataContext()
        {
            vm = new ProfileWindowViewModel();

            ImageButtonContext close = new ImageButtonContext();
            close.ImageSource = TryFindResource("close") as ImageSource;

            vm.close = close;

            vm.header = new LabeledHeaderContext();
            vm.objectlist = new LabeledHeaderContext();
            if (type == WindowProfileType.Player)
                vm.profiletype = App.Current.Resources["players"] as ImageSource;
            else if (type == WindowProfileType.Staff)
                vm.profiletype = App.Current.Resources["managers"] as ImageSource;
            else if (type == WindowProfileType.Team)
                vm.profiletype = App.Current.Resources["clubs"] as ImageSource;

            setControlValues();
            setLocalization();

            this.DataContext = vm;
        }

        public void setControlValues()
        {

        }

        public void setLocalization()
        {
            ObservableCollection<string> WindowProfileLabels = globalFuncs.localization.WindowProfileLabels;
            vm.header.Header = WindowProfileLabels[(int)type];
            vm.objectlist.Header = WindowProfileLabels[(int)type + 3];
        }

        private int lastSelectedItemID = -1;
        private void _data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                lastSelectedItemID = ((ProfileViewModel)e.AddedItems[0]).ID;
        }

        public void ListItemClicked(object sender, EventArgs e)
        {
            if (sender != null)
            {
                String name = (String)((CheckBox)sender).Content;
                for (int i = 0; i < this._data.Items.Count; ++i)
                {
                    ProfileViewModel vm = (ProfileViewModel)this._data.Items[i];
                    if (vm.SelectionButton.Content.Equals(name))
                    {
                        if (lastSelectedItemID == vm.ID)
                        {
                            vm.SelectionButton.IsChecked = true;
                            return;
                        }
                    }
                }
            }

            for (int i = 0; i < this._data.Items.Count; ++i)
            {
                ProfileViewModel vm = (ProfileViewModel)this._data.Items[i];
                vm.SelectionButton.IsChecked = false;
            }
            
            for (int i = 0; i < this._data.Items.Count; ++i)
            {
                ProfileViewModel vm = (ProfileViewModel)this._data.Items[i];
                if (vm.SelectionButton.Content.Equals(((CheckBox)sender).Content))
                {
                    if (!this._data.SelectedItem.Equals(vm))
                    {
                        this._data.SelectedItem = vm;
                        vm.SelectionButton.IsChecked = true;
                        //this._data
                        break;
                    }
                }
            }
        }

        private void ProfileButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Grid g = (Grid)b.Parent;
            CheckBox b2 = (CheckBox)g.Children[0];

            for (int i = 0; i < this.data.Count; ++i)
            {
                ProfileViewModel vm = (ProfileViewModel)this.data[i];
                if (vm.SelectionButton.Content.Equals(((CheckBox)b2).Content))
                {
                    if (this._data.SelectedItem.Equals(vm))
                    {
                        int index = this._data.SelectedIndex;
                        if (this.data.Count - 1 > 0)
                        {
                            if (index + 1 < this.data.Count)
                            {
                                this._data.SelectedIndex = index + 1;
                                this.data[index + 1].SelectionButton.IsChecked = true;
                            }
                            else
                            {
                                this._data.SelectedIndex = index - 1;
                                this.data[index - 1].SelectionButton.IsChecked = true;
                            }
                            this.data.RemoveAt(i);
                        }
                        else
                            globalFuncs.closeWindow(this);
                    }
                    else
                        this.data.RemoveAt(i);
                    break;
                }
            }
        }

        public int checkExists(int ID)
        {
            for (int i = 0; i < data.Count; ++i)
            {
                if ((int)data[i].ID == ID)
                    return i;
            }
            return -1;
        }

        public void addToContext(BaseObject obj, GridViewModel r)
        {
            int index = checkExists((int)r.ID);
            if (index == -1)
            {
                ProfileViewModel vm = null;
                if (obj is Player)
                {
                    vm = new ProfilePlayerViewModel();
                    Player player = (Player)obj;
                    PlayerGridViewModel _r = (PlayerGridViewModel)r;
                    ((ProfilePlayerViewModel)vm).setProfileViewModel(ref player, ref _r);
                }
                else if (obj is Staff)
                {
                    vm = new ProfileStaffViewModel();
                    Staff staff = (Staff)obj;
                    StaffGridViewModel _r = (StaffGridViewModel)r;
                    ((ProfileStaffViewModel)vm).setProfileViewModel(ref staff, ref _r);
                }
                else if (obj is Team)
                {
                    vm = new ProfileTeamViewModel();
                    Team team = (Team)obj;
                    TeamGridViewModel _r = (TeamGridViewModel)r;
                    ((ProfileTeamViewModel)vm).setProfileViewModel(ref team, ref _r);
                }
                data.Add(vm);
                if (data.Count > 12) data.RemoveAt(0);
            }
        }

        public void finishedAdding()
        {
            for (int i = 0; i < this.data.Count - 1; ++i)
                this.data[i].SelectionButton.IsChecked = false;
               this._data.Items.Refresh();
            this._data.SelectedIndex = this._data.Items.Count - 1;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            for (int i = 0; i < this.data.Count; ++i)
                this.data[i] = null;
            this.data.Clear();
        }

		private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            globalFuncs.closeWindow(this);
        }

        private void WindowPreferences_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
	}
}