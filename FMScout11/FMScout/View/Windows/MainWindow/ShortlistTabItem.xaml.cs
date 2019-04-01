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
using System.IO;
using Microsoft.Win32;
using FMScout.ControlContext;
using FMScout.ViewModel;
using Young3.FMSearch.Core.Entities.InGame;
using System.Collections;
using System.Threading;
using System.Windows.Controls.Primitives;

namespace FMScout.View.MainWindow
{
	public partial class ShortlistTabItem : UserControl
	{
        //public DataTable dataTable = null;
        public ObservableCollection<PlayerGridViewModel> dataTable = null;
        //public List<DataColumn> dataColumnList = null;
        public ShortlistTabItemViewModel vm = null;
        public WindowMain windowMain = null;
        public WindowQuickColumns windowQuickColumns = null;
        private Settings settings = null;
        private Context context = null;
        private GlobalFuncs globalFuncs = null;

        private delegate void LoadDelegate(ref List<int> playersID);
        private delegate void ImportDelegate(ref List<PlayerGridViewModel> playerRows);
        private delegate void CurrentResultDelegate(PlayerGridViewModel newRow);
        private delegate void FinalResultDelegate();
        private delegate void ProgressBarDelegate();
        private delegate void SetProgressBarValueDelegate();

        public ShortlistTabItem()
        {
            this.InitializeComponent();
            this.progressBar.Opacity = 0;

            settings = GlobalSettings.getSettings();
            context = ScoutContext.getScoutContext();
            globalFuncs = Globals.getGlobalFuncs();
            setDataContext();

            this.ButtonColumns.Click += new RoutedEventHandler(ButtonColumns_Click);
            this.dataGrid.SelectionChanged += new SelectionChangedEventHandler(dataGrid_SelectionChanged);
            this.dataGrid.MouseDoubleClick += new MouseButtonEventHandler(dataGrid_MouseDoubleClick);
            this.dataGrid.MouseLeftButtonUp += new MouseButtonEventHandler(dataGrid_MouseLeftButtonUp);
            this.dataGrid.MouseRightButtonUp += new MouseButtonEventHandler(dataGrid_MouseRightButtonUp);
        }

        private void dataGrid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            int selectedRows = this.dataGrid.SelectedItems.Count;
            if (selectedRows > 0)
            {
                DataGridContextMenuViewModel cvm = new DataGridContextMenuViewModel();

                ImageTextButtonContext removeshortlist = new ImageTextButtonContext();
                removeshortlist.ImageSource = TryFindResource("remove") as ImageSource;
                removeshortlist.ImageStretch = Stretch.UniformToFill;
                removeshortlist.ImageWidth = 12;
                removeshortlist.ImageHeight = 12;
                removeshortlist.TextBlockText = "Remove from Shortlist";

                cvm.removeshortlist = removeshortlist;
                ContextMenu m = TryFindResource("dataGridMenu") as ContextMenu;
                m.DataContext = cvm;
                this.dataGrid.ContextMenu = m;
                this.dataGrid.ContextMenuClosing += new ContextMenuEventHandler(ContextMenu_Closing);
            }
        }

        private void ContextMenu_Closing(object sender, RoutedEventArgs e)
        {
            this.dataGrid.ContextMenuClosing -= new ContextMenuEventHandler(ContextMenu_Closing);
            this.dataGrid.ContextMenu = null;
        }

