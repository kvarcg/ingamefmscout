using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FMScout.ControlContext;
using FMScout.ViewModel;
using Young3.FMSearch.Core.Entities.InGame;
using System.Threading;

namespace FMScout.View.MainWindow
{
    public partial class PlayerSearchTabItem : UserControl
    {
        //public DataTable dataTable = null;
        public ObservableCollection<PlayerGridViewModel> dataTable = null;
        //public List<DataColumn> dataColumnList = null;
        public PlayerSearchAttributes attributes = null; 
        public PlayerSearchTabItemViewModel vm = null;        
        public WindowMain windowMain = null;
        private WindowProfile windowProfile = null;
        public WindowQuickColumns windowQuickColumns = null;
        private Settings settings = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;
        private SearchUIPlayer searchUI = null;
        private bool searching = false;
        private bool stopSearching = false;
        private bool finishedLoading = true;

        private delegate void SearchDelegate();
        private delegate void CurrentResultDelegate(PlayerGridViewModel newRow);
        private delegate void FinalResultDelegate();
        private delegate void ProgressBarDelegate();
        private delegate void SetProgressBarValueDelegate();

        public PlayerSearchTabItem()
        {
            this.InitializeComponent();
			this.progressBar.Opacity = 0;
            settings = GlobalSettings.getSettings();
            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs();
            
            searchUI = new SearchUIPlayer();

            //this.eu.IsEnabled = false;
            //this.contractStatus.IsEnabled = false;
            //this.ownership.IsEnabled = false;

            this.fullName.TextBox.TextBoxInfoMinChars = 2;
            setDataContext();
            this.ButtonSearch.Click += new RoutedEventHandler(ButtonSearch_Click);
            this.ButtonWonder.Click += new RoutedEventHandler(ButtonWonder_Click);
            this.ButtonColumns.Click += new RoutedEventHandler(ButtonColumns_Click);
            this.dataGrid.SelectionChanged += new SelectionChangedEventHandler(dataGrid_SelectionChanged);
            this.dataGrid.MouseDoubleClick += new MouseButtonEventHandler(dataGrid_MouseDoubleClick);
            this.dataGrid.MouseLeftButtonUp += new MouseButtonEventHandler(dataGrid_MouseLeftButtonUp);
            this.dataGrid.MouseRightButtonUp += new MouseButtonEventHandler(dataGrid_MouseRightButtonUp);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.dataTable.Count > 0)
                this.windowMain.MenuItemAddToShortlist.IsEnabled = true;
            else
                this.windowMain.MenuItemAddToShortlist.IsEnabled = false;
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.dataGrid.SelectedItems.Count > 0)
                createWindowProfile(-1);

