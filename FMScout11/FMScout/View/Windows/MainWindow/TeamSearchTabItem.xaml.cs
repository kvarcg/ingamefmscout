using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class TeamSearchTabItem : UserControl
    {
        //public DataTable dataTable = null;
        public ObservableCollection<TeamGridViewModel> dataTable = null;
        //public List<DataColumn> dataColumnList = null;
        public TeamSearchTabItemViewModel vm = null;
        public WindowMain windowMain = null;
        private WindowProfile windowProfile = null;
        public WindowQuickColumns windowQuickColumns = null;
        private Settings settings = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;
        private SearchUITeam searchUI = null;
        private bool searching = false;
        private bool stopSearching = false;
        private bool finishedLoading = false;

        private delegate void SearchDelegate();
        private delegate void CurrentResultDelegate(TeamGridViewModel newRow);
        private delegate void FinalResultDelegate();
        private delegate void ProgressBarDelegate();
        private delegate void SetProgressBarValueDelegate();

        public TeamSearchTabItem()
        {
            this.InitializeComponent();
            this.progressBar.Opacity = 0;

            settings = GlobalSettings.getSettings();
            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs();

            searchUI = new SearchUITeam();

            this.teamtype.Visibility = Visibility.Collapsed;

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
                windowProfile = new WindowProfile(WindowProfileType.Team);
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
                List<TeamGridViewModel> rows = new List<TeamGridViewModel>();
                for (int i = 0; i < this.dataGrid.SelectedItems.Count; ++i)
                    rows.Add((TeamGridViewModel)this.dataGrid.SelectedItems[i]);

                foreach (Team team in context.fm.Teams)
                {
                    int ID = team.ID;
                    for (int i = 0; i < rows.Count; ++i)
                    {
                        TeamGridViewModel r = rows[i];
                        if (ID == (int)rows[i].ID)
                        {
                            windowProfile.addToContext(team, r);
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
            List<TeamGridViewModel> rows = new List<TeamGridViewModel>();
            for (int i = 0; i < this.dataGrid.SelectedItems.Count; ++i)
                rows.Add((TeamGridViewModel)this.dataGrid.SelectedItems[i]);

            foreach (DictionaryEntry entry in globalFuncs.allClubs)
            {
                Club club = (Club)entry.Value;
                int curTeam = 0;
                //for (curTeam = 0; curTeam < club.Teams.Count; ++curTeam)
                //{
                //    if (club.Teams[curTeam].Type == TeamTypeEnum.First)
                //        break;
                //    else if (club.Teams[curTeam].Type == TeamTypeEnum.Amateur)
                //        break;
                //    //else if (club.Teams[curTeam].Type == TeamTypeEnum.Empty)
                //    //  break;
                //}

                if (curTeam == club.Teams.Count) --curTeam;

                TeamGridViewModel r = new TeamGridViewModel();
                this.addTeamToGrid(club, ref r, curTeam);
                windowProfile.addToContext(club.Teams[curTeam], r);
                break;
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
            this.GroupBoxTeamsSearch1.IsEnabled = res;
            this.GroupBoxTeamsSearch2.IsEnabled = res;
            this.GroupBoxTeamsSearch3.IsEnabled = res;
            this.windowMain.MenuItemClearTeamFields.IsEnabled = res;
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
            this.windowMain.vm.tabteams.TextBlockText = globalFuncs.localization.WindowMainLabels[2];
            this.vm.search.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_STOP];
            //this.ButtonSearch.IsEnabled = false;
            setControlAvailability(false);
            this.ButtonWonder.IsEnabled = false;
            this.vm.results.Text = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEARCHING] + "...";

            searchUI.name = this.name.TextBox.Text.ToLower();
            searchUI.nation = this.nation.TextBox.Text.ToLower();
            searchUI.stadium = this.stadium.TextBox.Text.ToLower();
            globalFuncs.specialCharactersReplacement(ref searchUI.name);
            globalFuncs.specialCharactersReplacement(ref searchUI.nation);
            globalFuncs.specialCharactersReplacement(ref searchUI.stadium); 
            searchUI.teamtypeIndex = this.teamtype.ComboBox.SelectedIndex;
            if (this.teamtype.ComboBox.SelectedItem != null)
                searchUI.teamtypeItem = (String)this.teamtype.ComboBox.SelectedItem;
            searchUI.regionIndex = this.region.ComboBox.SelectedIndex;
            if (this.region.ComboBox.SelectedItem != null)
                searchUI.regionItem = globalFuncs.localization.regionsNative[searchUI.regionIndex];
            searchUI.reputationIndex = this.reputation.ComboBox.SelectedIndex;
            if (this.reputation.ComboBox.SelectedItem != null)
                searchUI.reputationItem = (String)this.reputation.ComboBox.SelectedItem;
            searchUI.transferBudgetMin = (int)this.transferBudget.NumericUpDownMin.Value;
            searchUI.transferBudgetMax = (int)this.transferBudget.NumericUpDownMax.Value;
            searchUI.wageBudgetMin = (int)this.wageBudget.NumericUpDownMin.Value;
            searchUI.wageBudgetMax = (int)this.wageBudget.NumericUpDownMax.Value;
            
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

                List<string> name_substrings = new List<string>();
                bool empty_name = globalFuncs.multiEntryTextBox(ref name_substrings, searchUI.name);
                List<string> nationality_substrings = new List<string>();
                bool empty_nationality = globalFuncs.multiEntryTextBox(ref nationality_substrings, searchUI.nation);
                List<string> stadium_substrings = new List<string>();
                bool empty_stadium = globalFuncs.multiEntryTextBox(ref stadium_substrings, searchUI.stadium);

                TEAMSTATE teamState = TEAMSTATE.TS_NONNATIONAL;

                TeamGridViewModel newRow;
                DateTime timerStart = DateTime.Now;
                Club club = null;
                string clubName = "";
                string clubNationality = "";
                string clubStadium = "";
                double counter = 0;
                double total = 100.0 / (double)globalFuncs.allClubs.Count;
                foreach (DictionaryEntry entry in globalFuncs.allClubs)
                {
                    progressBarValue = counter * total;
                    ++counter;
                    if (stopSearching)
                    {
                        searching = false;
                        stopSearching = false;
                        break;
                    }

                    club = (Club)entry.Value;
                    // check empty name
                    if (club.Name.Length == 0 && club.Country != null)
                        System.Diagnostics.Debug.WriteLine(club.Name);
                    if (club.Name.Length == 0 && club.Country != null)
                        continue;

                    // check name
                    if (!empty_name)
                    {
                        clubName = club.Name.ToLower();
                        globalFuncs.specialCharactersReplacement(ref clubName);
                        int no_of_successes = 0;
                        foreach (string str in name_substrings)
                        {
                            if (clubName.Contains(str))
                                ++no_of_successes;
                        }

                        if (no_of_successes != name_substrings.Count)
                            continue;
                    }

                    teamState = TEAMSTATE.TS_NONNATIONAL;
                    if (club.Country != null)
                    {
                        if (globalFuncs.localization.regionsNative.Contains(club.Country.Name))
                        {
                            teamState = TEAMSTATE.TS_NATIONAL;
                        }
                    }

                    // check nationality
                    if (!empty_nationality)
                    {
                        if (club.Country != null)
                        {
                            if (teamState == TEAMSTATE.TS_NONNATIONAL)
                                clubNationality = club.Country.Name.ToLower();
                            else
                                clubNationality = club.Name.ToLower();

                            int no_of_successes = 0;
                            foreach (string str in nationality_substrings)
                            {
                                if (clubNationality.Contains(str))
                                    ++no_of_successes;
                            }

                            if (no_of_successes != nationality_substrings.Count)
                                continue;

                            if (teamState == TEAMSTATE.TS_NATIONAL)
                                clubNationality = club.Country.Name.ToLower();
                        }
                        else
                            globalFuncs.logging.update("Error: Not added " + club.Name + ". Reason: Country null");
                    }

                    // check type
                    if (searchUI.teamtypeIndex > 0)
                    {
                        if (searchUI.teamtypeIndex == 1 && teamState == TEAMSTATE.TS_NONNATIONAL)
                            continue;
                        else if (searchUI.teamtypeIndex == 2 && teamState == TEAMSTATE.TS_NATIONAL)
                            continue;
                    }

                    int curTeam = 0;
                    //for (curTeam = 0; curTeam < club.Teams.Count; ++curTeam)
                    //{
                    //    if (club.Teams[curTeam].Type == TeamTypeEnum.First)
                    //        break;
                    //    else if (club.Teams[curTeam].Type == TeamTypeEnum.Amateur)
                    //        break;
                    //    //else if (club.Teams[curTeam].Type == TeamTypeEnum.Empty)
                    //    //  break;
                    //}

                    if (curTeam == club.Teams.Count) --curTeam;
                    if (curTeam < 0) continue;

                    // check reputation
                    if (searchUI.reputationIndex > 0)
                    {
                        if (searchUI.reputationIndex == 1 && club.Teams[curTeam].Reputation < 8000)
                            continue;
                        else if (searchUI.reputationIndex == 2 && (!(club.Teams[curTeam].Reputation > 5000 && club.Teams[curTeam].Reputation <= 8000)))
                            continue;
                        else if (searchUI.reputationIndex == 3 && (!(club.Teams[curTeam].Reputation > 3000 && club.Teams[curTeam].Reputation <= 5000)))
                            continue;
                        else if (searchUI.reputationIndex == 4 && (!(club.Teams[curTeam].Reputation > 0 && club.Teams[curTeam].Reputation <= 3000)))
                            continue;
                    }

                    // check region
                    if (searchUI.regionIndex > 0 && club.Country != null)
                    {
                        if (teamState == TEAMSTATE.TS_NONNATIONAL)
                        {
                            if (!searchUI.regionItem.Contains(club.Country.Continent.Name))
                                continue;
                        }
                        else
                        {
                            if (!searchUI.regionItem.Contains(club.Country.Name))
                                continue;
                        }
                    }

                    // check stadium
                    if (!empty_stadium)
                    {
                        if (club.Teams[curTeam].Stadium != null)
                        {
                            clubStadium = club.Teams[curTeam].Stadium.Name.ToLower();
                            globalFuncs.specialCharactersReplacement(ref clubStadium);
                            int no_of_successes = 0;
                            foreach (string str in stadium_substrings)
                            {
                                if (clubStadium.Contains(str))
                                    ++no_of_successes;
                            }

                            if (no_of_successes != stadium_substrings.Count)
                                continue;
                        }
                        else
                        {
                            globalFuncs.logging.update("Error: No stadium found for " + club.Name);
                            continue;
                        }
                    }

                    // check transfer budget
                    if (searchUI.transferBudgetMin > 0)
                    {
                        if (club.Finances.SeasonTransferBudget < searchUI.transferBudgetMin
                            || club.Finances.SeasonTransferBudget > searchUI.transferBudgetMax)
                            continue;
                    }
                    else
                    {
                        if (club.Finances.SeasonTransferBudget > searchUI.transferBudgetMax)
                            continue;
                    }

                    if (searchUI.transferBudgetMin > 0)
                    {
                        if (club.Finances.WageBudget < searchUI.wageBudgetMin
                        || club.Finances.WageBudget > searchUI.wageBudgetMax)
                            continue;
                    }
                    else
                    {
                        if (club.Finances.WageBudget < searchUI.wageBudgetMin)
                            continue;
                    }

                    newRow = new TeamGridViewModel();
                    newRow.TeamState = teamState;
                    addTeamToGrid(club, ref newRow, curTeam);
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

        public void currentResult(TeamGridViewModel newRow)
        {
            if (newRow != null) dataTable.Add(newRow);
            this.vm.results.Text = dataTable.Count + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_TEAMENTRIESFOUND] + ".";
            this.windowMain.vm.tabteams.TextBlockText = globalFuncs.localization.WindowMainLabels[2] +
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
            this.vm.results.Text += globalFuncs.localization.SearchingResults[ScoutLocalization.SR_QUERYTOOK] + " " + globalFuncs.scoutTimer.secondsFloat() + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEC] + ".";
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
            this.windowMain.vm.tabteams.TextBlockText = globalFuncs.localization.WindowMainLabels[2];
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

                TEAMSTATE teamState = TEAMSTATE.TS_NONNATIONAL;

                TeamGridViewModel newRow;
                DateTime timerStart = DateTime.Now;
                double counter = 0;
                double total = 100.0 / (double)globalFuncs.allClubs.Count;
                foreach (DictionaryEntry entry in globalFuncs.allClubs)
                {
                    progressBarValue = counter * total;
                    ++counter;

                    if (stopSearching)
                    {
                        searching = false;
                        stopSearching = false;
                        break;
                    }

                    Club club = (Club)entry.Value;
                    // check empty name
                    if (club.Name.Length == 0)
                        continue;

                    int curTeam = 0;
                    //for (curTeam = 0; curTeam < club.Teams.Count; ++curTeam)
                    //{
                    //    if (club.Teams[curTeam].Type == TeamTypeEnum.First)
                    //        break;
                    //    else if (club.Teams[curTeam].Type == TeamTypeEnum.Amateur)
                    //        break;
                    //    //else if (club.Teams[curTeam].Type == TeamTypeEnum.Empty)
                    //    //  break;
                    //}

                    if (curTeam == club.Teams.Count) --curTeam;

                    teamState = TEAMSTATE.TS_NONNATIONAL;
                    if (club.Country != null)
                    {
                        if (globalFuncs.localization.regionsNative.Contains(club.Country.Name))
                            teamState = TEAMSTATE.TS_NATIONAL;
                    }

                    // check reputation
                    if (club.Teams[curTeam].Reputation <= settings.wonderteamsMinRep)
                        continue;

                    newRow = new TeamGridViewModel();
                    newRow.TeamState = teamState;
                    addTeamToGrid(club, ref newRow, curTeam);
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

        public void currentResultWonder(TeamGridViewModel newRow)
        {
            if (newRow != null) dataTable.Add(newRow);
            this.vm.results.Text = dataTable.Count + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_WONDERTEAMENTRIESFOUND] + ".";
            this.windowMain.vm.tabteams.TextBlockText = globalFuncs.localization.WindowMainLabels[2] +
                " (" + dataTable.Count + ")";
        }

        public void finalResultWonder()
        {
            currentResultWonder(null);
            this.ButtonWonder.IsEnabled = true;
            this.vm.wonder.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_WONDERTEAMS];
            setControlAvailability(true);
            this.ButtonSearch.IsEnabled = true;
            globalFuncs.scoutTimer.stop();
            this.vm.results.Text += globalFuncs.localization.SearchingResults[ScoutLocalization.SR_QUERYTOOK] + " " + globalFuncs.scoutTimer.secondsFloat() + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEC] + ".";
        }

        public void addTeamToGrid(Club club, ref TeamGridViewModel newRow, int curTeam)
        {
            PreferencesSettings settings = GlobalSettings.getSettings().curPreferencesSettings;

            newRow.ID = club.ID;
            newRow.Name = club.Name.ToString();
            if (club.Country == null)
                newRow.Nation = club.Name;
            else
                newRow.Nation = club.Country.Name;
            newRow.Reputation = club.Teams[curTeam].Reputation;
            newRow.Status = club.ClubStatus.ToString();
            if (club.Teams[curTeam].Stadium != null)
            {
                newRow.Stadium = club.Teams[curTeam].Stadium.Name;
                newRow.Decay = 0;// club.Teams[curTeam].Stadium.Decay;
                newRow.FieldWidth = club.Teams[curTeam].Stadium.FieldWidth;
                newRow.FieldLength = club.Teams[curTeam].Stadium.FieldLength;
                newRow.CurrentCapacity = club.Teams[curTeam].Stadium.StadiumCapacity;
                newRow.SeatingCapacity = club.Teams[curTeam].Stadium.SeatingCapacity;
                newRow.ExpansionCapacity = club.Teams[curTeam].Stadium.ExpansionCapacity;
                newRow.UsedCapacity = club.Teams[curTeam].Stadium.UsedCapacity;
            }
            else
            {
                newRow.Stadium = " None";
                newRow.Decay = 0;
                newRow.FieldWidth = 0;
                newRow.FieldLength = 0;
                newRow.CurrentCapacity = 0;
                newRow.SeatingCapacity = 0;
                newRow.ExpansionCapacity = 0;
                newRow.UsedCapacity = 0;
            }

            if (club.Finances.RemainingTransferBudget != 29476012)
            {
                newRow.TransferBudget = (int)(club.Finances.SeasonTransferBudget * settings.currencyMultiplier.multiplier);
                newRow.RemainingBudget = (int)(club.Finances.RemainingTransferBudget * settings.currencyMultiplier.multiplier);
                newRow.WageBudget = (int)(club.Finances.WageBudget * settings.currencyMultiplier.multiplier);
                newRow.WageUsed = (int)(club.Finances.UsedWage * settings.currencyMultiplier.multiplier);
            }
            else
            {
                newRow.TransferBudget = 0;
                newRow.RemainingBudget = 0;
                newRow.WageBudget = 0;
                newRow.WageUsed = 0;
            }
            newRow.BudgetBalance = (int)(club.Finances.Balance * settings.currencyMultiplier.multiplier);
            newRow.TransferRevenueAvailable = (int)(club.Finances.TransferRevenueMadeAvailable * 0.01f);
            newRow.CurrentAffiliatedClubs = club.TeamNumber;
            newRow.MaxAffiliatedClubs = club.MaxAffiliatedClubNumber;
            newRow.TrainingFacilities = club.TrainingFacilities;
            newRow.YouthFacilities = club.YouthFacilities;
            newRow.MaximumAttendance = club.MaximumAttendance;
            newRow.AverageAttendance = club.AverageAttendance;
            newRow.MinimumAttendance = club.MinimumAttendance;
        }

        public void setDataContext()
        {
            LabeledTextBoxContext name = new LabeledTextBoxContext();
            LabeledTextBoxContext nation = new LabeledTextBoxContext();
            LabeledTextBoxContext stadium = new LabeledTextBoxContext();
            LabeledComboBoxContext teamtype = new LabeledComboBoxContext();
            LabeledComboBoxContext reputation = new LabeledComboBoxContext();
            LabeledComboBoxContext region = new LabeledComboBoxContext();
            LabeledNumericMinMaxContext transferBudget = new LabeledNumericMinMaxContext();
            LabeledNumericMinMaxContext wageBudget = new LabeledNumericMinMaxContext();
            ImageTextButtonContext search = new ImageTextButtonContext();
            ImageTextButtonContext wonder = new ImageTextButtonContext();

            name.LabelWidth = 60;
            name.TextBoxWidth = 110;

            nation.LabelWidth = 60;
            nation.TextBoxWidth = 110;

            stadium.LabelWidth = 60;
            stadium.TextBoxWidth = 110;

            teamtype.LabelWidth = 70;
            teamtype.ComboBoxWidth = 100;

            reputation.LabelWidth = 70;
            reputation.ComboBoxWidth = 100;

            region.LabelWidth = 70;
            region.ComboBoxWidth = 100;

            transferBudget.LabelWidth = 100;
            transferBudget.NumericUpDownMinMaxWidth = 82;
            transferBudget.Maximum = 200000000;

            wageBudget.LabelWidth = 100;
            wageBudget.Maximum = 200000000;
            wageBudget.NumericUpDownMinMaxWidth = 82;

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
            teamtype.ComboBoxItems = localization.teamtypes;
            region.ComboBoxItems = localization.regions;
            reputation.ComboBoxItems = localization.reputations;

            vm = new TeamSearchTabItemViewModel();
            vm.name = name;
            vm.nation = nation;
            vm.stadium = stadium;
            vm.teamtype = teamtype;
            vm.reputation = reputation;
            vm.region = region;
            vm.transferBudget = transferBudget;
            vm.wageBudget = wageBudget;
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
            vm.name.TextBoxText = "";
            vm.nation.TextBoxText = "";
            vm.stadium.TextBoxText = "";

            vm.teamtype.ComboBoxSelectedIndex = -1;
            vm.reputation.ComboBoxSelectedIndex = -1;
            vm.region.ComboBoxSelectedIndex = -1;

            vm.transferBudget.ValueMin = 0;
            vm.transferBudget.ValueMax = 200000000;
            this.transferBudget.NumericUpDownMin.Value = vm.transferBudget.ValueMin;
            this.transferBudget.NumericUpDownMax.Value = vm.transferBudget.ValueMax;

            vm.wageBudget.ValueMin = 0;
            vm.wageBudget.ValueMax = 200000000;
            this.wageBudget.NumericUpDownMin.Value = vm.wageBudget.ValueMin;
            this.wageBudget.NumericUpDownMax.Value = vm.wageBudget.ValueMax;

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

            ObservableCollection<string> searchLabels = globalFuncs.localization.TeamSearchLabels;
            index = -1;

            vm.name.LabelContent = searchLabels[++index];
            vm.name.TextBoxInfoText = searchLabels[++index];
            vm.nation.LabelContent = searchLabels[++index];
            vm.nation.TextBoxInfoText = searchLabels[++index];
            vm.stadium.LabelContent = searchLabels[++index];
            vm.stadium.TextBoxInfoText = searchLabels[++index];
            vm.teamtype.LabelContent = searchLabels[++index];
            vm.reputation.LabelContent = searchLabels[++index];
            vm.region.LabelContent = searchLabels[++index];
            vm.transferBudget.LabelContent = searchLabels[++index];
            vm.wageBudget.LabelContent = searchLabels[++index];
            vm.search.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEARCH];
            vm.wonder.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_WONDERTEAMS];

            for (int i = 0; i < this.dataGrid.Columns.Count; ++i)
                this.dataGrid.Columns[i].Header = globalFuncs.localization.teamColumns[i];
        }

        public void initDataTableColumns(ref List<int> columnsWidth)
        {
            dataTable = new ObservableCollection<TeamGridViewModel>();

            for (int i = 0; i < columnsWidth.Count; ++i)
            {
                char[] c = { ' ' };
                Binding items = new Binding();
                String colName = globalFuncs.localization.teamColumns[i];
                String natColName = globalFuncs.localization.teamNativeColumns[i];
                String pathString = natColName.Replace(" ", "");
                PropertyPath path = new PropertyPath(pathString);
                items.Path = path;
                String sortedPathString = pathString;
                if (natColName.Equals("Contract Started") || natColName.Equals("Contract Expiring"))
                    sortedPathString += "Ticks";

                DataGridColumn dc = new DataGridTextColumn()
                {
                    Header = colName,
                    Width = columnsWidth[i],
                    Binding = items,
                    SortMemberPath = sortedPathString,
                    Visibility = Visibility.Collapsed,
                    IsReadOnly = true
                };

                this.dataGrid.Columns.Add(dc);
            }

            List<int> settingColumns = settings.curPreferencesSettings.teamColumns;
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