        private void removeShortlist_Click(object sender, RoutedEventArgs e)
        {
            int selectedRows = this.dataGrid.SelectedItems.Count;
            if (selectedRows > 0)
            {
                List<int> playerRows = new List<int>();
                for (int i = 0; i < this.dataGrid.SelectedItems.Count; ++i)
                    playerRows.Add(((PlayerGridViewModel)this.dataGrid.SelectedItems[i]).ID);

                this.removeFromShortlist(ref playerRows);
            }
        }


        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.dataGrid.SelectedItems.Count > 0)
                this.windowMain.MenuItemExportSelectedShortlist.IsEnabled = true;
            else
                this.windowMain.MenuItemExportSelectedShortlist.IsEnabled = false;
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.dataGrid.SelectedItems.Count > 0)
                this.windowMain.PlayerSearch.createWindowProfile(-1);
        }

        private void dataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.windowMain.PlayerSearch.addToContext(ref this.dataGrid);
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

        public void loadShortlist()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sports Interactive\\Football Manager 2011\\shortlists";
            openFileDialog.Multiselect = true;
            openFileDialog.DefaultExt = "slf";
            // The Filter property requires a search string after the pipe ( | )
            openFileDialog.Filter = "FM2011 Shortlists (*.slf)|*.slf";//|CSV Spreadsheet(*.csv)|*.csv";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileNames.Length > 0)
            {
                string ext = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf(".") + 1);
                List<int> playersToLoad = new List<int>();
                if (ext.Equals("slf"))
                {
                    int count = 0;
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        ++count;
                        using (FileStream stream = new FileStream(filename, FileMode.Open))
                        {
                            byte[] header = new byte[14];
                            stream.Read(header, 0, header.Length);
                            byte[] shortlistNameLengthByte = new byte[4];
                            stream.Read(shortlistNameLengthByte, 0, shortlistNameLengthByte.Length);
                            int shortlistNameLength = globalFuncs.ReadInt32(shortlistNameLengthByte);
                            byte[][] shortlistNameChar = new byte[shortlistNameLength][];
                            for (int i = 0; i < shortlistNameLength; ++i)
                            {
                                shortlistNameChar[i] = new byte[2];
                                stream.Read(shortlistNameChar[i], 0, shortlistNameChar[i].Length);
                            }

                            byte[] bogusByte = new byte[2];
                            stream.Read(bogusByte, 0, bogusByte.Length);
                            int bogus = globalFuncs.ReadInt16(bogusByte);

                            byte[] noItemsByte = new byte[4];
                            stream.Read(noItemsByte, 0, noItemsByte.Length);
                            int noItems = globalFuncs.ReadInt32(noItemsByte);

                            byte[][] playersIDByte = new byte[noItems][];
                            for (int i = 0; i < noItems; ++i)
                            {
                                playersIDByte[i] = new byte[4];
                                stream.Read(playersIDByte[i], 0, playersIDByte[i].Length);
                                playersToLoad.Add(globalFuncs.ReadInt32(playersIDByte[i]));
                            }

                            byte[] endByte = new byte[4];
                            stream.Read(endByte, 0, 1);
                            int end = globalFuncs.ReadInt32(endByte);
                        }
                    }
                }
                else if (ext.Equals("csv"))
                {
                    int count = 0;
                    string separator = ",";
                    if (ext.Equals("txt"))
                        separator = " ";
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        ++count;
                        using (FileStream stream = new FileStream(filename, FileMode.Open))
                        {
                            using (StreamReader sw = new StreamReader(stream))
                            {
                                string columnLine = sw.ReadLine();
                                List<int> players = new List<int>();

                                while (!sw.EndOfStream)
                                {
                                    string token = sw.ReadLine();
                                    string t = token;
                                    token = token.Substring(0, token.IndexOf(separator));
                                    char[] rem = { '"', ',', '\\' };
                                    token = token.TrimStart(rem);
                                    token = token.TrimEnd(rem);
                                    playersToLoad.Add(Int32.Parse(token));
                                }
                            }
                        }
                    }
                }

                if (playersToLoad.Count > 0)
                {
                    globalFuncs.scoutTimer.start();
                    this.windowMain.CurrentGameDate.Text = context.fm.MetaData.IngameDate.ToLongDateString();
                    this.windowMain.vm.tabshortlist.TextBlockText = globalFuncs.localization.WindowMainLabels[3];
                    setControlAvailability(false);
                    this.vm.results.Text = "Importing...";

                    LoadDelegate d = new LoadDelegate(this.loadShortlistPlayers);
                    d.BeginInvoke(ref playersToLoad, null, null);
                    ProgressBarDelegate p = new ProgressBarDelegate(this.updateProgressBar);
                    p.BeginInvoke(null, null);
                }
            }
        }

        public void loadShortlistPlayers(ref List<int> playersID)
        {
            CurrentResultDelegate currentResultDelegate = new CurrentResultDelegate(currentResult);
            FinalResultDelegate finalResultDelegate = new FinalResultDelegate(finalResult);

            List<string> player_positions = new List<string>();
            List<string> player_sides = new List<string>();

            PLAYERCLUBSTATE playerClubState = PLAYERCLUBSTATE.PCS_FREE;
            PLAYEREUSTATE playerEUState = PLAYEREUSTATE.PES_NONEU;

            PlayerGridViewModel newRow;
            List<String> playerNationalities = new List<String>();
            String playerClub = "";
            Contract contract = null;
            double counter = 0;
            double total = 100.0 / (double)context.fm.Players.Count();

            // remove duplicate rows
            for (int i = 0; i < playersID.Count; ++i)
            {
                if (context.shortlistIDList.Contains(playersID[i]))
                    playersID.Remove(playersID[i]);
            }

            foreach (Player _player in context.fm.Players)
            {
                progressBarValue = counter * total;
                ++counter;
                Player player = null;
                for (int i = 0; i < playersID.Count; ++i)
                {
                    // check ID
                    player = null;
                    if (_player.ID.Equals(playersID[i]))
                    {
                        if (!context.shortlistIDList.Contains(_player.ID))
                        {
                            player = _player;
                            playersID.RemoveAt(i);
                            break;
                        }
                    }
                }

                if (player != null)
                {
                    // check position
                    string playerPosition = "";
                    context.find_player_position(player, ref playerPosition, ref player_positions, ref player_sides, true);

                    // check club
                    playerClub = globalFuncs.localization.SearchingResults[ScoutLocalization.SR_FREEPLAYER];
                    playerClubState = PLAYERCLUBSTATE.PCS_FREE;
                    context.findPlayerContractQuery(player, ref contract, ref playerClub, ref playerClubState);

                    // check nation
                    playerNationalities.Clear();
                    playerNationalities.Add(player.Nationality.Name);
                    // other nationalities
                    List<PlayerRelations> relations = player.Relations.Relations;
                    if (player.Relations != null)
                    {
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
                    addShortlistPlayerToGrid(player, ref newRow, ref playerPosition, ref playerClub, ref playerNationalities);

                    this.Dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    currentResultDelegate, newRow);
                }
            }

            this.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  finalResultDelegate);
        }

        public void addShortlistPlayerToGrid(Player player, ref PlayerGridViewModel newRow, ref string playerPosition, ref string playerClub, ref List<String> playerNationalities)
        {
            PreferencesSettings settings = GlobalSettings.getSettings().curPreferencesSettings;
            context.shortlistIDList.Add(player.ID, context.shortlistIDList.Count);

            for (int i = 0; i < this.windowMain.PlayerSearch.dataTable.Count; ++i)
            {
                PlayerGridViewModel r = this.windowMain.PlayerSearch.dataTable[i];
                if (player.ID == r.ID)
                {
                    r.S = true;
                    r.imageButton.ImageSource = globalFuncs.shortlistSelected;
                    break;
                }
            }

            this.windowMain.PlayerSearch.addPlayerToGrid(player, ref newRow, ref playerPosition, ref playerClub, ref playerNationalities);
        }

        public void exportShortlist(bool allrows)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sports Interactive\\Football Manager 2011\\shortlists";
            saveFileDialog.DefaultExt = "slf";
            // The Filter property requires a search string after the pipe ( | )
            saveFileDialog.Filter = "FM2011 Shortlists (*.slf)|*.slf";//|CSV Spreadsheet(*.csv)|*.csv|TXT document(*.txt)|*.txt";
            saveFileDialog.Title = "Save an FM2011 Shortlist File";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                string ext = saveFileDialog.FileName.Substring(saveFileDialog.FileName.LastIndexOf(".") + 1);
                int count = 0;
                if (allrows)
                    count = this.dataGrid.Items.Count;
                else
                    count = this.dataGrid.SelectedItems.Count;
                int playerID = 0;
                // Saves the file via a FileStream created by the OpenFile method.
                if (count > 0)
                {
                    using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                    {
                        stream.SetLength(0);
                        if (ext.Equals("slf"))
                        {
                            using (BinaryWriter sw = new BinaryWriter(stream))
                            {
                                int[] playersID = new int[count];
                                sw.Write(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
                                sw.Write(globalFuncs.FromIntToHex(saveFileDialog.FileName.Length));
                                sw.Write(globalFuncs.FromStringToHex(saveFileDialog.FileName));
                                sw.Write(new byte[] { 0x0, 0x0 });
                                sw.Write(globalFuncs.FromIntToHex(playersID.Length));
                                for (int i = 0; i < playersID.Length; ++i)
                                {
                                    if (allrows)
                                        playerID = ((PlayerGridViewModel)(this.dataGrid.Items[i])).ID;
                                    else
                                        playerID = ((PlayerGridViewModel)(this.dataGrid.SelectedItems[i])).ID;

                                    playersID[i] = playerID;
                                    sw.Write(globalFuncs.FromIntToHex(playersID[i]));
                                }
                                sw.Write(0x0);
                                sw.Close();
                            }
                        }
                        /*else if (ext.Equals("csv") || ext.Equals("txt"))
                         {
                             string separator = ",";
                             using (StreamWriter sw = new StreamWriter(stream))
                             {
                                 string strValue = string.Empty;
                                 strValue = "\"" + this.dataGrid.Columns[0].Header + "\"";
                                 for (int i = 1; i < this.dataGrid.Columns.Count - 1; i++)
                                 {
                                     if (this.dataGrid.Columns[i].Visibility == Visibility.Visible)
                                         strValue += separator + "\"" + this.dataGrid.Columns[i].Header + "\"";
                                 }
                                 strValue += Environment.NewLine;
                                 IList items = null;
                                 if (allrows)
                                     items = this.dataGrid.Items;
                                 else
                                     items = this.dataGrid.SelectedItems;

                                 for (int i = 0; i < items.Count; i++)
                                 {
                                     Console.WriteLine(((PlayerGridViewModel)items[i]).FullName);
                                 }
                                 strValue += Environment.NewLine;
                                 
                                 for (int i = 0; i < items.Count; i++)
                                 {
                                     DataRowView drv = (DataRowView)items[i];
                                     string cellOfColumn1 = drv[0].ToString();
                                     string cellOfColumn2 = drv[1].ToString();
                                     //for (int j = 1; j < this.dataGrid.Columns.Count - 1; j++)
                                     //{
                                     //    if (this.dataGrid.Columns[i].Visibility == Visibility.Visible)
                                     //    {
                                     //        DataGridCell cell = GetCell(this.dataGrid, i, j);
                                     //        String content = ((TextBlock)cell.Content).Text;
                                     //        if (!string.IsNullOrEmpty(content.ToString()))
                                     //            strValue += separator + "\"" + content.ToString() + "\"";
                                     //            else
                                     //                strValue += separator + " ";
                                            
                                     //    }
                                     //}
                                    // PlayerGridViewModel row = (PlayerGridViewModel)(this.dataGrid.Items[i]);
                                     //for (int j = 0; j < row.ItemArray.Length; j++)
                                     //{
                                     //    object rowItem = row.ItemArray[j];
                                     //    //if (j == 0 || this.dataGrid.Items[i].Cells[j].Visible)
                                     //    //{
                                     //        if (j != 0)
                                     //        {
                                     //            if (!string.IsNullOrEmpty(rowItem.ToString()))
                                     //                strValue += separator + "\"" + rowItem.ToString() + "\"";
                                     //            else
                                     //                strValue += separator + " ";
                                     //        }
                                     //        else
                                     //        {
                                     //            if (!string.IsNullOrEmpty(rowItem.ToString()))
                                     //                strValue += "\"" + rowItem.ToString() + "\"";
                                     //            else
                                     //                strValue += " ";
                                     //        }
                                     //    //}
                                     //}
                                     strValue += Environment.NewLine;
                                 }
                                 sw.Write(strValue.ToCharArray());
                                 sw.Close();
                             }
                         }
                         stream.Close();*/
                    }
                }
            }
        }

        public static DataGridCell GetCell(DataGrid dataGrid, int row, int column)
        {
            DataGridRow rowContainer = Helper.GetRow(dataGrid, row);
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = Helper.GetVisualChild<DataGridCellsPresenter>(rowContainer);

                // try to get the cell but it may possibly be virtualized
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                if (cell == null)
                {
                    // now try to bring into view and retreive the cell
                    dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);

                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                }

                return cell;
            }

            return null;
        }

        public static class Helper
        {
            #region GetCell

            public static DataGridCell GetCell(DataGrid dataGrid, int row, int column)
            {
                DataGridRow rowContainer = GetRow(dataGrid, row);
                if (rowContainer != null)
                {
                    DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);

                    // try to get the cell but it may possibly be virtualized
                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                    if (cell == null)
                    {
                        // now try to bring into view and retreive the cell
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);

                        cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                    }

                    return cell;
                }

                return null;
            }

            #endregion GetCell

            #region GetRow

            /// <summary>
            /// Gets the DataGridRow based on the given index
            /// </summary>
            /// <param name="index">the index of the container to get</param>
            public static DataGridRow GetRow(DataGrid dataGrid, int index)
            {
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
                if (row == null)
                {
                    // may be virtualized, bring into view and try again
                    dataGrid.ScrollIntoView(dataGrid.Items[index]);
                    dataGrid.UpdateLayout();

                    row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
                }

                return row;
            }

            #endregion GetRow

            #region GetRowHeader

            /// <summary>
            /// Gets the DataGridRowHeader based on the row index.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public static DataGridRowHeader GetRowHeader(DataGrid dataGrid, int index)
            {
                return GetRowHeader(GetRow(dataGrid, index));
            }

            /// <summary>
            /// Returns the DataGridRowHeader based on the given row.
            /// </summary>
            /// <param name="row">Uses reflection to access and return RowHeader</param>
            public static DataGridRowHeader GetRowHeader(DataGridRow row)
            {
                if (row != null)
                {
                    return GetVisualChild<DataGridRowHeader>(row);
                }
                return null;
            }

            #endregion GetRowHeader

            #region GetColumnHeader

            public static DataGridColumnHeader GetColumnHeader(DataGrid dataGrid, int index)
            {
                DataGridColumnHeadersPresenter presenter = GetVisualChild<DataGridColumnHeadersPresenter>(dataGrid);

                if (presenter != null)
                {
                    return (DataGridColumnHeader)presenter.ItemContainerGenerator.ContainerFromIndex(index);
                }

                return null;
            }

            #endregion GetColumnHeader

            #region GetVisualChild

            public static T GetVisualChild<T>(Visual parent) where T : Visual
            {
                T child = default(T);

                int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                    child = v as T;
                    if (child == null)
                    {
                        child = GetVisualChild<T>(v);
                    }
                    if (child != null)
                    {
                        break;
                    }
                }

                return child;
            }

            public static T GetVisualChild<T>(Visual parent, int index) where T : Visual
            {
                T child = default(T);

                int encounter = 0;
                Queue<Visual> queue = new Queue<Visual>();
                queue.Enqueue(parent);
                while (queue.Count > 0)
                {
                    Visual v = queue.Dequeue();
                    child = v as T;
                    if (child != null)
                    {
                        if (encounter == index)
                            break;
                        encounter++;
                    }
                    else
                    {
                        int numVisuals = VisualTreeHelper.GetChildrenCount(v);
                        for (int i = 0; i < numVisuals; i++)
                        {
                            queue.Enqueue((Visual)VisualTreeHelper.GetChild(v, i));
                        }
                    }
                }

                return child;
            }

            public static bool VisualChildExists(Visual parent, DependencyObject visualToFind)
            {
                Queue<Visual> queue = new Queue<Visual>();
                queue.Enqueue(parent);
                while (queue.Count > 0)
                {
                    Visual v = queue.Dequeue();
                    DependencyObject child = v as DependencyObject;
                    if (child != null)
                    {
                        if (child == visualToFind)
                            return true;
                    }
                    else
                    {
                        int numVisuals = VisualTreeHelper.GetChildrenCount(v);
                        for (int i = 0; i < numVisuals; i++)
                        {
                            queue.Enqueue((Visual)VisualTreeHelper.GetChild(v, i));
                        }
                    }
                }

                return false;
            }
            #endregion GetVisualChild
        }

        public void addToShortlist(ref DataGrid playerDataGrid)
        {
            List<int> playerRows = new List<int>();
            for (int i = 0; i < playerDataGrid.SelectedItems.Count; ++i)
            {
                PlayerGridViewModel row = (PlayerGridViewModel)playerDataGrid.SelectedItems[i];
                if (!context.shortlistIDList.Contains(row.ID))
                    playerRows.Add(row.ID);
            }

            if (playerRows.Count > 0)
            {
                globalFuncs.scoutTimer.start();
                this.windowMain.CurrentGameDate.Text = context.fm.MetaData.IngameDate.ToLongDateString();
                this.windowMain.vm.tabshortlist.TextBlockText = globalFuncs.localization.WindowMainLabels[3];
                setControlAvailability(false);
                this.vm.results.Text = "Importing...";

                LoadDelegate d = new LoadDelegate(this.loadShortlistPlayers);
                d.BeginInvoke(ref playerRows, null, null);
                ProgressBarDelegate p = new ProgressBarDelegate(this.updateProgressBar);
                p.BeginInvoke(null, null);
            }
        }

        public void addToShortlist(int ID)
        {
            List<int> playersToLoad = new List<int>();
            playersToLoad.Add(ID);
            globalFuncs.scoutTimer.start();
            this.windowMain.CurrentGameDate.Text = context.fm.MetaData.IngameDate.ToLongDateString();
            this.windowMain.vm.tabshortlist.TextBlockText = globalFuncs.localization.WindowMainLabels[3];
            setControlAvailability(false);
            this.vm.results.Text = "Importing...";

            LoadDelegate d = new LoadDelegate(this.loadShortlistPlayers);
            d.BeginInvoke(ref playersToLoad, null, null);
            ProgressBarDelegate p = new ProgressBarDelegate(this.updateProgressBar);
            p.BeginInvoke(null, null); 
        }

        public void removeFromShortlist(ref List<int> playerRows)
        {
            removeShortlistPlayers(ref playerRows);
        }   

        public void removeFromShortlist(int ID)
        {
            List<int> playerRows = new List<int>();
            playerRows.Add(ID);
            removeShortlistPlayers(ref playerRows);
        }

        private void removeShortlistPlayers(ref List<int> playerRows)
        {
            if (playerRows.Count > 0)
            {
                this.windowMain.CurrentGameDate.Text = context.fm.MetaData.IngameDate.ToLongDateString();
                this.windowMain.vm.tabshortlist.TextBlockText = globalFuncs.localization.WindowMainLabels[3];
                setControlAvailability(false);
                ObservableCollection<PlayerGridViewModel> playerGridDataTable = this.windowMain.PlayerSearch.dataTable;
                for (int i = 0; i < playerRows.Count; ++i)
                {
                    int playertoremoverowID = playerRows[i];
                    for (int j = 0; j < this.dataTable.Count; ++j)
                    {
                        PlayerGridViewModel shortlistrow = this.dataTable[j];
                        if (shortlistrow.ID == playertoremoverowID)
                        {
                            for (int k = 0; k < playerGridDataTable.Count; ++k)
                            {
                                PlayerGridViewModel r = playerGridDataTable[k];
                                if (r.ID == shortlistrow.ID)
                                {
                                    r.S = false;
                                    r.imageButton.ImageSource = globalFuncs.shortlistUnselected;
                                    break;
                                }
                            }
                            shortlistrow.S = false;
                            context.shortlistIDList.Remove(shortlistrow.ID);
                            this.dataTable.Remove(shortlistrow);
                            break;
                        }
                    }
                }                
                setControlAvailability(true);
                this.vm.results.Text = dataTable.Count + " shortlist entries found.";
                this.windowMain.vm.tabshortlist.TextBlockText = globalFuncs.localization.WindowMainLabels[3];
                if (this.dataTable.Count > 0) 
                  this.windowMain.vm.tabshortlist.TextBlockText += " (" + dataTable.Count + ")";
            }
        }

        public void currentResult(PlayerGridViewModel newRow)
        {
            if (newRow != null) dataTable.Add(newRow); 
            this.vm.results.Text = dataTable.Count + " shortlist entries found.";
            this.windowMain.vm.tabshortlist.TextBlockText = globalFuncs.localization.WindowMainLabels[3] +
                " (" + dataTable.Count + ")";
        }

        public void finalResult()
        {
            currentResult(null);
            setControlAvailability(true);
            globalFuncs.scoutTimer.stop();
            this.vm.results.Text += globalFuncs.localization.SearchingResults[ScoutLocalization.SR_QUERYTOOK] + " " + globalFuncs.scoutTimer.secondsFloat() + " " + globalFuncs.localization.SearchingResults[ScoutLocalization.SR_SEC] + ".";
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
            this.dataGrid.IsEnabled = res;

            if (!res)
            {
                this.windowMain.MenuItemClearShortlist.IsEnabled = res;
                this.windowMain.MenuItemAddToShortlist.IsEnabled = res;
                this.windowMain.MenuItemImportShortlist.IsEnabled = res;
                this.windowMain.MenuItemExportShortlist.IsEnabled = res;
                
                this.progressBar.Value = 0;
                globalFuncs.FadeInElement(this.progressBar, globalFuncs.progressBarDuration, 1, true);
                setProgressBarValueDelegate = new SetProgressBarValueDelegate(this.setProgressBarValue);
            }
            else
            {
                globalFuncs.FadeOutElement(this.progressBar, globalFuncs.progressBarDuration, this.progressBar.Opacity);
                setProgressBarValueDelegate = null;

                if (dataTable.Count > 0)
                {
                    this.windowMain.MenuItemClearShortlist.IsEnabled = res;
                    this.windowMain.MenuItemAddToShortlist.IsEnabled = res;
                    this.windowMain.MenuItemImportShortlist.IsEnabled = res;
                    this.windowMain.MenuItemExportShortlist.IsEnabled = res;
                }
            }
        }

		public void initDataTableColumns(ref List<int> columnsWidth)
        {
            dataTable = new ObservableCollection<PlayerGridViewModel>();

            this.dataGrid.CanUserDeleteRows = true;

            for (int i = 0; i < columnsWidth.Count; ++i)
            {
                char[] c = { ' ' };
                Binding items = new Binding();
                String colName = globalFuncs.localization.shortlistColumns[i];
                String natColName = globalFuncs.localization.shortlistNativeColumns[i];
                String pathString = natColName.Replace(" ", "");
                pathString = pathString.Replace("%", "perc");
                PropertyPath path = new PropertyPath(pathString);
                items.Path = path;
                String sortedPathString = pathString;
                if (natColName.Equals("Contract Started") || natColName.Equals("Contract Expiring"))
                    sortedPathString += "Ticks";

                DataGridColumn dc = null;
                if (natColName.Contains("Status"))
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
            }


            List<int> settingColumns = settings.curPreferencesSettings.shortlistColumns;
            for (int i = 0; i < settingColumns.Count; ++i)
                this.dataGrid.Columns[settingColumns[i]].Visibility = Visibility.Visible;

            vm.dataGrid = dataTable;
            this.dataGrid.DataContext = vm.dataGrid;

            this.vm.results = new TextBlockContext();
		}

        public void clearData()
        {
            //ObservableCollection<PlayerGridViewModel> playerGridDataTable = this.windowMain.PlayerSearch.dataTable;
            //for (int i = 0; i < playerGridDataTable.Count; ++i)
            //{
            //    PlayerGridViewModel r = playerGridDataTable[i];
            //    if (context.shortlistIDList.Contains(r.ID))
            //    {
            //        r.S = false;
            //        r.imageButton.ImageSource = globalFuncs.shortlistUnselected;
            //        context.shortlistIDList.Remove(r.ID);
            //        if (context.shortlistIDList.Count == 0)
            //            break;
            //    }
            //}
            //this.dataTable.Clear();
            //this.windowMain.MenuItemAddToShortlist.IsEnabled = false;
            //this.windowMain.MenuItemExportShortlist.IsEnabled = false;
            //this.windowMain.MenuItemExportSelectedShortlist.IsEnabled = false;
            //context.shortlistIDList.Clear();
            setControlValues();
        }

        public void setDataContext()
        {
            vm = new ShortlistTabItemViewModel();

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

            for (int i = 0; i < this.dataGrid.Columns.Count; ++i)
                this.dataGrid.Columns[i].Header = globalFuncs.localization.shortlistColumns[i];
        }
	}
}