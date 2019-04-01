using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;
using FMScout.ControlContext;
using FMScout.ViewModel;
using FMScout.ViewModel.MainWindow;
using Young3.FMSearch.Core.Entities.InGame;

namespace FMScout.View
{
    public partial class WindowMain : Window
    {
        private const int playerTabIndex = 0;
        private const int staffTabIndex = 1;
        private const int teamTabIndex = 2;
        private const int shortlistTabIndex = 3;
        private Settings settings = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;
        private CurrentScreenCheckDelegate currentScreenCheckDelegate = null;
        public MainWindowViewModel vm = null;

        private delegate void CurrentScreenCheckDelegate();
        private delegate void CurrentScreenUpdateDelegate(bool status, String text);

        public static readonly RoutedCommand LoadCommand = new RoutedCommand();
        public static readonly RoutedCommand ExitCommand = new RoutedCommand();
        public static readonly RoutedCommand ImportCommand = new RoutedCommand();
        public static readonly RoutedCommand ExportCommand = new RoutedCommand();
        public static readonly RoutedCommand ExportSelectedCommand = new RoutedCommand();
        public static readonly RoutedCommand AddCommand = new RoutedCommand();
        public static readonly RoutedCommand ViewPlayersCommand = new RoutedCommand();
        public static readonly RoutedCommand ViewStaffCommand = new RoutedCommand();
        public static readonly RoutedCommand ViewTeamsCommand = new RoutedCommand();
        public static readonly RoutedCommand ViewShortlistCommand = new RoutedCommand();
        public static readonly RoutedCommand SearchNowCommand = new RoutedCommand();
        public static readonly RoutedCommand ClearPlayersCommand = new RoutedCommand();
        public static readonly RoutedCommand ClearStaffCommand = new RoutedCommand();
        public static readonly RoutedCommand ClearTeamsCommand = new RoutedCommand();
        public static readonly RoutedCommand ClearShortlistCommand = new RoutedCommand();
        public static readonly RoutedCommand ClearAllCommand = new RoutedCommand();
        public static readonly RoutedCommand ToolsCommand = new RoutedCommand();
        public static readonly RoutedCommand AboutCommand = new RoutedCommand();
        public static readonly RoutedCommand DonateCommand = new RoutedCommand();

        private static void OnExecutedLoad(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedLoad(e);
        }

        protected virtual void OnExecutedLoad(ExecutedRoutedEventArgs e)
        {
            this.MenuItemLoadFM_Click(null, null);
        }

        private static void OnExecutedExit(object sender, ExecutedRoutedEventArgs e)
        {
            ((WindowMain)sender).OnExecutedExit(e);
        }

