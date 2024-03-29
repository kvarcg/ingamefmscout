using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Young3.FMSearch.Interface;
using Young3.FMSearch.Business.Entities.InGame;
using Young3.FMSearch.Business.Managers;
using System.Reflection;
using System.Globalization;
using System.Xml;
using System.IO;

namespace FMScout
{
    public struct PositionalRatings
    {
        public float FCStrong;
        public float FCQuick;
        public float AMR;
        public float AML;
        public float AMC;
        public float DMR;
        public float DML;
        public float DMC;
        public float DR;
        public float DL;
        public float DC;
        public float GK;
        public float bestPosR;
        public string bestPos;
        public string desc;
    }

    public struct CoachingRatings
    {
        public int Fitness;
        public int Goalkeepers;
        public int BallControl;
        public int Tactics;
        public int Defending;
        public int Attacking;
        public int Shooting;
        public int SetPieces;
        public string BestCR;
        public int BestCRStars;
    }

    public partial class MainForm : Form
    {
        public static string CurrentVersion
        {
            get
            {
                return "1.32";
            }
        }

        public FMContext fm = null;
        public AboutForm aboutForm = null;
        public PreferencesForm preferencesForm = null;
        public LoadingForm loadingForm = null;
        public DonationForm donationForm = null;
        public MiniScoutForm miniScoutForm = null;
        public DataTable playersDataTable = null;
        public DataTable staffDataTable = null;
        public DataTable teamsDataTable = null;
        public DataTable shortlistDataTable = null;
        public List<DataColumn> playersDataColumnList = null;
        public List<DataColumn> staffDataColumnList = null;
        public List<DataColumn> teamsDataColumnList = null;
        public List<DataColumn> shortlistDataColumnList = null;
        public List<int> playersLoanRowList = null;
        public List<int> playersCoOwnRowList = null;
        public List<int> playersFreeRowList = null;
        public List<int> playersEUMemberRowList = null;
        public List<int> shortlistLoanRowList = null;
        public List<int> shortlistCoOwnRowList = null;
        public List<int> shortlistFreeRowList = null;
        public List<int> shortlistEUMemberRowList = null;
        public int playersPRTotal;
        public int staffCRTotal;
        public List<int> staffFreeRowList = null;
        public List<int> staffNationalRowList = null;
        public List<int> teamNationalRowList = null;
        public Hashtable allClubs = null;
        public Hashtable playersPRID = null;
        public Hashtable staffCRID = null;
        public Hashtable nationalTeam = null;
        public Hashtable EUcountries = null;
        //public Hashtable playersIDList = null;
        public Hashtable shortlistIDList = null;
        public bool scoutLoaded = false;
        public int currentPlayerIndex = -1;
        public int currentStaffIndex = -1;
        public int currentTeamIndex = -1;
        public int currentShortlistIndex = -1;
        public string[] specialCharacters = {"à", "á", "â", "ã", "ä", "å", "æ", 
                                             "è", "é", "ê", "ë", 
                                             "ì", "í", "î", "ï", 
                                             "ð", "ò", "ó", "ô", "õ", "ö", "ø", 
                                             "ù", "ú", "û", "ü", 
                                             "ń"};
        public string[] normalCharacters = {"a", "a", "a", "a", "a", "a", "ae", 
                                             "e", "e", "e", "e", 
                                             "i", "i", "i", "i", 
                                             "o", "o", "o", "o", "o", "o", "o", 
                                             "u", "u", "u", "u", 
                                             "n"};

        public Color colorFileMenu;
        public Color colorFileMenuText;
        public Color colorTreeView;
        public Color colorTreeViewText;
        public Color colorSettingsPanel;
        public Color colorSettingsPanelText;
        public Color colorDisplayGrid;
        public Color colorDisplayGridText;
        public Color colorSearchFields;
        public Color colorSearchFieldsText;
        public Color colorProfileFields1;
        public Color colorProfileFields2;
        public Color colorProfileFieldsText;
        public Color colorProfileFieldsOnEdit;
        public Color colorProfileFieldsOnEditText;
        public Color colorMain;
        public Color colorMainText;
        public Color colorLowAttribute;
        public Color colorMediumAttribute;
        public Color colorGoodAttribute;
        public Color colorExcellentAttribute;
        public Color colorFreePlayer;
        public Color colorLoanedPlayer;
        public Color colorCoContractedPlayer;
        public Color colorEUMemberPlayer;
        public Color colorFreeStaff;
        public Color colorNationalStaff;
        public Color colorNationalTeam;
        public Color colorToolbar;
        public Color colorTreeViewToolbar;
        public Color colorGameDateText;
        public Color colorGrid;
        public Color colorPanels;
        public Color colorProfileHeaders;
        public Color colorGroupBoxes;
        public Color colorBorders;
        public int alphaGroupBoxes;
        public Font themeFont;
        public Font themeFontBold;
        private SolidBrush treeViewBrush;
        private SolidBrush treeViewTextBrush;
        private SolidBrush DisplayGridBrush;
        private SolidBrush GridBrush;

        public const int MaxPlayerAttributes = 134;
        public const int MaxStaffAttributes = 118;

        public MainForm(bool loadScout)
        {
            InitializeComponent();
            init(loadScout);
        }

        internal void init(bool loadScout)
        {
            playersDataTable = new DataTable();
            staffDataTable = new DataTable();
            teamsDataTable = new DataTable();
            shortlistDataTable = new DataTable();
            playersDataColumnList = new List<DataColumn>();
            staffDataColumnList = new List<DataColumn>();
            teamsDataColumnList = new List<DataColumn>();
            shortlistDataColumnList = new List<DataColumn>();
            playersLoanRowList = new List<int>();
            playersCoOwnRowList = new List<int>();
            playersFreeRowList = new List<int>();
            playersEUMemberRowList = new List<int>();
            shortlistLoanRowList = new List<int>();
            shortlistCoOwnRowList = new List<int>();
            shortlistFreeRowList = new List<int>();
            shortlistEUMemberRowList = new List<int>();
            playersPRTotal = 0;
            staffCRTotal = 0;
            staffFreeRowList = new List<int>();
            staffNationalRowList = new List<int>();
            teamNationalRowList = new List<int>();
            //playersIDList = new Hashtable();
            shortlistIDList = new Hashtable();
            treeViewBrush = new SolidBrush(Color.Black);
            treeViewTextBrush = new SolidBrush(Color.Black);
            DisplayGridBrush = new SolidBrush(Color.Black);
            GridBrush = new SolidBrush(Color.Black);
            aboutForm = new AboutForm();
            loadingForm = new LoadingForm(this);
            donationForm = new DonationForm();
            preferencesForm = new PreferencesForm(this);
            if (loadScout)
                miniScoutForm = new MiniScoutForm(this);
            preferencesForm.init();
            nationalTeam = new Hashtable();
            nationalTeam.Add("African", 0);
            nationalTeam.Add("Asian", 1);
            nationalTeam.Add("European", 2);
            nationalTeam.Add("North American", 3);
            nationalTeam.Add("Oceanic", 4);
            nationalTeam.Add("South American", 5);
            int i = -1;
            EUcountries = new Hashtable();
            EUcountries.Add("Austria", ++i);
            EUcountries.Add("Belgium", ++i);
            EUcountries.Add("Bulgaria", ++i);
            EUcountries.Add("Cyprus", ++i);
            EUcountries.Add("Czech Republic", ++i);
            EUcountries.Add("Denmark", ++i);
            EUcountries.Add("England", ++i);
            EUcountries.Add("Estonia", ++i);
            EUcountries.Add("Finland", ++i);
            EUcountries.Add("France", ++i);
            EUcountries.Add("Germany", ++i);
            EUcountries.Add("Greece", ++i);
            EUcountries.Add("Hungary", ++i);
            EUcountries.Add("Ireland", ++i);
            EUcountries.Add("Italy", ++i);
            EUcountries.Add("Latvia", ++i);
            EUcountries.Add("Lithuania", ++i);
            EUcountries.Add("Luxembourg", ++i);
            EUcountries.Add("Malta", ++i);
            EUcountries.Add("Holland", ++i);
            EUcountries.Add("Poland", ++i);
            EUcountries.Add("Portugal", ++i);
            EUcountries.Add("Romania", ++i);
            EUcountries.Add("Slovakia", ++i);
            EUcountries.Add("Slovenia", ++i);
            EUcountries.Add("Spain", ++i);
            EUcountries.Add("Sweden", ++i);
            reset();

            this.PlayerProfilePanel.BackgroundImage = global::FMScout.Properties.Resources.background1;
            this.PlayerProfilePanel.BackgroundImageLayout = ImageLayout.Stretch;

            this.StaffProfilePanel.BackgroundImage = global::FMScout.Properties.Resources.background1;
            this.StaffProfilePanel.BackgroundImageLayout = ImageLayout.Stretch;

            this.TeamProfilePanel.BackgroundImage = global::FMScout.Properties.Resources.background1;
            this.TeamProfilePanel.BackgroundImageLayout = ImageLayout.Stretch;

            this.PlayersSearchAttributesPanel.BackgroundImage = global::FMScout.Properties.Resources.background3;
            this.PlayersSearchAttributesPanel.BackgroundImageLayout = ImageLayout.Stretch;

            this.StaffSearchAttributesPanel.BackgroundImage = global::FMScout.Properties.Resources.background3;
            this.StaffSearchAttributesPanel.BackgroundImageLayout = ImageLayout.Stretch;
        }

        internal void reset()
        {
            // hide panels
            this.PlayersColumnsPanel.Visible = false;
            this.StaffColumnsPanel.Visible = false;
            this.TeamsColumnsPanel.Visible = false;
            this.ShortlistColumnsPanel.Visible = false;

            // hide editing buttons
            this.EditingToolStripSeparator.Enabled = false;
            this.ProfileSaveEditingToolStrip.Enabled = false;
            this.ProfileCancelEditingToolStrip.Enabled = false;

            this.PlayerProfileHealButton.Enabled = false;
            this.TeamProfileHealTeamButton.Enabled = false;

            this.shortlistToolStripMenuItem.Enabled = false;
            this.ImportShortlistToolStrip.Enabled = false;
            this.ViewShortlistToolStrip.Enabled = false;

            this.MiniScoutButton.Enabled = false;

            this.ActiveButton.Enabled = false;
            this.LeftPanel.Enabled = false;
            this.searchToolStripMenuItem.Enabled = false;
            this.clearToolStripMenuItem.Enabled = false;
            this.toolsToolStripMenuItem.Enabled = false;
            this.PlayersTabControl.Enabled = false;
            this.StaffTabControl.Enabled = false;
            this.TeamsTabControl.Enabled = false;
            this.ShortlistTabControl.Enabled = false;
            this.PlayerProfileTechnicalSkillsGroupBox.Visible = true;
            this.PlayerProfileGoalkeepingSkillsGroupBox.Visible = false;
            this.PreferencesToolStrip.Enabled = false;

            this.CurrentSearchToolStripButton.Enabled = false;
            this.CurrentViewProfileToolStrip.Enabled = false;
            this.RemoveRowToolStripButton.Enabled = false;
            this.EditingToolStripSeparator.Enabled = false;
            this.CurrentFieldsClearToolStripButton.Enabled = false;
            this.CurrentTableClearToolStripButton.Enabled = false;
            this.profileViewToolStripMenuItem.Enabled = false;
            this.CurrentViewProfileToolStrip.Enabled = false;
            this.addToShortlistToolStripMenuItem.Enabled = false;
            this.AddToShortlistToolStrip.Enabled = false;
            this.exportShortlistToolStripMenuItem.Enabled = false;
            this.exportSelectedToolStripMenuItem.Enabled = false;
            this.ExportShortlistAllToolStrip.Enabled = false;
            this.ExportShortlistSelectedToolStrip.Enabled = false;

            this.AddToShotlistToolStripContextMenuItem.Visible = false;
            this.ViewProfileContextMenuItem.Visible = false;
            this.RemoveRowToolStripContextMenuItem.Visible = false;

            this.CurrentGameDateLabel.Enabled = false;
            this.CurrentGameDateLabel.Text = "Game Status: Not Loaded";
            this.SettingsLabel.Enabled = false;
            this.SettingsLabel.Text = infoText(false);
        }

        private void loadFM2009ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadFM();
        }

        internal bool loadMiniScoutFM()
        {
            if (fm != null) fm.Dispose();
            fm = new FMContext();
            if (fm.LoadGame())
            {
                fm.LoadData();
                if (allClubs == null)
                    allClubs = new Hashtable();
                else
                    allClubs.Clear();
                foreach (Team team in fm.Teams)
                {
                    if (!allClubs.Contains(team.Club.ID))
                        allClubs.Add(team.Club.ID, team.Club);
                }

                if (playersPRID == null)
                    playersPRID = new Hashtable(fm.Players.Count);
                else
                    playersPRID.Clear();

                if (staffCRID == null)
                    staffCRID = new Hashtable(fm.NonPlayingStaff.Count);
                else
                    staffCRID.Clear();
                return true;
            }
            return false;
        }

        internal void loadFM()
        {
            this.loadingForm.Location = new Point(this.Left + (this.Width / 2 - this.loadingForm.Width / 2), this.Top + (this.Height / 2 - this.loadingForm.Height / 2));
            this.loadingForm.startLoading();
            this.Enabled = false;

            if (fm != null) fm.Dispose();
            fm = new FMContext();
            if (fm.LoadGame())
            {
                fm.LoadData();
                this.loadingForm.setLoading("\r\nSuccessfully Loaded FM 2009 " + fm.GetCurrentFMVersion());
                this.MiniScoutButton.Enabled = true;
                this.LeftPanel.Enabled = true;
                this.ActiveButton.Enabled = true;
                this.searchToolStripMenuItem.Enabled = true;
                this.profileViewToolStripMenuItem.Enabled = false;
                this.clearToolStripMenuItem.Enabled = true;
                this.toolsToolStripMenuItem.Enabled = true;
                this.shortlistToolStripMenuItem.Enabled = true;
                this.ImportShortlistToolStrip.Enabled = true;
                this.PlayersTabControl.Enabled = true;
                this.StaffTabControl.Enabled = true;
                this.TeamsTabControl.Enabled = true;
                this.ShortlistTabControl.Enabled = true;
                this.TeamProfileListPlayersButton.Enabled = false;
                this.TeamProfileHealTeamButton.Enabled = false;
                this.PlayerProfileSelectSkillsButton.Enabled = false;
                this.PlayerProfileHealButton.Enabled = false;
                this.PreferencesToolStrip.Enabled = true;
                this.ClearAllToolStrip.Enabled = true;
                this.CurrentGameDateLabel.Enabled = true;
                this.CurrentGameDateLabel.Text = "Current Game Date: " + fm.IngameDate.ToLongDateString();
                this.SettingsLabel.Enabled = true;
                this.PlayersTabControl.Visible = true;
                this.StaffTabControl.Visible = false;
                this.TeamsTabControl.Visible = false;
                this.ShortlistTabControl.Visible = false;
                this.PlayerProfileMovePanel.Enabled = false;
                this.StaffProfileMovePanel.Enabled = false;
                this.TeamProfileMovePanel.Enabled = false;
                this.treeView.SelectedNode = this.treeView.TopNode;
                clearPlayerProfileTab();
                clearStaffProfileTab();
                clearTeamProfileTab();

                string activeTooltip = "- Shows player when FM screen shows a player profile\r\n";
                activeTooltip += "- Shows staff when FM screen shows a staff profile\r\n";
                activeTooltip += "- Shows team when FM screen shows a team information screen\r\n";
                this.ActiveGameObjectToolTip.ToolTipTitle = "Current Game Screen";
                this.ActiveGameObjectToolTip.SetToolTip(this.ActiveButton, activeTooltip);

                string columnsToolTip = "- Right click on the button to open\r\n- Left click on the column box to confirm selection and hide";
                this.ColumnsCheckedListToolTip.ToolTipTitle = "Hint";
                this.ColumnsCheckedListToolTip.SetToolTip(this.PlayersSelectColumnsButton, columnsToolTip);
                this.ColumnsCheckedListToolTip.SetToolTip(this.StaffSelectColumnsButton, columnsToolTip);
                this.ColumnsCheckedListToolTip.SetToolTip(this.TeamsSelectColumnsButton, columnsToolTip);
                this.ColumnsCheckedListToolTip.SetToolTip(this.ShortlistSelectColumnsButton, columnsToolTip);

                this.treeView.CollapseAll();
                this.playersDataTable.Rows.Clear();
                this.PlayersDisplayGridView.DataSource = this.playersDataTable;
                this.PlayersResultLabel.Text = "";
                this.staffDataTable.Rows.Clear();
                this.StaffDisplayGridView.DataSource = this.staffDataTable;
                this.StaffResultLabel.Text = "";
                this.teamsDataTable.Rows.Clear();
                this.TeamsDisplayGridView.DataSource = this.teamsDataTable;
                this.TeamsResultLabel.Text = "";
                this.shortlistDataTable.Rows.Clear();
                this.ShortlistDisplayGridView.DataSource = this.shortlistDataTable;
                this.ShortlistResultLabel.Text = "";
                clearAllFields();

                if (allClubs == null)
                    allClubs = new Hashtable();
                else
                    allClubs.Clear();
                foreach (Team team in fm.Teams)
                {
                    if (!allClubs.Contains(team.Club.ID))
                        allClubs.Add(team.Club.ID, team.Club);
                }

                if (playersPRID == null)
                    playersPRID = new Hashtable(fm.Players.Count);
                else
                    playersPRID.Clear();

                if (staffCRID == null)
                    staffCRID = new Hashtable(fm.NonPlayingStaff.Count);
                else
                    staffCRID.Clear();

                setLabels();

                this.loadingForm.finishLoading(true);
            }
            else
            {
                this.treeView.CollapseAll();
                this.playersDataTable.Rows.Clear();
                this.PlayersDisplayGridView.DataSource = this.playersDataTable;
                this.PlayersResultLabel.Text = "";
                this.staffDataTable.Rows.Clear();
                this.StaffDisplayGridView.DataSource = this.staffDataTable;
                this.StaffResultLabel.Text = "";
                this.teamsDataTable.Rows.Clear();
                this.TeamsDisplayGridView.DataSource = this.teamsDataTable;
                this.TeamsResultLabel.Text = "";
                this.shortlistDataTable.Rows.Clear();
                this.ShortlistDisplayGridView.DataSource = this.shortlistDataTable;
                this.ShortlistResultLabel.Text = "";
                clearAllFields();
                reset();
                DateTime timerStart = DateTime.Now;
                int elapsed = 0;
                DateTime timerEnd;
                TimeSpan t;
                while (elapsed < 6)
                {
                    this.loadingForm.setLoading("Error Loading FM 2009\r\nMake sure you have a new or loaded game of FM 2009 up and running\r\n\r\nClosing in " + (5 - elapsed) + " seconds");
                    Application.DoEvents();
                    timerEnd = DateTime.Now;
                    t = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
                    elapsed = t.Seconds;
                }
                this.loadingForm.finishLoading(false);
            }
        }

        public void setLanguage(string language)
        {
            if (language.Equals("English"))
            {
                this.languageLabel.Image = global::FMScout.Properties.Resources.England;
            }
        }

        public void setThemes(PreferencesForm.Theme theme)
        {
            int i = -1;
            colorFileMenu = theme.color[++i];
            colorFileMenuText = theme.color[++i];
            colorTreeView = theme.color[++i];
            colorTreeViewText = theme.color[++i];
            colorSettingsPanel = theme.color[++i];
            colorSettingsPanelText = theme.color[++i];
            colorDisplayGrid = theme.color[++i];
            colorDisplayGridText = theme.color[++i];
            colorSearchFields = theme.color[++i];
            colorSearchFieldsText = theme.color[++i];
            colorProfileFields1 = theme.color[++i];
            colorProfileFields2 = theme.color[++i];
            colorProfileFieldsText = theme.color[++i];
            colorProfileFieldsOnEdit = theme.color[++i];
            colorProfileFieldsOnEditText = theme.color[++i];
            colorMain = theme.color[++i];
            colorMainText = theme.color[++i];
            colorLowAttribute = theme.color[++i];
            colorMediumAttribute = theme.color[++i];
            colorGoodAttribute = theme.color[++i];
            colorExcellentAttribute = theme.color[++i];
            colorFreePlayer = theme.color[++i];
            colorLoanedPlayer = theme.color[++i];
            colorCoContractedPlayer = theme.color[++i];
            colorEUMemberPlayer = theme.color[++i];
            colorFreeStaff = theme.color[++i];
            colorNationalStaff = theme.color[++i];
            colorNationalTeam = theme.color[++i];
            colorToolbar = theme.color[++i];
            colorTreeViewToolbar = theme.color[++i];
            colorGameDateText = theme.color[++i];
            colorGrid = theme.color[++i];
            colorPanels = theme.color[++i];
            colorProfileHeaders = theme.color[++i];
            colorGroupBoxes = theme.color[++i];
            colorBorders = theme.color[++i];
            this.alphaGroupBoxes = theme.alpha;

            this.themeFont = theme.font;
            this.themeFontBold = new Font(theme.font, FontStyle.Bold);
            this.treeViewBrush.Color = colorTreeView;
            this.treeViewTextBrush.Color = colorTreeViewText;
            this.DisplayGridBrush.Color = colorDisplayGrid;
            this.GridBrush.Color = colorGrid;
            this.Font = theme.font;
            this.BackColor = colorMain;
            this.ForeColor = colorMainText;
            this.MainMenuStrip.Font = theme.font;
            this.loadingForm.Font = theme.font;
            this.loadingForm.ForeColor = colorMainText;
            this.aboutForm.Font = theme.font;
            this.aboutForm.ForeColor = colorMainText;
            this.donationForm.Font = theme.font;
            this.donationForm.ForeColor = colorMainText;

            if (miniScoutForm != null)
                this.miniScoutForm.setColors();
            this.preferencesForm.setColors();
            this.loadingForm.BackColor = colorMain;
            this.aboutForm.BackColor = colorMain;
            this.donationForm.BackColor = colorMain;

            int alphaPanel = 80;
            int alphaButton = 40;
            this.PlayerProfileMovePanel.BackColor = Color.FromArgb(alphaPanel, colorPanels);
            this.PlayerProfileMoveLeftButton.BackColor = Color.FromArgb(alphaButton, colorPanels);
            this.PlayerProfileMoveRightButton.BackColor = Color.FromArgb(alphaButton, colorPanels);
            this.StaffProfileMovePanel.BackColor = Color.FromArgb(alphaPanel, colorPanels);
            this.StaffProfileMoveLeftButton.BackColor = Color.FromArgb(alphaButton, colorPanels);
            this.StaffProfileMoveRightButton.BackColor = Color.FromArgb(alphaButton, colorPanels);
            this.TeamProfileMovePanel.BackColor = Color.FromArgb(alphaPanel, colorPanels);
            this.TeamProfileMoveLeftButton.BackColor = Color.FromArgb(alphaButton, colorPanels);
            this.TeamProfileMoveRightButton.BackColor = Color.FromArgb(alphaButton, colorPanels);

            this.PlayersTabControl.Font = theme.font;
            this.PlayersTabControl.ForeColor = colorMainText;
            this.StaffTabControl.Font = theme.font;
            this.StaffTabControl.ForeColor = colorMainText;
            this.TeamsTabControl.Font = theme.font;
            this.TeamsTabControl.ForeColor = colorMainText;
            this.ShortlistTabControl.Font = theme.font;
            this.ShortlistTabControl.ForeColor = colorMainText;

            this.MainMenuStrip.BackColor = colorFileMenu;
            this.MainMenuStrip.ForeColor = colorFileMenuText;
            this.CurrentGameDateLabel.BackColor = colorFileMenu;
            this.CurrentGameDateLabel.ForeColor = colorGameDateText;
            this.mainToolStrip.BackColor = colorToolbar;
            this.treeViewToolStrip.BackColor = colorTreeViewToolbar;
            this.treeView.BackColor = colorTreeView;
            this.treeView.ForeColor = colorTreeViewText;
            this.ActiveButton.BackColor = colorMain;
            this.SettingsLabel.BackColor = colorSettingsPanel;
            this.SettingsLabel.ForeColor = colorSettingsPanelText;
            this.PlayersTabControl.BackColor = colorMain;
            this.PlayersTabPage.BackColor = colorMain;
            this.PlayersTabPagePanel.BackColor = colorPanels;
            this.PlayersDisplayGridView.BackgroundColor = colorDisplayGrid;
            this.PlayersDisplayGridView.DefaultCellStyle.BackColor = colorDisplayGrid;
            this.PlayersDisplayGridView.DefaultCellStyle.ForeColor = colorDisplayGridText;
            this.PlayersDisplayGridView.GridColor = colorGrid;
            this.PlayersResultLabel.BackColor = colorGroupBoxes;
            this.PlayersSearchAttributes.BackColor = colorMain;
            this.PlayersSearchAttributesPanel.BackColor = colorPanels;
            this.PlayerProfile.BackColor = colorMain;
            this.PlayerProfilePanel.BackColor = colorPanels;
            this.StaffTabControl.BackColor = colorMain;
            this.StaffTabPage.BackColor = colorMain;
            this.StaffTabPagePanel.BackColor = colorPanels;
            this.StaffDisplayGridView.BackgroundColor = colorDisplayGrid;
            this.StaffDisplayGridView.DefaultCellStyle.BackColor = colorDisplayGrid;
            this.StaffDisplayGridView.DefaultCellStyle.ForeColor = colorDisplayGridText;
            this.StaffDisplayGridView.GridColor = colorGrid;
            this.StaffResultLabel.BackColor = colorGroupBoxes;
            this.StaffSearchAttributes.BackColor = colorMain;
            this.StaffSearchAttributesPanel.BackColor = colorPanels;
            this.StaffProfile.BackColor = colorMain;
            this.StaffProfilePanel.BackColor = colorPanels;
            this.TeamsTabControl.BackColor = colorMain;
            this.TeamsTabPage.BackColor = colorMain;
            this.TeamsTabPagePanel.BackColor = colorPanels;
            this.TeamsDisplayGridView.BackgroundColor = colorDisplayGrid;
            this.TeamsDisplayGridView.DefaultCellStyle.BackColor = colorDisplayGrid;
            this.TeamsDisplayGridView.DefaultCellStyle.ForeColor = colorDisplayGridText;
            this.TeamsDisplayGridView.GridColor = colorGrid;
            this.TeamsResultLabel.BackColor = colorGroupBoxes;
            this.TeamProfile.BackColor = colorMain;
            this.TeamProfilePanel.BackColor = colorPanels;
            this.ShortlistTabControl.BackColor = colorMain;
            this.ShortlistTabPage.BackColor = colorMain;
            this.ShortlistTabPagePanel.BackColor = colorPanels;
            this.ShortlistDisplayGridView.BackgroundColor = colorDisplayGrid;
            this.ShortlistDisplayGridView.DefaultCellStyle.BackColor = colorDisplayGrid;
            this.ShortlistDisplayGridView.DefaultCellStyle.ForeColor = colorDisplayGridText;
            this.ShortlistDisplayGridView.GridColor = colorGrid;
            this.ShortlistResultLabel.BackColor = colorGroupBoxes;

            colorMainSet(ref this.PlayersColumnsPanel);
            colorMainSet(ref this.StaffColumnsPanel);
            colorMainSet(ref this.TeamsColumnsPanel);
            colorMainSet(ref this.ShortlistColumnsPanel);

            colorMainSet(ref this.PlayersColumnsCheckedListBox);
            colorMainSet(ref this.StaffColumnsCheckedListBox);
            colorMainSet(ref this.TeamsColumnsCheckedListBox);
            colorMainSet(ref this.ShortlistColumnsCheckedListBox);

            colorGroupBoxMain(ref this.PlayersSearchGroupBox);
            colorGroupBoxMain(ref this.PlayersSearch1GroupBox);
            colorGroupBoxMain(ref this.PlayersSearch2GroupBox);
            colorGroupBoxMain(ref this.PlayersSearch3GroupBox);
            colorGroupBoxMain(ref this.PlayersResultsGroupBox);
            colorProfileGroupBox(ref this.PlayerTechnicalSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerPhysicalSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerMentalSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerGoalkeepingSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerHiddenSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerMentalTraitsSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerProfilePersonalDetailsGroupBox);
            colorProfileGroupBox(ref this.PlayerProfileContractDetailsGroupBox);
            colorProfileGroupBox(ref this.PlayerProfileOtherGroupBox);
            colorProfileGroupBox(ref this.PlayerProfileMentalTraitsSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerProfileReputationGroupBox);
            colorProfileGroupBox(ref this.PlayerProfileTechnicalSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerProfilePhysicalSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerProfileMentalSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerProfileGoalkeepingSkillsGroupBox);
            colorProfileGroupBox(ref this.PlayerProfileHiddenSkillsGroupBox);
            colorGroupBoxMain(ref this.StaffSearchGroupBox);
            colorGroupBoxMain(ref this.StaffSearch1GroupBox);
            colorGroupBoxMain(ref this.StaffSearch2GroupBox);
            colorGroupBoxMain(ref this.StaffSearch3GroupBox);
            colorGroupBoxMain(ref this.StaffResultsGroupBox);
            colorProfileGroupBox(ref this.StaffCoachingSkillsGroupBox);
            colorProfileGroupBox(ref this.StaffMentalSkillsGroupBox);
            colorProfileGroupBox(ref this.StaffTacticalSkillsGroupBox);
            colorProfileGroupBox(ref this.StaffNonTacticalSkillsGroupBox);
            colorProfileGroupBox(ref this.StaffChairmanSkillsGroupBox);
            colorProfileGroupBox(ref this.StaffProfilePersonalDetailsGroupBox);
            colorProfileGroupBox(ref this.StaffProfileContractDetailsGroupBox);
            colorProfileGroupBox(ref this.StaffProfileCoachingSkillsGroupBox);
            colorProfileGroupBox(ref this.StaffProfileMentalSkillsGroupBox);
            colorProfileGroupBox(ref this.StaffProfileTacticalSkillsGroupBox);
            colorProfileGroupBox(ref this.StaffProfileNonTacticalSkillsGroupBox);
            colorProfileGroupBox(ref this.StaffProfileChairmanSkillsGroupBox);
            colorProfileGroupBox(ref this.StaffProfileReputationGroupBox);
            colorProfileGroupBox(ref this.StaffProfileBestRatingsGroupBox);
            colorGroupBoxMain(ref this.TeamsSearchGroupBox);
            colorGroupBoxMain(ref this.TeamsSearch1GroupBox);
            colorGroupBoxMain(ref this.TeamsSearch2GroupBox);
            colorGroupBoxMain(ref this.TeamsResultsGroupBox);
            colorProfileGroupBox(ref this.TeamProfileGeneralDetailsGroupBox);
            colorProfileGroupBox(ref this.TeamProfileFinanceDetailsGroupBox);
            colorProfileGroupBox(ref this.TeamProfileStadiumDetailsGroupBox);
            colorGroupBoxMain(ref this.ShortlistResultsGroupBox);

            setPlayersTabColor();
            setStaffTabColor();
            setTeamTabColor();

            updateChanges(true);
        }

        internal void colorGroupBoxPreferences(ref Controls.GroupBoxAdvanced groupbox)
        {
            groupbox.BackColor = Color.Transparent;
            groupbox.CustomGroupBoxColor = colorProfileHeaders;
            groupbox.BackgroundColor = Color.FromArgb(alphaGroupBoxes, colorProfileFields1);
            groupbox.BackgroundColor2 = Color.FromArgb(alphaGroupBoxes, colorProfileFields2);
            groupbox.ForeColor = colorMainText;
            groupbox.BorderColor = colorBorders;
        }

        internal void colorCheckedListBoxPreferences(ref CheckedListBox checkedlistbox)
        {
            checkedlistbox.BackColor = colorSearchFields;
            checkedlistbox.ForeColor = colorSearchFieldsText;
        }

        internal void colorNumericUpDownPreferences(ref NumericUpDown numericupdown)
        {
            numericupdown.BackColor = colorSearchFields;
            numericupdown.ForeColor = colorSearchFieldsText;
        }

        internal void colorTrackBarPreferences(ref TrackBar trackbar)
        {
            trackbar.BackColor = colorGroupBoxes;
            trackbar.ForeColor = colorSearchFieldsText;
        }

        internal void colorGroupBoxMain(ref Controls.GroupBoxAdvanced groupbox)
        {
            groupbox.BackColor = Color.Transparent;
            groupbox.CustomGroupBoxColor = colorProfileHeaders;
            groupbox.BackgroundColor = colorGroupBoxes;
            groupbox.ForeColor = colorMainText;
            groupbox.BorderColor = colorBorders;
        }

        internal void colorProfileGroupBox(ref Controls.GroupBoxAdvanced groupbox)
        {
            groupbox.BackColor = Color.Transparent;
            groupbox.CustomGroupBoxColor = colorProfileHeaders;
            groupbox.BackgroundColor = Color.FromArgb(alphaGroupBoxes, colorProfileFields1);
            groupbox.BackgroundColor2 = Color.FromArgb(alphaGroupBoxes, colorProfileFields2);
            groupbox.ForeColor = colorMainText;
            groupbox.BorderColor = colorBorders;
        }

        private void setPlayersTabColor()
        {
            colorMainSet(ref this.PlayersSearchButton);
            colorMainSet(ref this.PlayersWonderkidButton);

            // players search
            colorMainSet(ref this.PlayerRegionComboBox);
            colorMainSet(ref this.PlayerRegionLabel);
            colorMainSet(ref this.PlayerFullNamelabel);
            colorMainSet(ref this.PlayerFullNameTextBox);
            colorMainSet(ref this.PlayerNationLabel);
            colorMainSet(ref this.PlayerNationTextBox);
            colorMainSet(ref this.PlayerClubLabel);
            colorMainSet(ref this.PlayerClubTextBox);
            colorMainSet(ref this.PlayerOwnerShipComboBox);
            colorMainSet(ref this.PlayerOwnershipLabel);
            colorMainSet(ref this.PlayerContractStatusComboBox);
            colorMainSet(ref this.PlayerContractStatusLabel);
            colorMainSet(ref this.PlayerWageLabel);
            colorMainSet(ref this.PlayerWageMinNumericUpDown);
            colorMainSet(ref this.PlayerWageMaxNumericUpDown);
            colorMainSet(ref this.PlayerWageDashLabel);
            colorMainSet(ref this.PlayerPrefFootLabel);
            colorMainSet(ref this.PlayerPrefFootComboBox);

            colorMainSet(ref this.PlayersAgeDashLabel);
            colorMainSet(ref this.PlayerCurrentValueMinNumericUpDown);
            colorMainSet(ref this.PlayersCADashLabel);
            colorMainSet(ref this.PlayerValueLabel);
            colorMainSet(ref this.PlayersPADashLabel);
            colorMainSet(ref this.PlayerSaleValueLabel);
            colorMainSet(ref this.PlayerAgeMinNumericUpDown);
            colorMainSet(ref this.PlayerAgeLabel);
            colorMainSet(ref this.PlayerrsValueDashLabel);
            colorMainSet(ref this.PlayerCALabel);
            colorMainSet(ref this.PlayersSaleValueDashLabel);
            colorMainSet(ref this.PlayerPALabel);
            colorMainSet(ref this.PlayerCurrentValueMaxNumericUpDown);
            colorMainSet(ref this.PlayerPAMinNumericUpDown);
            colorMainSet(ref this.PlayerSaleValueMaxNumericUpDown);
            colorMainSet(ref this.PlayerCAMaxNumericUpDown);
            colorMainSet(ref this.PlayerSaleValueMinNumericUpDown);
            colorMainSet(ref this.PlayerCAMinNumericUpDown);
            colorMainSet(ref this.PlayerAgeMaxNumericUpDown);
            colorMainSet(ref this.PlayerPAMaxNumericUpDown);

            colorMainSet(ref this.PlayerEULabel);
            colorMainSet(ref this.PlayerEUComboBox);
            colorMainSet(ref this.PlayerRegenLabel);
            colorMainSet(ref this.PlayerRegenComboBox);
            colorMainSet(ref this.PlayerSideLabel);
            colorMainSet(ref this.PlayerPositionLabel);
            colorMainSet(ref this.PlayerPosition1CheckedListBox);
            colorMainSet(ref this.PlayerPosition2CheckedListBox);

            colorMainSet(ref this.label296);
            colorMainSet(ref this.PlayerBestPositionComboBox);
            colorMainSet(ref this.PlayerPositionalRatingMinNumericUpDown);
            colorMainSet(ref this.PlayerBestPositionLabel);
            colorMainSet(ref this.PlayerPositionalRatingLabel);
            colorMainSet(ref this.PlayerPositionalRatingMaxNumericUpDown);

            // players attributes
            // technical
            colorInfo(ref this.PlayerTechniqueLabel);
            colorInfo(ref this.label76);
            colorMainSet(ref this.PlayerTechniqueMinNumericUpDown);
            colorMainSet(ref this.PlayerTechniqueMaxNumericUpDown);
            colorInfo(ref this.PlayerTacklingLabel);
            colorInfo(ref this.label5);
            colorMainSet(ref this.PlayerTacklingMinNumericUpDown);
            colorMainSet(ref this.PlayerTacklingMaxNumericUpDown);
            colorInfo(ref this.PlayerPenaltyTakingLabel);
            colorInfo(ref this.label13);
            colorMainSet(ref this.PlayerPenaltyTakingMinNumericUpDown);
            colorMainSet(ref this.PlayerPenaltyTakingMaxNumericUpDown);
            colorInfo(ref this.PlayerPassingLabel);
            colorInfo(ref this.label21);
            colorMainSet(ref this.PlayerPassingMinNumericUpDown);
            colorMainSet(ref this.PlayerPassingMaxNumericUpDown);
            colorInfo(ref this.PlayerMarkingLabel);
            colorInfo(ref this.label25);
            colorMainSet(ref this.PlayerMarkingMinNumericUpDown);
            colorMainSet(ref this.PlayerMarkingMaxNumericUpDown);
            colorInfo(ref this.PlayerLongThrowLabel);
            colorInfo(ref this.label29);
            colorMainSet(ref this.PlayerLongThrowsMinNumericUpDown);
            colorMainSet(ref this.PlayerLongThrowsMaxNumericUpDown);
            colorInfo(ref this.PlayerLongShotsLabel);
            colorInfo(ref this.label33);
            colorMainSet(ref this.PlayerLongShotsMinNumericUpDown);
            colorMainSet(ref this.PlayerLongShotsMaxNumericUpDown);
            colorInfo(ref this.PlayerHeadingLabel);
            colorInfo(ref this.label37);
            colorMainSet(ref this.PlayerHeadingMinNumericUpDown);
            colorMainSet(ref this.PlayerHeadingMaxNumericUpDown);
            colorInfo(ref this.PlayerFreeKicksLabel);
            colorInfo(ref this.label55);
            colorMainSet(ref this.PlayerFreeKicksMinNumericUpDown);
            colorMainSet(ref this.PlayerFreeKicksMaxNumericUpDown);
            colorInfo(ref this.PlayerFirstTouchLabel);
            colorInfo(ref this.label59);
            colorMainSet(ref this.PlayerFirstTouchMinNumericUpDown);
            colorMainSet(ref this.PlayerFirstTouchMaxNumericUpDown);
            colorInfo(ref this.PlayerFinishingLabel);
            colorInfo(ref this.label68);
            colorMainSet(ref this.PlayerFinishingMinNumericUpDown);
            colorMainSet(ref this.PlayerFinishingMaxNumericUpDown);
            colorInfo(ref this.PlayerDribblingLabel);
            colorInfo(ref this.label70);
            colorMainSet(ref this.PlayerDribblingMinNumericUpDown);
            colorMainSet(ref this.PlayerDribblingMaxNumericUpDown);
            colorInfo(ref this.PlayerCrossingLabel);
            colorInfo(ref this.label72);
            colorMainSet(ref this.PlayerCrossingMinNumericUpDown);
            colorMainSet(ref this.PlayerCrossingMaxNumericUpDown);
            colorInfo(ref this.PlayerCornersLabel);
            colorInfo(ref this.label74);
            colorMainSet(ref this.PlayerCornersMinNumericUpDown);
            colorMainSet(ref this.PlayerCornersMaxNumericUpDown);

            // physical
            colorInfo(ref this.PlayerRightFootLabel);
            colorInfo(ref this.label90);
            colorMainSet(ref this.PlayerRightFootMinNumericUpDown);
            colorMainSet(ref this.PlayerRightFootMaxNumericUpDown);
            colorInfo(ref this.PlayerLeftFootLabel);
            colorInfo(ref this.label92);
            colorMainSet(ref this.PlayerLeftFootMinNumericUpDown);
            colorMainSet(ref this.PlayerLeftFootMaxNumericUpDown);
            colorInfo(ref this.PlayerStrengthLabel);
            colorInfo(ref this.label94);
            colorMainSet(ref this.PlayerStrengthMinNumericUpDown);
            colorMainSet(ref this.PlayerStrengthMaxNumericUpDown);
            colorInfo(ref this.PlayerStaminaLabel);
            colorInfo(ref this.label96);
            colorMainSet(ref this.PlayerStaminaMinNumericUpDown);
            colorMainSet(ref this.PlayerStaminaMaxNumericUpDown);
            colorInfo(ref this.PlayerPaceLabel);
            colorInfo(ref this.label98);
            colorMainSet(ref this.PlayerPaceMinNumericUpDown);
            colorMainSet(ref this.PlayerPaceMaxNumericUpDown);
            colorInfo(ref this.PlayerNaturalFitnessLabel);
            colorInfo(ref this.label100);
            colorMainSet(ref this.PlayerNaturalFitnessMinNumericUpDown);
            colorMainSet(ref this.PlayerNaturalFitnessMaxNumericUpDown);
            colorInfo(ref this.PlayerJumpingLabel);
            colorInfo(ref this.label102);
            colorMainSet(ref this.PlayerJumpingMinNumericUpDown);
            colorMainSet(ref this.PlayerJumpingMaxNumericUpDown);
            colorInfo(ref this.PlayerBalanceLabel);
            colorInfo(ref this.label104);
            colorMainSet(ref this.PlayerBalanceMinNumericUpDown);
            colorMainSet(ref this.PlayerBalanceMaxNumericUpDown);
            colorInfo(ref this.PlayerAgilityLabel);
            colorInfo(ref this.label106);
            colorMainSet(ref this.PlayerAgilityMinNumericUpDown);
            colorMainSet(ref this.PlayerAgilityMaxNumericUpDown);
            colorInfo(ref this.PlayerAccelerationLabel);
            colorInfo(ref this.label108);
            colorMainSet(ref this.PlayerAccelerationMinNumericUpDown);
            colorMainSet(ref this.PlayerAccelerationMaxNumericUpDown);

            // mental
            colorInfo(ref this.PlayerWorkRateLabel);
            colorInfo(ref this.label122);
            colorMainSet(ref this.PlayerWorkRateMinNumericUpDown);
            colorMainSet(ref this.PlayerWorkRateMaxNumericUpDown);
            colorInfo(ref this.PlayerTeamworkLabel);
            colorInfo(ref this.label124);
            colorMainSet(ref this.PlayerTeamworkMinNumericUpDown);
            colorMainSet(ref this.PlayerTeamworkMaxNumericUpDown);
            colorInfo(ref this.PlayerPositioningLabel);
            colorInfo(ref this.label126);
            colorMainSet(ref this.PlayerPositioningMinNumericUpDown);
            colorMainSet(ref this.PlayerPositioningMaxNumericUpDown);
            colorInfo(ref this.PlayerOffTheBallLabel);
            colorInfo(ref this.label128);
            colorMainSet(ref this.PlayerOffTheBallMinNumericUpDown);
            colorMainSet(ref this.PlayerOffTheBallMaxNumericUpDown);
            colorInfo(ref this.PlayerInfluenceLabel);
            colorInfo(ref this.label78);
            colorMainSet(ref this.PlayerInfluenceMinNumericUpDown);
            colorMainSet(ref this.PlayerInfluenceMaxNumericUpDown);
            colorInfo(ref this.PlayerFlairLabel);
            colorInfo(ref this.label81);
            colorMainSet(ref this.PlayerFlairMinNumericUpDown);
            colorMainSet(ref this.PlayerFlairMaxNumericUpDown);
            colorInfo(ref this.PlayerDeterminationLabel);
            colorInfo(ref this.label85);
            colorMainSet(ref this.PlayerDeterminationMinNumericUpDown);
            colorMainSet(ref this.PlayerDeterminationMaxNumericUpDown);
            colorInfo(ref this.PlayerDecisionsLabel);
            colorInfo(ref this.label88);
            colorMainSet(ref this.PlayerDecisionsMinNumericUpDown);
            colorMainSet(ref this.PlayerDecisionsMaxNumericUpDown);
            colorInfo(ref this.PlayerCreativityLabel);
            colorInfo(ref this.label110);
            colorMainSet(ref this.PlayerCreativityMinNumericUpDown);
            colorMainSet(ref this.PlayerCreativityMaxNumericUpDown);
            colorInfo(ref this.PlayerConcentrationLabel);
            colorInfo(ref this.label112);
            colorMainSet(ref this.PlayerConcentrationMinNumericUpDown);
            colorMainSet(ref this.PlayerConcentrationMaxNumericUpDown);
            colorInfo(ref this.PlayerComposureLabel);
            colorInfo(ref this.label114);
            colorMainSet(ref this.PlayerComposureMinNumericUpDown);
            colorMainSet(ref this.PlayerComposureMaxNumericUpDown);
            colorInfo(ref this.PlayerBraveryLabel);
            colorInfo(ref this.label116);
            colorMainSet(ref this.PlayerBraveryMinNumericUpDown);
            colorMainSet(ref this.PlayerBraveryMaxNumericUpDown);
            colorInfo(ref this.PlayerAnticipationLabel);
            colorInfo(ref this.label118);
            colorMainSet(ref this.PlayerAnticipationMinNumericUpDown);
            colorMainSet(ref this.PlayerAnticipationMaxNumericUpDown);
            colorInfo(ref this.PlayerAggressionLabel);
            colorInfo(ref this.label120);
            colorMainSet(ref this.PlayerAggressionMinNumericUpDown);
            colorMainSet(ref this.PlayerAggressionMaxNumericUpDown);

            // goalkeeping
            colorInfo(ref this.PlayerThrowingLabel);
            colorInfo(ref this.label149);
            colorMainSet(ref this.PlayerThrowingMinNumericUpDown);
            colorMainSet(ref this.PlayerThrowingMaxNumericUpDown);
            colorInfo(ref this.PlayerTendencyToPunchLabel);
            colorInfo(ref this.label157);
            colorMainSet(ref this.PlayerTendencyToPunchMinNumericUpDown);
            colorMainSet(ref this.PlayerTendencyToPunchMaxNumericUpDown);
            colorInfo(ref this.PlayerRushingOutLabel);
            colorInfo(ref this.label165);
            colorMainSet(ref this.PlayerRushingOutMinNumericUpDown);
            colorMainSet(ref this.PlayerRushingOutMaxNumericUpDown);
            colorInfo(ref this.PlayerReflexesLabel);
            colorInfo(ref this.PlayerProfileTeamWorkLabel3);
            colorMainSet(ref this.PlayerReflexesMinNumericUpDown);
            colorMainSet(ref this.PlayerReflexesMaxNumericUpDown);
            colorInfo(ref this.PlayerOneOnOnesLabel);
            colorInfo(ref this.label180);
            colorMainSet(ref this.PlayerOneOnOnesMinNumericUpDown);
            colorMainSet(ref this.PlayerOneOnOnesMaxNumericUpDown);
            colorInfo(ref this.PlayerKickingLabel);
            colorInfo(ref this.label184);
            colorMainSet(ref this.PlayerKickingMinNumericUpDown);
            colorMainSet(ref this.PlayerKickingMaxNumericUpDown);
            colorInfo(ref this.PlayerHandlingLabel);
            colorInfo(ref this.label188);
            colorMainSet(ref this.PlayerHandlingMinNumericUpDown);
            colorMainSet(ref this.PlayerHandlingMaxNumericUpDown);
            colorInfo(ref this.PlayerEccentricityLabel);
            colorInfo(ref this.label192);
            colorMainSet(ref this.PlayerEccentricityMinNumericUpDown);
            colorMainSet(ref this.PlayerEccentricityMaxNumericUpDown);
            colorInfo(ref this.PlayerCommunicationLabel);
            colorInfo(ref this.label196);
            colorMainSet(ref this.PlayerCommunicationMinNumericUpDown);
            colorMainSet(ref this.PlayerCommunicationMaxNumericUpDown);
            colorInfo(ref this.PlayerCommandOfAreaTextBox);
            colorInfo(ref this.label200);
            colorMainSet(ref this.PlayerCommandOfAreaMinNumericUpDown);
            colorMainSet(ref this.PlayerCommandOfAreaMaxNumericUpDown);
            colorInfo(ref this.PlayerAerialAbilityLabel);
            colorInfo(ref this.label204);
            colorMainSet(ref this.PlayerAerialAbilityMinNumericUpDown);
            colorMainSet(ref this.PlayerAerialAbilityMaxNumericUpDown);

            // mental traits skills
            colorInfo(ref this.PlayerTemperamentLabel);
            colorInfo(ref this.label47);
            colorMainSet(ref this.PlayerTemperamentMinNumericUpDown);
            colorMainSet(ref this.PlayerTemperamentMaxNumericUpDown);
            colorInfo(ref this.PlayerSportsmanshipLabel);
            colorInfo(ref this.label51);
            colorMainSet(ref this.PlayerSportsmanshipMinNumericUpDown);
            colorMainSet(ref this.PlayerSportsmanshipMaxNumericUpDown);
            colorInfo(ref this.PlayerProfessionalismLabel);
            colorInfo(ref this.label57);
            colorMainSet(ref this.PlayerProfessionalismMinNumericUpDown);
            colorMainSet(ref this.PlayerProfessionalismMaxNumericUpDown);
            colorInfo(ref this.PlayerPressureLabel);
            colorInfo(ref this.label9);
            colorMainSet(ref this.PlayerPressureMinNumericUpDown);
            colorMainSet(ref this.PlayerPressureMaxNumericUpDown);
            colorInfo(ref this.PlayerLoyaltyLabel);
            colorInfo(ref this.label23);
            colorMainSet(ref this.PlayerLoyaltyMinNumericUpDown);
            colorMainSet(ref this.PlayerLoyaltyMaxNumericUpDown);
            colorInfo(ref this.PlayerControversyLabel);
            colorInfo(ref this.label31);
            colorMainSet(ref this.PlayerControversyMinNumericUpDown);
            colorMainSet(ref this.PlayerControversyMaxNumericUpDown);
            colorInfo(ref this.PlayerAmbitionLabel);
            colorInfo(ref this.label39);
            colorMainSet(ref this.PlayerAmbitionMinNumericUpDown);
            colorMainSet(ref this.PlayerAmbitionMaxNumericUpDown);
            colorInfo(ref this.PlayerAdaptabilityLabel);
            colorInfo(ref this.label43);
            colorMainSet(ref this.PlayerAdaptabilityMinNumericUpDown);
            colorMainSet(ref this.PlayerAdaptabilityMaxNumericUpDown);

            // hidden
            colorInfo(ref this.PlayerVersatilityLabel);
            colorInfo(ref this.label146);
            colorMainSet(ref this.PlayerVersatilityMinNumericUpDown);
            colorMainSet(ref this.PlayerVersatilityMaxNumericUpDown);
            colorInfo(ref this.PlayerInjuryPronenessLabel);
            colorInfo(ref this.label138);
            colorMainSet(ref this.PlayerInjuryPronenessMinNumericUpDown);
            colorMainSet(ref this.PlayerInjuryPronenessMaxNumericUpDown);
            colorInfo(ref this.PlayerImportantMatchesLabel);
            colorInfo(ref this.label140);
            colorMainSet(ref this.PlayerImportantMatchesMinNumericUpDown);
            colorMainSet(ref this.PlayerImportantMatchesMaxNumericUpDown);
            colorInfo(ref this.PlayerDirtynessLabel);
            colorInfo(ref this.label142);
            colorMainSet(ref this.PlayerDirtynessMinNumericUpDown);
            colorMainSet(ref this.PlayerDirtynessMaxNumericUpDown);
            colorInfo(ref this.PlayerConsistencyLabel);
            colorInfo(ref this.label144);
            colorMainSet(ref this.PlayerConsistencyMinNumericUpDown);
            colorMainSet(ref this.PlayerConsistencyMaxNumericUpDown);
        }

        private void setPlayerProfileColor()
        {
            colorMainSetNonTransp(ref this.PlayerProfileHealButton);
            colorMainSetNonTransp(ref this.PlayerProfileSelectSkillsButton);
            colorInfo(ref this.PlayerProfileShortlistButton);

            // labels
            // personal details
            colorInfo(ref this.PlayerProfileTeamSquadLabel);
            colorInfo(ref this.PlayerProfileBasicWageLabel);
            colorInfo(ref this.PlayerProfileContractStartedLabel);
            colorInfo(ref this.PlayerProfileContractExpiryLabel);
            colorInfo(ref this.PlayerProfileGoalBonusLabel);

            colorInfo(ref this.PlayerProfileLocalReputationLabel);
            colorInfo(ref this.PlayerProfileNationalReputationLabel);
            colorInfo(ref this.PlayerProfileWorldReputationLabel);
            colorInfo(ref this.PlayerProfileAppearanceBonusLabel);
            colorInfo(ref this.PlayerProfileCleanSheetBonusLabel);

            // technical skills
            colorInfo(ref this.PlayerProfileCornersLabel);
            colorInfo(ref this.PlayerProfileCrossingLabel);
            colorInfo(ref this.PlayerProfileDribblingLabel);
            colorInfo(ref this.PlayerProfileFinishingLabel);
            colorInfo(ref this.PlayerProfileFirstTouchLabel);
            colorInfo(ref this.PlayerProfileFreeKicksLabel);
            colorInfo(ref this.PlayerProfileHeadingLabel);
            colorInfo(ref this.PlayerProfileLongShotsLabel);
            colorInfo(ref this.PlayerProfileLongThrowsLabel);
            colorInfo(ref this.PlayerProfileMarkingLabel);
            colorInfo(ref this.PlayerProfilePassingLabel);
            colorInfo(ref this.PlayerProfilePenaltyTakingLabel);
            colorInfo(ref this.PlayerProfileTacklingLabel);
            colorInfo(ref this.PlayerProfileTechniqueLabel);

            // goalkeeping skills
            colorInfo(ref this.PlayerProfileAerialAbilityLabel);
            colorInfo(ref this.PlayerProfileCommandOfAreaLabel);
            colorInfo(ref this.PlayerProfileCommunicationLabel);
            colorInfo(ref this.PlayerProfileEccentricityLabel);
            colorInfo(ref this.PlayerProfileHandlingLabel);
            colorInfo(ref this.PlayerProfileKickingLabel);
            colorInfo(ref this.PlayerProfileOneOnOnesLabel);
            colorInfo(ref this.PlayerProfileReflexesLabel);
            colorInfo(ref this.PlayerProfileRushingOutLabel);
            colorInfo(ref this.PlayerProfileTendencyToPunchLabel);
            colorInfo(ref this.PlayerProfileThrowingLabel);

            // physical skills
            colorInfo(ref this.PlayerProfileAccelerationLabel);
            colorInfo(ref this.PlayerProfileAgilityLabel);
            colorInfo(ref this.PlayerProfileBalanceLabel);
            colorInfo(ref this.PlayerProfileJumpingLabel);
            colorInfo(ref this.PlayerProfileNaturalFitnessLabel);
            colorInfo(ref this.PlayerProfilePaceLabel);
            colorInfo(ref this.PlayerProfileStaminaLabel);
            colorInfo(ref this.PlayerProfileStrengthLabel);

            // mental skills
            colorInfo(ref this.PlayerProfileAggressionLabel);
            colorInfo(ref this.PlayerProfileAnticipationLabel);
            colorInfo(ref this.PlayerProfileBraveryLabel);
            colorInfo(ref this.PlayerProfileComposureLabel);
            colorInfo(ref this.PlayerProfileConcentrationLabel);
            colorInfo(ref this.PlayerProfileCreativityLabel);
            colorInfo(ref this.PlayerProfileDecisionsLabel);
            colorInfo(ref this.PlayerProfileDeterminationLabel);
            colorInfo(ref this.PlayerProfileFlairLabel);
            colorInfo(ref this.PlayerProfileInfluenceLabel);
            colorInfo(ref this.PlayerProfileOffTheBallLabel);
            colorInfo(ref this.PlayerProfilePositioningLabel);
            colorInfo(ref this.PlayerProfileTeamWorkLabel);
            colorInfo(ref this.PlayerProfileWorkRateLabel);

            // hidden skills
            colorInfo(ref this.PlayerProfileConsistencyLabel);
            colorInfo(ref this.PlayerProfileDirtynessLabel);
            colorInfo(ref this.PlayerProfileImportantMatchesLabel);
            colorInfo(ref this.PlayerProfileInjuryPronenessLabel);
            colorInfo(ref this.PlayerProfileVersatilityLabel);

            // mental traits skills
            colorInfo(ref this.PlayerProfileAdaptabilityLabel);
            colorInfo(ref this.PlayerProfileAmbitionLabel);
            colorInfo(ref this.PlayerProfileControversyLabel);
            colorInfo(ref this.PlayerProfileLoyaltyLabel);
            colorInfo(ref this.PlayerProfilePressureLabel);
            colorInfo(ref this.PlayerProfileProfessionalismLabel);
            colorInfo(ref this.PlayerProfileSportsmanshipLabel);
            colorInfo(ref this.PlayerProfileTemperamentLabel);

            // other
            colorInfo(ref this.PlayerProfileConditionLabel);
            colorInfo(ref this.PlayerProfileMoraleLabel);
            colorInfo(ref this.PlayerProfileHappinessLabel);
            colorInfo(ref this.PlayerProfileJadednessLabel);
            colorInfo(ref this.PlayerProfileSquadNoLabel);
            colorInfo(ref this.PlayerProfileLeftFootLabel);
            colorInfo(ref this.PlayerProfileRightFootLabel);

            // textboxes
            // personal details
            colorInfo(ref this.PlayerProfileIDTextBox);
            colorInfo2(ref this.PlayerProfileFullNameTextBox);
            colorInfo(ref this.PlayerProfileClubTextBox);
            colorInfo2(ref this.PlayerProfileTeamSquadTextBox);
            colorInfo(ref this.PlayerProfileNationalityTextBox);
            colorInfo2(ref this.PlayerProfileFormedTextBox);
            colorInfo(ref this.PlayerProfileEUMemberTextBox);
            colorInfo(ref this.PlayerProfileHomeGrownTextBox);
            colorInfo(ref this.PlayerProfileInternationalTextBox);
            colorInfo(ref this.PlayerProfilePreferredFootTextBox);
            colorInfo2(ref this.PlayerProfileBirthDateTextBox);
            colorInfo2(ref this.PlayerProfileAgeTextBox);
            colorInfo(ref this.PlayerProfileHeightTextBox);
            colorInfo(ref this.PlayerProfileWeightTextBox);
            colorInfo(ref this.PlayerProfileContractStartedTextBox);
            colorInfo2(ref this.PlayerProfileContractExpiryTextBox);
            colorInfo2(ref this.PlayerProfileValueTextBox);
            colorInfo2(ref this.PlayerProfileSaleValueTextBox);

            colorInfo(ref this.PlayerProfileLocalReputationTextBox);
            colorInfo2(ref this.PlayerProfileNationalReputationTextBox);
            colorInfo(ref this.PlayerProfileWorldReputationTextBox);
            colorInfo2(ref this.PlayerProfileCATextBox);
            colorInfo2(ref this.PlayerProfilePATextBox);
            colorInfo(ref this.PlayerProfileShortlistTextBox);
            colorInfo2(ref this.PlayerProfilePositionTextBox);
            colorInfo2(ref this.PlayerProfileAppearanceBonusTextBox);
            colorInfo(ref this.PlayerProfileWageTextBox);
            colorInfo(ref this.PlayerProfileGoalBonusTextBox);
            colorInfo2(ref this.PlayerProfileCleanSheetBonusTextBox);

            // technical skills
            colorAttribute(ref this.PlayerProfileCornersTextBox);
            colorAttribute2(ref this.PlayerProfileCrossingTextBox);
            colorAttribute(ref this.PlayerProfileDribblingTextBox);
            colorAttribute2(ref this.PlayerProfileFinishingTextBox);
            colorAttribute(ref this.PlayerProfileFirstTouchTextBox);
            colorAttribute2(ref this.PlayerProfileFreeKicksTextBox);
            colorAttribute(ref this.PlayerProfileHeadingTextBox);
            colorAttribute2(ref this.PlayerProfileLongShotsTextBox);
            colorAttribute(ref this.PlayerProfileLongThrowsTextBox);
            colorAttribute2(ref this.PlayerProfileMarkingTextBox);
            colorAttribute(ref this.PlayerProfilePassingTextBox);
            colorAttribute2(ref this.PlayerProfilePenaltyTakingTextBox);
            colorAttribute(ref this.PlayerProfileTacklingTextBox);
            colorAttribute2(ref this.PlayerProfileTechniqueTextBox);

            // goalkeeping skills
            colorAttribute(ref this.PlayerProfileAerialAbilityTextBox);
            colorAttribute2(ref this.PlayerProfileCommandOfAreaTextBox);
            colorAttribute(ref this.PlayerProfileCommunicationTextBox);
            colorAttribute2(ref this.PlayerProfileEccentricityTextBox);
            colorAttribute(ref this.PlayerProfileHandlingTextBox);
            colorAttribute2(ref this.PlayerProfileKickingTextBox);
            colorAttribute(ref this.PlayerProfileOneOnOnesTextBox);
            colorAttribute2(ref this.PlayerProfileReflexesTextBox);
            colorAttribute(ref this.PlayerProfileRushingOutTextBox);
            colorAttribute2(ref this.PlayerProfileTendencyToPunchTextBox);
            colorAttribute(ref this.PlayerProfileThrowingTextBox);

            // physical skills
            colorAttribute(ref this.PlayerProfileAccelerationTextBox);
            colorAttribute2(ref this.PlayerProfileAgilityTextBox);
            colorAttribute(ref this.PlayerProfileBalanceTextBox);
            colorAttribute2(ref this.PlayerProfileJumpingTextBox);
            colorAttribute(ref this.PlayerProfileNaturalFitnessTextBox);
            colorAttribute2(ref this.PlayerProfilePaceTextBox);
            colorAttribute(ref this.PlayerProfileStaminaTextBox);
            colorAttribute2(ref this.PlayerProfileStrengthTextBox);

            // mental skills
            colorAttribute(ref this.PlayerProfileAggressionTextBox);
            colorAttribute2(ref this.PlayerProfileAnticipationTextBox);
            colorAttribute(ref this.PlayerProfileBraveryTextBox);
            colorAttribute2(ref this.PlayerProfileComposureTextBox);
            colorAttribute(ref this.PlayerProfileConcentrationTextBox);
            colorAttribute2(ref this.PlayerProfileCreativityTextBox);
            colorAttribute(ref this.PlayerProfileDecisionsTextBox);
            colorAttribute2(ref this.PlayerProfileDeterminationTextBox);
            colorAttribute(ref this.PlayerProfileFlairTextBox);
            colorAttribute2(ref this.PlayerProfileInfluenceTextBox);
            colorAttribute(ref this.PlayerProfileOffTheBallTextBox);
            colorAttribute2(ref this.PlayerProfilePositioningTextBox);
            colorAttribute(ref this.PlayerProfileTeamWorkTextBox);
            colorAttribute2(ref this.PlayerProfileWorkRateTextBox);

            // hidden skills
            colorAttribute(ref this.PlayerProfileConsistencyTextBox);
            colorAttribute2(ref this.PlayerProfileDirtynessTextBox);
            colorAttribute(ref this.PlayerProfileImportantMatchesTextBox);
            colorAttribute2(ref this.PlayerProfileInjuryPronenessTextBox);
            colorAttribute(ref this.PlayerProfileVersatilityTextBox);

            // mental traits skills
            colorAttribute(ref this.PlayerProfileAdaptabilityTextBox);
            colorAttribute2(ref this.PlayerProfileAmbitionTextBox);
            colorAttribute(ref this.PlayerProfileControversyTextBox);
            colorAttribute2(ref this.PlayerProfileLoyaltyTextBox);
            colorAttribute(ref this.PlayerProfilePressureTextBox);
            colorAttribute2(ref this.PlayerProfileProfessionalismTextBox);
            colorAttribute(ref this.PlayerProfileSportsmanshipTextBox);
            colorAttribute2(ref this.PlayerProfileTemperamentTextBox);

            // other
            colorInfo(ref this.PlayerProfileConditionTextBox);
            colorInfo2(ref this.PlayerProfileMoraleTextBox);
            colorInfo(ref this.PlayerProfileJadednessTextBox);
            colorInfo2(ref this.PlayerProfileHappinessTextBox);
            colorInfo(ref this.PlayerProfileSquadNoTextBox);
            colorInfo2(ref this.PlayerProfileLeftFootTextBox);
            colorInfo(ref this.PlayerProfileRightFootTextBox);

            // positional ratings
            colorMainSet(ref this.PlayerProfilePositionalRatingLabel);
            colorRatings(ref this.PlayerProfileGKLabel);
            colorRatings(ref this.PlayerProfileDCLabel);
            colorRatings(ref this.PlayerProfileDLLabel);
            colorRatings(ref this.PlayerProfileDRLabel);
            colorRatings(ref this.PlayerProfileDMCLabel);
            colorRatings(ref this.PlayerProfileDMLLabel);
            colorRatings(ref this.PlayerProfileDMRLabel);
            colorRatings(ref this.PlayerProfileAMCLabel);
            colorRatings(ref this.PlayerProfileAMLLabel);
            colorRatings(ref this.PlayerProfileAMRLabel);
            colorRatings(ref this.PlayerProfileFCQuickLabel);
            colorRatings(ref this.PlayerProfileFCStrongLabel);
        }

        private void setStaffTabColor()
        {
            colorMainSet(ref this.StaffSearchButton);
            colorMainSet(ref this.StaffWonderStaffButton);

            // labels
            // staff search
            colorMainSet(ref this.StaffRegionLabel);
            colorMainSet(ref this.StaffRegionComboBox);
            colorMainSet(ref this.StaffClubLabel);
            colorMainSet(ref this.StaffFullNameLabel);
            colorMainSet(ref this.StaffRoleComboBox);
            colorMainSet(ref this.StaffClubTextBox);
            colorMainSet(ref this.StaffFullNameTextBox);
            colorMainSet(ref this.StaffNationLabel);
            colorMainSet(ref this.StaffNationTextBox);
            colorMainSet(ref this.StaffRoleLabel);
            colorMainSet(ref this.StaffPAMaxNumericUpDown);
            colorMainSet(ref this.StaffAgeLabel);
            colorMainSet(ref this.StaffPAMinNumericUpDown);
            colorMainSet(ref this.StaffAgeDashLabel);
            colorMainSet(ref this.StaffCAMinNumericUpDown);
            colorMainSet(ref this.StaffCALabel);
            colorMainSet(ref this.StaffAgeMinNumericUpDown);
            colorMainSet(ref this.StaffCADashLabel);
            colorMainSet(ref this.StaffAgeMaxNumericUpDown);
            colorMainSet(ref this.StaffPALabel);
            colorMainSet(ref this.StaffPADashLabel);
            colorMainSet(ref this.StaffCAMaxNumericUpDown);
            colorMainSet(ref this.StaffRegenLabel);
            colorMainSet(ref this.StaffRegenComboBox);
            colorMainSet(ref this.StaffContractStatusComboBox);
            colorMainSet(ref this.StaffContractStatusLabel);
            colorMainSet(ref this.StaffBestCRLabel);
            colorMainSet(ref this.StaffBestCRComboBox);

            colorMainSet(ref this.StaffRatingsFitnessLabel);
            colorMainSet(ref this.StaffRatingsFitnessDashLabel);
            colorMainSet(ref this.StaffRatingsFitnessMinNumericUpDown);
            colorMainSet(ref this.StaffRatingsFitnessMaxNumericUpDown);
            colorMainSet(ref this.StaffRatingsGoalkeepersLabel);
            colorMainSet(ref this.StaffRatingsGoalkeepersDashLabel);
            colorMainSet(ref this.StaffRatingsGoalkeepersMinNumericUpDown);
            colorMainSet(ref this.StaffRatingsGoalkeepersMaxNumericUpDown);
            colorMainSet(ref this.StaffRatingsTacticsLabel);
            colorMainSet(ref this.StaffRatingsTacticsDashLabel);
            colorMainSet(ref this.StaffRatingsTacticsMinNumericUpDown);
            colorMainSet(ref this.StaffRatingsTacticsMaxNumericUpDown);
            colorMainSet(ref this.StaffRatingsBallControlLabel);
            colorMainSet(ref this.StaffRatingsBallControlDashLabel);
            colorMainSet(ref this.StaffRatingsBallControlMinNumericUpDown);
            colorMainSet(ref this.StaffRatingsBallControlMaxNumericUpDown);
            colorMainSet(ref this.StaffRatingsDefendingLabel);
            colorMainSet(ref this.StaffRatingsDefendingDashLabel); 
            colorMainSet(ref this.StaffRatingsDefendingMinNumericUpDown);
            colorMainSet(ref this.StaffRatingsDefendingMaxNumericUpDown);
            colorMainSet(ref this.StaffRatingsAttackingLabel);
            colorMainSet(ref this.StaffRatingsAttackingDashLabel); 
            colorMainSet(ref this.StaffRatingsAttackingMinNumericUpDown);
            colorMainSet(ref this.StaffRatingsAttackingMaxNumericUpDown);
            colorMainSet(ref this.StaffRatingsShootingLabel);
            colorMainSet(ref this.StaffRatingsShootingDashLabel);
            colorMainSet(ref this.StaffRatingsShootingMinNumericUpDown);
            colorMainSet(ref this.StaffRatingsShootingMaxNumericUpDown);
            colorMainSet(ref this.StaffRatingsSetPiecesLabel);
            colorMainSet(ref this.StaffRatingsSetPiecesDashLabel);
            colorMainSet(ref this.StaffRatingsSetPiecesMinNumericUpDown);
            colorMainSet(ref this.StaffRatingsSetPiecesMaxNumericUpDown);

            // staff attributes
            // coaching skills
            colorInfo(ref this.StaffWorkingWithYoungstersLabel);
            colorInfo(ref this.label18);
            colorMainSet(ref this.StaffWorkingWithYoungstersMinNumericUpDown);
            colorMainSet(ref this.StaffWorkingWithYoungstersMaxNumericUpDown);
            colorInfo(ref this.StaffManManagementLabel);
            colorInfo(ref this.label20);
            colorMainSet(ref this.StaffManManagementMinNumericUpDown);
            colorMainSet(ref this.StaffManManagementMaxNumericUpDown);
            colorInfo(ref this.StaffTechnicalLabel);
            colorInfo(ref this.label10);
            colorMainSet(ref this.StaffTechnicalMinNumericUpDown);
            colorMainSet(ref this.StaffTechnicalMaxNumericUpDown);
            colorInfo(ref this.StaffTacticalLabel);
            colorInfo(ref this.label12);
            colorMainSet(ref this.StaffTacticalMinNumericUpDown);
            colorMainSet(ref this.StaffTacticalMaxNumericUpDown);
            colorInfo(ref this.StaffPlayerLabel);
            colorInfo(ref this.label14);
            colorMainSet(ref this.StaffPlayerMinNumericUpDown);
            colorMainSet(ref this.StaffPlayerMaxNumericUpDown);
            colorInfo(ref this.StaffMentalLabel);
            colorInfo(ref this.label16);
            colorMainSet(ref this.StaffMentalMinNumericUpDown);
            colorMainSet(ref this.StaffMentalMaxNumericUpDown);
            colorInfo(ref this.StaffGoalkeepersLabel);
            colorInfo(ref this.label6);
            colorMainSet(ref this.StaffGoalkeepersMinNumericUpDown);
            colorMainSet(ref this.StaffGoalkeepersMaxNumericUpDown);
            colorInfo(ref this.StaffFitnessLabel);
            colorInfo(ref this.label8);
            colorMainSet(ref this.StaffFitnessMinNumericUpDown);
            colorMainSet(ref this.StaffFitnessMaxNumericUpDown);
            colorInfo(ref this.StaffDefendingLabel);
            colorInfo(ref this.label4);
            colorMainSet(ref this.StaffDefendingMinNumericUpDown);
            colorMainSet(ref this.StaffDefendingMaxNumericUpDown);
            colorInfo(ref this.StaffAttackingLabel);
            colorInfo(ref this.label2);
            colorMainSet(ref this.StaffAttackingMinNumericUpDown);
            colorMainSet(ref this.StaffAttackingMaxNumericUpDown);

            // mental skills
            colorInfo(ref this.StaffTacticalKnowledgeLabel);
            colorInfo(ref this.label32);
            colorMainSet(ref this.StaffTacticalKnowledgeMinNumericUpDown);
            colorMainSet(ref this.StaffTacticalKnowledgeMaxNumericUpDown);
            colorInfo(ref this.StaffPhysiotherapyLabel);
            colorInfo(ref this.label34);
            colorMainSet(ref this.StaffPhysiotherapyMinNumericUpDown);
            colorMainSet(ref this.StaffPhysiotherapyMaxNumericUpDown);
            colorInfo(ref this.StaffMotivatingLabel);
            colorInfo(ref this.label36);
            colorMainSet(ref this.StaffMotivatingMinNumericUpDown);
            colorMainSet(ref this.StaffMotivatingMaxNumericUpDown);
            colorInfo(ref this.StaffLevelOfDisciplineLabel);
            colorInfo(ref this.label38);
            colorMainSet(ref this.StaffLevelOfDisciplineMinNumericUpDown);
            colorMainSet(ref this.StaffLevelOfDisciplineMaxNumericUpDown);
            colorInfo(ref this.StaffJudgingPlayerPotentialLabel);
            colorInfo(ref this.label40);
            colorMainSet(ref this.StaffJudgingPlayerPotentialMinNumericUpDown);
            colorMainSet(ref this.StaffJudgingPlayerPotentialMaxNumericUpDown);
            colorInfo(ref this.StaffJudgingPlayerAbilityLabel);
            colorInfo(ref this.label3);
            colorMainSet(ref this.StaffJudgingPlayerAbilityMinNumericUpDown);
            colorMainSet(ref this.StaffJudgingPlayerAbilityMaxNumericUpDown);
            colorInfo(ref this.StaffTemperamentLabel);
            colorInfo(ref this.label7);
            colorMainSet(ref this.StaffTemperamentMinNumericUpDown);
            colorMainSet(ref this.StaffTemperamentMaxNumericUpDown);
            colorInfo(ref this.StaffSportmanshipLabel);
            colorInfo(ref this.label11);
            colorMainSet(ref this.StaffSportmanshipMinNumericUpDown);
            colorMainSet(ref this.StaffSportmanshipMaxNumericUpDown);
            colorInfo(ref this.StaffProfessionalismLabel);
            colorInfo(ref this.label15);
            colorMainSet(ref this.StaffProfessionalismMinNumericUpDown);
            colorMainSet(ref this.StaffProfessionalismMaxNumericUpDown);
            colorInfo(ref this.StaffPressureLabel);
            colorInfo(ref this.label19);
            colorMainSet(ref this.StaffPressureMinNumericUpDown);
            colorMainSet(ref this.StaffPressureMaxNumericUpDown);
            colorInfo(ref this.StaffLoyaltyLabel);
            colorInfo(ref this.label22);
            colorMainSet(ref this.StaffLoyaltyMinNumericUpDown);
            colorMainSet(ref this.StaffLoyaltyMaxNumericUpDown);
            colorInfo(ref this.StaffDeterminationLabel);
            colorInfo(ref this.label24);
            colorMainSet(ref this.StaffDeterminationMinNumericUpDown);
            colorMainSet(ref this.StaffDeterminationMaxNumericUpDown);
            colorInfo(ref this.StaffControversyLabel);
            colorInfo(ref this.label26);
            colorMainSet(ref this.StaffControversyMinNumericUpDown);
            colorMainSet(ref this.StaffControversyMaxNumericUpDown);
            colorInfo(ref this.StaffAmbitionLabel);
            colorInfo(ref this.label28);
            colorMainSet(ref this.StaffAmbitionMinNumericUpDown);
            colorMainSet(ref this.StaffAmbitionMaxNumericUpDown);
            colorInfo(ref this.StaffAdaptabilityLabel);
            colorInfo(ref this.label30);
            colorMainSet(ref this.StaffAdaptabilityMinNumericUpDown);
            colorMainSet(ref this.StaffAdaptabilityMaxNumericUpDown);

            // tactical skills
            colorInfo(ref this.StaffWidthLabel);
            colorInfo(ref this.label62);
            colorMainSet(ref this.StaffWidthMinNumericUpDown);
            colorMainSet(ref this.StaffWidthMaxNumericUpDown);
            colorInfo(ref this.StaffUseOfSubstitutionsLabel);
            colorInfo(ref this.label64);
            colorMainSet(ref this.StaffUseOfSubstitutionsMinNumericUpDown);
            colorMainSet(ref this.StaffUseOfSubstitutionsMaxNumericUpDown);
            colorInfo(ref this.StaffUseOfPlaymakerLabel);
            colorInfo(ref this.label66);
            colorMainSet(ref this.StaffUseOfPlaymakerMinNumericUpDown);
            colorMainSet(ref this.StaffUseOfPlaymakerMaxNumericUpDown);
            colorInfo(ref this.StaffTempoLabel);
            colorInfo(ref this.label42);
            colorMainSet(ref this.StaffTempoMinNumericUpDown);
            colorMainSet(ref this.StaffTempoMaxNumericUpDown);
            colorInfo(ref this.StaffSittingBackLabel);
            colorInfo(ref this.label44);
            colorMainSet(ref this.StaffSittingBackMinNumericUpDown);
            colorMainSet(ref this.StaffSittingBackMaxNumericUpDown);
            colorInfo(ref this.StaffPressingLabel);
            colorInfo(ref this.label46);
            colorMainSet(ref this.StaffPressingMinNumericUpDown);
            colorMainSet(ref this.StaffPressingMaxNumericUpDown);
            colorInfo(ref this.StaffOffsideLabel);
            colorInfo(ref this.label48);
            colorMainSet(ref this.StaffOffsideMinNumericUpDown);
            colorMainSet(ref this.StaffOffsideMaxNumericUpDown);
            colorInfo(ref this.StaffMarkingLabel);
            colorInfo(ref this.label50);
            colorMainSet(ref this.StaffMarkingMinNumericUpDown);
            colorMainSet(ref this.StaffMarkingMaxNumericUpDown);
            colorInfo(ref this.StaffFreeRolesLabel);
            colorInfo(ref this.label52);
            colorMainSet(ref this.StaffFreeRolesMinNumericUpDown);
            colorMainSet(ref this.StaffFreeRolesMaxNumericUpDown);
            colorInfo(ref this.StaffFlexibilityLabel);
            colorInfo(ref this.label54);
            colorMainSet(ref this.StaffFlexibilityMinNumericUpDown);
            colorMainSet(ref this.StaffFlexibilityMaxNumericUpDown);
            colorInfo(ref this.StaffFlamboyancyLabel);
            colorInfo(ref this.label56);
            colorMainSet(ref this.StaffFlamboyancyMinNumericUpDown);
            colorMainSet(ref this.StaffFlamboyancyMaxNumericUpDown);
            colorInfo(ref this.StaffDirectnessLabel);
            colorInfo(ref this.label58);
            colorMainSet(ref this.StaffDirectnessMinNumericUpDown);
            colorMainSet(ref this.StaffDirectnessMaxNumericUpDown);
            colorInfo(ref this.StaffDepthLabel);
            colorInfo(ref this.label60);
            colorMainSet(ref this.StaffDepthMinNumericUpDown);
            colorMainSet(ref this.StaffDepthMaxNumericUpDown);

            // non tactical skills
            colorInfo(ref this.StaffSquadRotationLabel);
            colorInfo(ref this.label80);
            colorMainSet(ref this.StaffSquadRotationMinNumericUpDown);
            colorMainSet(ref this.StaffSquadRotationMaxNumericUpDown);
            colorInfo(ref this.StaffMindgamesLabel);
            colorInfo(ref this.label82);
            colorMainSet(ref this.StaffMindgamesMinNumericUpDown);
            colorMainSet(ref this.StaffMindgamesMaxNumericUpDown);
            colorInfo(ref this.StaffHardnessOfTrainingLabel);
            colorInfo(ref this.label84);
            colorMainSet(ref this.StaffHardnessOfTrainingMinNumericUpDown);
            colorMainSet(ref this.StaffHardnessOfTrainingMaxNumericUpDown);
            colorInfo(ref this.StaffBuyingPlayersLabel);
            colorInfo(ref this.label86);
            colorMainSet(ref this.StaffBuyingPlayersMinNumericUpDown);
            colorMainSet(ref this.StaffBuyingPlayersMaxNumericUpDown);

            // chairman skills
            colorInfo(ref this.StaffResourcesLabel);
            colorInfo(ref this.label255);
            colorMainSet(ref this.StaffResourcesMinNumericUpDown);
            colorMainSet(ref this.StaffResourcesMaxNumericUpDown);
            colorInfo(ref this.StaffPatienceLabel);
            colorInfo(ref this.label257);
            colorMainSet(ref this.StaffPatienceMinNumericUpDown);
            colorMainSet(ref this.StaffPatienceMaxNumericUpDown);
            colorInfo(ref this.StaffInterferenceLabel);
            colorInfo(ref this.label259);
            colorMainSet(ref this.StaffInterferenceMinNumericUpDown);
            colorMainSet(ref this.StaffInterferenceMaxNumericUpDown);
            colorInfo(ref this.StaffBusinesLabel);
            colorInfo(ref this.label261);
            colorMainSet(ref this.StaffBusinessMinNumericUpDown);
            colorMainSet(ref this.StaffBusinessMaxNumericUpDown);
        }

        private void setStaffProfileColor()
        {
            // labels
            // personal details
            colorInfo(ref this.StaffProfileLocalReputationLabel);
            colorInfo(ref this.StaffProfileNationalReputationLabel);
            colorInfo(ref this.StaffProfileWorldReputationLabel);
            colorInfo(ref this.StaffProfileContractStartedLabel);
            colorInfo(ref this.StaffProfileContractExpiryLabel);
            colorInfo(ref this.StaffProfileBasicWageLabel);

            // ratings
            colorInfo(ref this.StaffProfileRatingsFitnessLabel);
            colorInfo(ref this.StaffProfileRatingsGoalkeepersLabel);
            colorInfo(ref this.StaffProfileRatingsBallControlLabel);
            colorInfo(ref this.StaffProfileRatingsTacticsLabel);
            colorInfo(ref this.StaffProfileRatingsDefendingLabel);
            colorInfo(ref this.StaffProfileRatingsAttackingLabel);
            colorInfo(ref this.StaffProfileRatingsShootingLabel);
            colorInfo(ref this.StaffProfileRatingsSetPiecesLabel);

            colorInfo(ref this.StaffProfileRatingsFitnessPictureBox);
            colorInfo(ref this.StaffProfileRatingsGoalkeepersPictureBox);
            colorInfo(ref this.StaffProfileRatingsBallControlPictureBox);
            colorInfo(ref this.StaffProfileRatingsTacticsPictureBox);
            colorInfo(ref this.StaffProfileRatingsDefendingPictureBox);
            colorInfo(ref this.StaffProfileRatingsAttackingPictureBox);
            colorInfo(ref this.StaffProfileRatingsShootingPictureBox);
            colorInfo(ref this.StaffProfileRatingsSetPiecesPictureBox);

            // coaching skills
            colorInfo(ref this.StaffProfileAttackingLabel);
            colorInfo(ref this.StaffProfileDefendingLabel);
            colorInfo(ref this.StaffProfileFitnessLabel);
            colorInfo(ref this.StaffProfileGoalkeepersLabel);
            colorInfo(ref this.StaffProfileMentalLabel);
            colorInfo(ref this.StaffProfilePlayerLabel);
            colorInfo(ref this.StaffProfileTacticalLabel);
            colorInfo(ref this.StaffProfileTechnicalLabel);
            colorInfo(ref this.StaffProfileManManagementLabel);
            colorInfo(ref this.StaffProfileWorkingWithYoungstersLabel);

            // mental skills
            colorInfo(ref this.StaffProfileAdaptabilityLabel);
            colorInfo(ref this.StaffProfileAmbitionLabel);
            colorInfo(ref this.StaffProfileControversyLabel);
            colorInfo(ref this.StaffProfileDeterminationLabel);
            colorInfo(ref this.StaffProfileLoyaltyLabel);
            colorInfo(ref this.StaffProfilePressureLabel);
            colorInfo(ref this.StaffProfileProfessionalismLabel);
            colorInfo(ref this.StaffProfileSportsmanshipLabel);
            colorInfo(ref this.StaffProfileTemperamentLabel);
            colorInfo(ref this.StaffProfileJudgingPlayerAbilityLabel);
            colorInfo(ref this.StaffProfileJudgingPlayerPotentialLabel);
            colorInfo(ref this.StaffProfileLevelOfDisciplineLabel);
            colorInfo(ref this.StaffProfileMotivatingLabel);
            colorInfo(ref this.StaffProfilePhysiotherapyLabel);
            colorInfo(ref this.StaffProfileTacticalKnowledgeLabel);

            // tactical skills
            colorInfo(ref this.StaffProfileDepthLabel);
            colorInfo(ref this.StaffProfileDirectnessLabel);
            colorInfo(ref this.StaffProfileFlamboyancyLabel);
            colorInfo(ref this.StaffProfileFlexibilityLabel);
            colorInfo(ref this.StaffProfileFreeRolesLabel);
            colorInfo(ref this.StaffProfileMarkingLabel);
            colorInfo(ref this.StaffProfileOffsideLabel);
            colorInfo(ref this.StaffProfilePressingLabel);
            colorInfo(ref this.StaffProfileSittingBackLabel);
            colorInfo(ref this.StaffProfileTempoLabel);
            colorInfo(ref this.StaffProfileUseOfPlaymakerLabel);
            colorInfo(ref this.StaffProfileUseOfSubstitutionsLabel);
            colorInfo(ref this.StaffProfileWidthLabel);

            // non tactical skills
            colorInfo(ref this.StaffProfileBuyingPlayersLabel);
            colorInfo(ref this.StaffProfileHardnessOfTrainingLabel);
            colorInfo(ref this.StaffProfileMindGamesLabel);
            colorInfo(ref this.StaffProfileSquadRotationLabel);

            // chairman skills
            colorInfo(ref this.StaffProfileBusinessLabel);
            colorInfo(ref this.StaffProfileInterferenceLabel);
            colorInfo(ref this.StaffProfilePatienceLabel);
            colorInfo(ref this.StaffProfileResourcesLabel);

            // textboxes
            // personal details
            colorInfo(ref this.StaffProfileIDTextBox);
            colorInfo2(ref this.StaffProfileFullNameTextBox);
            colorInfo(ref this.StaffProfileClubTextBox);
            colorInfo(ref this.StaffProfileNationalityTextBox);
            colorInfo(ref this.StaffProfileInternationalTextBox);
            colorInfo2(ref this.StaffProfileBirthDateTextBox);
            colorInfo2(ref this.StaffProfileAgeTextBox);
            colorInfo(ref this.StaffProfileLocalReputationTextBox);
            colorInfo2(ref this.StaffProfileNationalReputationTextBox);
            colorInfo(ref this.StaffProfileWorldReputationTextBox);
            colorInfo(ref this.StaffProfileContractStartedTextBox);
            colorInfo2(ref this.StaffProfileContractExpiryTextBox);
            colorInfo(ref this.StaffProfileWageTextBox);
            colorInfo2(ref this.StaffProfileCATextBox);
            colorInfo2(ref this.StaffProfilePATextBox);
            colorInfo2(ref this.StaffProfileRoleTextBox);

            // coaching skills
            colorAttribute(ref this.StaffProfileAttackingTextBox);
            colorAttribute2(ref this.StaffProfileDefendingTextBox);
            colorAttribute(ref this.StaffProfileFitnessTextBox);
            colorAttribute2(ref this.StaffProfileGoalkeepersTextBox);
            colorAttribute(ref this.StaffProfileMentalTextBox);
            colorAttribute2(ref this.StaffProfilePlayerTextBox);
            colorAttribute(ref this.StaffProfileTacticalTextBox);
            colorAttribute2(ref this.StaffProfileTechnicalTextBox);
            colorAttribute(ref this.StaffProfileManManagementTextBox);
            colorAttribute2(ref this.StaffProfileWorkingWithYoungstersTextBox);

            // mental skills
            colorAttribute(ref this.StaffProfileAdaptabilityTextBox);
            colorAttribute2(ref this.StaffProfileAmbitionTextBox);
            colorAttribute(ref this.StaffProfileControversyTextBox);
            colorAttribute2(ref this.StaffProfileDeterminationTextBox);
            colorAttribute(ref this.StaffProfileLoyaltyTextBox);
            colorAttribute2(ref this.StaffProfilePressureTextBox);
            colorAttribute(ref this.StaffProfileProfessionalismTextBox);
            colorAttribute2(ref this.StaffProfileSportsmanshipTextBox);
            colorAttribute(ref this.StaffProfileTemperamentTextBox);
            colorAttribute2(ref this.StaffProfileJudgingPlayerAbilityTextBox);
            colorAttribute(ref this.StaffProfileJudgingPlayerPotentialTextBox);
            colorAttribute2(ref this.StaffProfileLevelOfDisciplineTextBox);
            colorAttribute(ref this.StaffProfileMotivatingTextBox);
            colorAttribute2(ref this.StaffProfilePhysiotherapyTextBox);
            colorAttribute(ref this.StaffProfileTacticalKnowledgeTextBox);

            // tactical skills
            colorAttribute(ref this.StaffProfileDepthTextBox);
            colorAttribute2(ref this.StaffProfileDirectnessTextBox);
            colorAttribute(ref this.StaffProfileFlamboyancyTextBox);
            colorAttribute2(ref this.StaffProfileFlexibilityTextBox);
            colorAttribute(ref this.StaffProfileFreeRolesTextBox);
            colorAttribute2(ref this.StaffProfileMarkingTextBox);
            colorAttribute(ref this.StaffProfileOffsideTextBox);
            colorAttribute2(ref this.StaffProfilePressingTextBox);
            colorAttribute(ref this.StaffProfileSittingBackTextBox);
            colorAttribute2(ref this.StaffProfileTempoTextBox);
            colorAttribute(ref this.StaffProfileUseOfPlaymakerTextBox);
            colorAttribute2(ref this.StaffProfileUseOfSubstitutionsTextBox);
            colorAttribute(ref this.StaffProfileWidthTextBox);

            // non tactical skills
            colorAttribute(ref this.StaffProfileBuyingPlayersTextBox);
            colorAttribute2(ref this.StaffProfileHardnessOfTrainingTextBox);
            colorAttribute(ref this.StaffProfileMindGamesTextBox);
            colorAttribute2(ref this.StaffProfileSquadRotationTextBox);

            // chairman skills
            colorAttribute(ref this.StaffProfileBusinessTextBox);
            colorAttribute2(ref this.StaffProfileInterferenceTextBox);
            colorAttribute(ref this.StaffProfilePatienceTextBox);
            colorAttribute2(ref this.StaffProfileResourcesTextBox);
        }

        private void setTeamTabColor()
        {
            colorMainSet(ref this.TeamsSearchButton);
            colorMainSet(ref this.TeamsWonderTeamsButton);

            colorMainSet(ref this.TeamTypeComboBox);
            colorMainSet(ref this.TeamTeamTypeLabel);
            colorMainSet(ref this.TeamStadiumNameTextBox);
            colorMainSet(ref this.TeamStadiumNameLabel);
            colorMainSet(ref this.TeamRegionLabel);
            colorMainSet(ref this.TeamRegionComboBox);
            colorMainSet(ref this.TeamReputationComboBox);
            colorMainSet(ref this.TeamNameLabel);
            colorMainSet(ref this.TeamNameTextBox);
            colorMainSet(ref this.TeamNationLabel);
            colorMainSet(ref this.TeamNationalityTextBox);
            colorMainSet(ref this.TeamReputationLabel);
        }

        private void setTeamProfileColor()
        {
            colorMainSetNonTransp(ref this.TeamProfileHealTeamButton);
            colorMainSetNonTransp(ref this.TeamProfileListPlayersButton);
            colorMainSetNonTransp(ref this.TeamProfileListStaffButton);
            colorMainSet(ref this.TeamProfileListPlayersComboBox);

            // labels
            // general details
            colorInfo(ref this.TeamProfileYearFoundedLabel);
            colorInfo(ref this.TeamProfileNationalLabel);
            colorInfo(ref this.TeamProfileStatusLabel);
            colorInfo(ref this.TeamProfileMaxAffiliatedClubsLabel);
            colorInfo(ref this.TeamProfileAffiliatedClubsLabel);
            colorInfo(ref this.TeamProfileTrainingGroundLabel);
            colorInfo(ref this.TeamProfileYouthGroundLabel);
            colorInfo(ref this.TeamProfileYouthAcademyLabel);
            colorInfo(ref this.TeamProfileReputationLabel);
            colorInfo(ref this.TeamProfileTotalTransferLabel);
            colorInfo(ref this.TeamProfileRemainingTransferLabel);
            colorInfo(ref this.TeamProfileBalanceLabel);
            colorInfo(ref this.TeamProfileTotalWageLabel);
            colorInfo(ref this.TeamProfileUsedWageLabel);
            colorInfo(ref this.TeamProfileRevenueAvailableLabel);

            // stadium details
            colorInfo(ref this.TeamProfileDecayLabel);
            colorInfo(ref this.TeamProfileFieldLengthLabel);
            colorInfo(ref this.TeamProfileFieldWidthLabel);
            colorInfo(ref this.TeamProfileCurrentCapacityLabel);
            colorInfo(ref this.TeamProfileSeatingCapacityLabel);
            colorInfo(ref this.TeamProfileExpansionCapacityLabel);
            colorInfo(ref this.TeamProfileUsedCapacityLabel);
            colorInfo(ref this.TeamProfileDecayLabel);
            colorInfo(ref this.TeamProfileMaxAttendanceLabel);
            colorInfo(ref this.TeamProfileAverageAttendanceLabel);
            colorInfo(ref this.TeamProfileMinAttendanceLabel);

            // textboxes
            // general details
            colorInfo(ref this.TeamProfileIDTextBox);
            colorInfo2(ref this.TeamProfileNameTextBox);
            colorInfo(ref this.TeamProfileNationalityTextBox);
            colorInfo2(ref this.TeamProfileYearFoundedTextBox);
            colorInfo(ref this.TeamProfileNationalTextBox);
            colorInfo2(ref this.TeamProfileStatusTextBox);
            colorInfo(ref this.TeamProfileMaxAffiliatedClubsTextBox);
            colorInfo2(ref this.TeamProfileAffiliatedClubsTextBox);
            colorInfo(ref this.TeamProfileTrainingGroundTextBox);
            colorInfo2(ref this.TeamProfileYouthGroundTextBox);
            colorInfo(ref this.TeamProfileYouthAcademyTextBox);
            colorInfo(ref this.TeamProfileReputationTextBox);
            colorInfo(ref this.TeamProfileTotalTransferTextBox);
            colorInfo2(ref this.TeamProfileRemainingTransferTextBox);
            colorInfo(ref this.TeamProfileBalanceTextBox);
            colorInfo2(ref this.TeamProfileTotalWageTextBox);
            colorInfo(ref this.TeamProfileUsedWageTextBox);
            colorInfo2(ref this.TeamProfileRevenueAvailableTextBox);

            // stadium details
            colorInfo(ref this.TeamProfileStadiumIDTextBox);
            colorInfo2(ref this.TeamProfileStadiumNameTextBox);
            colorInfo(ref this.TeamProfileStadiumOwnerTextBox);
            colorInfo2(ref this.TeamProfileStadiumLocationTextBox);
            colorInfo(ref this.TeamProfileStadiumNearbyStadiumTextBox);
            colorInfo2(ref this.TeamProfileStadiumDecayTextBox);
            colorInfo(ref this.TeamProfileStadiumFieldWidthTextBox);
            colorInfo2(ref this.TeamProfileStadiumFieldLengthTextBox);
            colorInfo(ref this.TeamProfileStadiumCurrentCapacityTextBox);
            colorInfo2(ref this.TeamProfileStadiumSeatingCapacityTextBox);
            colorInfo(ref this.TeamProfileStadiumExpansionCapacityTextBox);
            colorInfo2(ref this.TeamProfileStadiumUsedCapacityTextBox);
            colorInfo2(ref this.TeamProfileMaxAttendanceTextBox);
            colorInfo(ref this.TeamProfileAverageAttendanceTextBox);
            colorInfo2(ref this.TeamProfileMinAttendanceTextBox);
        }

        public void updateChanges(bool editing)
        {
            if (fm != null)
            {
                if (fm.FMLoaded())
                {
                    if (!this.PlayerProfileIDTextBox.Text.Equals("ID"))
                        setPlayerProfile(Int32.Parse(this.PlayerProfileIDTextBox.Text), false);
                    else
                        setPlayerProfileColor();

                    if (!this.StaffProfileIDTextBox.Text.Equals("ID"))
                        setStaffProfile(Int32.Parse(this.PlayerProfileIDTextBox.Text), false);
                    else
                        setStaffProfileColor();

                    if (!this.TeamProfileIDTextBox.Text.Equals("ID"))
                        setTeamProfile(Int32.Parse(this.TeamProfileIDTextBox.Text), false);
                    else
                        setTeamProfileColor();
                }
            }
        }

        public void colorMainSet(ref CheckBox checkbox)
        {
            checkbox.BackColor = colorMain;
            checkbox.ForeColor = colorMainText;
        }

        public void colorMainSet(ref ComboBox combobox)
        {
            combobox.BackColor = colorSearchFields;
            combobox.ForeColor = colorSearchFieldsText;
        }

        public void colorMainSet(ref Panel panel)
        {
            panel.BackColor = Color.Transparent;
            panel.ForeColor = colorMainText;
        }

        public void colorMainSet(ref Button button)
        {
            button.BackColor = Color.Transparent;
            button.ForeColor = colorMainText;
        }

        public void colorMainSetNonTransp(ref Button button)
        {
            button.BackColor = colorGroupBoxes;
            button.ForeColor = colorMainText;
        }

        public void colorMainSet(ref NumericUpDown numericupdown)
        {
            numericupdown.BackColor = colorSearchFields;
            numericupdown.ForeColor = colorSearchFieldsText;
        }

        public void colorMainSet(ref Label label)
        {
            label.BackColor = colorGroupBoxes;
            label.ForeColor = colorMainText;
        }

        public void colorMainSet(ref TextBox textbox)
        {
            textbox.BackColor = colorSearchFields;
            textbox.ForeColor = colorSearchFieldsText;
        }

        public void colorMainSet(ref CheckedListBox checkedlistbox)
        {
            checkedlistbox.BackColor = colorSearchFields;
            checkedlistbox.ForeColor = colorSearchFieldsText;
        }

        public void colorInfo(ref TextBox textbox)
        {
            if (textbox.ReadOnly)
            {
                textbox.BackColor = colorProfileFields1;
                textbox.ForeColor = colorProfileFieldsText;
            }
            else
            {
                textbox.BackColor = colorProfileFieldsOnEdit;
                textbox.ForeColor = colorProfileFieldsOnEditText;
            }
        }

        public void colorInfo2(ref TextBox textbox)
        {
            if (textbox.ReadOnly)
            {
                textbox.BackColor = colorProfileFields2;
                textbox.ForeColor = colorProfileFieldsText;
            }
            else
            {
                textbox.BackColor = colorProfileFieldsOnEdit;
                textbox.ForeColor = colorProfileFieldsOnEditText;
            }
        }

        public void colorAttribute(ref TextBox textbox)
        {
            int value = Int32.Parse(textbox.Text);

            if (textbox.ReadOnly)
            {
                textbox.BackColor = colorProfileFields1;
                if (value >= 0 && value <= 5)
                    textbox.ForeColor = colorLowAttribute;
                else if (value > 5 && value <= 10)
                    textbox.ForeColor = colorMediumAttribute;
                else if (value > 10 && value <= 15)
                    textbox.ForeColor = colorGoodAttribute;
                else if (value > 15 && value <= 20)
                    textbox.ForeColor = colorExcellentAttribute;
            }
            else
            {
                textbox.BackColor = colorProfileFieldsOnEdit;
                textbox.ForeColor = colorProfileFieldsOnEditText;
            }
            textbox.Font = themeFontBold;
        }

        public void colorAttribute(ref FMScout.Controls.TextBoxAdvanced textbox)
        {
            int value = Int32.Parse(textbox.Text);

            if (textbox.ReadOnly)
            {
                textbox.BackColor = Color.Transparent;
                if (value >= 0 && value <= 5)
                    textbox.ForeColor = colorLowAttribute;
                else if (value > 5 && value <= 10)
                    textbox.ForeColor = colorMediumAttribute;
                else if (value > 10 && value <= 15)
                    textbox.ForeColor = colorGoodAttribute;
                else if (value > 15 && value <= 20)
                    textbox.ForeColor = colorExcellentAttribute;
            }
            else
            {
                textbox.BackColor = colorProfileFieldsOnEdit;
                textbox.ForeColor = colorProfileFieldsOnEditText;
            }
            textbox.Font = themeFontBold;
        }


        public void colorAttribute2(ref TextBox textbox)
        {
            int value = Int32.Parse(textbox.Text);

            if (textbox.ReadOnly)
            {
                textbox.BackColor = colorProfileFields2;
                if (value >= 0 && value <= 5)
                    textbox.ForeColor = colorLowAttribute;
                else if (value > 5 && value <= 10)
                    textbox.ForeColor = colorMediumAttribute;
                else if (value > 10 && value <= 15)
                    textbox.ForeColor = colorGoodAttribute;
                else if (value > 15 && value <= 20)
                    textbox.ForeColor = colorExcellentAttribute;
            }
            else
            {
                textbox.BackColor = colorProfileFieldsOnEdit;
                textbox.ForeColor = colorProfileFieldsOnEditText;
            }
            textbox.Font = themeFontBold;
        }

        public void colorInfo(ref Label label)
        {
            label.BackColor = Color.Transparent;
            label.ForeColor = colorProfileFieldsText;
        }

        public void colorRatings(ref Label label)
        {
            float value = float.Parse(label.Text.Substring(0, label.Text.Length - 2));
            label.BackColor = Color.FromArgb(120, colorProfileFields1.R, colorProfileFields1.G, colorProfileFields1.B);
            if (value >= 0 && value <= 50)
                label.ForeColor = colorLowAttribute;
            else if (value > 50 && value <= 65)
                label.ForeColor = colorMediumAttribute;
            else if (value > 65 && value <= 75)
                label.ForeColor = colorGoodAttribute;
            else if (value > 75 && value <= 100)
                label.ForeColor = colorExcellentAttribute;

            label.Font = themeFontBold;
        }

        public void colorInfo(ref Button button)
        {
            button.BackColor = Color.Transparent;
            button.ForeColor = colorProfileFieldsText;
        }

        public void colorInfo(ref PictureBox picturebox)
        {
            picturebox.BackColor = Color.Transparent;
            picturebox.ForeColor = colorProfileFieldsText;
        }

        public void colorAttribute(ref Label label)
        {
            int value = Int32.Parse(label.Text);
            label.BackColor = Color.Transparent;
            if (value >= 0 && value <= 5)
                label.ForeColor = colorLowAttribute;
            else if (value > 5 && value <= 10)
                label.ForeColor = colorMediumAttribute;
            else if (value > 10 && value <= 15)
                label.ForeColor = colorGoodAttribute;
            else if (value > 15 && value <= 20)
                label.ForeColor = colorExcellentAttribute;

            label.Font = themeFontBold;
        }

        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = false;
            // Draw the background and node text for a selected node.
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, NodeBounds(e.Node));
                e.Graphics.DrawString(e.Node.Text, this.treeView.Font, Brushes.White, e.Node.Bounds.Left, e.Node.Bounds.Top);
            }
            else
            {
                e.Graphics.FillRectangle(treeViewBrush, NodeBounds(e.Node));
                e.Graphics.DrawString(e.Node.Text, this.treeView.Font, treeViewTextBrush, e.Node.Bounds.Left, e.Node.Bounds.Top);
            }

            // Image center
            int xPos = (int)((e.Node.Bounds.Left - (int)(8 * 0.5)) * 0.5);
            int yPos = (int)((e.Node.Bounds.Top + e.Node.Bounds.Bottom - 8) * 0.5);

            Icon ic;

            if (e.Node.Level == 0 && e.Node.FirstNode != null)
            {
                if (!e.Node.IsExpanded)
                {

                    e.Graphics.FillRectangle(treeViewBrush, xPos, yPos, 8, 8);
                    ic = Icon.FromHandle(((Bitmap)global::FMScout.Properties.Resources.NodeDefault).GetHicon());
                }
                else
                {
                    e.Graphics.FillRectangle(treeViewBrush, xPos, yPos, 8, 8);
                    ic = Icon.FromHandle(((Bitmap)global::FMScout.Properties.Resources.NodeExpanded).GetHicon());
                }

                e.Graphics.DrawIcon(ic, xPos, yPos);
            }
        }

        // Selects a node that is clicked on its label or tag text.
        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode clickedNode = this.treeView.GetNodeAt(e.X, e.Y);
            if (clickedNode != null)
            {
                if (NodeBounds(clickedNode).Contains(e.X, e.Y))
                    this.treeView.SelectedNode = clickedNode;
            }
        }

        // Returns the bounds of the specified node, including the region 
        // occupied by the node label and any node tag displayed.
        private Rectangle NodeBounds(TreeNode node)
        {
            // Set the return value to the normal node bounds.
            Rectangle bounds = node.Bounds;
            if (node.Text != null)
            {
                // Retrieve a Graphics object from the TreeView handle
                // and use it to calculate the display width of the tag.
                Graphics g = this.treeView.CreateGraphics();
                int tagWidth = (int)g.MeasureString(node.Text.ToString(), this.treeView.Font).Width;

                // Adjust the node bounds using the calculated value.
                //bounds.Offset(tagWidth / 2, 0);
                bounds = Rectangle.Inflate(bounds, 4, 0);
                bounds.Offset(2, 0);
                g.Dispose();
            }

            return bounds;

        }

        public void setPlayerEditing(bool editing)
        {
            // player profile tab
            //this.PlayerProfileIDTextBox.ReadOnly = !editing;
            //this.PlayerProfileFullNameTextBox.ReadOnly = !editing;
            //this.PlayerProfileClubTextBox.ReadOnly = !editing;
            //this.PlayerProfileTeamSquadTextBox.ReadOnly = !editing;
            //this.PlayerProfileNationalityTextBox.ReadOnly = !editing;
            //this.PlayerProfileFormedTextBox.ReadOnly = !editing;
            //this.PlayerProfileEUMemberTextBox.ReadOnly = !editing;
            //this.PlayerProfileHomeGrownTextBox.ReadOnly = !editing;
            //this.PlayerProfileInternationalTextBox.ReadOnly = !editing;
            //this.PlayerProfileBirthDateTextBox.ReadOnly = !editing;
            //this.PlayerProfileAgeTextBox.ReadOnly = !editing;
            this.PlayerProfileHeightTextBox.ReadOnly = !editing;
            this.PlayerProfileValueTextBox.ReadOnly = !editing;
            this.PlayerProfileSaleValueTextBox.ReadOnly = !editing;
            //this.PlayerProfileContractStartedTextBox.ReadOnly = !editing;
            //this.PlayerProfileContractExpiryTextBox.ReadOnly = !editing;
            this.PlayerProfileWageTextBox.ReadOnly = !editing;
            this.PlayerProfileCATextBox.ReadOnly = !editing;
            this.PlayerProfileWeightTextBox.ReadOnly = !editing;
            //this.PlayerProfilePreferredFootTextBox.ReadOnly = !editing;
            //this.PlayerProfileContractExpiryTextBox.ReadOnly = !editing;
            this.PlayerProfilePATextBox.ReadOnly = !editing;
            //this.PlayerProfileShortlistTextBox.ReadOnly = !editing;
            //this.PlayerProfilePositionTextBox.ReadOnly = !editing;
            this.PlayerProfileAppearanceBonusTextBox.ReadOnly = !editing;
            this.PlayerProfileGoalBonusTextBox.ReadOnly = !editing;
            this.PlayerProfileCleanSheetBonusTextBox.ReadOnly = !editing;
            this.PlayerProfileAerialAbilityTextBox.ReadOnly = !editing;
            this.PlayerProfileCommandOfAreaTextBox.ReadOnly = !editing;
            this.PlayerProfileCommunicationTextBox.ReadOnly = !editing;
            this.PlayerProfileEccentricityTextBox.ReadOnly = !editing;
            this.PlayerProfileHandlingTextBox.ReadOnly = !editing;
            this.PlayerProfileKickingTextBox.ReadOnly = !editing;
            this.PlayerProfileOneOnOnesTextBox.ReadOnly = !editing;
            this.PlayerProfileReflexesTextBox.ReadOnly = !editing;
            this.PlayerProfileRushingOutTextBox.ReadOnly = !editing;
            this.PlayerProfileTendencyToPunchTextBox.ReadOnly = !editing;
            this.PlayerProfileThrowingTextBox.ReadOnly = !editing;
            this.PlayerProfileCornersTextBox.ReadOnly = !editing;
            this.PlayerProfileCrossingTextBox.ReadOnly = !editing;
            this.PlayerProfileDribblingTextBox.ReadOnly = !editing;
            this.PlayerProfileFinishingTextBox.ReadOnly = !editing;
            this.PlayerProfileFirstTouchTextBox.ReadOnly = !editing;
            this.PlayerProfileFreeKicksTextBox.ReadOnly = !editing;
            this.PlayerProfileHeadingTextBox.ReadOnly = !editing;
            this.PlayerProfileLongShotsTextBox.ReadOnly = !editing;
            this.PlayerProfileLongThrowsTextBox.ReadOnly = !editing;
            this.PlayerProfileMarkingTextBox.ReadOnly = !editing;
            this.PlayerProfilePassingTextBox.ReadOnly = !editing;
            this.PlayerProfilePenaltyTakingTextBox.ReadOnly = !editing;
            this.PlayerProfileTacklingTextBox.ReadOnly = !editing;
            this.PlayerProfileTechniqueTextBox.ReadOnly = !editing;
            this.PlayerProfileAccelerationTextBox.ReadOnly = !editing;
            this.PlayerProfileAgilityTextBox.ReadOnly = !editing;
            this.PlayerProfileBalanceTextBox.ReadOnly = !editing;
            this.PlayerProfileJumpingTextBox.ReadOnly = !editing;
            this.PlayerProfileNaturalFitnessTextBox.ReadOnly = !editing;
            this.PlayerProfilePaceTextBox.ReadOnly = !editing;
            this.PlayerProfileStaminaTextBox.ReadOnly = !editing;
            this.PlayerProfileStrengthTextBox.ReadOnly = !editing;
            this.PlayerProfileAggressionTextBox.ReadOnly = !editing;
            this.PlayerProfileAnticipationTextBox.ReadOnly = !editing;
            this.PlayerProfileBraveryTextBox.ReadOnly = !editing;
            this.PlayerProfileComposureTextBox.ReadOnly = !editing;
            this.PlayerProfileConcentrationTextBox.ReadOnly = !editing;
            this.PlayerProfileCreativityTextBox.ReadOnly = !editing;
            this.PlayerProfileDecisionsTextBox.ReadOnly = !editing;
            this.PlayerProfileDeterminationTextBox.ReadOnly = !editing;
            this.PlayerProfileFlairTextBox.ReadOnly = !editing;
            this.PlayerProfileInfluenceTextBox.ReadOnly = !editing;
            this.PlayerProfileOffTheBallTextBox.ReadOnly = !editing;
            this.PlayerProfilePositioningTextBox.ReadOnly = !editing;
            this.PlayerProfileTeamWorkTextBox.ReadOnly = !editing;
            this.PlayerProfileWorkRateTextBox.ReadOnly = !editing;
            this.PlayerProfileConsistencyTextBox.ReadOnly = !editing;
            this.PlayerProfileDirtynessTextBox.ReadOnly = !editing;
            this.PlayerProfileImportantMatchesTextBox.ReadOnly = !editing;
            this.PlayerProfileInjuryPronenessTextBox.ReadOnly = !editing;
            this.PlayerProfileVersatilityTextBox.ReadOnly = !editing;
            this.PlayerProfileAdaptabilityTextBox.ReadOnly = !editing;
            this.PlayerProfileAmbitionTextBox.ReadOnly = !editing;
            this.PlayerProfileControversyTextBox.ReadOnly = !editing;
            this.PlayerProfileLoyaltyTextBox.ReadOnly = !editing;
            this.PlayerProfilePressureTextBox.ReadOnly = !editing;
            this.PlayerProfileProfessionalismTextBox.ReadOnly = !editing;
            this.PlayerProfileSportsmanshipTextBox.ReadOnly = !editing;
            this.PlayerProfileTemperamentTextBox.ReadOnly = !editing;

            this.PlayerProfileConditionTextBox.ReadOnly = !editing;
            this.PlayerProfileMoraleTextBox.ReadOnly = !editing;
            //this.PlayerProfileHappinessTextBox.ReadOnly = !editing;
            //this.PlayerProfileJadednessTextBox.ReadOnly = !editing;
            //this.PlayerProfileSquadNoTextBox.ReadOnly = !editing;
            this.PlayerProfileLeftFootTextBox.ReadOnly = !editing;
            this.PlayerProfileRightFootTextBox.ReadOnly = !editing;
            this.PlayerProfileWorldReputationTextBox.ReadOnly = !editing;
            this.PlayerProfileNationalReputationTextBox.ReadOnly = !editing;
            this.PlayerProfileLocalReputationTextBox.ReadOnly = !editing;
        }

        public void setStaffEditing(bool editing)
        {
            // staff profile tab
            //this.StaffProfileIDTextBox.ReadOnly = !editing;
            //this.StaffProfileFullNameTextBox.ReadOnly = !editing;
            //this.StaffProfileClubTextBox.ReadOnly = !editing;
            //this.StaffProfileNationalityTextBox.ReadOnly = !editing;
            //this.StaffProfileInternationalTextBox.ReadOnly = !editing;
            //this.StaffProfileBirthDateTextBox.ReadOnly = !editing;
            //this.StaffProfileAgeTextBox.ReadOnly = !editing;
            //this.StaffProfileContractStartedTextBox.ReadOnly = !editing;
            //this.StaffProfileContractExpiryTextBox.ReadOnly = !editing;
            //this.StaffProfileWageTextBox.ReadOnly = !editing;
            this.StaffProfileCATextBox.ReadOnly = !editing;
            this.StaffProfilePATextBox.ReadOnly = !editing;
            //this.StaffProfileRoleTextBox.ReadOnly = !editing;
            this.StaffProfileAttackingTextBox.ReadOnly = !editing;
            this.StaffProfileDefendingTextBox.ReadOnly = !editing;
            this.StaffProfileFitnessTextBox.ReadOnly = !editing;
            this.StaffProfileGoalkeepersTextBox.ReadOnly = !editing;
            this.StaffProfileMentalTextBox.ReadOnly = !editing;
            this.StaffProfilePlayerTextBox.ReadOnly = !editing;
            this.StaffProfileTacticalTextBox.ReadOnly = !editing;
            this.StaffProfileTechnicalTextBox.ReadOnly = !editing;
            this.StaffProfileManManagementTextBox.ReadOnly = !editing;
            this.StaffProfileWorkingWithYoungstersTextBox.ReadOnly = !editing;
            this.StaffProfileAdaptabilityTextBox.ReadOnly = !editing;
            this.StaffProfileAmbitionTextBox.ReadOnly = !editing;
            this.StaffProfileControversyTextBox.ReadOnly = !editing;
            this.StaffProfileDeterminationTextBox.ReadOnly = !editing;
            this.StaffProfileLoyaltyTextBox.ReadOnly = !editing;
            this.StaffProfilePressureTextBox.ReadOnly = !editing;
            this.StaffProfileProfessionalismTextBox.ReadOnly = !editing;
            this.StaffProfileSportsmanshipTextBox.ReadOnly = !editing;
            this.StaffProfileTemperamentTextBox.ReadOnly = !editing;
            this.StaffProfileJudgingPlayerAbilityTextBox.ReadOnly = !editing;
            this.StaffProfileJudgingPlayerPotentialTextBox.ReadOnly = !editing;
            this.StaffProfileLevelOfDisciplineTextBox.ReadOnly = !editing;
            this.StaffProfileMotivatingTextBox.ReadOnly = !editing;
            this.StaffProfilePhysiotherapyTextBox.ReadOnly = !editing;
            this.StaffProfileTacticalKnowledgeTextBox.ReadOnly = !editing;
            this.StaffProfileDepthTextBox.ReadOnly = !editing;
            this.StaffProfileDirectnessTextBox.ReadOnly = !editing;
            this.StaffProfileFlamboyancyTextBox.ReadOnly = !editing;
            this.StaffProfileFlexibilityTextBox.ReadOnly = !editing;
            this.StaffProfileFreeRolesTextBox.ReadOnly = !editing;
            this.StaffProfileMarkingTextBox.ReadOnly = !editing;
            this.StaffProfileOffsideTextBox.ReadOnly = !editing;
            this.StaffProfilePressingTextBox.ReadOnly = !editing;
            this.StaffProfileSittingBackTextBox.ReadOnly = !editing;
            this.StaffProfileTempoTextBox.ReadOnly = !editing;
            this.StaffProfileUseOfPlaymakerTextBox.ReadOnly = !editing;
            this.StaffProfileUseOfSubstitutionsTextBox.ReadOnly = !editing;
            this.StaffProfileWidthTextBox.ReadOnly = !editing;
            this.StaffProfileBuyingPlayersTextBox.ReadOnly = !editing;
            this.StaffProfileHardnessOfTrainingTextBox.ReadOnly = !editing;
            this.StaffProfileMindGamesTextBox.ReadOnly = !editing;
            this.StaffProfileSquadRotationTextBox.ReadOnly = !editing;
            this.StaffProfileBusinessTextBox.ReadOnly = !editing;
            this.StaffProfileInterferenceTextBox.ReadOnly = !editing;
            this.StaffProfilePatienceTextBox.ReadOnly = !editing;
            this.StaffProfileResourcesTextBox.ReadOnly = !editing;
            this.StaffProfileWorldReputationTextBox.ReadOnly = !editing;
            this.StaffProfileNationalReputationTextBox.ReadOnly = !editing;
            this.StaffProfileLocalReputationTextBox.ReadOnly = !editing;
        }

        public void setTeamEditing(bool editing)
        {
            // teams profile tab
            //this.TeamProfileIDTextBox.ReadOnly = !editing;
            //this.TeamProfileNameTextBox.ReadOnly = !editing;
            //this.TeamProfileNationalityTextBox.ReadOnly = !editing;
            //this.TeamProfileYearFoundedTextBox.ReadOnly = !editing;
            //this.TeamProfileNationalTextBox.ReadOnly = !editing;
            //this.TeamProfileStatusTextBox.ReadOnly = !editing;
            //this.TeamProfileNoPlayersTextBox.ReadOnly = !editing;
            //this.TeamProfileNoScoutsTextBox.ReadOnly = !editing;
            this.TeamProfileMaxAffiliatedClubsTextBox.ReadOnly = !editing;
            this.TeamProfileAffiliatedClubsTextBox.ReadOnly = !editing;
            this.TeamProfileTrainingGroundTextBox.ReadOnly = !editing;
            this.TeamProfileYouthGroundTextBox.ReadOnly = !editing;
            this.TeamProfileYouthAcademyTextBox.ReadOnly = !editing;
            this.TeamProfileReputationTextBox.ReadOnly = !editing;
            this.TeamProfileTotalTransferTextBox.ReadOnly = !editing;
            this.TeamProfileRemainingTransferTextBox.ReadOnly = !editing;
            this.TeamProfileBalanceTextBox.ReadOnly = !editing;
            this.TeamProfileTotalWageTextBox.ReadOnly = !editing;
            this.TeamProfileUsedWageTextBox.ReadOnly = !editing;
            //this.TeamProfileRevenueAvailableTextBox.ReadOnly = !editing;
            //this.TeamProfileStadiumIDTextBox.ReadOnly = !editing;
            //this.TeamProfileStadiumNameTextBox.ReadOnly = !editing;
            //this.TeamProfileStadiumOwnerTextBox.ReadOnly = !editing;
            //this.TeamProfileStadiumLocationTextBox.ReadOnly = !editing;
            //this.TeamProfileStadiumNearbyStadiumTextBox.ReadOnly = !editing;
            this.TeamProfileStadiumDecayTextBox.ReadOnly = !editing;
            this.TeamProfileStadiumFieldWidthTextBox.ReadOnly = !editing;
            this.TeamProfileStadiumFieldLengthTextBox.ReadOnly = !editing;
            this.TeamProfileStadiumCurrentCapacityTextBox.ReadOnly = !editing;
            this.TeamProfileStadiumSeatingCapacityTextBox.ReadOnly = !editing;
            this.TeamProfileStadiumExpansionCapacityTextBox.ReadOnly = !editing;
            this.TeamProfileStadiumUsedCapacityTextBox.ReadOnly = !editing;
            this.TeamProfileMaxAttendanceTextBox.ReadOnly = !editing;
            this.TeamProfileAverageAttendanceTextBox.ReadOnly = !editing;
            this.TeamProfileMinAttendanceTextBox.ReadOnly = !editing;
        }

        internal void calculatePR()
        {
            float playersPRIDprogress = 0.0f;
            int playersCounter = 0;
            DateTime timerStart = DateTime.Now;
            float percent = 1 / (float)fm.Players.Count * 100.0f;


            foreach (Player player in fm.Players)
            {
                if (player.ToString().Length == 0)
                    continue;

                //calculatePlayerPR(player);

                playersPRIDprogress = playersCounter * percent;
                ++playersCounter;
                this.loadingForm.reportProgress(playersPRIDprogress);
            }

            this.loadingForm.reportProgress(100.0f);

            DateTime timerEnd = DateTime.Now;
            TimeSpan timer = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
            float time = timer.Seconds;
        }

        internal void calculatePlayerPR(Player player)
        {
            ++playersPRTotal;
            float GKcoef = 1 / (0.4f + 0.4f + 0.6f - 0.2f + 1.0f + 0.4f + 1.0f + 0.2f + 0.4f + 0.1f + 0.6f + 0.1f + 0.2f + /*0.1f +*/ 0.1f
                    + 0.2f + 0.1f + 0.8f + 0.2f + 0.1f + 0.8f + 0.2f + /*0.1f*/ -0.1f + 1.0f);

            float DCcoef = 1 / (0.1f + 1.0f + 0.8f + 0.2f + 1.0f + 0.2f + 0.2f + 0.4f + 0.6f + 0.2f + 0.4f + 0.2f + 0.8f + 0.4f +
                    0.2f + 0.4f + 0.2f + 0.2f + 0.8f + 0.4f + 0.2f + 0.8f - 1.0f + 1.0f);

            float DRcoef = 1 / (0.4f + 0.2f + 0.1f + 0.4f + 0.6f + 0.2f + 1.0f + 0.2f + 0.2f + 0.8f + 0.4f + 0.2f + 0.4f + 0.2f +
                    0.4f + 0.1f + 1.0f + 0.2f + 0.2f + 0.6f + 0.2f + 0.2f + 0.6f + 0.6f + 0.6f + 0.6f - 1.0f + 0.2f + 1.0f);

            float DLcoef = 1 / (0.4f + 0.2f + 0.1f + 0.4f + 0.6f + 0.2f + 1.0f + 0.2f + 0.2f + 0.8f + 0.4f + 0.2f + 0.4f + 0.2f +
                0.4f + 0.1f + 1.0f + 0.2f + 0.2f + 0.6f + 0.2f + 0.2f + 0.6f + 0.6f + 0.6f + 0.6f - 1.0f + 0.2f + 1.0f);

            float DMCcoef = 1 / (0.1f + 0.2f + 0.1f + 0.2f + 0.2f + 0.2f + 0.6f + 1.0f + 0.4f + 0.2f + 0.6f + 0.2f + 0.3f + 0.2f +
                    0.4f + 0.4f + 0.1f + 0.2f + 0.8f + 0.3f + 0.8f + 0.6f + 0.2f + 0.2f + 0.2f + 0.6f + 0.8f + 0.6f - 1.0f + 0.5f + 0.5f);

            float DMRcoef = 1 / (0.6f + 0.4f + 0.1f + 0.2f + 0.2f + 0.4f + 0.4f + 0.8f + 0.4f + 0.2f + 0.8f + 0.2f + 0.2f +
                    0.2f + 0.2f + 0.4f + 0.1f + 0.2f + 1.0f + 0.2f + 0.3f + 0.8f + 0.2f + 0.2f + 0.2f + 0.8f + 0.8f +
                    0.4f - 1.0f + 0.2f + 1.0f);

            float DMLcoef = 1 / (0.6f + 0.4f + 0.1f + 0.2f + 0.2f + 0.4f + 0.4f + 0.8f + 0.4f + 0.2f + 0.8f + 0.2f + 0.2f +
                   0.2f + 0.2f + 0.4f + 0.1f + 0.2f + 1.0f + 0.2f + 0.3f + 0.8f + 0.2f + 0.2f + 0.2f + 0.8f + 0.8f +
                   0.4f - 1.0f + 0.2f + 1.0f);

            float AMCcoef = 1 / (0.1f + 0.6f + 0.4f + 0.3f + 0.1f + 1.0f + 0.2f + 0.8f + 0.4f + 0.4f + 0.1f + 1.0f + 0.4f + 0.1f +
                    0.6f + 0.2f + 0.4f + 0.4f + 0.8f + 0.2f + 0.2f + 0.8f + 0.6f + 0.2f + -1.0f + 0.5f + 0.5f);
            float AMRcoef = 1 / (1.0f + 1.0f + 0.4f + 0.3f + 0.1f + 0.4f + 0.2f + 0.8f + 0.4f + 0.4f + 0.1f + 0.4f + 0.2f + 0.1f +
                    0.8f + 0.2f + 0.2f + 0.4f + 1.0f + 0.2f + 0.2f + 1.0f + 0.6f + 0.2f - 1.0f + 0.2f + 1.0f);
            float AMLcoef = 1 / (1.0f + 1.0f + 0.4f + 0.3f + 0.1f + 0.4f + 0.2f + 0.8f + 0.4f + 0.4f + 0.1f + 0.4f + 0.2f + 0.1f +
                   0.8f + 0.2f + 0.2f + 0.4f + 1.0f + 0.2f + 0.2f + 1.0f + 0.6f + 0.2f - 1.0f + 0.2f + 1.0f);

            float FCQuickcoef = 1 / (0.2f + 0.8f + 1.0f + 0.4f + 0.4f + 0.6f + 0.8f + 0.4f + 0.6f + 0.1f + 0.4f + 0.2f + 0.1f +
                    0.8f + 0.1f + 0.2f + 1.0f + 0.4f + 0.2f + 0.2f + 1.0f + 0.4f + 0.2f - 1.0f + 0.5f + 0.5f);
            float FCStrongcoef = 1 / (0.2f + 0.4f + 0.8f + 0.4f + 1.0f + 0.4f + 0.4f + 0.4f + 0.2f + 0.4f + 0.1f + 0.2f + 0.2f +
                    0.1f + 1.0f + 0.2f + 0.1f + 0.2f + 0.4f + 0.2f + 0.2f + 1.0f + 0.4f + 0.4f + 1.0f - 1.0f + 0.5f + 0.5f);

            PositionalRatings pr = new PositionalRatings();

            // GK
            pr.GK += (int)player.GoalKeepingSkills.AerialAbility * 0.4f
            + (int)player.GoalKeepingSkills.CommandOfArea * 0.4f
            + (int)player.GoalKeepingSkills.Communication * 0.6f
            + (int)player.GoalKeepingSkills.Eccentricity * -0.2f
            + (int)player.GoalKeepingSkills.Handling * 1.0f
            + (int)player.GoalKeepingSkills.OneOnOnes * 0.4f
            + (int)player.GoalKeepingSkills.Reflexes * 1.0f

            + (int)player.PhysicalSkills.Acceleration * 0.2f
            + (int)player.PhysicalSkills.Agility * 0.4f
            + (int)player.PhysicalSkills.Balance * 0.1f
            + (int)player.PhysicalSkills.Jumping * 0.6f
            + (int)player.PhysicalSkills.NaturalFitness * 0.1f
            + (int)player.PhysicalSkills.Strength * 0.2f

            //+ (int)player.MentalSkills.Aggression * 0.1f
            + (int)player.MentalSkills.Anticipation * 0.1f
            + (int)player.MentalSkills.Bravery * 0.4f
            + (int)player.MentalSkills.Composure * 0.1f
            + (int)player.MentalSkills.Concentration * 0.8f
            + (int)player.MentalSkills.Decisions * 0.2f
            + (int)player.MentalSkills.Influence * 0.2f
            + (int)player.MentalSkills.Positioning * 0.8f
            + (int)player.MentalSkills.Teamwork * 0.2f
            + (int)player.HiddenSkills.InjuryProness * -1.0f;

            if (player.Length > 190)
                pr.GK += 100;
            else if (player.Length > 180)
                pr.GK += 50;

            pr.GK *= GKcoef;

            pr.bestPos = "GK";
            pr.bestPosR = pr.GK;
            pr.desc = "Goalkeeper";

            // DC
            pr.DC += (int)player.TechnicalSkills.FirstTouch * 0.1f
            + (int)player.TechnicalSkills.Heading * 1.0f
            + (int)player.TechnicalSkills.Marking * 0.8f
            + (int)player.TechnicalSkills.Passing * 0.2f
            + (int)player.TechnicalSkills.Tackling * 1.0f
            + (int)player.TechnicalSkills.Technique * 0.2f

            + (int)player.MentalSkills.Aggression * 0.2f
            + (int)player.MentalSkills.Anticipation * 0.4f
            + (int)player.MentalSkills.Bravery * 0.6f
            + (int)player.MentalSkills.Composure * 0.2f
            + (int)player.MentalSkills.Concentration * 0.4f
                //+ (int)player.MentalSkills.Decisions * 0.4f
            + (int)player.MentalSkills.Influence * 0.2f
            + (int)player.MentalSkills.Positioning * 0.8f
            + (int)player.MentalSkills.Teamwork * 0.4f
            + (int)player.MentalSkills.Workrate * 0.2f

            + (int)player.PhysicalSkills.Acceleration * 0.4f
            + (int)player.PhysicalSkills.Agility * 0.2f
            + (int)player.PhysicalSkills.Balance * 0.2f
            + (int)player.PhysicalSkills.Jumping * 0.8f
            + (int)player.PhysicalSkills.Pace * 0.4f
            + (int)player.PhysicalSkills.Stamina * 0.2f
            + (int)player.PhysicalSkills.Strength * 0.8f

            + (int)player.HiddenSkills.InjuryProness * -1.0f;

            if (player.Length > 190)
                pr.DC += 100;
            else if (player.Length > 180)
                pr.DC += 50;
            pr.DC *= DCcoef;

            if (pr.DC > pr.bestPosR)
            {
                pr.bestPos = "DC";
                pr.bestPosR = pr.DC;
                pr.desc = "Central Defender";
            }

            // DRL
            pr.DR += (int)player.TechnicalSkills.Crossing * 0.4f
            + (int)player.TechnicalSkills.Dribbling * 0.2f
            + (int)player.TechnicalSkills.FirstTouch * 0.1f
            + (int)player.TechnicalSkills.Heading * 0.4f
            + (int)player.TechnicalSkills.Marking * 0.6f
            + (int)player.TechnicalSkills.Passing * 0.2f
            + (int)player.TechnicalSkills.Tackling * 1.0f
            + (int)player.TechnicalSkills.Technique * 0.2f

            + (int)player.MentalSkills.Aggression * 0.2f
            + (int)player.MentalSkills.Anticipation * 0.8f
            + (int)player.MentalSkills.Bravery * 0.4f
            + (int)player.MentalSkills.Composure * 0.2f
            + (int)player.MentalSkills.Concentration * 0.4f
            + (int)player.MentalSkills.Creativity * 0.2f
            + (int)player.MentalSkills.Decisions * 0.4f
            + (int)player.MentalSkills.Influence * 0.1f
            + (int)player.MentalSkills.Positioning * 1.0f
            + (int)player.MentalSkills.Teamwork * 0.2f
            + (int)player.MentalSkills.Workrate * 0.2f

            + (int)player.PhysicalSkills.Acceleration * 0.6f
            + (int)player.PhysicalSkills.Agility * 0.2f
            + (int)player.PhysicalSkills.Balance * 0.2f
            + (int)player.PhysicalSkills.Jumping * 0.6f
                //+ (int)player.PhysicalSkills.NaturalFitness * 0.1f
            + (int)player.PhysicalSkills.Pace * 0.6f
            + (int)player.PhysicalSkills.Stamina * 0.6f
            + (int)player.PhysicalSkills.Strength * 0.6f

            + (int)player.HiddenSkills.InjuryProness * -1.0f;

            pr.DL = pr.DR;

            pr.DR += (int)player.PhysicalSkills.LeftFoot * 0.2f + (int)player.PhysicalSkills.RightFoot;
            pr.DL += (int)player.PhysicalSkills.RightFoot * 0.2f + (int)player.PhysicalSkills.LeftFoot;

            pr.DR *= DRcoef;
            pr.DL *= DLcoef;

            if (pr.DR > pr.bestPosR)
            {
                pr.bestPos = "DR";
                pr.bestPosR = pr.DR;
                pr.desc = "Right Back";
                if ((int)player.MentalSkills.Creativity > pr.DR &&
                    (int)player.TechnicalSkills.Crossing > pr.DR &&
                    (int)player.TechnicalSkills.Dribbling > pr.DR)
                {
                    pr.desc = "Attacking Right Back";
                }
            }
            if (pr.DL > pr.bestPosR)
            {
                pr.bestPos = "DL";
                pr.bestPosR = pr.DL;
                pr.desc = "Left Back";
                if ((int)player.MentalSkills.Creativity > pr.DL &&
                    (int)player.TechnicalSkills.Crossing > pr.DL &&
                    (int)player.TechnicalSkills.Dribbling > pr.DL)
                {
                    pr.desc = "Attacking Left Back";
                }
            }

            // DMC
            pr.DMC += (int)player.TechnicalSkills.Crossing * 0.1f
            + (int)player.TechnicalSkills.Dribbling * 0.2f
            + (int)player.TechnicalSkills.Finishing * 0.1f
            + (int)player.TechnicalSkills.FirstTouch * 0.2f
            + (int)player.TechnicalSkills.Heading * 0.2f
            + (int)player.TechnicalSkills.Marking * 0.2f
            + (int)player.TechnicalSkills.Passing * 0.6f
            + (int)player.TechnicalSkills.Tackling * 1.0f
            + (int)player.TechnicalSkills.Technique * 0.4f

            + (int)player.MentalSkills.Aggression * 0.2f
            + (int)player.MentalSkills.Anticipation * 0.6f
            + (int)player.MentalSkills.Bravery * 0.2f
            + (int)player.MentalSkills.Composure * 0.3f
            + (int)player.MentalSkills.Concentration * 0.2f
            + (int)player.MentalSkills.Creativity * 0.4f
            + (int)player.MentalSkills.Decisions * 0.4f
            + (int)player.MentalSkills.Influence * 0.1f
            + (int)player.MentalSkills.OffTheBall * 0.2f
            + (int)player.MentalSkills.Positioning * 0.8f
            + (int)player.MentalSkills.Teamwork * 0.3f
            + (int)player.MentalSkills.Workrate * 0.8f

            + (int)player.PhysicalSkills.Acceleration * 0.6f
            + (int)player.PhysicalSkills.Agility * 0.2f
            + (int)player.PhysicalSkills.Balance * 0.2f
            + (int)player.PhysicalSkills.Jumping * 0.2f
            + (int)player.PhysicalSkills.Pace * 0.6f
            + (int)player.PhysicalSkills.Stamina * 0.8f
            + (int)player.PhysicalSkills.Strength * 0.6f

            + (int)player.HiddenSkills.InjuryProness * -1.0f

            + (int)player.PhysicalSkills.LeftFoot * 0.5f
            + (int)player.PhysicalSkills.RightFoot * 0.5f;

            pr.DMC *= DMCcoef;
            if (pr.DMC > pr.bestPosR)
            {
                pr.bestPos = "DMC";
                pr.bestPosR = pr.DMC;
                pr.desc = "Defensive Midfielder";
                if ((int)player.MentalSkills.Creativity > pr.DMC &&
                    (int)player.MentalSkills.Flair > pr.DMC &&
                    (int)player.TechnicalSkills.Technique > pr.DMC)
                {
                    pr.desc = "Defensive Midfielder Playmaker";
                }
            }

            // DMRL
            pr.DMR += (int)player.TechnicalSkills.Crossing * 0.6f
            + (int)player.TechnicalSkills.Dribbling * 0.4f
            + (int)player.TechnicalSkills.Finishing * 0.1f
            + (int)player.TechnicalSkills.FirstTouch * 0.2f
            + (int)player.TechnicalSkills.Heading * 0.2f
            + (int)player.TechnicalSkills.Marking * 0.4f
            + (int)player.TechnicalSkills.Passing * 0.4f
            + (int)player.TechnicalSkills.Tackling * 0.8f
            + (int)player.TechnicalSkills.Technique * 0.4f

            + (int)player.MentalSkills.Aggression * 0.2f
            + (int)player.MentalSkills.Anticipation * 0.8f
            + (int)player.MentalSkills.Bravery * 0.2f
            + (int)player.MentalSkills.Composure * 0.2f
            + (int)player.MentalSkills.Concentration * 0.2f
            + (int)player.MentalSkills.Creativity * 0.2f
            + (int)player.MentalSkills.Decisions * 0.4f
            + (int)player.MentalSkills.Influence * 0.1f
            + (int)player.MentalSkills.OffTheBall * 0.2f
            + (int)player.MentalSkills.Positioning * 1.0f
            + (int)player.MentalSkills.Teamwork * 0.2f
            + (int)player.MentalSkills.Workrate * 0.3f

            + (int)player.PhysicalSkills.Acceleration * 0.8f
            + (int)player.PhysicalSkills.Agility * 0.2f
            + (int)player.PhysicalSkills.Balance * 0.2f
            + (int)player.PhysicalSkills.Jumping * 0.2f
            + (int)player.PhysicalSkills.Pace * 0.8f
            + (int)player.PhysicalSkills.Stamina * 0.8f
            + (int)player.PhysicalSkills.Strength * 0.4f

            + (int)player.HiddenSkills.InjuryProness * -1.0f;

            pr.DML = pr.DMR;

            pr.DMR += (int)player.PhysicalSkills.LeftFoot * 0.2f + (int)player.PhysicalSkills.RightFoot;
            pr.DML += (int)player.PhysicalSkills.RightFoot * 0.2f + (int)player.PhysicalSkills.LeftFoot;

            pr.DMR *= DMRcoef;
            pr.DML *= DMLcoef;

            if (pr.DMR > pr.bestPosR)
            {
                pr.bestPos = "DMR";
                pr.bestPosR = pr.DMR;
                pr.desc = "Defending Right Midfielder";
            }
            if (pr.DML > pr.bestPosR)
            {
                pr.bestPos = "DML";
                pr.bestPosR = pr.DML;
                pr.desc = "Defending Left Midfielder";
            }

            // AMC
            pr.AMC += (int)player.TechnicalSkills.Crossing * 0.1f
            + (int)player.TechnicalSkills.Dribbling * 0.6f
            + (int)player.TechnicalSkills.Finishing * 0.4f
            + (int)player.TechnicalSkills.FirstTouch * 0.3f
            + (int)player.TechnicalSkills.Heading * 0.1f
            + (int)player.TechnicalSkills.Passing * 1.0f
            + (int)player.TechnicalSkills.Tackling * 0.2f
            + (int)player.TechnicalSkills.Technique * 0.8f

            + (int)player.MentalSkills.Anticipation * 0.4f
                //+ (int)player.MentalSkills.Bravery * 0.3f
            + (int)player.MentalSkills.Composure * 0.4f
            + (int)player.MentalSkills.Concentration * 0.1f
            + (int)player.MentalSkills.Creativity * 1.0f
            + (int)player.MentalSkills.Decisions * 0.4f
            + (int)player.MentalSkills.Influence * 0.1f
                //+ (int)player.MentalSkills.Flair * 0.5f
            + (int)player.MentalSkills.OffTheBall * 0.6f
            + (int)player.MentalSkills.Positioning * 0.2f
            + (int)player.MentalSkills.Teamwork * 0.4f
            + (int)player.MentalSkills.Workrate * 0.4f

            + (int)player.PhysicalSkills.Acceleration * 0.8f
            + (int)player.PhysicalSkills.Agility * 0.2f
            + (int)player.PhysicalSkills.Balance * 0.2f
                //+ (int)player.PhysicalSkills.Jumping * 0.1f
            + (int)player.PhysicalSkills.Pace * 0.8f
            + (int)player.PhysicalSkills.Stamina * 0.6f
            + (int)player.PhysicalSkills.Strength * 0.2f

            + (int)player.HiddenSkills.InjuryProness * -1.0f

            + (int)player.PhysicalSkills.LeftFoot * 0.5f
            + (int)player.PhysicalSkills.RightFoot * 0.5f;

            pr.AMC *= AMCcoef;
            if (pr.AMC > pr.bestPosR)
            {
                pr.bestPos = "AMC";
                pr.bestPosR = pr.AMC;
                pr.desc = "Attacking Midfielder";
            }

            // AMRL
            pr.AMR += (int)player.TechnicalSkills.Crossing * 1.0f
            + (int)player.TechnicalSkills.Dribbling * 1.0f
            + (int)player.TechnicalSkills.Finishing * 0.4f
            + (int)player.TechnicalSkills.FirstTouch * 0.3f
            + (int)player.TechnicalSkills.Heading * 0.1f
                //+ (int)player.TechnicalSkills.Marking * 0.1f
            + (int)player.TechnicalSkills.Passing * 0.4f
            + (int)player.TechnicalSkills.Tackling * 0.2f
            + (int)player.TechnicalSkills.Technique * 0.8f

            //+ (int)player.MentalSkills.Aggression * 0.1f
            + (int)player.MentalSkills.Anticipation * 0.4f
                //+ (int)player.MentalSkills.Bravery * 0.1f
            + (int)player.MentalSkills.Composure * 0.4f
            + (int)player.MentalSkills.Concentration * 0.1f
            + (int)player.MentalSkills.Creativity * 0.4f
            + (int)player.MentalSkills.Decisions * 0.2f
            + (int)player.MentalSkills.Influence * 0.1f
            + (int)player.MentalSkills.OffTheBall * 0.8f
            + (int)player.MentalSkills.Positioning * 0.2f
            + (int)player.MentalSkills.Teamwork * 0.2f
            + (int)player.MentalSkills.Workrate * 0.4f

            + (int)player.PhysicalSkills.Acceleration * 1.0f
            + (int)player.PhysicalSkills.Agility * 0.2f
            + (int)player.PhysicalSkills.Balance * 0.2f
                //+ (int)player.PhysicalSkills.Jumping * 0.2f
            + (int)player.PhysicalSkills.Pace * 1.0f
            + (int)player.PhysicalSkills.Stamina * 0.6f
            + (int)player.PhysicalSkills.Strength * 0.2f

            + (int)player.HiddenSkills.InjuryProness * -1.0f;

            pr.AML = pr.AMR;

            pr.AMR += (int)player.PhysicalSkills.LeftFoot * 0.2f + (int)player.PhysicalSkills.RightFoot;
            pr.AML += (int)player.PhysicalSkills.RightFoot * 0.2f + (int)player.PhysicalSkills.LeftFoot;

            pr.AMR *= AMRcoef;
            pr.AML *= AMLcoef;

            if (pr.AMR > pr.bestPosR)
            {
                pr.bestPos = "AMR";
                pr.bestPosR = pr.AMR;
                pr.desc = "Attacking Right Midfielder";
            }
            if (pr.AML > pr.bestPosR)
            {
                pr.bestPos = "AML";
                pr.bestPosR = pr.AML;
                pr.desc = "Attacking Left Midfielder";
            }

            // FC-ST
            pr.FCQuick += (int)player.TechnicalSkills.Crossing * 0.2f
            + (int)player.TechnicalSkills.Dribbling * 0.8f
            + (int)player.TechnicalSkills.Finishing * 1.0f
            + (int)player.TechnicalSkills.FirstTouch * 0.4f
            + (int)player.TechnicalSkills.Heading * 0.4f
            + (int)player.TechnicalSkills.Passing * 0.6f
                //+ (int)player.TechnicalSkills.Tackling * 0.1f
            + (int)player.TechnicalSkills.Technique * 0.8f

            + (int)player.MentalSkills.Anticipation * 0.4f
                //+ (int)player.MentalSkills.Bravery * 0.3f
            + (int)player.MentalSkills.Composure * 0.6f
            + (int)player.MentalSkills.Concentration * 0.1f
            + (int)player.MentalSkills.Creativity * 0.4f
            + (int)player.MentalSkills.Decisions * 0.2f
            + (int)player.MentalSkills.Influence * 0.1f
                //+ (int)player.MentalSkills.Flair * 0.4f
            + (int)player.MentalSkills.OffTheBall * 0.8f
                //+ (int)player.MentalSkills.Positioning * 0.4f
            + (int)player.MentalSkills.Teamwork * 0.1f
            + (int)player.MentalSkills.Workrate * 0.2f

            + (int)player.PhysicalSkills.Acceleration * 1.0f
            + (int)player.PhysicalSkills.Agility * 0.4f
            + (int)player.PhysicalSkills.Balance * 0.2f
            + (int)player.PhysicalSkills.Jumping * 0.2f
            + (int)player.PhysicalSkills.Pace * 1.0f
            + (int)player.PhysicalSkills.Stamina * 0.4f
            + (int)player.PhysicalSkills.Strength * 0.2f

            + (int)player.HiddenSkills.InjuryProness * -1.0f

            + (int)player.PhysicalSkills.LeftFoot * 0.5f
            + (int)player.PhysicalSkills.RightFoot * 0.5f;

            pr.FCQuick *= FCQuickcoef;

            if (pr.FCQuick > pr.bestPosR)
            {
                pr.bestPos = "Quick FC";
                pr.bestPosR = pr.FCQuick;
                pr.desc = "Quick Forward";
            }

            pr.FCStrong += (int)player.TechnicalSkills.Crossing * 0.2f
            + (int)player.TechnicalSkills.Dribbling * 0.4f
            + (int)player.TechnicalSkills.Finishing * 0.8f
            + (int)player.TechnicalSkills.FirstTouch * 0.4f
            + (int)player.TechnicalSkills.Heading * 1.0f
            + (int)player.TechnicalSkills.Passing * 0.4f
                //+ (int)player.TechnicalSkills.Tackling * 0.1f
            + (int)player.TechnicalSkills.Technique * 0.4f

            + (int)player.MentalSkills.Anticipation * 0.4f
            + (int)player.MentalSkills.Bravery * 0.2f
            + (int)player.MentalSkills.Composure * 0.4f
            + (int)player.MentalSkills.Concentration * 0.1f
            + (int)player.MentalSkills.Creativity * 0.2f
            + (int)player.MentalSkills.Decisions * 0.2f
            + (int)player.MentalSkills.Influence * 0.1f
            + (int)player.MentalSkills.OffTheBall * 1.0f
            + (int)player.MentalSkills.Positioning * 0.2f
            + (int)player.MentalSkills.Teamwork * 0.1f
            + (int)player.MentalSkills.Workrate * 0.2f

            + (int)player.PhysicalSkills.Acceleration * 0.4f
            + (int)player.PhysicalSkills.Agility * 0.2f
            + (int)player.PhysicalSkills.Balance * 0.2f
            + (int)player.PhysicalSkills.Jumping * 1.0f
            + (int)player.PhysicalSkills.Pace * 0.4f
            + (int)player.PhysicalSkills.Stamina * 0.4f
            + (int)player.PhysicalSkills.Strength * 1.0f

            + (int)player.HiddenSkills.InjuryProness * -1.0f

            + (int)player.PhysicalSkills.LeftFoot * 0.5f
            + (int)player.PhysicalSkills.RightFoot * 0.5f;

            if (player.Length > 190)
                pr.FCStrong += 100;
            else if (player.Length > 180)
                pr.FCStrong += 50;

            pr.FCStrong *= FCStrongcoef;
            if (pr.FCStrong > pr.bestPosR)
            {
                pr.bestPos = "Strong FC";
                pr.bestPosR = pr.FCStrong;
                pr.desc = "Powerful Forward";
            }

            playersPRID.Add(player.ID, pr);
        }

        internal void calcCR(ref int stars, float value, int max, int dec)
        {
            stars = 1;
            for (int i = 7; i > 1; --i)
            {
                if (value >= max)
                {
                    stars = i;
                    return;
                }
                max -= dec;
            }
        }

        internal void calculateStaffCR(Staff staff)
        {
            ++staffCRTotal;

            CoachingRatings cr = new CoachingRatings();

            float ddm = (int)(staff.StaffMentalTraitsSkills.Determination * 0.2 + 0.5) +
                staff.MentalSkills.LevelOfDiscipline + (int)(staff.MentalSkills.Motivating * 0.2 + 0.5);

            // fitness
            float value = (int)(staff.CoachingSkills.CoachingFitness * 0.2 + 0.5) * 9 + ddm * 2;
            int max = 270;
            int dec = 30;
            calcCR(ref cr.Fitness, value, max, dec);
            cr.BestCR = "Fitness";
            cr.BestCRStars = cr.Fitness;

            // goalkeepers
            value = (int)(staff.CoachingSkills.CoachingGoalkeepers * 0.2 + 0.5) * 2 + ddm;
            max = 90;
            dec = 10;
            calcCR(ref cr.Goalkeepers, value, max, dec);

            if (cr.Goalkeepers > cr.BestCRStars)
            {
                cr.BestCRStars = cr.Goalkeepers;
                cr.BestCR = "Goalkeepers";
            }
            else if (cr.Goalkeepers == cr.BestCRStars) cr.BestCR += "/Goalkeepers";

            // ball control
            value = (int)(staff.CoachingSkills.CoachingTechnical * 0.2 + 0.5) * 6 + (int)(staff.CoachingSkills.CoachingMental * 0.2 + 0.5) * 3 + ddm * 2;
            max = 270;
            dec = 30;
            calcCR(ref cr.BallControl, value, max, dec);

            if (cr.BallControl > cr.BestCRStars)
            {
                cr.BestCRStars = cr.BallControl;
                cr.BestCR = "Ball Control";
            }
            else if (cr.BallControl == cr.BestCRStars) cr.BestCR += "/Ball Control";

            // tactics
            value = (int)(staff.CoachingSkills.CoachingTactical * 0.2 + 0.5) * 2 + ddm;
            max = 90;
            dec = 10;
            calcCR(ref cr.Tactics, value, max, dec);

            if (cr.Tactics > cr.BestCRStars)
            {
                cr.BestCRStars = cr.Tactics;
                cr.BestCR = "Tactics";
            }
            else if (cr.Tactics == cr.BestCRStars) cr.BestCR += "/Tactics";

            // defending
            value = (int)(staff.CoachingSkills.CoachingDefending * 0.2 + 0.5) * 8 + ((int)(staff.CoachingSkills.CoachingTactical * 0.2 + 0.5) + ddm) * 3;
            max = 360;
            dec = 40;
            calcCR(ref cr.Defending, value, max, dec);

            if (cr.Defending > cr.BestCRStars)
            {
                cr.BestCRStars = cr.Defending;
                cr.BestCR = "Defending";
            }
            else if (cr.Defending == cr.BestCRStars) cr.BestCR += "/Defending";

            // attacking
            value = (int)(staff.CoachingSkills.CoachingAttacking * 0.2 + 0.5) * 6 + (int)(staff.CoachingSkills.CoachingTactical * 0.2 + 0.5) * 3 + ddm * 2;
            max = 270;
            dec = 30;
            calcCR(ref cr.Attacking, value, max, dec);

            if (cr.Attacking > cr.BestCRStars)
            {
                cr.BestCRStars = cr.Attacking;
                cr.BestCR = "Attacking";
            }
            else if (cr.Attacking == cr.BestCRStars) cr.BestCR += "/Attacking";

            // shooting
            value = (int)(staff.CoachingSkills.CoachingTechnical * 0.2 + 0.5) * 6 + (int)(staff.CoachingSkills.CoachingAttacking * 0.2 + 0.5) * 3 + ddm * 2;
            max = 270;
            dec = 30;
            calcCR(ref cr.Shooting, value, max, dec);

            if (cr.Shooting > cr.BestCRStars)
            {
                cr.BestCRStars = cr.Shooting;
                cr.BestCR = "Shooting";
            }
            else if (cr.Shooting == cr.BestCRStars) cr.BestCR += "/Shooting";

            // set pieces
            value = ((int)(staff.CoachingSkills.CoachingAttacking * 0.2 + 0.5) + (int)(staff.CoachingSkills.CoachingMental * 0.2 + 0.5) +
                     (int)(staff.CoachingSkills.CoachingTechnical * 0.2 + 0.5)) * 3 + ddm * 2;
            max = 270;
            dec = 30;
            calcCR(ref cr.SetPieces, value, max, dec);

            if (cr.SetPieces > cr.BestCRStars)
            {
                cr.BestCRStars = cr.SetPieces;
                cr.BestCR = "Set Pieces";
            }
            else if (cr.SetPieces == cr.BestCRStars) cr.BestCR += "/Set Pieces";

            staffCRID.Add(staff.ID, cr);
        }

        private void loadShortlist()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments + "\\Sports Interactive\\Football Manager 2009\\shortlists";
            openFileDialog.Multiselect = true;
            openFileDialog.DefaultExt = "slf";
            // The Filter property requires a search string after the pipe ( | )
            openFileDialog.Filter = "FM2009 Shortlists (*.slf)|*.slf|CSV Spreadsheet(*.csv)|*.csv";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileNames.Length > 0)
            {
                string ext = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf(".") + 1);
                this.ShortlistResultLabel.Text += "Searching...";
                playersPRTotal = 0;
                DateTime timerStart = DateTime.Now;
                this.ShortlistDisplayGridView.ScrollBars = ScrollBars.None;
                if (ext.Equals("slf"))
                {
                    //int count = 0;
                    foreach (string filename in openFileDialog.FileNames)
                    {
                      //  ++count;
                        using (FileStream stream = new FileStream(filename, FileMode.Open))
                        {
                            byte[] header = new byte[14];
                            stream.Read(header, 0, header.Length);
                            byte[] shortlistNameLengthByte = new byte[4];
                            stream.Read(shortlistNameLengthByte, 0, shortlistNameLengthByte.Length);
                            int shortlistNameLength = ReadInt32(shortlistNameLengthByte);
                            byte[][] shortlistNameChar = new byte[shortlistNameLength][];
                            for (int i = 0; i < shortlistNameLength; ++i)
                            {
                                shortlistNameChar[i] = new byte[2];
                                stream.Read(shortlistNameChar[i], 0, shortlistNameChar[i].Length);
                            }

                            byte[] bogusByte = new byte[2];
                            stream.Read(bogusByte, 0, bogusByte.Length);
                            int bogus = ReadInt16(bogusByte);

                            byte[] noItemsByte = new byte[4];
                            stream.Read(noItemsByte, 0, noItemsByte.Length);
                            int noItems = ReadInt32(noItemsByte);

                            byte[][] playersIDByte = new byte[noItems][];
                            int[] playersID = new int[noItems];
                            for (int i = 0; i < noItems; ++i)
                            {
                                playersIDByte[i] = new byte[4];
                                stream.Read(playersIDByte[i], 0, playersIDByte[i].Length);
                                playersID[i] = ReadInt32(playersIDByte[i]);
                            }

                            byte[] endByte = new byte[4];
                            stream.Read(endByte, 0, 1);
                            int end = ReadInt32(endByte);

                            if (end == 0)
                            {
                              //  if (count == 1)
                              //      loadShortlistPlayers(ref playersID, true);
                              //  else
                                    loadShortlistPlayers(ref playersID, false);
                            }
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
                                    players.Add(Int32.Parse(token));
                                }

                                int[] playersID = players.ToArray();
                                //  if (count == 1)
                                //      loadShortlistPlayers(ref playersID, true);
                                //  else
                                loadShortlistPlayers(ref playersID, false);
                            }
                        }
                    }
                }
                this.ShortlistDisplayGridView.DataSource = shortlistDataTable;
                this.ShortlistDisplayGridView.ScrollBars = ScrollBars.Both;

                this.ShortlistResultLabel.Text = shortlistDataTable.Rows.Count + " shortlist entries found.";
                if (playersPRTotal > 0) this.ShortlistResultLabel.Text += " Calculated PR for " + playersPRTotal + " players.";
                DateTime timerEnd = DateTime.Now;
                TimeSpan timer = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
                this.ShortlistResultLabel.Text += " Query took " + (float)timer.Seconds + " sec.";
                this.treeView.SelectedNode = this.treeView.Nodes["ShortlistNode"];
            }
        }

        private void exportShortlist(bool allrows)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments + "\\Sports Interactive\\Football Manager 2009\\shortlists";
            saveFileDialog.DefaultExt = "slf";
            // The Filter property requires a search string after the pipe ( | )
            saveFileDialog.Filter = "FM2009 Shortlists (*.slf)|*.slf|CSV Spreadsheet(*.csv)|*.csv|TXT document(*.txt)|*.txt";
            saveFileDialog.Title = "Save an FM2009 Shortlist File";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                string ext = saveFileDialog.FileName.Substring(saveFileDialog.FileName.LastIndexOf(".") + 1);
                int count = 0;
                if (allrows)
                    count = this.ShortlistDisplayGridView.Rows.Count;
                else
                    count = this.ShortlistDisplayGridView.SelectedRows.Count;
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
                                if (fm.GetCurrentFMVersion().Equals("9.3.0"))
                                    sw.Write(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
                                else
                                    sw.Write(new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 });
                                sw.Write(FromIntToHex(saveFileDialog.FileName.Length));
                                sw.Write(FromStringToHex(saveFileDialog.FileName));
                                sw.Write(new byte[] { 0x0, 0x0 });
                                sw.Write(FromIntToHex(playersID.Length));
                                for (int i = 0; i < playersID.Length; ++i)
                                {
                                    if (allrows)
                                        playerID = (int)((DataRow)((DataRowView)(this.ShortlistDisplayGridView.Rows[i].DataBoundItem)).Row)["ID"];
                                    else
                                        playerID = (int)((DataRow)((DataRowView)(this.ShortlistDisplayGridView.SelectedRows[i].DataBoundItem)).Row)["ID"];

                                    playersID[i] = playerID;
                                    sw.Write(FromIntToHex(playersID[i]));
                                }
                                sw.Write(0x0);
                                sw.Close();
                            }
                        }
                        else if (ext.Equals("csv") || ext.Equals("txt"))
                        {
                            string separator = ",";
                            using (StreamWriter sw = new StreamWriter(stream))
                            {
                                string strValue = string.Empty;
                                strValue = "\"" + this.ShortlistDisplayGridView.Columns[0].Name + "\"";
                                for (int i = 1; i < this.ShortlistDisplayGridView.Columns.Count - 1; i++)
                                {
                                    if (this.ShortlistDisplayGridView.Columns[i].Visible)
                                        strValue += separator + "\"" + this.PlayersDisplayGridView.Columns[i].Name + "\"";
                                }
                                strValue += Environment.NewLine;
                                if (allrows)
                                {
                                    for (int i = 0; i < this.ShortlistDisplayGridView.Rows.Count; i++)
                                    {
                                        for (int j = 0; j < this.ShortlistDisplayGridView.Rows[i].Cells.Count - 1; j++)
                                        {
                                            if (j == 0 || this.ShortlistDisplayGridView.Rows[i].Cells[j].Visible)
                                            {
                                                if (j != 0)
                                                {
                                                    if (!string.IsNullOrEmpty(this.ShortlistDisplayGridView[j, i].Value.ToString()))
                                                        strValue += separator + "\"" + this.ShortlistDisplayGridView[j, i].Value.ToString() + "\"";
                                                    else
                                                        strValue += separator + " ";
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(this.ShortlistDisplayGridView[j, i].Value.ToString()))
                                                        strValue += "\"" + this.ShortlistDisplayGridView[j, i].Value.ToString() + "\"";
                                                    else
                                                        strValue += " ";
                                                }
                                            }
                                        }
                                        strValue += Environment.NewLine;
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < this.ShortlistDisplayGridView.SelectedRows.Count; i++)
                                    {
                                        for (int j = 0; j < this.ShortlistDisplayGridView.SelectedRows[i].Cells.Count - 1; j++)
                                        {
                                            if (j == 0 || this.ShortlistDisplayGridView.SelectedRows[i].Cells[j].Visible)
                                            {
                                                if (j != 0)
                                                {
                                                    if (!string.IsNullOrEmpty(this.ShortlistDisplayGridView[j, i].Value.ToString()))
                                                        strValue += separator + "\"" + this.ShortlistDisplayGridView[j, i].Value.ToString() + "\"";
                                                    else
                                                        strValue += separator + " ";
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(this.ShortlistDisplayGridView[j, i].Value.ToString()))
                                                        strValue += "\"" + this.ShortlistDisplayGridView[j, i].Value.ToString() + "\"";
                                                    else
                                                        strValue += " ";
                                                }
                                            }
                                        }
                                        strValue += Environment.NewLine;
                                    }
                                }
                                sw.Write(strValue.ToCharArray());
                                sw.Close();
                            }
                        }
                        stream.Close();
                    }
                }
            }
        }

        private void addToShortlist()
        {
            DataRow newRow;
            int ID;
            for (int i = 0; i < this.PlayersDisplayGridView.SelectedRows.Count; ++i)
            {
                ID = (int)this.PlayersDisplayGridView.SelectedRows[i].Cells["ID"].Value;
                if (!this.shortlistIDList.Contains(ID))
                {
                    newRow = ((DataRowView)(this.PlayersDisplayGridView.SelectedRows[i].DataBoundItem)).Row;
                    shortlistDataTable.ImportRow(((DataRowView)(this.PlayersDisplayGridView.SelectedRows[i].DataBoundItem)).Row);
                    this.shortlistIDList.Add(ID, this.shortlistIDList.Count);
                }
            }
            this.ShortlistDisplayGridView.DataSource = this.shortlistDataTable;
            this.PlayersDisplayGridView.Refresh();

            this.ShortlistResultLabel.Text = this.ShortlistDisplayGridView.Rows.Count + " shortlist entries found.";
        }

        private void removeFromGrid()
        {
            if (this.PlayersTabControl.Visible)
            {
                removeRow(ref this.PlayersDisplayGridView);
                this.PlayersResultLabel.Text = this.PlayersDisplayGridView.RowCount + " player entries found.";
                if (this.PlayersDisplayGridView.RowCount == 0) this.PlayersResultLabel.Text = "";

            }
            else if (this.StaffTabControl.Visible)
            {
                removeRow(ref this.StaffDisplayGridView);
                this.StaffResultLabel.Text = StaffDisplayGridView.RowCount + " staff entries found.";
                if (this.StaffDisplayGridView.RowCount == 0) this.StaffResultLabel.Text = "";
            }
            else if (this.TeamsTabControl.Visible)
            {
                removeRow(ref this.TeamsDisplayGridView);
                this.TeamsResultLabel.Text = TeamsDisplayGridView.RowCount + " shortlist entries found.";
                if (this.TeamsDisplayGridView.RowCount == 0) this.TeamsResultLabel.Text = "";
            }
            else if (this.ShortlistTabControl.Visible)
            {
                if (this.ShortlistDisplayGridView.SelectedRows.Count == 0) return;
                foreach (DataGridViewRow r in this.ShortlistDisplayGridView.SelectedRows)
                {
                    this.shortlistIDList.Remove(r.Cells["ID"].Value);
                    if (!this.PlayerProfileIDTextBox.Text.Equals("ID"))
                    {
                        if (Int32.Parse(this.PlayerProfileIDTextBox.Text) == (int)r.Cells["ID"].Value)
                            setPlayerProfile((int)r.Cells["ID"].Value, false);
                    }
                    this.ShortlistDisplayGridView.Rows.Remove(r);
                }
                this.ShortlistResultLabel.Text = shortlistDataTable.Rows.Count + " shortlist entries found.";
                if (this.ShortlistDisplayGridView.RowCount == 0) this.ShortlistResultLabel.Text = "";
            }
        }

        private void removeRow(ref DataGridView datagridview)
        {
            if (datagridview.SelectedRows.Count == 0) return;
            
            foreach (DataGridViewRow r in datagridview.SelectedRows)
                datagridview.Rows.Remove(r);
        }

        private void PlayerProfileShortlistButton_Click(object sender, EventArgs e)
        {
            if (scoutLoaded && fm.FMLoaded())
                shortlistPlayer();                
        }

        private void shortlistPlayer()
        {
            if (this.PlayerProfileShortlistTextBox.Text.Equals("Not Shortlisted"))
            {
                this.PlayerProfileShortlistTextBox.Text = "Shortlisted";
                this.PlayerProfileShortlistButton.BackgroundImage = global::FMScout.Properties.Resources.Minus;
                int[] playerID = new int[] { Int32.Parse(this.PlayerProfileIDTextBox.Text) };
                loadShortlistPlayers(ref playerID, false);
            }
            else if (this.PlayerProfileShortlistTextBox.Text.Equals("Shortlisted"))
            {
                this.PlayerProfileShortlistTextBox.Text = "Not Shortlisted";
                this.PlayerProfileShortlistButton.BackgroundImage = global::FMScout.Properties.Resources.Add;
                foreach (DataGridViewRow r in this.ShortlistDisplayGridView.Rows)
                {
                    if (Int32.Parse(this.PlayerProfileIDTextBox.Text) == (int)r.Cells["ID"].Value)
                    {
                        this.shortlistIDList.Remove(r.Cells["ID"].Value);
                        this.ShortlistDisplayGridView.Rows.Remove(r);
                        break;
                    }
                }
            }
            this.ShortlistResultLabel.Text = this.ShortlistDisplayGridView.Rows.Count + " shortlist entries found.";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RemoveRowToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                removeFromGrid();
        }

        private void loadShortlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
            {
                loadShortlist();
                this.treeView.SelectedNode = this.treeView.Nodes["ShortlistNode"];
            }
        }

        private void addToShortlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                addToShortlist();
        }

        private void profileViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                currentProfileView(sender, e);
        }

        private void viewShortlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                viewShortlist(sender, e);
        }

        private void ViewShortlistToolStrip_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                viewShortlist(sender, e);
        }

        private void viewShortlist(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                this.treeView.SelectedNode = this.treeView.Nodes["ShortlistNode"];
        }

        private void CurrentViewProfileToolStrip_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                currentProfileView(sender, e);
        }

        private void clearPlayerFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                clearPlayerFields();
        }

        private void clearStaffFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                clearStaffFields();
        }

        private void clearTeamFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                clearTeamsFields();
        }

        private void clearShortlistTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                clearShortlistTable();
        }

        private void clearAllFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                clearAllFields();
        }

        private void searchNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                searchNow(sender, e);
        }

        private void CurrentSearchToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                searchNow(sender, e);
        }

        private void AddToShotlistToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                addToShortlist();
        }

        private void ViewProfileContextMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                currentProfileView(sender, e);
        }

        private void RemoveRowToolStripContextMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                removeFromGrid();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.scoutLoaded)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.treeView.SelectedNode == this.treeView.Nodes["PlayersNode"].Nodes["SearchPlayersNode"]
                        || this.treeView.SelectedNode == this.treeView.Nodes["PlayersNode"])
                        PlayersSearchButton_Click(sender, e);
                    else if (this.treeView.SelectedNode == this.treeView.Nodes["StaffNode"].Nodes["SearchStaffNode"]
                        || this.treeView.SelectedNode == this.treeView.Nodes["StaffNode"])
                        StaffSearchButton_Click(sender, e);
                    else if (this.treeView.SelectedNode == this.treeView.Nodes["TeamsNode"].Nodes["SearchTeamsNode"]
                        || this.treeView.SelectedNode == this.treeView.Nodes["TeamsNode"])
                        TeamsSearchButton_Click(sender, e);
                }
                else if (e.KeyCode == Keys.Escape)
                {

                }
            }
        }

        private void PreferencesToolStrip_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                loadPreferences();
        }

        private void ClearAllToolStrip_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                clearAllFields();
        }

        private void BugReportToolStrip_Click(object sender, EventArgs e)
        {
            this.aboutForm.bugReport();
        }

        private void EmailToolStripButton_Click(object sender, EventArgs e)
        {
            this.aboutForm.email();
        }

        private void HomepageToolStrip_Click(object sender, EventArgs e)
        {
            this.aboutForm.homepage();
        }

        private void ImportShortlistToolStrip_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
            {
                loadShortlist();
                this.treeView.SelectedNode = this.treeView.Nodes["ShortlistNode"];
            }
        }

        private void ExportShortlistAllToolStrip_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                exportShortlist(true);
        }

        private void ExportShortlistSelectedToolStrip_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                exportShortlist(false);
        }

        private void AddToShortlistToolStrip_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
                addToShortlist();
        }

        private void playersViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
            {
                this.treeView.SelectedNode = this.treeView.Nodes["PlayersNode"].Nodes["SearchPlayersNode"];
                this.PlayersTabControl.SelectedTab = this.PlayersTabPage;
            }
        }

        private void staffViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
            {
                this.treeView.SelectedNode = this.treeView.Nodes["StaffNode"].Nodes["SearchStaffNode"];
                this.StaffTabControl.SelectedTab = this.StaffTabPage;
            }
        }

        private void teamsViewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.scoutLoaded)
            {
                this.treeView.SelectedNode = this.treeView.Nodes["TeamsNode"].Nodes["SearchTeamsNode"];
                this.TeamsTabControl.SelectedTab = this.TeamsTabPage;
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (scoutLoaded)
                loadPreferences();
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            donationForm.Location = new Point(this.Left + (this.Width / 2 - donationForm.Width / 2), this.Top + (this.Height / 2 - donationForm.Height / 2));
            donationForm.Show(this);
        }

        private void loadPreferences()
        {
            if (this.scoutLoaded)
            {
                preferencesForm.Location = new Point(this.Left + (this.Width / 2 - preferencesForm.Width / 2), this.Top + (this.Height / 2 - preferencesForm.Height / 2));
                preferencesForm.MainTabControl.SelectedTab = preferencesForm.GeneralSettingsTabPage;
                preferencesForm.Show(this);
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            aboutForm.Location = new Point(this.Left + (this.Width / 2 - aboutForm.Width / 2), this.Top + (this.Height / 2 - aboutForm.Height / 2));
            aboutForm.Show(this);
        }

        private void MiniScoutButton_Click(object sender, EventArgs e)
        {
            miniScoutForm.Location = new Point(this.Right - this.miniScoutForm.Width - 10, this.Top + 10);
            miniScoutForm.ShowMiniScout();
        }

        public Player ActivePlayerObject()
        {
            return fm.ActivePlayerObject;
        }

        public Staff ActiveStaffObject()
        {
            return fm.ActiveStaffObject;
        }

        public Team ActiveTeamObject()
        {
            return fm.ActiveTeamObject;
        }

        public void setLabels()
        {
            if (fm.FMLoaded()) this.SettingsLabel.Text = infoText(true);
        }

        public string infoText(bool loaded)
        {
            String text = "";
            if (loaded)
            {
                text = "Current Settings:";
                text += "\r\nGame Status: Loaded";
                text += "\r\nGame Version: " + fm.GetCurrentFMVersion();
                text += "\r\nPlayers Found: " + fm.Players.Count;
                text += "\r\nStaff Found    : " + fm.NonPlayingStaff.Count;
                text += "\r\nTeams Found : " + allClubs.Count;
                text += "\r\nEditor Preferences:";
                text += "\r\nEditing    : " + this.preferencesForm.AllowEditing.ToString();
                text += "\r\nScout Preferences:";
                text += "\r\nCurrency : " + this.preferencesForm.CurrencyName;
                text += "\r\nWages   : " + this.preferencesForm.WagesName;
                text += "\r\nHeight    : " + this.preferencesForm.HeightName;
                text += "\r\nWeight   : " + this.preferencesForm.WeightName;
            }
            else
            {
                text = "Current Settings:";
                text += "\r\nGame Status: Not loaded";
                text += "\r\nEditing: No";
                text += "\r\nCurrency: None";
                text += "\r\nWages   : None";
                text += "\r\nHeight    : None";
                text += "\r\nWeight   : None";
            }
            return text;
        }

        private void PlayerProfileSelectSkillsButton_Click(object sender, EventArgs e)
        {
            if (this.PlayerProfileSelectSkillsButton.Text.Equals("View Goalkeeping Attributes"))
                playerProfileViewGoalkeepingSkills();
            else if (this.PlayerProfileSelectSkillsButton.Text.Equals("View Technical Attributes"))
                playerProfileViewTechnicalSkills();
        }

        private void playerProfileViewTechnicalSkills()
        {
            this.PlayerProfileSelectSkillsButton.Text = "View Goalkeeping Attributes";
            this.PlayerProfileTechnicalSkillsGroupBox.Visible = true;
            this.PlayerProfileGoalkeepingSkillsGroupBox.Visible = false;
        }

        private void playerProfileViewGoalkeepingSkills()
        {
            this.PlayerProfileSelectSkillsButton.Text = "View Technical Attributes";
            this.PlayerProfileTechnicalSkillsGroupBox.Visible = false;
            this.PlayerProfileGoalkeepingSkillsGroupBox.Visible = true;
        }

        private void LoadFMToolStrip_Click(object sender, EventArgs e)
        {
            loadFM();
        }

        private void PreviousNodeToolStrip_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                if (treeView.SelectedNode.PrevNode != null)
                {
                    if (treeView.SelectedNode.PrevNode.LastNode == null)
                        treeView.SelectedNode = treeView.SelectedNode.PrevNode;
                    else
                        treeView.SelectedNode = treeView.SelectedNode.PrevNode.LastNode;
                }
                else if (treeView.SelectedNode.FirstNode == null)
                {
                    if (treeView.SelectedNode.Parent != null)
                    {
                        if (treeView.SelectedNode.Parent.PrevNode != null)
                            treeView.SelectedNode = treeView.SelectedNode.Parent;
                    }
                }
            }
        }

        private void NextNodeToolStrip_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                if (treeView.SelectedNode.FirstNode != null)
                {
                    treeView.SelectedNode = treeView.SelectedNode.FirstNode;
                }
                else if (treeView.SelectedNode.NextNode != null)
                {
                    treeView.SelectedNode = treeView.SelectedNode.NextNode;
                }
                else if (treeView.SelectedNode.LastNode == null)
                {
                    if (treeView.SelectedNode.Parent != null)
                    {
                        if (treeView.SelectedNode.Parent.NextNode != null)
                            treeView.SelectedNode = treeView.SelectedNode.Parent.NextNode;
                    }
                }
            }
        }

        private void PlayersTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] names = { "Players", "Players Attributes", "Player Profile" };
            string[] nodes = { "SearchPlayersNode", "PlayersAttributesNode", "PlayerProfileNode" };

            for (int i = 0; i < nodes.Length; ++i)
            {
                if (this.PlayersTabControl.SelectedTab.Text.Equals(names[i]))
                    this.treeView.SelectedNode = this.treeView.Nodes["PlayersNode"].Nodes[nodes[i]];
            }
        }

        private void StaffTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] names = { "Staff", "Staff Attributes", "Staff Profile" };
            string[] nodes = { "SearchStaffNode", "StaffAttributesNode", "StaffProfileNode" };

            for (int i = 0; i < nodes.Length; ++i)
            {
                if (this.StaffTabControl.SelectedTab.Text.Equals(names[i]))
                    this.treeView.SelectedNode = this.treeView.Nodes["StaffNode"].Nodes[nodes[i]];
            }
        }

        private void TeamsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] names = { "Teams", "Team Profile" };
            string[] nodes = { "SearchTeamsNode", "TeamProfileNode" };

            for (int i = 0; i < nodes.Length; ++i)
            {
                if (this.TeamsTabControl.SelectedTab.Text.Equals(names[i]))
                    this.treeView.SelectedNode = this.treeView.Nodes["TeamsNode"].Nodes[nodes[i]];
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView treeview = (TreeView)(sender);
            TreeNode node = treeview.SelectedNode;
            String nodeName = node.Text;
            String nodeToCheck = "";
            if (node.Parent == null)
                nodeToCheck = nodeName;
            else
                nodeToCheck = node.Parent.Text;

            this.EditingToolStripSeparator.Enabled = false;
            this.ProfileSaveEditingToolStrip.Enabled = false;
            this.ProfileCancelEditingToolStrip.Enabled = false;

            this.viewShortlistToolStripMenuItem.Enabled = false;
            this.ViewShortlistToolStrip.Enabled = false;
            this.CurrentFieldsClearToolStripButton.Enabled = false;
            this.CurrentSearchToolStripButton.Enabled = false;

            resetButtons();

            if (nodeName.Equals("Players") || nodeName.Equals("Search Players"))
            {
                enablePlayerButtons();
                this.PlayersTabControl.SelectedTab = this.PlayersTabPage;
            }
            else if (nodeName.Equals("Players Attributes")) this.PlayersTabControl.SelectedTab = this.PlayersSearchAttributes;
            else if (nodeName.Equals("Player Profile"))
            {
                if (this.preferencesForm.AllowEditing && !this.PlayerProfileIDTextBox.Text.Equals("ID"))
                {
                    this.EditingToolStripSeparator.Enabled = true;
                    this.ProfileSaveEditingToolStrip.Enabled = true;
                    this.ProfileCancelEditingToolStrip.Enabled = true;
                }
                this.PlayersTabControl.SelectedTab = this.PlayerProfile;
            }
            else if (nodeName.Equals("Staff") || nodeName.Equals("Search Staff"))
            {
                enableStaffButtons();
                this.StaffTabControl.SelectedTab = this.StaffTabPage;
            }
            else if (nodeName.Equals("Staff Attributes")) this.StaffTabControl.SelectedTab = this.StaffSearchAttributes;
            else if (nodeName.Equals("Staff Profile"))
            {
                if (this.preferencesForm.AllowEditing && !this.StaffProfileFullNameTextBox.Text.Equals("Full Name"))
                {
                    this.EditingToolStripSeparator.Enabled = true;
                    this.ProfileSaveEditingToolStrip.Enabled = true;
                    this.ProfileCancelEditingToolStrip.Enabled = true;
                }
                this.StaffTabControl.SelectedTab = this.StaffProfile;
            }
            else if (nodeName.Equals("Teams") || nodeName.Equals("Search Teams"))
            {
                enableTeamButtons();
                this.TeamsTabControl.SelectedTab = this.TeamsTabPage;
            }
            else if (nodeName.Equals("Team Profile"))
            {
                if (this.preferencesForm.AllowEditing && !this.TeamProfileIDTextBox.Text.Equals("ID"))
                {
                    this.EditingToolStripSeparator.Enabled = true;
                    this.ProfileSaveEditingToolStrip.Enabled = true;
                    this.ProfileCancelEditingToolStrip.Enabled = true;
                }
                this.TeamsTabControl.SelectedTab = this.TeamProfile;
            }
            else if (nodeName.Equals("Shortlist")) enableShortlistButtons();

            if (!nodeToCheck.Equals("Shortlist"))
            {
                this.viewShortlistToolStripMenuItem.Enabled = true;
                this.ViewShortlistToolStrip.Enabled = true;
                this.CurrentFieldsClearToolStripButton.Enabled = true;
                this.CurrentSearchToolStripButton.Enabled = true;

                if (nodeToCheck.Equals("Players"))
                {
                    if (!this.PlayersTabControl.Visible)
                    {
                        this.PlayersTabControl.Visible = true;
                        this.StaffTabControl.Visible = false;
                        this.TeamsTabControl.Visible = false;
                        this.ShortlistTabControl.Visible = false;
                    }
                }
                else if (nodeToCheck.Equals("Staff"))
                {
                    if (!this.StaffTabControl.Visible)
                    {
                        this.PlayersTabControl.Visible = false;
                        this.StaffTabControl.Visible = true;
                        this.TeamsTabControl.Visible = false;
                        this.ShortlistTabControl.Visible = false;
                    }
                }
                else if (nodeToCheck.Equals("Teams"))
                {
                    if (!this.TeamsTabControl.Visible)
                    {
                        this.PlayersTabControl.Visible = false;
                        this.StaffTabControl.Visible = false;
                        this.TeamsTabControl.Visible = true;
                        this.ShortlistTabControl.Visible = false;
                    }
                }
            }
            else if (nodeToCheck.Equals("Shortlist"))
            {
                if (!this.ShortlistTabControl.Visible)
                {
                    this.PlayersTabControl.Visible = false;
                    this.StaffTabControl.Visible = false;
                    this.TeamsTabControl.Visible = false;
                    this.ShortlistTabControl.Visible = true;
                }
            }

            this.treeView.Focus();
        }

        private void resetButtons()
        {
            this.RemoveRowToolStripButton.Enabled = false;
            this.CurrentTableClearToolStripButton.Enabled = false;
            this.profileViewToolStripMenuItem.Enabled = false;
            this.CurrentViewProfileToolStrip.Enabled = false;
            this.addToShortlistToolStripMenuItem.Enabled = false;
            this.AddToShortlistToolStrip.Enabled = false;
            this.exportShortlistToolStripMenuItem.Enabled = false;
            this.exportSelectedToolStripMenuItem.Enabled = false;
            this.ExportShortlistAllToolStrip.Enabled = false;
            this.ExportShortlistSelectedToolStrip.Enabled = false;

            this.AddToShotlistToolStripContextMenuItem.Visible = false;
            this.ViewProfileContextMenuItem.Visible = false;
            this.RemoveRowToolStripContextMenuItem.Visible = false;
        }

        private void enablePlayerButtons()
        {
            if (this.PlayersDisplayGridView.RowCount > 0)
            {
                this.RemoveRowToolStripButton.Enabled = true;
                this.CurrentTableClearToolStripButton.Enabled = true;
                this.profileViewToolStripMenuItem.Enabled = true;
                this.CurrentViewProfileToolStrip.Enabled = true;
                this.addToShortlistToolStripMenuItem.Enabled = true;
                this.AddToShortlistToolStrip.Enabled = true;

                this.PlayerProfileMovePanel.Enabled = true;

                if (this.PlayersDisplayGridView.SelectedRows.Count > 0)
                {
                    this.AddToShotlistToolStripContextMenuItem.Visible = true;
                    this.RemoveRowToolStripContextMenuItem.Visible = true;
                    if (this.PlayersDisplayGridView.SelectedRows.Count == 1)
                        this.ViewProfileContextMenuItem.Visible = true;
                }
            }
        }

        private void enableStaffButtons()
        {
            if (this.StaffDisplayGridView.RowCount > 0)
            {
                this.RemoveRowToolStripButton.Enabled = true;
                this.CurrentTableClearToolStripButton.Enabled = true;
                this.profileViewToolStripMenuItem.Enabled = true;
                this.CurrentViewProfileToolStrip.Enabled = true;

                if (this.StaffDisplayGridView.SelectedRows.Count > 0)
                {
                    this.RemoveRowToolStripContextMenuItem.Visible = true;
                    if (this.StaffDisplayGridView.SelectedRows.Count == 1)
                        this.ViewProfileContextMenuItem.Visible = true;
                }
            }
        }

        private void enableTeamButtons()
        {
            if (this.TeamsDisplayGridView.RowCount > 0)
            {
                this.RemoveRowToolStripButton.Enabled = true;
                this.CurrentTableClearToolStripButton.Enabled = true;
                this.profileViewToolStripMenuItem.Enabled = true;
                this.CurrentViewProfileToolStrip.Enabled = true;

                if (this.TeamsDisplayGridView.SelectedRows.Count > 0)
                {
                    this.RemoveRowToolStripContextMenuItem.Visible = true;
                    if (this.TeamsDisplayGridView.SelectedRows.Count == 1)
                        this.ViewProfileContextMenuItem.Visible = true;
                }
            }
        }

        private void enableShortlistButtons()
        {
            if (this.ShortlistDisplayGridView.RowCount > 0)
            {
                this.RemoveRowToolStripButton.Enabled = true;
                this.CurrentTableClearToolStripButton.Enabled = true;
                this.profileViewToolStripMenuItem.Enabled = true;
                this.CurrentViewProfileToolStrip.Enabled = true;
                this.exportShortlistToolStripMenuItem.Enabled = true;
                this.exportSelectedToolStripMenuItem.Enabled = true;
                this.ExportShortlistAllToolStrip.Enabled = true;
                this.ExportShortlistSelectedToolStrip.Enabled = true;

                if (this.ShortlistDisplayGridView.SelectedRows.Count > 0)
                {
                    this.RemoveRowToolStripContextMenuItem.Visible = true;
                    if (this.ShortlistDisplayGridView.SelectedRows.Count == 1)
                        this.ViewProfileContextMenuItem.Visible = true;
                }
            }
        }

        private void searchNow(object sender, EventArgs e)
        {
            if (this.PlayersTabControl.Visible)
            {
                this.treeView.SelectedNode = this.treeView.Nodes["PlayersNode"].Nodes["SearchPlayersNode"];
                this.PlayersTabControl.SelectedTab = this.PlayersTabPage;
                Application.DoEvents();
                PlayersSearchButton_Click(sender, e);
            }
            else if (this.StaffTabControl.Visible)
            {
                this.treeView.SelectedNode = this.treeView.Nodes["StaffNode"].Nodes["SearchStaffNode"];
                this.StaffTabControl.SelectedTab = this.StaffTabPage;
                Application.DoEvents();
                StaffSearchButton_Click(sender, e);
            }
            else if (this.TeamsTabControl.Visible)
            {
                this.treeView.SelectedNode = this.treeView.Nodes["TeamsNode"].Nodes["SearchTeamsNode"];
                this.TeamsTabControl.SelectedTab = this.TeamsTabPage;
                Application.DoEvents();
                TeamsSearchButton_Click(sender, e);
            }
        }

        private void currentProfileView(object sender, EventArgs e)
        {
            if (this.PlayersTabControl.Visible)
            {
                if (this.PlayersDisplayGridView.SelectedRows.Count == 0) return;
                DataRow r = ((DataRowView)(this.PlayersDisplayGridView.SelectedRows[0].DataBoundItem)).Row;
                viewPlayerProfile(ref r);
            }
            else if (this.StaffTabControl.Visible)
            {
                if (this.StaffDisplayGridView.SelectedRows.Count == 0) return;
                DataRow r = ((DataRowView)(this.StaffDisplayGridView.SelectedRows[0].DataBoundItem)).Row;
                viewStaffProfile(ref r);
            }
            else if (this.TeamsTabControl.Visible)
            {
                if (this.TeamsDisplayGridView.SelectedRows.Count == 0) return;
                DataRow r = ((DataRowView)(this.TeamsDisplayGridView.SelectedRows[0].DataBoundItem)).Row;
                viewTeamProfile(ref r);
            }
            else if (this.ShortlistTabControl.Visible)
            {
                if (this.ShortlistDisplayGridView.SelectedRows.Count == 0) return;
                DataRow r = ((DataRowView)(this.PlayersDisplayGridView.SelectedRows[0].DataBoundItem)).Row;
                viewShortlistProfile(ref r);
            }
        }

        private void viewPlayerProfile(ref DataRow r)
        {
            int playerID = (int)r["ID"];
            this.treeView.SelectedNode = this.treeView.Nodes["PlayersNode"].Nodes["PlayerProfileNode"];
            setPlayerProfile(playerID, true);
            
            currentPlayerIndex = this.PlayersDisplayGridView.SelectedRows[0].Index;
            currentShortlistIndex = -1;
            checkPlayerMoveBoundaries();
        }

        private void viewStaffProfile(ref DataRow r)
        {
            int staffID = (int)r["ID"];
            this.treeView.SelectedNode = this.treeView.Nodes["StaffNode"].Nodes["StaffProfileNode"];
            setStaffProfile(staffID, true);
            
            currentStaffIndex = this.StaffDisplayGridView.SelectedRows[0].Index;
            checkStaffMoveBoundaries();
        }

        private void viewTeamProfile(ref DataRow r)
        {
            int teamID = (int)r["ID"];
            this.treeView.SelectedNode = this.treeView.Nodes["TeamsNode"].Nodes["TeamProfileNode"];
            setTeamProfile(teamID, true);

            currentTeamIndex = this.TeamsDisplayGridView.SelectedRows[0].Index;
            checkTeamMoveBoundaries();
        }

        private void viewShortlistProfile(ref DataRow r)
        {
            int playerID = (int)r["ID"];
            this.treeView.SelectedNode = this.treeView.Nodes["PlayersNode"].Nodes["PlayerProfileNode"];
            setPlayerProfile(playerID, true);

            currentShortlistIndex = this.ShortlistDisplayGridView.SelectedRows[0].Index;
            currentPlayerIndex = -1;
            checkShortlistMoveBoundaries();
        }

        private void checkPlayerMoveBoundaries()
        {
            this.PlayerProfileMovePanel.Enabled = true;
            this.PlayerProfileMoveRightButton.Enabled = true;
            this.PlayerProfileMoveLeftButton.Enabled = true;

            if (currentPlayerIndex >= this.PlayersDisplayGridView.RowCount - 1)
                this.PlayerProfileMoveRightButton.Enabled = false;
            if (currentPlayerIndex <= 0)
                this.PlayerProfileMoveLeftButton.Enabled = false;
        }

        private void checkStaffMoveBoundaries()
        {
            this.StaffProfileMovePanel.Enabled = true;
            this.StaffProfileMoveRightButton.Enabled = true;
            this.StaffProfileMoveLeftButton.Enabled = true;

            if (currentStaffIndex >= this.StaffDisplayGridView.RowCount - 1)
                this.StaffProfileMoveRightButton.Enabled = false;
            if (currentStaffIndex <= 0)
                this.StaffProfileMoveLeftButton.Enabled = false;
        }

        private void checkTeamMoveBoundaries()
        {
            this.TeamProfileMovePanel.Enabled = true;
            this.TeamProfileMoveRightButton.Enabled = true;
            this.TeamProfileMoveLeftButton.Enabled = true;

            if (currentTeamIndex >= this.TeamsDisplayGridView.RowCount - 1)
                this.TeamProfileMoveRightButton.Enabled = false;
            if (currentTeamIndex <= 0)
                this.TeamProfileMoveLeftButton.Enabled = false;
        }

        private void checkShortlistMoveBoundaries()
        {
            this.PlayerProfileMovePanel.Enabled = true;
            this.PlayerProfileMoveRightButton.Enabled = true;
            this.PlayerProfileMoveLeftButton.Enabled = true;

            if (currentShortlistIndex >= this.ShortlistDisplayGridView.RowCount - 1)
                this.PlayerProfileMoveRightButton.Enabled = false;
            if (currentShortlistIndex <= 0)
                this.PlayerProfileMoveLeftButton.Enabled = false;
        }

        private void CurrentFieldsClearToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.PlayersTabControl.Visible) clearPlayerFields();
            else if (this.StaffTabControl.Visible) clearStaffFields();
            else if (this.TeamsTabControl.Visible) clearTeamsFields();
        }

        private void CurrentTableClearToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.PlayersTabControl.Visible)
            {
                this.treeView.SelectedNode = this.treeView.Nodes["PlayersNode"].Nodes["SearchPlayersNode"];
                this.playersDataTable.Rows.Clear();
                this.PlayersDisplayGridView.DataSource = this.playersDataTable;
                this.PlayersResultLabel.Text = "";
                this.currentPlayerIndex = -1;
            }
            else if (this.StaffTabControl.Visible)
            {
                this.treeView.SelectedNode = this.treeView.Nodes["StaffNode"].Nodes["SearchStaffNode"];
                this.staffDataTable.Rows.Clear();
                this.StaffDisplayGridView.DataSource = this.staffDataTable;
                this.StaffResultLabel.Text = "";
                this.currentStaffIndex = -1;
            }
            else if (this.TeamsTabControl.Visible)
            {
                this.treeView.SelectedNode = this.treeView.Nodes["TeamsNode"].Nodes["SearchTeamsNode"];
                this.teamsDataTable.Rows.Clear();
                this.TeamsDisplayGridView.DataSource = this.teamsDataTable;
                this.TeamsResultLabel.Text = "";
                this.currentTeamIndex = -1;
            }
            else if (this.ShortlistTabControl.Visible)
            {
                this.treeView.SelectedNode = this.treeView.Nodes["ShortlistNode"];
                this.shortlistDataTable.Rows.Clear();
                this.ShortlistDisplayGridView.DataSource = this.shortlistDataTable;
                this.ShortlistResultLabel.Text = "";
                this.shortlistIDList.Clear();
                this.currentShortlistIndex = -1;
            }
        }

        private void PlayerProfileMoveLeftButton_Click(object sender, EventArgs e)
        {
            if (currentPlayerIndex != -1)
            {
                --currentPlayerIndex;
                setPlayerProfile((int)this.PlayersDisplayGridView.Rows[currentPlayerIndex].Cells["ID"].Value, false);
                checkPlayerMoveBoundaries();
            }
            else if (currentShortlistIndex != -1)
            {
                --currentShortlistIndex;
                setPlayerProfile((int)this.ShortlistDisplayGridView.Rows[currentShortlistIndex].Cells["ID"].Value, false);
                checkShortlistMoveBoundaries();
            }
        }

        private void PlayerProfileMoveRightButton_Click(object sender, EventArgs e)
        {
            if (currentPlayerIndex != -1)
            {
                ++currentPlayerIndex;
                setPlayerProfile((int)this.PlayersDisplayGridView.Rows[currentPlayerIndex].Cells["ID"].Value, false);
                checkPlayerMoveBoundaries();
            }
            else if (currentShortlistIndex != -1)
            {
                ++currentShortlistIndex;
                setPlayerProfile((int)this.ShortlistDisplayGridView.Rows[currentShortlistIndex].Cells["ID"].Value, false);
                checkShortlistMoveBoundaries();
            }
        }

        private void StaffProfileMoveLeftButton_Click(object sender, EventArgs e)
        {
            --currentStaffIndex;
            setStaffProfile((int)this.StaffDisplayGridView.Rows[currentStaffIndex].Cells["ID"].Value, false);
            checkStaffMoveBoundaries();
        }

        private void StaffProfileMoveRightButton_Click(object sender, EventArgs e)
        {
            ++currentStaffIndex;
            setStaffProfile((int)this.StaffDisplayGridView.Rows[currentStaffIndex].Cells["ID"].Value, false);
            checkStaffMoveBoundaries();
        }

        private void TeamProfileMoveLeftButton_Click(object sender, EventArgs e)
        {
            --currentTeamIndex;
            setTeamProfile((int)this.TeamsDisplayGridView.Rows[currentTeamIndex].Cells["ID"].Value, false);
            checkTeamMoveBoundaries();
        }

        private void TeamProfileMoveRightButton_Click(object sender, EventArgs e)
        {
            ++currentTeamIndex;
            setTeamProfile((int)this.TeamsDisplayGridView.Rows[currentTeamIndex].Cells["ID"].Value, false);
            checkTeamMoveBoundaries();
        }

        public void setPlayersDataTableColumns(ref List<int> playersColumns)
        {
            for (int i = 0; i < this.PlayersDisplayGridView.Columns.Count - 1; ++i)
            {
                this.PlayersDisplayGridView.Columns[i].Visible = false;
                this.PlayersColumnsCheckedListBox.SetItemChecked(i, false);
            }
            for (int i = 0; i < playersColumns.Count; ++i)
            {
                this.PlayersDisplayGridView.Columns[playersColumns[i]].Visible = true;
                this.PlayersColumnsCheckedListBox.SetItemChecked(playersColumns[i], true);
            }

            this.PlayersDisplayGridView.Columns["Current Value"].DefaultCellStyle.Format = "C0";
            this.PlayersDisplayGridView.Columns["Current Value"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.PlayersDisplayGridView.Columns["Sale Value"].DefaultCellStyle.Format = "C0";
            this.PlayersDisplayGridView.Columns["Sale Value"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.PlayersDisplayGridView.Columns["Current Wage"].DefaultCellStyle.Format = "C0";
            this.PlayersDisplayGridView.Columns["Current Wage"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.PlayersDisplayGridView.Columns["Best PR%"].DefaultCellStyle.Format = "P0";
            this.PlayersDisplayGridView.Columns["Best PR%"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
        }

        public void setStaffDataTableColumns(ref List<int> staffColumns)
        {
            for (int i = 0; i < this.StaffDisplayGridView.Columns.Count - 1; ++i)
            {
                this.StaffDisplayGridView.Columns[i].Visible = false;
                this.StaffColumnsCheckedListBox.SetItemChecked(i, false);
            }
            for (int i = 0; i < staffColumns.Count; ++i)
            {
                this.StaffDisplayGridView.Columns[staffColumns[i]].Visible = true;
                this.StaffColumnsCheckedListBox.SetItemChecked(staffColumns[i], true);
            }

            this.StaffDisplayGridView.Columns["Current Wage"].DefaultCellStyle.Format = "C0";
            this.StaffDisplayGridView.Columns["Current Wage"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
        }

        public void setTeamsDataTableColumns(ref List<int> teamsColumns)
        {
            for (int i = 0; i < this.TeamsDisplayGridView.Columns.Count - 1; ++i)
            {
                this.TeamsDisplayGridView.Columns[i].Visible = false;
                this.TeamsColumnsCheckedListBox.SetItemChecked(i, false);
            }
            for (int i = 0; i < teamsColumns.Count; ++i)
            {
                this.TeamsDisplayGridView.Columns[teamsColumns[i]].Visible = true;
                this.TeamsColumnsCheckedListBox.SetItemChecked(teamsColumns[i], true);
            }

            this.TeamsDisplayGridView.Columns["Transfer Budget"].DefaultCellStyle.Format = "C0";
            this.TeamsDisplayGridView.Columns["Transfer Budget"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.TeamsDisplayGridView.Columns["Remaining Budget"].DefaultCellStyle.Format = "C0";
            this.TeamsDisplayGridView.Columns["Remaining Budget"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.TeamsDisplayGridView.Columns["Wage Budget"].DefaultCellStyle.Format = "C0";
            this.TeamsDisplayGridView.Columns["Wage Budget"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.TeamsDisplayGridView.Columns["Wage Used"].DefaultCellStyle.Format = "C0";
            this.TeamsDisplayGridView.Columns["Wage Used"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.TeamsDisplayGridView.Columns["Budget Balance"].DefaultCellStyle.Format = "C0";
            this.TeamsDisplayGridView.Columns["Budget Balance"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.TeamsDisplayGridView.Columns["Transfer Revenue Available"].DefaultCellStyle.Format = "P0";
            this.TeamsDisplayGridView.Columns["Transfer Revenue Available"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
        }

        public void setShortlistDataTableColumns(ref List<int> shortlistColumns)
        {
            for (int i = 0; i < this.ShortlistDisplayGridView.Columns.Count - 1; ++i)
            {
                this.ShortlistDisplayGridView.Columns[i].Visible = false;
                this.ShortlistColumnsCheckedListBox.SetItemChecked(i, false);
            }
            for (int i = 0; i < shortlistColumns.Count; ++i)
            {
                this.ShortlistDisplayGridView.Columns[shortlistColumns[i]].Visible = true;
                this.ShortlistColumnsCheckedListBox.SetItemChecked(shortlistColumns[i], true);
            }

            this.ShortlistDisplayGridView.Columns["Current Value"].DefaultCellStyle.Format = "C0";
            this.ShortlistDisplayGridView.Columns["Current Value"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.ShortlistDisplayGridView.Columns["Sale Value"].DefaultCellStyle.Format = "C0";
            this.ShortlistDisplayGridView.Columns["Sale Value"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.ShortlistDisplayGridView.Columns["Current Wage"].DefaultCellStyle.Format = "C0";
            this.ShortlistDisplayGridView.Columns["Current Wage"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
            this.ShortlistDisplayGridView.Columns["Best PR%"].DefaultCellStyle.Format = "P0";
            this.ShortlistDisplayGridView.Columns["Best PR%"].DefaultCellStyle.FormatProvider = this.preferencesForm.CurrencyFormat;
        }

        private void clearPlayerFields()
        {
            clearPlayersTab();
            clearPlayerProfileTab();
            this.currentPlayerIndex = -1;
        }

        private void clearStaffFields()
        {
            clearStaffTab();
            clearStaffProfileTab();
            this.currentStaffIndex = -1;
        }

        private void clearTeamsFields()
        {
            clearTeamsTab();
            clearTeamProfileTab();
            this.currentTeamIndex = -1;
        }

        private void clearShortlistTable()
        {
            this.shortlistDataTable.Rows.Clear();
            this.ShortlistDisplayGridView.DataSource = this.shortlistDataTable;
            this.ShortlistResultLabel.Text = "";
            this.currentShortlistIndex = -1;
        }

        private void clearAllFields()
        {
            clearPlayerFields();
            clearStaffFields();
            clearTeamsFields();
            clearShortlistTable();
        }

        private void ActiveButton_Click(object sender, EventArgs e)
        {
            if (scoutLoaded && fm.FMLoaded())
            {
                object objectActive = this.fm.ActivePlayerObject;
                if (objectActive != null)
                {
                    currentPlayerIndex = -1;
                    currentShortlistIndex = -1;
                    this.PlayerProfileMoveLeftButton.Enabled = false;
                    this.PlayerProfileMoveRightButton.Enabled = false;
                    setPlayerProfile(((Player)(objectActive)).ID, true);
                    this.treeView.SelectedNode = this.treeView.Nodes["PlayersNode"].Nodes["PlayerProfileNode"];
                }
                object staffActive = this.fm.ActiveStaffObject;
                //object humanActive = this.fm.ActiveHumanManager;
                if (staffActive != null)// || humanActive != null)
                {
                    currentStaffIndex = -1;
                    this.StaffProfileMoveLeftButton.Enabled = false;
                    this.StaffProfileMoveRightButton.Enabled = false;
                    if (staffActive != null)
                        setStaffProfile(((Staff)(staffActive)).ID, true);
                    //else if (humanActive != null)
                      //  setStaffProfile(((Staff)(humanActive)).ID, true);
                    this.treeView.SelectedNode = this.treeView.Nodes["StaffNode"].Nodes["StaffProfileNode"];
                }
                object teamActive = this.fm.ActiveTeamObject;
                if (teamActive != null)
                {
                    currentTeamIndex = -1;
                    this.TeamProfileMoveLeftButton.Enabled = false;
                    this.TeamProfileMoveRightButton.Enabled = false;
                    setTeamProfile(((Team)(teamActive)).Club.ID, true);
                    this.treeView.SelectedNode = this.treeView.Nodes["TeamsNode"].Nodes["TeamProfileNode"];
                }
            }
        }

        private void PlayersDisplayGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow r = ((DataRow)((DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem).Row);
                if (!(r["ID"].Equals(System.DBNull.Value)))
                    viewPlayerProfile(ref r);
            }
        }

        private void StaffDisplayGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow r = ((DataRow)((DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem).Row);
                if (!(r["ID"].Equals(System.DBNull.Value)))
                    viewStaffProfile(ref r);
            }
        }

        private void TeamsDisplayGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow r = ((DataRow)((DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem).Row);
                if (!(r["ID"].Equals(System.DBNull.Value)))
                    viewTeamProfile(ref r);
            }
        }

        private void ShortlistDisplayGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow r = ((DataRow)((DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem).Row);
                if (!(r["ID"].Equals(System.DBNull.Value)))
                    viewShortlistProfile(ref r);
            }
        }

        private void PlayersDisplayGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (this.treeView.SelectedNode != this.treeView.Nodes["PlayersNode"].Nodes["SearchPlayersNode"])
                this.treeView.SelectedNode = this.treeView.Nodes["PlayersNode"].Nodes["SearchPlayersNode"];
            else
            {
                resetButtons();
                enablePlayerButtons();
            }
        }

        private void StaffDisplayGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (this.treeView.SelectedNode != this.treeView.Nodes["StaffNode"].Nodes["SearchStaffNode"])
                this.treeView.SelectedNode = this.treeView.Nodes["StaffNode"].Nodes["SearchStaffNode"];
            else
            {
                resetButtons();
                enableStaffButtons();
            }
        }

        private void TeamsDisplayGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (this.treeView.SelectedNode != this.treeView.Nodes["TeamsNode"].Nodes["SearchTeamsNode"])
                this.treeView.SelectedNode = this.treeView.Nodes["TeamsNode"].Nodes["SearchTeamsNode"];
            else
            {
                resetButtons();
                enableTeamButtons();
            }
        }

        private void ShortlistDisplayGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (this.treeView.SelectedNode != this.treeView.Nodes["ShortlistNode"])
            {
                //this.treeView.SelectedNode = this.treeView.Nodes["ShortlistNode"];
            }
            else
            {
                resetButtons();
                enableShortlistButtons();
            }
        }

        private void PlayersDisplayGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow r = ((DataRow)((DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem).Row);
                if (!(r["ID"].Equals(System.DBNull.Value)))
                {

                    int ID = (int)r["ID"];
                    foreach (int i in playersFreeRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorFreePlayer;
                            return;
                        }
                    }
                    foreach (int i in playersLoanRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorLoanedPlayer;
                            return;
                        }
                    }
                    foreach (int i in playersCoOwnRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorCoContractedPlayer;
                            return;
                        }
                    }
                    foreach (int i in playersEUMemberRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorEUMemberPlayer;
                            return;
                        }
                    }
                }
            }
        }

        private void StaffDisplayGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow r = ((DataRow)((DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem).Row);
                if (!(r["ID"].Equals(System.DBNull.Value)))
                {
                    int ID = (int)r["ID"];
                    foreach (int i in staffFreeRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorFreeStaff;
                            return;
                        }
                    }
                    foreach (int i in staffNationalRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorNationalStaff;
                            return;
                        }
                    }
                }
            }
        }

        private void PlayersDisplayGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (this.PlayersDisplayGridView.Columns["S"].Index == e.ColumnIndex && e.RowIndex >= 0)
            {
                int ID = (int)this.PlayersDisplayGridView[this.PlayersDisplayGridView.Columns["ID"].Index, e.RowIndex].Value;

                Rectangle newRect = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 4, e.CellBounds.Height - 4);

                e.Graphics.FillRectangle(DisplayGridBrush, e.CellBounds);

                using (Pen gridLinePen = new Pen(GridBrush))
                {
                    // bottom
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                        e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                        e.CellBounds.Bottom - 1);
                    // right
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                        e.CellBounds.Top, e.CellBounds.Right - 1,
                        e.CellBounds.Bottom - 1);
                }
                for (int j = 0; j < this.PlayersDisplayGridView.SelectedRows.Count; ++j)
                {
                    if (this.PlayersDisplayGridView.SelectedRows[j].Index == e.RowIndex)
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.CellBounds.X + 1, e.CellBounds.Y, e.CellBounds.Width - 2, e.CellBounds.Height - 1);
                        break;
                    }
                }

                Point P = new Point((int)(e.CellBounds.X + (e.CellBounds.Width * 0.5) - (14 * 0.5)), e.CellBounds.Y + 1);

                if (shortlistIDList.Contains(ID))
                    e.Graphics.DrawImage(global::FMScout.Properties.Resources.tick, P.X, P.Y, 14, 14);

                e.Handled = true;
            }
        }

        private void StaffDisplayGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (this.StaffDisplayGridView.Columns["Best CR Stars"].Index == e.ColumnIndex && e.RowIndex >= 0)
            {
                Rectangle newRect = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 4, e.CellBounds.Height - 4);

                e.Graphics.FillRectangle(DisplayGridBrush, e.CellBounds);

                using (Pen gridLinePen = new Pen(GridBrush))
                {
                    // bottom
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                        e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                        e.CellBounds.Bottom - 1);
                    // right
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                        e.CellBounds.Top, e.CellBounds.Right - 1,
                        e.CellBounds.Bottom - 1);
                }
                for (int i = 0; i < this.StaffDisplayGridView.SelectedRows.Count; ++i)
                {
                    if (this.StaffDisplayGridView.SelectedRows[i].Index == e.RowIndex)
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1);
                        break;
                    }
                }
                int width = 18;
                int offset = (int)(e.CellBounds.Width / 8);
                Point P = new Point(e.CellBounds.X, e.CellBounds.Y + 1);
                for (int i = 0; i < 7; ++i)
                {
                    P.X += offset;
                    if (i < (int)e.Value)
                        e.Graphics.DrawImage(global::FMScout.Properties.Resources.StarHigh, P.X - (int)(width * 0.5), P.Y, width, width);
                    else
                        e.Graphics.DrawImage(global::FMScout.Properties.Resources.Star, P.X - (int)(width * 0.5), P.Y, width, width);
                }
                e.Handled = true;
            }
        }

        private void ShortlistDisplayGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow r = ((DataRow)((DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem).Row);
                if (!(r["ID"].Equals(System.DBNull.Value)))
                {
                    int ID = (int)(r["ID"]);
                    foreach (int i in shortlistFreeRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorFreePlayer;
                            return;
                        }
                    }
                    foreach (int i in shortlistLoanRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorLoanedPlayer;
                            return;
                        }
                    }
                    foreach (int i in shortlistCoOwnRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorCoContractedPlayer;
                            return;
                        }
                    }
                    foreach (int i in shortlistEUMemberRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorEUMemberPlayer;
                            return;
                        }
                    }
                }
            }
        }

        private void TeamsDisplayGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow r = ((DataRow)((DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem).Row);
                if (!(r["ID"].Equals(System.DBNull.Value)))
                {

                    int ID = (int)(r["ID"]);
                    foreach (int i in teamNationalRowList)
                    {
                        if (ID.Equals(i))
                        {
                            e.CellStyle.ForeColor = colorNationalTeam;
                            return;
                        }
                    }
                }
            }
        }

        private void PlayerProfileHealButton_Click(object sender, EventArgs e)
        {
            //this.PlayerProfileHealButton.Text = "Healing...";
            //this.PlayerProfileHealButton.Enabled = false;
            if (this.PlayerProfileIDTextBox.Text != "")
            {
                int id = Int32.Parse(this.PlayerProfileIDTextBox.Text);
                foreach (Player player in fm.Players)
                {
                    if (player.ID == id)
                    {
                        player.Heal();
                        return;
                    }
                }
            }
            //this.PlayerProfileHealButton.Enabled = true;
            //this.PlayerProfileHealButton.Text = "Heal";
        }

        private void TeamProfileHealTeamButton_Click(object sender, EventArgs e)
        {
            //this.TeamProfileHealTeamButton.Text = "Healing Team...";
            //this.TeamProfileHealTeamButton.Enabled = false;
            if (this.TeamProfileIDTextBox.Text != "")
            {
                int id = Int32.Parse(this.TeamProfileIDTextBox.Text);
                foreach (Player player in fm.Players)
                {
                    if (player.LoanContract != null)
                    {
                        if (player.LoanContract.Club.ID.Equals(id))
                            player.Heal();
                    }
                    else if (player.Contract != null)
                    {
                        if (player.Contract.Club.ID.Equals(id))
                            player.Heal();
                    }
                }
            }
            //this.TeamProfileHealTeamButton.Text = "Heal Team";
            //this.TeamProfileHealTeamButton.Enabled = true;
        }

        private void specialCharactersReplacement(ref string strToRep)
        {
            for (int i = 0; i < strToRep.Length; ++i)
            {
                for (int j = 0; j < specialCharacters.Length; ++j)
                {
                    if (strToRep[i].ToString().Equals(specialCharacters[j].ToString()))
                        strToRep = strToRep.Replace(strToRep[i].ToString(), normalCharacters[j]);
                }
            }
        }

        private bool multiEntryTextBox(ref List<string> name_substrings, string name_input)
        {
            string fullname = name_input;
            Regex RE = new Regex(@"( )");
            if (!fullname.Equals(""))
            {
                foreach (string token in RE.Split(fullname))
                {
                    string str = token.Trim();
                    if (str.Length > 0) name_substrings.Add(token.Trim().ToLower());
                }
            }
            else
                return true;
            return false;
        }

        internal bool find_player_position(Player player, ref string pos, ref List<string> positions, ref List<string> sides, bool return_after_pos)
        {
            PositionalSkills pos_skills = player.PositionalSkills;
            sbyte comp = (sbyte)(15);
            bool is_goalkeeper = false;
            bool is_sweeper = false;
            bool is_wingback = false;
            bool is_defender = false;
            bool is_defensive_midfielder = false;
            bool is_midfielder = false;
            bool is_attacking_midfielder = false;
            bool is_forward = false;
            bool is_right = false;
            bool is_left = false;
            bool is_centre = false;
            bool is_free_role = false;

            if (pos_skills.Goalkeeper > comp)
            {
                is_goalkeeper = true;
                pos += "GK";
            }
            if (pos_skills.Sweeper > comp)
            {
                is_sweeper = true;
                if (pos.Length != 0) pos += ",";
                pos += "SW";
            }
            if (pos_skills.WingBack > comp)
            {
                is_wingback = true;
                if (pos.Length != 0) pos += ",";
                pos += "W";
            }
            if (pos_skills.Defender > comp)
            {
                is_defender = true;
                if (pos.Length != 0) pos += ",";
                pos += "D";
            }
            if (pos_skills.DefensiveMidfielder > comp)
            {
                is_defensive_midfielder = true;
                if (pos.Length != 0) pos += ",";
                pos += "DM";
            }
            if (pos_skills.Midfielder > comp)
            {
                is_midfielder = true;
                if (pos.Length != 0) pos += ",";
                pos += "M";
            }
            if (pos_skills.AttackingMidfielder > comp)
            {
                if (pos.Length != 0) pos += ",";
                pos += "AM";
                is_attacking_midfielder = true;
            }
            if (pos_skills.Attacker > comp)
            {
                if (pos.Length != 0) pos += ",";
                pos += "F";
                is_forward = true;
            }

            pos += " ";

            if (pos_skills.Right > comp)
            {
                pos += "R";
                is_right = true;
            }
            if (pos_skills.Left > comp)
            {
                pos += "L";
                is_left = true;
            }
            if (pos_skills.Centre > comp)
            {
                pos += "C";
                is_centre = true;
            }   
            if (pos_skills.FreeRole > comp)
            {
                pos += "\\Free Role";
                is_free_role = true;
            }

            if (return_after_pos) return true;

            bool found = false;
            foreach (string player_pos in positions)
            {
                if (player_pos.Equals("GK") && (is_goalkeeper)) found = true;
                else if (player_pos.Equals("SW") && (is_sweeper)) found = true;
                else if (player_pos.Equals("D") && (is_defender)) found = true;
                else if (player_pos.Equals("W") && (is_wingback)) found = true;
                else if (player_pos.Equals("DM") && (is_defensive_midfielder)) found = true;
                else if (player_pos.Equals("M") && (is_midfielder)) found = true;
                else if (player_pos.Equals("AM") && (is_attacking_midfielder)) found = true;
                else if (player_pos.Equals("ST") && (is_forward)) found = true;
                if (found == false) return false;
                found = false;
            }
            if (sides.Count != 0) pos += " ";
            foreach (string player_side in sides)
            {
                if (player_side.Equals("R") && (is_right)) found = true;
                else if (player_side.Equals("L") && (is_left)) found = true;
                else if (player_side.Equals("C") && (is_centre)) found = true;
                else if (player_side.Equals("Free") && (is_free_role)) found = true;
                if (found == false) return false;
                found = false;
            }

            return true;
        }

        internal bool find_staff_role(Staff staff, ref string staff_role, bool return_after_role)
        {
            if (staff.Contract.Club.Name.Length != 0 || staff.Team.Club.Chairman.ToString().Equals(staff.ToString()))
            {
                if (staff.Team.Club.Chairman.ToString().Equals(staff.ToString()))
                    staff_role = "Chairman";
                else
                {
                    int job = staff.Contract.JobType;
                    if (job == 16)
                        staff_role = "Manager";
                    else if (job == 20)
                        staff_role = "Assistant Manager";
                    else if (job == 54)
                        staff_role = "1st Team Coach";
                    else if (job == 48)
                        staff_role = "Youth Coach";
                    else if (job == 2)
                        staff_role = "Coach";
                    else if (job == 26)
                        staff_role = "Fitness Coach";
                    else if (job == 34)
                        staff_role = "Goalkeeping Coach";
                    else if (job == 12)
                        staff_role = "Physio";
                    else if (job == 14)
                        staff_role = "Scout";
                }

                if (return_after_role) return true;

                if (this.StaffRoleComboBox.SelectedItem.Equals("1st/Youth/Coach"))
                {
                    if (staff_role.Equals("1st Team Coach") ||
                        staff_role.Equals("Youth Coach") ||
                        staff_role.Equals("Coach"))
                        return true;
                }
                else if (this.StaffRoleComboBox.SelectedItem.Equals(staff_role)) return true;
                return false;
            }
            else
            {
                bool found = false;
                for (int i = 0; i < staff.Team.Club.Directors.Count; ++i)
                {
                    if (staff.ID.Equals(staff.Team.Club.Directors[i].ID))
                    {
                        staff_role = "Director";
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    if (return_after_role) return true;

                    if (this.StaffRoleComboBox.SelectedItem.Equals(staff_role)) return true;
                    return false;
                }

                sbyte comp = (sbyte)(16);
                if (staff.RolesSkills.Manager >= comp)
                    staff_role = "Manager";
                if (staff.RolesSkills.AssistantManager >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += "Assistant Manager";
                }
                if (staff.RolesSkills.Coach >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += "Coach";
                }
                if (staff.RolesSkills.FitnessCoach >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += "Fitness Coach";
                }
                if (staff.RolesSkills.GoalkeepingCoach >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += "Goalkeeping Coach";
                }
                if (staff.RolesSkills.Physio >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += "Physio";
                }
                if (staff.RolesSkills.Scout >= comp)
                {
                    if (staff_role.Length != 0) staff_role += ",";
                    staff_role += "Scout";
                }

                if (return_after_role) return true;
                char[] sep = { ',' };
                string[] roles = staff_role.Split(sep);
                foreach (string role in roles)
                {
                    if (this.StaffRoleComboBox.SelectedItem.Equals("1st/Youth/Coach"))
                    {
                        if (staff_role.Equals("1st Team Coach") ||
                            staff_role.Equals("Youth Coach") ||
                            staff_role.Equals("Coach"))
                            return true;
                    }
                    else if (this.StaffRoleComboBox.SelectedItem.Equals(role)) return true;
                }
                return false;
            }
        }

        private void setPlayerProfile(int playerID, bool selectTab)
        {
            clearPlayerProfileTab();
            foreach (Player player in fm.Players)
            {
                if (playerID.Equals(player.ID))
                {
                    this.PlayerProfileSelectSkillsButton.Enabled = true;

                    if (this.preferencesForm.AllowEditing)
                    {
                        this.PlayerProfileHealButton.Enabled = true;
                        this.ProfileSaveEditingToolStrip.Enabled = true;
                        this.ProfileCancelEditingToolStrip.Enabled = true;
                        setPlayerEditing(true);
                    }

                    //List<PlayerRelations> playerRelations = player.Relations;

                    // personal details
                    this.PlayerProfileIDTextBox.Text = player.ID.ToString();
                    this.PlayerProfileFullNameTextBox.Text = player.ToString();
                    if (player.Nickname.Length > 0)
                        this.PlayerProfileFullNameTextBox.Text += " (" + player.Nickname + ")";

                    string playerContractExpiryDate = "No contract";
                    string playerContractStartDate = "No contract";
                    int playerContractWage = 0;
                    int playerAppearance = 0;
                    int playerGoal = 0;
                    int playerCleanSheet = 0;
                    Club playerContractClub = null;
                    if (player.Contract.Club.Name.Length == 0)
                    {
                        this.PlayerProfileClubTextBox.Text = "Free Player";
                        this.PlayerProfileTeamSquadTextBox.Text = "None";
                    }
                    else
                    {
                        this.PlayerProfileClubTextBox.Text = player.Contract.Club.Name;
                        this.PlayerProfileTeamSquadTextBox.Text = player.Team.Type.ToString();
                        playerContractExpiryDate = player.Contract.ContractExpiryDate.ToShortDateString();
                        playerContractStartDate = player.Contract.ContractStarted.ToShortDateString();
                        playerContractWage = player.Contract.WagePerWeek;
                        playerAppearance = player.Contract.AppearanceBonus;
                        playerGoal = player.Contract.GoalBonus;
                        playerCleanSheet = player.Contract.CleanSheetBonus;
                        playerContractClub = player.Contract.Club;
                        if (player.CoContract != null)
                        {
                            this.PlayerProfileClubTextBox.Text += Environment.NewLine + "/" + player.CoContract.Club.Name;
                            if (player.Team.Club.Name.Equals(player.CoContract.Club.Name))
                            {
                                playerContractExpiryDate = player.CoContract.ContractExpiryDate.ToShortDateString();
                                playerContractStartDate = player.CoContract.ContractStarted.ToShortDateString();
                                playerContractWage = player.CoContract.WagePerWeek;
                                playerAppearance = player.CoContract.AppearanceBonus;
                                playerGoal = player.CoContract.GoalBonus;
                                playerCleanSheet = player.CoContract.CleanSheetBonus;
                                playerContractClub = player.CoContract.Club;
                            }
                        }

                        if (player.LoanContract != null)
                        {
                            this.PlayerProfileClubTextBox.Text += Environment.NewLine + "(" + player.LoanContract.Club.Name + ")";
                            playerContractExpiryDate = player.LoanContract.ContractExpiryDate.ToShortDateString();
                            playerContractStartDate = player.LoanContract.ContractStarted.ToShortDateString();
                            playerContractWage = player.LoanContract.WagePerWeek;
                            playerAppearance = 0;
                            playerGoal = 0;
                            playerCleanSheet = 0;
                            playerContractClub = player.LoanContract.Club;
                        }
                    }

                    bool EUmember = false;
                    this.PlayerProfileNationalityTextBox.Text = player.Nationality.Name + " (" + player.Nationality.Continent.Name + ")";
                    if (EUcountries.Contains(player.Nationality.Name)) EUmember = true;
                    // other nationalities
                    for (int playerRelationIndex = 0; playerRelationIndex < player.RelationsTotal; ++playerRelationIndex)
                    {
                        if (player.Relations[playerRelationIndex].ObjectType == RelationObjectType.Country &&
                            player.Relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                        {
                            if (!EUmember)
                            {
                                if (EUcountries.Contains(player.Relations[playerRelationIndex].Country.Name))
                                    EUmember = true;
                            }
                            this.PlayerProfileNationalityTextBox.Text += ",\r\n" + player.Relations[playerRelationIndex].Country.Name + " (" + player.Relations[playerRelationIndex].Country.Continent.Name + ")";
                            break;
                        }
                    }

                    this.PlayerProfileHomeGrownTextBox.Text = "Not HG";
                    for (int playerRelationIndex = 0; playerRelationIndex < player.RelationsTotal; ++playerRelationIndex)
                    {
                        if (player.Relations[playerRelationIndex].RelationType == RelationType.FormedAtClub)
                        {
                            this.PlayerProfileFormedTextBox.Text += ": " + player.Relations[playerRelationIndex].Club.Name;
                            if (playerContractClub != null)
                            {
                                if (player.Relations[playerRelationIndex].Club.ID.Equals(playerContractClub.ID))
                                    this.PlayerProfileHomeGrownTextBox.Text = "Club HG";
                            }
                            break;
                        }
                    }
                    for (int playerRelationIndex = 0; playerRelationIndex < player.RelationsTotal; ++playerRelationIndex)
                    {
                        if (player.Relations[playerRelationIndex].RelationType == RelationType.FormedAtCountry)
                        {
                            this.PlayerProfileFormedTextBox.Text += " (" + player.Relations[playerRelationIndex].Country.Name + ")";
                            if (playerContractClub != null)
                            {
                                if (!PlayerProfileHomeGrownTextBox.Text.Equals("Club HG") &&
                                 player.Relations[playerRelationIndex].Country.Name.Equals(playerContractClub.Country.Name))
                                    this.PlayerProfileHomeGrownTextBox.Text = "Country HG";
                            }
                            break;
                        }
                    }
                    this.PlayerProfileEUMemberTextBox.Text = "EU Member: ";
                    if (EUmember) this.PlayerProfileEUMemberTextBox.Text += "Yes";
                    else this.PlayerProfileEUMemberTextBox.Text += "No";

                    if (player.InternationalCaps != 0)
                        this.PlayerProfileInternationalTextBox.Text = player.InternationalCaps.ToString() + " caps/" + player.InternationalGoals.ToString() + " goals";
                    else
                        this.PlayerProfileInternationalTextBox.Text = "Uncapped";
                    this.PlayerProfileBirthDateTextBox.Text = player.DateOfBirth.Date.ToShortDateString();
                    this.PlayerProfileAgeTextBox.Text = player.Age.ToString() + " years old";
                    this.PlayerProfileHeightTextBox.Text = (float.Parse(player.Length.ToString()) * this.preferencesForm.HeightMultiplier).ToString() + this.preferencesForm.HeightString;
                    this.PlayerProfileWeightTextBox.Text = (float.Parse(player.Weight.ToString()) * this.preferencesForm.WeightMultiplier).ToString() + this.preferencesForm.WeightString;
                    if (this.PlayerProfileClubTextBox.Text.Equals("Free Player"))
                    {
                        this.PlayerProfileContractStartedTextBox.Text = playerContractExpiryDate;
                        this.PlayerProfileContractExpiryTextBox.Text = playerContractStartDate;
                    }
                    else
                    {
                        this.PlayerProfileContractStartedTextBox.Text = player.Contract.ContractStarted.Date.ToShortDateString();
                        this.PlayerProfileContractExpiryTextBox.Text = player.Contract.ContractExpiryDate.Date.ToShortDateString();
                    }

                    this.PlayerProfileValueTextBox.Text = (player.Value * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat);
                    if (player.SaleValue != -1)
                        this.PlayerProfileSaleValueTextBox.Text = (player.SaleValue * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat);
                    else
                        this.PlayerProfileSaleValueTextBox.Text = "No Sale Value";

                    if (player.PhysicalSkills.LeftFoot > 75 && player.PhysicalSkills.RightFoot > 75)
                        this.PlayerProfilePreferredFootTextBox.Text = "Both Feet";
                    else if (player.PhysicalSkills.LeftFoot > 75)
                        this.PlayerProfilePreferredFootTextBox.Text = "Left Foot";
                    else if (player.PhysicalSkills.RightFoot > 75)
                        this.PlayerProfilePreferredFootTextBox.Text = "Right Foot";
                    this.PlayerProfileLocalReputationTextBox.Text = player.CurrentPrestige.ToString();
                    this.PlayerProfileNationalReputationTextBox.Text = player.NationalPrestige.ToString();
                    this.PlayerProfileWorldReputationTextBox.Text = player.InternationalPrestige.ToString();
                    this.PlayerProfileCATextBox.Text = player.CurrentPlayingAbility.ToString() + " CA";
                    this.PlayerProfilePATextBox.Text = player.PotentialPlayingAbility.ToString() + " PA";
                    string playerPosition = "";
                    List<string> positions = new List<string>();
                    List<string> sides = new List<string>();
                    find_player_position(player, ref playerPosition, ref positions, ref sides, true);

                    this.PlayerProfileShortlistButton.Visible = true;
                    if (this.shortlistIDList.Contains(Int32.Parse(this.PlayerProfileIDTextBox.Text)))
                    {
                        this.PlayerProfileShortlistButton.BackgroundImage = global::FMScout.Properties.Resources.Minus;
                        this.PlayerProfileShortlistTextBox.Text = "Shortlisted";
                    }
                    else
                    {
                        this.PlayerProfileShortlistButton.BackgroundImage = global::FMScout.Properties.Resources.Add;
                        this.PlayerProfileShortlistTextBox.Text = "Not Shortlisted"; 
                    }

                    this.PlayerProfilePositionTextBox.Text = playerPosition;

                    if (playerPosition.Contains("GK")) playerProfileViewGoalkeepingSkills();
                    else playerProfileViewTechnicalSkills();

                    if (playerAppearance != 0)
                        this.PlayerProfileAppearanceBonusTextBox.Text = (playerAppearance * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat) + this.preferencesForm.WagesString;
                    else
                        this.PlayerProfileAppearanceBonusTextBox.Text = "No Bonus";

                    if (playerContractWage != 0)
                        this.PlayerProfileWageTextBox.Text = (playerContractWage * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat) + this.preferencesForm.WagesString;
                    else
                        this.PlayerProfileWageTextBox.Text = "No Wage";

                    if (playerGoal != 0)
                        this.PlayerProfileGoalBonusTextBox.Text = (playerGoal * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat) + this.preferencesForm.WagesString;
                    else
                        this.PlayerProfileGoalBonusTextBox.Text = "No Bonus";

                    if (playerCleanSheet != 0)
                        this.PlayerProfileCleanSheetBonusTextBox.Text = (playerCleanSheet * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat) + this.preferencesForm.WagesString;
                    else
                        this.PlayerProfileCleanSheetBonusTextBox.Text = "No Bonus";

                    // technical skills
                    this.PlayerProfileCornersTextBox.Text = ((int)(player.TechnicalSkills.Corners * 0.2 + 0.5)).ToString();
                    this.PlayerProfileCrossingTextBox.Text = ((int)(player.TechnicalSkills.Crossing * 0.2 + 0.5)).ToString();
                    this.PlayerProfileDribblingTextBox.Text = ((int)(player.TechnicalSkills.Dribbling * 0.2 + 0.5)).ToString();
                    this.PlayerProfileFinishingTextBox.Text = ((int)(player.TechnicalSkills.Finishing * 0.2 + 0.5)).ToString();
                    this.PlayerProfileFirstTouchTextBox.Text = ((int)(player.TechnicalSkills.FirstTouch * 0.2 + 0.5)).ToString();
                    this.PlayerProfileFreeKicksTextBox.Text = ((int)(player.TechnicalSkills.Freekicks * 0.2 + 0.5)).ToString();
                    this.PlayerProfileHeadingTextBox.Text = ((int)(player.TechnicalSkills.Heading * 0.2 + 0.5)).ToString();
                    this.PlayerProfileLongShotsTextBox.Text = ((int)(player.TechnicalSkills.LongShots * 0.2 + 0.5)).ToString();
                    this.PlayerProfileLongThrowsTextBox.Text = ((int)(player.TechnicalSkills.Longthrows * 0.2 + 0.5)).ToString();
                    this.PlayerProfileMarkingTextBox.Text = ((int)(player.TechnicalSkills.Marking * 0.2 + 0.5)).ToString();
                    this.PlayerProfilePassingTextBox.Text = ((int)(player.TechnicalSkills.Passing * 0.2 + 0.5)).ToString();
                    this.PlayerProfilePenaltyTakingTextBox.Text = ((int)(player.TechnicalSkills.PenaltyTaking * 0.2 + 0.5)).ToString();
                    this.PlayerProfileTacklingTextBox.Text = ((int)(player.TechnicalSkills.Tackling * 0.2 + 0.5)).ToString();
                    this.PlayerProfileTechniqueTextBox.Text = ((int)(player.TechnicalSkills.Technique * 0.2 + 0.5)).ToString();

                    // goalkeeping skills
                    this.PlayerProfileAerialAbilityTextBox.Text = ((int)(player.GoalKeepingSkills.AerialAbility * 0.2 + 0.5)).ToString();
                    this.PlayerProfileCommandOfAreaTextBox.Text = ((int)(player.GoalKeepingSkills.CommandOfArea * 0.2 + 0.5)).ToString();
                    this.PlayerProfileCommunicationTextBox.Text = ((int)(player.GoalKeepingSkills.Communication * 0.2 + 0.5)).ToString();
                    this.PlayerProfileEccentricityTextBox.Text = ((int)(player.GoalKeepingSkills.Eccentricity * 0.2 + 0.5)).ToString();
                    this.PlayerProfileHandlingTextBox.Text = ((int)(player.GoalKeepingSkills.Handling * 0.2 + 0.5)).ToString();
                    this.PlayerProfileKickingTextBox.Text = ((int)(player.GoalKeepingSkills.Kicking * 0.2 + 0.5)).ToString();
                    this.PlayerProfileOneOnOnesTextBox.Text = ((int)(player.GoalKeepingSkills.OneOnOnes * 0.2 + 0.5)).ToString();
                    this.PlayerProfileReflexesTextBox.Text = ((int)(player.GoalKeepingSkills.Reflexes * 0.2 + 0.5)).ToString();
                    this.PlayerProfileRushingOutTextBox.Text = ((int)(player.GoalKeepingSkills.RushingOut * 0.2 + 0.5)).ToString();
                    this.PlayerProfileTendencyToPunchTextBox.Text = ((int)(player.GoalKeepingSkills.TendencyToPunch * 0.2 + 0.5)).ToString();
                    this.PlayerProfileThrowingTextBox.Text = ((int)(player.GoalKeepingSkills.Throwing * 0.2 + 0.5)).ToString();

                    // physical skills
                    this.PlayerProfileAccelerationTextBox.Text = ((int)(player.PhysicalSkills.Acceleration * 0.2 + 0.5)).ToString();
                    this.PlayerProfileAgilityTextBox.Text = ((int)(player.PhysicalSkills.Agility * 0.2 + 0.5)).ToString();
                    this.PlayerProfileBalanceTextBox.Text = ((int)(player.PhysicalSkills.Balance * 0.2 + 0.5)).ToString();
                    this.PlayerProfileJumpingTextBox.Text = ((int)(player.PhysicalSkills.Jumping * 0.2 + 0.5)).ToString();
                    this.PlayerProfileNaturalFitnessTextBox.Text = ((int)(player.PhysicalSkills.NaturalFitness * 0.2 + 0.5)).ToString();
                    this.PlayerProfilePaceTextBox.Text = ((int)(player.PhysicalSkills.Pace * 0.2 + 0.5)).ToString();
                    this.PlayerProfileStaminaTextBox.Text = ((int)(player.PhysicalSkills.Stamina * 0.2 + 0.5)).ToString();
                    this.PlayerProfileStrengthTextBox.Text = ((int)(player.PhysicalSkills.Strength * 0.2 + 0.5)).ToString();

                    // mental skills
                    this.PlayerProfileAggressionTextBox.Text = ((int)(player.MentalSkills.Aggression * 0.2 + 0.5)).ToString();
                    this.PlayerProfileAnticipationTextBox.Text = ((int)(player.MentalSkills.Anticipation * 0.2 + 0.5)).ToString();
                    this.PlayerProfileBraveryTextBox.Text = ((int)(player.MentalSkills.Bravery * 0.2 + 0.5)).ToString();
                    this.PlayerProfileComposureTextBox.Text = ((int)(player.MentalSkills.Composure * 0.2 + 0.5)).ToString();
                    this.PlayerProfileConcentrationTextBox.Text = ((int)(player.MentalSkills.Concentration * 0.2 + 0.5)).ToString();
                    this.PlayerProfileCreativityTextBox.Text = ((int)(player.MentalSkills.Creativity * 0.2 + 0.5)).ToString();
                    this.PlayerProfileDecisionsTextBox.Text = ((int)(player.MentalSkills.Decisions * 0.2 + 0.5)).ToString();
                    this.PlayerProfileDeterminationTextBox.Text = ((int)(player.MentalSkills.Determination * 0.2 + 0.5)).ToString();
                    this.PlayerProfileFlairTextBox.Text = ((int)(player.MentalSkills.Flair * 0.2 + 0.5)).ToString();
                    this.PlayerProfileInfluenceTextBox.Text = ((int)(player.MentalSkills.Influence * 0.2 + 0.5)).ToString();
                    this.PlayerProfileOffTheBallTextBox.Text = ((int)(player.MentalSkills.OffTheBall * 0.2 + 0.5)).ToString();
                    this.PlayerProfilePositioningTextBox.Text = ((int)(player.MentalSkills.Positioning * 0.2 + 0.5)).ToString();
                    this.PlayerProfileTeamWorkTextBox.Text = ((int)(player.MentalSkills.Teamwork * 0.2 + 0.5)).ToString();
                    this.PlayerProfileWorkRateTextBox.Text = ((int)(player.MentalSkills.Workrate * 0.2 + 0.5)).ToString();

                    // hidden skills
                    this.PlayerProfileConsistencyTextBox.Text = ((int)(player.HiddenSkills.Consistency * 0.2 + 0.5)).ToString();
                    this.PlayerProfileDirtynessTextBox.Text = ((int)(player.HiddenSkills.Dirtyness * 0.2 + 0.5)).ToString();
                    this.PlayerProfileImportantMatchesTextBox.Text = ((int)(player.HiddenSkills.ImportantMatches * 0.2 + 0.5)).ToString();
                    this.PlayerProfileInjuryPronenessTextBox.Text = ((int)(player.HiddenSkills.InjuryProness * 0.2 + 0.5)).ToString();
                    this.PlayerProfileVersatilityTextBox.Text = ((int)(player.HiddenSkills.Versatility * 0.2 + 0.5)).ToString();

                    // mental traits skills
                    this.PlayerProfileAdaptabilityTextBox.Text = player.MentalTraitsSkills.Adaptability.ToString();
                    this.PlayerProfileAmbitionTextBox.Text = player.MentalTraitsSkills.Ambition.ToString();
                    this.PlayerProfileControversyTextBox.Text = player.MentalTraitsSkills.Controversy.ToString();
                    this.PlayerProfileLoyaltyTextBox.Text = player.MentalTraitsSkills.Loyalty.ToString();
                    this.PlayerProfilePressureTextBox.Text = player.MentalTraitsSkills.Pressure.ToString();
                    this.PlayerProfileProfessionalismTextBox.Text = player.MentalTraitsSkills.Professionalism.ToString();
                    this.PlayerProfileSportsmanshipTextBox.Text = player.MentalTraitsSkills.Sportsmanship.ToString();
                    this.PlayerProfileTemperamentTextBox.Text = player.MentalTraitsSkills.Temperament.ToString();

                    // other
                    this.PlayerProfileConditionTextBox.Text = (player.Condition * 0.01f).ToString("P0", this.preferencesForm.CurrencyFormat);
                    this.PlayerProfileMoraleTextBox.Text = player.Morale.ToString();
                    this.PlayerProfileHappinessTextBox.Text = (player.Contract.Happiness * 0.01f).ToString("P0", this.preferencesForm.CurrencyFormat);
                    this.PlayerProfileJadednessTextBox.Text = player.Jadedness.ToString();
                    this.PlayerProfileSquadNoTextBox.Text = player.Contract.SquadNumber.ToString();
                    this.PlayerProfileLeftFootTextBox.Text = ((int)(player.PhysicalSkills.LeftFoot * 0.2 + 0.5)).ToString();
                    this.PlayerProfileRightFootTextBox.Text = ((int)(player.PhysicalSkills.RightFoot * 0.2 + 0.5)).ToString();

                    // positional ratings
                    if (!this.playersPRID.Contains(player.ID))
                        calculatePlayerPR(player);
                    PositionalRatings pr = (PositionalRatings)this.playersPRID[player.ID];
                    this.PlayerProfilePositionalRatingLabel.Text = "Positional Rating:\r\nBest as: " + pr.desc + "\r\n(" + (pr.bestPosR * 0.01f).ToString("P2") + ")";
                    this.PlayerProfileGKLabel.Text = (pr.GK * 0.01f).ToString("P2");
                    this.PlayerProfileDCLabel.Text = (pr.DC * 0.01f).ToString("P2");
                    this.PlayerProfileDLLabel.Text = (pr.DL * 0.01f).ToString("P2");
                    this.PlayerProfileDRLabel.Text = (pr.DR * 0.01f).ToString("P2");
                    this.PlayerProfileDMCLabel.Text = (pr.DMC * 0.01f).ToString("P2");
                    this.PlayerProfileDMLLabel.Text = (pr.DML * 0.01f).ToString("P2");
                    this.PlayerProfileDMRLabel.Text = (pr.DMR * 0.01f).ToString("P2");
                    this.PlayerProfileAMCLabel.Text = (pr.AMC * 0.01f).ToString("P2");
                    this.PlayerProfileAMLLabel.Text = (pr.AML * 0.01f).ToString("P2");
                    this.PlayerProfileAMRLabel.Text = (pr.AMR * 0.01f).ToString("P2");
                    this.PlayerProfileFCQuickLabel.Text = (pr.FCQuick * 0.01f).ToString("P2");
                    this.PlayerProfileFCStrongLabel.Text = (pr.FCStrong * 0.01f).ToString("P2");

                    if (pr.GK == pr.bestPosR) this.PlayerProfileGKLabel.Font = themeFontBold;
                    if (pr.DC == pr.bestPosR) this.PlayerProfileDCLabel.Font = themeFontBold;
                    if (pr.DL == pr.bestPosR) this.PlayerProfileDLLabel.Font = themeFontBold;
                    if (pr.DR == pr.bestPosR) this.PlayerProfileDRLabel.Font = themeFontBold;
                    if (pr.DMC == pr.bestPosR) this.PlayerProfileDMCLabel.Font = themeFontBold;
                    if (pr.DML == pr.bestPosR) this.PlayerProfileDMLLabel.Font = themeFontBold;
                    if (pr.DMR == pr.bestPosR) this.PlayerProfileDMRLabel.Font = themeFontBold;
                    if (pr.AMC == pr.bestPosR) this.PlayerProfileAMCLabel.Font = themeFontBold;
                    if (pr.AML == pr.bestPosR) this.PlayerProfileAMLLabel.Font = themeFontBold;
                    if (pr.AMR == pr.bestPosR) this.PlayerProfileAMRLabel.Font = themeFontBold;
                    if (pr.FCQuick == pr.bestPosR) this.PlayerProfileFCQuickLabel.Font = themeFontBold;
                    if (pr.FCStrong == pr.bestPosR) this.PlayerProfileFCStrongLabel.Font = themeFontBold;

                    // editing
                    if (this.preferencesForm.AllowEditing)
                    {
                        this.PlayerProfileHeightTextBox.Text = (float.Parse(player.Length.ToString()) * this.preferencesForm.HeightMultiplier).ToString();
                        this.PlayerProfileWeightTextBox.Text = (float.Parse(player.Weight.ToString()) * this.preferencesForm.WeightMultiplier).ToString();
                        this.PlayerProfileCATextBox.Text = player.CurrentPlayingAbility.ToString();
                        this.PlayerProfilePATextBox.Text = player.PotentialPlayingAbility.ToString();
                        this.PlayerProfileConditionTextBox.Text = player.Condition.ToString();

                        if (this.PlayerProfileClubTextBox.Text.Equals("Free Player"))
                        {
                            this.PlayerProfileValueTextBox.ReadOnly = true;
                            this.PlayerProfileSaleValueTextBox.ReadOnly = true;
                            this.PlayerProfileCleanSheetBonusTextBox.ReadOnly = true;
                            this.PlayerProfileGoalBonusTextBox.ReadOnly = true;
                            this.PlayerProfileAppearanceBonusTextBox.ReadOnly = true;
                            this.PlayerProfileWageTextBox.ReadOnly = true;
                        }
                        else
                        {
                            this.PlayerProfileValueTextBox.Text = ((int)(player.Value * this.preferencesForm.Currency)).ToString();
                            this.PlayerProfileSaleValueTextBox.Text = ((int)(player.SaleValue * this.preferencesForm.Currency)).ToString();
                            this.PlayerProfileCleanSheetBonusTextBox.Text = ((int)(playerCleanSheet * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                            this.PlayerProfileGoalBonusTextBox.Text = ((int)(playerGoal * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                            this.PlayerProfileAppearanceBonusTextBox.Text = ((int)(playerAppearance * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                            this.PlayerProfileWageTextBox.Text = ((int)(playerContractWage * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();

                            if (player.CoContract != null)
                            {
                                if (player.Team.Club.Name.Equals(player.CoContract.Club.Name))
                                {
                                    this.PlayerProfileCleanSheetBonusTextBox.Text = ((int)(playerCleanSheet * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                                    this.PlayerProfileGoalBonusTextBox.Text = ((int)(playerGoal * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                                    this.PlayerProfileAppearanceBonusTextBox.Text = ((int)(playerAppearance * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                                    this.PlayerProfileWageTextBox.Text = ((int)(playerContractWage * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                                }
                            }
                            if (player.LoanContract != null)
                            {
                                this.PlayerProfileCleanSheetBonusTextBox.Text = ((int)(playerCleanSheet * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                                this.PlayerProfileGoalBonusTextBox.Text = ((int)(playerGoal * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                                this.PlayerProfileAppearanceBonusTextBox.Text = ((int)(playerAppearance * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                                this.PlayerProfileWageTextBox.Text = ((int)(playerContractWage * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                            }
                        }
                    }

                    setPlayerProfileColor();

                    if (selectTab)
                        this.PlayersTabControl.SelectedTab = this.PlayerProfile;
                    return;
                }
            }
        }

        private void setStaffProfile(int staffID, bool selectTab)
        {
            clearStaffProfileTab();
            foreach (Staff staff in fm.NonPlayingStaff)
            {
                if (staffID.Equals(staff.ID))
                {
                    if (this.preferencesForm.AllowEditing)
                    {
                        this.ProfileSaveEditingToolStrip.Enabled = true;
                        this.ProfileCancelEditingToolStrip.Enabled = true;
                        setStaffEditing(true);
                    }

                    // personal details
                    this.StaffProfileIDTextBox.Text = staff.ID.ToString();
                    this.StaffProfileFullNameTextBox.Text = staff.ToString();
                    if (staff.Contract.Club.Name.Length == 0 && staff.Team.Club.Chairman.ID != staff.ID) // && staff.Team.Club.Directors[0].ID != staff.ID)
                        this.StaffProfileClubTextBox.Text = "Free Agent";
                    else
                    {
                        // FIXME: directors loop
                        if (staff.Team.Club.Chairman.ID != staff.ID)
                        {
                            if (staff.Team.Club.Directors.Count > 0)
                            {
                                if (staff.Team.Club.Directors[0].ID != staff.ID)
                                    this.StaffProfileClubTextBox.Text = staff.Contract.Club.Name;
                            }
                        }
                        else
                            this.StaffProfileClubTextBox.Text = staff.Team.Club.Name;
                    }
                    this.StaffProfileNationalityTextBox.Text = staff.Nationality.Name + " (" + staff.Nationality.Continent.Name + ")";
                    if (staff.InternationalCaps != 0)
                        this.StaffProfileInternationalTextBox.Text = staff.InternationalCaps.ToString() + " caps/" + staff.InternationalGoals.ToString() + " goals";
                    else
                        this.StaffProfileInternationalTextBox.Text = "Uncapped";
                    this.StaffProfileBirthDateTextBox.Text = staff.DateOfBirth.Date.ToShortDateString();
                    this.StaffProfileAgeTextBox.Text = staff.Age.ToString() + " years old, Male";
                    this.StaffProfileLocalReputationTextBox.Text = staff.CurrentPrestige.ToString();
                    this.StaffProfileNationalReputationTextBox.Text = staff.NationalPrestige.ToString();
                    this.StaffProfileWorldReputationTextBox.Text = staff.InternationalPrestige.ToString();
                    string staffRole = "";
                    find_staff_role(staff, ref staffRole, true);
                    if (staffRole.Length == 0)
                        this.StaffProfileRoleTextBox.Text = "No Role";
                    else
                        this.StaffProfileRoleTextBox.Text = staffRole;
                    if (StaffProfileClubTextBox.Text.Equals("Free Agent") || StaffProfileRoleTextBox.Text.Equals("Chairman")
                        || StaffProfileRoleTextBox.Text.Equals("Director"))
                    {
                        this.StaffProfileContractStartedTextBox.Text = "No contract";
                        this.StaffProfileContractExpiryTextBox.Text = "No contract";
                        this.StaffProfileWageTextBox.Text = "No Wage";
                    }
                    else
                    {
                        this.StaffProfileContractStartedTextBox.Text = staff.Contract.ContractStarted.Date.ToShortDateString();
                        this.StaffProfileContractExpiryTextBox.Text = staff.Contract.ContractExpiryDate.Date.ToShortDateString();
                        this.StaffProfileWageTextBox.Text = (staff.Contract.WagePerWeek * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat) + this.preferencesForm.WagesString;
                    }

                    if (staff.NationalTeam.Club.Name.Length != 0)
                    {
                        if (nationalTeam.Contains(staff.NationalTeam.Club.Country.Name))
                            this.StaffProfileClubTextBox.Text = staff.NationalTeam.Club.Name;
                    }

                    this.StaffProfileCATextBox.Text = staff.CurrentCoachingAbility + " CA";
                    this.StaffProfilePATextBox.Text = staff.PotentialCoachingAbility + " PA";

                    //for (int i = 0; i < 10; ++i)
                    //{
                    //    Console.Write(fm.NonPlayingStaff[i].MemoryAddress);
                    //    Console.Write(" ");
                    //    Console.WriteLine(ProcessManager.ReadInt32(fm.NonPlayingStaff[i].MemoryAddress));
                    //}

                    //for (int i = 0; i < 10; ++i)
                    //{
                    //    Console.Write(fm.Players[i].MemoryAddress);
                    //    Console.Write(" ");
                    //    Console.WriteLine(ProcessManager.ReadInt32(fm.Players[i].MemoryAddress));
                    //}

                    //for (int i = 0; i < 4; ++i)
                    //{
                    //    Console.Write(fm.HumanManagers[i].MemoryAddress);
                    //    Console.Write(" ");
                    //}

                    // coaching skills
                    this.StaffProfileAttackingTextBox.Text = ((int)(staff.CoachingSkills.CoachingAttacking * 0.2 + 0.5)).ToString();
                    this.StaffProfileDefendingTextBox.Text = ((int)(staff.CoachingSkills.CoachingDefending * 0.2 + 0.5)).ToString();
                    this.StaffProfileFitnessTextBox.Text = ((int)(staff.CoachingSkills.CoachingFitness * 0.2 + 0.5)).ToString();
                    this.StaffProfileGoalkeepersTextBox.Text = ((int)(staff.CoachingSkills.CoachingGoalkeepers * 0.2 + 0.5)).ToString();
                    this.StaffProfileMentalTextBox.Text = ((int)(staff.CoachingSkills.CoachingMental * 0.2 + 0.5)).ToString();
                    this.StaffProfilePlayerTextBox.Text = ((int)(staff.CoachingSkills.CoachingPlayer * 0.2 + 0.5)).ToString();
                    this.StaffProfileTacticalTextBox.Text = ((int)(staff.CoachingSkills.CoachingTactical * 0.2 + 0.5)).ToString();
                    this.StaffProfileTechnicalTextBox.Text = ((int)(staff.CoachingSkills.CoachingTechnical * 0.2 + 0.5)).ToString();
                    this.StaffProfileManManagementTextBox.Text = ((int)(staff.CoachingSkills.ManManagement * 0.2 + 0.5)).ToString();
                    this.StaffProfileWorkingWithYoungstersTextBox.Text = ((int)(staff.CoachingSkills.WorkingWithYoungsters + 0.5)).ToString();

                    // mental skills
                    this.StaffProfileAdaptabilityTextBox.Text = ((int)(staff.MentalSkills.Adaptability + 0.5)).ToString();
                    this.StaffProfileAmbitionTextBox.Text = ((int)(staff.StaffMentalTraitsSkills.Ambition + 0.5)).ToString();
                    this.StaffProfileControversyTextBox.Text = ((int)(staff.StaffMentalTraitsSkills.Controversy + 0.5)).ToString();
                    this.StaffProfileDeterminationTextBox.Text = ((int)(staff.StaffMentalTraitsSkills.Determination * 0.2 + 0.5)).ToString();
                    this.StaffProfileLoyaltyTextBox.Text = ((int)(staff.StaffMentalTraitsSkills.Loyalty + 0.5)).ToString();
                    this.StaffProfilePressureTextBox.Text = ((int)(staff.StaffMentalTraitsSkills.Pressure + 0.5)).ToString();
                    this.StaffProfileProfessionalismTextBox.Text = ((int)(staff.StaffMentalTraitsSkills.Professionalism + 0.5)).ToString();
                    this.StaffProfileSportsmanshipTextBox.Text = ((int)(staff.StaffMentalTraitsSkills.Sportsmanship + 0.5)).ToString();
                    this.StaffProfileTemperamentTextBox.Text = ((int)(staff.StaffMentalTraitsSkills.Temperament + 0.5)).ToString();
                    this.StaffProfileJudgingPlayerAbilityTextBox.Text = ((int)(staff.MentalSkills.JudgingPlayerAbility * 0.2 + 0.5)).ToString();
                    this.StaffProfileJudgingPlayerPotentialTextBox.Text = ((int)(staff.MentalSkills.JudgingPlayerPotential * 0.2 + 0.5)).ToString();
                    this.StaffProfileLevelOfDisciplineTextBox.Text = ((int)(staff.MentalSkills.LevelOfDiscipline + 0.5)).ToString();
                    this.StaffProfileMotivatingTextBox.Text = ((int)(staff.MentalSkills.Motivating * 0.2 + 0.5)).ToString();
                    this.StaffProfilePhysiotherapyTextBox.Text = ((int)(staff.MentalSkills.Physiotherapy * 0.2 + 0.5)).ToString();
                    this.StaffProfileTacticalKnowledgeTextBox.Text = ((int)(staff.MentalSkills.TacticalKnowledge * 0.2 + 0.5)).ToString();

                    // tactical skills
                    this.StaffProfileDepthTextBox.Text = ((int)(staff.TacticalSkills.Depth + 0.5)).ToString();
                    this.StaffProfileDirectnessTextBox.Text = ((int)(staff.TacticalSkills.Directness + 0.5)).ToString();
                    this.StaffProfileFlamboyancyTextBox.Text = ((int)(staff.TacticalSkills.Flamboyancy + 0.5)).ToString();
                    this.StaffProfileFlexibilityTextBox.Text = ((int)(staff.TacticalSkills.Flexibility + 0.5)).ToString();
                    this.StaffProfileFreeRolesTextBox.Text = ((int)(staff.TacticalSkills.FreeRoles + 0.5)).ToString();
                    this.StaffProfileMarkingTextBox.Text = ((int)(staff.TacticalSkills.Marking + 0.5)).ToString();
                    this.StaffProfileOffsideTextBox.Text = ((int)(staff.TacticalSkills.OffSide + 0.5)).ToString();
                    this.StaffProfilePressingTextBox.Text = ((int)(staff.TacticalSkills.Pressing + 0.5)).ToString();
                    this.StaffProfileSittingBackTextBox.Text = ((int)(staff.TacticalSkills.SittingBack + 0.5)).ToString();
                    this.StaffProfileTempoTextBox.Text = ((int)(staff.TacticalSkills.Tempo + 0.5)).ToString();
                    this.StaffProfileUseOfPlaymakerTextBox.Text = ((int)(staff.TacticalSkills.UseOfPlaymaker + 0.5)).ToString();
                    this.StaffProfileUseOfSubstitutionsTextBox.Text = ((int)(staff.TacticalSkills.UseOfSubstitutions + 0.5)).ToString();
                    this.StaffProfileWidthTextBox.Text = ((int)(staff.TacticalSkills.Width + 0.5)).ToString();

                    // non tactical skills
                    this.StaffProfileBuyingPlayersTextBox.Text = ((int)(staff.NonTacticalSkills.BuyingPlayers + 0.5)).ToString();
                    this.StaffProfileHardnessOfTrainingTextBox.Text = ((int)(staff.NonTacticalSkills.HardnessOfTraining + 0.5)).ToString();
                    this.StaffProfileMindGamesTextBox.Text = ((int)(staff.NonTacticalSkills.MindGames + 0.5)).ToString();
                    this.StaffProfileSquadRotationTextBox.Text = ((int)(staff.NonTacticalSkills.SquadRotation + 0.5)).ToString();

                    // chairman skills
                    this.StaffProfileBusinessTextBox.Text = ((int)(staff.ChairmanSkills.Business + 0.5)).ToString();
                    this.StaffProfileInterferenceTextBox.Text = ((int)(staff.ChairmanSkills.Interference + 0.5)).ToString();
                    this.StaffProfilePatienceTextBox.Text = ((int)(staff.ChairmanSkills.Patience + 0.5)).ToString();
                    this.StaffProfileResourcesTextBox.Text = ((int)(staff.ChairmanSkills.Resources + 0.5)).ToString();

                    // coaching ratings
                    if (!this.staffCRID.Contains(staff.ID))
                        calculateStaffCR(staff);

                    this.StaffProfileBestRatingsGroupBox.Refresh();

                    // editing
                    if (this.preferencesForm.AllowEditing)
                    {
                        this.StaffProfileCATextBox.Text = staff.CurrentCoachingAbility.ToString();
                        this.StaffProfilePATextBox.Text = staff.PotentialCoachingAbility.ToString();
                        if (this.StaffProfileClubTextBox.Text.Equals("Free Agent"))
                            this.StaffProfileWageTextBox.ReadOnly = true;
                        else
                            this.StaffProfileWageTextBox.Text = ((int)(staff.Contract.WagePerWeek * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString();
                    }

                    setStaffProfileColor();

                    if (selectTab)
                        this.StaffTabControl.SelectedTab = this.StaffProfile;
                    return;
                }
            }
        }

        private void setTeamProfile(int teamID, bool selectTab)
        {
            clearTeamProfileTab();
            foreach (Team team in fm.Teams)
            {
                if (teamID.Equals(team.Club.ID))
                {
                    string isNational = "No";
                    if (nationalTeam.Contains(team.Club.Country.Name)) isNational = "Yes";

                    this.TeamProfileListPlayersComboBox.Items.Clear();
                    if (isNational.Equals("No"))
                    {
                        this.TeamProfileListStaffButton.Enabled = true;
                        this.TeamProfileListPlayersComboBox.Items.Add("All Squads");
                        for (int i = 0; i < team.Club.Teams.Count; ++i)
                        {
                            if (team.Club.Teams[i].Type != TeamType.Empty)
                                this.TeamProfileListPlayersComboBox.Items.Add(team.Club.Teams[i].Type.ToString() + " Team");
                        }

                        if (this.TeamProfileListPlayersComboBox.Items.Count > 1)
                        {
                            if (this.TeamProfileListPlayersComboBox.Items.Count == 2)
                            {
                                this.TeamProfileListPlayersComboBox.Items.RemoveAt(0);
                                this.TeamProfileListPlayersComboBox.SelectedIndex = 0;
                                this.TeamProfileListPlayersComboBox.Enabled = false;
                            }
                            else
                            {
                                this.TeamProfileListPlayersComboBox.SelectedIndex = 0;
                                this.TeamProfileListPlayersComboBox.Enabled = true;
                            }
                            this.TeamProfileListPlayersButton.Enabled = true;
                        }
                        else
                        {
                            this.TeamProfileListPlayersButton.Enabled = true;
                            this.TeamProfileListPlayersComboBox.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        this.TeamProfileListStaffButton.Enabled = true;
                        this.TeamProfileListPlayersButton.Enabled = true;
                        this.TeamProfileListPlayersComboBox.Enabled = false;
                        this.TeamProfileListPlayersComboBox.Items.Add("Nation Players");
                        this.TeamProfileListPlayersComboBox.SelectedIndex = 0;
                    }

                    if (this.preferencesForm.AllowEditing)
                    {
                        if (this.TeamProfileListPlayersComboBox.Items.Count > 1 && isNational.Equals("No"))
                            this.TeamProfileHealTeamButton.Enabled = true;
                        this.ProfileSaveEditingToolStrip.Enabled = true;
                        this.ProfileCancelEditingToolStrip.Enabled = true;
                        setTeamEditing(true);
                    }

                    // general details
                    this.TeamProfileIDTextBox.Text = team.Club.ID.ToString();
                    this.TeamProfileNameTextBox.Text = team.Club.Name;
                    if (isNational.Equals("No"))
                    {
                        this.TeamProfileNationalityTextBox.Text = team.Club.Country.Name;
                        this.TeamProfileNationalityTextBox.Text += " (" + team.Club.Country.Continent.Name + ")";
                        this.TeamProfileYearFoundedTextBox.Text = team.Club.YearFounded.ToString();
                        this.TeamProfileMaxAttendanceTextBox.Text = team.Club.MaximumAttendance.ToString();
                        this.TeamProfileAverageAttendanceTextBox.Text = team.Club.AverageAttendance.ToString();
                        this.TeamProfileMinAttendanceTextBox.Text = team.Club.MinimumAttendance.ToString();

                        this.TeamProfileFinanceDetailsGroupBox.Visible = true;
                        this.TeamProfileStadiumDetailsGroupBox.Visible = true;

                        // finance details
                        if (team.Club.Finances.RemainingTransferBudget != 27571580)
                        {
                            this.TeamProfileTotalTransferTextBox.Text = (team.Club.Finances.SeasonTransferBudget * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat);
                            this.TeamProfileRemainingTransferTextBox.Text = (team.Club.Finances.RemainingTransferBudget * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat);
                            this.TeamProfileTotalWageTextBox.Text = (team.Club.Finances.WageBudget * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat);
                            this.TeamProfileUsedWageTextBox.Text = (team.Club.Finances.UsedWage * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat);
                        }
                        else
                        {
                            this.TeamProfileTotalTransferTextBox.Text = "No budget";
                            this.TeamProfileRemainingTransferTextBox.Text = "No budget";
                            this.TeamProfileTotalWageTextBox.Text = "No budget";
                            this.TeamProfileUsedWageTextBox.Text = "No budget";
                        }

                        this.TeamProfileBalanceTextBox.Text = (team.Club.Finances.Balance * this.preferencesForm.Currency).ToString("C0", this.preferencesForm.CurrencyFormat);

                        if (team.Club.Finances.TransferRevenueMadeAvailable != 0)
                            this.TeamProfileRevenueAvailableTextBox.Text = (team.Club.Finances.TransferRevenueMadeAvailable * 0.01f).ToString("P0");
                        else
                            this.TeamProfileRevenueAvailableTextBox.Text = "0%";

                        // stadium details
                        this.TeamProfileStadiumIDTextBox.Text = team.Stadium.ID.ToString();
                        this.TeamProfileStadiumNameTextBox.Text = team.Stadium.Name.ToString();
                        if (team.Stadium.Owner.Name.Length != 0)
                            this.TeamProfileStadiumOwnerTextBox.Text = team.Stadium.Owner.Name.ToString();
                        else
                            this.TeamProfileStadiumOwnerTextBox.Text = "No Owner";
                        this.TeamProfileStadiumLocationTextBox.Text = team.Stadium.City.Name.ToString();
                        if (team.Stadium.NearbyStadium.Name.Length != 0)
                            this.TeamProfileStadiumNearbyStadiumTextBox.Text = team.Stadium.NearbyStadium.Name;
                        else
                            this.TeamProfileStadiumNearbyStadiumTextBox.Text = "No Nearby Stadium";
                        this.TeamProfileStadiumDecayTextBox.Text = team.Stadium.Decay.ToString();
                        this.TeamProfileStadiumFieldWidthTextBox.Text = (float.Parse(team.Stadium.FieldWidth.ToString())).ToString() + "m";
                        this.TeamProfileStadiumFieldLengthTextBox.Text = (float.Parse(team.Stadium.FieldLength.ToString())).ToString() + "m";
                        this.TeamProfileStadiumCurrentCapacityTextBox.Text = team.Stadium.StadiumCapacity.ToString();
                        this.TeamProfileStadiumSeatingCapacityTextBox.Text = team.Stadium.SeatingCapacity.ToString();
                        this.TeamProfileStadiumExpansionCapacityTextBox.Text = team.Stadium.ExpansionCapacity.ToString();
                        this.TeamProfileStadiumUsedCapacityTextBox.Text = team.Stadium.UsedCapacity.ToString();
                    }
                    else
                    {
                        this.TeamProfileNationalityTextBox.Text = team.Club.Country.Name;
                        this.TeamProfileYearFoundedTextBox.Text = "None";
                        this.TeamProfileMaxAttendanceTextBox.Text = "None";
                        this.TeamProfileAverageAttendanceTextBox.Text = "None";
                        this.TeamProfileMinAttendanceTextBox.Text = "None";

                        this.TeamProfileFinanceDetailsGroupBox.Visible = false;
                        this.TeamProfileStadiumDetailsGroupBox.Visible = false;
                    }

                    this.TeamProfileNationalTextBox.Text = isNational;
                    this.TeamProfileStatusTextBox.Text = team.Club.Status.ToString();
                    this.TeamProfileMaxAffiliatedClubsTextBox.Text = team.Club.MaxAffiliatedClubNumber.ToString();
                    this.TeamProfileAffiliatedClubsTextBox.Text = team.Club.NumberOfAffiliatedClubs.ToString();
                    this.TeamProfileTrainingGroundTextBox.Text = team.Club.TrainingGround.ToString();
                    this.TeamProfileYouthGroundTextBox.Text = team.Club.YouthGround.ToString();
                    if (team.Club.YouthAcademy == 0)
                        this.TeamProfileYouthAcademyTextBox.Text = "No";
                    else if (team.Club.YouthAcademy == 1)
                        this.TeamProfileYouthAcademyTextBox.Text = "Yes";
                    else
                        this.TeamProfileYouthAcademyTextBox.Text = "Unavailable";
                    this.TeamProfileReputationTextBox.Text = team.Prestige.ToString();

                    // editing
                    if (this.preferencesForm.AllowEditing)
                    {
                        if (isNational.Equals("No"))
                        {
                            if (team.Club.Finances.RemainingTransferBudget != 27571580)
                            {
                                this.TeamProfileTotalTransferTextBox.Text = ((int)(team.Club.Finances.SeasonTransferBudget * this.preferencesForm.Currency)).ToString();
                                this.TeamProfileRemainingTransferTextBox.Text = ((int)(team.Club.Finances.RemainingTransferBudget * this.preferencesForm.Currency)).ToString();
                                this.TeamProfileTotalWageTextBox.Text = ((int)(team.Club.Finances.WageBudget * this.preferencesForm.Currency)).ToString();
                                this.TeamProfileUsedWageTextBox.Text = ((int)(team.Club.Finances.UsedWage * this.preferencesForm.Currency)).ToString();
                            }
                            else
                            {
                                this.TeamProfileTotalTransferTextBox.ReadOnly = true;
                                this.TeamProfileRemainingTransferTextBox.ReadOnly = true;
                                this.TeamProfileTotalWageTextBox.ReadOnly = true;
                                this.TeamProfileUsedWageTextBox.ReadOnly = true;
                            }
                            this.TeamProfileBalanceTextBox.Text = ((int)(team.Club.Finances.Balance * this.preferencesForm.Currency)).ToString();
                            this.TeamProfileStadiumFieldWidthTextBox.Text = (float.Parse(team.Stadium.FieldWidth.ToString()) * this.preferencesForm.HeightMultiplier).ToString();
                            this.TeamProfileStadiumFieldLengthTextBox.Text = (float.Parse(team.Stadium.FieldLength.ToString()) * this.preferencesForm.HeightMultiplier).ToString();

                            if (!this.TeamProfileYouthAcademyTextBox.Text.Equals("Unavailable"))
                                this.TeamProfileYouthAcademyTextBox.Text = team.Club.YouthAcademy.ToString();
                            else
                                this.TeamProfileYouthAcademyTextBox.ReadOnly = true;
                        }
                        else
                        {
                            this.TeamProfileMaxAffiliatedClubsTextBox.ReadOnly = true;
                            this.TeamProfileAffiliatedClubsTextBox.ReadOnly = true;
                            this.TeamProfileTrainingGroundTextBox.ReadOnly = true;
                            this.TeamProfileYouthGroundTextBox.ReadOnly = true;
                            this.TeamProfileYouthAcademyTextBox.ReadOnly = true;
                            this.TeamProfileTotalTransferTextBox.ReadOnly = true;
                            this.TeamProfileRemainingTransferTextBox.ReadOnly = true;
                            this.TeamProfileBalanceTextBox.ReadOnly = true;
                            this.TeamProfileTotalWageTextBox.ReadOnly = true;
                            this.TeamProfileUsedWageTextBox.ReadOnly = true;
                            this.TeamProfileStadiumDecayTextBox.ReadOnly = true;
                            this.TeamProfileStadiumFieldWidthTextBox.ReadOnly = true;
                            this.TeamProfileStadiumFieldLengthTextBox.ReadOnly = true;
                            this.TeamProfileStadiumCurrentCapacityTextBox.ReadOnly = true;
                            this.TeamProfileStadiumSeatingCapacityTextBox.ReadOnly = true;
                            this.TeamProfileStadiumExpansionCapacityTextBox.ReadOnly = true;
                            this.TeamProfileStadiumUsedCapacityTextBox.ReadOnly = true;
                            this.TeamProfileMaxAttendanceTextBox.ReadOnly = true;
                            this.TeamProfileAverageAttendanceTextBox.ReadOnly = true;
                            this.TeamProfileMinAttendanceTextBox.ReadOnly = true;
                        }
                    }

                    setTeamProfileColor();

                    if (selectTab)
                        this.TeamsTabControl.SelectedTab = this.TeamProfile;
                    return;
                }
            }
        }

        private void exportShortlistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportShortlist(true);
        }

        private void exportSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportShortlist(false);
        }

        private void loadShortlistPlayers(ref int[] playersID, bool delete)
        {
            List<string> player_positions = new List<string>();
            List<string> player_sides = new List<string>();

            if (delete)
            {
                shortlistLoanRowList.Clear();
                shortlistCoOwnRowList.Clear();
                shortlistFreeRowList.Clear();
                shortlistEUMemberRowList.Clear();
                shortlistIDList.Clear();
                shortlistDataTable.Clear();
                this.ShortlistDisplayGridView.DataSource = shortlistDataTable;
            }
            int playerClubState = 0;
            int playerEUState = 0;

            DataRow newRow;
            string playerNationality = "";
            for (int i = 0; i < playersID.Length; ++i)
            {
                Player player = null;
                foreach (Player _player in fm.Players)
                {
                    // check ID
                    player = null;
                    if (_player.ID.Equals(playersID[i]))
                    {
                        if (!shortlistIDList.Contains(_player.ID))
                        {
                            player = _player;
                            break;
                        }
                    }
                }
                if (player != null)
                {
                    // check position
                    string playerPosition = "";
                    find_player_position(player, ref playerPosition, ref player_positions, ref player_sides, true);

                    // check club
                    playerClubState = 0;
                    string playerClub = "";
                    if (player.LoanContract != null)
                    {
                        playerClub = player.Contract.Club.Name;
                        playerClubState = 2;
                    }
                    else if (player.CoContract != null)
                    {
                        playerClub = player.Contract.Club.Name + "/" + player.CoContract.Club.Name;
                        playerClubState = 1;
                    }
                    else
                        playerClub = player.Contract.Club.Name;

                    if (playerClub.Length == 0)
                    {
                        playerClub = " Free Player";
                        playerClubState = 3;
                    }

                    playerEUState = 0;
                    playerNationality = player.Nationality.Name;
                    if (EUcountries.Contains(player.Nationality.Name)) playerEUState = 1;
                    // other nationalities
                    for (int playerRelationIndex = 0; playerRelationIndex < player.RelationsTotal; ++playerRelationIndex)
                    {
                        if (player.Relations[playerRelationIndex].ObjectType == RelationObjectType.Country &&
                            player.Relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                        {
                            playerNationality += "/" + player.Relations[playerRelationIndex].Country.Name;
                            if (EUcountries.Contains(player.Relations[playerRelationIndex].Country.Name)) playerEUState = 1;
                        }
                    }

                    newRow = shortlistDataTable.NewRow();
                    addPlayerToGrid(player, ref newRow, ref playerPosition, ref playerClub, ref playerNationality);
                    shortlistIDList.Add(player.ID, shortlistIDList.Count);
                    shortlistDataTable.Rows.Add(newRow);

                    if (playerClubState == 1)
                        shortlistCoOwnRowList.Add(player.ID);
                    else if (playerClubState == 2)
                        shortlistLoanRowList.Add(player.ID);
                    else if (playerClubState == 3)
                        shortlistFreeRowList.Add(player.ID);
                    if (playerEUState == 4)
                        shortlistEUMemberRowList.Add(player.ID);

                    /*DataRow datarow;

                    if (playersIDList.Contains(player.ID))
                    {
                        datarow = ((DataRowView)(this.PlayersDisplayGridView.Rows[(int)playersIDList[player.ID]].DataBoundItem)).Row;
                        datarow["S"] = "1";
                    }*/
                }
            }
        }

        private void PlayersSearchButton_Click(object sender, EventArgs e)
        {
            this.CurrentGameDateLabel.Text = "Current Game Date: " + fm.IngameDate.ToLongDateString();

            this.PlayersSearchButton.Text = "Searching...";
            this.PlayersSearchButton.Enabled = false;
            this.PlayersResultLabel.Text += "Searching...";

            playersLoanRowList.Clear();
            playersCoOwnRowList.Clear();
            playersFreeRowList.Clear();
            playersEUMemberRowList.Clear();
            //playersIDList.Clear(); 
            playersPRTotal = 0;
            
            List<string> name_substrings = new List<string>();
            bool empty_name = multiEntryTextBox(ref name_substrings, this.PlayerFullNameTextBox.Text);
            List<string> nationality_substrings = new List<string>();
            bool empty_nationality = multiEntryTextBox(ref nationality_substrings, this.PlayerNationTextBox.Text);
            List<string> club_substrings = new List<string>();
            bool empty_club = multiEntryTextBox(ref club_substrings, this.PlayerClubTextBox.Text);
            bool empty_position = false;
            List<string> player_positions = new List<string>();
            foreach (string player_pos in this.PlayerPosition1CheckedListBox.CheckedItems)
                player_positions.Add(player_pos);
            List<string> player_sides = new List<string>();
            foreach (string player_side in this.PlayerPosition2CheckedListBox.CheckedItems)
                player_sides.Add(player_side);
            if (player_positions.Count == 0 && player_sides.Count == 0) empty_position = true;

            // init special attributes
            int[] numericUpDownArray = new int[MaxPlayerAttributes];
            initPlayersSpecialAttributes(ref numericUpDownArray);

            DateTime tempGameDate = fm.IngameDate;
            if (PlayerContractStatusComboBox.SelectedIndex >= 0)
            {
                if (PlayerContractStatusComboBox.SelectedItem.Equals("Expiring (6 months)"))
                    tempGameDate = tempGameDate.AddMonths(6);
                else if (PlayerContractStatusComboBox.SelectedItem.Equals("Expiring (1 year)"))
                    tempGameDate = tempGameDate.AddYears(1);
            }
            playersDataTable.Clear();
            this.PlayersDisplayGridView.DataSource = playersDataTable;
            this.PlayersDisplayGridView.ScrollBars = ScrollBars.None;
            int playerClubState = 0;
            int playerEUState = 0;

            DataRow newRow;
            DateTime timerStart = DateTime.Now;
            string playerName = "";
            string playerNickname = "";
            string playerNationality = "";
            Contract contract = null;
            foreach (Player player in fm.Players)
            {
                // check empty name
                if (player.FirstName.Length == 0)
                    continue;

                // check name
                if (!empty_name)
                {
                    playerName = player.ToString().ToLower();
                    playerNickname = player.Nickname.ToLower();
                    specialCharactersReplacement(ref playerName);
                    specialCharactersReplacement(ref playerNickname);
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
                    if (!find_player_position(player, ref playerPosition, ref player_positions, ref player_sides, false))
                        continue;
                }
                else
                    find_player_position(player, ref playerPosition, ref player_positions, ref player_sides, true);

                // check nation
                if (!empty_nationality)
                {
                    playerNationality = player.Nationality.Name.ToLower();
                    specialCharactersReplacement(ref playerNationality);
                    int no_of_successes = 0;
                    foreach (string str in nationality_substrings)
                    {
                        if (playerNationality.Contains(str))
                            ++no_of_successes;
                    }

                    if (no_of_successes != nationality_substrings.Count)
                        continue;
                }

                // check club
                if (this.PlayerOwnerShipComboBox.SelectedIndex > 0)
                {
                    if (this.PlayerOwnerShipComboBox.SelectedIndex == 1)
                    {
                        if (player.LoanContract == null)
                            continue;
                    }
                    else if (this.PlayerOwnerShipComboBox.SelectedIndex == 2)
                    {
                        if (player.CoContract == null)
                            continue;
                    }
                    else
                    {
                        if (player.LoanContract == null)
                            continue;
                        if (player.CoContract == null)
                            continue;
                    }
                }

                playerClubState = 0;
                string playerClub = "";
                contract = null;
                if (player.LoanContract != null)
                {
                    playerClub = player.LoanContract.Club.Name;
                    playerClubState = 2;
                    contract = player.LoanContract;
                }
                else if (player.CoContract != null)
                {
                    playerClub = player.Contract.Club.Name + "/" + player.CoContract.Club.Name;
                    playerClubState = 1;
                    contract = player.CoContract;
                }
                else
                {
                    playerClub = player.Contract.Club.Name;
                    contract = player.Contract;
                }

                if (playerClub.Length == 0)
                {
                    playerClub = " Free Player";
                    playerClubState = 3;
                }

                if (!empty_club)
                {
                    specialCharactersReplacement(ref playerClub);
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
                if (PlayerContractStatusComboBox.SelectedIndex > 0)
                {
                    if (PlayerContractStatusComboBox.SelectedIndex == 3)
                    {
                        if (!playerClub.Equals(" Free Player"))
                            continue;
                    }
                    else if (player.Contract.Club.Name.Length > 0)
                    {
                        if (player.Contract.ContractExpiryDate > tempGameDate)
                            continue;
                    }
                }

                // check region
                if (PlayerRegionComboBox.SelectedIndex > 0)
                {
                    if (!player.Nationality.Continent.Name.Contains(PlayerRegionComboBox.SelectedItem.ToString()))
                        continue;
                }

                // check preferred foot
                if (PlayerPrefFootComboBox.SelectedIndex > 0)
                {
                    if (PlayerPrefFootComboBox.SelectedIndex == 1)
                    {
                        if (player.PhysicalSkills.RightFoot <= 75)
                            continue;
                    }
                    else if (PlayerPrefFootComboBox.SelectedIndex == 2)
                    {
                        if (player.PhysicalSkills.LeftFoot <= 75)
                            continue;
                    }
                }

                // check regen
                if (PlayerRegenComboBox.SelectedIndex == 1)
                {
                    if (player.ID < 1394640000)
                        continue;
                }
                else if (PlayerRegenComboBox.SelectedIndex == 2)
                {
                    if (player.ID >= 1394640000)
                        continue;
                }

                // check EU
                playerEUState = 0;
                playerNationality = player.Nationality.Name;
                if (PlayerEUComboBox.SelectedIndex == 1)
                {
                    if (EUcountries.Contains(player.Nationality.Name)) playerEUState = 1;

                    if (playerEUState != 1)
                    {
                        // other nationalities
                        bool found = false;
                        for (int playerRelationIndex = 0; playerRelationIndex < player.RelationsTotal; ++playerRelationIndex)
                        {
                            if (player.Relations[playerRelationIndex].ObjectType == RelationObjectType.Country &&
                                player.Relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                            {
                                playerNationality += "/" + player.Relations[playerRelationIndex].Country.Name;
                                if (EUcountries.Contains(player.Relations[playerRelationIndex].Country.Name))
                                {
                                    playerEUState = 1;
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (!found) continue;
                    }
                }
                else if (PlayerEUComboBox.SelectedIndex == 2)
                {
                    if (!EUcountries.Contains(player.Nationality.Name))
                    {
                        // other nationalities
                        bool found = false;
                        for (int playerRelationIndex = 0; playerRelationIndex < player.RelationsTotal; ++playerRelationIndex)
                        {
                            if (player.Relations[playerRelationIndex].ObjectType == RelationObjectType.Country &&
                                player.Relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                            {
                                playerNationality += "/" + player.Relations[playerRelationIndex].Country.Name;
                                if (EUcountries.Contains(player.Relations[playerRelationIndex].Country.Name))
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (found) continue;
                    }
                    else continue;
                }
                else
                {
                    if (EUcountries.Contains(player.Nationality.Name)) playerEUState = 1;

                    // other nationalities
                    if (playerEUState != 1)
                    {
                        // other nationalities
                        for (int playerRelationIndex = 0; playerRelationIndex < player.RelationsTotal; ++playerRelationIndex)
                        {
                            if (player.Relations[playerRelationIndex].ObjectType == RelationObjectType.Country &&
                                player.Relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                            {
                                playerNationality += "/" + player.Relations[playerRelationIndex].Country.Name;
                                if (EUcountries.Contains(player.Relations[playerRelationIndex].Country.Name))
                                {
                                    playerEUState = 1;
                                    break;
                                }
                            }
                        }
                    }
                }

                // wages
                if (!playerClub.Equals(" Free Player"))
                {
                    if (this.PlayerWageMinNumericUpDown.Value >= contract.WagePerWeek * this.preferencesForm.WagesMultiplier ||
                        this.PlayerWageMaxNumericUpDown.Value <= contract.WagePerWeek * this.preferencesForm.WagesMultiplier)
                        continue;
                }

                // special attributes
                if (!testPlayerSpecialAttributes(player, ref numericUpDownArray))
                    continue;

                // check best positional rating
                if (this.PlayerPositionalRatingMinNumericUpDown.Value >= 0 || this.PlayerPositionalRatingMaxNumericUpDown.Value <= this.PlayerPositionalRatingMaxNumericUpDown.Maximum)
                {
                    if (!this.playersPRID.Contains(player.ID))
                        calculatePlayerPR(player);
                    if (((PositionalRatings)this.playersPRID[player.ID]).bestPosR < (float)this.PlayerPositionalRatingMinNumericUpDown.Value ||
                        ((PositionalRatings)this.playersPRID[player.ID]).bestPosR > (float)this.PlayerPositionalRatingMaxNumericUpDown.Value)
                        continue;
                }

                // check best position
                if (this.PlayerBestPositionComboBox.SelectedIndex > 0)
                {
                    if (!((PositionalRatings)this.playersPRID[player.ID]).bestPos.Equals(this.PlayerBestPositionComboBox.SelectedItem.ToString()))
                        continue;
                }

                newRow = playersDataTable.NewRow();
                addPlayerToGrid(player, ref newRow, ref playerPosition, ref playerClub, ref playerNationality);
                //playersIDList.Add(player.ID, playersIDList.Count);
                //if (shortlistIDList.Contains(player.ID)) newRow["S"] = "1";
                playersDataTable.Rows.Add(newRow);

                if (playerClubState == 1)
                    playersCoOwnRowList.Add(player.ID);
                else if (playerClubState == 2)
                    playersLoanRowList.Add(player.ID);
                else if (playerClubState == 3)
                    playersFreeRowList.Add(player.ID);
                if (playerEUState == 1)
                    playersEUMemberRowList.Add(player.ID);

                this.PlayersResultLabel.Text = playersDataTable.Rows.Count + " player entries found.";
                if (playersPRTotal > 0) this.PlayersResultLabel.Text += " Calculated PR for " + playersPRTotal + " players.";
                this.PlayersResultLabel.Refresh();
            }

            this.PlayersDisplayGridView.ScrollBars = ScrollBars.Both;
            this.PlayersResultLabel.Text = playersDataTable.Rows.Count + " player entries found.";
            if (playersPRTotal > 0) this.PlayersResultLabel.Text += " Calculated PR for " + playersPRTotal + " players.";
            this.PlayersDisplayGridView.DataSource = playersDataTable;
            DateTime timerEnd = DateTime.Now;
            TimeSpan timer = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
            this.PlayersResultLabel.Text += " Query took " + (float)timer.Seconds + " sec.";
            this.PlayersSearchButton.Enabled = true;
            this.PlayersSearchButton.Text = "Search";
        }

        private void PlayersWonderkidButton_Click(object sender, EventArgs e)
        {
            this.CurrentGameDateLabel.Text = "Current Game Date: " + fm.IngameDate.ToLongDateString();

            this.PlayersWonderkidButton.Text = "Searching...";
            this.PlayersWonderkidButton.Enabled = false;
            this.PlayersResultLabel.Text += "Searching...";

            playersLoanRowList.Clear();
            playersCoOwnRowList.Clear();
            playersFreeRowList.Clear();
            playersEUMemberRowList.Clear();
            //playersIDList.Clear();
            playersPRTotal = 0;

            List<string> player_positions = new List<string>();
            List<string> player_sides = new List<string>();

            playersDataTable.Clear();
            this.PlayersDisplayGridView.DataSource = playersDataTable;
            this.PlayersDisplayGridView.ScrollBars = ScrollBars.None;
            int playerClubState = 0;
            int playerEUState = 0;

            DataRow newRow;
            DateTime timerStart = DateTime.Now;
            string playerNationality = "";
            foreach (Player player in fm.Players)
            {
                // check empty name
                if (player.FirstName.Length == 0)
                    continue;

                // check age
                if (player.Age <= 0 || player.Age > this.preferencesForm.wonderkidsMaxAge)
                    continue;

                // check PA
                if (player.PotentialPlayingAbility < this.preferencesForm.wonderkidsMinPA)
                    continue;

                // check club
                playerClubState = 0;
                string playerClub = "";
                if (player.LoanContract != null)
                {
                    playerClub = player.Contract.Club.Name;
                    playerClubState = 2;
                }
                else if (player.CoContract != null)
                {
                    playerClub = player.Contract.Club.Name + "/" + player.CoContract.Club.Name;
                    playerClubState = 1;
                }
                else
                    playerClub = player.Contract.Club.Name;

                if (playerClub.Length == 0)
                {
                    playerClub = " Free Player";
                    playerClubState = 3;
                }

                // check position
                string playerPosition = "";
                find_player_position(player, ref playerPosition, ref player_positions, ref player_sides, true);

                playerEUState = 0;
                playerNationality = player.Nationality.Name;
                if (EUcountries.Contains(player.Nationality.Name)) playerEUState = 1;
                // other nationalities
                for (int playerRelationIndex = 0; playerRelationIndex < player.RelationsTotal; ++playerRelationIndex)
                {
                    if (player.Relations[playerRelationIndex].ObjectType == RelationObjectType.Country &&
                        player.Relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                    {
                        playerNationality += "/" + player.Relations[playerRelationIndex].Country.Name;
                        if (EUcountries.Contains(player.Relations[playerRelationIndex].Country.Name)) playerEUState = 1;
                        break;
                    }
                }

                newRow = playersDataTable.NewRow();
                addPlayerToGrid(player, ref newRow, ref playerPosition, ref playerClub, ref playerNationality);
                //playersIDList.Add(player.ID, playersIDList.Count);
                //if (shortlistIDList.Contains(player.ID)) newRow["S"] = "1";
                playersDataTable.Rows.Add(newRow);

                if (playerClubState == 1)
                    playersCoOwnRowList.Add(player.ID);
                else if (playerClubState == 2)
                    playersLoanRowList.Add(player.ID);
                else if (playerClubState == 3)
                    playersFreeRowList.Add(player.ID);
                if (playerEUState == 1)
                    playersEUMemberRowList.Add(player.ID);

                this.PlayersResultLabel.Text = playersDataTable.Rows.Count + " wonderkid entries found.";
                if (playersPRTotal > 0) this.PlayersResultLabel.Text += " Calculated PR for " + playersPRTotal + " players.";
                this.PlayersResultLabel.Refresh();
            }

            this.PlayersDisplayGridView.ScrollBars = ScrollBars.Both;
            this.PlayersResultLabel.Text = playersDataTable.Rows.Count + " wonderkid entries found.";
            if (playersPRTotal > 0) this.PlayersResultLabel.Text += " Calculated PR for " + playersPRTotal + " players.";
            this.PlayersDisplayGridView.DataSource = playersDataTable;
            DateTime timerEnd = DateTime.Now;
            TimeSpan timer = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
            this.PlayersResultLabel.Text += " Query took " + (float)timer.Seconds + " sec.";
            this.PlayersWonderkidButton.Enabled = true;
            this.PlayersWonderkidButton.Text = "Wonderkids";
        }

        private void StaffSearchButton_Click(object sender, EventArgs e)
        {
            this.CurrentGameDateLabel.Text = "Current Game Date: " + fm.IngameDate.ToLongDateString();

            this.StaffSearchButton.Text = "Searching...";
            this.StaffSearchButton.Enabled = false;
            this.StaffResultLabel.Text += "Searching...";

            staffFreeRowList.Clear();
            staffNationalRowList.Clear();
            staffCRTotal = 0;

            List<string> name_substrings = new List<string>();
            bool empty_name = multiEntryTextBox(ref name_substrings, this.StaffFullNameTextBox.Text);
            List<string> nationality_substrings = new List<string>();
            bool empty_nationality = multiEntryTextBox(ref nationality_substrings, this.StaffNationTextBox.Text);
            List<string> club_substrings = new List<string>();
            bool empty_club = multiEntryTextBox(ref club_substrings, this.StaffClubTextBox.Text);

            // init special attributes
            int[] numericUpDownArray = new int[98];
            initStaffSpecialAttributes(ref numericUpDownArray);

            DateTime tempGameDate = fm.IngameDate;
            if (StaffContractStatusComboBox.SelectedIndex >= 0)
            {
                if (StaffContractStatusComboBox.SelectedItem.Equals("Expiring (6 months)"))
                    tempGameDate = tempGameDate.AddMonths(6);
                else if (StaffContractStatusComboBox.SelectedItem.Equals("Expiring (1 year)"))
                    tempGameDate = tempGameDate.AddYears(1);
            }

            staffDataTable.Clear();
            this.StaffDisplayGridView.DataSource = staffDataTable;
            this.StaffDisplayGridView.ScrollBars = ScrollBars.None;
            int staffClubState = 0;

            DataRow newRow;
            DateTime timerStart = DateTime.Now;
            string staffName = "";
            string staffNickname = "";
            string staffNationality = "";
            string staffRole = "";
            string staffClub = "";
            foreach (Staff staff in fm.NonPlayingStaff)
            {
                // check empty name
                if (staff.FirstName.Length == 0)
                    continue;

                // check name
                if (!empty_name)
                {
                    staffName = staff.ToString().ToLower();
                    staffNickname = staff.Nickname.ToLower();
                    specialCharactersReplacement(ref staffName);
                    specialCharactersReplacement(ref staffNickname);
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
                if (this.StaffRoleComboBox.SelectedIndex > 0)
                {
                    if (!find_staff_role(staff, ref staffRole, false))
                        continue;
                }
                else
                    find_staff_role(staff, ref staffRole, true);

                // check nation
                if (!empty_nationality)
                {
                    staffNationality = staff.Nationality.Name.ToLower();
                    specialCharactersReplacement(ref staffNationality);
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
                staffClubState = 0;
                staffClub = "";
                if (staff.Team.Club.Chairman.ID != staff.ID && staff.Contract.Club.Name.Length != 0)
                    staffClub = staff.Contract.Club.Name;
                else if (staff.NationalTeam.Club.Name.Length != 0)
                {
                    if (nationalTeam.Contains(staff.NationalTeam.Club.Country.Name))
                    {
                        staffClub = staff.NationalTeam.Club.Name;
                        staffClubState = 2;
                    }
                }
                else
                    staffClub = staff.Team.Club.Name;

                if (this.StaffClubTextBox.Text.Length > 0)
                {
                    if (!staffClub.ToLower().Contains(this.StaffClubTextBox.Text.ToString().ToLower()))
                        continue;
                }

                if (staffClub.Length == 0)
                {
                    staffClub += " Free Agent";
                    staffClubState = 1;
                }

                if (!empty_club)
                {
                    specialCharactersReplacement(ref staffClub);
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
                if (StaffRegionComboBox.SelectedIndex > 0)
                {
                    if (!staff.Nationality.Continent.Name.Contains(StaffRegionComboBox.SelectedItem.ToString()))
                        continue;
                }

                // check contract status
                if (StaffContractStatusComboBox.SelectedIndex > 0)
                {
                    if (StaffContractStatusComboBox.SelectedIndex == 3)
                    {
                        if (!staffClub.Equals(" Free Agent"))
                            continue;
                    }
                    else if (staff.Contract.Club.Name.Length > 0)
                    {
                        if (staff.Contract.ContractExpiryDate > tempGameDate)
                            continue;
                    }
                }

                // check regen
                if (StaffRegenComboBox.SelectedIndex == 1)
                {
                    if (staff.ID < 1394640000)
                        continue;
                }
                else if (StaffRegenComboBox.SelectedIndex == 2)
                {
                    if (staff.ID >= 1394640000)
                        continue;
                }


                // special attributes
                if (!testStaffSpecialAttributes(staff, ref numericUpDownArray))
                    continue;

                if (!this.staffCRID.Contains(staff.ID))
                    calculateStaffCR(staff);

                CoachingRatings cr = (CoachingRatings)this.staffCRID[staff.ID];

                // check best coaching rating
                if (this.StaffRatingsFitnessMinNumericUpDown.Value > cr.Fitness || this.StaffRatingsFitnessMaxNumericUpDown.Value < cr.Fitness)
                    continue;
                if (this.StaffRatingsGoalkeepersMinNumericUpDown.Value > cr.Goalkeepers || this.StaffRatingsGoalkeepersMaxNumericUpDown.Value < cr.Goalkeepers)
                    continue;
                if (this.StaffRatingsBallControlMinNumericUpDown.Value > cr.BallControl || this.StaffRatingsBallControlMaxNumericUpDown.Value < cr.BallControl)
                    continue;
                if (this.StaffRatingsTacticsMinNumericUpDown.Value > cr.Tactics || this.StaffRatingsTacticsMaxNumericUpDown.Value < cr.Tactics)
                    continue;
                if (this.StaffRatingsDefendingMinNumericUpDown.Value > cr.Defending || this.StaffRatingsDefendingMaxNumericUpDown.Value < cr.Defending)
                    continue;
                if (this.StaffRatingsAttackingMinNumericUpDown.Value > cr.Attacking || this.StaffRatingsAttackingMaxNumericUpDown.Value < cr.Attacking)
                    continue;
                if (this.StaffRatingsShootingMinNumericUpDown.Value > cr.Shooting || this.StaffRatingsShootingMaxNumericUpDown.Value < cr.Shooting)
                    continue;
                if (this.StaffRatingsSetPiecesMinNumericUpDown.Value > cr.SetPieces || this.StaffRatingsSetPiecesMaxNumericUpDown.Value < cr.SetPieces)
                    continue;

                // check best position
                if (this.StaffBestCRComboBox.SelectedIndex > 0)
                {
                    if (!((CoachingRatings)this.staffCRID[staff.ID]).BestCR.Contains(this.StaffBestCRComboBox.SelectedItem.ToString()))
                        continue;
                }

                newRow = staffDataTable.NewRow();
                addStaffToGrid(staff, ref newRow, ref staffRole, ref staffClub);
                staffDataTable.Rows.Add(newRow);

                if (staffClubState == 1)
                    staffFreeRowList.Add(staff.ID);
                else if (staffClubState == 2)
                    staffNationalRowList.Add(staff.ID);

                this.StaffResultLabel.Text = staffDataTable.Rows.Count + " staff entries found.";
                if (staffCRTotal > 0) this.StaffResultLabel.Text += " Calculated CR for " + staffCRTotal + " staff.";
                this.StaffResultLabel.Refresh();
            }

            this.StaffDisplayGridView.DataSource = staffDataTable;
            this.StaffDisplayGridView.ScrollBars = ScrollBars.Both;
            this.StaffResultLabel.Text = staffDataTable.Rows.Count + " staff entries found.";
            if (staffCRTotal > 0) this.StaffResultLabel.Text += " Calculated CR for " + staffCRTotal + " staff.";
            DateTime timerEnd = DateTime.Now;
            TimeSpan timer = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
            this.StaffResultLabel.Text += " Query took " + (float)timer.Seconds + " sec.";
            this.StaffSearchButton.Enabled = true;
            this.StaffSearchButton.Text = "Search";
        }

        private void StaffWonderStaffButton_Click(object sender, EventArgs e)
        {
            this.CurrentGameDateLabel.Text = "Current Game Date: " + fm.IngameDate.ToLongDateString();

            this.StaffWonderStaffButton.Text = "Searching...";
            this.StaffWonderStaffButton.Enabled = false;
            this.StaffResultLabel.Text += "Searching...";

            staffFreeRowList.Clear();
            staffNationalRowList.Clear();
            staffCRTotal = 0;

            staffDataTable.Clear();
            this.StaffDisplayGridView.DataSource = staffDataTable;
            this.StaffDisplayGridView.ScrollBars = ScrollBars.None;
            int staffClubState = 0;

            DataRow newRow;
            DateTime timerStart = DateTime.Now;
            string staffClub = "";
            foreach (Staff staff in fm.NonPlayingStaff)
            {
                // check empty name
                if (staff.FirstName.Length == 0)
                    continue;

                // check PA
                if (staff.PotentialCoachingAbility < this.preferencesForm.wonderstaffMinPA)
                    continue;

                // check club
                staffClubState = 0;
                staffClub = "";
                if (staff.Team.Club.Chairman.ID != staff.ID && staff.Contract.Club.Name.Length != 0)
                    staffClub = staff.Contract.Club.Name;
                else if (staff.NationalTeam.Club.Name.Length != 0)
                {
                    if (nationalTeam.Contains(staff.NationalTeam.Club.Country.Name))
                    {
                        staffClub = staff.NationalTeam.Club.Name;
                        staffClubState = 2;
                    }
                }
                else
                    staffClub = staff.Team.Club.Name;
                if (staffClub.Length == 0)
                {
                    staffClub += " Free Agent";
                    staffClubState = 1;
                }

                // check position
                string staffRole = "";
                find_staff_role(staff, ref staffRole, true);

                newRow = staffDataTable.NewRow();
                addStaffToGrid(staff, ref newRow, ref staffRole, ref staffClub);
                staffDataTable.Rows.Add(newRow);

                if (staffClubState == 1)
                    staffFreeRowList.Add(staff.ID);
                else if (staffClubState == 2)
                    staffNationalRowList.Add(staff.ID);

                this.StaffResultLabel.Text = staffDataTable.Rows.Count + " wonderstaff entries found.";
                if (staffCRTotal > 0) this.StaffResultLabel.Text += " Calculated CR for " + staffCRTotal + " staff.";
                this.StaffResultLabel.Refresh();
            }

            this.StaffDisplayGridView.DataSource = staffDataTable;
            this.StaffDisplayGridView.ScrollBars = ScrollBars.Both;
            this.StaffResultLabel.Text = staffDataTable.Rows.Count + " wonderstaff entries found.";
            if (staffCRTotal > 0) this.StaffResultLabel.Text += " Calculated CR for " + staffCRTotal + " staff.";
            DateTime timerEnd = DateTime.Now;
            TimeSpan timer = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
            this.StaffResultLabel.Text += " Query took " + (float)timer.Seconds + " sec.";
            this.StaffWonderStaffButton.Enabled = true;
            this.StaffWonderStaffButton.Text = "WonderStaff";
        }

        private void TeamsSearchButton_Click(object sender, EventArgs e)
        {
            this.CurrentGameDateLabel.Text = "Current Game Date: " + fm.IngameDate.ToLongDateString();

            this.TeamsSearchButton.Text = "Searching...";
            this.TeamsSearchButton.Enabled = false;
            this.TeamsResultLabel.Text += "Searching...";

            teamNationalRowList.Clear();

            bool isNational = false;
            List<string> name_substrings = new List<string>();
            bool empty_name = multiEntryTextBox(ref name_substrings, this.TeamNameTextBox.Text);
            List<string> nationality_substrings = new List<string>();
            bool empty_nationality = multiEntryTextBox(ref nationality_substrings, this.TeamNationalityTextBox.Text);
            List<string> stadium_substrings = new List<string>();
            bool empty_stadium = multiEntryTextBox(ref stadium_substrings, this.TeamStadiumNameTextBox.Text);

            teamsDataTable.Clear();
            this.TeamsDisplayGridView.DataSource = teamsDataTable;
            this.TeamsDisplayGridView.ScrollBars = ScrollBars.None;
            int teamClubState = 0;

            DataRow newRow;
            DateTime timerStart = DateTime.Now;
            Club club = null;
            string clubName = "";
            string clubNationality = "";
            string clubStadium = "";
            foreach (DictionaryEntry entry in allClubs)
            {
                club = (Club)entry.Value;
                // check empty name
                if (club.Name.Length == 0 || club.Country.Name.Length == 0)
                    continue;

                // check name
                if (!empty_name)
                {
                    clubName = club.Name.ToLower();
                    specialCharactersReplacement(ref clubName);
                    int no_of_successes = 0;
                    foreach (string str in name_substrings)
                    {
                        if (clubName.ToLower().Contains(str))
                            ++no_of_successes;
                    }

                    if (no_of_successes != name_substrings.Count)
                        continue;
                }

                // check nationality
                if (club.Country.Name.Length == 0)
                {
                    if (!empty_nationality)
                    {
                        clubNationality = club.Name.ToLower();
                        specialCharactersReplacement(ref clubNationality);
                        int no_of_successes = 0;
                        foreach (string str in nationality_substrings)
                        {
                            if (clubNationality.Contains(str))
                                ++no_of_successes;
                        }

                        if (no_of_successes != nationality_substrings.Count)
                            continue;
                    }
                }
                else
                {
                    if (!empty_nationality)
                    {
                        clubNationality = club.Country.Name.ToLower();
                        specialCharactersReplacement(ref clubNationality);
                        int no_of_successes = 0;
                        foreach (string str in nationality_substrings)
                        {
                            if (clubNationality.Contains(str))
                                ++no_of_successes;
                        }

                        if (no_of_successes != nationality_substrings.Count)
                            continue;
                    }
                }

                isNational = false;
                teamClubState = 0;
                if (nationalTeam.Contains(club.Country.Name))
                {
                    isNational = true;
                    teamClubState = 1;
                }

                // check type
                if (this.TeamTypeComboBox.SelectedIndex > 0)
                {
                    if (this.TeamTypeComboBox.SelectedIndex == 1 && !isNational)
                        continue;
                    else if (this.TeamTypeComboBox.SelectedIndex == 2 && isNational)
                        continue;
                }

                int curTeam = 0;
                for (curTeam = 0; curTeam < club.Teams.Count; ++curTeam)
                {
                    if (club.Teams[curTeam].Type == TeamType.First)
                        break;
                    else if (club.Teams[curTeam].Type == TeamType.Amateur)
                        break;
                    //else if (club.Teams[curTeam].Type == TeamType.Empty)
                    //  break;
                }

                if (curTeam == club.Teams.Count) --curTeam;

                // check reputation
                if (this.TeamReputationComboBox.SelectedIndex > 0)
                {
                    if (this.TeamReputationComboBox.SelectedIndex == 1 && club.Teams[curTeam].Prestige < 8000)
                        continue;
                    else if (this.TeamReputationComboBox.SelectedIndex == 2 && (!(club.Teams[curTeam].Prestige > 5000 && club.Teams[curTeam].Prestige <= 8000)))
                        continue;
                    else if (this.TeamReputationComboBox.SelectedIndex == 3 && (!(club.Teams[curTeam].Prestige > 3000 && club.Teams[curTeam].Prestige <= 5000)))
                        continue;
                    else if (this.TeamReputationComboBox.SelectedIndex == 4 && (!(club.Teams[curTeam].Prestige > 0 && club.Teams[curTeam].Prestige <= 3000)))
                        continue;
                }

                // check region
                if (TeamRegionComboBox.SelectedIndex > 0)
                {
                    if (!isNational)
                    {
                        if (!club.Country.Continent.Name.Contains(TeamRegionComboBox.SelectedItem.ToString()))
                            continue;
                    }
                    else
                    {
                        if (!club.Country.Name.Contains(TeamRegionComboBox.SelectedItem.ToString()))
                            continue;
                    }
                }

                // check stadium
                if (!empty_stadium)
                {
                    clubStadium = club.Teams[curTeam].Stadium.Name.ToLower();
                    specialCharactersReplacement(ref clubStadium);
                    int no_of_successes = 0;
                    foreach (string str in stadium_substrings)
                    {
                        if (clubStadium.Contains(str))
                            ++no_of_successes;
                    }

                    if (no_of_successes != stadium_substrings.Count)
                        continue;
                }

                // check transfer budget
                if (club.Finances.SeasonTransferBudget < this.TeamTransferBudgetMinNumericUpDown.Value
                || club.Finances.SeasonTransferBudget > this.TeamTransferBudgetMaxNumericUpDown.Value)
                    continue;

                if (club.Finances.WageBudget < this.TeamWageBudgetMinNumericUpDown.Value
                || club.Finances.WageBudget > this.TeamWageBudgetMaxNumericUpDown.Value)
                    continue;

                newRow = teamsDataTable.NewRow();
                addTeamToGrid(club, ref newRow, curTeam);
                teamsDataTable.Rows.Add(newRow);

                if (teamClubState == 1)
                    teamNationalRowList.Add(club.Teams[curTeam].Club.ID);

                this.TeamsResultLabel.Text = teamsDataTable.Rows.Count + " team entries found.";
                this.TeamsResultLabel.Refresh();
            }

            this.TeamsDisplayGridView.DataSource = teamsDataTable;
            this.TeamsDisplayGridView.ScrollBars = ScrollBars.Both;
            this.TeamsResultLabel.Text = teamsDataTable.Rows.Count + " team entries found.";
            DateTime timerEnd = DateTime.Now;
            TimeSpan timer = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
            this.TeamsResultLabel.Text += " Query took " + (float)timer.Seconds + " sec.";
            this.TeamsSearchButton.Enabled = true;
            this.TeamsSearchButton.Text = "Search";
        }

        private void TeamsWonderTeamsButton_Click(object sender, EventArgs e)
        {
            this.CurrentGameDateLabel.Text = "Current Game Date: " + fm.IngameDate.ToLongDateString();

            this.TeamsWonderTeamsButton.Text = "Searching...";
            this.TeamsWonderTeamsButton.Enabled = false;
            this.TeamsResultLabel.Text = "Searching...";

            teamNationalRowList.Clear();

            teamsDataTable.Clear();
            this.TeamsDisplayGridView.DataSource = teamsDataTable;
            this.TeamsDisplayGridView.ScrollBars = ScrollBars.None;
            int teamClubState = 0;

            DataRow newRow;
            DateTime timerStart = DateTime.Now;
            foreach (DictionaryEntry entry in allClubs)
            {
                Club club = (Club)entry.Value;
                // check empty name
                if (club.Name.Length == 0)
                    continue;

                int curTeam = 0;
                for (curTeam = 0; curTeam < club.Teams.Count; ++curTeam)
                {
                    if (club.Teams[curTeam].Type == TeamType.First)
                        break;
                    else if (club.Teams[curTeam].Type == TeamType.Amateur)
                        break;
                    //else if (club.Teams[curTeam].Type == TeamType.Empty)
                    //  break;
                }

                if (curTeam == club.Teams.Count) --curTeam;

                teamClubState = 0;
                if (nationalTeam.Contains(club.Country.Name))
                    teamClubState = 1;

                // check reputation
                if (club.Teams[curTeam].Prestige <= this.preferencesForm.wonderteamsMinRep)
                    continue;

                newRow = teamsDataTable.NewRow();
                addTeamToGrid(club, ref newRow, curTeam);
                teamsDataTable.Rows.Add(newRow);

                if (teamClubState == 1)
                    teamNationalRowList.Add(club.Teams[curTeam].Club.ID);

                this.TeamsResultLabel.Text = teamsDataTable.Rows.Count + " wonderteams entries found.";
                this.TeamsResultLabel.Refresh();
            }

            this.TeamsDisplayGridView.DataSource = teamsDataTable;
            this.TeamsDisplayGridView.ScrollBars = ScrollBars.Both;
            this.TeamsResultLabel.Text = teamsDataTable.Rows.Count + " wonderteams entries found.";
            DateTime timerEnd = DateTime.Now;
            TimeSpan timer = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
            this.TeamsResultLabel.Text += " Query took " + (float)timer.Seconds + " sec.";
            this.TeamsWonderTeamsButton.Enabled = true;
            this.TeamsWonderTeamsButton.Text = "WonderTeams";
        }

        private void TeamProfileListPlayersButton_Click(object sender, EventArgs e)
        {
            if (this.TeamProfileIDTextBox.Text.Equals(""))
                return;

            int id = Int32.Parse(this.TeamProfileIDTextBox.Text);
            string isNational = this.TeamProfileNationalTextBox.Text;
            string teamNationality = this.TeamProfileNameTextBox.Text;

            this.TeamProfileListPlayersButton.Text = "Listing...";
            this.TeamProfileListPlayersButton.Enabled = false;
            playersLoanRowList.Clear();
            playersCoOwnRowList.Clear();
            playersFreeRowList.Clear();
            playersEUMemberRowList.Clear();
            playersPRTotal = 0;

            List<string> player_positions = new List<string>();
            List<string> player_sides = new List<string>();

            // init special attributes
            int[] numericUpDownArray = new int[MaxPlayerAttributes];
            initPlayersSpecialAttributes(ref numericUpDownArray);

            playersDataTable.Clear();
            this.PlayersDisplayGridView.DataSource = playersDataTable;
            this.PlayersDisplayGridView.ScrollBars = ScrollBars.None;
            int playerClubState = 0;
            int playerEUState = 0;

            DataRow newRow;
            DateTime timerStart = DateTime.Now;
            string listing = this.TeamProfileListPlayersComboBox.SelectedItem.ToString();
            if (!listing.Equals("All Squads"))
                listing = listing.Substring(0, listing.Length - 5);
            else listing = "";

            Contract contract = null;
            string playerPosition = "";
            string playerClub = "";
            string playerNationality = "";
            foreach (Player player in fm.Players)
            {
                // check empty name
                if (player.FirstName.Length == 0)
                    continue;

                if (isNational.Equals("Yes"))
                {
                    if (!player.Nationality.Name.Equals(teamNationality))
                        continue;
                }
                else if (isNational.Equals("No"))
                {
                    if (player.LoanContract != null)
                    {
                        if (!player.LoanContract.Club.ID.Equals(id))
                            continue;
                        contract = player.LoanContract;
                    }
                    else if (player.Contract != null)
                    {
                        if (!player.Contract.Club.ID.Equals(id))
                            continue;
                        contract = player.Contract;
                    }

                    bool foundType = false;
                    if (!listing.Equals(""))
                    {
                        for (int i = 0; i < contract.Club.Teams.Count; ++i)
                        {
                            if (contract.Club.Teams[i].Type.ToString().Equals(listing))
                            {
                                foundType = true;
                                break;
                            }
                        }

                        if (!foundType)
                            continue;
                    }
                }

                playerPosition = "";
                find_player_position(player, ref playerPosition, ref player_positions, ref player_sides, true);

                playerClub = "";
                if (player.LoanContract != null)
                {
                    playerClub = player.Contract.Club.Name;
                    playerClubState = 2;
                }
                else if (player.CoContract != null)
                {
                    playerClub = player.Contract.Club.Name + "/" + player.CoContract.Club.Name;
                    playerClubState = 1;
                }
                else
                {
                    playerClub = player.Contract.Club.Name;
                    playerClubState = 0;
                }

                if (playerClub.Length == 0)
                {
                    playerClub = " Free Player";
                    playerClubState = 3;
                }

                playerEUState = 0;
                playerNationality = player.Nationality.Name;
                if (EUcountries.Contains(player.Nationality.Name)) playerEUState = 1;
                // other nationalities
                for (int playerRelationIndex = 0; playerRelationIndex < player.RelationsTotal; ++playerRelationIndex)
                {
                    if (player.Relations[playerRelationIndex].ObjectType == RelationObjectType.Country &&
                        player.Relations[playerRelationIndex].RelationType == RelationType.OtherNationality)
                    {
                        playerNationality += "/" + player.Relations[playerRelationIndex].Country.Name;
                        if (EUcountries.Contains(player.Relations[playerRelationIndex].Country.Name)) playerEUState = 1;
                        break;
                    }
                }

                newRow = playersDataTable.NewRow();
                addPlayerToGrid(player, ref newRow, ref playerPosition, ref playerClub, ref playerNationality);
                playersDataTable.Rows.Add(newRow);

                if (playerClubState == 1)
                    playersCoOwnRowList.Add(player.ID);
                else if (playerClubState == 2)
                    playersLoanRowList.Add(player.ID);
                else if (playerClubState == 3)
                    playersFreeRowList.Add(player.ID);
                if (playerEUState == 1)
                    playersEUMemberRowList.Add(player.ID);

                this.PlayersResultLabel.Text = playersDataTable.Rows.Count + " player entries found.";
                if (playersPRTotal > 0) this.PlayersResultLabel.Text += " Calculated PR for " + playersPRTotal + " players.";
                this.PlayersResultLabel.Refresh();
            }

            this.PlayerClubTextBox.Text = playerClub;
            this.PlayersDisplayGridView.ScrollBars = ScrollBars.Both;
            this.PlayersResultLabel.Text = playersDataTable.Rows.Count + " player entries found.";
            if (playersPRTotal > 0) this.PlayersResultLabel.Text += " Calculated PR for " + playersPRTotal + " players.";
            this.PlayersDisplayGridView.DataSource = playersDataTable;
            DateTime timerEnd = DateTime.Now;
            TimeSpan timer = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
            this.PlayersResultLabel.Text += " Query took " + (float)timer.Seconds + " sec.";
            this.TeamProfileListPlayersButton.Enabled = true;
            this.TeamProfileListPlayersButton.Text = "List Players";
            this.StaffTabControl.Visible = false;
            this.TeamsTabControl.Visible = false;
            this.PlayersTabControl.Visible = true;
            this.PlayersTabControl.SelectedTab = this.PlayersTabPage;
            this.treeView.SelectedNode = this.treeView.Nodes["PlayersNode"].Nodes["SearchPlayersNode"];
        }

        private void TeamProfileListStaffButton_Click(object sender, EventArgs e)
        {
            if (this.TeamProfileIDTextBox.Text.Equals(""))
                return;

            int id = Int32.Parse(this.TeamProfileIDTextBox.Text);

            this.TeamProfileListStaffButton.Text = "Listing...";
            this.TeamProfileListStaffButton.Enabled = false;
            staffFreeRowList.Clear();
            staffCRTotal = 0;

            List<string> staff_role = new List<string>();

            // init special attributes
            int[] numericUpDownArray = new int[MaxStaffAttributes];
            initStaffSpecialAttributes(ref numericUpDownArray);

            staffDataTable.Clear();
            this.StaffDisplayGridView.DataSource = staffDataTable;
            this.StaffDisplayGridView.ScrollBars = ScrollBars.None;
            int staffClubState = 0;

            DataRow newRow;
            DateTime timerStart = DateTime.Now;
            string staffRole = "";
            string staffClub = "";
            foreach (Staff staff in fm.NonPlayingStaff)
            {
                // check empty name
                if (staff.FirstName.Length == 0)
                    continue;

                if (!staff.Contract.Club.ID.Equals(id))
                    continue;

                staffRole = "";
                find_staff_role(staff, ref staffRole, true);

                staffClub = staff.Contract.Club.Name;
                staffClubState = 0;
                if (staffClub.Length == 0)
                {
                    staffClub = " Free Agent";
                    staffClubState = 1;
                }

                newRow = staffDataTable.NewRow();
                addStaffToGrid(staff, ref newRow, ref staffRole, ref staffClub);
                staffDataTable.Rows.Add(newRow);

                if (staffClubState == 1)
                    playersCoOwnRowList.Add(staff.ID);

                this.StaffResultLabel.Text = staffDataTable.Rows.Count + " staff entries found.";
                if (staffCRTotal > 0) this.StaffResultLabel.Text += " Calculated CR for " + staffCRTotal + " staff.";
                this.StaffResultLabel.Refresh();
            }

            this.StaffClubTextBox.Text = staffClub;
            this.StaffDisplayGridView.ScrollBars = ScrollBars.Both;
            this.StaffResultLabel.Text = staffDataTable.Rows.Count + " staff entries found.";
            if (staffCRTotal > 0) this.StaffResultLabel.Text += " Calculated CR for " + staffCRTotal + " staff.";
            this.StaffDisplayGridView.DataSource = staffDataTable;
            DateTime timerEnd = DateTime.Now;
            TimeSpan timer = new TimeSpan(timerEnd.Ticks - timerStart.Ticks);
            this.StaffResultLabel.Text += " Query took " + (float)timer.Seconds + " sec.";
            this.TeamProfileListStaffButton.Enabled = true;
            this.TeamProfileListStaffButton.Text = "List Staff";
            this.PlayersTabControl.Visible = false;
            this.TeamsTabControl.Visible = false;
            this.StaffTabControl.Visible = true;
            this.StaffTabControl.SelectedTab = this.StaffTabPage;
            this.treeView.SelectedNode = this.treeView.Nodes["StaffNode"].Nodes["SearchStaffNode"];
        }

        public void addPlayerToGrid(Player player, ref DataRow newRow, ref string playerPosition, ref string playerClub, ref string playerNationality)
        {
            newRow["ID"] = player.ID;
            if (!player.Nickname.Equals(""))
                newRow["Full Name"] = player.Nickname.ToString();
            else
                newRow["Full Name"] = player.ToString();
            newRow["Nation"] = playerNationality;
            newRow["Club"] = playerClub;
            newRow["Team Squad"] = player.Team.Type.ToString();
            newRow["Position"] = playerPosition;
            newRow["Age"] = player.Age;
            newRow["CA"] = player.CurrentPlayingAbility;
            newRow["PA"] = player.PotentialPlayingAbility;
            newRow["ADiff"] = player.PotentialPlayingAbility - player.CurrentPlayingAbility;
            if (!this.playersPRID.Contains(player.ID))
                calculatePlayerPR(player);
            PositionalRatings pr = (PositionalRatings)this.playersPRID[player.ID];
            newRow["Best PR"] = pr.bestPos;
            newRow["Best PR%"] = pr.bestPosR * 0.01f;
            newRow["Current Value"] = player.Value * this.preferencesForm.Currency;
            newRow["Sale Value"] = player.SaleValue * this.preferencesForm.Currency;
            if (!playerClub.Equals(" Free Player"))
            {
                newRow["Contract Started"] = player.Contract.ContractStarted.Date;
                newRow["Contract Expiring"] = player.Contract.ContractExpiryDate.Date;
            }
            int playerWage = 0;
            if (player.LoanContract != null)
                playerWage = player.LoanContract.WagePerWeek;
            else if (player.CoContract != null)
            {
                if (player.Contract.Club.Name.Equals(player.Team.Club.Name))
                    playerWage = player.Contract.WagePerWeek;
                else
                    playerWage = player.CoContract.WagePerWeek;
            }
            else
                playerWage = player.Contract.WagePerWeek;

            newRow["Current Wage"] = playerWage * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency;
            newRow["World Reputation"] = player.InternationalPrestige;
            newRow["National Reputation"] = player.NationalPrestige;
            newRow["Local Reputation"] = player.CurrentPrestige;
            newRow["Corners"] = player.TechnicalSkills.Corners * 0.2f;
            newRow["Crossing"] = player.TechnicalSkills.Crossing * 0.2f;
            newRow["Dribbling"] = player.TechnicalSkills.Dribbling * 0.2f;
            newRow["Finishing"] = player.TechnicalSkills.Finishing * 0.2f;
            newRow["First Touch"] = player.TechnicalSkills.FirstTouch * 0.2f;
            newRow["Free Kicks"] = player.TechnicalSkills.Freekicks * 0.2f;
            newRow["Heading"] = player.TechnicalSkills.Heading * 0.2f;
            newRow["Long Shots"] = player.TechnicalSkills.LongShots * 0.2f;
            newRow["Long Throws"] = player.TechnicalSkills.Longthrows * 0.2f;
            newRow["Marking"] = player.TechnicalSkills.Marking * 0.2f;
            newRow["Passing"] = player.TechnicalSkills.Passing * 0.2f;
            newRow["Penalty Taking"] = player.TechnicalSkills.PenaltyTaking * 0.2f;
            newRow["Tackling"] = player.TechnicalSkills.Tackling * 0.2f;
            newRow["Technique"] = player.TechnicalSkills.Technique * 0.2f;
            newRow["Acceleration"] = player.PhysicalSkills.Acceleration * 0.2f;
            newRow["Agility"] = player.PhysicalSkills.Agility * 0.2f;
            newRow["Balance"] = player.PhysicalSkills.Balance * 0.2f;
            newRow["Jumping"] = player.PhysicalSkills.Jumping * 0.2f;
            newRow["Natural Fitness"] = player.PhysicalSkills.NaturalFitness * 0.2f;
            newRow["Pace"] = player.PhysicalSkills.Pace * 0.2f;
            newRow["Stamina"] = player.PhysicalSkills.Stamina * 0.2f;
            newRow["Strength"] = player.PhysicalSkills.Strength * 0.2f;
            newRow["Left Foot"] = player.PhysicalSkills.LeftFoot * 0.2f;
            newRow["Right Foot"] = player.PhysicalSkills.RightFoot * 0.2f;
            newRow["Aggression"] = player.MentalSkills.Aggression * 0.2f;
            newRow["Anticipation"] = player.MentalSkills.Anticipation * 0.2f;
            newRow["Bravery"] = player.MentalSkills.Bravery * 0.2f;
            newRow["Composure"] = player.MentalSkills.Composure * 0.2f;
            newRow["Creativity"] = player.MentalSkills.Creativity * 0.2f;
            newRow["Concentration"] = player.MentalSkills.Concentration * 0.2f;
            newRow["Decisions"] = player.MentalSkills.Decisions * 0.2f;
            newRow["Determination"] = player.MentalSkills.Determination * 0.2f;
            newRow["Flair"] = player.MentalSkills.Flair * 0.2f;
            newRow["Influence"] = player.MentalSkills.Influence * 0.2f;
            newRow["Off The Ball"] = player.MentalSkills.OffTheBall * 0.2f;
            newRow["Positioning"] = player.MentalSkills.Positioning * 0.2f;
            newRow["Team Work"] = player.MentalSkills.Teamwork * 0.2f;
            newRow["Work Rate"] = player.MentalSkills.Workrate * 0.2f;
            newRow["Consistency"] = player.HiddenSkills.Consistency * 0.2f;
            newRow["Dirtyness"] = player.HiddenSkills.Dirtyness * 0.2f;
            newRow["Important Matches"] = player.HiddenSkills.ImportantMatches * 0.2f;
            newRow["Injury Proneness"] = player.HiddenSkills.InjuryProness * 0.2f;
            newRow["Versatility"] = player.HiddenSkills.Versatility * 0.2f;
            newRow["Aerial Ability"] = player.GoalKeepingSkills.AerialAbility * 0.2f;
            newRow["Command Of Area"] = player.GoalKeepingSkills.CommandOfArea * 0.2f;
            newRow["Communication"] = player.GoalKeepingSkills.Communication * 0.2f;
            newRow["Eccentricity"] = player.GoalKeepingSkills.Eccentricity * 0.2f;
            newRow["Handling"] = player.GoalKeepingSkills.Handling * 0.2f;
            newRow["Kicking"] = player.GoalKeepingSkills.Kicking * 0.2f;
            newRow["One On Ones"] = player.GoalKeepingSkills.OneOnOnes * 0.2f;
            newRow["Reflexes"] = player.GoalKeepingSkills.Reflexes * 0.2f;
            newRow["Rushing Out"] = player.GoalKeepingSkills.RushingOut * 0.2f;
            newRow["Tendency To Punch"] = player.GoalKeepingSkills.TendencyToPunch * 0.2f;
            newRow["Throwing"] = player.GoalKeepingSkills.Throwing * 0.2f;
            newRow["Adaptability"] = player.MentalTraitsSkills.Adaptability;
            newRow["Ambition"] = player.MentalTraitsSkills.Ambition;
            newRow["Controversy"] = player.MentalTraitsSkills.Controversy;
            newRow["Loyalty"] = player.MentalTraitsSkills.Loyalty;
            newRow["Pressure"] = player.MentalTraitsSkills.Pressure;
            newRow["Professionalism"] = player.MentalTraitsSkills.Professionalism;
            newRow["Sportsmanship"] = player.MentalTraitsSkills.Sportsmanship;
            newRow["Temperament"] = player.MentalTraitsSkills.Temperament;
        }

        public void addStaffToGrid(Staff staff, ref DataRow newRow, ref string staffRole, ref string staffClub)
        {
            newRow["ID"] = staff.ID;
            newRow["Full Name"] = staff.ToString();
            newRow["Nation"] = staff.Nationality.Name;
            newRow["Club"] = staffClub;
            newRow["Role"] = staffRole;
            newRow["Age"] = staff.Age;
            newRow["CA"] = staff.CurrentCoachingAbility;
            newRow["PA"] = staff.PotentialCoachingAbility;
            newRow["ADiff"] = staff.CurrentCoachingAbility - staff.PotentialCoachingAbility;
            if (!this.staffCRID.Contains(staff.ID))
                calculateStaffCR(staff);
            CoachingRatings cr = (CoachingRatings)this.staffCRID[staff.ID];
            newRow["Best CR"] = cr.BestCR;
            newRow["Best CR Stars"] = cr.BestCRStars;
            if (staff.Contract.Club.Name.Length > 0)
            {
                newRow["Contract Started"] = staff.Contract.ContractStarted.Date;
                newRow["Contract Expiring"] = staff.Contract.ContractExpiryDate.Date;
            }
            newRow["Current Wage"] = staff.Contract.WagePerWeek * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency;
            newRow["World Reputation"] = staff.InternationalPrestige;
            newRow["National Reputation"] = staff.NationalPrestige;
            newRow["Local Reputation"] = staff.CurrentPrestige;
            newRow["Attacking"] = staff.CoachingSkills.CoachingAttacking * 0.2f;
            newRow["Defending"] = staff.CoachingSkills.CoachingDefending * 0.2f;
            newRow["Fitness"] = staff.CoachingSkills.CoachingFitness * 0.2f;
            newRow["Goalkeepers"] = staff.CoachingSkills.CoachingGoalkeepers * 0.2f;
            newRow["Mental"] = staff.CoachingSkills.CoachingMental * 0.2f;
            newRow["Player"] = staff.CoachingSkills.CoachingPlayer * 0.2f;
            newRow["Tactical"] = staff.CoachingSkills.CoachingTactical * 0.2f;
            newRow["Technical"] = staff.CoachingSkills.CoachingTechnical * 0.2f;
            newRow["Man Management"] = staff.CoachingSkills.ManManagement * 0.2f;
            newRow["Working With Youngsters"] = staff.CoachingSkills.WorkingWithYoungsters;
            newRow["Adaptability"] = staff.MentalSkills.Adaptability;
            newRow["Ambition"] = staff.StaffMentalTraitsSkills.Ambition;
            newRow["Controversy"] = staff.StaffMentalTraitsSkills.Controversy;
            newRow["Determination"] = staff.StaffMentalTraitsSkills.Determination * 0.2f;
            newRow["Loyalty"] = staff.StaffMentalTraitsSkills.Loyalty;
            newRow["Pressure"] = staff.StaffMentalTraitsSkills.Pressure;
            newRow["Professionalism"] = staff.StaffMentalTraitsSkills.Professionalism;
            newRow["Sportsmanship"] = staff.StaffMentalTraitsSkills.Sportsmanship;
            newRow["Temperament"] = staff.StaffMentalTraitsSkills.Temperament;
            newRow["Judging Player Ability"] = staff.MentalSkills.JudgingPlayerAbility * 0.2f;
            newRow["Judging Player Potential"] = staff.MentalSkills.JudgingPlayerPotential * 0.2f;
            newRow["Level Of Discipline"] = staff.MentalSkills.LevelOfDiscipline;
            newRow["Motivating"] = staff.MentalSkills.Motivating * 0.2f;
            newRow["Physiotherapy"] = staff.MentalSkills.Physiotherapy * 0.2f;
            newRow["Tactical Knowledge"] = staff.MentalSkills.TacticalKnowledge * 0.2f;
            newRow["Depth"] = staff.TacticalSkills.Depth;
            newRow["Directness"] = staff.TacticalSkills.Directness;
            newRow["Flamboyancy"] = staff.TacticalSkills.Flamboyancy;
            newRow["Flexibility"] = staff.TacticalSkills.Flexibility;
            newRow["Free Roles"] = staff.TacticalSkills.FreeRoles;
            newRow["Marking"] = staff.TacticalSkills.Marking;
            newRow["Offside"] = staff.TacticalSkills.OffSide;
            newRow["Pressing"] = staff.TacticalSkills.Pressing;
            newRow["Sitting Back"] = staff.TacticalSkills.SittingBack;
            newRow["Tempo"] = staff.TacticalSkills.Tempo;
            newRow["Use Of Playmaker"] = staff.TacticalSkills.UseOfPlaymaker;
            newRow["Use Of Substitutions"] = staff.TacticalSkills.UseOfSubstitutions;
            newRow["Width"] = staff.TacticalSkills.Width;
            newRow["Buying Players"] = staff.NonTacticalSkills.BuyingPlayers;
            newRow["Hardness Of Training"] = staff.NonTacticalSkills.HardnessOfTraining;
            newRow["Mind Games"] = staff.NonTacticalSkills.MindGames;
            newRow["Squad Rotation"] = staff.NonTacticalSkills.SquadRotation;
            newRow["Business"] = staff.ChairmanSkills.Business;
            newRow["Interference"] = staff.ChairmanSkills.Interference;
            newRow["Patience"] = staff.ChairmanSkills.Patience;
            newRow["Resources"] = staff.ChairmanSkills.Resources;
        }

        public void addTeamToGrid(Club club, ref DataRow newRow, int curTeam)
        {
            newRow["ID"] = club.ID;
            newRow["Name"] = club.Name.ToString();
            if (club.Country.Name.Length == 0)
                newRow["Nation"] = club.Name;
            else
                newRow["Nation"] = club.Country.Name;
            newRow["Reputation"] = club.Teams[curTeam].Prestige;
            newRow["Status"] = club.Status.ToString();
            if (club.Teams[curTeam].Stadium.Name.Length != 0)
                newRow["Stadium"] = club.Teams[curTeam].Stadium.Name;
            else
                newRow["Stadium"] = " None";
            if (club.Finances.RemainingTransferBudget != 27571580 && club.Finances.RemainingTransferBudget != 27727004)
            {
                newRow["Transfer Budget"] = club.Finances.SeasonTransferBudget * this.preferencesForm.Currency;
                newRow["Remaining Budget"] = club.Finances.RemainingTransferBudget * this.preferencesForm.Currency;
                newRow["Wage Budget"] = club.Finances.WageBudget * this.preferencesForm.Currency;
                newRow["Wage Used"] = club.Finances.UsedWage * this.preferencesForm.Currency;
            }
            else
            {
                newRow["Transfer Budget"] = 0 * this.preferencesForm.Currency;
                newRow["Remaining Budget"] = 0 * this.preferencesForm.Currency;
                newRow["Wage Budget"] = 0 * this.preferencesForm.Currency;
                newRow["Wage Used"] = 0 * this.preferencesForm.Currency;
            }
            newRow["Budget Balance"] = club.Finances.Balance * this.preferencesForm.Currency;
            newRow["Transfer Revenue Available"] = club.Finances.TransferRevenueMadeAvailable * 0.01f;
            newRow["Current Affiliated Clubs"] = club.TeamNumber;
            newRow["Max Affiliated Clubs"] = club.MaxAffiliatedClubNumber;
            newRow["Training Ground"] = club.TrainingGround;
            newRow["Maximum Attendance"] = club.MaximumAttendance;
            newRow["Average Attendance"] = club.AverageAttendance;
            newRow["Minimum Attendance"] = club.MinimumAttendance;
            newRow["Decay"] = club.Teams[curTeam].Stadium.Decay;
            newRow["Field Width"] = club.Teams[curTeam].Stadium.FieldWidth;
            newRow["Field Length"] = club.Teams[curTeam].Stadium.FieldLength;
            newRow["Current Capacity"] = club.Teams[curTeam].Stadium.StadiumCapacity;
            newRow["Seating Capacity"] = club.Teams[curTeam].Stadium.SeatingCapacity;
            newRow["Expansion Capacity"] = club.Teams[curTeam].Stadium.ExpansionCapacity;
            newRow["Used Capacity"] = club.Teams[curTeam].Stadium.UsedCapacity;
        }

        public void initPlayersDataTableColumns(ref List<int> playersColumnsWidth)
        {
            playersDataColumnList.Add(new DataColumn("S", typeof(int)));
            playersDataColumnList.Add(new DataColumn("ID", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Full Name", typeof(string)));
            playersDataColumnList.Add(new DataColumn("Nation", typeof(string)));
            playersDataColumnList.Add(new DataColumn("Club", typeof(string)));
            playersDataColumnList.Add(new DataColumn("Team Squad", typeof(string)));
            playersDataColumnList.Add(new DataColumn("Position", typeof(string)));
            playersDataColumnList.Add(new DataColumn("Age", typeof(int)));
            playersDataColumnList.Add(new DataColumn("CA", typeof(int)));
            playersDataColumnList.Add(new DataColumn("PA", typeof(int)));
            playersDataColumnList.Add(new DataColumn("ADiff", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Best PR", typeof(string)));
            playersDataColumnList.Add(new DataColumn("Best PR%", typeof(double)));
            playersDataColumnList.Add(new DataColumn("Current Value", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Sale Value", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Contract Started", typeof(DateTime)));
            playersDataColumnList.Add(new DataColumn("Contract Expiring", typeof(DateTime)));
            playersDataColumnList.Add(new DataColumn("Current Wage", typeof(int)));
            playersDataColumnList.Add(new DataColumn("World Reputation", typeof(int)));
            playersDataColumnList.Add(new DataColumn("National Reputation", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Local Reputation", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Corners", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Crossing", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Dribbling", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Finishing", typeof(int)));
            playersDataColumnList.Add(new DataColumn("First Touch", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Free Kicks", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Heading", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Long Shots", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Long Throws", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Marking", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Passing", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Penalty Taking", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Tackling", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Technique", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Acceleration", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Agility", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Balance", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Jumping", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Natural Fitness", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Pace", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Stamina", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Strength", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Left Foot", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Right Foot", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Aggression", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Anticipation", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Bravery", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Composure", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Concentration", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Creativity", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Decisions", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Determination", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Flair", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Influence", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Off The Ball", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Positioning", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Team Work", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Work Rate", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Consistency", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Dirtyness", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Important Matches", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Injury Proneness", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Versatility", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Aerial Ability", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Command Of Area", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Communication", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Eccentricity", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Handling", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Kicking", typeof(int)));
            playersDataColumnList.Add(new DataColumn("One On Ones", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Reflexes", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Rushing Out", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Tendency To Punch", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Throwing", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Adaptability", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Ambition", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Controversy", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Loyalty", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Pressure", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Professionalism", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Sportsmanship", typeof(int)));
            playersDataColumnList.Add(new DataColumn("Temperament", typeof(int)));
            playersDataColumnList.Add(new DataColumn(""));

            this.playersDataTable.Rows.Clear();
            this.playersDataTable.Columns.Clear();
            this.playersDataTable.Columns.AddRange(playersDataColumnList.ToArray());
            this.PlayersDisplayGridView.DataSource = playersDataTable;
            for (int i = 0; i < playersColumnsWidth.Count; ++i)
            {
                DataGridViewColumn dc = this.PlayersDisplayGridView.Columns[i];
                dc.Width = playersColumnsWidth[i];
                dc.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dc.ToolTipText = dc.Name;
            }
            this.PlayersDisplayGridView.Columns[this.PlayersDisplayGridView.ColumnCount - 1].HeaderText = "";
            this.PlayersDisplayGridView.Columns[this.PlayersDisplayGridView.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.PlayersDisplayGridView.Columns[this.PlayersDisplayGridView.ColumnCount - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.PlayersDisplayGridView.Columns[this.PlayersDisplayGridView.ColumnCount - 1].ReadOnly = true;
        }

        public void initStaffDataTableColumns(ref List<int> staffColumnsWidth)
        {
            staffDataColumnList.Add(new DataColumn("ID", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Full Name", typeof(string)));
            staffDataColumnList.Add(new DataColumn("Nation", typeof(string)));
            staffDataColumnList.Add(new DataColumn("Club", typeof(string)));
            staffDataColumnList.Add(new DataColumn("Role", typeof(string)));
            staffDataColumnList.Add(new DataColumn("Age", typeof(int)));
            staffDataColumnList.Add(new DataColumn("CA", typeof(int)));
            staffDataColumnList.Add(new DataColumn("PA", typeof(int)));
            staffDataColumnList.Add(new DataColumn("ADiff", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Best CR", typeof(string)));
            staffDataColumnList.Add(new DataColumn("Best CR Stars", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Contract Started", typeof(DateTime)));
            staffDataColumnList.Add(new DataColumn("Contract Expiring", typeof(DateTime)));
            staffDataColumnList.Add(new DataColumn("Current Wage", typeof(int)));
            staffDataColumnList.Add(new DataColumn("World Reputation", typeof(int)));
            staffDataColumnList.Add(new DataColumn("National Reputation", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Local Reputation", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Attacking", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Defending", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Fitness", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Goalkeepers", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Mental", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Player", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Tactical", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Technical", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Man Management", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Working With Youngsters", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Adaptability", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Ambition", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Controversy", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Determination", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Loyalty", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Pressure", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Professionalism", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Sportsmanship", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Temperament", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Judging Player Ability", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Judging Player Potential", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Level Of Discipline", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Motivating", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Physiotherapy", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Tactical Knowledge", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Depth", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Directness", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Flamboyancy", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Flexibility", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Free Roles", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Marking", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Offside", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Pressing", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Sitting Back", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Tempo", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Use Of Playmaker", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Use Of Substitutions", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Width", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Buying Players", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Hardness Of Training", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Mind Games", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Squad Rotation", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Business", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Interference", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Patience", typeof(int)));
            staffDataColumnList.Add(new DataColumn("Resources", typeof(int)));
            staffDataColumnList.Add(new DataColumn(""));

            this.staffDataTable.Rows.Clear();
            this.staffDataTable.Columns.Clear();
            this.staffDataTable.Columns.AddRange(staffDataColumnList.ToArray());
            this.StaffDisplayGridView.DataSource = staffDataTable;
            for (int i = 0; i < staffColumnsWidth.Count; ++i)
            {
                DataGridViewColumn dc = this.StaffDisplayGridView.Columns[i];
                dc.Width = staffColumnsWidth[i];
                dc.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dc.ToolTipText = dc.Name;
            }
            this.StaffDisplayGridView.Columns[this.StaffDisplayGridView.ColumnCount - 1].HeaderText = "";
            this.StaffDisplayGridView.Columns[this.StaffDisplayGridView.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.StaffDisplayGridView.Columns[this.StaffDisplayGridView.ColumnCount - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.StaffDisplayGridView.Columns[this.StaffDisplayGridView.ColumnCount - 1].ReadOnly = true;
        }

        public void initTeamsDataTableColumns(ref List<int> teamsColumnsWidth)
        {
            teamsDataColumnList.Add(new DataColumn("ID", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Name", typeof(string)));
            teamsDataColumnList.Add(new DataColumn("Nation", typeof(string)));
            teamsDataColumnList.Add(new DataColumn("Reputation", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Status", typeof(string)));
            teamsDataColumnList.Add(new DataColumn("Stadium", typeof(string)));
            teamsDataColumnList.Add(new DataColumn("Transfer Budget", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Remaining Budget", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Wage Budget", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Wage Used", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Budget Balance", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Transfer Revenue Available", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Current Affiliated Clubs", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Max Affiliated Clubs", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Training Ground", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Maximum Attendance", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Average Attendance", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Minimum Attendance", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Decay", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Field Width", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Field Length", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Current Capacity", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Seating Capacity", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Expansion Capacity", typeof(int)));
            teamsDataColumnList.Add(new DataColumn("Used Capacity", typeof(int)));
            teamsDataColumnList.Add(new DataColumn(""));

            this.teamsDataTable.Rows.Clear();
            this.teamsDataTable.Columns.Clear();
            this.teamsDataTable.Columns.AddRange(teamsDataColumnList.ToArray());
            this.TeamsDisplayGridView.DataSource = teamsDataTable;
            for (int i = 0; i < teamsColumnsWidth.Count; ++i)
            {
                DataGridViewColumn dc = this.TeamsDisplayGridView.Columns[i];
                dc.Width = teamsColumnsWidth[i];
                dc.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dc.ToolTipText = dc.Name;
            }
            this.TeamsDisplayGridView.Columns[this.TeamsDisplayGridView.ColumnCount - 1].HeaderText = "";
            this.TeamsDisplayGridView.Columns[this.TeamsDisplayGridView.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.TeamsDisplayGridView.Columns[this.TeamsDisplayGridView.ColumnCount - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.TeamsDisplayGridView.Columns[this.TeamsDisplayGridView.ColumnCount - 1].ReadOnly = true;
        }

        public void initShortlistDataTableColumns(ref List<int> shortlistColumnsWidth)
        {
            shortlistDataColumnList.Add(new DataColumn("ID", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Full Name", typeof(string)));
            shortlistDataColumnList.Add(new DataColumn("Nation", typeof(string)));
            shortlistDataColumnList.Add(new DataColumn("Club", typeof(string)));
            shortlistDataColumnList.Add(new DataColumn("Team Squad", typeof(string)));
            shortlistDataColumnList.Add(new DataColumn("Position", typeof(string)));
            shortlistDataColumnList.Add(new DataColumn("Age", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("CA", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("PA", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("ADiff", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Best PR", typeof(string)));
            shortlistDataColumnList.Add(new DataColumn("Best PR%", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Current Value", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Sale Value", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Contract Started", typeof(DateTime)));
            shortlistDataColumnList.Add(new DataColumn("Contract Expiring", typeof(DateTime)));
            shortlistDataColumnList.Add(new DataColumn("Current Wage", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("World Reputation", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("National Reputation", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Local Reputation", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Corners", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Crossing", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Dribbling", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Finishing", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("First Touch", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Free Kicks", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Heading", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Long Shots", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Long Throws", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Marking", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Passing", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Penalty Taking", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Tackling", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Technique", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Acceleration", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Agility", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Balance", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Jumping", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Natural Fitness", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Pace", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Stamina", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Strength", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Left Foot", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Right Foot", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Aggression", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Anticipation", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Bravery", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Composure", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Concentration", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Creativity", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Decisions", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Determination", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Flair", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Influence", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Off The Ball", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Positioning", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Team Work", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Work Rate", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Consistency", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Dirtyness", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Important Matches", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Injury Proneness", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Versatility", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Aerial Ability", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Command Of Area", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Communication", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Eccentricity", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Handling", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Kicking", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("One On Ones", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Reflexes", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Rushing Out", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Tendency To Punch", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Throwing", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Adaptability", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Ambition", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Controversy", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Loyalty", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Pressure", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Professionalism", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Sportsmanship", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn("Temperament", typeof(int)));
            shortlistDataColumnList.Add(new DataColumn(""));

            this.shortlistDataTable.Rows.Clear();
            this.shortlistDataTable.Columns.Clear();
            this.shortlistDataTable.Columns.AddRange(shortlistDataColumnList.ToArray());
            this.ShortlistDisplayGridView.DataSource = shortlistDataTable;
            for (int i = 0; i < shortlistColumnsWidth.Count; ++i)
            {
                DataGridViewColumn dc = this.ShortlistDisplayGridView.Columns[i];
                dc.Width = shortlistColumnsWidth[i];
                dc.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dc.ToolTipText = dc.Name;
            }
            this.ShortlistDisplayGridView.Columns[this.ShortlistDisplayGridView.ColumnCount - 1].HeaderText = "";
            this.ShortlistDisplayGridView.Columns[this.ShortlistDisplayGridView.ColumnCount - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.ShortlistDisplayGridView.Columns[this.ShortlistDisplayGridView.ColumnCount - 1].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.ShortlistDisplayGridView.Columns[this.ShortlistDisplayGridView.ColumnCount - 1].ReadOnly = true;
        }

        public void clearPlayersTab()
        {
            this.PlayersSearchButton.Enabled = true;
            this.PlayersWonderkidButton.Enabled = true;

            this.PlayersSearchButton.Text = "Search";
            this.PlayersWonderkidButton.Text = "WonderKids";
            this.PlayersResultLabel.Text = "";
            this.PlayerFullNameTextBox.Text = "";
            for (int i = 0; i < PlayerPosition1CheckedListBox.Items.Count; i++)
                PlayerPosition1CheckedListBox.SetItemChecked(i, false);
            for (int i = 0; i < PlayerPosition2CheckedListBox.Items.Count; i++)
                PlayerPosition2CheckedListBox.SetItemChecked(i, false);
            this.PlayerContractStatusComboBox.SelectedIndex = -1;
            this.PlayerOwnerShipComboBox.SelectedIndex = -1;
            this.PlayerPosition1CheckedListBox.ClearSelected();
            this.PlayerPosition2CheckedListBox.ClearSelected();
            this.PlayerNationTextBox.Text = "";
            this.PlayerClubTextBox.Text = "";
            this.PlayerRegionComboBox.SelectedIndex = -1;
            this.PlayerRegenComboBox.SelectedIndex = -1;
            this.PlayerEUComboBox.SelectedIndex = -1;
            this.PlayerPrefFootComboBox.SelectedIndex = 0;
            this.PlayerBestPositionComboBox.SelectedIndex = -1;
            this.PlayerPositionalRatingMinNumericUpDown.Value = 0;
            this.PlayerPositionalRatingMaxNumericUpDown.Value = 100;
            this.PlayerAgeMinNumericUpDown.Value = 0;
            this.PlayerAgeMaxNumericUpDown.Value = 99;
            this.PlayerCAMinNumericUpDown.Value = 0;
            this.PlayerCAMaxNumericUpDown.Value = 200;
            this.PlayerPAMinNumericUpDown.Value = 0;
            this.PlayerPAMaxNumericUpDown.Value = 200;
            this.PlayerWageMinNumericUpDown.Value = this.PlayerWageMinNumericUpDown.Minimum;
            this.PlayerWageMaxNumericUpDown.Value = this.PlayerWageMaxNumericUpDown.Maximum;
            this.PlayerCurrentValueMinNumericUpDown.Value = 0;
            this.PlayerCurrentValueMaxNumericUpDown.Value = 200000000;
            this.PlayerSaleValueMinNumericUpDown.Value = 0;
            this.PlayerSaleValueMaxNumericUpDown.Value = 200000000;
            this.PlayerAerialAbilityMinNumericUpDown.Value = 0;
            this.PlayerAerialAbilityMaxNumericUpDown.Value = 20;
            this.PlayerCommandOfAreaMinNumericUpDown.Value = 0;
            this.PlayerCommandOfAreaMaxNumericUpDown.Value = 20;
            this.PlayerCommunicationMinNumericUpDown.Value = 0;
            this.PlayerCommunicationMaxNumericUpDown.Value = 20;
            this.PlayerEccentricityMinNumericUpDown.Value = 0;
            this.PlayerEccentricityMaxNumericUpDown.Value = 20;
            this.PlayerHandlingMinNumericUpDown.Value = 0;
            this.PlayerHandlingMaxNumericUpDown.Value = 20;
            this.PlayerKickingMinNumericUpDown.Value = 0;
            this.PlayerKickingMaxNumericUpDown.Value = 20;
            this.PlayerOneOnOnesMinNumericUpDown.Value = 0;
            this.PlayerOneOnOnesMaxNumericUpDown.Value = 20;
            this.PlayerReflexesMinNumericUpDown.Value = 0;
            this.PlayerReflexesMaxNumericUpDown.Value = 20;
            this.PlayerRushingOutMinNumericUpDown.Value = 0;
            this.PlayerRushingOutMaxNumericUpDown.Value = 20;
            this.PlayerTendencyToPunchMinNumericUpDown.Value = 0;
            this.PlayerTendencyToPunchMaxNumericUpDown.Value = 20;
            this.PlayerThrowingMinNumericUpDown.Value = 0;
            this.PlayerThrowingMaxNumericUpDown.Value = 20;
            this.PlayerCornersMinNumericUpDown.Value = 0;
            this.PlayerCornersMaxNumericUpDown.Value = 20;
            this.PlayerCrossingMinNumericUpDown.Value = 0;
            this.PlayerCrossingMaxNumericUpDown.Value = 20;
            this.PlayerDribblingMinNumericUpDown.Value = 0;
            this.PlayerDribblingMaxNumericUpDown.Value = 20;
            this.PlayerFinishingMinNumericUpDown.Value = 0;
            this.PlayerFinishingMaxNumericUpDown.Value = 20;
            this.PlayerFirstTouchMinNumericUpDown.Value = 0;
            this.PlayerFirstTouchMaxNumericUpDown.Value = 20;
            this.PlayerFreeKicksMinNumericUpDown.Value = 0;
            this.PlayerFreeKicksMaxNumericUpDown.Value = 20;
            this.PlayerHeadingMinNumericUpDown.Value = 0;
            this.PlayerHeadingMaxNumericUpDown.Value = 20;
            this.PlayerLongShotsMinNumericUpDown.Value = 0;
            this.PlayerLongShotsMaxNumericUpDown.Value = 20;
            this.PlayerLongThrowsMinNumericUpDown.Value = 0;
            this.PlayerLongThrowsMaxNumericUpDown.Value = 20;
            this.PlayerMarkingMinNumericUpDown.Value = 0;
            this.PlayerMarkingMaxNumericUpDown.Value = 20;
            this.PlayerPassingMinNumericUpDown.Value = 0;
            this.PlayerPassingMaxNumericUpDown.Value = 20;
            this.PlayerPenaltyTakingMinNumericUpDown.Value = 0;
            this.PlayerPenaltyTakingMaxNumericUpDown.Value = 20;
            this.PlayerTacklingMinNumericUpDown.Value = 0;
            this.PlayerTacklingMaxNumericUpDown.Value = 20;
            this.PlayerTechniqueMinNumericUpDown.Value = 0;
            this.PlayerTechniqueMaxNumericUpDown.Value = 20;
            this.PlayerAccelerationMinNumericUpDown.Value = 0;
            this.PlayerAccelerationMaxNumericUpDown.Value = 20;
            this.PlayerAgilityMinNumericUpDown.Value = 0;
            this.PlayerAgilityMaxNumericUpDown.Value = 20;
            this.PlayerBalanceMinNumericUpDown.Value = 0;
            this.PlayerBalanceMaxNumericUpDown.Value = 20;
            this.PlayerJumpingMinNumericUpDown.Value = 0;
            this.PlayerJumpingMaxNumericUpDown.Value = 20;
            this.PlayerNaturalFitnessMinNumericUpDown.Value = 0;
            this.PlayerNaturalFitnessMaxNumericUpDown.Value = 20;
            this.PlayerPaceMinNumericUpDown.Value = 0;
            this.PlayerPaceMaxNumericUpDown.Value = 20;
            this.PlayerStaminaMinNumericUpDown.Value = 0;
            this.PlayerStaminaMaxNumericUpDown.Value = 20;
            this.PlayerStrengthMinNumericUpDown.Value = 0;
            this.PlayerStrengthMaxNumericUpDown.Value = 20;
            this.PlayerLeftFootMinNumericUpDown.Value = 0;
            this.PlayerLeftFootMaxNumericUpDown.Value = 20;
            this.PlayerRightFootMinNumericUpDown.Value = 0;
            this.PlayerRightFootMaxNumericUpDown.Value = 20;
            this.PlayerAggressionMinNumericUpDown.Value = 0;
            this.PlayerAggressionMaxNumericUpDown.Value = 20;
            this.PlayerAnticipationMinNumericUpDown.Value = 0;
            this.PlayerAnticipationMaxNumericUpDown.Value = 20;
            this.PlayerBraveryMinNumericUpDown.Value = 0;
            this.PlayerBraveryMaxNumericUpDown.Value = 20;
            this.PlayerComposureMinNumericUpDown.Value = 0;
            this.PlayerComposureMaxNumericUpDown.Value = 20;
            this.PlayerConcentrationMinNumericUpDown.Value = 0;
            this.PlayerConcentrationMaxNumericUpDown.Value = 20;
            this.PlayerCreativityMinNumericUpDown.Value = 0;
            this.PlayerCreativityMaxNumericUpDown.Value = 20;
            this.PlayerDecisionsMinNumericUpDown.Value = 0;
            this.PlayerDecisionsMaxNumericUpDown.Value = 20;
            this.PlayerDeterminationMinNumericUpDown.Value = 0;
            this.PlayerDeterminationMaxNumericUpDown.Value = 20;
            this.PlayerFlairMinNumericUpDown.Value = 0;
            this.PlayerFlairMaxNumericUpDown.Value = 20;
            this.PlayerInfluenceMinNumericUpDown.Value = 0;
            this.PlayerInfluenceMaxNumericUpDown.Value = 20;
            this.PlayerOffTheBallMinNumericUpDown.Value = 0;
            this.PlayerOffTheBallMaxNumericUpDown.Value = 20;
            this.PlayerPositioningMinNumericUpDown.Value = 0;
            this.PlayerPositioningMaxNumericUpDown.Value = 20;
            this.PlayerTeamworkMinNumericUpDown.Value = 0;
            this.PlayerTeamworkMaxNumericUpDown.Value = 20;
            this.PlayerWorkRateMinNumericUpDown.Value = 0;
            this.PlayerWorkRateMaxNumericUpDown.Value = 20;
            this.PlayerConsistencyMinNumericUpDown.Value = 0;
            this.PlayerConsistencyMaxNumericUpDown.Value = 20;
            this.PlayerDirtynessMinNumericUpDown.Value = 0;
            this.PlayerDirtynessMaxNumericUpDown.Value = 20;
            this.PlayerImportantMatchesMinNumericUpDown.Value = 0;
            this.PlayerImportantMatchesMaxNumericUpDown.Value = 20;
            this.PlayerInjuryPronenessMinNumericUpDown.Value = 0;
            this.PlayerInjuryPronenessMaxNumericUpDown.Value = 20;
            this.PlayerVersatilityMinNumericUpDown.Value = 0;
            this.PlayerVersatilityMaxNumericUpDown.Value = 20;
            this.PlayerAdaptabilityMinNumericUpDown.Value = 0;
            this.PlayerAdaptabilityMaxNumericUpDown.Value = 20;
            this.PlayerAmbitionMinNumericUpDown.Value = 0;
            this.PlayerAmbitionMaxNumericUpDown.Value = 20;
            this.PlayerControversyMinNumericUpDown.Value = 0;
            this.PlayerControversyMaxNumericUpDown.Value = 20;
            this.PlayerLoyaltyMinNumericUpDown.Value = 0;
            this.PlayerLoyaltyMaxNumericUpDown.Value = 20;
            this.PlayerPressureMinNumericUpDown.Value = 0;
            this.PlayerPressureMaxNumericUpDown.Value = 20;
            this.PlayerProfessionalismMinNumericUpDown.Value = 0;
            this.PlayerProfessionalismMaxNumericUpDown.Value = 20;
            this.PlayerSportsmanshipMinNumericUpDown.Value = 0;
            this.PlayerSportsmanshipMaxNumericUpDown.Value = 20;
            this.PlayerTemperamentMinNumericUpDown.Value = 0;
            this.PlayerTemperamentMaxNumericUpDown.Value = 20;
        }

        public void clearPlayerProfileTab()
        {
            this.PlayerProfileSelectSkillsButton.Enabled = false;
            this.PlayerProfileHealButton.Enabled = false;
            this.ProfileSaveEditingToolStrip.Enabled = false;
            this.ProfileCancelEditingToolStrip.Enabled = false;
            this.PlayerProfileShortlistButton.Visible = false;

            this.PlayerProfileIDTextBox.Text = "ID";
            this.PlayerProfileFullNameTextBox.Text = "Full Name";
            this.PlayerProfileClubTextBox.Text = "Club";
            this.PlayerProfileTeamSquadTextBox.Text = "No Squad";
            this.PlayerProfileNationalityTextBox.Text = "Nationality";
            this.PlayerProfileFormedTextBox.Text = "Formed At";
            this.PlayerProfileEUMemberTextBox.Text = "EU Member";
            this.PlayerProfileHomeGrownTextBox.Text = "HomeGrown";
            this.PlayerProfileInternationalTextBox.Text = "International";
            this.PlayerProfileBirthDateTextBox.Text = "Birth Date";
            this.PlayerProfileAgeTextBox.Text = "Age";
            this.PlayerProfileHeightTextBox.Text = "Height";
            this.PlayerProfileValueTextBox.Text = "Value";
            this.PlayerProfileSaleValueTextBox.Text = "Sale Value";
            this.PlayerProfileContractStartedTextBox.Text = "No Date";
            this.PlayerProfileContractExpiryTextBox.Text = "No Date";
            this.PlayerProfileWageTextBox.Text = "0";
            this.PlayerProfileCATextBox.Text = "CA";
            this.PlayerProfileWeightTextBox.Text = "Weight";
            this.PlayerProfilePreferredFootTextBox.Text = "Pref Foot";
            this.PlayerProfilePATextBox.Text = "PA";
            this.PlayerProfileShortlistTextBox.Text = "Shortlist";
            this.PlayerProfilePositionTextBox.Text = "Position";
            this.PlayerProfileAppearanceBonusTextBox.Text = "0";
            this.PlayerProfileGoalBonusTextBox.Text = "0";
            this.PlayerProfileCleanSheetBonusTextBox.Text = "0";
            this.PlayerProfileAerialAbilityTextBox.Text = "0";
            this.PlayerProfileCommandOfAreaTextBox.Text = "0";
            this.PlayerProfileCommunicationTextBox.Text = "0";
            this.PlayerProfileEccentricityTextBox.Text = "0";
            this.PlayerProfileHandlingTextBox.Text = "0";
            this.PlayerProfileKickingTextBox.Text = "0";
            this.PlayerProfileOneOnOnesTextBox.Text = "0";
            this.PlayerProfileReflexesTextBox.Text = "0";
            this.PlayerProfileRushingOutTextBox.Text = "0";
            this.PlayerProfileTendencyToPunchTextBox.Text = "0";
            this.PlayerProfileThrowingTextBox.Text = "0";
            this.PlayerProfileCornersTextBox.Text = "0";
            this.PlayerProfileCrossingTextBox.Text = "0";
            this.PlayerProfileDribblingTextBox.Text = "0";
            this.PlayerProfileFinishingTextBox.Text = "0";
            this.PlayerProfileFirstTouchTextBox.Text = "0";
            this.PlayerProfileFreeKicksTextBox.Text = "0";
            this.PlayerProfileHeadingTextBox.Text = "0";
            this.PlayerProfileLongShotsTextBox.Text = "0";
            this.PlayerProfileLongThrowsTextBox.Text = "0";
            this.PlayerProfileMarkingTextBox.Text = "0";
            this.PlayerProfilePassingTextBox.Text = "0";
            this.PlayerProfilePenaltyTakingTextBox.Text = "0";
            this.PlayerProfileTacklingTextBox.Text = "0";
            this.PlayerProfileTechniqueTextBox.Text = "0";
            this.PlayerProfileAccelerationTextBox.Text = "0";
            this.PlayerProfileAgilityTextBox.Text = "0";
            this.PlayerProfileBalanceTextBox.Text = "0";
            this.PlayerProfileJumpingTextBox.Text = "0";
            this.PlayerProfileNaturalFitnessTextBox.Text = "0";
            this.PlayerProfilePaceTextBox.Text = "0";
            this.PlayerProfileStaminaTextBox.Text = "0";
            this.PlayerProfileStrengthTextBox.Text = "0";
            this.PlayerProfileAggressionTextBox.Text = "0";
            this.PlayerProfileAnticipationTextBox.Text = "0";
            this.PlayerProfileBraveryTextBox.Text = "0";
            this.PlayerProfileComposureTextBox.Text = "0";
            this.PlayerProfileConcentrationTextBox.Text = "0";
            this.PlayerProfileCreativityTextBox.Text = "0";
            this.PlayerProfileDecisionsTextBox.Text = "0";
            this.PlayerProfileDeterminationTextBox.Text = "0";
            this.PlayerProfileFlairTextBox.Text = "0";
            this.PlayerProfileInfluenceTextBox.Text = "0";
            this.PlayerProfileOffTheBallTextBox.Text = "0";
            this.PlayerProfilePositioningTextBox.Text = "0";
            this.PlayerProfileTeamWorkTextBox.Text = "0";
            this.PlayerProfileWorkRateTextBox.Text = "0";
            this.PlayerProfileConsistencyTextBox.Text = "0";
            this.PlayerProfileDirtynessTextBox.Text = "0";
            this.PlayerProfileImportantMatchesTextBox.Text = "0";
            this.PlayerProfileInjuryPronenessTextBox.Text = "0";
            this.PlayerProfileVersatilityTextBox.Text = "0";
            this.PlayerProfileAdaptabilityTextBox.Text = "0";
            this.PlayerProfileAmbitionTextBox.Text = "0";
            this.PlayerProfileControversyTextBox.Text = "0";
            this.PlayerProfileLoyaltyTextBox.Text = "0";
            this.PlayerProfilePressureTextBox.Text = "0";
            this.PlayerProfileProfessionalismTextBox.Text = "0";
            this.PlayerProfileSportsmanshipTextBox.Text = "0";
            this.PlayerProfileTemperamentTextBox.Text = "0";
            this.PlayerProfileConditionTextBox.Text = "0";
            this.PlayerProfileMoraleTextBox.Text = "0";
            this.PlayerProfileHappinessTextBox.Text = "0";
            this.PlayerProfileJadednessTextBox.Text = "0";
            this.PlayerProfileSquadNoTextBox.Text = "0";
            this.PlayerProfileLeftFootTextBox.Text = "0";
            this.PlayerProfileRightFootTextBox.Text = "0";
            this.PlayerProfileWorldReputationTextBox.Text = "0";
            this.PlayerProfileNationalReputationTextBox.Text = "0";
            this.PlayerProfileLocalReputationTextBox.Text = "0";
            this.PlayerProfilePositionalRatingLabel.Text = "Positional Rating:\r\nBest as: None";
            this.PlayerProfileGKLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileDLLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileDRLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileDCLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileDMLLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileDMRLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileDMCLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileAMLLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileAMRLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileAMCLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileFCQuickLabel.Text = (0.0f).ToString("P1");
            this.PlayerProfileFCStrongLabel.Text = (0.0f).ToString("P1");

            this.PlayerProfileGKLabel.Font = themeFont;
            this.PlayerProfileDCLabel.Font = themeFont;
            this.PlayerProfileDLLabel.Font = themeFont;
            this.PlayerProfileDRLabel.Font = themeFont;
            this.PlayerProfileDMCLabel.Font = themeFont;
            this.PlayerProfileDMLLabel.Font = themeFont;
            this.PlayerProfileDMRLabel.Font = themeFont;
            this.PlayerProfileAMCLabel.Font = themeFont;
            this.PlayerProfileAMLLabel.Font = themeFont;
            this.PlayerProfileAMRLabel.Font = themeFont;
            this.PlayerProfileFCQuickLabel.Font = themeFont;
            this.PlayerProfileFCStrongLabel.Font = themeFont;

            setPlayerEditing(false);
            setPlayerProfileColor();
        }

        public void clearStaffTab()
        {
            this.StaffSearchButton.Enabled = true;
            this.StaffWonderStaffButton.Enabled = true;

            this.StaffSearchButton.Text = "Search";
            this.StaffWonderStaffButton.Text = "WonderStaff";
            this.StaffResultLabel.Text = "";
            this.StaffFullNameTextBox.Text = "";
            this.StaffNationTextBox.Text = "";
            this.StaffClubTextBox.Text = "";
            this.StaffRegionComboBox.SelectedIndex = -1;
            this.StaffContractStatusComboBox.SelectedIndex = -1;
            this.StaffRoleComboBox.SelectedIndex = -1;
            this.StaffRegenComboBox.SelectedIndex = -1;
            this.StaffBestCRComboBox.SelectedIndex = -1;
            this.StaffRatingsFitnessMinNumericUpDown.Value = 1;
            this.StaffRatingsFitnessMaxNumericUpDown.Value = 7;
            this.StaffRatingsGoalkeepersMinNumericUpDown.Value = 1;
            this.StaffRatingsGoalkeepersMaxNumericUpDown.Value = 7;
            this.StaffRatingsTacticsMinNumericUpDown.Value = 1;
            this.StaffRatingsTacticsMaxNumericUpDown.Value = 7;
            this.StaffRatingsBallControlMinNumericUpDown.Value = 1;
            this.StaffRatingsBallControlMaxNumericUpDown.Value = 7;
            this.StaffRatingsDefendingMinNumericUpDown.Value = 1;
            this.StaffRatingsDefendingMaxNumericUpDown.Value = 7;
            this.StaffRatingsAttackingMinNumericUpDown.Value = 1;
            this.StaffRatingsAttackingMaxNumericUpDown.Value = 7;
            this.StaffRatingsShootingMinNumericUpDown.Value = 1;
            this.StaffRatingsShootingMaxNumericUpDown.Value = 7;
            this.StaffRatingsSetPiecesMinNumericUpDown.Value = 1;
            this.StaffRatingsSetPiecesMaxNumericUpDown.Value = 7;
            this.StaffAgeMinNumericUpDown.Value = 0;
            this.StaffAgeMaxNumericUpDown.Value = 99;
            this.StaffCAMinNumericUpDown.Value = 0;
            this.StaffCAMaxNumericUpDown.Value = 200;
            this.StaffPAMinNumericUpDown.Value = 0;
            this.StaffPAMaxNumericUpDown.Value = 200;
            this.StaffAttackingMinNumericUpDown.Value = 0;
            this.StaffAttackingMaxNumericUpDown.Value = 20;
            this.StaffDefendingMinNumericUpDown.Value = 0;
            this.StaffDefendingMaxNumericUpDown.Value = 20;
            this.StaffFitnessMinNumericUpDown.Value = 0;
            this.StaffFitnessMaxNumericUpDown.Value = 20;
            this.StaffGoalkeepersMinNumericUpDown.Value = 0;
            this.StaffGoalkeepersMaxNumericUpDown.Value = 20;
            this.StaffMentalMinNumericUpDown.Value = 0;
            this.StaffMentalMaxNumericUpDown.Value = 20;
            this.StaffPlayerMinNumericUpDown.Value = 0;
            this.StaffPlayerMaxNumericUpDown.Value = 20;
            this.StaffTacticalMinNumericUpDown.Value = 0;
            this.StaffTacticalMaxNumericUpDown.Value = 20;
            this.StaffTechnicalMinNumericUpDown.Value = 0;
            this.StaffTechnicalMaxNumericUpDown.Value = 20;
            this.StaffManManagementMinNumericUpDown.Value = 0;
            this.StaffManManagementMaxNumericUpDown.Value = 20;
            this.StaffWorkingWithYoungstersMinNumericUpDown.Value = 0;
            this.StaffWorkingWithYoungstersMaxNumericUpDown.Value = 20;
            this.StaffAdaptabilityMinNumericUpDown.Value = 0;
            this.StaffAdaptabilityMaxNumericUpDown.Value = 20;
            this.StaffAmbitionMinNumericUpDown.Value = 0;
            this.StaffAmbitionMaxNumericUpDown.Value = 20;
            this.StaffControversyMinNumericUpDown.Value = 0;
            this.StaffControversyMaxNumericUpDown.Value = 20;
            this.StaffDeterminationMinNumericUpDown.Value = 0;
            this.StaffDeterminationMaxNumericUpDown.Value = 20;
            this.StaffLoyaltyMinNumericUpDown.Value = 0;
            this.StaffLoyaltyMaxNumericUpDown.Value = 20;
            this.StaffPressureMinNumericUpDown.Value = 0;
            this.StaffPressureMaxNumericUpDown.Value = 20;
            this.StaffProfessionalismMinNumericUpDown.Value = 0;
            this.StaffProfessionalismMaxNumericUpDown.Value = 20;
            this.StaffSportmanshipMinNumericUpDown.Value = 0;
            this.StaffSportmanshipMaxNumericUpDown.Value = 20;
            this.StaffTemperamentMinNumericUpDown.Value = 0;
            this.StaffTemperamentMaxNumericUpDown.Value = 20;
            this.StaffJudgingPlayerAbilityMinNumericUpDown.Value = 0;
            this.StaffJudgingPlayerAbilityMaxNumericUpDown.Value = 20;
            this.StaffJudgingPlayerPotentialMinNumericUpDown.Value = 0;
            this.StaffJudgingPlayerPotentialMaxNumericUpDown.Value = 20;
            this.StaffLevelOfDisciplineMinNumericUpDown.Value = 0;
            this.StaffLevelOfDisciplineMaxNumericUpDown.Value = 20;
            this.StaffMotivatingMinNumericUpDown.Value = 0;
            this.StaffMotivatingMaxNumericUpDown.Value = 20;
            this.StaffPhysiotherapyMinNumericUpDown.Value = 0;
            this.StaffPhysiotherapyMaxNumericUpDown.Value = 20;
            this.StaffTacticalKnowledgeMinNumericUpDown.Value = 0;
            this.StaffTacticalKnowledgeMaxNumericUpDown.Value = 20;
            this.StaffDepthMinNumericUpDown.Value = 0;
            this.StaffDepthMaxNumericUpDown.Value = 20;
            this.StaffDirectnessMinNumericUpDown.Value = 0;
            this.StaffDirectnessMaxNumericUpDown.Value = 20;
            this.StaffFlamboyancyMinNumericUpDown.Value = 0;
            this.StaffFlamboyancyMaxNumericUpDown.Value = 20;
            this.StaffFlexibilityMinNumericUpDown.Value = 0;
            this.StaffFlexibilityMaxNumericUpDown.Value = 20;
            this.StaffFreeRolesMinNumericUpDown.Value = 0;
            this.StaffFreeRolesMaxNumericUpDown.Value = 20;
            this.StaffMarkingMinNumericUpDown.Value = 0;
            this.StaffMarkingMaxNumericUpDown.Value = 20;
            this.StaffOffsideMinNumericUpDown.Value = 0;
            this.StaffOffsideMaxNumericUpDown.Value = 20;
            this.StaffPressingMinNumericUpDown.Value = 0;
            this.StaffPressingMaxNumericUpDown.Value = 20;
            this.StaffSittingBackMinNumericUpDown.Value = 0;
            this.StaffSittingBackMaxNumericUpDown.Value = 20;
            this.StaffTempoMinNumericUpDown.Value = 0;
            this.StaffTempoMaxNumericUpDown.Value = 20;
            this.StaffUseOfPlaymakerMinNumericUpDown.Value = 0;
            this.StaffUseOfPlaymakerMaxNumericUpDown.Value = 20;
            this.StaffUseOfSubstitutionsMinNumericUpDown.Value = 0;
            this.StaffUseOfSubstitutionsMaxNumericUpDown.Value = 20;
            this.StaffWidthMinNumericUpDown.Value = 0;
            this.StaffWidthMaxNumericUpDown.Value = 20;
            this.StaffBuyingPlayersMinNumericUpDown.Value = 0;
            this.StaffBuyingPlayersMaxNumericUpDown.Value = 20;
            this.StaffHardnessOfTrainingMinNumericUpDown.Value = 0;
            this.StaffHardnessOfTrainingMaxNumericUpDown.Value = 20;
            this.StaffMindgamesMinNumericUpDown.Value = 0;
            this.StaffMindgamesMaxNumericUpDown.Value = 20;
            this.StaffSquadRotationMinNumericUpDown.Value = 0;
            this.StaffSquadRotationMaxNumericUpDown.Value = 20;
            this.StaffBusinessMinNumericUpDown.Value = 0;
            this.StaffBusinessMaxNumericUpDown.Value = 20;
            this.StaffInterferenceMinNumericUpDown.Value = 0;
            this.StaffInterferenceMaxNumericUpDown.Value = 20;
            this.StaffPatienceMinNumericUpDown.Value = 0;
            this.StaffPatienceMaxNumericUpDown.Value = 20;
            this.StaffResourcesMinNumericUpDown.Value = 0;
            this.StaffResourcesMaxNumericUpDown.Value = 20;
        }

        public void clearStaffProfileTab()
        {
            this.ProfileSaveEditingToolStrip.Enabled = false;
            this.ProfileCancelEditingToolStrip.Enabled = false;

            this.StaffProfileIDTextBox.Text = "ID";
            this.StaffProfileFullNameTextBox.Text = "Full Name";
            this.StaffProfileClubTextBox.Text = "Club";
            this.StaffProfileNationalityTextBox.Text = "Nationality";
            this.StaffProfileInternationalTextBox.Text = "International";
            this.StaffProfileBirthDateTextBox.Text = "Birth Date";
            this.StaffProfileAgeTextBox.Text = "Age";
            this.StaffProfileContractStartedTextBox.Text = "No Date";
            this.StaffProfileContractExpiryTextBox.Text = "No Date";
            this.StaffProfileWageTextBox.Text = "0";
            this.StaffProfileCATextBox.Text = "CA";
            this.StaffProfilePATextBox.Text = "PA";
            this.StaffProfileRoleTextBox.Text = "Role";
            this.StaffProfileAttackingTextBox.Text = "0";
            this.StaffProfileDefendingTextBox.Text = "0";
            this.StaffProfileFitnessTextBox.Text = "0";
            this.StaffProfileGoalkeepersTextBox.Text = "0";
            this.StaffProfileMentalTextBox.Text = "0";
            this.StaffProfilePlayerTextBox.Text = "0";
            this.StaffProfileTacticalTextBox.Text = "0";
            this.StaffProfileTechnicalTextBox.Text = "0";
            this.StaffProfileManManagementTextBox.Text = "0";
            this.StaffProfileWorkingWithYoungstersTextBox.Text = "0";
            this.StaffProfileAdaptabilityTextBox.Text = "0";
            this.StaffProfileAmbitionTextBox.Text = "0";
            this.StaffProfileControversyTextBox.Text = "0";
            this.StaffProfileDeterminationTextBox.Text = "0";
            this.StaffProfileLoyaltyTextBox.Text = "0";
            this.StaffProfilePressureTextBox.Text = "0";
            this.StaffProfileProfessionalismTextBox.Text = "0";
            this.StaffProfileSportsmanshipTextBox.Text = "0";
            this.StaffProfileTemperamentTextBox.Text = "0";
            this.StaffProfileJudgingPlayerAbilityTextBox.Text = "0";
            this.StaffProfileJudgingPlayerPotentialTextBox.Text = "0";
            this.StaffProfileLevelOfDisciplineTextBox.Text = "0";
            this.StaffProfileMotivatingTextBox.Text = "0";
            this.StaffProfilePhysiotherapyTextBox.Text = "0";
            this.StaffProfileTacticalKnowledgeTextBox.Text = "0";
            this.StaffProfileDepthTextBox.Text = "0";
            this.StaffProfileDirectnessTextBox.Text = "0";
            this.StaffProfileFlamboyancyTextBox.Text = "0";
            this.StaffProfileFlexibilityTextBox.Text = "0";
            this.StaffProfileFreeRolesTextBox.Text = "0";
            this.StaffProfileMarkingTextBox.Text = "0";
            this.StaffProfileOffsideTextBox.Text = "0";
            this.StaffProfilePressingTextBox.Text = "0";
            this.StaffProfileSittingBackTextBox.Text = "0";
            this.StaffProfileTempoTextBox.Text = "0";
            this.StaffProfileUseOfPlaymakerTextBox.Text = "0";
            this.StaffProfileUseOfSubstitutionsTextBox.Text = "0";
            this.StaffProfileWidthTextBox.Text = "0";
            this.StaffProfileBuyingPlayersTextBox.Text = "0";
            this.StaffProfileHardnessOfTrainingTextBox.Text = "0";
            this.StaffProfileMindGamesTextBox.Text = "0";
            this.StaffProfileSquadRotationTextBox.Text = "0";
            this.StaffProfileBusinessTextBox.Text = "0";
            this.StaffProfileInterferenceTextBox.Text = "0";
            this.StaffProfilePatienceTextBox.Text = "0";
            this.StaffProfileResourcesTextBox.Text = "0";
            this.StaffProfileWorldReputationTextBox.Text = "0";
            this.StaffProfileNationalReputationTextBox.Text = "0";
            this.StaffProfileLocalReputationTextBox.Text = "0";
            this.StaffProfileBestRatingsGroupBox.Refresh();

            setStaffEditing(false);
            setStaffProfileColor();
        }

        public void clearTeamsTab()
        {
            this.TeamsSearchButton.Enabled = true;
            this.TeamsWonderTeamsButton.Enabled = true;

            this.TeamsSearchButton.Text = "Search";
            this.TeamsWonderTeamsButton.Text = "WonderTeams";
            this.TeamsResultLabel.Text = "";
            this.TeamNameTextBox.Text = "";
            this.TeamNationalityTextBox.Text = "";
            this.TeamTypeComboBox.SelectedIndex = -1;
            this.TeamReputationComboBox.SelectedIndex = -1;
            this.TeamRegionComboBox.SelectedIndex = -1;
            this.TeamStadiumNameTextBox.Text = "";
            this.TeamTransferBudgetMinNumericUpDown.Value = 0;
            this.TeamTransferBudgetMaxNumericUpDown.Value = 200000000;
            this.TeamWageBudgetMinNumericUpDown.Value = 0;
            this.TeamWageBudgetMaxNumericUpDown.Value = 200000000;
        }

        private void clearTeamProfileTab()
        {
            this.ProfileSaveEditingToolStrip.Enabled = false;
            this.ProfileCancelEditingToolStrip.Enabled = false;
            this.TeamProfileListPlayersComboBox.Enabled = true;
            this.TeamProfileListPlayersComboBox.SelectedIndex = -1;
            this.TeamProfileListPlayersComboBox.Enabled = false;
            this.TeamProfileListPlayersButton.Enabled = false;
            this.TeamProfileListStaffButton.Enabled = false;
            this.TeamProfileHealTeamButton.Enabled = false;

            this.TeamProfileIDTextBox.Text = "ID";
            this.TeamProfileNameTextBox.Text = "Name";
            this.TeamProfileNationalityTextBox.Text = "Nationality";
            this.TeamProfileYearFoundedTextBox.Text = "None";
            this.TeamProfileNationalTextBox.Text = "None";
            this.TeamProfileStatusTextBox.Text = "None";
            this.TeamProfileMaxAffiliatedClubsTextBox.Text = "0";
            this.TeamProfileAffiliatedClubsTextBox.Text = "0";
            this.TeamProfileTrainingGroundTextBox.Text = "0";
            this.TeamProfileYouthGroundTextBox.Text = "0";
            this.TeamProfileYouthAcademyTextBox.Text = "None";
            this.TeamProfileReputationTextBox.Text = "0";

            // finance details
            this.TeamProfileTotalTransferTextBox.Text = "0";
            this.TeamProfileRemainingTransferTextBox.Text = "0";
            this.TeamProfileBalanceTextBox.Text = "0";
            this.TeamProfileTotalWageTextBox.Text = "0";
            this.TeamProfileUsedWageTextBox.Text = "0";
            this.TeamProfileRevenueAvailableTextBox.Text = "0";

            // stadium details
            this.TeamProfileStadiumIDTextBox.Text = "ID";
            this.TeamProfileStadiumNameTextBox.Text = "Name";
            this.TeamProfileStadiumOwnerTextBox.Text = "Owner";
            this.TeamProfileStadiumLocationTextBox.Text = "City";
            this.TeamProfileStadiumNearbyStadiumTextBox.Text = "Nearby Stadium";
            this.TeamProfileStadiumDecayTextBox.Text = "0";
            this.TeamProfileStadiumFieldWidthTextBox.Text = "0";
            this.TeamProfileStadiumFieldLengthTextBox.Text = "0";
            this.TeamProfileStadiumCurrentCapacityTextBox.Text = "0";
            this.TeamProfileStadiumSeatingCapacityTextBox.Text = "0";
            this.TeamProfileStadiumExpansionCapacityTextBox.Text = "0";
            this.TeamProfileStadiumUsedCapacityTextBox.Text = "0";
            this.TeamProfileMaxAttendanceTextBox.Text = "0";
            this.TeamProfileAverageAttendanceTextBox.Text = "0";
            this.TeamProfileMinAttendanceTextBox.Text = "0";

            setTeamEditing(false);
            setTeamProfileColor();
        }

        private void initPlayersSpecialAttributes(ref int[] numericUpDownArray)
        {
            int c = -1;
            numericUpDownArray[++c] = (int)this.PlayerAgeMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerAgeMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerCAMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerCAMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerPAMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerPAMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerCurrentValueMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerCurrentValueMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerSaleValueMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerSaleValueMaxNumericUpDown.Value;
            numericUpDownArray[++c] = ((int)((float)this.PlayerAerialAbilityMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerAerialAbilityMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerCommandOfAreaMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerCommandOfAreaMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerCommunicationMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerCommunicationMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerEccentricityMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerEccentricityMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerHandlingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerHandlingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerKickingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerKickingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerOneOnOnesMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerOneOnOnesMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerReflexesMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerReflexesMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerRushingOutMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerRushingOutMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerTendencyToPunchMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerTendencyToPunchMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerThrowingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerThrowingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerCornersMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerCornersMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerCrossingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerCrossingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerDribblingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerDribblingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerFinishingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerFinishingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerFirstTouchMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerFirstTouchMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerFreeKicksMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerFreeKicksMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerHeadingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerHeadingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerLongShotsMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerLongShotsMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerLongThrowsMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerLongThrowsMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerMarkingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerMarkingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerPassingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerPassingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerPenaltyTakingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerPenaltyTakingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerTacklingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerTacklingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerTechniqueMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerTechniqueMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerAccelerationMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerAccelerationMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerAgilityMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerAgilityMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerBalanceMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerBalanceMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerJumpingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerJumpingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerNaturalFitnessMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerNaturalFitnessMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerPaceMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerPaceMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerStaminaMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerStaminaMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerStrengthMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerStrengthMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerLeftFootMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerLeftFootMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerRightFootMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerRightFootMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerAggressionMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerAggressionMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerAnticipationMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerAnticipationMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerBraveryMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerBraveryMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerComposureMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerComposureMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerConcentrationMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerConcentrationMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerCreativityMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerCreativityMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerDecisionsMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerDecisionsMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerDeterminationMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerDeterminationMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerFlairMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerFlairMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerInfluenceMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerInfluenceMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerOffTheBallMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerOffTheBallMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerPositioningMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerPositioningMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerTeamworkMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerTeamworkMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerWorkRateMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerWorkRateMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerConsistencyMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerConsistencyMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerDirtynessMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerDirtynessMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerImportantMatchesMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerImportantMatchesMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerInjuryPronenessMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerInjuryPronenessMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerVersatilityMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.PlayerVersatilityMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = (int)this.PlayerAdaptabilityMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerAdaptabilityMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerAmbitionMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerAmbitionMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerControversyMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerControversyMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerLoyaltyMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerLoyaltyMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerPressureMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerPressureMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerProfessionalismMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerProfessionalismMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerSportsmanshipMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerSportsmanshipMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerTemperamentMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.PlayerTemperamentMaxNumericUpDown.Value;
        }

        private void initStaffSpecialAttributes(ref int[] numericUpDownArray)
        {
            int c = -1;
            numericUpDownArray[++c] = (int)this.StaffAgeMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.StaffAgeMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.StaffCAMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.StaffCAMaxNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.StaffPAMinNumericUpDown.Value;
            numericUpDownArray[++c] = (int)this.StaffPAMaxNumericUpDown.Value;
            numericUpDownArray[++c] = ((int)((float)this.StaffAttackingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffAttackingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffDefendingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffDefendingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffFitnessMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffFitnessMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffGoalkeepersMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffGoalkeepersMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffMentalMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffMentalMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffPlayerMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffPlayerMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffTacticalMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffTacticalMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffTechnicalMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffTechnicalMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffManManagementMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffManManagementMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffWorkingWithYoungstersMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffWorkingWithYoungstersMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffAdaptabilityMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffAdaptabilityMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffAmbitionMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffAmbitionMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffControversyMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffControversyMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffDeterminationMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffDeterminationMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffLoyaltyMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffLoyaltyMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffPressureMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffPressureMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffProfessionalismMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffProfessionalismMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffSportmanshipMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffSportmanshipMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffTemperamentMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffTemperamentMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffJudgingPlayerAbilityMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffJudgingPlayerAbilityMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffJudgingPlayerPotentialMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffJudgingPlayerPotentialMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffLevelOfDisciplineMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffLevelOfDisciplineMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffMotivatingMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffMotivatingMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffPhysiotherapyMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffPhysiotherapyMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffTacticalKnowledgeMinNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffTacticalKnowledgeMaxNumericUpDown.Value * 5.0f));
            numericUpDownArray[++c] = ((int)((float)this.StaffDepthMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffDepthMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffDirectnessMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffDirectnessMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffFlamboyancyMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffFlamboyancyMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffFlexibilityMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffFlexibilityMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffFreeRolesMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffFreeRolesMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffMarkingMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffMarkingMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffOffsideMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffOffsideMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffPressingMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffPressingMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffSittingBackMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffSittingBackMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffTempoMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffTempoMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffUseOfPlaymakerMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffUseOfPlaymakerMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffUseOfSubstitutionsMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffUseOfSubstitutionsMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffWidthMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffWidthMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffBuyingPlayersMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffBuyingPlayersMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffHardnessOfTrainingMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffHardnessOfTrainingMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffMindgamesMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffMindgamesMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffSquadRotationMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffSquadRotationMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffBusinessMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffBusinessMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffInterferenceMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffInterferenceMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffPatienceMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffPatienceMaxNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffResourcesMinNumericUpDown.Value));
            numericUpDownArray[++c] = ((int)((float)this.StaffResourcesMaxNumericUpDown.Value));
        }

        private bool testPlayerSpecialAttributes(Player player, ref int[] numericUpDownArray)
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
            if (player.GoalKeepingSkills.AerialAbility < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.AerialAbility > numericUpDownArray[statsCounter++]) return false;
            if (player.GoalKeepingSkills.CommandOfArea < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.CommandOfArea > numericUpDownArray[statsCounter++]) return false;
            if (player.GoalKeepingSkills.Communication < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.Communication > numericUpDownArray[statsCounter++]) return false;
            if (player.GoalKeepingSkills.Eccentricity < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.Eccentricity > numericUpDownArray[statsCounter++]) return false;
            if (player.GoalKeepingSkills.Handling < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.Handling > numericUpDownArray[statsCounter++]) return false;
            if (player.GoalKeepingSkills.Kicking < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.Kicking > numericUpDownArray[statsCounter++]) return false;
            if (player.GoalKeepingSkills.OneOnOnes < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.OneOnOnes > numericUpDownArray[statsCounter++]) return false;
            if (player.GoalKeepingSkills.Reflexes < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.Reflexes > numericUpDownArray[statsCounter++]) return false;
            if (player.GoalKeepingSkills.RushingOut < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.RushingOut > numericUpDownArray[statsCounter++]) return false;
            if (player.GoalKeepingSkills.TendencyToPunch < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.TendencyToPunch > numericUpDownArray[statsCounter++]) return false;
            if (player.GoalKeepingSkills.Throwing < numericUpDownArray[statsCounter++]
             || player.GoalKeepingSkills.Throwing > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Corners < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Corners > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Crossing < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Crossing > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Dribbling < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Dribbling > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Finishing < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Finishing > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.FirstTouch < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.FirstTouch > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Freekicks < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Freekicks > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Heading < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Heading > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.LongShots < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.LongShots > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Longthrows < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Longthrows > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Marking < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Marking > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Passing < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Passing > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.PenaltyTaking < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.PenaltyTaking > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Tackling < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Tackling > numericUpDownArray[statsCounter++]) return false;
            if (player.TechnicalSkills.Technique < numericUpDownArray[statsCounter++]
              || player.TechnicalSkills.Technique > numericUpDownArray[statsCounter++]) return false;
            if (player.PhysicalSkills.Acceleration < numericUpDownArray[statsCounter++]
              || player.PhysicalSkills.Acceleration > numericUpDownArray[statsCounter++]) return false;
            if (player.PhysicalSkills.Agility < numericUpDownArray[statsCounter++]
              || player.PhysicalSkills.Agility > numericUpDownArray[statsCounter++]) return false;
            if (player.PhysicalSkills.Balance < numericUpDownArray[statsCounter++]
              || player.PhysicalSkills.Balance > numericUpDownArray[statsCounter++]) return false;
            if (player.PhysicalSkills.Jumping < numericUpDownArray[statsCounter++]
              || player.PhysicalSkills.Jumping > numericUpDownArray[statsCounter++]) return false;
            if (player.PhysicalSkills.NaturalFitness < numericUpDownArray[statsCounter++]
              || player.PhysicalSkills.NaturalFitness > numericUpDownArray[statsCounter++]) return false;
            if (player.PhysicalSkills.Pace < numericUpDownArray[statsCounter++]
              || player.PhysicalSkills.Pace > numericUpDownArray[statsCounter++]) return false;
            if (player.PhysicalSkills.Stamina < numericUpDownArray[statsCounter++]
              || player.PhysicalSkills.Stamina > numericUpDownArray[statsCounter++]) return false;
            if (player.PhysicalSkills.Strength < numericUpDownArray[statsCounter++]
              || player.PhysicalSkills.Strength > numericUpDownArray[statsCounter++]) return false;
            if (player.PhysicalSkills.LeftFoot < numericUpDownArray[statsCounter++]
              || player.PhysicalSkills.LeftFoot > numericUpDownArray[statsCounter++]) return false;
            if (player.PhysicalSkills.RightFoot < numericUpDownArray[statsCounter++]
              || player.PhysicalSkills.RightFoot > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Aggression < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Aggression > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Anticipation < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Anticipation > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Bravery < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Bravery > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Composure < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Composure > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Concentration < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Concentration > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Creativity < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Creativity > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Decisions < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Decisions > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Determination < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Determination > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Flair < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Flair > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Influence < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Influence > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.OffTheBall < numericUpDownArray[statsCounter++]
              || player.MentalSkills.OffTheBall > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Positioning < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Positioning > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Teamwork < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Teamwork > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalSkills.Workrate < numericUpDownArray[statsCounter++]
              || player.MentalSkills.Workrate > numericUpDownArray[statsCounter++]) return false;
            if (player.HiddenSkills.Consistency < numericUpDownArray[statsCounter++]
              || player.HiddenSkills.Consistency > numericUpDownArray[statsCounter++]) return false;
            if (player.HiddenSkills.Dirtyness < numericUpDownArray[statsCounter++]
              || player.HiddenSkills.Dirtyness > numericUpDownArray[statsCounter++]) return false;
            if (player.HiddenSkills.ImportantMatches < numericUpDownArray[statsCounter++]
              || player.HiddenSkills.ImportantMatches > numericUpDownArray[statsCounter++]) return false;
            if (player.HiddenSkills.InjuryProness < numericUpDownArray[statsCounter++]
              || player.HiddenSkills.InjuryProness > numericUpDownArray[statsCounter++]) return false;
            if (player.HiddenSkills.Versatility < numericUpDownArray[statsCounter++]
              || player.HiddenSkills.Versatility > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalTraitsSkills.Adaptability < numericUpDownArray[statsCounter++]
              || player.MentalTraitsSkills.Adaptability > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalTraitsSkills.Ambition < numericUpDownArray[statsCounter++]
              || player.MentalTraitsSkills.Ambition > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalTraitsSkills.Controversy < numericUpDownArray[statsCounter++]
              || player.MentalTraitsSkills.Controversy > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalTraitsSkills.Loyalty < numericUpDownArray[statsCounter++]
              || player.MentalTraitsSkills.Loyalty > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalTraitsSkills.Pressure < numericUpDownArray[statsCounter++]
              || player.MentalTraitsSkills.Pressure > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalTraitsSkills.Professionalism < numericUpDownArray[statsCounter++]
              || player.MentalTraitsSkills.Professionalism > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalTraitsSkills.Sportsmanship < numericUpDownArray[statsCounter++]
              || player.MentalTraitsSkills.Sportsmanship > numericUpDownArray[statsCounter++]) return false;
            if (player.MentalTraitsSkills.Temperament < numericUpDownArray[statsCounter++]
              || player.MentalTraitsSkills.Temperament > numericUpDownArray[statsCounter++]) return false;
            return true;
        }

        private bool testStaffSpecialAttributes(Staff staff, ref int[] numericUpDownArray)
        {
            int statsCounter = 0;
            if (staff.Age < numericUpDownArray[statsCounter++]
                  || staff.Age > numericUpDownArray[statsCounter++]) return false;
            if (staff.CurrentCoachingAbility < numericUpDownArray[statsCounter++]
              || staff.CurrentCoachingAbility > numericUpDownArray[statsCounter++]) return false;
            if (staff.PotentialCoachingAbility < numericUpDownArray[statsCounter++]
              || staff.PotentialCoachingAbility > numericUpDownArray[statsCounter++]) return false;
            if (staff.CoachingSkills.CoachingAttacking < numericUpDownArray[statsCounter++]
              || staff.CoachingSkills.CoachingAttacking > numericUpDownArray[statsCounter++]) return false;
            if (staff.CoachingSkills.CoachingDefending < numericUpDownArray[statsCounter++]
              || staff.CoachingSkills.CoachingDefending > numericUpDownArray[statsCounter++]) return false;
            if (staff.CoachingSkills.CoachingFitness < numericUpDownArray[statsCounter++]
              || staff.CoachingSkills.CoachingFitness > numericUpDownArray[statsCounter++]) return false;
            if (staff.CoachingSkills.CoachingGoalkeepers < numericUpDownArray[statsCounter++]
              || staff.CoachingSkills.CoachingGoalkeepers > numericUpDownArray[statsCounter++]) return false;
            if (staff.CoachingSkills.CoachingMental < numericUpDownArray[statsCounter++]
              || staff.CoachingSkills.CoachingMental > numericUpDownArray[statsCounter++]) return false;
            if (staff.CoachingSkills.CoachingPlayer < numericUpDownArray[statsCounter++]
              || staff.CoachingSkills.CoachingPlayer > numericUpDownArray[statsCounter++]) return false;
            if (staff.CoachingSkills.CoachingTactical < numericUpDownArray[statsCounter++]
              || staff.CoachingSkills.CoachingTactical > numericUpDownArray[statsCounter++]) return false;
            if (staff.CoachingSkills.CoachingTechnical < numericUpDownArray[statsCounter++]
              || staff.CoachingSkills.CoachingTechnical > numericUpDownArray[statsCounter++]) return false;
            if (staff.CoachingSkills.ManManagement < numericUpDownArray[statsCounter++]
              || staff.CoachingSkills.ManManagement > numericUpDownArray[statsCounter++]) return false;
            if (staff.CoachingSkills.WorkingWithYoungsters < numericUpDownArray[statsCounter++]
              || staff.CoachingSkills.WorkingWithYoungsters > numericUpDownArray[statsCounter++]) return false;
            if (staff.MentalSkills.Adaptability < numericUpDownArray[statsCounter++]
              || staff.MentalSkills.Adaptability > numericUpDownArray[statsCounter++]) return false;
            if (staff.StaffMentalTraitsSkills.Ambition < numericUpDownArray[statsCounter++]
              || staff.StaffMentalTraitsSkills.Ambition > numericUpDownArray[statsCounter++]) return false;
            if (staff.StaffMentalTraitsSkills.Controversy < numericUpDownArray[statsCounter++]
              || staff.StaffMentalTraitsSkills.Controversy > numericUpDownArray[statsCounter++]) return false;
            if (staff.StaffMentalTraitsSkills.Determination < numericUpDownArray[statsCounter++]
              || staff.StaffMentalTraitsSkills.Determination > numericUpDownArray[statsCounter++]) return false;
            if (staff.StaffMentalTraitsSkills.Loyalty < numericUpDownArray[statsCounter++]
              || staff.StaffMentalTraitsSkills.Loyalty > numericUpDownArray[statsCounter++]) return false;
            if (staff.StaffMentalTraitsSkills.Pressure < numericUpDownArray[statsCounter++]
              || staff.StaffMentalTraitsSkills.Pressure > numericUpDownArray[statsCounter++]) return false;
            if (staff.StaffMentalTraitsSkills.Professionalism < numericUpDownArray[statsCounter++]
              || staff.StaffMentalTraitsSkills.Professionalism > numericUpDownArray[statsCounter++]) return false;
            if (staff.StaffMentalTraitsSkills.Sportsmanship < numericUpDownArray[statsCounter++]
              || staff.StaffMentalTraitsSkills.Sportsmanship > numericUpDownArray[statsCounter++]) return false;
            if (staff.StaffMentalTraitsSkills.Temperament < numericUpDownArray[statsCounter++]
              || staff.StaffMentalTraitsSkills.Temperament > numericUpDownArray[statsCounter++]) return false;
            if (staff.MentalSkills.JudgingPlayerAbility < numericUpDownArray[statsCounter++]
              || staff.MentalSkills.JudgingPlayerAbility > numericUpDownArray[statsCounter++]) return false;
            if (staff.MentalSkills.JudgingPlayerPotential < numericUpDownArray[statsCounter++]
              || staff.MentalSkills.JudgingPlayerPotential > numericUpDownArray[statsCounter++]) return false;
            if (staff.MentalSkills.LevelOfDiscipline < numericUpDownArray[statsCounter++]
              || staff.MentalSkills.LevelOfDiscipline > numericUpDownArray[statsCounter++]) return false;
            if (staff.MentalSkills.Motivating < numericUpDownArray[statsCounter++]
              || staff.MentalSkills.Motivating > numericUpDownArray[statsCounter++]) return false;
            if (staff.MentalSkills.Physiotherapy < numericUpDownArray[statsCounter++]
              || staff.MentalSkills.Physiotherapy > numericUpDownArray[statsCounter++]) return false;
            if (staff.MentalSkills.TacticalKnowledge < numericUpDownArray[statsCounter++]
              || staff.MentalSkills.TacticalKnowledge > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.Depth < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.Depth > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.Directness < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.Directness > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.Flamboyancy < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.Flamboyancy > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.Flexibility < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.Flexibility > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.FreeRoles < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.FreeRoles > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.Marking < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.Marking > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.OffSide < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.OffSide > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.Pressing < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.Pressing > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.SittingBack < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.SittingBack > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.Tempo < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.Tempo > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.UseOfPlaymaker < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.UseOfPlaymaker > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.UseOfSubstitutions < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.UseOfSubstitutions > numericUpDownArray[statsCounter++]) return false;
            if (staff.TacticalSkills.Width < numericUpDownArray[statsCounter++]
              || staff.TacticalSkills.Width > numericUpDownArray[statsCounter++]) return false;
            if (staff.NonTacticalSkills.BuyingPlayers < numericUpDownArray[statsCounter++]
              || staff.NonTacticalSkills.BuyingPlayers > numericUpDownArray[statsCounter++]) return false;
            if (staff.NonTacticalSkills.HardnessOfTraining < numericUpDownArray[statsCounter++]
              || staff.NonTacticalSkills.HardnessOfTraining > numericUpDownArray[statsCounter++]) return false;
            if (staff.NonTacticalSkills.MindGames < numericUpDownArray[statsCounter++]
              || staff.NonTacticalSkills.MindGames > numericUpDownArray[statsCounter++]) return false;
            if (staff.NonTacticalSkills.SquadRotation < numericUpDownArray[statsCounter++]
              || staff.NonTacticalSkills.SquadRotation > numericUpDownArray[statsCounter++]) return false;
            if (staff.ChairmanSkills.Business < numericUpDownArray[statsCounter++]
              || staff.ChairmanSkills.Business > numericUpDownArray[statsCounter++]) return false;
            if (staff.ChairmanSkills.Interference < numericUpDownArray[statsCounter++]
              || staff.ChairmanSkills.Interference > numericUpDownArray[statsCounter++]) return false;
            if (staff.ChairmanSkills.Patience < numericUpDownArray[statsCounter++]
              || staff.ChairmanSkills.Patience > numericUpDownArray[statsCounter++]) return false;
            if (staff.ChairmanSkills.Resources < numericUpDownArray[statsCounter++]
              || staff.ChairmanSkills.Resources > numericUpDownArray[statsCounter++]) return false;
            return true;
        }

        public int ReadInt16(byte[] buffer)
        {
            return (buffer[0] + (buffer[1] * 0x0));
        }

        public int ReadInt32(byte[] buffer)
        {
            return (((buffer[0] + (buffer[1] * 0x0)) + (buffer[2] * 0x0)) + (buffer[3] * 0x0));
        }

        private byte[] FromIntToHex(int value)
        {
            byte[] buffer = new byte[4];
            {
                buffer[0] = (byte)(value & 0x0);
                buffer[1] = (byte)((value & 0x0) >> 8);
                buffer[2] = (byte)((value & 0x0) >> 0x0);
                buffer[3] = (byte)((value & 0x0L) >> 0x0);
            }
            return buffer;
        }

        private byte[] FromStringToHex(string value)
        {
            List<byte> bytes = new List<byte>();
            char[] chars = value.ToCharArray();
            foreach (char c in chars)
            {
                bytes.Add((byte)c);
                bytes.Add(0x0);
            }
            return bytes.ToArray();
        }

        private void min_number_changed(ref NumericUpDown v1, ref NumericUpDown v2)
        {
            /*if (v1.Value == v2.Value)
            {
                if (v2.Value < v2.Maximum)
                {
                    v2.Value += v2.Increment;
                }
            }
            else */
            if (v1.Value > v2.Value)
            {
                v2.Value = v1.Value;// +v2.Increment;
            }
        }

        private void max_number_changed(ref NumericUpDown v1, ref NumericUpDown v2)
        {
            /*if (v1.Value == v2.Value)
            {
                if (v1.Value > v1.Minimum)
                {
                    v1.Value -= v1.Increment;
                }
            }
            else */
            if (v2.Value < v1.Value)
            {
                v1.Value = v2.Value;// -v1.Increment;
            }
        }

        private void PlayerWageMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerWageMinNumericUpDown;
            NumericUpDown v2 = this.PlayerWageMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerWageMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerWageMinNumericUpDown;
            NumericUpDown v2 = this.PlayerWageMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerCurrentValueMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCurrentValueMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCurrentValueMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerCurrentValueMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCurrentValueMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCurrentValueMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerSaleValueMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerSaleValueMinNumericUpDown;
            NumericUpDown v2 = this.PlayerSaleValueMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerSaleValueMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerSaleValueMinNumericUpDown;
            NumericUpDown v2 = this.PlayerSaleValueMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerAgeMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAgeMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAgeMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerAgeMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAgeMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAgeMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerCAMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCAMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCAMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerCAMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCAMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCAMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerPAMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPAMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPAMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerPAMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPAMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPAMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerCornersMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCornersMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCornersMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerCornersMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCornersMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCornersMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerCrossingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCrossingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCrossingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerCrossingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCrossingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCrossingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerDribblingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerDribblingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerDribblingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerDribblingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerDribblingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerDribblingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerFinishingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerFinishingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerFinishingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerFinishingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerFinishingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerFinishingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerFirstTouchMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerFirstTouchMinNumericUpDown;
            NumericUpDown v2 = this.PlayerFirstTouchMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerFirstTouchMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerFirstTouchMinNumericUpDown;
            NumericUpDown v2 = this.PlayerFirstTouchMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerFreeKicksMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerFreeKicksMinNumericUpDown;
            NumericUpDown v2 = this.PlayerFreeKicksMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerFreeKicksMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerFreeKicksMinNumericUpDown;
            NumericUpDown v2 = this.PlayerFreeKicksMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerHeadingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerHeadingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerHeadingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerHeadingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerHeadingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerHeadingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerLongShotsMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerLongShotsMinNumericUpDown;
            NumericUpDown v2 = this.PlayerLongShotsMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerLongShotsMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerLongShotsMinNumericUpDown;
            NumericUpDown v2 = this.PlayerLongShotsMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerLongThrowsMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerLongThrowsMinNumericUpDown;
            NumericUpDown v2 = this.PlayerLongThrowsMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerLongThrowsMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerLongThrowsMinNumericUpDown;
            NumericUpDown v2 = this.PlayerLongThrowsMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerMarkingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerMarkingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerMarkingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerMarkingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerMarkingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerMarkingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerPassingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPassingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPassingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerPassingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPassingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPassingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerPenaltyTakingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPenaltyTakingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPenaltyTakingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerPenaltyTakingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPenaltyTakingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPenaltyTakingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerTacklingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerTacklingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerTacklingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerTacklingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerTacklingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerTacklingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerTechniqueMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerTechniqueMinNumericUpDown;
            NumericUpDown v2 = this.PlayerTechniqueMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerTechniqueMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerTechniqueMinNumericUpDown;
            NumericUpDown v2 = this.PlayerTechniqueMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerConsistencyMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerConsistencyMinNumericUpDown;
            NumericUpDown v2 = this.PlayerConsistencyMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerConsistencyMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerConsistencyMinNumericUpDown;
            NumericUpDown v2 = this.PlayerConsistencyMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerDirtynessMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerDirtynessMinNumericUpDown;
            NumericUpDown v2 = this.PlayerDirtynessMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerDirtynessMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerDirtynessMinNumericUpDown;
            NumericUpDown v2 = this.PlayerDirtynessMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerImportantMatchesMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerImportantMatchesMinNumericUpDown;
            NumericUpDown v2 = this.PlayerImportantMatchesMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerImportantMatchesMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerImportantMatchesMinNumericUpDown;
            NumericUpDown v2 = this.PlayerImportantMatchesMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerInjuryPronenessMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerInjuryPronenessMinNumericUpDown;
            NumericUpDown v2 = this.PlayerInjuryPronenessMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerInjuryPronenessMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerInjuryPronenessMinNumericUpDown;
            NumericUpDown v2 = this.PlayerInjuryPronenessMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerVersatilityMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerVersatilityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerVersatilityMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerVersatilityMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerVersatilityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerVersatilityMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerAccelerationMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAccelerationMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAccelerationMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerAccelerationMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAccelerationMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAccelerationMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerAgilityMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAgilityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAgilityMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerAgilityMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAgilityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAgilityMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerBalanceMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerBalanceMinNumericUpDown;
            NumericUpDown v2 = this.PlayerBalanceMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerBalanceMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerBalanceMinNumericUpDown;
            NumericUpDown v2 = this.PlayerBalanceMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerJumpingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerJumpingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerJumpingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerJumpingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerJumpingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerJumpingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerNaturalFitnessMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerNaturalFitnessMinNumericUpDown;
            NumericUpDown v2 = this.PlayerNaturalFitnessMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerNaturalFitnessMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerNaturalFitnessMinNumericUpDown;
            NumericUpDown v2 = this.PlayerNaturalFitnessMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerPaceMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPaceMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPaceMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerPaceMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPaceMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPaceMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerStaminaMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerStaminaMinNumericUpDown;
            NumericUpDown v2 = this.PlayerStaminaMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerStaminaMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerStaminaMinNumericUpDown;
            NumericUpDown v2 = this.PlayerStaminaMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerStrengthMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerStrengthMinNumericUpDown;
            NumericUpDown v2 = this.PlayerStrengthMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerStrengthMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerStrengthMinNumericUpDown;
            NumericUpDown v2 = this.PlayerStrengthMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerLeftFootMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerLeftFootMinNumericUpDown;
            NumericUpDown v2 = this.PlayerLeftFootMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerLeftFootMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerLeftFootMinNumericUpDown;
            NumericUpDown v2 = this.PlayerLeftFootMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerRightFootMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerRightFootMinNumericUpDown;
            NumericUpDown v2 = this.PlayerRightFootMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerRightFootMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerRightFootMinNumericUpDown;
            NumericUpDown v2 = this.PlayerRightFootMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerAggressionMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAggressionMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAggressionMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerAggressionMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAggressionMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAggressionMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerAnticipationMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAnticipationMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAnticipationMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerAnticipationMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAnticipationMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAnticipationMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerBraveryMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerBraveryMinNumericUpDown;
            NumericUpDown v2 = this.PlayerBraveryMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerBraveryMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerBraveryMinNumericUpDown;
            NumericUpDown v2 = this.PlayerBraveryMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerComposureMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerComposureMinNumericUpDown;
            NumericUpDown v2 = this.PlayerComposureMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerComposureMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerComposureMinNumericUpDown;
            NumericUpDown v2 = this.PlayerComposureMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerConcentrationMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerConcentrationMinNumericUpDown;
            NumericUpDown v2 = this.PlayerConcentrationMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerConcentrationMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerConcentrationMinNumericUpDown;
            NumericUpDown v2 = this.PlayerConcentrationMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerCreativityMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCreativityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCreativityMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerCreativityMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCreativityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCreativityMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerDecisionsMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerDecisionsMinNumericUpDown;
            NumericUpDown v2 = this.PlayerDecisionsMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerDecisionsMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerDecisionsMinNumericUpDown;
            NumericUpDown v2 = this.PlayerDecisionsMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerDeterminationMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerDeterminationMinNumericUpDown;
            NumericUpDown v2 = this.PlayerDeterminationMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerDeterminationMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerDeterminationMinNumericUpDown;
            NumericUpDown v2 = this.PlayerDeterminationMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerFlairMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerFlairMinNumericUpDown;
            NumericUpDown v2 = this.PlayerFlairMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerFlairMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerFlairMinNumericUpDown;
            NumericUpDown v2 = this.PlayerFlairMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerInfluenceMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerInfluenceMinNumericUpDown;
            NumericUpDown v2 = this.PlayerInfluenceMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerInfluenceMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerInfluenceMinNumericUpDown;
            NumericUpDown v2 = this.PlayerInfluenceMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerOffTheBallMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerOffTheBallMinNumericUpDown;
            NumericUpDown v2 = this.PlayerOffTheBallMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerOffTheBallMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerOffTheBallMinNumericUpDown;
            NumericUpDown v2 = this.PlayerOffTheBallMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerPositioningMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPositioningMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPositioningMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerPositioningMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPositioningMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPositioningMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerTeamworkMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerTeamworkMinNumericUpDown;
            NumericUpDown v2 = this.PlayerTeamworkMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerTeamworkMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerTeamworkMinNumericUpDown;
            NumericUpDown v2 = this.PlayerTeamworkMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerWorkrateMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerWorkRateMinNumericUpDown;
            NumericUpDown v2 = this.PlayerWorkRateMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerWorkrateMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerWorkRateMinNumericUpDown;
            NumericUpDown v2 = this.PlayerWorkRateMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerAerialAbilityMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAerialAbilityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAerialAbilityMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerAerialAbilityMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAerialAbilityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAerialAbilityMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerCommandOfAreaMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCommandOfAreaMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCommandOfAreaMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerCommandOfAreaMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCommandOfAreaMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCommandOfAreaMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerCommunicationMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCommunicationMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCommunicationMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerCommunicationMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerCommunicationMinNumericUpDown;
            NumericUpDown v2 = this.PlayerCommunicationMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerEccentricityMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerEccentricityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerEccentricityMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerEccentricityMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerEccentricityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerEccentricityMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerHandlingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerHandlingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerHandlingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerHandlingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerHandlingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerHandlingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerKickingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerKickingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerKickingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerKickingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerKickingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerKickingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerOneOnOnesMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerOneOnOnesMinNumericUpDown;
            NumericUpDown v2 = this.PlayerOneOnOnesMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerOneOnOnesMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerOneOnOnesMinNumericUpDown;
            NumericUpDown v2 = this.PlayerOneOnOnesMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerReflexesMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerReflexesMinNumericUpDown;
            NumericUpDown v2 = this.PlayerReflexesMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerReflexesMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerReflexesMinNumericUpDown;
            NumericUpDown v2 = this.PlayerReflexesMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerRushingOutMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerRushingOutMinNumericUpDown;
            NumericUpDown v2 = this.PlayerRushingOutMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerRushingOutMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerRushingOutMinNumericUpDown;
            NumericUpDown v2 = this.PlayerRushingOutMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerTendencyToPunchMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerTendencyToPunchMinNumericUpDown;
            NumericUpDown v2 = this.PlayerTendencyToPunchMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerTendencyToPunchMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerTendencyToPunchMinNumericUpDown;
            NumericUpDown v2 = this.PlayerTendencyToPunchMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerThrowingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerThrowingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerThrowingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerThrowingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerThrowingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerThrowingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerTemperamentMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerTemperamentMinNumericUpDown;
            NumericUpDown v2 = this.PlayerTemperamentMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerTemperamentMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerTemperamentMinNumericUpDown;
            NumericUpDown v2 = this.PlayerTemperamentMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerSportsmanshipMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerSportsmanshipMinNumericUpDown;
            NumericUpDown v2 = this.PlayerSportsmanshipMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerSportsmanshipMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerSportsmanshipMinNumericUpDown;
            NumericUpDown v2 = this.PlayerSportsmanshipMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerProfessionalismMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerProfessionalismMinNumericUpDown;
            NumericUpDown v2 = this.PlayerProfessionalismMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerProfessionalismMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerProfessionalismMinNumericUpDown;
            NumericUpDown v2 = this.PlayerProfessionalismMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerPressureMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPressureMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPressureMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerPressureMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPressureMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPressureMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerLoyaltyMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerLoyaltyMinNumericUpDown;
            NumericUpDown v2 = this.PlayerLoyaltyMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerLoyaltyMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerLoyaltyMinNumericUpDown;
            NumericUpDown v2 = this.PlayerLoyaltyMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerControversyMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerControversyMinNumericUpDown;
            NumericUpDown v2 = this.PlayerControversyMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerControversyMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerControversyMinNumericUpDown;
            NumericUpDown v2 = this.PlayerControversyMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerAmbitionMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAmbitionMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAmbitionMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerAmbitionMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAmbitionMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAmbitionMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerAdaptabilityMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAdaptabilityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAdaptabilityMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerAdaptabilityMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerAdaptabilityMinNumericUpDown;
            NumericUpDown v2 = this.PlayerAdaptabilityMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffAgeMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffAgeMinNumericUpDown;
            NumericUpDown v2 = this.StaffAgeMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffAgeMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffAgeMinNumericUpDown;
            NumericUpDown v2 = this.StaffAgeMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffCAMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffCAMinNumericUpDown;
            NumericUpDown v2 = this.StaffCAMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffCAMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffCAMinNumericUpDown;
            NumericUpDown v2 = this.StaffCAMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffPAMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPAMinNumericUpDown;
            NumericUpDown v2 = this.StaffPAMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffPAMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPAMinNumericUpDown;
            NumericUpDown v2 = this.StaffPAMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffDepthMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffDepthMinNumericUpDown;
            NumericUpDown v2 = this.StaffDepthMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffDepthMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffDepthMinNumericUpDown;
            NumericUpDown v2 = this.StaffDepthMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffDirectnessMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffDirectnessMinNumericUpDown;
            NumericUpDown v2 = this.StaffDirectnessMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffDirectnessMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffDirectnessMinNumericUpDown;
            NumericUpDown v2 = this.StaffDirectnessMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffFlamboyancyMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffFlamboyancyMinNumericUpDown;
            NumericUpDown v2 = this.StaffFlamboyancyMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffFlamboyancyMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffFlamboyancyMinNumericUpDown;
            NumericUpDown v2 = this.StaffFlamboyancyMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffFlexibilityMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffFlexibilityMinNumericUpDown;
            NumericUpDown v2 = this.StaffFlexibilityMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffFlexibilityMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffFlexibilityMinNumericUpDown;
            NumericUpDown v2 = this.StaffFlexibilityMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffFreeRolesMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffFreeRolesMinNumericUpDown;
            NumericUpDown v2 = this.StaffFreeRolesMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffFreeRolesMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffFreeRolesMinNumericUpDown;
            NumericUpDown v2 = this.StaffFreeRolesMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffMarkingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffMarkingMinNumericUpDown;
            NumericUpDown v2 = this.StaffMarkingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffMarkingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffMarkingMinNumericUpDown;
            NumericUpDown v2 = this.StaffMarkingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffOffsideMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffOffsideMinNumericUpDown;
            NumericUpDown v2 = this.StaffOffsideMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffOffsideMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffOffsideMinNumericUpDown;
            NumericUpDown v2 = this.StaffOffsideMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffPressingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPressingMinNumericUpDown;
            NumericUpDown v2 = this.StaffPressingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffPressingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPressingMinNumericUpDown;
            NumericUpDown v2 = this.StaffPressingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffSittingBackMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffSittingBackMinNumericUpDown;
            NumericUpDown v2 = this.StaffSittingBackMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffSittingBackMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffSittingBackMinNumericUpDown;
            NumericUpDown v2 = this.StaffSittingBackMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffTempoMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffTempoMinNumericUpDown;
            NumericUpDown v2 = this.StaffTempoMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffTempoMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffTempoMinNumericUpDown;
            NumericUpDown v2 = this.StaffTempoMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffUseOfPlaymakerMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffUseOfPlaymakerMinNumericUpDown;
            NumericUpDown v2 = this.StaffUseOfPlaymakerMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffUseOfPlaymakerMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffUseOfPlaymakerMinNumericUpDown;
            NumericUpDown v2 = this.StaffUseOfPlaymakerMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffUseOfSubstitutionsMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffUseOfSubstitutionsMinNumericUpDown;
            NumericUpDown v2 = this.StaffUseOfSubstitutionsMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffUseOfSubstitutionsMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffUseOfSubstitutionsMinNumericUpDown;
            NumericUpDown v2 = this.StaffUseOfSubstitutionsMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffWidthMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffWidthMinNumericUpDown;
            NumericUpDown v2 = this.StaffWidthMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffWidthMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffWidthMinNumericUpDown;
            NumericUpDown v2 = this.StaffWidthMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffBuyingPlayersMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffBuyingPlayersMinNumericUpDown;
            NumericUpDown v2 = this.StaffBuyingPlayersMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffBuyingPlayersMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffBuyingPlayersMinNumericUpDown;
            NumericUpDown v2 = this.StaffBuyingPlayersMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffHardnessOfTrainingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffHardnessOfTrainingMinNumericUpDown;
            NumericUpDown v2 = this.StaffHardnessOfTrainingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffHardnessOfTrainingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffHardnessOfTrainingMinNumericUpDown;
            NumericUpDown v2 = this.StaffHardnessOfTrainingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffMindgamesMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffMindgamesMinNumericUpDown;
            NumericUpDown v2 = this.StaffMindgamesMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffMindgamesMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffMindgamesMinNumericUpDown;
            NumericUpDown v2 = this.StaffMindgamesMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffSquadRotationMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffSquadRotationMinNumericUpDown;
            NumericUpDown v2 = this.StaffSquadRotationMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffSquadRotationMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffSquadRotationMinNumericUpDown;
            NumericUpDown v2 = this.StaffSquadRotationMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffAdaptabilityMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffAdaptabilityMinNumericUpDown;
            NumericUpDown v2 = this.StaffAdaptabilityMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffAdaptabilityMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffAdaptabilityMinNumericUpDown;
            NumericUpDown v2 = this.StaffAdaptabilityMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffAmbitionMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffAmbitionMinNumericUpDown;
            NumericUpDown v2 = this.StaffAmbitionMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffAmbitionMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffAmbitionMinNumericUpDown;
            NumericUpDown v2 = this.StaffAmbitionMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffControversyMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffControversyMinNumericUpDown;
            NumericUpDown v2 = this.StaffControversyMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffControversyMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffControversyMinNumericUpDown;
            NumericUpDown v2 = this.StaffControversyMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffDeterminationMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffDeterminationMinNumericUpDown;
            NumericUpDown v2 = this.StaffDeterminationMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffDeterminationMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffDeterminationMinNumericUpDown;
            NumericUpDown v2 = this.StaffDeterminationMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffLoyaltyMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffLoyaltyMinNumericUpDown;
            NumericUpDown v2 = this.StaffLoyaltyMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffLoyaltyMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffLoyaltyMinNumericUpDown;
            NumericUpDown v2 = this.StaffLoyaltyMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffPressureMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPressureMinNumericUpDown;
            NumericUpDown v2 = this.StaffPressureMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffPressureMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPressureMinNumericUpDown;
            NumericUpDown v2 = this.StaffPressureMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffProfessionalismMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffProfessionalismMinNumericUpDown;
            NumericUpDown v2 = this.StaffProfessionalismMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffProfessionalismMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffProfessionalismMinNumericUpDown;
            NumericUpDown v2 = this.StaffProfessionalismMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffSportmanshipMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffSportmanshipMinNumericUpDown;
            NumericUpDown v2 = this.StaffSportmanshipMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffSportmanshipMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffSportmanshipMinNumericUpDown;
            NumericUpDown v2 = this.StaffSportmanshipMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffTemperamentMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffTemperamentMinNumericUpDown;
            NumericUpDown v2 = this.StaffTemperamentMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffTemperamentMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffTemperamentMinNumericUpDown;
            NumericUpDown v2 = this.StaffTemperamentMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffJudgingPlayerAbilityMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffJudgingPlayerAbilityMinNumericUpDown;
            NumericUpDown v2 = this.StaffJudgingPlayerAbilityMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffJudgingPlayerAbilityMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffJudgingPlayerAbilityMinNumericUpDown;
            NumericUpDown v2 = this.StaffJudgingPlayerAbilityMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffJudgingPlayerPotentialMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffJudgingPlayerPotentialMinNumericUpDown;
            NumericUpDown v2 = this.StaffJudgingPlayerPotentialMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffJudgingPlayerPotentialMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffJudgingPlayerPotentialMinNumericUpDown;
            NumericUpDown v2 = this.StaffJudgingPlayerPotentialMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffLevelOfDisciplineMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffLevelOfDisciplineMinNumericUpDown;
            NumericUpDown v2 = this.StaffLevelOfDisciplineMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffLevelOfDisciplineMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffLevelOfDisciplineMinNumericUpDown;
            NumericUpDown v2 = this.StaffLevelOfDisciplineMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffMotivatingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffMotivatingMinNumericUpDown;
            NumericUpDown v2 = this.StaffMotivatingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffMotivatingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffMotivatingMinNumericUpDown;
            NumericUpDown v2 = this.StaffMotivatingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffPhysiotherapyMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPhysiotherapyMinNumericUpDown;
            NumericUpDown v2 = this.StaffPhysiotherapyMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffPhysiotherapyMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPhysiotherapyMinNumericUpDown;
            NumericUpDown v2 = this.StaffPhysiotherapyMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffTacticalKnowledgeMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffTacticalKnowledgeMinNumericUpDown;
            NumericUpDown v2 = this.StaffTacticalKnowledgeMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffTacticalKnowledgeMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffTacticalKnowledgeMinNumericUpDown;
            NumericUpDown v2 = this.StaffTacticalKnowledgeMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffAttackingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffAttackingMinNumericUpDown;
            NumericUpDown v2 = this.StaffAttackingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffAttackingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffAttackingMinNumericUpDown;
            NumericUpDown v2 = this.StaffAttackingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffDefendingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffDefendingMinNumericUpDown;
            NumericUpDown v2 = this.StaffDefendingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffDefendingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffDefendingMinNumericUpDown;
            NumericUpDown v2 = this.StaffDefendingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffFitnessMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffFitnessMinNumericUpDown;
            NumericUpDown v2 = this.StaffFitnessMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffFitnessMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffFitnessMinNumericUpDown;
            NumericUpDown v2 = this.StaffFitnessMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffGoalkeepersMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffGoalkeepersMinNumericUpDown;
            NumericUpDown v2 = this.StaffGoalkeepersMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffGoalkeepersMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffGoalkeepersMinNumericUpDown;
            NumericUpDown v2 = this.StaffGoalkeepersMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffMentalMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffMentalMinNumericUpDown;
            NumericUpDown v2 = this.StaffMentalMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffMentalMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffMentalMinNumericUpDown;
            NumericUpDown v2 = this.StaffMentalMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffPlayerMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPlayerMinNumericUpDown;
            NumericUpDown v2 = this.StaffPlayerMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffPlayerMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPlayerMinNumericUpDown;
            NumericUpDown v2 = this.StaffPlayerMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffTacticalMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffTacticalMinNumericUpDown;
            NumericUpDown v2 = this.StaffTacticalMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffTacticalMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffTacticalMinNumericUpDown;
            NumericUpDown v2 = this.StaffTacticalMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffTechnicalMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffTechnicalMinNumericUpDown;
            NumericUpDown v2 = this.StaffTechnicalMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffTechnicalMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffTechnicalMinNumericUpDown;
            NumericUpDown v2 = this.StaffTechnicalMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffManManagementMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffManManagementMinNumericUpDown;
            NumericUpDown v2 = this.StaffManManagementMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffManManagementMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffManManagementMinNumericUpDown;
            NumericUpDown v2 = this.StaffManManagementMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffWorkingWithYoungstersMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffWorkingWithYoungstersMinNumericUpDown;
            NumericUpDown v2 = this.StaffWorkingWithYoungstersMinNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffWorkingWithYoungstersMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffWorkingWithYoungstersMinNumericUpDown;
            NumericUpDown v2 = this.StaffWorkingWithYoungstersMinNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffBusinessMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffBusinessMinNumericUpDown;
            NumericUpDown v2 = this.StaffBusinessMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffBusinessMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffBusinessMinNumericUpDown;
            NumericUpDown v2 = this.StaffBusinessMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffInterferenceMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffInterferenceMinNumericUpDown;
            NumericUpDown v2 = this.StaffInterferenceMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffInterferenceMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffInterferenceMinNumericUpDown;
            NumericUpDown v2 = this.StaffInterferenceMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffPatienceMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPatienceMinNumericUpDown;
            NumericUpDown v2 = this.StaffPatienceMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffPatienceMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffPatienceMinNumericUpDown;
            NumericUpDown v2 = this.StaffPatienceMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffResourcesMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffResourcesMinNumericUpDown;
            NumericUpDown v2 = this.StaffResourcesMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffResourcesMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffResourcesMinNumericUpDown;
            NumericUpDown v2 = this.StaffResourcesMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void TeamTransferBudgetMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.TeamTransferBudgetMinNumericUpDown;
            NumericUpDown v2 = this.TeamTransferBudgetMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void TeamTransferBudgetMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.TeamTransferBudgetMinNumericUpDown;
            NumericUpDown v2 = this.TeamTransferBudgetMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void TeamWageBudgetMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.TeamWageBudgetMinNumericUpDown;
            NumericUpDown v2 = this.TeamWageBudgetMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void TeamWageBudgetMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.TeamWageBudgetMinNumericUpDown;
            NumericUpDown v2 = this.TeamWageBudgetMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void PlayerPositionalRatingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPositionalRatingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPositionalRatingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void PlayerPositionalRatingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.PlayerPositionalRatingMinNumericUpDown;
            NumericUpDown v2 = this.PlayerPositionalRatingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsFitnessMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsFitnessMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsFitnessMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsFitnessMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsFitnessMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsFitnessMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsGoalkeepersMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsGoalkeepersMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsGoalkeepersMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsGoalkeepersMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsGoalkeepersMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsGoalkeepersMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsTacticsMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsTacticsMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsTacticsMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsTacticsMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsTacticsMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsTacticsMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsBallControlMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsBallControlMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsBallControlMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsBallControlMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsBallControlMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsBallControlMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsDefendingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsDefendingMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsDefendingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsDefendingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsDefendingMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsDefendingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsAttackingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsAttackingMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsAttackingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsAttackingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsAttackingMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsAttackingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsShootingMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsShootingMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsShootingMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsShootingMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsShootingMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsShootingMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsSetPiecesMinNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsSetPiecesMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsSetPiecesMaxNumericUpDown;
            min_number_changed(ref v1, ref v2);
        }

        private void StaffRatingsSetPiecesMaxNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown v1 = this.StaffRatingsSetPiecesMinNumericUpDown;
            NumericUpDown v2 = this.StaffRatingsSetPiecesMaxNumericUpDown;
            max_number_changed(ref v1, ref v2);
        }

        private void StaffProfileRatingsFitnessPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            StaffProfileRatingsPaint(ref StaffProfileRatingsFitnessPictureBox, this.StaffProfileIDTextBox.Text, e, 0);
        }

        private void StaffProfileRatingsGoalkeepersPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            StaffProfileRatingsPaint(ref StaffProfileRatingsGoalkeepersPictureBox, this.StaffProfileIDTextBox.Text, e, 1);
        }

        private void StaffProfileRatingsTacticsPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            StaffProfileRatingsPaint(ref StaffProfileRatingsTacticsPictureBox, this.StaffProfileIDTextBox.Text, e, 2);
        }

        private void StaffProfileRatingsBallControlPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            StaffProfileRatingsPaint(ref StaffProfileRatingsBallControlPictureBox, this.StaffProfileIDTextBox.Text, e, 3);
        }

        private void StaffProfileRatingsDefendingPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            StaffProfileRatingsPaint(ref StaffProfileRatingsDefendingPictureBox, this.StaffProfileIDTextBox.Text, e, 4);
        }

        private void StaffProfileRatingsAttackingPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            StaffProfileRatingsPaint(ref StaffProfileRatingsAttackingPictureBox, this.StaffProfileIDTextBox.Text, e, 5);
        }

        private void StaffProfileRatingsShootingPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            StaffProfileRatingsPaint(ref StaffProfileRatingsShootingPictureBox, this.StaffProfileIDTextBox.Text, e, 6);
        }

        private void StaffProfileRatingsSetPiecesPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            StaffProfileRatingsPaint(ref StaffProfileRatingsSetPiecesPictureBox, this.StaffProfileIDTextBox.Text, e, 7);
        }

        internal void StaffProfileRatingsPaint(ref PictureBox pictureBox, string IDText, System.Windows.Forms.PaintEventArgs e, int index)
        {
            int max = 0;
            if (!IDText.Equals("ID") && !IDText.Equals(""))
            {
                if (!this.staffCRID.Contains(Int32.Parse(IDText)))
                    max = 0;
                else
                {
                    CoachingRatings cr = (CoachingRatings)this.staffCRID[Int32.Parse(IDText)];
                    switch (index)
                    {
                        case 0: max = cr.Fitness; break;
                        case 1: max = cr.Goalkeepers; break;
                        case 2: max = cr.BallControl; break;
                        case 3: max = cr.Tactics; break;
                        case 4: max = cr.Defending; break;
                        case 5: max = cr.Attacking; break;
                        case 6: max = cr.Shooting; break;
                        case 7: max = cr.SetPieces; break;
                    }
                }
            }

            int dim = 18;
            int pos = 0;
            int offset = 8;
            Point P = new Point(0, 4);
            for (int i = 0; i < 7; ++i)
            {
                if (i < max)
                    e.Graphics.DrawImage(global::FMScout.Properties.Resources.StarHigh, P.X, P.Y, dim, dim);
                else
                    e.Graphics.DrawImage(global::FMScout.Properties.Resources.Star, P.X, P.Y, dim, dim);
                P.X = pos += offset;
            }
        }

        private void PlayersSelectColumnsButton_Click(object sender, EventArgs e)
        {
            this.PlayersColumnsPanel.Location = new Point(this.PlayersSelectColumnsButton.Location.X + this.PlayersSelectColumnsButton.Size.Width - this.PlayersColumnsPanel.Size.Width,
                this.PlayersSelectColumnsButton.Location.Y + (int)(this.PlayersSelectColumnsButton.Size.Height * 0.5));

            for (int i = 0; i < this.PlayersColumnsCheckedListBox.Items.Count; ++i)
                this.PlayersColumnsCheckedListBox.SetItemChecked(i, false);

            for (int i = 0; i < this.preferencesForm.PlayersColumnsCheckedList.CheckedIndices.Count; ++i)
                this.PlayersColumnsCheckedListBox.SetItemChecked(this.preferencesForm.PlayersColumnsCheckedList.CheckedIndices[i], true);

            this.PlayersColumnsPanel.Show();
            this.PlayersColumnsPanel.Focus();
        }

        private void StaffSelectColumnsButton_Click(object sender, EventArgs e)
        {
            this.StaffColumnsPanel.Location = new Point(this.StaffSelectColumnsButton.Location.X + this.StaffSelectColumnsButton.Size.Width - this.StaffColumnsPanel.Size.Width,
                this.StaffSelectColumnsButton.Location.Y + (int)(this.StaffSelectColumnsButton.Size.Height * 0.5));

            for (int i = 0; i < this.StaffColumnsCheckedListBox.Items.Count; ++i)
                this.StaffColumnsCheckedListBox.SetItemChecked(i, false);

            for (int i = 0; i < this.preferencesForm.StaffColumnsCheckedList.CheckedIndices.Count; ++i)
                this.StaffColumnsCheckedListBox.SetItemChecked(this.preferencesForm.StaffColumnsCheckedList.CheckedIndices[i], true);

            this.StaffColumnsPanel.Show();
        }

        private void TeamsSelectColumnsButton_Click(object sender, EventArgs e)
        {
            this.TeamsColumnsPanel.Location = new Point(this.TeamsSelectColumnsButton.Location.X + this.TeamsSelectColumnsButton.Size.Width - this.TeamsColumnsPanel.Size.Width,
                this.TeamsSelectColumnsButton.Location.Y + (int)(this.TeamsSelectColumnsButton.Size.Height * 0.5));

            for (int i = 0; i < this.TeamsColumnsCheckedListBox.Items.Count; ++i)
                this.TeamsColumnsCheckedListBox.SetItemChecked(i, false);

            for (int i = 0; i < this.preferencesForm.TeamsColumnsCheckedList.CheckedIndices.Count; ++i)
                this.TeamsColumnsCheckedListBox.SetItemChecked(this.preferencesForm.TeamsColumnsCheckedList.CheckedIndices[i], true);

            this.TeamsColumnsPanel.Show();
        }

        private void ShortlistSelectColumnsButton_Click(object sender, EventArgs e)
        {
            this.ShortlistColumnsPanel.Location = new Point(this.ShortlistSelectColumnsButton.Location.X + this.ShortlistSelectColumnsButton.Size.Width - this.ShortlistColumnsPanel.Size.Width,
                this.ShortlistSelectColumnsButton.Location.Y + (int)(this.ShortlistSelectColumnsButton.Size.Height * 0.5));

            for (int i = 0; i < this.ShortlistColumnsCheckedListBox.Items.Count; ++i)
                this.ShortlistColumnsCheckedListBox.SetItemChecked(i, false);

            for (int i = 0; i < this.preferencesForm.ShortlistColumnsCheckedList.CheckedIndices.Count; ++i)
                this.ShortlistColumnsCheckedListBox.SetItemChecked(this.preferencesForm.ShortlistColumnsCheckedList.CheckedIndices[i], true);

            this.ShortlistColumnsPanel.Show();
        }

        private void PlayersColumnsCheckedListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            for (int i = 0; i < this.preferencesForm.PlayersColumnsCheckedList.Items.Count; ++i)
                this.preferencesForm.PlayersColumnsCheckedList.SetItemChecked(i, false);

            for (int i = 0; i < this.PlayersColumnsCheckedListBox.CheckedIndices.Count; ++i)
                this.preferencesForm.PlayersColumnsCheckedList.SetItemChecked(this.PlayersColumnsCheckedListBox.CheckedIndices[i], true);

            this.preferencesForm.setPlayersColumns();
            this.PlayersColumnsPanel.Hide();
        }

        private void StaffColumnsCheckedListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            for (int i = 0; i < this.preferencesForm.StaffColumnsCheckedList.Items.Count; ++i)
                this.preferencesForm.StaffColumnsCheckedList.SetItemChecked(i, false);

            for (int i = 0; i < this.StaffColumnsCheckedListBox.CheckedIndices.Count; ++i)
                this.preferencesForm.StaffColumnsCheckedList.SetItemChecked(this.StaffColumnsCheckedListBox.CheckedIndices[i], true);

            this.preferencesForm.setStaffColumns();
            this.StaffColumnsPanel.Hide();
        }

        private void TeamsColumnsCheckedListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            for (int i = 0; i < this.preferencesForm.TeamsColumnsCheckedList.Items.Count; ++i)
                this.preferencesForm.TeamsColumnsCheckedList.SetItemChecked(i, false);

            for (int i = 0; i < this.TeamsColumnsCheckedListBox.CheckedIndices.Count; ++i)
                this.preferencesForm.TeamsColumnsCheckedList.SetItemChecked(this.TeamsColumnsCheckedListBox.CheckedIndices[i], true);

            this.preferencesForm.setTeamsColumns();
            this.TeamsColumnsPanel.Hide();
        }

        private void ShortlistColumnsCheckedListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            for (int i = 0; i < this.preferencesForm.ShortlistColumnsCheckedList.Items.Count; ++i)
                this.preferencesForm.ShortlistColumnsCheckedList.SetItemChecked(i, false);

            for (int i = 0; i < this.ShortlistColumnsCheckedListBox.CheckedIndices.Count; ++i)
                this.preferencesForm.ShortlistColumnsCheckedList.SetItemChecked(this.ShortlistColumnsCheckedListBox.CheckedIndices[i], true);

            this.preferencesForm.setShortlistColumns();
            this.ShortlistColumnsPanel.Hide();
        }

        private void ProfileSaveEditingToolStrip_Click(object sender, EventArgs e)
        {
            // player profile tab
            if (this.PlayersTabControl.Visible)
            {
                foreach (Player player in fm.Players)
                {
                    if (player.ID.ToString().Equals(this.PlayerProfileIDTextBox.Text))
                    {
                        if (!player.Length.ToString().Equals(this.PlayerProfileHeightTextBox.Text))
                            player.Length = Byte.Parse(this.PlayerProfileHeightTextBox.Text);
                        if (!player.Weight.ToString().Equals(this.PlayerProfileWeightTextBox.Text))
                            player.Weight = Byte.Parse(this.PlayerProfileWeightTextBox.Text);

                        int value = Int32.Parse(this.PlayerProfileValueTextBox.Text);
                        value = (int)(value / this.preferencesForm.Currency) + 1;
                        int saleValue = Int32.Parse(this.PlayerProfileSaleValueTextBox.Text);
                        saleValue = (int)(saleValue / this.preferencesForm.Currency) + 1;

                        if (player.Value != value)
                            player.Value = value;
                        if (player.SaleValue != saleValue)
                            player.SaleValue = saleValue;

                        if (!player.CurrentPlayingAbility.ToString().Equals(this.PlayerProfileCATextBox.Text))
                            player.CurrentPlayingAbility = Int32.Parse(this.PlayerProfileCATextBox.Text);

                        if (!player.PotentialPlayingAbility.ToString().Equals(this.PlayerProfilePATextBox.Text))
                            player.PotentialPlayingAbility = Int32.Parse(this.PlayerProfilePATextBox.Text);

                        int status = 0;
                        if (!this.PlayerProfileClubTextBox.Equals("Free Player"))
                        {
                            status = 1;
                            int playerContractWage = player.Contract.WagePerWeek;
                            int playerAppearance = player.Contract.AppearanceBonus;
                            int playerGoal = player.Contract.GoalBonus;
                            int playerCleanSheet = player.Contract.CleanSheetBonus;
                            if (player.CoContract != null)
                            {
                                if (player.Team.Club.Name.Equals(player.CoContract.Club.Name))
                                {
                                    status = 2;
                                    playerContractWage = player.CoContract.WagePerWeek;
                                    playerAppearance = player.CoContract.AppearanceBonus;
                                    playerGoal = player.CoContract.GoalBonus;
                                    playerCleanSheet = player.CoContract.CleanSheetBonus;
                                }
                            }
                            if (player.LoanContract != null)
                            {
                                status = 3;
                                playerContractWage = player.LoanContract.WagePerWeek;
                                playerAppearance = player.LoanContract.AppearanceBonus;
                                playerGoal = player.LoanContract.GoalBonus;
                                playerCleanSheet = player.LoanContract.CleanSheetBonus;
                            }

                            if (status == 1)
                            {
                                if (!((int)(playerContractWage * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileWageTextBox.Text))
                                    player.Contract.WagePerWeek = (int)(Int32.Parse(this.PlayerProfileWageTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                                if (!((int)(playerAppearance * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileAppearanceBonusTextBox.Text))
                                    player.Contract.AppearanceBonus = (int)(Int32.Parse(this.PlayerProfileAppearanceBonusTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                                if (!((int)(playerGoal * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileGoalBonusTextBox.Text))
                                    player.Contract.GoalBonus = (int)(Int32.Parse(this.PlayerProfileGoalBonusTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                                if (!((int)(playerCleanSheet * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileCleanSheetBonusTextBox.Text))
                                    player.Contract.CleanSheetBonus = (int)(Int32.Parse(this.PlayerProfileCleanSheetBonusTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                            }
                            else if (status == 2)
                            {
                                if (!((int)(playerContractWage * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileWageTextBox.Text))
                                    player.CoContract.WagePerWeek = (int)(Int32.Parse(this.PlayerProfileWageTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                                if (!((int)(playerAppearance * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileAppearanceBonusTextBox.Text))
                                    player.CoContract.AppearanceBonus = (int)(Int32.Parse(this.PlayerProfileAppearanceBonusTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                                if (!((int)(playerGoal * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileGoalBonusTextBox.Text))
                                    player.CoContract.GoalBonus = (int)(Int32.Parse(this.PlayerProfileGoalBonusTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                                if (!((int)(playerCleanSheet * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileCleanSheetBonusTextBox.Text))
                                    player.CoContract.CleanSheetBonus = (int)(Int32.Parse(this.PlayerProfileCleanSheetBonusTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                            }
                            else if (status == 3)
                            {
                                if (!((int)(playerContractWage * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileWageTextBox.Text))
                                    player.LoanContract.WagePerWeek = (int)(Int32.Parse(this.PlayerProfileWageTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                                if (!((int)(playerAppearance * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileAppearanceBonusTextBox.Text))
                                    player.LoanContract.AppearanceBonus = (int)(Int32.Parse(this.PlayerProfileAppearanceBonusTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                                if (!((int)(playerGoal * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileGoalBonusTextBox.Text))
                                    player.LoanContract.GoalBonus = (int)(Int32.Parse(this.PlayerProfileGoalBonusTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                                if (!((int)(playerCleanSheet * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.PlayerProfileCleanSheetBonusTextBox.Text))
                                    player.LoanContract.CleanSheetBonus = (int)(Int32.Parse(this.PlayerProfileCleanSheetBonusTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                            }
                        }

                        // goalkeeping skills
                        if (!((int)(player.GoalKeepingSkills.AerialAbility * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileAerialAbilityTextBox.Text))
                            player.GoalKeepingSkills.AerialAbility = (sbyte)(Int32.Parse(this.PlayerProfileAerialAbilityTextBox.Text) * 5.0);
                        if (!((int)(player.GoalKeepingSkills.CommandOfArea * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileCommandOfAreaTextBox.Text))
                            player.GoalKeepingSkills.CommandOfArea = (sbyte)(Int32.Parse(this.PlayerProfileCommandOfAreaTextBox.Text) * 5.0);
                        if (!((int)(player.GoalKeepingSkills.Communication * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileCommunicationTextBox.Text))
                            player.GoalKeepingSkills.Communication = (sbyte)(Int32.Parse(this.PlayerProfileCommunicationTextBox.Text) * 5.0);
                        if (!((int)(player.GoalKeepingSkills.Eccentricity * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileEccentricityTextBox.Text))
                            player.GoalKeepingSkills.Eccentricity = (sbyte)(Int32.Parse(this.PlayerProfileEccentricityTextBox.Text) * 5.0);
                        if (!((int)(player.GoalKeepingSkills.Handling * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileHandlingTextBox.Text))
                            player.GoalKeepingSkills.Handling = (sbyte)(Int32.Parse(this.PlayerProfileHandlingTextBox.Text) * 5.0);
                        if (!((int)(player.GoalKeepingSkills.Kicking * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileKickingTextBox.Text))
                            player.GoalKeepingSkills.Kicking = (sbyte)(Int32.Parse(this.PlayerProfileKickingTextBox.Text) * 5.0);
                        if (!((int)(player.GoalKeepingSkills.OneOnOnes * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileOneOnOnesTextBox.Text))
                            player.GoalKeepingSkills.OneOnOnes = (sbyte)(Int32.Parse(this.PlayerProfileOneOnOnesTextBox.Text) * 5.0);
                        if (!((int)(player.GoalKeepingSkills.Reflexes * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileReflexesTextBox.Text))
                            player.GoalKeepingSkills.Reflexes = (sbyte)(Int32.Parse(this.PlayerProfileReflexesTextBox.Text) * 5.0);
                        if (!((int)(player.GoalKeepingSkills.RushingOut * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileRushingOutTextBox.Text))
                            player.GoalKeepingSkills.RushingOut = (sbyte)(Int32.Parse(this.PlayerProfileRushingOutTextBox.Text) * 5.0);
                        if (!((int)(player.GoalKeepingSkills.Throwing * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileThrowingTextBox.Text))
                            player.GoalKeepingSkills.Throwing = (sbyte)(Int32.Parse(this.PlayerProfileThrowingTextBox.Text) * 5.0);

                        // technical skills
                        if (!((int)(player.TechnicalSkills.Corners * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileCornersTextBox.Text))
                            player.TechnicalSkills.Corners = (sbyte)(Int32.Parse(this.PlayerProfileCornersTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.Crossing * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileCrossingTextBox.Text))
                            player.TechnicalSkills.Crossing = (sbyte)(Int32.Parse(this.PlayerProfileCrossingTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.Dribbling * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileDribblingTextBox.Text))
                            player.TechnicalSkills.Dribbling = (sbyte)(Int32.Parse(this.PlayerProfileDribblingTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.Finishing * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileFinishingTextBox.Text))
                            player.TechnicalSkills.Finishing = (sbyte)(Int32.Parse(this.PlayerProfileFinishingTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.FirstTouch * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileFirstTouchTextBox.Text))
                            player.TechnicalSkills.FirstTouch = (sbyte)(Int32.Parse(this.PlayerProfileFirstTouchTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.Freekicks * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileFreeKicksTextBox.Text))
                            player.TechnicalSkills.Freekicks = (sbyte)(Int32.Parse(this.PlayerProfileFreeKicksTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.Heading * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileHeadingTextBox.Text))
                            player.TechnicalSkills.Heading = (sbyte)(Int32.Parse(this.PlayerProfileHeadingTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.LongShots * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileLongShotsTextBox.Text))
                            player.TechnicalSkills.LongShots = (sbyte)(Int32.Parse(this.PlayerProfileLongShotsTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.Longthrows * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileLongThrowsTextBox.Text))
                            player.TechnicalSkills.Longthrows = (sbyte)(Int32.Parse(this.PlayerProfileLongThrowsTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.Marking * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileMarkingTextBox.Text))
                            player.TechnicalSkills.Marking = (sbyte)(Int32.Parse(this.PlayerProfileMarkingTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.Passing * 0.2 + 0.5)).ToString().Equals(this.PlayerProfilePassingTextBox.Text))
                            player.TechnicalSkills.Passing = (sbyte)(Int32.Parse(this.PlayerProfilePassingTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.PenaltyTaking * 0.2 + 0.5)).ToString().Equals(this.PlayerProfilePenaltyTakingTextBox.Text))
                            player.TechnicalSkills.PenaltyTaking = (sbyte)(Int32.Parse(this.PlayerProfilePenaltyTakingTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.Tackling * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileTacklingTextBox.Text))
                            player.TechnicalSkills.Tackling = (sbyte)(Int32.Parse(this.PlayerProfileTacklingTextBox.Text) * 5.0);
                        if (!((int)(player.TechnicalSkills.Technique * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileTechniqueTextBox.Text))
                            player.TechnicalSkills.Technique = (sbyte)(Int32.Parse(this.PlayerProfileTechniqueTextBox.Text) * 5.0);

                        // physical skills
                        if (!((int)(player.PhysicalSkills.Acceleration * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileAccelerationTextBox.Text))
                            player.PhysicalSkills.Acceleration = (sbyte)(Int32.Parse(this.PlayerProfileAccelerationTextBox.Text) * 5.0);
                        if (!((int)(player.PhysicalSkills.Agility * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileAgilityTextBox.Text))
                            player.PhysicalSkills.Agility = (sbyte)(Int32.Parse(this.PlayerProfileAgilityTextBox.Text) * 5.0);
                        if (!((int)(player.PhysicalSkills.Balance * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileBalanceTextBox.Text))
                            player.PhysicalSkills.Balance = (sbyte)(Int32.Parse(this.PlayerProfileBalanceTextBox.Text) * 5.0);
                        if (!((int)(player.PhysicalSkills.Jumping * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileJumpingTextBox.Text))
                            player.PhysicalSkills.Jumping = (sbyte)(Int32.Parse(this.PlayerProfileJumpingTextBox.Text) * 5.0);
                        if (!((int)(player.PhysicalSkills.NaturalFitness * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileNaturalFitnessTextBox.Text))
                            player.PhysicalSkills.NaturalFitness = (sbyte)(Int32.Parse(this.PlayerProfileNaturalFitnessTextBox.Text) * 5.0);
                        if (!((int)(player.PhysicalSkills.Pace * 0.2 + 0.5)).ToString().Equals(this.PlayerProfilePaceTextBox.Text))
                            player.PhysicalSkills.Pace = (sbyte)(Int32.Parse(this.PlayerProfilePaceTextBox.Text) * 5.0);
                        if (!((int)(player.PhysicalSkills.Stamina * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileStaminaTextBox.Text))
                            player.PhysicalSkills.Stamina = (sbyte)(Int32.Parse(this.PlayerProfileStaminaTextBox.Text) * 5.0);
                        if (!((int)(player.PhysicalSkills.Strength * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileStrengthTextBox.Text))
                            player.PhysicalSkills.Strength = (sbyte)(Int32.Parse(this.PlayerProfileStrengthTextBox.Text) * 5.0);

                        // mental skills
                        if (!((int)(player.MentalSkills.Aggression * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileAggressionTextBox.Text))
                            player.MentalSkills.Aggression = (sbyte)(Int32.Parse(this.PlayerProfileAggressionTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Anticipation * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileAnticipationTextBox.Text))
                            player.MentalSkills.Anticipation = (sbyte)(Int32.Parse(this.PlayerProfileAnticipationTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Bravery * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileBraveryTextBox.Text))
                            player.MentalSkills.Bravery = (sbyte)(Int32.Parse(this.PlayerProfileBraveryTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Composure * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileComposureTextBox.Text))
                            player.MentalSkills.Composure = (sbyte)(Int32.Parse(this.PlayerProfileComposureTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Concentration * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileConcentrationTextBox.Text))
                            player.MentalSkills.Concentration = (sbyte)(Int32.Parse(this.PlayerProfileConcentrationTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Creativity * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileCreativityTextBox.Text))
                            player.MentalSkills.Creativity = (sbyte)(Int32.Parse(this.PlayerProfileCreativityTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Decisions * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileDecisionsTextBox.Text))
                            player.MentalSkills.Decisions = (sbyte)(Int32.Parse(this.PlayerProfileDecisionsTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Determination * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileDeterminationTextBox.Text))
                            player.MentalSkills.Determination = (sbyte)(Int32.Parse(this.PlayerProfileDeterminationTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Flair * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileFlairTextBox.Text))
                            player.MentalSkills.Flair = (sbyte)(Int32.Parse(this.PlayerProfileFlairTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Influence * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileInfluenceTextBox.Text))
                            player.MentalSkills.Influence = (sbyte)(Int32.Parse(this.PlayerProfileInfluenceTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.OffTheBall * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileOffTheBallTextBox.Text))
                            player.MentalSkills.OffTheBall = (sbyte)(Int32.Parse(this.PlayerProfileOffTheBallTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Positioning * 0.2 + 0.5)).ToString().Equals(this.PlayerProfilePositioningTextBox.Text))
                            player.MentalSkills.Positioning = (sbyte)(Int32.Parse(this.PlayerProfilePositioningTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Teamwork * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileTeamWorkTextBox.Text))
                            player.MentalSkills.Teamwork = (sbyte)(Int32.Parse(this.PlayerProfileTeamWorkTextBox.Text) * 5.0);
                        if (!((int)(player.MentalSkills.Workrate * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileWorkRateTextBox.Text))
                            player.MentalSkills.Workrate = (sbyte)(Int32.Parse(this.PlayerProfileWorkRateTextBox.Text) * 5.0);

                        // hidden skills
                        if (!((int)(player.HiddenSkills.Consistency * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileConsistencyTextBox.Text))
                            player.HiddenSkills.Consistency = (sbyte)(Int32.Parse(this.PlayerProfileConsistencyTextBox.Text) * 5.0);
                        if (!((int)(player.HiddenSkills.Dirtyness * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileDirtynessTextBox.Text))
                            player.HiddenSkills.Dirtyness = (sbyte)(Int32.Parse(this.PlayerProfileDirtynessTextBox.Text) * 5.0);
                        if (!((int)(player.HiddenSkills.ImportantMatches * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileImportantMatchesTextBox.Text))
                            player.HiddenSkills.ImportantMatches = (sbyte)(Int32.Parse(this.PlayerProfileImportantMatchesTextBox.Text) * 5.0);
                        if (!((int)(player.HiddenSkills.InjuryProness * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileInjuryPronenessTextBox.Text))
                            player.HiddenSkills.InjuryProness = (sbyte)(Int32.Parse(this.PlayerProfileInjuryPronenessTextBox.Text) * 5.0);
                        if (!((int)(player.HiddenSkills.Versatility * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileVersatilityTextBox.Text))
                            player.HiddenSkills.Versatility = (sbyte)(Int32.Parse(this.PlayerProfileVersatilityTextBox.Text) * 5.0);

                        // personality skills
                        if (!((int)player.MentalTraitsSkills.Adaptability).ToString().Equals(this.PlayerProfileAdaptabilityTextBox.Text))
                            player.MentalTraitsSkills.Adaptability = (sbyte)Int32.Parse(this.PlayerProfileAdaptabilityTextBox.Text);
                        if (!((int)player.MentalTraitsSkills.Ambition).ToString().Equals(this.PlayerProfileAmbitionTextBox.Text))
                            player.MentalTraitsSkills.Ambition = (sbyte)Int32.Parse(this.PlayerProfileAmbitionTextBox.Text);
                        if (!((int)player.MentalTraitsSkills.Controversy).ToString().Equals(this.PlayerProfileControversyTextBox.Text))
                            player.MentalTraitsSkills.Controversy = (sbyte)Int32.Parse(this.PlayerProfileControversyTextBox.Text);
                        if (!((int)player.MentalTraitsSkills.Loyalty).ToString().Equals(this.PlayerProfileLoyaltyTextBox.Text))
                            player.MentalTraitsSkills.Loyalty = (sbyte)Int32.Parse(this.PlayerProfileLoyaltyTextBox.Text);
                        if (!((int)player.MentalTraitsSkills.Pressure).ToString().Equals(this.PlayerProfilePressureTextBox.Text))
                            player.MentalTraitsSkills.Pressure = (sbyte)Int32.Parse(this.PlayerProfilePressureTextBox.Text);
                        if (!((int)player.MentalTraitsSkills.Professionalism).ToString().Equals(this.PlayerProfileProfessionalismTextBox.Text))
                            player.MentalTraitsSkills.Professionalism = (sbyte)Int32.Parse(this.PlayerProfileProfessionalismTextBox.Text);
                        if (!((int)player.MentalTraitsSkills.Sportsmanship).ToString().Equals(this.PlayerProfileSportsmanshipTextBox.Text))
                            player.MentalTraitsSkills.Sportsmanship = (sbyte)Int32.Parse(this.PlayerProfileSportsmanshipTextBox.Text);
                        if (!((int)player.MentalTraitsSkills.Temperament).ToString().Equals(this.PlayerProfileTemperamentTextBox.Text))
                            player.MentalTraitsSkills.Temperament = (sbyte)Int32.Parse(this.PlayerProfileTemperamentTextBox.Text);

                        if (!player.Condition.ToString().Equals(this.PlayerProfileConditionTextBox.Text))
                            player.Condition = Int32.Parse(this.PlayerProfileConditionTextBox.Text);
                        if (!player.Morale.ToString().Equals(this.PlayerProfileMoraleTextBox.Text))
                            player.Morale = Byte.Parse(this.PlayerProfileMoraleTextBox.Text);
                        //this.PlayerProfileHappinessTextBox.Text;
                        //this.PlayerProfileJadednessTextBox.Text;
                        //this.PlayerProfileSquadNoTextBox.Text;
                        if (!((int)(player.PhysicalSkills.LeftFoot * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileLeftFootTextBox.Text))
                            player.PhysicalSkills.LeftFoot = (sbyte)(Int32.Parse(this.PlayerProfileLeftFootTextBox.Text) * 5.0);
                        if (!((int)(player.PhysicalSkills.RightFoot * 0.2 + 0.5)).ToString().Equals(this.PlayerProfileRightFootTextBox.Text))
                            player.PhysicalSkills.RightFoot = (sbyte)(Int32.Parse(this.PlayerProfileRightFootTextBox.Text) * 5.0);

                        if (!player.InternationalPrestige.ToString().Equals(this.PlayerProfileWorldReputationTextBox.Text))
                            player.InternationalPrestige = Int32.Parse(this.PlayerProfileWorldReputationTextBox.Text);
                        if (!player.NationalPrestige.ToString().Equals(this.PlayerProfileNationalReputationTextBox.Text))
                            player.NationalPrestige = Int32.Parse(this.PlayerProfileNationalReputationTextBox.Text);
                        if (!player.CurrentPrestige.ToString().Equals(this.PlayerProfileLocalReputationTextBox.Text))
                            player.CurrentPrestige = Int32.Parse(this.PlayerProfileLocalReputationTextBox.Text);

                        setPlayerProfile(player.ID, false);
                        return;
                    }
                }
            }
            else if (this.StaffTabControl.Visible)
            {
                foreach (Staff staff in fm.NonPlayingStaff)
                {
                    if (staff.ToString().Equals(this.StaffProfileFullNameTextBox.Text))
                    {
                        // staff profile tab
                        if (!StaffProfileClubTextBox.Text.Equals("Free Agent"))
                        {
                            if (!((int)(staff.Contract.WagePerWeek * this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency)).ToString().Equals(this.StaffProfileWageTextBox.Text))
                                staff.Contract.WagePerWeek = (int)(Int32.Parse(this.StaffProfileWageTextBox.Text) / this.preferencesForm.WagesMultiplier * this.preferencesForm.Currency);
                        }

                        if (staff.CurrentCoachingAbility.ToString() != this.StaffProfileCATextBox.Text)
                            staff.CurrentCoachingAbility = Int32.Parse(this.StaffProfileCATextBox.Text);
                        if (staff.PotentialCoachingAbility.ToString() != this.StaffProfilePATextBox.Text)
                            staff.PotentialCoachingAbility = Int32.Parse(this.StaffProfilePATextBox.Text);

                        // coaching skills
                        if (!((int)(staff.CoachingSkills.CoachingAttacking * 0.2 + 0.5)).ToString().Equals(this.StaffProfileAttackingTextBox.Text))
                            staff.CoachingSkills.CoachingAttacking = (sbyte)(Int32.Parse(this.StaffProfileAttackingTextBox.Text) * 5.0);
                        if (!((int)(staff.CoachingSkills.CoachingDefending * 0.2 + 0.5)).ToString().Equals(this.StaffProfileDefendingTextBox.Text))
                            staff.CoachingSkills.CoachingDefending = (sbyte)(Int32.Parse(this.StaffProfileDefendingTextBox.Text) * 5.0);
                        if (!((int)(staff.CoachingSkills.CoachingFitness * 0.2 + 0.5)).ToString().Equals(this.StaffProfileFitnessTextBox.Text))
                            staff.CoachingSkills.CoachingFitness = (sbyte)(Int32.Parse(this.StaffProfileFitnessTextBox.Text) * 5.0);
                        if (!((int)(staff.CoachingSkills.CoachingGoalkeepers * 0.2 + 0.5)).ToString().Equals(this.StaffProfileGoalkeepersTextBox.Text))
                            staff.CoachingSkills.CoachingGoalkeepers = (sbyte)(Int32.Parse(this.StaffProfileGoalkeepersTextBox.Text) * 5.0);
                        if (!((int)(staff.CoachingSkills.CoachingMental * 0.2 + 0.5)).ToString().Equals(this.StaffProfileMentalTextBox.Text))
                            staff.CoachingSkills.CoachingMental = (sbyte)(Int32.Parse(this.StaffProfileMentalTextBox.Text) * 5.0);
                        if (!((int)(staff.CoachingSkills.CoachingPlayer * 0.2 + 0.5)).ToString().Equals(this.StaffProfilePlayerTextBox.Text))
                            staff.CoachingSkills.CoachingPlayer = (sbyte)(Int32.Parse(this.StaffProfilePlayerTextBox.Text) * 5.0);
                        if (!((int)(staff.CoachingSkills.CoachingTactical * 0.2 + 0.5)).ToString().Equals(this.StaffProfileTacticalTextBox.Text))
                            staff.CoachingSkills.CoachingTactical = (sbyte)(Int32.Parse(this.StaffProfileTacticalTextBox.Text) * 5.0);
                        if (!((int)(staff.CoachingSkills.CoachingTechnical * 0.2 + 0.5)).ToString().Equals(this.StaffProfileTechnicalTextBox.Text))
                            staff.CoachingSkills.CoachingTechnical = (sbyte)(Int32.Parse(this.StaffProfileTechnicalTextBox.Text) * 5.0);
                        if (!((int)(staff.CoachingSkills.ManManagement * 0.2 + 0.5)).ToString().Equals(this.StaffProfileManManagementTextBox.Text))
                            staff.CoachingSkills.ManManagement = (sbyte)(Int32.Parse(this.StaffProfileManManagementTextBox.Text) * 5.0);
                        if (!((int)(staff.CoachingSkills.WorkingWithYoungsters * 0.2 + 0.5)).ToString().Equals(this.StaffProfileWorkingWithYoungstersTextBox.Text))
                            staff.CoachingSkills.WorkingWithYoungsters = (sbyte)(Int32.Parse(this.StaffProfileWorkingWithYoungstersTextBox.Text) * 5.0);

                        // mental skills
                        if (!((int)(staff.MentalSkills.Adaptability * 0.2 + 0.5)).ToString().Equals(this.StaffProfileAdaptabilityTextBox.Text))
                            staff.MentalSkills.Adaptability = (sbyte)(Int32.Parse(this.StaffProfileAdaptabilityTextBox.Text) * 5.0);
                        if (!((int)(staff.StaffMentalTraitsSkills.Ambition * 0.2 + 0.5)).ToString().Equals(this.StaffProfileAmbitionTextBox.Text))
                            staff.StaffMentalTraitsSkills.Ambition = (sbyte)(Int32.Parse(this.StaffProfileAmbitionTextBox.Text) * 5.0);
                        if (!((int)(staff.StaffMentalTraitsSkills.Controversy * 0.2 + 0.5)).ToString().Equals(this.StaffProfileControversyTextBox.Text))
                            staff.StaffMentalTraitsSkills.Controversy = (sbyte)(Int32.Parse(this.StaffProfileControversyTextBox.Text) * 5.0);
                        if (!((int)(staff.StaffMentalTraitsSkills.Determination * 0.2 + 0.5)).ToString().Equals(this.StaffProfileDeterminationTextBox.Text))
                            staff.StaffMentalTraitsSkills.Determination = (sbyte)(Int32.Parse(this.StaffProfileDeterminationTextBox.Text) * 5.0);
                        if (!((int)(staff.StaffMentalTraitsSkills.Loyalty * 0.2 + 0.5)).ToString().Equals(this.StaffProfileLoyaltyTextBox.Text))
                            staff.StaffMentalTraitsSkills.Loyalty = (sbyte)(Int32.Parse(this.StaffProfileLoyaltyTextBox.Text) * 5.0);
                        if (!((int)(staff.StaffMentalTraitsSkills.Pressure * 0.2 + 0.5)).ToString().Equals(this.StaffProfilePressureTextBox.Text))
                            staff.StaffMentalTraitsSkills.Pressure = (sbyte)(Int32.Parse(this.StaffProfilePressureTextBox.Text) * 5.0);
                        if (!((int)(staff.StaffMentalTraitsSkills.Professionalism * 0.2 + 0.5)).ToString().Equals(this.StaffProfileProfessionalismTextBox.Text))
                            staff.StaffMentalTraitsSkills.Professionalism = (sbyte)(Int32.Parse(this.StaffProfileProfessionalismTextBox.Text) * 5.0);
                        if (!((int)(staff.StaffMentalTraitsSkills.Sportsmanship * 0.2 + 0.5)).ToString().Equals(this.StaffProfileSportsmanshipTextBox.Text))
                            staff.StaffMentalTraitsSkills.Sportsmanship = (sbyte)(Int32.Parse(this.StaffProfileSportsmanshipTextBox.Text) * 5.0);
                        if (!((int)(staff.StaffMentalTraitsSkills.Temperament * 0.2 + 0.5)).ToString().Equals(this.StaffProfileTemperamentTextBox.Text))
                            staff.StaffMentalTraitsSkills.Temperament = (sbyte)(Int32.Parse(this.StaffProfileTemperamentTextBox.Text) * 5.0);
                        if (!((int)(staff.MentalSkills.JudgingPlayerAbility * 0.2 + 0.5)).ToString().Equals(this.StaffProfileJudgingPlayerAbilityTextBox.Text))
                            staff.MentalSkills.JudgingPlayerAbility = (sbyte)(Int32.Parse(this.StaffProfileJudgingPlayerAbilityTextBox.Text) * 5.0);
                        if (!((int)(staff.MentalSkills.JudgingPlayerPotential * 0.2 + 0.5)).ToString().Equals(this.StaffProfileJudgingPlayerPotentialTextBox.Text))
                            staff.MentalSkills.JudgingPlayerPotential = (sbyte)(Int32.Parse(this.StaffProfileJudgingPlayerPotentialTextBox.Text) * 5.0);
                        if (!((int)(staff.MentalSkills.LevelOfDiscipline * 0.2 + 0.5)).ToString().Equals(this.StaffProfileLevelOfDisciplineTextBox.Text))
                            staff.MentalSkills.LevelOfDiscipline = (sbyte)(Int32.Parse(this.StaffProfileLevelOfDisciplineTextBox.Text) * 5.0);
                        if (!((int)(staff.MentalSkills.Motivating * 0.2 + 0.5)).ToString().Equals(this.StaffProfileMotivatingTextBox.Text))
                            staff.MentalSkills.Motivating = (sbyte)(Int32.Parse(this.StaffProfileMotivatingTextBox.Text) * 5.0);
                        if (!((int)(staff.MentalSkills.Physiotherapy * 0.2 + 0.5)).ToString().Equals(this.StaffProfilePhysiotherapyTextBox.Text))
                            staff.MentalSkills.Physiotherapy = (sbyte)(Int32.Parse(this.StaffProfilePhysiotherapyTextBox.Text) * 5.0);
                        if (!((int)(staff.MentalSkills.TacticalKnowledge * 0.2 + 0.5)).ToString().Equals(this.StaffProfileTacticalKnowledgeTextBox.Text))
                            staff.MentalSkills.TacticalKnowledge = (sbyte)(Int32.Parse(this.StaffProfileTacticalKnowledgeTextBox.Text) * 5.0);

                        // tactical skills
                        if (!((int)(staff.TacticalSkills.Depth * 0.2 + 0.5)).ToString().Equals(this.StaffProfileDepthTextBox.Text))
                            staff.TacticalSkills.Depth = (sbyte)(Int32.Parse(this.StaffProfileDepthTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.Directness * 0.2 + 0.5)).ToString().Equals(this.StaffProfileDirectnessTextBox.Text))
                            staff.TacticalSkills.Directness = (sbyte)(Int32.Parse(this.StaffProfileDirectnessTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.Flamboyancy * 0.2 + 0.5)).ToString().Equals(this.StaffProfileFlamboyancyTextBox.Text))
                            staff.TacticalSkills.Flamboyancy = (sbyte)(Int32.Parse(this.StaffProfileFlamboyancyTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.Flexibility * 0.2 + 0.5)).ToString().Equals(this.StaffProfileFlexibilityTextBox.Text))
                            staff.TacticalSkills.Flexibility = (sbyte)(Int32.Parse(this.StaffProfileFlexibilityTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.FreeRoles * 0.2 + 0.5)).ToString().Equals(this.StaffProfileFreeRolesTextBox.Text))
                            staff.TacticalSkills.FreeRoles = (sbyte)(Int32.Parse(this.StaffProfileFreeRolesTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.Marking * 0.2 + 0.5)).ToString().Equals(this.StaffProfileMarkingTextBox.Text))
                            staff.TacticalSkills.Marking = (sbyte)(Int32.Parse(this.StaffProfileMarkingTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.OffSide * 0.2 + 0.5)).ToString().Equals(this.StaffProfileOffsideTextBox.Text))
                            staff.TacticalSkills.OffSide = (sbyte)(Int32.Parse(this.StaffProfileOffsideTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.Pressing * 0.2 + 0.5)).ToString().Equals(this.StaffProfilePressingTextBox.Text))
                            staff.TacticalSkills.Pressing = (sbyte)(Int32.Parse(this.StaffProfilePressingTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.SittingBack * 0.2 + 0.5)).ToString().Equals(this.StaffProfileSittingBackTextBox.Text))
                            staff.TacticalSkills.SittingBack = (sbyte)(Int32.Parse(this.StaffProfileSittingBackTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.Tempo * 0.2 + 0.5)).ToString().Equals(this.StaffProfileTempoTextBox.Text))
                            staff.TacticalSkills.Tempo = (sbyte)(Int32.Parse(this.StaffProfileTempoTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.UseOfPlaymaker * 0.2 + 0.5)).ToString().Equals(this.StaffProfileUseOfPlaymakerTextBox.Text))
                            staff.TacticalSkills.UseOfPlaymaker = (sbyte)(Int32.Parse(this.StaffProfileUseOfPlaymakerTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.UseOfSubstitutions * 0.2 + 0.5)).ToString().Equals(this.StaffProfileUseOfSubstitutionsTextBox.Text))
                            staff.TacticalSkills.UseOfSubstitutions = (sbyte)(Int32.Parse(this.StaffProfileUseOfSubstitutionsTextBox.Text) * 5.0);
                        if (!((int)(staff.TacticalSkills.Width * 0.2 + 0.5)).ToString().Equals(this.StaffProfileWidthTextBox.Text))
                            staff.TacticalSkills.Width = (sbyte)(Int32.Parse(this.StaffProfileWidthTextBox.Text) * 5.0);

                        // non tactical skills
                        if (!((int)(staff.NonTacticalSkills.BuyingPlayers * 0.2 + 0.5)).ToString().Equals(this.StaffProfileBuyingPlayersTextBox.Text))
                            staff.NonTacticalSkills.BuyingPlayers = (sbyte)(Int32.Parse(this.StaffProfileBuyingPlayersTextBox.Text) * 5.0);
                        if (!((int)(staff.NonTacticalSkills.HardnessOfTraining * 0.2 + 0.5)).ToString().Equals(this.StaffProfileHardnessOfTrainingTextBox.Text))
                            staff.NonTacticalSkills.HardnessOfTraining = (sbyte)(Int32.Parse(this.StaffProfileHardnessOfTrainingTextBox.Text) * 5.0);
                        if (!((int)(staff.NonTacticalSkills.MindGames * 0.2 + 0.5)).ToString().Equals(this.StaffProfileMindGamesTextBox.Text))
                            staff.NonTacticalSkills.MindGames = (sbyte)(Int32.Parse(this.StaffProfileMindGamesTextBox.Text) * 5.0);
                        if (!((int)(staff.NonTacticalSkills.SquadRotation * 0.2 + 0.5)).ToString().Equals(this.StaffProfileSquadRotationTextBox.Text))
                            staff.NonTacticalSkills.SquadRotation = (sbyte)(Int32.Parse(this.StaffProfileSquadRotationTextBox.Text) * 5.0);

                        // chairman skills
                        if (!((int)(staff.ChairmanSkills.Business * 0.2 + 0.5)).ToString().Equals(this.StaffProfileBusinessTextBox.Text))
                            staff.ChairmanSkills.Business = (sbyte)(Int32.Parse(this.StaffProfileBusinessTextBox.Text) * 5.0);
                        if (!((int)(staff.ChairmanSkills.Interference * 0.2 + 0.5)).ToString().Equals(this.StaffProfileInterferenceTextBox.Text))
                            staff.ChairmanSkills.Interference = (sbyte)(Int32.Parse(this.StaffProfileInterferenceTextBox.Text) * 5.0);
                        if (!((int)(staff.ChairmanSkills.Patience * 0.2 + 0.5)).ToString().Equals(this.StaffProfilePatienceTextBox.Text))
                            staff.ChairmanSkills.Patience = (sbyte)(Int32.Parse(this.StaffProfilePatienceTextBox.Text) * 5.0);
                        if (!((int)(staff.ChairmanSkills.Resources * 0.2 + 0.5)).ToString().Equals(this.StaffProfileResourcesTextBox.Text))
                            staff.ChairmanSkills.Resources = (sbyte)(Int32.Parse(this.StaffProfileResourcesTextBox.Text) * 5.0);

                        if (!staff.InternationalPrestige.ToString().Equals(this.StaffProfileWorldReputationTextBox.Text))
                            staff.InternationalPrestige = Int32.Parse(this.StaffProfileWorldReputationTextBox.Text);
                        if (!staff.NationalPrestige.ToString().Equals(this.StaffProfileNationalReputationTextBox.Text))
                            staff.NationalPrestige = Int32.Parse(this.StaffProfileNationalReputationTextBox.Text);
                        if (!staff.CurrentPrestige.ToString().Equals(this.StaffProfileLocalReputationTextBox.Text))
                            staff.CurrentPrestige = Int32.Parse(this.StaffProfileLocalReputationTextBox.Text);

                        setStaffProfile(staff.ID, false);
                        return;
                    }
                }
            }
            else if (this.TeamsTabControl.Visible)
            {
                foreach (Team team in fm.Teams)
                {
                    if (team.Club.ID.ToString().Equals(this.TeamProfileIDTextBox.Text))
                    {
                        // teams profile tab
                        int reputation = Int32.Parse(this.TeamProfileReputationTextBox.Text);
                        if (team.Prestige != reputation)
                            team.Prestige = reputation;
                        if (this.TeamProfileNationalTextBox.Text.Equals("No"))
                        {
                            //this.TeamProfileIDTextBox.Text;
                            //this.TeamProfileNameTextBox.Text;
                            //this.TeamProfileNationalityTextBox.Text;
                            //this.TeamProfileYearFoundedTextBox.Text;
                            //this.TeamProfileNationalTextBox.Text;
                            //this.TeamProfileStatusTextBox.Text;
                            //this.TeamProfileNoPlayersTextBox.Text;
                            //this.TeamProfileNoScoutsTextBox.Text;
                            int maxAffiliatedClubs = Int32.Parse(this.TeamProfileMaxAffiliatedClubsTextBox.Text);
                            int affiliatedClubs = Int32.Parse(this.TeamProfileAffiliatedClubsTextBox.Text);
                            int trainingGround = Int32.Parse(this.TeamProfileTrainingGroundTextBox.Text);
                            int youthGround = Int32.Parse(this.TeamProfileYouthGroundTextBox.Text);
                            int youthAcademy = Int32.Parse(this.TeamProfileYouthAcademyTextBox.Text);
                            int seasonValue = Int32.Parse(this.TeamProfileTotalTransferTextBox.Text);
                            seasonValue = (int)(seasonValue / this.preferencesForm.Currency) + 1;
                            int remainingValue = Int32.Parse(this.TeamProfileRemainingTransferTextBox.Text);
                            remainingValue = (int)(remainingValue / this.preferencesForm.Currency) + 1;
                            int balanceValue = Int32.Parse(this.TeamProfileBalanceTextBox.Text);
                            balanceValue = (int)(balanceValue / this.preferencesForm.Currency) + 1;
                            int wageValue = Int32.Parse(this.TeamProfileTotalWageTextBox.Text);
                            wageValue = (int)(wageValue / this.preferencesForm.Currency) + 1;
                            int wageUsedValue = Int32.Parse(this.TeamProfileUsedWageTextBox.Text);
                            wageUsedValue = (int)(wageUsedValue / this.preferencesForm.Currency) + 1;
                            int maxAttendance = Int32.Parse(this.TeamProfileMaxAttendanceTextBox.Text);
                            int averageAttendance = Int32.Parse(this.TeamProfileAverageAttendanceTextBox.Text);
                            int minAttendance = Int32.Parse(this.TeamProfileMinAttendanceTextBox.Text);

                            if (team.Club.MaxAffiliatedClubNumber != maxAffiliatedClubs)
                                team.Club.MaxAffiliatedClubNumber = (byte)maxAffiliatedClubs;
                            if (team.Club.NumberOfAffiliatedClubs != affiliatedClubs)
                                team.Club.NumberOfAffiliatedClubs = (byte)affiliatedClubs;
                            if (team.Club.TrainingGround != trainingGround)
                                team.Club.TrainingGround = (byte)trainingGround;
                            if (team.Club.YouthGround != youthGround)
                                team.Club.YouthGround = (byte)youthGround;
                            if (!this.TeamProfileYouthAcademyTextBox.ReadOnly)
                            {
                                if (team.Club.YouthAcademy != youthAcademy)
                                    team.Club.YouthAcademy = (byte)youthAcademy;
                            }
                            if (team.Club.Finances.SeasonTransferBudget != seasonValue)
                                team.Club.Finances.SeasonTransferBudget = seasonValue;
                            if (team.Club.Finances.RemainingTransferBudget != remainingValue)
                                team.Club.Finances.RemainingTransferBudget = remainingValue;
                            if (team.Club.Finances.Balance != balanceValue)
                                team.Club.Finances.Balance = balanceValue;
                            if (team.Club.Finances.WageBudget != wageValue)
                                team.Club.Finances.WageBudget = wageValue;
                            if (team.Club.Finances.UsedWage != wageUsedValue)
                                team.Club.Finances.UsedWage = wageUsedValue;
                            if (team.Club.MaximumAttendance != maxAttendance)
                                team.Club.MaximumAttendance = maxAttendance;
                            if (team.Club.AverageAttendance != averageAttendance)
                                team.Club.AverageAttendance = averageAttendance;
                            if (team.Club.MinimumAttendance != minAttendance)
                                team.Club.MinimumAttendance = minAttendance;

                            //this.TeamProfileRevenueAvailableTextBox.Text;
                            //this.TeamProfileStadiumIDTextBox.Text;
                            //this.TeamProfileStadiumNameTextBox.Text;
                            //this.TeamProfileStadiumOwnerTextBox.Text;
                            //this.TeamProfileStadiumLocationTextBox.Text;
                            //this.TeamProfileStadiumNearbyStadiumTextBox.Text;
                            byte decay = byte.Parse(this.TeamProfileStadiumDecayTextBox.Text);
                            byte fieldWidth = byte.Parse(this.TeamProfileStadiumFieldWidthTextBox.Text);
                            byte fieldLength = byte.Parse(this.TeamProfileStadiumFieldLengthTextBox.Text);
                            int currentCapacity = Int32.Parse(this.TeamProfileStadiumCurrentCapacityTextBox.Text);
                            int seatingCapacity = Int32.Parse(this.TeamProfileStadiumSeatingCapacityTextBox.Text);
                            int expansionCapacity = Int32.Parse(this.TeamProfileStadiumExpansionCapacityTextBox.Text);
                            int usedCapacity = Int32.Parse(this.TeamProfileStadiumUsedCapacityTextBox.Text);

                            if (team.Stadium.Decay != decay)
                                team.Stadium.Decay = decay;
                            if (team.Stadium.FieldWidth != fieldWidth)
                                team.Stadium.FieldWidth = fieldWidth;
                            if (team.Stadium.FieldLength != fieldLength)
                                team.Stadium.FieldLength = fieldLength;
                            if (team.Stadium.StadiumCapacity != currentCapacity)
                                team.Stadium.StadiumCapacity = currentCapacity;
                            if (team.Stadium.SeatingCapacity != seatingCapacity)
                                team.Stadium.SeatingCapacity = seatingCapacity;
                            if (team.Stadium.ExpansionCapacity != expansionCapacity)
                                team.Stadium.ExpansionCapacity = expansionCapacity;
                            if (team.Stadium.UsedCapacity != usedCapacity)
                                team.Stadium.UsedCapacity = usedCapacity;
                        }

                        setTeamProfile(team.Club.ID, false);
                        return;
                    }
                }
            }
        }

        private void ProfileCancelEditingToolStrip_Click(object sender, EventArgs e)
        {
            if (this.PlayersTabControl.Visible)
            {
                foreach (Player player in fm.Players)
                {
                    if (player.ID.ToString().Equals(this.PlayerProfileIDTextBox.Text))
                    {
                        setPlayerProfile(player.ID, false);
                        return;
                    }
                }
            }
            else if (this.StaffTabControl.Visible)
            {
                foreach (Staff staff in fm.NonPlayingStaff)
                {
                    if (staff.ToString().Equals(this.StaffProfileFullNameTextBox.Text))
                    {
                        setStaffProfile(staff.ID, false);
                        return;
                    }
                }
            }
            else if (this.TeamsTabControl.Visible)
            {
                foreach (Team team in fm.Teams)
                {
                    if (team.Club.ID.ToString().Equals(this.TeamProfileIDTextBox.Text))
                    {
                        setTeamProfile(team.Club.ID, false);
                        return;
                    }
                }
            }
        }
    }
}