using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;
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
using FMScout.ControlContext;
using FMScout.ViewModel;
using FMScout.View;
using Young3.FMSearch.Core.Entities.InGame;
using System.Threading;

namespace FMScout.View.MainWindow
{
    public partial class StaffSearchTabItem : UserControl
    {
        //public DataTable dataTable = null;
        public ObservableCollection<StaffGridViewModel> dataTable = null;
        //public List<DataColumn> dataColumnList = null;
        public StaffSearchAttributes attributes = null; 
        public StaffSearchTabItemViewModel vm = null;
        public WindowMain windowMain = null;
        private WindowProfile windowProfile = null;
        public WindowQuickColumns windowQuickColumns = null;
        private Settings settings = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;
        private SearchUIStaff searchUI = null;
        private bool searching = false;
        private bool stopSearching = false;
        private bool finishedLoading = true;

        private delegate void SearchDelegate();
        private delegate void CurrentResultDelegate(StaffGridViewModel newRow);
        private delegate void FinalResultDelegate();
        private delegate void ProgressBarDelegate();
        private delegate void SetProgressBarValueDelegate();

        public StaffSearchTabItem()
        {
            InitializeComponent();
            this.progressBar.Opacity = 0;

            settings = GlobalSettings.getSettings();
            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs();

            searchUI = new SearchUIStaff();

            setDataContext();
            this.ButtonSearch.Click += new RoutedEventHandler(ButtonSearch_Click);
            this.ButtonWonder.Click += new RoutedEventHandler(ButtonWonder_Click);
            this.ButtonColumns.Click += new RoutedEventHandler(ButtonColumns_Click);
            this.dataGrid.MouseDoubleClick += new MouseButtonEventHandler(dataGrid_MouseDoubleClick);
            this.dataGrid.MouseLeftButtonUp += new MouseButtonEventHandler(dataGrid_MouseLeftButtonUp);
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.dataGrid.SelectedItems.Count > 0)
                createWindowProfile(-1);
        }

        public void createWindowProfile(int activeObjectID)
        {
            if (windowProfile == null)
            {
                windowProfile = new WindowProfile(WindowProfileType.Staff);
                windowProfile.setLocalization();
                if (activeObjectID == -1)
                    addToContext();
                else
                    addToContext(activeObjectID);
                windowProfile.Owner = windowMain;
                windowProfile.Opacity = 0;
                windowProfile.Show();
                windowProfile.Closed += new EventHandler(windowProfile_Closed);
                globalFuncs.FadeInElement(windowProfile, 0.5, globalFuncs.windowFinalOpacity, true);
            }
            else
            {
                if (activeObjectID == -1)
                    addToContext();
                else
                    addToContext(activeObjectID);
            }
        }

        private void addToContext()
        {
            if (windowProfile == null) return;
            int selectedRows = this.dataGrid.SelectedItems.Count;
            int curSelectedRows = 0;
            if (selectedRows > 0)
            {
                List<StaffGridViewModel> rows = new List<StaffGridViewModel>();
                for (int i = 0; i < this.dataGrid.SelectedItems.Count; ++i)
                    rows.Add((StaffGridViewModel)this.dataGrid.SelectedItems[i]);

                foreach (Staff staff in context.fm.Staff)
                {
                    int ID = staff.ID;
                    for (int i = 0; i < rows.Count; ++i)
                    {
                        StaffGridViewModel r = rows[i];
                        if (ID == (int)rows[i].ID)
                        {
                            windowProfile.addToContext(staff, r);
                            ++curSelectedRows;
                            break;
                        }
                    }
                    if (curSelectedRows > selectedRows) break;
                }
                windowProfile.finishedAdding();
            }
        }

        private void addToContext(int ID)
        {
            if (windowProfile == null) return;
            foreach (Staff staff in context.fm.Staff)
            {
                if (ID == staff.ID)
                {
                    // check role
                    String staffRole = "";
                    context.find_staff_role(staff, ref staffRole, ref searchUI.role, true);

                    //check club
                    String staffClub = "";
                    STAFFCLUBSTATE staffClubState = STAFFCLUBSTATE.SCS_FREE;
                    context.find_staff_club(staff, ref staffClub, ref staffClubState);

                    StaffGridViewModel r = new StaffGridViewModel();
                    this.addStaffToGrid(staff, ref r, ref staffRole, ref staffClub);
                    windowProfile.addToContext(staff, r);
                    break;
                }
            }
            windowProfile.finishedAdding();
        }