            int counter = 0;
            System.Diagnostics.Debug.WriteLine("players");
            foreach (Player p in context.fm.Players)
            {
                String name = p.ToString();
                int m = p.MemoryAddress;

                System.Diagnostics.Debug.WriteLine(name + " " + m);
                ++counter;
                if (counter == 20) break;
            }
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine("staff");
            counter = 0;
            foreach (Staff p in context.fm.Staff)
            {
                String name = p.ToString();
                int m = p.MemoryAddress;

                System.Diagnostics.Debug.WriteLine(name + " " + m);
                ++counter;
                if (counter == 20) break;
            }
        }

        public void createWindowProfile(int activeObjectID)
        {
            if (windowProfile == null)
            {
                windowProfile = new WindowProfile(WindowProfileType.Player);
                windowProfile.setLocalization();
                if (activeObjectID == -1)
                    addToContext(ref this.dataGrid);
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
                    addToContext(ref this.dataGrid);
                else
                    addToContext(activeObjectID);
            }
        }

        public void addToContext(ref DataGrid localDataGrid)
        {
            if (windowProfile == null) return;
            int selectedRows = localDataGrid.SelectedItems.Count;
            int curSelectedRows = 0;
            if (selectedRows > 0)
            {
                List<PlayerGridViewModel> rows = new List<PlayerGridViewModel>();
                for (int i = 0; i < localDataGrid.SelectedItems.Count; ++i)
                    rows.Add((PlayerGridViewModel)localDataGrid.SelectedItems[i]);

                foreach (Player player in context.fm.Players)
                {
                    int ID = player.ID;
                    for (int i = 0; i < rows.Count; ++i)
                    {
                        PlayerGridViewModel r = rows[i];
                        if (ID == (int)rows[i].ID)
                        {
                            windowProfile.addToContext(player, r);
                            ++curSelectedRows;
                            break;
                        }
                    }
                    if (curSelectedRows > selectedRows) break;
                }
                windowProfile.finishedAdding();
            }
        }

        public void addToContext(int ID)
        {
            if (windowProfile == null) return;
            foreach (Player player in context.fm.Players)
            {
                if (ID == player.ID)
                {
                    String playerClub = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEPLAYER];
                    Contract contract = null;
                    PLAYERCLUBSTATE playerClubState = PLAYERCLUBSTATE.PCS_FREE;
                    context.findPlayerContractQuery(player, ref contract, ref playerClub, ref playerClubState);

                    // check position
                    string playerPosition = "";
                    List<String> player_positions = new List<String>();
                    List<String> player_sides = new List<String>();
                    context.find_player_position(player, ref playerPosition, ref player_positions, ref player_sides, true);
                    List<String> playerNationalities = new List<String>();
                    playerNationalities.Add(player.Nationality.Name);
                    // other nationalities
                    if (player.Relations != null)
                    {
                        List<PlayerRelations> relations = player.Relations.Relations;
                        for (int playerRelationIndex = 0; playerRelationIndex < player.Relations.RelationsTotal; ++playerRelationIndex)
                        {
                            if (relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                                playerNationalities.Add(relations[playerRelationIndex].Country.Name);
                        }
                    }
                    PlayerGridViewModel r = new PlayerGridViewModel();
                    this.addPlayerToGrid(player, ref r, ref playerPosition, ref playerClub, ref playerNationalities);
                    windowProfile.addToContext(player, r);
                    break;
                }
            }
            windowProfile.finishedAdding();
        }

        private void dataGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            int selectedRows = this.dataGrid.SelectedItems.Count;
            if (selectedRows > 0)
            {
                ObservableCollection<String> ProfileGenericLabels = globalFuncs.localization.ProfileGenericLabels;
                
                DataGridContextMenuViewModel cvm = new DataGridContextMenuViewModel();
                ImageTextButtonContext addshortlist = new ImageTextButtonContext();
                addshortlist.ImageSource = TryFindResource("add") as ImageSource;
                addshortlist.ImageStretch = Stretch.Uniform;
				addshortlist.ImageWidth = 12;
				addshortlist.ImageHeight = 12;
                addshortlist.TextBlockText = ProfileGenericLabels[ScoutLocalization.PG_ADDTOSHORTLIST];

                ImageTextButtonContext removeshortlist = new ImageTextButtonContext();
                removeshortlist.ImageSource = TryFindResource("remove") as ImageSource;
				removeshortlist.ImageStretch = Stretch.UniformToFill;
                removeshortlist.ImageWidth = 12;
				removeshortlist.ImageHeight = 12;
                removeshortlist.TextBlockText = ProfileGenericLabels[ScoutLocalization.PG_REMOVEFROMSHORTLIST];

                cvm.addshortlist = addshortlist;
                cvm.removeshortlist = removeshortlist;
                ContextMenu m = TryFindResource("dataGridMenu") as ContextMenu;
                m.DataContext = cvm;
                this.dataGrid.ContextMenu = m;
                ((MenuItem)m.Items[0]).Visibility = Visibility.Visible;
                ((MenuItem)m.Items[1]).Visibility = Visibility.Visible;
                bool allexist = true;
                bool allnew = true;
                for (int i = 0; i < this.dataGrid.SelectedItems.Count; ++i)
                {
                    PlayerGridViewModel p = (PlayerGridViewModel)this.dataGrid.SelectedItems[i];
                    if (context.shortlistIDList.Contains(p.ID))
                        allnew = false;
                    else
                        allexist = false;
                }
                if (allnew) ((MenuItem)m.Items[1]).Visibility = Visibility.Collapsed;
                else if (allexist) ((MenuItem)m.Items[0]).Visibility = Visibility.Collapsed;
                this.dataGrid.ContextMenuClosing += new ContextMenuEventHandler(ContextMenu_Closing);
            }
        }

        private void ContextMenu_Closing(object sender, RoutedEventArgs e)
        {
            this.dataGrid.ContextMenuClosing -= new ContextMenuEventHandler(ContextMenu_Closing);
            this.dataGrid.ContextMenu = null;
        }

        private void addShortlist_Click(object sender, RoutedEventArgs e)
        {
            int selectedRows = this.dataGrid.SelectedItems.Count;
            if (selectedRows > 0)
            {
                this.windowMain.Shortlist.addToShortlist(ref this.dataGrid);
            }
        }

        private void removeShortlist_Click(object sender, RoutedEventArgs e)
        {
            int selectedRows = this.dataGrid.SelectedItems.Count;
            if (selectedRows > 0)
            {
                List<int> playerRows = new List<int>();
                for (int i = 0; i < this.dataGrid.SelectedItems.Count; ++i)
                    playerRows.Add(((PlayerGridViewModel)this.dataGrid.SelectedItems[i]).ID);
                
                this.windowMain.Shortlist.removeFromShortlist(ref playerRows);
            }
        }

        private void dataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            addToContext(ref this.dataGrid);
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
            //System.Diagnostics.Debug.WriteLine(progressBarValue);
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
            this.GroupBoxPlayersSearch1.IsEnabled = res;
            this.GroupBoxPlayersSearch2.IsEnabled = res;
            this.GroupBoxPlayersSearch3.IsEnabled = res;
            this.windowMain.PlayerSearchAttributes.TechnicalAttributes.IsEnabled = res;
            this.windowMain.PlayerSearchAttributes.PhysicalAttributes.IsEnabled = res;
            this.windowMain.PlayerSearchAttributes.MentalAttributes.IsEnabled = res;
            this.windowMain.PlayerSearchAttributes.GoalkeepingAttributes.IsEnabled = res;
            this.windowMain.PlayerSearchAttributes.MentalTraitsAttributes.IsEnabled = res;
            this.windowMain.PlayerSearchAttributes.HiddenAttributes.IsEnabled = res;
            this.windowMain.MenuItemClearPlayerFields.IsEnabled = res;
            this.dataGrid.IsEnabled = res;
            this.searching = !res;
            if (!res)
            {
                finishedLoading = false;
                this.dataTable.Clear(); 
                this.windowMain.MenuItemAddToShortlist.IsEnabled = false;
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
            System.Diagnostics.Debug.WriteLine(this.searching);
            if (searching)
            {
                stopSearching = true;
                searching = false;
                return;
            }
            
            this.windowMain.CurrentGameDate.Text = context.fm.MetaData.IngameDate.ToLongDateString();
            this.windowMain.vm.tabplayers.TextBlockText = globalFuncs.localization.WindowMainLabels[0];
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

            List<CheckBox> positions = new List<CheckBox>()
            {
                this.GK, this.SW, this.D, this.WB, this.DM, this.M, this.AM, this.ST
            };
            List<CheckBox> sides = new List<CheckBox>()
            {
                this.L, this.R, this.C, this.Free
            };
            searchUI.player_positions.Clear();
            foreach (CheckBox player_pos in positions)
            {
                if (player_pos.IsChecked == true) searchUI.player_positions.Add((String)player_pos.Content);
            }
            searchUI.player_sides.Clear();
            foreach (CheckBox player_side in sides)
            {
                if (player_side.IsChecked == true) searchUI.player_sides.Add((String)player_side.Content);
            }

            searchUI.regionIndex = this.region.ComboBox.SelectedIndex;
            if (this.region.ComboBox.SelectedItem != null)
                searchUI.regionItem = globalFuncs.localization.regionsNative[searchUI.regionIndex];
            searchUI.ownershipIndex = this.ownership.ComboBox.SelectedIndex;
            searchUI.contractStatusIndex = this.contractStatus.ComboBox.SelectedIndex;
            searchUI.euIndex = this.eu.ComboBox.SelectedIndex;
            searchUI.regenIndex = this.regen.ComboBox.SelectedIndex;
            searchUI.prefFootIndex = this.prefFoot.ComboBox.SelectedIndex;
            searchUI.bestprIndex = this.bestpr.ComboBox.SelectedIndex;
            if (this.bestpr.ComboBox.SelectedItem != null)
                searchUI.bestprItem = (String)this.bestpr.ComboBox.SelectedItem;
            searchUI.wageMin = (int)this.wage.NumericUpDownMin.Value;
            searchUI.wageMax = (int)this.wage.NumericUpDownMax.Value;
            searchUI.prMin = (int)this.pr.NumericUpDownMin.Value;
            searchUI.prMax = (int)this.pr.NumericUpDownMax.Value;
            searchUI.prMaximum = (int)this.pr.NumericUpDownMax.Maximum;
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

                context.playersPRTotal = 0;

                List<string> name_substrings = new List<string>();
                bool empty_name = globalFuncs.multiEntryTextBox(ref name_substrings, searchUI.fullname);
                List<string> nationality_substrings = new List<string>();
                bool empty_nationality = globalFuncs.multiEntryTextBox(ref nationality_substrings, searchUI.nation);
                List<string> club_substrings = new List<string>();
                bool empty_club = globalFuncs.multiEntryTextBox(ref club_substrings, searchUI.club);
                bool empty_position = false;
                if (searchUI.player_positions.Count == 0 && searchUI.player_sides.Count == 0) empty_position = true;

                PLAYERCLUBSTATE playerClubState = PLAYERCLUBSTATE.PCS_FREE;
                PLAYEREUSTATE playerEUState = PLAYEREUSTATE.PES_NONEU;
                DateTime oldDate = new DateTime(1910, 01, 01);

                PlayerGridViewModel newRow;
                DateTime timerStart = DateTime.Now;
                string playerName = "";
                string playerNickname = "";
                List<String> playerNationalities = new List<String>();
                string playerClub = "";
                Contract contract = null;
                double counter = 0;
                double total = 100.0 / (double)context.fm.Players.Count();
                //try
                //{

                foreach (Player player in context.fm.Players)
                {
                    progressBarValue = counter * total;
                    ++counter;
                    if (stopSearching)
                    {
                        searching = false;
                        stopSearching = false;
                        break;
                    }

                    // bugged players
                    if (player.Age < 13 || player.Age > 90 || player.Nationality == null)
                        continue;

                    // check empty name
                    if (player.FirstName.Length == 0)
                        continue;

                    // check name
                    if (!empty_name)
                    {
                        playerName = player.FirstName.ToLower() + " " + player.LastName.ToLower();
                        playerNickname = player.Nickname.ToLower();
                        globalFuncs.specialCharactersReplacement(ref playerName);
                        globalFuncs.specialCharactersReplacement(ref playerNickname);
                        int no_of_successes = 0;
                        foreach (string str in name_substrings)
                        {
                            if (playerName.Contains(str) || playerNickname.Contains(str))
                                ++no_of_successes;
                        }

                        if (no_of_successes != name_substrings.Count)
                            continue;
                    }

                    // check position
                    string playerPosition = "";
                    if (!empty_position)
                    {
                        if (!context.find_player_position(player, ref playerPosition, ref searchUI.player_positions, ref searchUI.player_sides, false))
                            continue;
                    }
                    else
                        context.find_player_position(player, ref playerPosition, ref searchUI.player_positions, ref searchUI.player_sides, true);

                    // check nation
                    playerNationalities.Clear();
                    playerNationalities.Add(player.Nationality.Name);
                    // other nationalities
                    if (player.Relations != null)
                    {
                        List<PlayerRelations> relations = player.Relations.Relations;
                        for (int playerRelationIndex = 0; playerRelationIndex < player.Relations.RelationsTotal; ++playerRelationIndex)
                        {
                            if (relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                                playerNationalities.Add(relations[playerRelationIndex].Country.Name);
                        }
                    }

                    if (!empty_nationality)
                    {
                        int no_of_successes = 0;
                        for (int i = 0; i < playerNationalities.Count; ++i)
                        {
                            String playerNationality = playerNationalities[i].ToLower();
                            globalFuncs.specialCharactersReplacement(ref playerNationality);
                            foreach (string str in nationality_substrings)
                            {
                                if (playerNationality.Contains(str))
                                    ++no_of_successes;
                            }
                        }
                        if (no_of_successes != nationality_substrings.Count)
                            continue;
                    }

                    // check club
                    if (searchUI.ownershipIndex > 0)
                    {
                        if (searchUI.ownershipIndex == 1)
                        {
                            if (player.ContractSecond == null)
                                continue;
                            else if (player.ContractSecond != null && player.ContractSecond.Club != null)
                            {
                                if (player.ContractSecond.ContractTypeSecond != ContractTypeSecond.Loan)
                                    continue;
                            }
                        }
                        else if (searchUI.ownershipIndex == 2)
                        {
                            if (player.ContractSecond == null)
                                continue;
                            else if (player.ContractSecond != null && player.ContractSecond.Club != null)
                            {
                                if (player.ContractSecond.ContractTypeSecond != ContractTypeSecond.Coowned)
                                    continue;
                            }
                        }
                    }

                    playerClub = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEPLAYER];
                    playerClubState = PLAYERCLUBSTATE.PCS_FREE;
                    context.findPlayerContractQuery(player, ref contract, ref playerClub, ref playerClubState);

                    if (!empty_club)
                    {
                        globalFuncs.specialCharactersReplacement(ref playerClub);
                        int no_of_successes = 0;
                        foreach (string str in club_substrings)
                        {
                            if (playerClub.ToLower().Contains(str))
                                ++no_of_successes;
                        }

                        if (no_of_successes != club_substrings.Count)
                            continue;
                    }

                    // check contract status
                    if (searchUI.contractStatusIndex > 0)
                    {
                        if (searchUI.contractStatusIndex == 3)
                        {
                            if (!playerClub.Equals(globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEPLAYER]))
                                continue;
                        }
                        else if (contract != null)
                        {
                            if (contract.ContractExpiryDate.CompareTo(oldDate) <= 0) continue;
                            else if (contract.ContractExpiryDate.Ticks > searchUI.gameDate) continue;
                        }
                        else
                            continue;
                    }

                    // check region
                    if (searchUI.regionIndex > 0)
                    {
                        if (!searchUI.regionItem.Contains(player.Nationality.Continent.Name))
                            continue;
                    }

                    // check preferred foot
                    if (searchUI.prefFootIndex > 0)
                    {
                        if (searchUI.prefFootIndex == 1)
                        {
                            if (player.Skills.RightFoot <= 75)
                                continue;
                        }
                        else if (searchUI.prefFootIndex == 2)
                        {
                            if (player.Skills.LeftFoot <= 75)
                                continue;
                        }
                    }

                    // check regen
                    if (searchUI.regenIndex == 1)
                    {
                        if (player.ID < globalFuncs.regenMinNumber)
                            continue;
                    }
                    else if (searchUI.regenIndex == 2)
                    {
                        if (player.ID >= globalFuncs.regenMinNumber)
                            continue;
                    }

                    // check EU
                    playerEUState = PLAYEREUSTATE.PES_NONEU;
                    for (int i = 0; i < playerNationalities.Count; ++i)
                    {
                        if (globalFuncs.EUcountries.Contains(playerNationalities[i]))
                        {
                            playerEUState = PLAYEREUSTATE.PES_EU;
                            break;
                        }
                    }

                    if (searchUI.euIndex == 1)
                    {
                        if (playerEUState == PLAYEREUSTATE.PES_NONEU) continue;
                    }
                    else if (searchUI.euIndex == 2)
                    {
                        if (playerEUState == PLAYEREUSTATE.PES_EU) continue;
                    }

                    // wages
                    if (!playerClub.Equals(globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEPLAYER]))
                    {
                        if ((float)searchUI.wageMin >= contract.WagePerWeek * settings.wageMultiplier.multiplier ||
                            (float)searchUI.wageMax <= contract.WagePerWeek * settings.wageMultiplier.multiplier)
                            continue;
                    }

                    // special attributes
                    if (!testSpecialAttributes(player, ref searchUI.numericUpDownArray))
                        continue;

                    // check best positional rating
                    if (searchUI.prMin >= 0 || searchUI.prMax <= searchUI.prMaximum)
                    {
                        if (!context.playersPRID.Contains(player.ID))
                            context.calculatePlayerPR(player);
                        if (((PositionalRatings)context.playersPRID[player.ID]).bestPosR < (float)searchUI.prMin ||
                            ((PositionalRatings)context.playersPRID[player.ID]).bestPosR > (float)searchUI.prMax)
                            continue;
                    }

                    // check best position
                    if (searchUI.bestprIndex > 0)
                    {
                        if (!((PositionalRatings)context.playersPRID[player.ID]).bestPos.Equals(searchUI.bestprItem))
                            continue;
                    }

                    newRow = new PlayerGridViewModel();
                    newRow.EUState = playerEUState;
                    newRow.ClubState = playerClubState;
                    addPlayerToGrid(player, ref newRow, ref playerPosition, ref playerClub, ref playerNationalities);

                    this.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      currentResultDelegate, newRow);
                }
                //}
                //catch (Exception e)
                //{
                //    Globals.getGlobalFuncs().logging.update(string.Format("An error occured with Msg: {0}", e.Message));
                //    Globals.getGlobalFuncs().logging.update(string.Format("Error Stack Trace: {0}", e.StackTrace));
                //    System.Diagnostics.Debug.WriteLine(string.Format("An error occured with Msg: {0}", e.Message));
                //    System.Diagnostics.Debug.WriteLine(string.Format("Error Stack Trace: {0}", e.StackTrace));
                //}

                this.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      finalResultDelegate);
            }
            catch (Exception e)
            {
                globalFuncs.logging.setErrorLog(ref e, false);
            }
        }

        public void currentResult(PlayerGridViewModel newRow)
        {
            if (newRow != null) dataTable.Add(newRow);
            this.vm.results.Text = dataTable.Count + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_PLAYERENTRIESFOUND] + ".";
            if (context.playersPRTotal > 0) this.vm.results.Text += " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_CALCPRFOR] +
                " " + context.playersPRTotal + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_PLAYERS] + ".";
            this.windowMain.vm.tabplayers.TextBlockText = globalFuncs.localization.WindowMainLabels[0] +
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
            this.windowMain.vm.tabplayers.TextBlockText = globalFuncs.localization.WindowMainLabels[0];
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
           // try
            {
                CurrentResultDelegate currentResultDelegate = new CurrentResultDelegate(currentResultWonder);
                FinalResultDelegate finalResultDelegate = new FinalResultDelegate(finalResultWonder);

                PreferencesSettings settings = GlobalSettings.getSettings().curPreferencesSettings;

                context.playersPRTotal = 0;

                List<string> player_positions = new List<string>();
                List<string> player_sides = new List<string>();

                PLAYERCLUBSTATE playerClubState = PLAYERCLUBSTATE.PCS_FREE;
                PLAYEREUSTATE playerEUState = PLAYEREUSTATE.PES_NONEU;

                PlayerGridViewModel newRow;
                DateTime timerStart = DateTime.Now;
                List<String> playerNationalities = new List<String>();
                string playerClub = "";
                Contract contract = null;
                string playerPosition = "";
                double counter = 0;
                double total = 100.0 / (double)context.fm.Players.Count();
                foreach (Player player in context.fm.Players)
                {
                    progressBarValue = counter * total;
                    ++counter;
                    if (stopSearching)
                    {
                        searching = false;
                        stopSearching = false;
                        break;
                    }

                    // bugged players
                    if (player.Age < 13 || player.Age > 90 || player.Nationality == null)
                        continue;

                    // check empty name
                    if (player.FirstName.Length == 0)
                        continue;

                    // check age
                    if (player.Age <= 0 || player.Age > settings.wonderkidsMaxAge)
                        continue;

                    // check PA
                    if (player.PotentialPlayingAbility < settings.wonderkidsMinPA)
                        continue;

                    // check club
                    playerClub = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEPLAYER];
                    contract = null;
                    playerClubState = PLAYERCLUBSTATE.PCS_FREE;
                    context.findPlayerContractQuery(player, ref contract, ref playerClub, ref playerClubState);

                    // check position
                    playerPosition = "";
                    context.find_player_position(player, ref playerPosition, ref player_positions, ref player_sides, true);

                    // check nation
                    playerNationalities.Clear();
                    playerNationalities.Add(player.Nationality.Name);
                    // other nationalities
                    if (player.Relations != null)
                    {
                        List<PlayerRelations> relations = player.Relations.Relations;
                        for (int playerRelationIndex = 0; playerRelationIndex < player.Relations.RelationsTotal; ++playerRelationIndex)
                        {
                            if (relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                                playerNationalities.Add(relations[playerRelationIndex].Country.Name);
                        }
                    }
                    // check EU
                    playerEUState = PLAYEREUSTATE.PES_NONEU;
                    for (int i = 0; i < playerNationalities.Count; ++i)
                    {
                        if (globalFuncs.EUcountries.Contains(playerNationalities[i]))
                        {
                            playerEUState = PLAYEREUSTATE.PES_EU;
                            break;
                        }
                    }

                    newRow = new PlayerGridViewModel();
                    newRow.EUState = playerEUState;
                    newRow.ClubState = playerClubState;
                    addPlayerToGrid(player, ref newRow, ref playerPosition, ref playerClub, ref playerNationalities);

                    this.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      currentResultDelegate, newRow);
                }

                this.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      finalResultDelegate);
            }
            //catch (Exception e)
            {
              //  globalFuncs.logging.setErrorLog(ref e, false);
            }
        }

        public void currentResultWonder(PlayerGridViewModel newRow)
        {
            if (newRow != null) dataTable.Add(newRow);
            this.vm.results.Text = dataTable.Count + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_WONDERKIDENTRIESFOUND] + ".";
            if (context.playersPRTotal > 0) this.vm.results.Text += " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_CALCPRFOR] +
                " " + context.staffCRTotal + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_PLAYERS] + ".";
            this.windowMain.vm.tabplayers.TextBlockText = globalFuncs.localization.WindowMainLabels[0] +
                " (" + dataTable.Count + ")";
        }

        public void finalResultWonder()
        {
            currentResultWonder(null);
            this.ButtonWonder.IsEnabled = true;
            this.vm.wonder.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_WONDERKIDS];
            setControlAvailability(true);
            this.ButtonSearch.IsEnabled = true;
            globalFuncs.scoutTimer.stop();
            this.vm.results.Text += globalFuncs.localization.SearchingResults[ScoutLocalization.SR_QUERYTOOK] + " " + globalFuncs.scoutTimer.secondsFloat() + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEC] + ".";
        }

        public void addPlayerToGrid(Player player, ref PlayerGridViewModel newRow, ref string playerPosition, ref string playerClub, ref List<String> playerNationalities)
        {
            PreferencesSettings settings = GlobalSettings.getSettings().curPreferencesSettings;
            ScoutLocalization localization = globalFuncs.localization;

            newRow.imageButton = new ImageButtonContext();
            if (context.shortlistIDList.Contains(player.ID))
            {
                newRow.S = true;
                newRow.imageButton.ImageSource = globalFuncs.shortlistSelected;
            }
            else
            {
                newRow.S = false;
                newRow.imageButton.ImageSource = globalFuncs.shortlistUnselected;
            }

            newRow.ID = player.ID;
            if (player.Nickname.Equals(""))
                newRow.FullName = player.FirstName + " " + player.LastName;
            else
                newRow.FullName = player.Nickname;
            for (int i = 0; i < playerNationalities.Count; ++i)
            {
                if (i != 0) newRow.Nation += "/";
                newRow.Nation += playerNationalities[i];
            }
            newRow.Club = playerClub;
            newRow.TeamSquad = localization.ProfileGenericLabels[ScoutLocalization.PG_NONE];
            if (player.Team != null)
                newRow.TeamSquad = player.Team.Type.ToString();
            newRow.Position = playerPosition;
            newRow.Age = player.Age;
            newRow.CA = player.CurrentPlayingAbility;
            newRow.PA = player.PotentialPlayingAbility;
            newRow.ADiff = player.PotentialPlayingAbility - player.CurrentPlayingAbility;
            if (!context.playersPRID.Contains(player.ID))
                context.calculatePlayerPR(player);
            PositionalRatings pr = (PositionalRatings)context.playersPRID[player.ID];
            newRow.BestPR = pr.bestPos;
            newRow.BestPRperc = (pr.bestPosR * 0.01f).ToString("P2");
            newRow.CurrentValue = (int)(player.Value * settings.currencyMultiplier.multiplier);
            newRow.SaleValue = (int)(player.SaleValue * settings.currencyMultiplier.multiplier);

            int playerWage = 0;
            String contractStarted = localization.ProfileGenericLabels[ScoutLocalization.PG_NONE]; ;
            String contractExpiring = localization.ProfileGenericLabels[ScoutLocalization.PG_NONE]; ;
            long contractStartedTicks = 0;
            long contractExpiringTicks = 0;
            Contract contract = null;
            context.findPlayerContractGrid(player, ref contract, ref playerWage);
            if (contract != null)
            {
                contractStarted = contract.ContractStarted.Date.ToShortDateString();
                contractExpiring = contract.ContractExpiryDate.Date.ToShortDateString();
                contractStartedTicks = contract.ContractStarted.Ticks;
                contractExpiringTicks = contract.ContractExpiryDate.Ticks;
            }

            newRow.isFree = newRow.ClubState == PLAYERCLUBSTATE.PCS_FREE;
            if (contract != null)
            {
                newRow.isLoan = newRow.ClubState == PLAYERCLUBSTATE.PCS_LOAN;
                newRow.IsCoown = newRow.ClubState == PLAYERCLUBSTATE.PCS_COOWN;
                long ticks = context.fm.MetaData.IngameDate.AddMonths(6).Ticks;
                newRow.IsExpiring6mo = ticks < contract.ContractExpiryDate.Ticks;
                ticks = context.fm.MetaData.IngameDate.AddYears(1).Ticks;
                newRow.IsExpiring1y = ticks < contract.ContractExpiryDate.Ticks;
                bool expiring = ticks < contract.ContractExpiryDate.Ticks;

                DateTime oldDate = new DateTime(1910, 01, 01);
                if (contract.ContractExpiryDate.CompareTo(oldDate) > 0)
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
            newRow.IsRegen = player.ID >= globalFuncs.regenMinNumber;

            newRow.IsBest = pr.bestPosR > globalFuncs.bestPRMinNumber;
            newRow.IsEU = newRow.EUState == PLAYEREUSTATE.PES_EU;

            newRow.IsWonder = (player.Age <= 0 || player.Age > settings.wonderkidsMaxAge) && (player.PotentialPlayingAbility < settings.wonderkidsMinPA);

            newRow.isWanted = false;
            newRow.IsMultiple = playerNationalities.Count > 1;

            newRow.statusTooltip = String.Empty;
            if (newRow.isFree) newRow.statusTooltip = localization.statusTooltips[ScoutLocalization.ST_FREE_PLAYER];
            if (newRow.isLoan)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_LOAN];
            }
            if (newRow.IsCoown)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_COOWNED];
            }
            if (newRow.IsExpiring6mo)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine; 
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_EXPIRING_6MO];
            }
            else if (newRow.IsExpiring1y)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_EXPIRING_1Y];
            }
            if (newRow.IsRegen)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_REGEN];
            }
            if (newRow.IsBest)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_BEST_PR];
            }
            if (newRow.IsEU)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_EU];
            }
            if (newRow.IsWonder)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_WONDERKID];
            }
            if (newRow.IsMultiple)
            {
                if (newRow.statusTooltip.Length > 0) newRow.statusTooltip += Environment.NewLine;
                newRow.statusTooltip += localization.statusTooltips[ScoutLocalization.ST_MULTIPLE_NATIONALITIES];
            }

            newRow.ContractStarted = contractStarted;
            newRow.ContractStartedTicks = contractStartedTicks;
            newRow.ContractExpiring = contractExpiring;
            newRow.ContractExpiringTicks = contractExpiringTicks;
            newRow.CurrentWage = (int)(playerWage * settings.wageMultiplier.multiplier * settings.currencyMultiplier.multiplier);
            newRow.WorldReputation = player.WorldReputation;
            newRow.NationalReputation = player.HomeReputation;
            newRow.LocalReputation = player.CurrentReputation;
            newRow.Corners = (int)(player.Skills.Corners * 0.2f);
            newRow.Crossing = (int)(player.Skills.Crossing * 0.2f);
            newRow.Dribbling = (int)(player.Skills.Dribbling * 0.2f);
            newRow.Finishing = (int)(player.Skills.Finishing * 0.2f);
            newRow.FirstTouch = (int)(player.Skills.FirstTouch * 0.2f);
            newRow.FreeKicks = (int)(player.Skills.Freekicks * 0.2f);
            newRow.Heading = (int)(player.Skills.Heading * 0.2f);
            newRow.LongShots = (int)(player.Skills.LongShots * 0.2f);
            newRow.LongThrows = (int)(player.Skills.Longthrows * 0.2f);
            newRow.Marking = (int)(player.Skills.Marking * 0.2f);
            newRow.Passing = (int)(player.Skills.Passing * 0.2f);
            newRow.PenaltyTaking = (int)(player.Skills.PenaltyTaking * 0.2f);
            newRow.Tackling = (int)(player.Skills.Tackling * 0.2f);
            newRow.Technique = (int)(player.Skills.Technique * 0.2f);
            newRow.Acceleration = (int)(player.Skills.Acceleration * 0.2f);
            newRow.Agility = (int)(player.Skills.Agility * 0.2f);
            newRow.Balance = (int)(player.Skills.Balance * 0.2f);
            newRow.Jumping = (int)(player.Skills.Jumping * 0.2f);
            newRow.NaturalFitness = (int)(player.Skills.NaturalFitness * 0.2f);
            newRow.Pace = (int)(player.Skills.Pace * 0.2f);
            newRow.Stamina = (int)(player.Skills.Stamina * 0.2f);
            newRow.Strength = (int)(player.Skills.Strength * 0.2f);
            newRow.LeftFoot = (int)(player.Skills.LeftFoot * 0.2f);
            newRow.RightFoot = (int)(player.Skills.RightFoot * 0.2f);
            newRow.Aggression = (int)(player.Skills.Aggression * 0.2f);
            newRow.Anticipation = (int)(player.Skills.Anticipation * 0.2f);
            newRow.Bravery = (int)(player.Skills.Bravery * 0.2f);
            newRow.Composure = (int)(player.Skills.Composure * 0.2f);
            newRow.Creativity = (int)(player.Skills.Creativity * 0.2f);
            newRow.Concentration = (int)(player.Skills.Concentration * 0.2f);
            newRow.Decisions = (int)(player.Skills.Decisions * 0.2f);
            newRow.Determination = (int)(player.Skills.Determination * 0.2f);
            newRow.Flair = (int)(player.Skills.Flair * 0.2f);
            newRow.Influence = (int)(player.Skills.Influence * 0.2f);
            newRow.OffTheBall = (int)(player.Skills.OffTheBall * 0.2f);
            newRow.Positioning = (int)(player.Skills.Positioning * 0.2f);
            newRow.TeamWork = (int)(player.Skills.Teamwork * 0.2f);
            newRow.WorkRate = (int)(player.Skills.Workrate * 0.2f);
            newRow.Consistency = (int)(player.Skills.Consistency * 0.2f);
            newRow.Dirtyness = (int)(player.Skills.Dirtyness * 0.2f);
            newRow.ImportantMatches = (int)(player.Skills.ImportantMatches * 0.2f);
            newRow.InjuryProneness = (int)(player.Skills.InjuryProness * 0.2f);
            newRow.Versatility = (int)(player.Skills.Versatility * 0.2f);
            newRow.AerialAbility = (int)(player.Skills.AerialAbility * 0.2f);
            newRow.CommandOfArea = (int)(player.Skills.CommandOfArea * 0.2f);
            newRow.Communication = (int)(player.Skills.Communication * 0.2f);
            newRow.Eccentricity = (int)(player.Skills.Eccentricity * 0.2f);
            newRow.Handling = (int)(player.Skills.Handling * 0.2f);
            newRow.Kicking = (int)(player.Skills.Kicking * 0.2f);
            newRow.OneOnOnes = (int)(player.Skills.OneOnOnes * 0.2f);
            newRow.Reflexes = (int)(player.Skills.Reflexes * 0.2f);
            newRow.RushingOut = (int)(player.Skills.RushingOut * 0.2f);
            newRow.TendencyToPunch = (int)(player.Skills.TendencyToPunch * 0.2f);
            newRow.Throwing = (int)(player.Skills.Throwing * 0.2f);
            newRow.Adaptability = player.Skills.Adaptability;
            newRow.Ambition = player.Skills.Ambition;
            newRow.Controversy = player.Skills.Controversy;
            newRow.Loyalty = player.Skills.Loyalty;
            newRow.Pressure = player.Skills.Pressure;
            newRow.Professionalism = player.Skills.Professionalism;
            newRow.Sportsmanship = player.Skills.Sportsmanship;
            newRow.Temperament = player.Skills.Temperament;
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
            numericUpDownArray[++c] = (int)this.value.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)this.value.NumericUpDownMax.Value;
            numericUpDownArray[++c] = (int)this.saleValue.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)this.saleValue.NumericUpDownMax.Value;
            numericUpDownArray[++c] = ((int)((float)attributes.aerialability.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.aerialability.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.commandofarea.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.commandofarea.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.communication.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.communication.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.eccentricity.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.eccentricity.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.handling.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.handling.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.kicking.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.kicking.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.oneonones.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.oneonones.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.reflexes.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.reflexes.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.rushingout.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.rushingout.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.tendencytopunch.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.tendencytopunch.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.throwing.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.throwing.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.corners.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.corners.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.crossing.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.crossing.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.dribbling.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.dribbling.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.finishing.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.finishing.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.firsttouch.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.firsttouch.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.freekicks.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.freekicks.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.heading.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.heading.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.longshots.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.longshots.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.longthrows.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.longthrows.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.marking.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.marking.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.passing.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.passing.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.penaltytaking.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.penaltytaking.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.tackling.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.tackling.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.technique.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.technique.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.acceleration.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.acceleration.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.agility.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.agility.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.balance.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.balance.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.jumping.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.jumping.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.naturalfitness.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.naturalfitness.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.pace.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.pace.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.stamina.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.stamina.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.strength.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.strength.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.leftfoot.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.leftfoot.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.rightfoot.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.rightfoot.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.aggression.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.aggression.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.anticipation.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.anticipation.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.bravery.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.bravery.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.composure.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.composure.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.concentration.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.concentration.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.creativity.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.creativity.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.decisions.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.decisions.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.determination.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.determination.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.flair.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.flair.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.influence.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.influence.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.offtheball.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.offtheball.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.positioning.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.positioning.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.teamwork.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.teamwork.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.workrate.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.workrate.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.consistency.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.consistency.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.dirtyness.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.dirtyness.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.importantmatches.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.importantmatches.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.injuryproneness.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.injuryproneness.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.versatility.NumericUpDownMin.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)attributes.versatility.NumericUpDownMax.Value * 5.0f));
            numericUpDownArray[++c] = (int)attributes.adaptability.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)attributes.adaptability.NumericUpDownMax.Value;
            numericUpDownArray[++c] = (int)attributes.ambition.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)attributes.ambition.NumericUpDownMax.Value;
            numericUpDownArray[++c] = (int)attributes.controversy.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)attributes.controversy.NumericUpDownMax.Value;
            numericUpDownArray[++c] = (int)attributes.loyalty.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)attributes.loyalty.NumericUpDownMax.Value;
            numericUpDownArray[++c] = (int)attributes.pressure.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)attributes.pressure.NumericUpDownMax.Value;
            numericUpDownArray[++c] = (int)attributes.professionalism.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)attributes.professionalism.NumericUpDownMax.Value;
            numericUpDownArray[++c] = (int)attributes.sportsmanship.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)attributes.sportsmanship.NumericUpDownMax.Value;
            numericUpDownArray[++c] = (int)attributes.temperament.NumericUpDownMin.Value;
            numericUpDownArray[++c] = (int)attributes.temperament.NumericUpDownMax.Value;
        }

        private bool testSpecialAttributes(Player player, ref int[] numericUpDownArray)
        {
            int statsCounter = 0;
            // special attributes
            if (player.Age < numericUpDownArray[statsCounter++]
              || player.Age > numericUpDownArray[statsCounter++]) return false;
            if (player.CurrentPlayingAbility < numericUpDownArray[statsCounter++]
              || player.CurrentPlayingAbility > numericUpDownArray[statsCounter++]) return false;
            if (player.PotentialPlayingAbility < numericUpDownArray[statsCounter++]
              || player.PotentialPlayingAbility > numericUpDownArray[statsCounter++]) return false;
            if (player.Value < numericUpDownArray[statsCounter++]
              || player.Value > numericUpDownArray[statsCounter++]) return false;
            int minSaleValue = numericUpDownArray[statsCounter++];
            if (minSaleValue == 0) minSaleValue = -1;
            if (player.SaleValue < minSaleValue
              || player.SaleValue > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.AerialAbility < numericUpDownArray[statsCounter++]
             || player.Skills.AerialAbility > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.CommandOfArea < numericUpDownArray[statsCounter++]
             || player.Skills.CommandOfArea > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Communication < numericUpDownArray[statsCounter++]
             || player.Skills.Communication > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Eccentricity < numericUpDownArray[statsCounter++]
             || player.Skills.Eccentricity > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Handling < numericUpDownArray[statsCounter++]
             || player.Skills.Handling > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Kicking < numericUpDownArray[statsCounter++]
             || player.Skills.Kicking > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.OneOnOnes < numericUpDownArray[statsCounter++]
             || player.Skills.OneOnOnes > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Reflexes < numericUpDownArray[statsCounter++]
             || player.Skills.Reflexes > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.RushingOut < numericUpDownArray[statsCounter++]
             || player.Skills.RushingOut > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.TendencyToPunch < numericUpDownArray[statsCounter++]
             || player.Skills.TendencyToPunch > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Throwing < numericUpDownArray[statsCounter++]
             || player.Skills.Throwing > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Corners < numericUpDownArray[statsCounter++]
              || player.Skills.Corners > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Crossing < numericUpDownArray[statsCounter++]
              || player.Skills.Crossing > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Dribbling < numericUpDownArray[statsCounter++]
              || player.Skills.Dribbling > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Finishing < numericUpDownArray[statsCounter++]
              || player.Skills.Finishing > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.FirstTouch < numericUpDownArray[statsCounter++]
              || player.Skills.FirstTouch > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Freekicks < numericUpDownArray[statsCounter++]
              || player.Skills.Freekicks > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Heading < numericUpDownArray[statsCounter++]
              || player.Skills.Heading > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.LongShots < numericUpDownArray[statsCounter++]
              || player.Skills.LongShots > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Longthrows < numericUpDownArray[statsCounter++]
              || player.Skills.Longthrows > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Marking < numericUpDownArray[statsCounter++]
              || player.Skills.Marking > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Passing < numericUpDownArray[statsCounter++]
              || player.Skills.Passing > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.PenaltyTaking < numericUpDownArray[statsCounter++]
              || player.Skills.PenaltyTaking > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Tackling < numericUpDownArray[statsCounter++]
              || player.Skills.Tackling > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Technique < numericUpDownArray[statsCounter++]
              || player.Skills.Technique > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Acceleration < numericUpDownArray[statsCounter++]
              || player.Skills.Acceleration > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Agility < numericUpDownArray[statsCounter++]
              || player.Skills.Agility > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Balance < numericUpDownArray[statsCounter++]
              || player.Skills.Balance > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Jumping < numericUpDownArray[statsCounter++]
              || player.Skills.Jumping > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.NaturalFitness < numericUpDownArray[statsCounter++]
              || player.Skills.NaturalFitness > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Pace < numericUpDownArray[statsCounter++]
              || player.Skills.Pace > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Stamina < numericUpDownArray[statsCounter++]
              || player.Skills.Stamina > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Strength < numericUpDownArray[statsCounter++]
              || player.Skills.Strength > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.LeftFoot < numericUpDownArray[statsCounter++]
              || player.Skills.LeftFoot > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.RightFoot < numericUpDownArray[statsCounter++]
              || player.Skills.RightFoot > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Aggression < numericUpDownArray[statsCounter++]
              || player.Skills.Aggression > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Anticipation < numericUpDownArray[statsCounter++]
              || player.Skills.Anticipation > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Bravery < numericUpDownArray[statsCounter++]
              || player.Skills.Bravery > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Composure < numericUpDownArray[statsCounter++]
              || player.Skills.Composure > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Concentration < numericUpDownArray[statsCounter++]
              || player.Skills.Concentration > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Creativity < numericUpDownArray[statsCounter++]
              || player.Skills.Creativity > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Decisions < numericUpDownArray[statsCounter++]
              || player.Skills.Decisions > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Determination < numericUpDownArray[statsCounter++]
              || player.Skills.Determination > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Flair < numericUpDownArray[statsCounter++]
              || player.Skills.Flair > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Influence < numericUpDownArray[statsCounter++]
              || player.Skills.Influence > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.OffTheBall < numericUpDownArray[statsCounter++]
              || player.Skills.OffTheBall > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Positioning < numericUpDownArray[statsCounter++]
              || player.Skills.Positioning > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Teamwork < numericUpDownArray[statsCounter++]
              || player.Skills.Teamwork > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Workrate < numericUpDownArray[statsCounter++]
              || player.Skills.Workrate > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Consistency < numericUpDownArray[statsCounter++]
              || player.Skills.Consistency > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Dirtyness < numericUpDownArray[statsCounter++]
              || player.Skills.Dirtyness > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.ImportantMatches < numericUpDownArray[statsCounter++]
              || player.Skills.ImportantMatches > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.InjuryProness < numericUpDownArray[statsCounter++]
              || player.Skills.InjuryProness > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Versatility < numericUpDownArray[statsCounter++]
              || player.Skills.Versatility > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Adaptability < numericUpDownArray[statsCounter++]
              || player.Skills.Adaptability > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Ambition < numericUpDownArray[statsCounter++]
              || player.Skills.Ambition > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Controversy < numericUpDownArray[statsCounter++]
              || player.Skills.Controversy > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Loyalty < numericUpDownArray[statsCounter++]
              || player.Skills.Loyalty > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Pressure < numericUpDownArray[statsCounter++]
              || player.Skills.Pressure > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Professionalism < numericUpDownArray[statsCounter++]
              || player.Skills.Professionalism > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Sportsmanship < numericUpDownArray[statsCounter++]
              || player.Skills.Sportsmanship > numericUpDownArray[statsCounter++]) return false;
            if (player.Skills.Temperament < numericUpDownArray[statsCounter++]
              || player.Skills.Temperament > numericUpDownArray[statsCounter++]) return false;
            return true;
        }

        public void setDataContext()
        {
            LabeledTextBoxContext fullName = new LabeledTextBoxContext();
            LabeledTextBoxContext nation = new LabeledTextBoxContext();
            LabeledTextBoxContext club = new LabeledTextBoxContext();
            LabeledComboBoxContext region = new LabeledComboBoxContext();
            LabeledNumericMinMaxContext age = new LabeledNumericMinMaxContext();
            LabeledNumericMinMaxContext ca = new LabeledNumericMinMaxContext();
            LabeledNumericMinMaxContext pa = new LabeledNumericMinMaxContext();
            LabeledNumericMinMaxContext pr = new LabeledNumericMinMaxContext();
            LabeledComboBoxContext bestpr = new LabeledComboBoxContext();
            LabeledNumericMinMaxContext wage = new LabeledNumericMinMaxContext();
            LabeledNumericMinMaxContext value = new LabeledNumericMinMaxContext();
            LabeledNumericMinMaxContext saleValue = new LabeledNumericMinMaxContext();
            LabeledComboBoxContext contractStatus = new LabeledComboBoxContext();
            LabeledComboBoxContext ownership = new LabeledComboBoxContext();
            LabeledComboBoxContext eu = new LabeledComboBoxContext();
            LabeledComboBoxContext regen = new LabeledComboBoxContext();
            LabeledComboBoxContext prefFoot = new LabeledComboBoxContext();
            ImageTextButtonContext search = new ImageTextButtonContext();
            ImageTextButtonContext wonder = new ImageTextButtonContext();

            fullName.LabelWidth = 60;
            fullName.TextBoxWidth = 110;

            nation.LabelWidth = 60;
            nation.TextBoxWidth = 110;

            club.LabelWidth = 60;
            club.TextBoxWidth = 110;

            region.LabelWidth = 60;
            region.ComboBoxWidth = 110;

            age.LabelWidth = 34;
            age.Maximum = 100;
            age.NumericUpDownMinMaxWidth = 50;

            ca.LabelWidth = 34;
            ca.Maximum = 200;
            ca.NumericUpDownMinMaxWidth = 50;

            pa.LabelWidth = 34;
            pa.Maximum = 200;
            pa.NumericUpDownMinMaxWidth = 50;

            pr.LabelWidth = 34;
            pr.Maximum = 100;
            pr.NumericUpDownMinMaxWidth = 50;

            bestpr.LabelWidth = 70;
            bestpr.ComboBoxWidth = 80;

            wage.LabelWidth = 65;
            wage.ValueMax = 40000000;
            wage.Maximum = 40000000;
            wage.NumericUpDownMinMaxWidth = 82;

            value.LabelWidth = 65;
            value.Maximum = 200000000;
            value.NumericUpDownMinMaxWidth = 82;

            saleValue.LabelWidth = 65;
            saleValue.Maximum = 200000000;
            saleValue.NumericUpDownMinMaxWidth = 82;

            contractStatus.LabelWidth = 100;
            contractStatus.ComboBoxWidth = 124;

            ownership.LabelWidth = 100;
            ownership.ComboBoxWidth = 124;

            eu.StackPanelOrientation = Orientation.Vertical;
            eu.LabelWidth = 54;
            eu.LabelHeight = 16;
            eu.LabelAlignment = HorizontalAlignment.Center;
            eu.ComboBoxWidth = 34;
            eu.ComboBoxAlignment = HorizontalAlignment.Center;

            regen.StackPanelOrientation = Orientation.Vertical;
            regen.LabelWidth = 54;
            regen.LabelHeight = 16; 
            regen.LabelAlignment = HorizontalAlignment.Center;
            regen.ComboBoxWidth = 34;
            regen.ComboBoxAlignment = HorizontalAlignment.Center;

            prefFoot.StackPanelOrientation = Orientation.Vertical;
            prefFoot.LabelWidth = 54;
            prefFoot.LabelHeight = 16; 
            prefFoot.LabelAlignment = HorizontalAlignment.Center;
            prefFoot.ComboBoxWidth = 34;
            prefFoot.ComboBoxAlignment = HorizontalAlignment.Center;

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
            region.ComboBoxItems = localization.regions;
            bestpr.ComboBoxItems = localization.bestprs;
            contractStatus.ComboBoxItems = localization.contractStatuses;
            ownership.ComboBoxItems = localization.ownerShips;
            regen.ComboBoxItems = localization.YesNoEmpty;
            prefFoot.ComboBoxItems = localization.prefFoots;
            eu.ComboBoxItems = localization.YesNoEmpty;

            vm = new PlayerSearchTabItemViewModel();
            vm.fullName = fullName;
            vm.nation = nation;
            vm.club = club;
            vm.region = region;
            vm.age = age;
            vm.ca = ca;
            vm.pa = pa;
            vm.pr = pr;
            vm.bestpr = bestpr;
            vm.wage = wage;
            vm.value = value;
            vm.saleValue = saleValue;
            vm.contractStatus = contractStatus;
            vm.ownership = ownership;
            vm.eu = eu;
            vm.regen = regen;
            vm.prefFoot = prefFoot;
            vm.positions = new LabeledHeaderContext();
            vm.sides = new LabeledHeaderContext();
            vm.GK = new LabeledHeaderContext();
            vm.SW = new LabeledHeaderContext(); 
            vm.D = new LabeledHeaderContext();
            vm.WB  = new LabeledHeaderContext();
            vm.DM  = new LabeledHeaderContext();
            vm.M = new LabeledHeaderContext();
            vm.AM  = new LabeledHeaderContext();
            vm.ST  = new LabeledHeaderContext();
            vm.left = new LabeledHeaderContext();
            vm.right = new LabeledHeaderContext();
            vm.center = new LabeledHeaderContext();
            vm.free = new LabeledHeaderContext();
            vm.groupboxsearch = new LabeledHeaderContext();
            vm.groupboxresults = new LabeledHeaderContext();
            vm.customizecolumns = new LabeledHeaderContext();
            vm.search = search;
            vm.wonder = wonder;
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

            vm.pr.ValueMin = 0;
            vm.pr.ValueMax = 100;
            this.pr.NumericUpDownMin.Value = vm.pr.ValueMin;
            this.pr.NumericUpDownMax.Value = vm.pr.ValueMax;

            vm.wage.ValueMin = 0;
            vm.wage.ValueMax = 40000000;
            this.wage.NumericUpDownMin.Value = vm.wage.ValueMin;
            this.wage.NumericUpDownMax.Value = vm.wage.ValueMax;

            vm.value.ValueMin = 0;
            vm.value.ValueMax = 200000000;
            this.value.NumericUpDownMin.Value = vm.value.ValueMin;
            this.value.NumericUpDownMax.Value = vm.value.ValueMax;

            vm.saleValue.ValueMin = 0;
            vm.saleValue.ValueMax = 200000000;
            this.saleValue.NumericUpDownMin.Value = vm.saleValue.ValueMin;
            this.saleValue.NumericUpDownMax.Value = vm.saleValue.ValueMax;

            vm.region.ComboBoxSelectedIndex = -1;
            vm.bestpr.ComboBoxSelectedIndex = -1;
            vm.contractStatus.ComboBoxSelectedIndex = -1;
            vm.ownership.ComboBoxSelectedIndex = -1;
            vm.eu.ComboBoxSelectedIndex = -1;
            vm.regen.ComboBoxSelectedIndex = -1;
            vm.prefFoot.ComboBoxSelectedIndex = 0;
            vm.results.Text = "";
        }

        public void setLocalization()
        {
            ScoutLocalization localization = globalFuncs.localization;
            ObservableCollection<String> GeneralSearchLabels = globalFuncs.localization.GeneralSearchLabels;
            int index = -1;
            vm.groupboxsearch.Header = GeneralSearchLabels[++index];
            vm.groupboxresults.Header = GeneralSearchLabels[++index];
            vm.customizecolumns.Header = GeneralSearchLabels[++index]; 
            
            ObservableCollection<string> searchLabels = globalFuncs.localization.PlayerSearchLabels;
            index = -1;

            vm.fullName.LabelContent = searchLabels[++index];
            vm.fullName.TextBoxInfoText = searchLabels[++index];
            vm.nation.LabelContent = searchLabels[++index];
            vm.nation.TextBoxInfoText = searchLabels[++index];
            vm.club.LabelContent = searchLabels[++index];
            vm.club.TextBoxInfoText = searchLabels[++index];
            vm.region.LabelContent = searchLabels[++index];
            vm.age.LabelContent = searchLabels[++index];
            vm.ca.LabelContent = searchLabels[++index];
            vm.pa.LabelContent = searchLabels[++index];
            vm.pr.LabelContent = searchLabels[++index];
            vm.bestpr.LabelContent = searchLabels[++index];
            vm.wage.LabelContent = searchLabels[++index];
            vm.value.LabelContent = searchLabels[++index];
            vm.saleValue.LabelContent = searchLabels[++index];
            vm.contractStatus.LabelContent = searchLabels[++index];
            vm.ownership.LabelContent = searchLabels[++index];
            vm.eu.LabelContent = searchLabels[++index];
            vm.regen.LabelContent = searchLabels[++index];
            vm.prefFoot.LabelContent = searchLabels[++index];
            vm.positions.Header = searchLabels[++index];
            vm.sides.Header = searchLabels[++index];
            vm.GK.Header = searchLabels[++index];
            vm.SW.Header = searchLabels[++index];
            vm.D.Header = searchLabels[++index];
            vm.WB.Header = searchLabels[++index];
            vm.DM.Header = searchLabels[++index];
            vm.M.Header = searchLabels[++index];
            vm.AM.Header = searchLabels[++index];
            vm.ST.Header = searchLabels[++index];
            vm.left.Header = searchLabels[++index];
            vm.right.Header = searchLabels[++index];
            vm.center.Header = searchLabels[++index];
            vm.free.Header = searchLabels[++index];

            vm.search.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEARCH];
            vm.wonder.TextBlockText = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_WONDERKIDS];

            for (int i = 0; i < this.dataGrid.Columns.Count; ++i)
                this.dataGrid.Columns[i].Header = globalFuncs.localization.playerColumns[i];
        }

        public void initDataTableColumns(ref List<int> columnsWidth)
        {
            dataTable = new ObservableCollection<PlayerGridViewModel>();
            //dataTable = new DataTable();

            this.dataGrid.IsReadOnly = false;

            for (int i = 0; i < columnsWidth.Count; ++i)
            {
                char[] c = { ' ' };
                Binding items = new Binding();
                String colName = globalFuncs.localization.playerColumns[i];
                String natColName = globalFuncs.localization.playerNativeColumns[i];
                String pathString = natColName.Replace(" ", "");
                pathString = pathString.Replace("%", "perc");
                PropertyPath path = new PropertyPath(pathString);
                items.Path = path;
                String sortedPathString = pathString;
                if (natColName.Equals("Contract Started") || natColName.Equals("Contract Expiring"))
                    sortedPathString += "Ticks";

                DataGridColumn dc = null;
                if (natColName.Equals("S"))
                {
                    dc = new DataGridTemplateColumn()
                    {
                        Header = colName,
                        Width = columnsWidth[i],
                        SortMemberPath = sortedPathString,
                        Visibility = Visibility.Collapsed,
                        CellTemplate = App.Current.Resources["ShortlistGrid"] as DataTemplate
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
                        CellTemplate = App.Current.Resources["playerStatusDisplay"] as DataTemplate
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
                //this.dataTable.Columns.Add(new DataColumn(colName));
            }
            List<int> settingColumns = settings.curPreferencesSettings.playerColumns;
            for (int i = 0; i < settingColumns.Count; ++i)
                this.dataGrid.Columns[settingColumns[i]].Visibility = Visibility.Visible;

            //for (int i = 0; i < 440; ++i)
            //{
            //    PlayerGridViewModel r = new PlayerGridViewModel();
            //    r.ID = i;
            //    r.FullName = "Name" + i;
            //    dataTable.Add(r);
            //}
            //vm.dataGrid = dataTable.DefaultView;
            vm.dataGrid = dataTable; 
            this.dataGrid.DataContext = vm.dataGrid;
        }
        
        public void clearData()
        {
            setControlValues();
            this.dataTable.Clear();
            this.windowMain.MenuItemAddToShortlist.IsEnabled = false;
        }
    }
}