        protected virtual void OnExecutedExit(ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private static void OnExecutedImport(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedImport(e);
        }

        protected virtual void OnExecutedImport(ExecutedRoutedEventArgs e)
        {
            this.MenuItemImportShortlist_Click(null, null);
        }

        private static void OnExecutedExport(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedExport(e);
        }

        protected virtual void OnExecutedExport(ExecutedRoutedEventArgs e)
        {
            this.MenuItemExportShortlist_Click(null, null);
        }

        private static void OnExecutedExportSelected(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedExportSelected(e);
        }

        protected virtual void OnExecutedExportSelected(ExecutedRoutedEventArgs e)
        {
            this.MenuItemExportSelectedShortlist_Click(null, null);
        }

        private static void OnExecutedAdd(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedAdd(e);
        }

        protected virtual void OnExecutedAdd(ExecutedRoutedEventArgs e)
        {
            this.MenuItemAddToShortlist_Click(null, null);
        }

        private static void OnExecutedViewPlayers(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedViewPlayers(e);
        }

        protected virtual void OnExecutedViewPlayers(ExecutedRoutedEventArgs e)
        {
            this.MenuItemSearchPlayersView_Click(null, null);
        }

        private static void OnExecutedViewStaff(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedViewStaff(e);
        }

        protected virtual void OnExecutedViewStaff(ExecutedRoutedEventArgs e)
        {
            this.MenuItemSearchStaffView_Click(null, null);
        }

        private static void OnExecutedViewTeams(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedViewTeams(e);
        }

        protected virtual void OnExecutedViewTeams(ExecutedRoutedEventArgs e)
        {
            this.MenuItemSearchTeamsView_Click(null, null);
        }

        private static void OnExecutedViewShortlist(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedViewShortlist(e);
        }

        protected virtual void OnExecutedViewShortlist(ExecutedRoutedEventArgs e)
        {
            this.MenuItemSearchShortlistView_Click(null, null);
        } 

        private static void OnExecutedSearchNow(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedSearchNow(e);
        }

        protected virtual void OnExecutedSearchNow(ExecutedRoutedEventArgs e)
        {
            MenuItemSearchSearchNow_Click(null, null);
        }
        
        private static void OnExecutedClearPlayers(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedClearPlayers(e);
        }

        protected virtual void OnExecutedClearPlayers(ExecutedRoutedEventArgs e)
        {
            this.MenuItemClearPlayerFields_Click(null, null);
        }

        private static void OnExecutedClearStaff(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedClearStaff(e);
        }

        protected virtual void OnExecutedClearStaff(ExecutedRoutedEventArgs e)
        {
            this.MenuItemClearStaffFields_Click(null, null);
        }

        private static void OnExecutedClearTeams(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedClearTeams(e);
        }

        protected virtual void OnExecutedClearTeams(ExecutedRoutedEventArgs e)
        {
            this.MenuItemClearTeamFields_Click(null, null);
        }

        private static void OnExecutedClearShortlist(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedClearShortlist(e);
        }

        protected virtual void OnExecutedClearShortlist(ExecutedRoutedEventArgs e)
        {
            this.MenuItemClearShortlist_Click(null, null);
        }

        private static void OnExecutedClearAll(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedClearAll(e);
        }

        protected virtual void OnExecutedClearAll(ExecutedRoutedEventArgs e)
        {
            this.MenuItemClearAll_Click(null, null);
        }

        private static void OnExecutedTools(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedTools(e);
        }

        protected virtual void OnExecutedTools(ExecutedRoutedEventArgs e)
        {
            this.MenuItemToolsPreferences_Click(null, null);
        }

        private static void OnExecutedAbout(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedAbout(e);
        }

        protected virtual void OnExecutedAbout(ExecutedRoutedEventArgs e)
        {
            this.MenuItemHelpAbout_Click(null, null);
        }

        private static void OnExecutedDonate(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is WindowMain)
                ((WindowMain)sender).OnExecutedDonate(e);
        }

        protected virtual void OnExecutedDonate(ExecutedRoutedEventArgs e)
        {
            this.MenuItemHelpDonate_Click(null, null);
        }