        private void dataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            addToContext();
        }

        private void windowProfile_Closed(object sender, EventArgs e)
        {
            windowProfile.Closed -= new EventHandler(windowProfile_Closed);
            windowProfile = null;
        }

        public void ButtonColumns_Click(object sender, RoutedEventArgs e)
        {
            if (windowQuickColumns == null)
            {
                windowQuickColumns = new WindowQuickColumns();
                windowQuickColumns.setColumns(this.dataGrid.Columns);
                windowQuickColumns.Owner = this.windowMain;
                windowQuickColumns.Opacity = 0;
                windowQuickColumns.Show();
                windowQuickColumns.setPosition(ref this.ButtonColumns);
                windowQuickColumns.ButtonOK.Click += new RoutedEventHandler(windowQuickColumns_OK);
                windowQuickColumns.Closed += new EventHandler(windowQuickColumns_Closed);
                globalFuncs.FadeInElement(windowQuickColumns, globalFuncs.windowDuration, globalFuncs.windowFinalOpacity, true);
            }
        }

        private void windowQuickColumns_Closed(object sender, EventArgs e)
        {
            windowQuickColumns.ButtonOK.Click -= new RoutedEventHandler(windowQuickColumns_OK);
            windowQuickColumns.Closed -= new EventHandler(windowQuickColumns_Closed);
            windowQuickColumns = null;
        }

        public void windowQuickColumns_OK(object sender, RoutedEventArgs e)
        {
            WrapPanel wrapPanel = windowQuickColumns.WrapPanelColumns;
            setColumns(ref wrapPanel);
        }

        public void setColumns(ref WrapPanel wrapPanel)
        {
            for (int i = 0; i < wrapPanel.Children.Count; ++i)
            {
                CheckBox item = (CheckBox)wrapPanel.Children[i];
                DataGridColumn column = this.dataGrid.Columns[i];
                if (item.IsChecked == true)
                {
                    if (column.Visibility == Visibility.Collapsed)
                        this.dataGrid.Columns[i].Visibility = Visibility.Visible;
                }
                else
                {
                    if (column.Visibility == Visibility.Visible)
                        this.dataGrid.Columns[i].Visibility = Visibility.Collapsed;
                }
            }
        }

        private double progressBarValue = 0;
        private void setProgressBarValue()
        {
            if (progressBarValue > 99) progressBarValue = 100;
            this.progressBar.Value = progressBarValue;
        }

        SetProgressBarValueDelegate setProgressBarValueDelegate = null;
        private void updateProgressBar()
        {
            while (true)
            {
                if (setProgressBarValueDelegate == null) return;
                this.Dispatcher.BeginInvoke(
                 System.Windows.Threading.DispatcherPriority.Normal,
                 setProgressBarValueDelegate);
                Thread.Sleep(10);
            }
        }

        public void setControlAvailability(bool res)
        {
            this.GroupBoxStaffSearch1.IsEnabled = res;
            this.GroupBoxStaffSearch2.IsEnabled = res;
            this.GroupBoxStaffSearch3.IsEnabled = res;
            this.windowMain.StaffSearchAttributes.TacticalAttributes.IsEnabled = res;
            this.windowMain.StaffSearchAttributes.MentalAttributes.IsEnabled = res;
            this.windowMain.StaffSearchAttributes.NonTacticalAttributes.IsEnabled = res;
            this.windowMain.StaffSearchAttributes.ChairmanAttributes.IsEnabled = res;
            this.windowMain.MenuItemClearStaffFields.IsEnabled = res;
            this.dataGrid.IsEnabled = res;
            this.searching = !res;
            if (!res)
            {
                finishedLoading = false;
                this.dataTable.Clear();
                this.progressBar.Value = 0;
                globalFuncs.FadeInElement(this.progressBar, globalFuncs.progressBarDuration, 1, true);
                setProgressBarValueDelegate = new SetProgressBarValueDelegate(this.setProgressBarValue);
            }
            else
            {
                finishedLoading = true;
                globalFuncs.FadeOutElement(this.progressBar, globalFuncs.progressBarDuration, this.progressBar.Opacity);
                setProgressBarValueDelegate = null;
                this.dataGrid.Items.Refresh();
            }
        }

        public void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            globalFuncs.scoutTimer.start();

            if (searching) 
            {
                stopSearching = true;
                searching = false;
                return;
            }

            this.windowMain.CurrentGameDate.Text = context.fm.MetaData.IngameDate.ToLongDateString();
            this.windowMain.vm.tabstaff.TextBlockText = globalFuncs.localization.WindowMainLabels[1];
            this.vm.search.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_STOP];
            //this.ButtonSearch.IsEnabled = false;
            setControlAvailability(false);
            this.ButtonWonder.IsEnabled = false;
            this.vm.results.Text = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEARCHING] + "...";

            searchUI.fullname = this.fullName.TextBox.Text.ToLower();
            searchUI.nation = this.nation.TextBox.Text.ToLower();
            searchUI.club = this.club.TextBox.Text.ToLower();
            globalFuncs.specialCharactersReplacement(ref searchUI.fullname);
            globalFuncs.specialCharactersReplacement(ref searchUI.nation);
            globalFuncs.specialCharactersReplacement(ref searchUI.club);
            searchUI.regionIndex = this.region.ComboBox.SelectedIndex;
            if (this.region.ComboBox.SelectedItem != null)
                searchUI.regionItem = globalFuncs.localization.regionsNative[searchUI.regionIndex];
            searchUI.roleIndex = this.role.ComboBox.SelectedIndex;
            if (this.role.ComboBox.SelectedItem != null)
                searchUI.role = (String)this.role.ComboBox.SelectedItem;
            searchUI.contractStatusIndex = this.contractStatus.ComboBox.SelectedIndex;
            searchUI.regenIndex = this.regen.ComboBox.SelectedIndex;
            searchUI.bestcrIndex = this.bestcr.ComboBox.SelectedIndex;
            if (searchUI.bestcrIndex > 0)
            {
                searchUI.bestcrItem = globalFuncs.localization.staffDisplayRatings[searchUI.bestcrIndex - 1];
            }
            searchUI.fitnessMin = (int)this.fitness.NumericUpDownMin.Value;
            searchUI.fitnessMax = (int)this.fitness.NumericUpDownMax.Value;
            searchUI.goalkeepersMin = (int)this.goalkeepers.NumericUpDownMin.Value;
            searchUI.goalkeepersMax = (int)this.goalkeepers.NumericUpDownMax.Value;
            searchUI.ballControlMin = (int)this.ballControl.NumericUpDownMin.Value;
            searchUI.ballControlMax = (int)this.ballControl.NumericUpDownMax.Value;
            searchUI.tacticsMin = (int)this.tactics.NumericUpDownMin.Value;
            searchUI.tacticsMax = (int)this.tactics.NumericUpDownMax.Value;
            searchUI.defendingMin = (int)this.defending.NumericUpDownMin.Value;
            searchUI.defendingMax = (int)this.defending.NumericUpDownMax.Value;
            searchUI.attackingMin = (int)this.attacking.NumericUpDownMin.Value;
            searchUI.attackingMax = (int)this.attacking.NumericUpDownMax.Value;
            searchUI.shootingMin = (int)this.shooting.NumericUpDownMin.Value;
            searchUI.shootingMax = (int)this.shooting.NumericUpDownMax.Value;
            searchUI.setPiecesMin = (int)this.setPieces.NumericUpDownMin.Value;
            searchUI.setPiecesMax = (int)this.setPieces.NumericUpDownMax.Value;

            searchUI.gameDate = context.fm.MetaData.IngameDate.Ticks;

            // init special attributes
            initSpecialAttributes(ref searchUI.numericUpDownArray);

            if (searchUI.contractStatusIndex >= 0)
            {
                if (searchUI.contractStatusIndex == 1)
                    searchUI.gameDate = context.fm.MetaData.IngameDate.AddMonths(6).Ticks;
                else if (searchUI.contractStatusIndex == 2)
                    searchUI.gameDate = context.fm.MetaData.IngameDate.AddYears(1).Ticks;
            }

            SearchDelegate d = new SearchDelegate(this.retrieveFields);
            d.BeginInvoke(null, null);
            ProgressBarDelegate p = new ProgressBarDelegate(this.updateProgressBar);
            p.BeginInvoke(null, null);
        }

        public void retrieveFields()
        {
            try
            {
                CurrentResultDelegate currentResultDelegate = new CurrentResultDelegate(currentResult);
                FinalResultDelegate finalResultDelegate = new FinalResultDelegate(finalResult);

                PreferencesSettings settings = GlobalSettings.getSettings().curPreferencesSettings;

                context.staffCRTotal = 0;

                List<string> name_substrings = new List<string>();
                bool empty_name = globalFuncs.multiEntryTextBox(ref name_substrings, searchUI.fullname);
                List<string> nationality_substrings = new List<string>();
                bool empty_nationality = globalFuncs.multiEntryTextBox(ref nationality_substrings, searchUI.nation);
                List<string> club_substrings = new List<string>();
                bool empty_club = globalFuncs.multiEntryTextBox(ref club_substrings, searchUI.club);

                STAFFCLUBSTATE staffClubState = STAFFCLUBSTATE.SCS_FREE;
                DateTime oldDate = new DateTime(1910, 01, 01);

                StaffGridViewModel newRow;
                DateTime timerStart = DateTime.Now;
                string staffName = "";
                string staffNickname = "";
                string staffNationality = "";
                string staffRole = "";
                string staffClub = "";
                double counter = 0;
                double total = 100.0 / (double)context.fm.Staff.Count();
                foreach (Staff staff in context.fm.Staff)
                {
                    progressBarValue = counter * total;
                    ++counter;
                    if (stopSearching)
                    {
                        searching = false;
                        stopSearching = false;
                        break;
                    }

                    // bugged staff
                    if (staff.Age < 13 || staff.Age > 90 || staff.Nationality == null)
                        continue;

                    // check empty name
                    if (staff.FirstName.Length == 0)
                        continue;

                    // check name
                    if (!empty_name)
                    {
                        staffName = staff.FirstName.ToLower() + " " + staff.LastName.ToLower();
                        staffNickname = staff.Nickname.ToLower();
                        globalFuncs.specialCharactersReplacement(ref staffName);
                        globalFuncs.specialCharactersReplacement(ref staffNickname);
                        int no_of_successes = 0;
                        foreach (string str in name_substrings)
                        {
                            if (staffName.Contains(str) || staffNickname.Contains(str))
                                ++no_of_successes;
                        }

                        if (no_of_successes != name_substrings.Count)
                            continue;
                    }

                    // check role
                    staffRole = "";
                    if (searchUI.roleIndex > 0)
                    {
                        if (!context.find_staff_role(staff, ref staffRole, ref searchUI.role, false))
                            continue;
                    }
                    else
                        context.find_staff_role(staff, ref staffRole, ref searchUI.role, true);

                    // check nation
                    if (!empty_nationality)
                    {
                        staffNationality = staff.Nationality.Name.ToLower();
                        globalFuncs.specialCharactersReplacement(ref staffNationality);
                        int no_of_successes = 0;
                        foreach (string str in nationality_substrings)
                        {
                            if (staffNationality.Contains(str))
                                ++no_of_successes;
                        }

                        if (no_of_successes != nationality_substrings.Count)
                            continue;
                    }

                    //check club
                    staffClubState = STAFFCLUBSTATE.SCS_FREE;
                    staffClub = "";
                    context.find_staff_club(staff, ref staffClub, ref staffClubState);

                    if (staffClub.Length > 0)
                    {
                        if (!staffClub.ToLower().Contains(searchUI.club.ToLower()))
                            continue;
                    }

                    if (!empty_club)
                    {
                        globalFuncs.specialCharactersReplacement(ref staffClub);
                        int no_of_successes = 0;
                        foreach (string str in club_substrings)
                        {
                            if (staffClub.ToLower().Contains(str))
                                ++no_of_successes;
                        }

                        if (no_of_successes != club_substrings.Count)
                            continue;
                    }

                    // check region
                    if (searchUI.regionIndex > 0 && staff.Nationality.Continent != null)
                    {
                        if (!searchUI.regionItem.Contains(staff.Nationality.Continent.Name))
                            continue;
                    }

                    // check contract status
                    if (searchUI.contractStatusIndex > 0)
                    {
                        if (searchUI.contractStatusIndex == 3)
                        {
                            if (!staffClub.Equals(globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEAGENT]))
                                continue;
                        }
                        else if (staff.Contract != null)
                        {
                            if (staff.Contract.ContractExpiryDate.CompareTo(oldDate) <= 0) continue;
                            else if (staff.Contract.ContractExpiryDate.Ticks > searchUI.gameDate) continue;
                        }
                        else
                            continue;
                    }

                    // check regen
                    if (searchUI.regenIndex == 1)
                    {
                        if (staff.ID < globalFuncs.regenMinNumber)
                            continue;
                    }
                    else if (searchUI.regenIndex == 2)
                    {
                        if (staff.ID >= globalFuncs.regenMinNumber)
                            continue;
                    }


                    // special attributes          
                    if (!(staff is HumanManager))
                    {
                        if (!testSpecialAttributes(staff, ref searchUI.numericUpDownArray))
                            continue;
                    }

                    if (!context.staffCRID.Contains(staff.ID))
                        context.calculateStaffCR(staff);

                    CoachingRatings cr = (CoachingRatings)context.staffCRID[staff.ID];

                    // check best coaching rating
                    if (searchUI.fitnessMin > cr.Fitness || searchUI.fitnessMax < cr.Fitness)
                        continue;
                    if (searchUI.goalkeepersMin > cr.Goalkeepers || searchUI.goalkeepersMax < cr.Goalkeepers)
                        continue;
                    if (searchUI.ballControlMin > cr.BallControl || searchUI.ballControlMax < cr.BallControl)
                        continue;
                    if (searchUI.tacticsMin > cr.Tactics || searchUI.tacticsMax < cr.Tactics)
                        continue;
                    if (searchUI.defendingMin > cr.Defending || searchUI.defendingMax < cr.Defending)
                        continue;
                    if (searchUI.attackingMin > cr.Attacking || searchUI.attackingMax < cr.Attacking)
                        continue;
                    if (searchUI.shootingMin > cr.Shooting || searchUI.shootingMax < cr.Shooting)
                        continue;
                    if (searchUI.setPiecesMin > cr.SetPieces || searchUI.setPiecesMax < cr.SetPieces)
                        continue;

                    // check best position
                    if (searchUI.bestcrIndex > 0)
                    {
                        if (!((CoachingRatings)context.staffCRID[staff.ID]).BestCR.Contains(searchUI.bestcrItem))
                            continue;
                    }

                    newRow = new StaffGridViewModel();
                    newRow.ClubState = staffClubState;
                    addStaffToGrid(staff, ref newRow, ref staffRole, ref staffClub);
                    //dataTable.Add(newRow);

                    this.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      currentResultDelegate, newRow);
                }

                this.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      finalResultDelegate);
            }
            catch (Exception e)
            {
                globalFuncs.logging.setErrorLog(ref e, false);
            }
        }

        public void currentResult(StaffGridViewModel newRow)
        {
            if (newRow != null) dataTable.Add(newRow);
            this.vm.results.Text = dataTable.Count + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_STAFFENTRIESFOUND] + ".";
            if (context.staffCRTotal > 0) this.vm.results.Text += " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_CALCCRFOR] +
                " " + context.staffCRTotal + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_STAFF] + ".";
            this.windowMain.vm.tabstaff.TextBlockText = globalFuncs.localization.WindowMainLabels[1] +
                " (" + dataTable.Count + ")";
        }

        public void finalResult()
        {
            currentResult(null);
            this.ButtonSearch.IsEnabled = true;
            this.vm.search.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEARCH];
            setControlAvailability(true);
            this.ButtonWonder.IsEnabled = true;
            globalFuncs.scoutTimer.stop();
            this.vm.results.Text += globalFuncs.localization.SearchingResults[ScoutLocalization.SR_QUERYTOOK] + " " + globalFuncs.scoutTimer.secondsFloat() + " " + 
                globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEC] + ".";
        }

        public void ButtonWonder_Click(object sender, RoutedEventArgs e)
        {
            globalFuncs.scoutTimer.start();

            if (searching)
            {
                stopSearching = true;
                searching = false;
                return;
            }

            this.windowMain.CurrentGameDate.Text = context.fm.MetaData.IngameDate.ToLongDateString();
            this.windowMain.vm.tabstaff.TextBlockText = globalFuncs.localization.WindowMainLabels[1];
            this.vm.wonder.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_STOP];
            //this.ButtonWonder.IsEnabled = false;
            this.vm.results.Text = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEARCHING] + "...";
            setControlAvailability(false);
            this.ButtonSearch.IsEnabled = false;

            SearchDelegate d = new SearchDelegate(this.retrieveFieldsWonder);
            d.BeginInvoke(null, null);
            ProgressBarDelegate p = new ProgressBarDelegate(this.updateProgressBar);
            p.BeginInvoke(null, null); 
        }

        public void retrieveFieldsWonder()
        {
            try
            {
                CurrentResultDelegate currentResultDelegate = new CurrentResultDelegate(currentResultWonder);
                FinalResultDelegate finalResultDelegate = new FinalResultDelegate(finalResultWonder);

                PreferencesSettings settings = GlobalSettings.getSettings().curPreferencesSettings;

                context.staffCRTotal = 0;

                STAFFCLUBSTATE staffClubState = STAFFCLUBSTATE.SCS_FREE;

                StaffGridViewModel newRow;
                DateTime timerStart = DateTime.Now;
                string staffClub = "";
                string staffRoleSelected = "";
                double counter = 0;
                double total = 100.0 / (double)context.fm.Staff.Count();
                foreach (Staff staff in context.fm.Staff)
                {
                    progressBarValue = counter * total;
                    ++counter;

                    if (stopSearching)
                    {
                        searching = false;
                        stopSearching = false;
                        break;
                    }

                    // bugged staff
                    if (staff.Age < 13 || staff.Age > 90 || staff.Nationality == null)
                        continue;

                    // check empty name
                    if (staff.FirstName.Length == 0)
                        continue;

                    // check PA
                    if (staff.PotentialCoachingAbility < settings.wonderstaffMinPA)
                        continue;

                    // check club
                    staffClubState = STAFFCLUBSTATE.SCS_FREE;
                    staffClub = "";
                    context.find_staff_club(staff, ref staffClub, ref staffClubState);

                    // check position
                    string staffRole = "";
                    context.find_staff_role(staff, ref staffRole, ref staffRoleSelected, true);

                    newRow = new StaffGridViewModel();
                    newRow.ClubState = staffClubState;
                    addStaffToGrid(staff, ref newRow, ref staffRole, ref staffClub);
                    //dataTable.Rows.Add(newRow);

                    this.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      currentResultDelegate, newRow);
                }

                this.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      finalResultDelegate);
            }
            catch (Exception e)
            {
                globalFuncs.logging.setErrorLog(ref e, false);
            }
        }

        public void currentResultWonder(StaffGridViewModel newRow)
        {
            if (newRow != null) dataTable.Add(newRow);
            this.vm.results.Text = dataTable.Count + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_WONDERSTAFFENTRIESFOUND] + ".";
            if (context.staffCRTotal > 0) this.vm.results.Text += " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_CALCCRFOR] +
                " " + context.staffCRTotal + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_STAFF] + ".";
            this.windowMain.vm.tabstaff.TextBlockText = globalFuncs.localization.WindowMainLabels[1] +
                " (" + dataTable.Count + ")";
        }

        public void finalResultWonder()
        {
            currentResultWonder(null);
            this.ButtonWonder.IsEnabled = true;
            this.vm.wonder.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_WONDERSTAFF];
            setControlAvailability(true);
            this.ButtonSearch.IsEnabled = true;
            globalFuncs.scoutTimer.stop();
            this.vm.results.Text += globalFuncs.localization.SearchingResults[ScoutLocalization.SR_QUERYTOOK] + " " + globalFuncs.scoutTimer.secondsFloat() + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEC] + ".";
        }

        public void addStaffToGrid(Staff staff, ref StaffGridViewModel newRow, ref string staffRole, ref string staffClub)
        {
            PreferencesSettings settings = GlobalSettings.getSettings().curPreferencesSettings;
            ScoutLocalization localization = globalFuncs.localization;

            newRow.ID = staff.ID;
            if (staff.Nickname.Equals(String.Empty))
                newRow.FullName = staff.FirstName + " " + staff.LastName;
            else
                newRow.FullName = staff.Nickname;
            newRow.Nation = staff.Nationality.Name;
            newRow.Club = staffClub;
            newRow.Role = staffRole;
            newRow.Age = staff.Age;
            newRow.CA = staff.CurrentCoachingAbility;
            newRow.PA = staff.PotentialCoachingAbility;
            newRow.ADiff = staff.PotentialCoachingAbility - staff.CurrentCoachingAbility;
           
            if (!context.staffCRID.Contains(staff.ID))
                context.calculateStaffCR(staff);
            CoachingRatings cr = (CoachingRatings)context.staffCRID[staff.ID];
            newRow.BestCR = cr.BestCR;
            newRow.BestCRStars = cr.BestCRStars;
            newRow.BestCRStarsImage = App.Current.Resources[cr.BestCRStars + "star"] as ImageSource;

            int staffWage = 0;
            String contractStarted = globalFuncs.localization.ProfileGenericLabels[ScoutLocalization.PG_NONE]; ;
            String contractExpiring = globalFuncs.localization.ProfileGenericLabels[ScoutLocalization.PG_NONE]; ;
            long contractStartedTicks = 0;
            long contractExpiringTicks = 0;
            if (staff.Contract != null)
            {
                contractStarted = staff.Contract.ContractStarted.Date.ToShortDateString();
                contractExpiring = staff.Contract.ContractExpiryDate.Date.ToShortDateString();
                contractStartedTicks = staff.Contract.ContractStarted.Ticks;
                contractExpiringTicks = staff.Contract.ContractExpiryDate.Ticks;
                staffWage = staff.Contract.WagePerWeek;
            }

            newRow.isFree = newRow.ClubState == STAFFCLUBSTATE.SCS_FREE;
            if (staff.Contract != null)
            {
                long ticks = context.fm.MetaData.IngameDate.AddMonths(6).Ticks;
                newRow.IsExpiring6mo = ticks < staff.Contract.ContractExpiryDate.Ticks;
                ticks = context.fm.MetaData.IngameDate.AddYears(1).Ticks;
                newRow.IsExpiring1y = ticks < staff.Contract.ContractExpiryDate.Ticks;
                ticks = context.fm.MetaData.IngameDate.AddYears(1).AddDays(1).Ticks;
                bool expiring = ticks < staff.Contract.ContractExpiryDate.Ticks;

                DateTime oldDate = new DateTime(1910, 01, 01);
                if (staff.Contract.ContractExpiryDate.CompareTo(oldDate) > 0)
                {
                    if (!expiring)
                    {
                        if (!newRow.IsExpiring6mo)
                        {
                            newRow.IsExpiring6mo = true;
                            newRow.IsExpiring1y = false;
                        }
                        else
                        {
                            newRow.IsExpiring6mo = false;
                            newRow.IsExpiring1y = true;
                        }
                    }
                    else
                    {
                        newRow.IsExpiring6mo = false;
                        newRow.IsExpiring1y = false;
                    }
                }
            }

            newRow.IsBest = cr.BestCRStars > globalFuncs.bestCRMinNumber;

            newRow.IsWonder = (staff.PotentialCoachingAbility >= settings.wonderstaffMinPA);

            newRow.statusTooltip = String.Empty;
            if (newRow.isFree) newRow.statusTooltip = localization.statusTooltips[ScoutLocalization.ST_FREE_AGENT];
            if (newRow.IsExpiring6mo)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_EXPIRING_6MO];
            }
            if (newRow.IsExpiring1y)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_EXPIRING_1Y];
            }
            if (newRow.IsBest)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_BEST_CR];
            }
            if (newRow.IsWonder)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_WONDERSTAFF];
            }

            newRow.ContractStarted = contractStarted;
            newRow.ContractStartedTicks = contractStartedTicks;
            newRow.ContractExpiring = contractExpiring;
            newRow.ContractExpiringTicks = contractExpiringTicks;
            newRow.CurrentWage = (int)(staffWage * settings.wageMultiplier.multiplier * settings.currencyMultiplier.multiplier);
            if (!(staff is HumanManager))
            {
                newRow.WorldReputation = staff.WorldReputation;
                newRow.NationalReputation = staff.HomeReputation;
                newRow.LocalReputation = staff.CurrentReputation;
                newRow.Attacking = (int)(staff.Skills.CoachingAttacking * 0.2f);
                newRow.Defending = (int)(staff.Skills.CoachingDefending * 0.2f);
                newRow.Fitness = (int)(staff.Skills.CoachingFitness * 0.2f);
                newRow.Goalkeepers = (int)(staff.Skills.CoachingGoalkeepers * 0.2f);
                newRow.Mental = (int)(staff.Skills.CoachingMental * 0.2f);
                newRow.Player = (int)(staff.Skills.CoachingPlayer * 0.2f);
                newRow.Tactical = (int)(staff.Skills.CoachingTactical * 0.2f);
                newRow.Technical = (int)(staff.Skills.CoachingTechnical * 0.2f);
                newRow.ManManagement = (int)(staff.Skills.ManManagement * 0.2f);
                newRow.WorkingWithYoungsters = staff.Skills.WorkingWithYoungsters;
                newRow.Adaptability = staff.Skills.Adaptability;
                newRow.Ambition = staff.Skills.Ambition;
                newRow.Controversy = staff.Skills.Controversy;
                newRow.Determination = (int)(staff.Skills.Determination * 0.2f);
                newRow.Loyalty = staff.Skills.Loyalty;
                newRow.Pressure = staff.Skills.Pressure;
                newRow.Professionalism = staff.Skills.Professionalism;
                newRow.Sportsmanship = staff.Skills.Sportsmanship;
                //newRow.Temperament = staff.Skills.Temperament;
                newRow.JudgingPlayerAbility = (int)(staff.Skills.JudgingPlayerAbility * 0.2f);
                newRow.JudgingPlayerPotential = (int)(staff.Skills.JudgingPlayerPotential * 0.2f);
                newRow.LevelOfDiscipline = staff.Skills.LevelOfDiscipline;
                newRow.Motivating = (int)(staff.Skills.Motivating * 0.2f);
                newRow.Physiotherapy = (int)(staff.Skills.Physiotherapy * 0.2f);
                newRow.TacticalKnowledge = (int)(staff.Skills.TacticalKnowledge * 0.2f);
                newRow.Depth = staff.Skills.Depth;
                newRow.Directness = staff.Skills.Directness;
                newRow.Flamboyancy = staff.Skills.Flamboyancy;
                newRow.Flexibility = staff.Skills.Flexibility;
                newRow.FreeRoles = staff.Skills.FreeRoles;
                newRow.Marking = staff.Skills.Marking;
                newRow.Offside = staff.Skills.OffSide;
                newRow.Pressing = staff.Skills.Pressing;
                newRow.SittingBack = staff.Skills.SittingBack;
                newRow.Tempo = staff.Skills.Tempo;
                newRow.UseOfPlaymaker = staff.Skills.UseOfPlaymaker;
                newRow.UseOfSubstitutions = staff.Skills.UseOfSubstitutions;
                newRow.Width = staff.Skills.Width;
                newRow.BuyingPlayers = staff.Skills.BuyingPlayers;
                newRow.HardnessOfTraining = staff.Skills.HardnessOfTraining;
                newRow.MindGames = staff.Skills.MindGames;
                newRow.SquadRotation = staff.Skills.SquadRotation;
                newRow.Business = staff.Skills.Business;
                newRow.Interference = staff.Skills.Interference;
                newRow.Patience = staff.Skills.Patience;
                newRow.Resources = staff.Skills.Resources;
            }
        }

        private void initSpecialAttributes(ref int[] numericUpDownArray)
        {
            int c = -1;
            numericUpDownArray[++c] = (int)this.age.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)this.age.NumericUpDownMax.Value;
            numericUpDownArray[++c] = (int)this.ca.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)this.ca.NumericUpDownMax.Value;
            numericUpDownArray[++c] = (int)this.pa.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)this.pa.NumericUpDownMax.Value;
            numericUpDownArray[++c] = ((int)((float)attributes.attacking.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.attacking.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.defending.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.defending.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.fitness.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.fitness.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.goalkeepers.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.goalkeepers.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.mental.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.mental.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.player.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.player.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.tactical.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.tactical.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.technical.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.technical.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.manmanagement.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.manmanagement.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.workingwithyoungsters.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.workingwithyoungsters.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.adaptability.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.adaptability.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.ambition.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.ambition.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.controversy.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.controversy.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.determination.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.determination.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.loyalty.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.loyalty.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.pressure.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.pressure.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.professionalism.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.professionalism.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.sportsmanship.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.sportsmanship.NumericUpDownMax.Value));
            //numericUpDownArray[++c] = ((int)((float)attributes.temperament.NumericUpDownMin.Value));
            //numericUpDownArray[++c] = ((int)((float)attributes.temperament.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.judgingplayerability.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.judgingplayerability.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.judgingplayerpotential.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.judgingplayerpotential.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.levelofdiscipline.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.levelofdiscipline.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.motivating.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.motivating.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.physiotherapy.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.physiotherapy.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.tacticalknowledge.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.tacticalknowledge.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.depth.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.depth.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.directness.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.directness.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.flamboyancy.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.flamboyancy.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.flexibility.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.flexibility.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.freeroles.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.freeroles.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.marking.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.marking.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.offside.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.offside.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.pressing.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.pressing.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.sittingback.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.sittingback.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.tempo.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.tempo.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.useofplaymaker.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.useofplaymaker.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.useofsubstitutions.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.useofsubstitutions.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.width.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.width.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.buyingplayers.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.buyingplayers.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.hardnessoftraining.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.hardnessoftraining.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.mindgames.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.mindgames.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.squadrotation.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.squadrotation.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.business.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.business.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.intereference.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.intereference.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.patience.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.patience.NumericUpDownMax.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.resources.NumericUpDownMin.Value));
            numericUpDownArray[++c] = ((int)((float)attributes.resources.NumericUpDownMax.Value));
        }

        private bool testSpecialAttributes(Staff staff, ref int[] numericUpDownArray)
        {
            int statsCounter = 0;
            if (staff.Age < numericUpDownArray[statsCounter++]
                  || staff.Age > numericUpDownArray[statsCounter++]) return false;
            if (staff.CurrentCoachingAbility < numericUpDownArray[statsCounter++]
              || staff.CurrentCoachingAbility > numericUpDownArray[statsCounter++]) return false;
            if (staff.PotentialCoachingAbility < numericUpDownArray[statsCounter++]
              || staff.PotentialCoachingAbility > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.CoachingAttacking < numericUpDownArray[statsCounter++]
              || staff.Skills.CoachingAttacking > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.CoachingDefending < numericUpDownArray[statsCounter++]
              || staff.Skills.CoachingDefending > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.CoachingFitness < numericUpDownArray[statsCounter++]
              || staff.Skills.CoachingFitness > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.CoachingGoalkeepers < numericUpDownArray[statsCounter++]
              || staff.Skills.CoachingGoalkeepers > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.CoachingMental < numericUpDownArray[statsCounter++]
              || staff.Skills.CoachingMental > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.CoachingPlayer < numericUpDownArray[statsCounter++]
              || staff.Skills.CoachingPlayer > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.CoachingTactical < numericUpDownArray[statsCounter++]
              || staff.Skills.CoachingTactical > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.CoachingTechnical < numericUpDownArray[statsCounter++]
              || staff.Skills.CoachingTechnical > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.ManManagement < numericUpDownArray[statsCounter++]
              || staff.Skills.ManManagement > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.WorkingWithYoungsters < numericUpDownArray[statsCounter++]
              || staff.Skills.WorkingWithYoungsters > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Adaptability < numericUpDownArray[statsCounter++]
              || staff.Skills.Adaptability > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Ambition < numericUpDownArray[statsCounter++]
              || staff.Skills.Ambition > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Controversy < numericUpDownArray[statsCounter++]
              || staff.Skills.Controversy > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Determination < numericUpDownArray[statsCounter++]
              || staff.Skills.Determination > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Loyalty < numericUpDownArray[statsCounter++]
              || staff.Skills.Loyalty > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Pressure < numericUpDownArray[statsCounter++]
              || staff.Skills.Pressure > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Professionalism < numericUpDownArray[statsCounter++]
              || staff.Skills.Professionalism > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Sportsmanship < numericUpDownArray[statsCounter++]
              || staff.Skills.Sportsmanship > numericUpDownArray[statsCounter++]) return false;
            //if (staff.Skills.Temperament < numericUpDownArray[statsCounter++]
            //  || staff.Skills.Temperament > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.JudgingPlayerAbility < numericUpDownArray[statsCounter++]
              || staff.Skills.JudgingPlayerAbility > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.JudgingPlayerPotential < numericUpDownArray[statsCounter++]
              || staff.Skills.JudgingPlayerPotential > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.LevelOfDiscipline < numericUpDownArray[statsCounter++]
              || staff.Skills.LevelOfDiscipline > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Motivating < numericUpDownArray[statsCounter++]
              || staff.Skills.Motivating > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Physiotherapy < numericUpDownArray[statsCounter++]
              || staff.Skills.Physiotherapy > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.TacticalKnowledge < numericUpDownArray[statsCounter++]
              || staff.Skills.TacticalKnowledge > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Depth < numericUpDownArray[statsCounter++]
              || staff.Skills.Depth > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Directness < numericUpDownArray[statsCounter++]
              || staff.Skills.Directness > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Flamboyancy < numericUpDownArray[statsCounter++]
              || staff.Skills.Flamboyancy > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Flexibility < numericUpDownArray[statsCounter++]
              || staff.Skills.Flexibility > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.FreeRoles < numericUpDownArray[statsCounter++]
              || staff.Skills.FreeRoles > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Marking < numericUpDownArray[statsCounter++]
              || staff.Skills.Marking > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.OffSide < numericUpDownArray[statsCounter++]
              || staff.Skills.OffSide > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Pressing < numericUpDownArray[statsCounter++]
              || staff.Skills.Pressing > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.SittingBack < numericUpDownArray[statsCounter++]
              || staff.Skills.SittingBack > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Tempo < numericUpDownArray[statsCounter++]
              || staff.Skills.Tempo > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.UseOfPlaymaker < numericUpDownArray[statsCounter++]
              || staff.Skills.UseOfPlaymaker > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.UseOfSubstitutions < numericUpDownArray[statsCounter++]
              || staff.Skills.UseOfSubstitutions > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Width < numericUpDownArray[statsCounter++]
              || staff.Skills.Width > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.BuyingPlayers < numericUpDownArray[statsCounter++]
              || staff.Skills.BuyingPlayers > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.HardnessOfTraining < numericUpDownArray[statsCounter++]
              || staff.Skills.HardnessOfTraining > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.MindGames < numericUpDownArray[statsCounter++]
              || staff.Skills.MindGames > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.SquadRotation < numericUpDownArray[statsCounter++]
              || staff.Skills.SquadRotation > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Business < numericUpDownArray[statsCounter++]
              || staff.Skills.Business > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Interference < numericUpDownArray[statsCounter++]
              || staff.Skills.Interference > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Patience < numericUpDownArray[statsCounter++]
              || staff.Skills.Patience > numericUpDownArray[statsCounter++]) return false;
            if (staff.Skills.Resources < numericUpDownArray[statsCounter++]
              || staff.Skills.Resources > numericUpDownArray[statsCounter++]) return false;
            return true;
        }

        public void setDataContext()
        {
            LabeledTextBoxContext fullName = new LabeledTextBoxContext();
            LabeledTextBoxContext nation = new LabeledTextBoxContext();
            LabeledTextBoxContext club = new LabeledTextBoxContext();
            LabeledComboBoxContext role = new LabeledComboBoxContext();
            LabeledComboBoxContext region = new LabeledComboBoxContext();
            LabeledNumericMinMaxContext age = new LabeledNumericMinMaxContext();
            LabeledNumericMinMaxContext ca = new LabeledNumericMinMaxContext();
            LabeledNumericMinMaxContext pa = new LabeledNumericMinMaxContext();
            LabeledComboBoxContext contractStatus = new LabeledComboBoxContext();
            List<LabeledNumericMinMaxContext> ratings = new List<LabeledNumericMinMaxContext>();
            LabeledComboBoxContext bestcr = new LabeledComboBoxContext();
            LabeledComboBoxContext regen = new LabeledComboBoxContext();
            ImageTextButtonContext search = new ImageTextButtonContext();
            ImageTextButtonContext wonder = new ImageTextButtonContext();
            
            fullName.LabelWidth = 60;
            fullName.TextBoxWidth = 110;

            nation.LabelWidth = 60;
            nation.TextBoxWidth = 110;

            club.LabelWidth = 60;
            club.TextBoxWidth = 110;

            role.LabelWidth = 60;
            role.ComboBoxWidth = 110;

            region.LabelWidth = 60;
            region.ComboBoxWidth = 110;

            age.LabelWidth = 90;
            age.Maximum = 100;
            age.NumericUpDownMinMaxWidth = 54;

            ca.LabelWidth = 90;
            ca.Maximum = 200;
            ca.NumericUpDownMinMaxWidth = 54;

            pa.LabelWidth = 90;
            pa.Maximum = 200;
            pa.NumericUpDownMinMaxWidth = 54;

            contractStatus.LabelWidth = 90;
            contractStatus.ComboBoxWidth = 124;

            for (int i = 1; i < globalFuncs.localization.bestcrs.Count; ++i)
            {
                ratings.Add(new LabeledNumericMinMaxContext());
                LabeledNumericMinMaxContext rating = ratings[i - 1];
                rating.LabelWidth = 70;
                rating.Minimum = 1;
                rating.Maximum = 10;
                rating.NumericUpDownMinMaxWidth = 40;
            }

            bestcr.LabelWidth = 75;
            bestcr.ComboBoxWidth = 110;

            regen.LabelWidth = 75;
            regen.LabelAlignment = HorizontalAlignment.Center;
            regen.ComboBoxWidth = 60;
            regen.ComboBoxAlignment = HorizontalAlignment.Center;

            search.ImageSource = TryFindResource("search") as ImageSource;
            search.ImageStretch = Stretch.Uniform;
            search.ImageHeight = 12;
            search.ImageWidth = 12;
            search.ImageMargin = new Thickness(0, 2, 0, 0);
            wonder.ImageSource = TryFindResource("awards") as ImageSource;
            wonder.ImageStretch = Stretch.Uniform;
            wonder.ImageHeight = 16;
            wonder.ImageWidth = 16;

            ScoutLocalization localization = globalFuncs.localization;
            role.ComboBoxItems = localization.staffRoles;
            region.ComboBoxItems = localization.regions;
            contractStatus.ComboBoxItems = localization.contractStatuses;
            bestcr.ComboBoxItems = localization.bestcrs;
            regen.ComboBoxItems = localization.YesNoEmpty;

            vm = new StaffSearchTabItemViewModel();
            vm.fullName = fullName;
            vm.nation = nation;
            vm.club = club;
            vm.role = role;
            vm.region = region;
            vm.age = age;
            vm.ca = ca;
            vm.pa = pa;
            vm.contractStatus = contractStatus;
            vm.fitness = ratings[0];
            vm.goalkeepers = ratings[1];
            vm.tactics = ratings[2];
            vm.ballControl = ratings[3];
            vm.defending = ratings[4];
            vm.attacking = ratings[5];
            vm.shooting = ratings[6];
            vm.setPieces = ratings[7];
            vm.bestcr = bestcr;
            vm.regen = regen;
            vm.search = search;
            vm.wonder = wonder;
            vm.groupboxsearch = new LabeledHeaderContext();
            vm.groupboxresults = new LabeledHeaderContext();
            vm.customizecolumns = new LabeledHeaderContext();
            vm.results = new TextBlockContext();

            setControlValues();
            setLocalization();

            this.DataContext = vm;
        }

        public void setControlValues()
        {
            vm.fullName.TextBoxText = "";
            vm.nation.TextBoxText = "";
            vm.club.TextBoxText = "";

            vm.contractStatus.ComboBoxSelectedIndex = -1;
            vm.regen.ComboBoxSelectedIndex = -1; 
            vm.role.ComboBoxSelectedIndex = -1; 
            vm.region.ComboBoxSelectedIndex = -1;
            vm.bestcr.ComboBoxSelectedIndex = -1;

            vm.age.ValueMin = 0;
            vm.age.ValueMax = 100;
            this.age.NumericUpDownMin.Value = vm.age.ValueMin;
            this.age.NumericUpDownMax.Value = vm.age.ValueMax;

            vm.ca.ValueMin = 0;
            vm.ca.ValueMax = 200;
            this.ca.NumericUpDownMin.Value = vm.ca.ValueMin;
            this.ca.NumericUpDownMax.Value = vm.ca.ValueMax;

            vm.pa.ValueMin = 0;
            vm.pa.ValueMax = 200;
            this.pa.NumericUpDownMin.Value = vm.pa.ValueMin;
            this.pa.NumericUpDownMax.Value = vm.pa.ValueMax;

            vm.fitness.ValueMin = 1;
            vm.fitness.ValueMax = 10;
            this.fitness.NumericUpDownMin.Value = vm.fitness.ValueMin;
            this.fitness.NumericUpDownMax.Value = vm.fitness.ValueMax;

            vm.goalkeepers.ValueMin = 1;
            vm.goalkeepers.ValueMax = 10;
            this.goalkeepers.NumericUpDownMin.Value = vm.goalkeepers.ValueMin;
            this.goalkeepers.NumericUpDownMax.Value = vm.goalkeepers.ValueMax;

            vm.tactics.ValueMin = 1;
            vm.tactics.ValueMax = 10;
            this.tactics.NumericUpDownMin.Value = vm.tactics.ValueMin;
            this.tactics.NumericUpDownMax.Value = vm.tactics.ValueMax;

            vm.ballControl.ValueMin = 1;
            vm.ballControl.ValueMax = 10;
            this.ballControl.NumericUpDownMin.Value = vm.ballControl.ValueMin;
            this.ballControl.NumericUpDownMax.Value = vm.ballControl.ValueMax;

            vm.defending.ValueMin = 1;
            vm.defending.ValueMax = 10;
            this.defending.NumericUpDownMin.Value = vm.defending.ValueMin;
            this.defending.NumericUpDownMax.Value = vm.defending.ValueMax;

            vm.attacking.ValueMin = 1;
            vm.attacking.ValueMax = 10;
            this.attacking.NumericUpDownMin.Value = vm.attacking.ValueMin;
            this.attacking.NumericUpDownMax.Value = vm.attacking.ValueMax;

            vm.shooting.ValueMin = 1;
            vm.shooting.ValueMax = 10;
            this.shooting.NumericUpDownMin.Value = vm.shooting.ValueMin;
            this.shooting.NumericUpDownMax.Value = vm.shooting.ValueMax;

            vm.setPieces.ValueMin = 1;
            vm.setPieces.ValueMax = 10;
            this.setPieces.NumericUpDownMin.Value = vm.setPieces.ValueMin;
            this.setPieces.NumericUpDownMax.Value = vm.setPieces.ValueMax;

            this.vm.results.Text = "";
        }

        public void setLocalization()
        {
            ScoutLocalization localization = globalFuncs.localization;
            ObservableCollection<String> GeneralSearchLabels = globalFuncs.localization.GeneralSearchLabels;
            int index = -1;
            vm.groupboxsearch.Header = GeneralSearchLabels[++index];
            vm.groupboxresults.Header = GeneralSearchLabels[++index];
            vm.customizecolumns.Header = GeneralSearchLabels[++index]; 
            
            ObservableCollection<string> searchLabels = globalFuncs.localization.StaffSearchLabels;
            index = -1;

            vm.fullName.LabelContent = searchLabels[++index];
            vm.fullName.TextBoxInfoText = searchLabels[++index];
            vm.nation.LabelContent = searchLabels[++index];
            vm.nation.TextBoxInfoText = searchLabels[++index];
            vm.club.LabelContent = searchLabels[++index];
            vm.club.TextBoxInfoText = searchLabels[++index];
            vm.role.LabelContent = searchLabels[++index];
            vm.region.LabelContent = searchLabels[++index];
            vm.age.LabelContent = searchLabels[++index];
            vm.ca.LabelContent = searchLabels[++index];
            vm.pa.LabelContent = searchLabels[++index];
            vm.contractStatus.LabelContent = searchLabels[++index];
            vm.fitness.LabelContent = localization.bestcrs[1];
            vm.goalkeepers.LabelContent = localization.bestcrs[2];
            vm.tactics.LabelContent = localization.bestcrs[3];
            vm.ballControl.LabelContent = localization.bestcrs[4];
            vm.defending.LabelContent = localization.bestcrs[5];
            vm.attacking.LabelContent = localization.bestcrs[6];
            vm.shooting.LabelContent = localization.bestcrs[7];
            vm.setPieces.LabelContent = localization.bestcrs[8];
            vm.bestcr.LabelContent = searchLabels[++index];
            vm.regen.LabelContent = searchLabels[++index];
            vm.search.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEARCH];
            vm.wonder.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_WONDERSTAFF];

            for (int i = 0; i < this.dataGrid.Columns.Count; ++i)
                this.dataGrid.Columns[i].Header = globalFuncs.localization.staffColumns[i];
        }

		public void initDataTableColumns(ref List<int> columnsWidth)
        {
            dataTable = new ObservableCollection<StaffGridViewModel>(); 

            for (int i = 0; i < columnsWidth.Count; ++i)
            {
                char[] c = { ' ' };
                Binding items = new Binding();
                String colName = globalFuncs.localization.staffColumns[i];
                String natColName = globalFuncs.localization.staffNativeColumns[i];
                String pathString = natColName.Replace(" ", "");
                pathString = pathString.Replace("%", "Stars");
                PropertyPath path = new PropertyPath(pathString);
                items.Path = path;
                String sortedPathString = pathString;
                if (natColName.Equals("Contract Started") || natColName.Equals("Contract Expiring"))
                    sortedPathString += "Ticks";

                DataGridColumn dc = null;
                if (natColName.Contains("Stars"))
                {
                    dc = new DataGridTemplateColumn()
                    {
                        Header = colName,
                        Width = columnsWidth[i],
                        SortMemberPath = sortedPathString,
                        Visibility = Visibility.Collapsed,
                        CellTemplate = App.Current.Resources["staffGridRating"] as DataTemplate
                    };
                }
                else if (natColName.Contains("Status"))
                {
                    dc = new DataGridTemplateColumn()
                    {
                        Header = colName,
                        Width = columnsWidth[i],
                        SortMemberPath = sortedPathString,
                        Visibility = Visibility.Collapsed,
                        CellTemplate = App.Current.Resources["staffStatusDisplay"] as DataTemplate
                    };
                }
                else
                {
                    dc = new DataGridTextColumn()
                    {
                        Header = colName,
                        Width = columnsWidth[i],
                        Binding = items,
                        SortMemberPath = sortedPathString,
                        Visibility = Visibility.Collapsed,
                        IsReadOnly = true
                    };
                }

                this.dataGrid.Columns.Add(dc);
            }

            List<int> settingColumns = settings.curPreferencesSettings.staffColumns;
            for (int i = 0; i < settingColumns.Count; ++i)
                this.dataGrid.Columns[settingColumns[i]].Visibility = Visibility.Visible;

            vm.dataGrid = dataTable;
            this.dataGrid.DataContext = vm.dataGrid;
		}

        public void clearData()
        {
            setControlValues();
            this.dataTable.Clear();
        }
    }
}