        public WindowMain()
        {
            Type ownerType = typeof(Window);
            // Load
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(LoadCommand, new KeyGesture(Key.L, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(LoadCommand, new ExecutedRoutedEventHandler(OnExecutedLoad)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ExitCommand, new KeyGesture(Key.X, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ExitCommand, new ExecutedRoutedEventHandler(OnExecutedExit)));
            // Shortlist
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ImportCommand, new KeyGesture(Key.Q, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ImportCommand, new ExecutedRoutedEventHandler(OnExecutedImport)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ExportCommand, new KeyGesture(Key.W, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ExportCommand, new ExecutedRoutedEventHandler(OnExecutedExport)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ExportSelectedCommand, new KeyGesture(Key.E, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ExportSelectedCommand, new ExecutedRoutedEventHandler(OnExecutedExportSelected)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(AddCommand, new KeyGesture(Key.R, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(AddCommand, new ExecutedRoutedEventHandler(OnExecutedAdd)));
            // View
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ViewPlayersCommand, new KeyGesture(Key.A, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ViewPlayersCommand, new ExecutedRoutedEventHandler(OnExecutedViewPlayers)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ViewStaffCommand, new KeyGesture(Key.S, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ViewStaffCommand, new ExecutedRoutedEventHandler(OnExecutedViewStaff)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ViewTeamsCommand, new KeyGesture(Key.D, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ViewTeamsCommand, new ExecutedRoutedEventHandler(OnExecutedViewTeams)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ViewShortlistCommand, new KeyGesture(Key.F, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ViewShortlistCommand, new ExecutedRoutedEventHandler(OnExecutedViewShortlist)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(SearchNowCommand, new KeyGesture(Key.Enter)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(SearchNowCommand, new ExecutedRoutedEventHandler(OnExecutedSearchNow)));
            // Clear
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ClearPlayersCommand, new KeyGesture(Key.Z, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ClearPlayersCommand, new ExecutedRoutedEventHandler(OnExecutedClearPlayers)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ClearStaffCommand, new KeyGesture(Key.X, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ClearStaffCommand, new ExecutedRoutedEventHandler(OnExecutedClearStaff)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ClearTeamsCommand, new KeyGesture(Key.C, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ClearTeamsCommand, new ExecutedRoutedEventHandler(OnExecutedClearTeams)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ClearShortlistCommand, new KeyGesture(Key.B, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ClearShortlistCommand, new ExecutedRoutedEventHandler(OnExecutedClearShortlist)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ClearAllCommand, new KeyGesture(Key.N, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ClearAllCommand, new ExecutedRoutedEventHandler(OnExecutedClearAll)));
            // Tools
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(ToolsCommand, new KeyGesture(Key.T, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ToolsCommand, new ExecutedRoutedEventHandler(OnExecutedTools)));
            // Help
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(AboutCommand, new KeyGesture(Key.H, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(AboutCommand, new ExecutedRoutedEventHandler(OnExecutedAbout)));
            CommandManager.RegisterClassInputBinding(ownerType, new InputBinding(DonateCommand, new KeyGesture(Key.P, ModifierKeys.Control)));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(DonateCommand, new ExecutedRoutedEventHandler(OnExecutedDonate)));

            InitializeComponent();

            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs();
            globalFuncs.windowMain = this;

            setDataContext();
            
            settings = GlobalSettings.getSettings();
            settings.init(); 

            this.PlayerSearch.attributes = this.PlayerSearchAttributes;
            this.StaffSearch.attributes = this.StaffSearchAttributes;
            this.ButtonCurrentScreen.Click += new RoutedEventHandler(ButtonCurrentScreen_Click);
            this.ButtonInformationScreen.Click += new RoutedEventHandler(ButtonInformationScreen_Click);

            this.PlayerSearch.windowMain = this;
            this.StaffSearch.windowMain = this;
            this.TeamSearch.windowMain = this;
            this.Shortlist.windowMain = this;
            //this.PlayerSearch.progressBar.Opacity = 1;
            globalFuncs.shortlistSelected = TryFindResource("shortlisted") as ImageSource;
            globalFuncs.shortlistUnselected = TryFindResource("nshortlisted") as ImageSource;

            reset(false);
            initDataTableColumns();
            clearAllFields();

            this.TabControl.SelectionChanged += new SelectionChangedEventHandler(TabControlSelectionChanged);
            this.MouseUp += new MouseButtonEventHandler(WindowMain_MouseUp);
        }

        private void setDataContext()
        {
            vm = new MainWindowViewModel();

            ImageTextButtonContext currentscreen = new ImageTextButtonContext();
            currentscreen.ImageSource = TryFindResource("status") as ImageSource;
            ImageTextButtonContext tabplayers = new ImageTextButtonContext();
            tabplayers.ImageSource = TryFindResource("players") as ImageSource; 
            ImageTextButtonContext tabstaff = new ImageTextButtonContext();
            tabstaff.ImageSource = TryFindResource("managers") as ImageSource; 
            ImageTextButtonContext tabteams = new ImageTextButtonContext();
            tabteams.ImageSource = TryFindResource("clubs") as ImageSource; 
            ImageTextButtonContext tabshortlist = new ImageTextButtonContext();
            tabshortlist.ImageSource = TryFindResource("shortlist") as ImageSource; 
            ImageTextButtonContext showinfo = new ImageTextButtonContext();
            showinfo.ImageSource = TryFindResource("info") as ImageSource;

            vm.currentscreen = currentscreen;
            vm.tabplayers = tabplayers;
            vm.tabstaff = tabstaff;
            vm.tabteams = tabteams;
            vm.tabshortlist = tabshortlist;
            vm.showinfo = showinfo;

            vm.general = new LabeledHeaderContext();
            vm.attributes = new LabeledHeaderContext();
            vm.currentgamedate = new LabeledHeaderContext();
            vm.menuload = new LabeledHeaderContext();
            vm.menuloadfm = new LabeledHeaderContext();
            vm.menuloadexit = new LabeledHeaderContext();
            vm.menushortlist = new LabeledHeaderContext();
            vm.menushortlistimport = new LabeledHeaderContext();
            vm.menushortlistexport = new LabeledHeaderContext();
            vm.menushortlistexportsel = new LabeledHeaderContext();
            vm.menushortlistadd = new LabeledHeaderContext();
            vm.menusearch = new LabeledHeaderContext();
            vm.menusearchplayers = new LabeledHeaderContext();
            vm.menusearchstaff = new LabeledHeaderContext();
            vm.menusearchteams = new LabeledHeaderContext();
            vm.menusearchshortlist = new LabeledHeaderContext();
            vm.menusearchnow = new LabeledHeaderContext();
            vm.menuclear = new LabeledHeaderContext();
            vm.menuclearplayers = new LabeledHeaderContext();
            vm.menuclearstaff = new LabeledHeaderContext();
            vm.menuclearteams = new LabeledHeaderContext();
            vm.menuclearshortlist = new LabeledHeaderContext();
            vm.menuclearall = new LabeledHeaderContext();
            vm.menutools = new LabeledHeaderContext();
            vm.menutoolspref = new LabeledHeaderContext();
            vm.menuhelp = new LabeledHeaderContext();
            vm.menuhelpabout = new LabeledHeaderContext();
            vm.menuhelpdonate = new LabeledHeaderContext();  

            setControlValues();
            setLocalization();

            this.DataContext = vm;
        }

        public void setControlValues() 
        {

        }

        public void changeLanguage()
        {
            this.setLocalization();
            this.PlayerSearch.setLocalization();
            this.PlayerSearchAttributes.setLocalization();
            this.StaffSearch.setLocalization();
            this.StaffSearchAttributes.setLocalization();
            this.TeamSearch.setLocalization();
            this.Shortlist.setLocalization();
        }

        public void setLocalization()
        {
            ObservableCollection<string> WindowMainLabels = globalFuncs.localization.WindowMainLabels;
            int index = -1;
            vm.tabplayers.TextBlockText = WindowMainLabels[++index];
            vm.tabstaff.TextBlockText = WindowMainLabels[++index];
            vm.tabteams.TextBlockText = WindowMainLabels[++index];
            vm.tabshortlist.TextBlockText = WindowMainLabels[++index];
            vm.general.Header = WindowMainLabels[++index];
            vm.attributes.Header = WindowMainLabels[++index];
            vm.currentgamedate.Header = WindowMainLabels[++index]; 
            vm.currentscreen.TextBlockText = WindowMainLabels[++index];
            vm.showinfo.TextBlockText = WindowMainLabels[++index];
            ObservableCollection<string> MenuLabels = globalFuncs.localization.MenuLabels;
            index = -1;
            vm.menuload.Header = MenuLabels[++index];
            vm.menuloadfm.Header = MenuLabels[index] + " FM 2010";
            vm.menuloadexit.Header = MenuLabels[++index];
            vm.menushortlist.Header = MenuLabels[++index];
            vm.menushortlistimport.Header = MenuLabels[++index];
            vm.menushortlistexport.Header = MenuLabels[++index];
            vm.menushortlistexportsel.Header = MenuLabels[++index];
            vm.menushortlistadd.Header = MenuLabels[++index];
            vm.menusearch.Header = MenuLabels[++index];
            vm.menusearchplayers.Header = MenuLabels[++index];
            vm.menusearchstaff.Header = MenuLabels[++index];
            vm.menusearchteams.Header = MenuLabels[++index];
            vm.menusearchshortlist.Header = MenuLabels[++index];
            vm.menusearchnow.Header = MenuLabels[++index];
            vm.menuclear.Header = MenuLabels[++index];
            vm.menuclearplayers.Header = MenuLabels[++index];
            vm.menuclearstaff.Header = MenuLabels[++index];
            vm.menuclearteams.Header = MenuLabels[++index];
            vm.menuclearshortlist.Header = MenuLabels[++index];
            vm.menuclearall.Header = MenuLabels[++index];
            vm.menutools.Header = MenuLabels[++index];
            vm.menutoolspref.Header = MenuLabels[++index];
            vm.menuhelp.Header = MenuLabels[++index];
            vm.menuhelpabout.Header = MenuLabels[++index];
            vm.menuhelpdonate.Header = MenuLabels[++index];  
        }

        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.MenuItemSearchSearchNow.IsEnabled = true;
            if (this.TabControl.SelectedIndex == 0)
            {
                this.results.DataContext = this.PlayerSearch.vm.results;
            }
            else if (this.TabControl.SelectedIndex == 1)
            {
                this.results.DataContext = this.StaffSearch.vm.results;
            }
            else if (this.TabControl.SelectedIndex == 2)
            {
                this.results.DataContext = this.TeamSearch.vm.results;
            }
            else if (this.TabControl.SelectedIndex == 3)
            {
                this.MenuItemSearchSearchNow.IsEnabled = false;
                this.results.DataContext = this.Shortlist.vm.results;
            }
        }

        private void WindowMain_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.PlayerSearch.windowQuickColumns != null)
            {
                this.PlayerSearch.windowQuickColumns_OK(null, null);
                globalFuncs.closeWindow(this.PlayerSearch.windowQuickColumns);
            }
            if (this.StaffSearch.windowQuickColumns != null)
            {
                this.StaffSearch.windowQuickColumns_OK(null, null);
                globalFuncs.closeWindow(this.StaffSearch.windowQuickColumns);
            }
            if (this.TeamSearch.windowQuickColumns != null)
            {
                this.TeamSearch.windowQuickColumns_OK(null, null);
                globalFuncs.closeWindow(this.TeamSearch.windowQuickColumns);
            }
            if (this.Shortlist.windowQuickColumns != null)
            {
                this.Shortlist.windowQuickColumns_OK(null, null);
                globalFuncs.closeWindow(this.Shortlist.windowQuickColumns);
            }
        }

        public void reset(bool resetStatus)
        {
            if (!resetStatus)
                this.CurrentGameDate.Text = globalFuncs.localization.WindowMainLabels[6];
            if (!resetStatus)
                currentScreenCheckDelegate = null;
            else
            {
                if (!globalFuncs.isDebug)
                {
                    currentScreenCheckDelegate = new CurrentScreenCheckDelegate(this.checkCurrentScreen);
                    currentScreenCheckDelegate.BeginInvoke(null, null);
                }
            }
            this.MenuItemShortlist.IsEnabled = resetStatus;
            this.MenuItemSearch.IsEnabled = resetStatus;
            this.MenuItemClear.IsEnabled = resetStatus;
            this.MenuItemTools.IsEnabled = resetStatus;
            this.ButtonCurrentScreen.IsEnabled = false;
            this.ButtonInformationScreen.IsEnabled = resetStatus;
            this.TabPlayers.IsEnabled = resetStatus;
            this.TabControl.SelectedIndex = 0;
            this.PlayerSearchTabItem.IsEnabled = resetStatus;
            this.PlayerSearch.GroupBoxPlayersSearch.IsEnabled = resetStatus;
            this.PlayerSearch.GroupBoxPlayerDataGridView.IsEnabled = resetStatus;
            this.PlayerAttributesTabItem.IsEnabled = resetStatus;
            this.TabStaff.IsEnabled = resetStatus;
            this.StaffSearchTabItem.IsEnabled = resetStatus;
            this.StaffSearch.GroupBoxStaffSearch.IsEnabled = resetStatus;
            this.StaffSearch.GroupBoxStaffDataGridView.IsEnabled = resetStatus;
            this.StaffAttributesTabItem.IsEnabled = resetStatus;
            this.TabTeams.IsEnabled = resetStatus;
            this.TeamSearch.IsEnabled = resetStatus;
            this.TeamSearch.GroupBoxTeamsSearch.IsEnabled = resetStatus;
            this.TeamSearch.GroupBoxTeamsDataGridView.IsEnabled = resetStatus;
            this.TabShortlist.IsEnabled = resetStatus;
            this.Shortlist.GroupBoxShortlistDataGridView.IsEnabled = resetStatus;
        }

        public void checkCurrentScreen()
        {
            String text = "";
            while (true)
            {
                object activeObject = context.fm.MetaData.ActiveObject;
                bool status = activeObject != null;

                if (activeObject is Player)
                {
                    text = ((Player)activeObject).ToString();
                }
                else if (activeObject is Staff)
                {
                    text = ((Staff)activeObject).ToString();
                }
                else if (activeObject is Team)
                {
                    if (((Team)activeObject).Club != null)
                        text = ((Team)activeObject).Club.Name;
                }

                this.Dispatcher.BeginInvoke(
                 System.Windows.Threading.DispatcherPriority.Normal,
                 new CurrentScreenUpdateDelegate(this.updateCurrentScreen),
                     status, text
                 );

                Thread.Sleep(2000);
            }
        }

        public void updateCurrentScreen(bool status, String text)
        {
            if (!status)
            {
                if (ButtonCurrentScreen.IsEnabled != false)
                {
                    ButtonCurrentScreen.IsEnabled = false;
                    vm.currentscreen.TextBlockText = globalFuncs.localization.WindowMainLabels[7];
                }
            }
            else
            {
                if (ButtonCurrentScreen.IsEnabled != true)
                    ButtonCurrentScreen.IsEnabled = true;
                vm.currentscreen.TextBlockText = text;
            }
        }

        public void ButtonCurrentScreen_Click(object sender, RoutedEventArgs e)
        {
            if (context.scoutLoaded)
            {
                object activeObject = context.fm.MetaData.ActiveObject;
                if (activeObject is Player)
                    this.PlayerSearch.createWindowProfile(((Player)activeObject).ID);
                else if (activeObject is Staff)
                    this.StaffSearch.createWindowProfile(((Staff)activeObject).ID);
                else if (activeObject is Team)
                    this.TeamSearch.createWindowProfile(((Team)activeObject).ID);
                else
                {
                    if (ButtonCurrentScreen.IsEnabled != true)
                        ButtonCurrentScreen.IsEnabled = true;
                }

            }
        }

        public void loadFM()
        {
            this.IsEnabled = false;
            this.reset(false);
            clearAllFields();
            
            WindowLoading windowLoading = new WindowLoading(this);
            windowLoading.Owner = this;
            windowLoading.Opacity = 0;
            windowLoading.setLoading(globalFuncs.localization.WindowLoadingLabels[0]);
            windowLoading.Show();
            Storyboard s = globalFuncs.FadeInElement(windowLoading, globalFuncs.windowDuration, globalFuncs.windowFinalOpacity, false);
            s.Completed += delegate(object _sender, EventArgs _e) { windowLoadingFadeInCompleted(ref windowLoading); };
            s.Begin();
        }

        public void windowLoadingFadeInCompleted(ref WindowLoading windowLoading)
        {
            windowStartLoadingDelegate d = new windowStartLoadingDelegate(this.startLoading);
            d.BeginInvoke(windowLoading, null, null);
        }

        private delegate void windowStartLoadingDelegate(WindowLoading windowLoading);
        private delegate void updateLabelDelegate(String str);
        private delegate void windowFinishLoadingDelegate(bool res);

        public void startLoading(WindowLoading windowLoading)
        {
            globalFuncs.scoutTimer.start();
            context.init();

            bool check = context.loaded;
            if (globalFuncs.isDebug) check = true;
            if (check)
            {
                this.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new updateLabelDelegate(windowLoading.setLoading),
                  globalFuncs.localization.WindowLoadingLabels[1] + " " + context.fm.MetaData.CurrentVersion + " ..." + globalFuncs.localization.WindowLoadingLabels[2]);

                this.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new updateLabelDelegate(windowLoading.setInfo),
                  globalFuncs.localization.WindowLoadingLabels[3]);

                if (!globalFuncs.isDebug)
                    context.loadFMData();

                if (!globalFuncs.isDebug) 
                    this.Dispatcher.BeginInvoke(
                   System.Windows.Threading.DispatcherPriority.Normal,
                   new updateLabelDelegate(this.updateDate),
                   context.fm.MetaData.IngameDate.ToLongDateString());

                this.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  new updateLabelDelegate(windowLoading.setInfo),
                  globalFuncs.localization.WindowLoadingLabels[4]);

                context.loadScoutData();

                this.PlayerSearch.fullName.TextBox.AutoSuggestionList = globalFuncs.playersFixed;
                this.PlayerSearch.fullName.TextBox.DisplayAutoSuggestionList = globalFuncs.players;
                this.StaffSearch.fullName.TextBox.AutoSuggestionList = globalFuncs.staffFixed;
                this.StaffSearch.fullName.TextBox.DisplayAutoSuggestionList = globalFuncs.staff;
                this.TeamSearch.name.TextBox.AutoSuggestionList = globalFuncs.clubs;
                this.PlayerSearch.nation.TextBox.AutoSuggestionList = globalFuncs.countries;
                this.StaffSearch.nation.TextBox.AutoSuggestionList = globalFuncs.countries;
                this.TeamSearch.nation.TextBox.AutoSuggestionList = globalFuncs.countries;
                this.PlayerSearch.club.TextBox.AutoSuggestionList = globalFuncs.clubs;
                this.StaffSearch.club.TextBox.AutoSuggestionList = globalFuncs.clubs;
                this.TeamSearch.stadium.TextBox.AutoSuggestionList = globalFuncs.stadiums;

                globalFuncs.scoutTimer.stop();
                String str = "";
                if (!globalFuncs.isDebug)
                {
                    str = globalFuncs.localization.WindowLoadingLabels[5] + " " + context.fm.MetaData.CurrentVersion + " " + 
                        globalFuncs.localization.WindowLoadingLabels[6] + " "
                    + globalFuncs.scoutTimer.seconds() + " " + globalFuncs.localization.WindowLoadingLabels[7];
                }
                else
                {
                    str = globalFuncs.localization.WindowLoadingLabels[5] + " debug " +
                        globalFuncs.localization.WindowLoadingLabels[6] + " "
                    + globalFuncs.scoutTimer.seconds() + " " + globalFuncs.localization.WindowLoadingLabels[7];
                }

                this.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new updateLabelDelegate(windowLoading.setLoading),
                    "");

                this.Dispatcher.BeginInvoke(
                   System.Windows.Threading.DispatcherPriority.Normal,
                   new updateLabelDelegate(windowLoading.setLoading),
                   str);
                Thread.Sleep(1000);

                this.Dispatcher.BeginInvoke(
                   System.Windows.Threading.DispatcherPriority.Normal,
                   new windowFinishLoadingDelegate(windowLoading.finishLoading),
                   true);
            }
            else
            {
                globalFuncs.scoutTimer.stop();
                for (int i = 5; i >= 0; --i)
                {
                    String str = globalFuncs.localization.WindowLoadingLabels[8] + Environment.NewLine +
                                 globalFuncs.localization.WindowLoadingLabels[9] + Environment.NewLine + Environment.NewLine +
                                 globalFuncs.localization.WindowLoadingLabels[10] + " " + i + " " + globalFuncs.localization.WindowLoadingLabels[7];
                    this.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new updateLabelDelegate(windowLoading.setLoading),
                    str);
                    Thread.Sleep(1000);
                }
                this.Dispatcher.BeginInvoke(
                   System.Windows.Threading.DispatcherPriority.Normal,
                   new windowFinishLoadingDelegate(windowLoading.finishLoading),
                   false);
            }
        }

        public void updateDate(String date)
        {
            this.CurrentGameDate.Text = date; 
            this.CurrentGameDate.SetResourceReference(ForegroundProperty, "ButtonFocusGradientBorder"); 
        }

        public void initDataTableColumns()
        {
            this.PlayerSearch.initDataTableColumns(ref settings.playerColumnsWidth);
            this.StaffSearch.initDataTableColumns(ref settings.staffColumnsWidth);
            this.TeamSearch.initDataTableColumns(ref settings.teamColumnsWidth);
            this.Shortlist.initDataTableColumns(ref settings.shortlistColumnsWidth);
        }

        private void clearPlayerFields()
        {
            this.PlayerSearch.clearData();
            this.PlayerSearchAttributes.clearData();
            vm.tabplayers.TextBlockText = globalFuncs.localization.WindowMainLabels[0];
        }

        private void clearStaffFields()
        {
            this.StaffSearch.clearData();
            this.StaffSearchAttributes.clearData();
            vm.tabstaff.TextBlockText = globalFuncs.localization.WindowMainLabels[1]; 
        }

        private void clearTeamFields()
        {
            this.TeamSearch.clearData();
            vm.tabteams.TextBlockText = globalFuncs.localization.WindowMainLabels[2]; 
        }

        private void clearShortlistFields()
        {
            this.Shortlist.clearData();
            vm.tabshortlist.TextBlockText = globalFuncs.localization.WindowMainLabels[3]; 
        }

        private void clearAllFields()
        {
            clearPlayerFields();
            clearStaffFields();
            clearTeamFields();
            clearShortlistFields();
        }

        private void MenuItemLoadFM_Click(object sender, RoutedEventArgs e)
        {
            loadFM();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItemImportShortlist_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            this.Shortlist.loadShortlist();
        }

        private void MenuItemExportShortlist_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            this.Shortlist.exportShortlist(true);
        }

        private void MenuItemExportSelectedShortlist_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            this.Shortlist.exportShortlist(false);
        }

        private void MenuItemAddToShortlist_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            this.Shortlist.addToShortlist(ref this.PlayerSearch.dataGrid);
        }

        private void MenuItemViewShortlist_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            this.TabControl.SelectedIndex = shortlistTabIndex;
        }

        private void MenuItemSearchPlayersView_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            this.TabControl.SelectedIndex = playerTabIndex;
        }

        private void MenuItemSearchStaffView_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            this.TabControl.SelectedIndex = staffTabIndex;
        }

        private void MenuItemSearchTeamsView_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            this.TabControl.SelectedIndex = teamTabIndex;
        }

        private void MenuItemSearchShortlistView_Click(object sender, RoutedEventArgs e) 
        {
            if (!context.scoutLoaded) return; 
            this.TabControl.SelectedIndex = shortlistTabIndex;
        }

        private void MenuItemSearchSearchNow_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            switch (this.TabControl.SelectedIndex)
            {
                case playerTabIndex: this.PlayerSearch.ButtonSearch_Click(null, null); break;
                case staffTabIndex: this.StaffSearch.ButtonSearch_Click(null, null); break;
                case teamTabIndex: this.TeamSearch.ButtonSearch_Click(null, null); break;
            }
        }

        private void MenuItemClearPlayerFields_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            clearPlayerFields();
        }

        private void MenuItemClearStaffFields_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            clearStaffFields();
        }

        private void MenuItemClearTeamFields_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            clearTeamFields();
        }

        private void MenuItemClearShortlist_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            clearShortlistFields();
        }

        private void MenuItemClearAll_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            clearAllFields();
        }

        private void MenuItemToolsPreferences_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            WindowPreferences w = new WindowPreferences();
            w.Owner = this;
            w.Opacity = 0;
            settings.settingSettings = false;
            w.Show();
            settings.settingSettings = true;
            globalFuncs.FadeInElement(w, globalFuncs.windowDuration, globalFuncs.windowFinalOpacity, true);
        }

        private void MenuItemHelpAbout_Click(object sender, RoutedEventArgs e)
        {
            WindowAbout w = new WindowAbout();
            w.Owner = this;
            w.Opacity = 0;
            w.Show();
            globalFuncs.FadeInElement(w, globalFuncs.windowDuration, globalFuncs.windowFinalOpacity, true);
        }

        private void MenuItemHelpDonate_Click(object sender, RoutedEventArgs e)
        {
            WindowDonate w = new WindowDonate();
            w.Owner = this;
            w.Opacity = 0;
            w.Show();
            globalFuncs.FadeInElement(w, globalFuncs.windowDuration, globalFuncs.windowFinalOpacity, true);
        }

        WindowInfo windowInfo = null;
        public void ButtonInformationScreen_Click(object sender, RoutedEventArgs e)
        {
            if (!context.scoutLoaded) return;
            if (windowInfo == null)
            {
                windowInfo = new WindowInfo();
                windowInfo.Owner = this;
                windowInfo.Opacity = 0;
                windowInfo.Show();
                windowInfo.setPosition(ref this.ButtonInformationScreen);
                globalFuncs.FadeInElement(windowInfo, globalFuncs.windowDuration, globalFuncs.windowFinalOpacity, true);
                windowInfo.Closed += new EventHandler(windowInfo_Closed);
                this.vm.showinfo.TextBlockText = globalFuncs.localization.WindowMainLabels[9];
            }
            else
            {
                globalFuncs.closeWindow(windowInfo);
                this.vm.showinfo.TextBlockText = globalFuncs.localization.WindowMainLabels[8];
            }
        }

        private void windowInfo_Closed(object sender, EventArgs e)
        {
            windowInfo = null;
            this.vm.showinfo.TextBlockText = globalFuncs.localization.WindowMainLabels[8];
        }
    }    
}
