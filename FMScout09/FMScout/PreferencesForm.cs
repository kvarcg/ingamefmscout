using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace FMScout
{
    public partial class PreferencesForm : Form
    {
        public class Settings
        {
            public List<int> playersDefColumns;
            public List<int> staffDefColumns;
            public List<int> teamsDefColumns;
            public List<int> shortlistDefColumns;
            public string defCurrency = "";
            public string defWages = "";
            public string defHeight = "";
            public string defWeight = "";
            public string defEditing = "";
            public string defLanguage = "";
            public string defColorTheme = "";
            public int wonderkidsMaxAge = 0;
            public int wonderkidsMinPA = 0;
            public int wonderstaffMinPA = 0;
            public int wonderteamsMinRep = 0;

            public Settings()
            {
                playersDefColumns = new List<int>();
                staffDefColumns = new List<int>();
                teamsDefColumns = new List<int>();
                shortlistDefColumns = new List<int>();
            }
        }

        public class Theme
        {
            public List<Color> color;
            public int alpha;
            public Font font;

            public Theme()
            {
                color = new List<Color>();
            }
        }

        public class Language
        {
            public List<string> words;

            public Language()
            {
                words = new List<string>();
            }
        }

        private MainForm mainForm = null;
        private float currency = 0.0f;
        private int wagesMultiplier = 0;
        private string wagesString = "";
        private float heightMultiplier = 0.0f;
        private string heightString = "";
        private float weightMultiplier = 0.0f;
        private string weightString = "";
        private string numberFormat = "{0:0,0}";
        private bool allowEditing = false;
        public int wonderkidsMaxAge = 0;
        public int wonderkidsMinPA = 0;
        public int wonderstaffMinPA = 0;
        public int wonderteamsMinRep = 0;
        public List<int> playersColumns;
        public List<int> staffColumns;
        public List<int> teamsColumns;
        public List<int> shortlistColumns;
        public List<int> playersColumnsWidth;
        public List<int> staffColumnsWidth;
        public List<int> teamsColumnsWidth;
        public List<int> shortlistColumnsWidth;
        public List<string> Languages;
        public string curLanguage = "";
        public bool curEditing = false;
        public List<IFormatProvider> cultureList;
        public string curColorTheme = "";
        public List<Settings> settingsList;
        public Settings curSettings = null;
        public List<Theme> themeList;
        private ColorDialog colorDialog;
        FontDialog fontDialog;
        Font curFont;
        System.Drawing.Text.PrivateFontCollection[] fmFont;

        public PreferencesForm(MainForm Mainform)
        {
            InitializeComponent();
            this.mainForm = Mainform;
        }

        public void init()
        {
            this.TransparencyLabel.Visible = false;
            this.GroupBoxesTransparencyTrackBar.Visible = false;
            this.GroupBoxesTransparencyResultLabel.Visible = false;

            playersColumns = new List<int>();
            staffColumns = new List<int>();
            teamsColumns = new List<int>();
            shortlistColumns = new List<int>();

            playersColumnsWidth = new List<int>();
            staffColumnsWidth = new List<int>();
            teamsColumnsWidth = new List<int>();
            shortlistColumnsWidth = new List<int>();
            Languages = new List<string>();
            cultureList = new List<IFormatProvider>();
            settingsList = new List<Settings>();
            themeList = new List<Theme>();
            settingsList.Add(new Settings());

            settingsList[0].defCurrency = "Euro";
            settingsList[0].defWages = "Yearly";
            settingsList[0].defHeight = "Centimeters";
            settingsList[0].defWeight = "Kilos";
            settingsList[0].defEditing = "No";
            settingsList[0].defColorTheme = "Blue";
            settingsList[0].defLanguage = "English";

            int[] playersDefCols = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            settingsList[0].playersDefColumns.AddRange(playersDefCols);
            int[] staffDefCols = { 1, 2, 3, 4, 5, 6, 7, 9, 10, 12, 13, 14 };
            settingsList[0].staffDefColumns.AddRange(staffDefCols);
            int[] teamsDefCols = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            settingsList[0].teamsDefColumns.AddRange(teamsDefCols);
            int[] shortlistDefCols = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            settingsList[0].shortlistDefColumns.AddRange(shortlistDefCols);

            settingsList[0].wonderkidsMaxAge = 20;
            settingsList[0].wonderkidsMinPA = 170;
            settingsList[0].wonderstaffMinPA = 170;
            settingsList[0].wonderteamsMinRep = 8000;

            colorDialog = new ColorDialog();
            fontDialog = new FontDialog();

            // add Columns Default Width
            int[] playersWidths = { 20, 80, 140, 120, 140, 80, 80, 40, 40, 40, 40, 100, 60, 100, 100, 100, 100, 100, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60
                           };
            playersColumnsWidth.AddRange(playersWidths);

            int[] staffWidths = { 80, 140, 120, 140, 80, 40, 40, 40, 40, 100, 100, 100, 100, 100, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60,
                               60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60,
                               60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60
                           };
            staffColumnsWidth.AddRange(staffWidths);

            int[] teamsWidths = { 80, 140, 120, 100, 100, 120, 100, 100, 100, 100, 100, 60, 60, 60, 100, 80, 80, 80, 80, 80, 80,
                                80, 80, 80, 80, 80};
            teamsColumnsWidth.AddRange(teamsWidths);

            int[] shortlistWidths = { 80, 140, 120, 140, 80, 80, 40, 40, 40, 40, 100, 60, 100, 100, 100, 100, 100, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60
                           };

            shortlistColumnsWidth.AddRange(shortlistWidths);

            //fmFont = new System.Drawing.Text.PrivateFontCollection[4];
            //string[] fmFontFile = new string[]{"Vera", "VeraBd", "VeraBI", "VeraIt"};

            //for (int i = 0; i < fmFont.Length; ++i)
            //{
            //    fmFont[i] = new System.Drawing.Text.PrivateFontCollection();
            //    fmFont[i].AddFontFile(System.Environment.CurrentDirectory + "\\Themes\\Fonts\\" + fmFontFile[i] + ".ttf");
            //}

            this.cultureList.Add(CultureInfo.CreateSpecificCulture("en-GB"));
            this.cultureList.Add(CultureInfo.CreateSpecificCulture("el-GR"));
            this.cultureList.Add(CultureInfo.CreateSpecificCulture("en-US"));
            this.LanguageComboBox.Items.Add("English");
            this.ThemesComboBox.Items.Add("Default");
            this.ThemesComboBox.Items.Add("Blue");
            this.SystemSettingsComboBox.Items.Add("Default");
            themeList.Add(new Theme());
            themeList[0].color.Add(SystemColors.Control);
            themeList[0].color.Add(SystemColors.ControlText);
            themeList[0].color.Add(SystemColors.Window);
            themeList[0].color.Add(SystemColors.ControlText);
            themeList[0].color.Add(SystemColors.Control);
            themeList[0].color.Add(Color.ForestGreen);
            themeList[0].color.Add(SystemColors.ButtonHighlight);
            themeList[0].color.Add(SystemColors.ControlText);
            themeList[0].color.Add(SystemColors.Window);
            themeList[0].color.Add(SystemColors.WindowText);
            themeList[0].color.Add(SystemColors.ControlLight);
            themeList[0].color.Add(SystemColors.Control);
            themeList[0].color.Add(Color.FromArgb(8, 76, 169));
            themeList[0].color.Add(SystemColors.InactiveCaption);
            themeList[0].color.Add(SystemColors.ControlText); 
            themeList[0].color.Add(SystemColors.Control);
            themeList[0].color.Add(SystemColors.ControlText);
            themeList[0].color.Add(Color.FromArgb(168, 170, 172));
            themeList[0].color.Add(Color.FromArgb(75, 76, 77));
            themeList[0].color.Add(Color.FromArgb(40, 40, 120));
            themeList[0].color.Add(Color.FromArgb(22, 23, 251));
            themeList[0].color.Add(Color.DarkGreen);
            themeList[0].color.Add(Color.RoyalBlue);
            themeList[0].color.Add(Color.IndianRed);
            themeList[0].color.Add(SystemColors.ControlText);
            themeList[0].color.Add(Color.DarkGreen);
            themeList[0].color.Add(Color.RoyalBlue);
            themeList[0].color.Add(Color.RoyalBlue);
            themeList[0].color.Add(SystemColors.Control);
            themeList[0].color.Add(SystemColors.Control); 
            themeList[0].color.Add(Color.RoyalBlue);
            themeList[0].color.Add(SystemColors.ButtonFace);
            themeList[0].color.Add(SystemColors.Control);
            themeList[0].color.Add(SystemColors.Control);
            themeList[0].color.Add(SystemColors.Control);
            themeList[0].color.Add(SystemColors.ControlLight);
            themeList[0].alpha = 255;
            themeList[0].font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);

            themeList.Add(new Theme());
            themeList[1].color.Add(SystemColors.ActiveCaption);
            themeList[1].color.Add(SystemColors.ControlText);
            themeList[1].color.Add(Color.FromArgb(155, 157, 220));
            themeList[1].color.Add(SystemColors.ControlText);
            themeList[1].color.Add(SystemColors.InactiveCaption);
            themeList[1].color.Add(Color.ForestGreen);
            themeList[1].color.Add(SystemColors.Control);
            themeList[1].color.Add(SystemColors.ControlText);
            themeList[1].color.Add(SystemColors.Window);
            themeList[1].color.Add(SystemColors.WindowText);
            themeList[1].color.Add(SystemColors.ActiveCaption);
            themeList[1].color.Add(SystemColors.InactiveCaption); 
            themeList[1].color.Add(Color.FromArgb(8, 76, 169));
            themeList[1].color.Add(SystemColors.GradientActiveCaption);
            themeList[1].color.Add(SystemColors.ControlText);
            themeList[1].color.Add(SystemColors.InactiveCaption);
            themeList[1].color.Add(SystemColors.ControlText);
            themeList[1].color.Add(Color.FromArgb(168, 170, 172));
            themeList[1].color.Add(Color.FromArgb(75, 76, 77));
            themeList[1].color.Add(Color.FromArgb(40, 40, 120));
            themeList[1].color.Add(Color.FromArgb(22, 23, 251));
            themeList[1].color.Add(Color.DarkGreen);
            themeList[1].color.Add(Color.RoyalBlue);
            themeList[1].color.Add(Color.IndianRed);
            themeList[1].color.Add(SystemColors.ControlText);
            themeList[1].color.Add(Color.DarkGreen);
            themeList[1].color.Add(Color.RoyalBlue);
            themeList[1].color.Add(Color.RoyalBlue);
            themeList[1].color.Add(SystemColors.InactiveCaption);
            themeList[1].color.Add(SystemColors.InactiveCaption); 
            themeList[1].color.Add(Color.RoyalBlue);
            themeList[1].color.Add(SystemColors.InactiveCaption);
            themeList[1].color.Add(SystemColors.InactiveCaption);
            themeList[1].color.Add(SystemColors.InactiveCaption);
            themeList[1].color.Add(SystemColors.InactiveCaption);
            themeList[1].color.Add(SystemColors.ControlLight);
            themeList[1].alpha = 255;
            themeList[1].font = new Font("Segoe UI", 8.25f, FontStyle.Regular);
            readThemes();
            this.ThemesComboBox.Items.Add("Custom");
            readSettings();

            readLanguages();

            setDefTheme();
            setDefLanguage();

            setComboBoxes();

            this.mainForm.initPlayersDataTableColumns(ref playersColumnsWidth);
            this.mainForm.initStaffDataTableColumns(ref staffColumnsWidth);
            this.mainForm.initTeamsDataTableColumns(ref teamsColumnsWidth);
            this.mainForm.initShortlistDataTableColumns(ref shortlistColumnsWidth);

            setDefaultColumns();
        }

        public void setColors()
        {
            this.BackColor = this.mainForm.colorMain;
            this.ForeColor = this.mainForm.colorMainText;
            this.Font = this.mainForm.themeFont;
            this.MainTabControl.BackColor = this.mainForm.colorMain;
            this.MainTabControl.ForeColor = this.mainForm.colorMainText;
            this.GeneralSettingsTabPagePanel.BackColor = this.mainForm.colorPanels;
            this.PlayersColumnsTabPagePanel.BackColor = this.mainForm.colorPanels;
            this.StaffColumnsTabPagePanel.BackColor = this.mainForm.colorPanels;
            this.TeamsColumnsTabPagePanel.BackColor = this.mainForm.colorPanels;
            this.ShortlistColumnsTabPagePanel.BackColor = this.mainForm.colorPanels;
            this.VisualSettingsTabPagePanel.BackColor = this.mainForm.colorPanels;
            this.mainForm.colorGroupBoxPreferences(ref this.GeneralSettingsGroupBox);
            this.mainForm.colorGroupBoxPreferences(ref this.PlayersColumnSettingsGroupBox);
            this.mainForm.colorGroupBoxPreferences(ref this.StaffColumnSettingsGroupBox);
            this.mainForm.colorGroupBoxPreferences(ref this.TeamsColumnSettingsGroupBox);
            this.mainForm.colorGroupBoxPreferences(ref this.ShortlistColumnSettingsGroupBox);
            this.mainForm.colorGroupBoxPreferences(ref this.VisualSettingsGroupBox);
            this.mainForm.colorCheckedListBoxPreferences(ref this.PlayersColumnsCheckedListBox);
            this.mainForm.colorCheckedListBoxPreferences(ref this.StaffColumnsCheckedListBox);
            this.mainForm.colorCheckedListBoxPreferences(ref this.TeamsColumnsCheckedListBox);
            this.mainForm.colorCheckedListBoxPreferences(ref this.ShortlistColumnsCheckedListBox);
            this.mainForm.colorTrackBarPreferences(ref this.GroupBoxesTransparencyTrackBar);
            this.mainForm.colorInfo(ref this.GroupBoxesTransparencyResultLabel);

            this.mainForm.colorInfo(ref this.WonderkidsMaxAgeLabel);
            this.mainForm.colorInfo(ref this.WonderkidsMinPALabel);
            this.mainForm.colorInfo(ref this.WonderstaffMinPALabel);
            this.mainForm.colorInfo(ref this.WonderteamsMinRepLabel);

            this.mainForm.colorMainSet(ref this.SaveChangesButton);
            this.mainForm.colorMainSet(ref this.CancelChangesButton);
            this.mainForm.colorInfo(ref this.SaveSettingsButton);
            this.mainForm.colorInfo(ref this.PlayerColumnsSelectAllButton);
            this.mainForm.colorInfo(ref this.PlayerColumnsClearButton);
            this.mainForm.colorInfo(ref this.PlayerColumnsDefaultButton);
            this.mainForm.colorInfo(ref this.StaffColumnsSelectAllButton);
            this.mainForm.colorInfo(ref this.StaffColumnsClearButton);
            this.mainForm.colorInfo(ref this.StaffColumnsDefaultButton);
            this.mainForm.colorInfo(ref this.TeamColumnsSelectAllButton);
            this.mainForm.colorInfo(ref this.TeamColumnsClearButton);
            this.mainForm.colorInfo(ref this.TeamColumnsDefaultButton);
            this.mainForm.colorInfo(ref this.ShortlistColumnsSelectAllButton);
            this.mainForm.colorInfo(ref this.ShortlistColumnsClearButton);
            this.mainForm.colorInfo(ref this.ShortlistColumnsDefaultButton);
            this.mainForm.colorInfo(ref this.SaveThemeButton);

            this.mainForm.colorInfo(ref this.CurrencyLabel);
            this.mainForm.colorInfo(ref this.WagesLabel);
            this.mainForm.colorInfo(ref this.WeightLabel);
            this.mainForm.colorInfo(ref this.HeightLabel);
            this.mainForm.colorInfo(ref this.AllowEditingLabel);
            this.mainForm.colorInfo(ref this.LanguageLabel);
            this.mainForm.colorInfo(ref this.ColorThemesLabel);
            this.mainForm.colorInfo(ref this.TableLabel);
            this.mainForm.colorInfo(ref this.SettingsLabel);
            this.mainForm.colorInfo(ref this.TreeLabel);
            this.mainForm.colorInfo(ref this.TreeViewToolbarLabel);
            this.mainForm.colorInfo(ref this.MainToolbarLabel);
            this.mainForm.colorInfo(ref this.FileMenuLabel);
            this.mainForm.colorInfo(ref this.MainLabel);
            this.mainForm.colorInfo(ref this.TableGridLabel);
            this.mainForm.colorInfo(ref this.ProfileFieldsOnEditLabel);
            this.mainForm.colorInfo(ref this.ProfileFieldsLabel);
            this.mainForm.colorInfo(ref this.BackgroundFontSelectLabel);
            this.mainForm.colorInfo(ref this.FontLabel);
            this.mainForm.colorInfo(ref this.GenericBackgroundLabel);
            this.mainForm.colorInfo(ref this.GenericTextColorLabel);
            this.mainForm.colorInfo(ref this.OtherSpecialTextColorsLabel);
            this.mainForm.colorInfo(ref this.SpecialTableColorsLabel);
            this.mainForm.colorInfo(ref this.ProfileAttributesLabel);
            this.mainForm.colorInfo(ref this.ExcellentAttributeLabel);
            this.mainForm.colorInfo(ref this.GoodAttributeLabel);
            this.mainForm.colorInfo(ref this.MediumAttributeLabel);
            this.mainForm.colorInfo(ref this.LowAttributeLabel);
            this.mainForm.colorInfo(ref this.GameDateLabel);
            this.mainForm.colorInfo(ref this.FreeStaffLabel);
            this.mainForm.colorInfo(ref this.CoContractPlayerLabel);
            this.mainForm.colorInfo(ref this.FreePlayerLabel);
            this.mainForm.colorInfo(ref this.LoanPlayerLabel);
            this.mainForm.colorInfo(ref this.SystemSettingsLabel);
            this.mainForm.colorInfo(ref this.SearchFieldsLabel);
            this.mainForm.colorInfo(ref this.NationalStaffLabel);
            this.mainForm.colorInfo(ref this.NationalTeamLabel);
            this.mainForm.colorInfo(ref this.EUMemberPlayerLabel);
            this.mainForm.colorInfo(ref this.WonderkidsMaxAgeLabel);
            this.mainForm.colorInfo(ref this.WonderkidsMinPALabel);
            this.mainForm.colorInfo(ref this.WonderstaffMinPALabel);
            this.mainForm.colorInfo(ref this.WonderteamsMinRepLabel);
            this.mainForm.colorInfo(ref this.GroupBoxesLabel);
            this.mainForm.colorInfo(ref this.ProfileHeadersLabel); 
            this.mainForm.colorInfo(ref this.BordersLabel);
            this.mainForm.colorInfo(ref this.TransparencyLabel);
            this.mainForm.colorInfo(ref this.PanelsLabel);

            this.mainForm.colorMainSet(ref this.SystemSettingsComboBox);
            this.mainForm.colorMainSet(ref this.LanguageComboBox);
            this.mainForm.colorMainSet(ref this.AllowEditingComboBox);
            this.mainForm.colorMainSet(ref this.WeightComboBox);
            this.mainForm.colorMainSet(ref this.HeightComboBox);
            this.mainForm.colorMainSet(ref this.WagesComboBox);
            this.mainForm.colorMainSet(ref this.CurrencyComboBox);
            this.mainForm.colorMainSet(ref this.WonderkidsMaxAgeNumericUpDown);
            this.mainForm.colorMainSet(ref this.WonderkidsMinPANumericUpDown);
            this.mainForm.colorMainSet(ref this.WonderstaffMinPANumericUpDown);
            this.mainForm.colorMainSet(ref this.WonderteamsMinRepNumericUpDown);
            this.mainForm.colorMainSet(ref this.ThemesComboBox);
        }

        public void setComboBoxes()
        {
            if (this.CurrencyComboBox.SelectedItem.Equals("British Pound"))
            {
                this.currency = 1.0f;
            }
            else if (this.CurrencyComboBox.SelectedItem.Equals("Euro"))
            {
                this.currency = 1.22f;
            }
            else if (this.CurrencyComboBox.SelectedItem.Equals("US Dollars"))
            {
                this.currency = 1.89f;
            }

            if (this.WagesComboBox.SelectedItem.Equals("Weekly"))
            {
                wagesMultiplier = 1;
                wagesString = " Weekly";
            }
            else if (this.WagesComboBox.SelectedItem.Equals("Monthly"))
            {
                wagesMultiplier = 4;
                wagesString = " Monthly";
            }
            else if (this.WagesComboBox.SelectedItem.Equals("Yearly"))
            {
                wagesMultiplier = 52;
                wagesString = " Yearly";
            }

            if (this.HeightComboBox.SelectedItem.Equals("Centimeters"))
            {
                heightMultiplier = 1.0f;
                heightString = " cm";
            }
            else if (this.HeightComboBox.SelectedItem.Equals("Meters"))
            {
                heightMultiplier = 0.01f;
                heightString = " meters";
            }

            if (this.WeightComboBox.SelectedItem.Equals("Kilos"))
            {
                weightMultiplier = 1.0f;
                weightString = " kg";
            }
            else if (this.WeightComboBox.SelectedItem.Equals("Pounds"))
            {
                weightMultiplier = 2.2f;
                weightString = " lbs";
            }

            if (this.AllowEditingComboBox.SelectedIndex == 0)
                allowEditing = false;
            else
                allowEditing = true;

            this.wonderkidsMaxAge = (int)this.WonderkidsMaxAgeNumericUpDown.Value;
            this.wonderkidsMinPA = (int)this.WonderkidsMinPANumericUpDown.Value;
            this.wonderstaffMinPA = (int)this.WonderstaffMinPANumericUpDown.Value;
            this.wonderteamsMinRep = (int)this.WonderteamsMinRepNumericUpDown.Value;

            if (curEditing != this.allowEditing)
                curEditing = this.allowEditing;

            setTheme();
            
            this.mainForm.updateChanges(curEditing);
        }

        public void setSettings()
        {
            this.CurrencyComboBox.SelectedItem = curSettings.defCurrency;
            if (this.CurrencyComboBox.SelectedItem == null)
                this.CurrencyComboBox.SelectedItem = curSettings.defCurrency;
            this.WagesComboBox.SelectedItem = curSettings.defWages;
            if (this.WagesComboBox.SelectedItem == null)
                this.WagesComboBox.SelectedItem = curSettings.defWages; 
            this.HeightComboBox.SelectedItem = curSettings.defHeight;
            if (this.HeightComboBox.SelectedItem == null)
                this.HeightComboBox.SelectedItem = curSettings.defHeight; 
            this.WeightComboBox.SelectedItem = curSettings.defWeight;
            if (this.WeightComboBox.SelectedItem == null)
                this.WeightComboBox.SelectedItem = curSettings.defWeight; 
            this.AllowEditingComboBox.SelectedItem = curSettings.defEditing;
            if (this.AllowEditingComboBox.SelectedItem == null)
                this.AllowEditingComboBox.SelectedItem = curSettings.defEditing; 
            this.LanguageComboBox.SelectedItem = curSettings.defLanguage;
            if (this.LanguageComboBox.SelectedItem == null)
                this.LanguageComboBox.SelectedItem = curSettings.defLanguage; 
            this.ThemesComboBox.SelectedItem = curSettings.defColorTheme;
            if (this.ThemesComboBox.SelectedItem == null)
                this.ThemesComboBox.SelectedItem = curSettings.defColorTheme;

            this.WonderkidsMaxAgeNumericUpDown.Value = curSettings.wonderkidsMaxAge;
            this.WonderkidsMinPANumericUpDown.Value = curSettings.wonderkidsMinPA;
            this.WonderstaffMinPANumericUpDown.Value = curSettings.wonderstaffMinPA;
            this.WonderteamsMinRepNumericUpDown.Value = curSettings.wonderteamsMinRep;

            // restore player columns
            for (int i = 0; i < this.PlayersColumnsCheckedListBox.Items.Count; ++i)
                this.PlayersColumnsCheckedListBox.SetItemChecked(i, false);
            for (int i = 0; i < curSettings.playersDefColumns.Count; ++i)
                this.PlayersColumnsCheckedListBox.SetItemChecked(curSettings.playersDefColumns[i], true);
            // restore staff columns
            for (int i = 0; i < this.StaffColumnsCheckedListBox.Items.Count; ++i)
                this.StaffColumnsCheckedListBox.SetItemChecked(i, false);
            for (int i = 0; i < curSettings.staffDefColumns.Count; ++i)
                this.StaffColumnsCheckedListBox.SetItemChecked(curSettings.staffDefColumns[i], true);
            // restore teams columns
            for (int i = 0; i < this.TeamsColumnsCheckedListBox.Items.Count; ++i)
                this.TeamsColumnsCheckedListBox.SetItemChecked(i, false);
            for (int i = 0; i < curSettings.teamsDefColumns.Count; ++i)
                this.TeamsColumnsCheckedListBox.SetItemChecked(curSettings.teamsDefColumns[i], true);
            // restore shortlist columns
            for (int i = 0; i < this.ShortlistColumnsCheckedListBox.Items.Count; ++i)
                this.ShortlistColumnsCheckedListBox.SetItemChecked(i, false);
            for (int i = 0; i < curSettings.shortlistDefColumns.Count; ++i)
                this.ShortlistColumnsCheckedListBox.SetItemChecked(curSettings.shortlistDefColumns[i], true);
        }

        public void readSettings()
        {
            string path = System.Environment.CurrentDirectory + "\\Settings";
            DirectoryInfo di = new DirectoryInfo(path);
            int mostRecentDateindex = 0;
            if (!di.Exists)
                Directory.CreateDirectory(path);
            else
            {
                FileInfo[] rgFiles = di.GetFiles("*.set");
                int counter = 0;
                DateTime lastDate = DateTime.MinValue;
                foreach (FileInfo fi in rgFiles)
                {
                    string name = fi.Name.Substring(0, fi.Name.Length - 4);
                    this.SystemSettingsComboBox.Items.Add(name);
                    using (FileStream stream = new FileStream(path + "\\" + fi.Name, FileMode.Open))
                    {
                        using (StreamReader sw = new StreamReader(stream))
                        {
                            string readLine;
                            settingsList.Add(new Settings());
                            int index = this.settingsList.Count - 1;
                            char[] sep = { ' ' };

                            readLine = sw.ReadLine();
                            DateTime setDateTime = new DateTime(Int64.Parse(readLine));

                            ++counter;
                            if (setDateTime > lastDate)
                            {
                                mostRecentDateindex = counter;
                                lastDate = setDateTime;
                            }
                            while (!sw.EndOfStream)
                            {
                                readLine = sw.ReadLine();

                                string[] ar = readLine.Split(sep);
                                if (ar.Length == 2 || ar.Length == 3)
                                {
                                    if (ar[0].Equals("ColorTheme"))
                                    {
                                        if (this.ThemesComboBox.Items.Contains(ar[1]))
                                            settingsList[index].defColorTheme = ar[1];
                                    }
                                    else if (ar[0].Equals("Language"))
                                    {
                                        if (this.LanguageComboBox.Items.Contains(ar[1]))
                                            settingsList[index].defLanguage = ar[1];
                                    }
                                    else if (ar[0].Equals("Currency"))
                                    {
                                        string concat = ar[1];
                                        if (ar.Length == 3)
                                            concat += " " + ar[2];
                                        if (this.CurrencyComboBox.Items.Contains(concat))
                                            settingsList[index].defCurrency += concat;
                                    }
                                    else if (ar[0].Equals("Wages"))
                                    {
                                        if (this.WagesComboBox.Items.Contains(ar[1]))
                                            settingsList[index].defWages = ar[1];
                                    }
                                    else if (ar[0].Equals("Height"))
                                    {
                                        if (this.HeightComboBox.Items.Contains(ar[1]))
                                            settingsList[index].defHeight = ar[1];
                                    }
                                    else if (ar[0].Equals("Weight"))
                                    {
                                        if (this.WeightComboBox.Items.Contains(ar[1]))
                                            settingsList[index].defWeight = ar[1];
                                    }
                                    else if (ar[0].Equals("Editing"))
                                    {
                                        if (this.AllowEditingComboBox.Items.Contains(ar[1]))
                                            settingsList[index].defEditing = ar[1];
                                    }
                                    else if (ar[0].Equals("WonderkidsMaxAge"))
                                    {
                                        settingsList[index].wonderkidsMaxAge = Int32.Parse(ar[1]);
                                    }
                                    else if (ar[0].Equals("WonderkidsMinPA"))
                                    {
                                        settingsList[index].wonderkidsMinPA = Int32.Parse(ar[1]);
                                    }
                                    else if (ar[0].Equals("WonderstaffMinPA"))
                                    {
                                        settingsList[index].wonderstaffMinPA = Int32.Parse(ar[1]);
                                    }
                                    else if (ar[0].Equals("WonderteamsMinRep"))
                                    {
                                        settingsList[index].wonderteamsMinRep = Int32.Parse(ar[1]);
                                    }
                                }
                                else if (ar.Length > 0)
                                {
                                    if (ar[0].Equals("PlayerColumns"))
                                    {
                                        // restore player columns
                                        for (int i = 1; i < ar.Length - 1; ++i)
                                            settingsList[index].playersDefColumns.Add(Int32.Parse(ar[i]));
                                    }
                                    else if (ar[0].Equals("StaffColumns"))
                                    {
                                        // restore staff columns
                                        for (int i = 1; i < ar.Length - 1; ++i)
                                            settingsList[index].staffDefColumns.Add(Int32.Parse(ar[i]));
                                    }
                                    if (ar[0].Equals("TeamColumns"))
                                    {
                                        // restore teams columns
                                        for (int i = 1; i < ar.Length - 1; ++i)
                                            settingsList[index].teamsDefColumns.Add(Int32.Parse(ar[i]));
                                    }
                                    if (ar[0].Equals("ShortlistColumns"))
                                    {
                                        // restore shortlist columns
                                        for (int i = 1; i < ar.Length - 1; ++i)
                                            settingsList[index].shortlistDefColumns.Add(Int32.Parse(ar[i]));
                                    }
                                }
                                else
                                    return;
                            }
                        }
                    }
                }
            }
            curSettings = this.settingsList[mostRecentDateindex];
            this.SystemSettingsComboBox.SelectedIndex = mostRecentDateindex;
            setSettings();
        }

        public void readThemes()
        {
            string path = System.Environment.CurrentDirectory + "\\Themes";
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
                Directory.CreateDirectory(path);
            else
            {
                FileInfo[] rgFiles = di.GetFiles("*.the");
                foreach (FileInfo fi in rgFiles)
                {
                    this.ThemesComboBox.Items.Add(fi.Name.Substring(0, fi.Name.Length - 4));
                    using (FileStream stream = new FileStream(path + "\\" + fi.Name, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sw = new StreamReader(stream))
                        {
                            string line = "";
                            themeList.Add(new Theme());
                            int index = this.themeList.Count - 1;
                            bool isAlpha = false;
                            bool isFont = false;
                            while (!sw.EndOfStream)
                            {
                                line = sw.ReadLine();
                                if (line.Equals("Colors"))
                                {
                                    isAlpha = false;
                                    isFont = false;
                                }
                                else if (line.Equals("Alpha"))
                                {
                                    isAlpha = true;
                                    isFont = false;
                                }
                                else if (line.Equals("Font"))
                                {
                                    isAlpha = false;
                                    isFont = true;
                                }
                                else
                                {
                                    if (!isAlpha && !isFont)
                                        themeList[index].color.Add(Color.FromArgb(Int32.Parse(line)));
                                    else if (isAlpha)
                                        themeList[index].alpha = Int32.Parse(line);
                                    else if (isFont)
                                    {
                                        string fontName = line;
                                        float fontSize = float.Parse(sw.ReadLine());
                                        string fontStylestr = sw.ReadLine();
                                        FontStyle fontStyle = FontStyle.Regular;
                                        foreach (FontStyle item in Enum.GetValues(typeof(FontStyle)))
                                        {
                                            if (fontStylestr.Equals(item.ToString()))
                                            {
                                                fontStyle = item;
                                                break;
                                            }
                                        }
                                        themeList[index].font = new Font(fontName, fontSize, fontStyle);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void setDefTheme()
        {
            curColorTheme = this.ThemesComboBox.SelectedItem.ToString();
            this.mainForm.setThemes(this.themeList[this.ThemesComboBox.SelectedIndex]);
            this.MainTabControl.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
            this.GeneralSettingsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
            this.PlayersColumnsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
            this.StaffColumnsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
            this.TeamsColumnsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
            this.ShortlistColumnsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
            this.VisualSettingsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
        }

        public void setTheme()
        {
            if (this.ThemesComboBox.SelectedItem.ToString().Equals("Custom"))
            {
                Theme customTheme = new Theme();
                customTheme.color.Add(this.FileMenuColorLabel.BackColor);
                customTheme.color.Add(this.FileMenuTextColorLabel.BackColor);
                customTheme.color.Add(this.TreeColorLabel.BackColor);
                customTheme.color.Add(this.TreeTextColorLabel.BackColor);
                customTheme.color.Add(this.SettingsPanelColorLabel.BackColor);
                customTheme.color.Add(this.SettingsPanelTextColorLabel.BackColor);
                customTheme.color.Add(this.TableColorLabel.BackColor);
                customTheme.color.Add(this.TableTextColorLabel.BackColor);
                customTheme.color.Add(this.SearchFieldsColorLabel.BackColor);
                customTheme.color.Add(this.SearchFieldsTextColorLabel.BackColor);
                customTheme.color.Add(this.ProfileFieldsColor1Label.BackColor);
                customTheme.color.Add(this.ProfileFieldsColor2Label.BackColor);
                customTheme.color.Add(this.ProfileFieldsTextColorLabel.BackColor);
                customTheme.color.Add(this.ProfileFieldsOnEditColorLabel.BackColor);
                customTheme.color.Add(this.ProfileFieldsOnEditTextColorLabel.BackColor);
                customTheme.color.Add(this.MainColorLabel.BackColor);
                customTheme.color.Add(this.MainTextColorLabel.BackColor);
                customTheme.color.Add(this.LowAttributeTextColorLabel.BackColor);
                customTheme.color.Add(this.MediumAttributeTextColorLabel.BackColor);
                customTheme.color.Add(this.GoodAttributeTextColorLabel.BackColor);
                customTheme.color.Add(this.ExcellentAttributeTextColorLabel.BackColor);
                customTheme.color.Add(this.FreePlayerColorLabel.BackColor);
                customTheme.color.Add(this.LoanPlayerColorLabel.BackColor);
                customTheme.color.Add(this.CoContractedColorLabel.BackColor);
                customTheme.color.Add(this.EUMemberColorLabel.BackColor);
                customTheme.color.Add(this.FreeStaffColorLabel.BackColor);
                customTheme.color.Add(this.NationalStaffColorLabel.BackColor);
                customTheme.color.Add(this.NationalTeamColorLabel.BackColor);
                customTheme.color.Add(this.MainToolbarColorLabel.BackColor);
                customTheme.color.Add(this.TreeToolbarColorLabel.BackColor);
                customTheme.color.Add(this.GameDateTextColorLabel.BackColor);
                customTheme.color.Add(this.TableGridColorLabel.BackColor);
                customTheme.color.Add(this.PanelsColorLabel.BackColor);
                customTheme.color.Add(this.ProfileHeadersColorLabel.BackColor);
                customTheme.color.Add(this.GroupBoxesColorLabel.BackColor);
                customTheme.color.Add(this.BordersColorsLabel.BackColor);
                customTheme.alpha = (int)this.GroupBoxesTransparencyTrackBar.Value;
                char[] sep = { ',' };
                string[] font = this.BackgroundFontSelectLabel.Text.Split(sep);
                FontStyle fontStyle = FontStyle.Regular;
                if (font[1][0].Equals(' '))
                    font[1] = font[1].Substring(1); 
                if (font[2][0].Equals(' '))
                    font[2] = font[2].Substring(1);
                foreach (FontStyle item in Enum.GetValues(typeof(FontStyle)))
                {
                    if (font[2].Equals(item.ToString()))
                    {
                        fontStyle = item;
                        break;
                    }
                }
                customTheme.font = new Font(font[0], float.Parse(font[1]), fontStyle);
                curFont = customTheme.font;
                this.mainForm.setThemes(customTheme);
                this.MainTabControl.BackColor = customTheme.color[15];
                this.GeneralSettingsTabPage.BackColor = customTheme.color[15];
                this.PlayersColumnsTabPage.BackColor = customTheme.color[15];
                this.StaffColumnsTabPage.BackColor = customTheme.color[15];
                this.TeamsColumnsTabPage.BackColor = customTheme.color[15];
                this.ShortlistColumnsTabPage.BackColor = customTheme.color[15];
                this.VisualSettingsTabPage.BackColor = customTheme.color[15];
                curColorTheme = this.ThemesComboBox.SelectedItem.ToString();
            }
            else if (!this.ThemesComboBox.SelectedItem.ToString().Equals(curColorTheme))
            {
                this.mainForm.setThemes(this.themeList[this.ThemesComboBox.SelectedIndex]);
                this.MainTabControl.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
                this.GeneralSettingsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
                this.PlayersColumnsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
                this.StaffColumnsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
                this.TeamsColumnsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
                this.ShortlistColumnsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
                this.VisualSettingsTabPage.BackColor = this.themeList[this.ThemesComboBox.SelectedIndex].color[15];
                curColorTheme = this.ThemesComboBox.SelectedItem.ToString();
            }
        }

        public void readLanguages()
        {
            string path = System.Environment.CurrentDirectory + "\\Languages\\";
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
                Directory.CreateDirectory(path);
            else
            {
                FileInfo[] rgFiles = di.GetFiles("*.lng");
                foreach (FileInfo fi in rgFiles)
                {
                    if (!fi.Name.Substring(0, fi.Name.Length - 4).Equals(curSettings.defLanguage))
                    {
                        this.LanguageComboBox.Items.Add(fi.Name.Substring(0, fi.Name.Length - 4));
                    }
                    using (FileStream stream = new FileStream(path + fi.Name, FileMode.Open))
                    {
                        using (StreamReader sw = new StreamReader(stream))
                        {
                            string line;
                            while (!sw.EndOfStream)
                            {
                                line = sw.ReadLine();
                            }
                        }
                    }
                }
            }
        }

        public void setDefLanguage()
        {
            if (!curLanguage.Equals(this.LanguageComboBox.SelectedItem.ToString()))
            {
                this.mainForm.setLanguage(this.LanguageComboBox.SelectedItem.ToString());
                curLanguage = this.LanguageComboBox.SelectedItem.ToString();
            }
        }

        public void setDefaultColumns()
        {
            setPlayersDefaultColumns();
            setStaffDefaultColumns();
            setTeamsDefaultColumns();
            setShortlistDefaultColumns();
            setColumns();
        }

        public void setPlayersDefaultColumns()
        {
            // restore player columns
            for (int i = 0; i < this.PlayersColumnsCheckedListBox.Items.Count; ++i)
                this.PlayersColumnsCheckedListBox.SetItemChecked(i, false);
            for (int i = 0; i < curSettings.playersDefColumns.Count; ++i)
                this.PlayersColumnsCheckedListBox.SetItemChecked(curSettings.playersDefColumns[i], true);
        }

        public void setStaffDefaultColumns()
        {
            // restore staff columns
            for (int i = 0; i < this.StaffColumnsCheckedListBox.Items.Count; ++i)
                this.StaffColumnsCheckedListBox.SetItemChecked(i, false);
            for (int i = 0; i < curSettings.staffDefColumns.Count; ++i)
                this.StaffColumnsCheckedListBox.SetItemChecked(curSettings.staffDefColumns[i], true);
        }

        public void setTeamsDefaultColumns()
        {
            // restore teams columns
            for (int i = 0; i < this.TeamsColumnsCheckedListBox.Items.Count; ++i)
                this.TeamsColumnsCheckedListBox.SetItemChecked(i, false);
            for (int i = 0; i < curSettings.teamsDefColumns.Count; ++i)
                this.TeamsColumnsCheckedListBox.SetItemChecked(curSettings.teamsDefColumns[i], true);
        }

        public void setShortlistDefaultColumns()
        {
            // restore shortlist columns
            for (int i = 0; i < this.ShortlistColumnsCheckedListBox.Items.Count; ++i)
                this.ShortlistColumnsCheckedListBox.SetItemChecked(i, false);
            for (int i = 0; i < curSettings.shortlistDefColumns.Count; ++i)
                this.ShortlistColumnsCheckedListBox.SetItemChecked(curSettings.shortlistDefColumns[i], true);
        }

        public void setColumns()
        {
            setPlayersColumns();
            setStaffColumns();
            setTeamsColumns();
            setShortlistColumns();
        }

        internal void setPlayersColumns()
        {
            playersColumns.Clear();
            int index = 0;
            for (int i = 0; i < this.PlayersColumnsCheckedListBox.CheckedIndices.Count; ++i)
            {
                index = this.PlayersColumnsCheckedListBox.CheckedIndices[i];
                playersColumns.Add(index);
            }
            this.mainForm.setPlayersDataTableColumns(ref playersColumns);
        }

        internal void setStaffColumns()
        {
            staffColumns.Clear();
            int index = 0;
            for (int i = 0; i < this.StaffColumnsCheckedListBox.CheckedIndices.Count; ++i)
            {
                index = this.StaffColumnsCheckedListBox.CheckedIndices[i];
                staffColumns.Add(index);
            }
            this.mainForm.setStaffDataTableColumns(ref staffColumns);
        }

        internal void setTeamsColumns()
        {
            teamsColumns.Clear();
            int index = 0;
            for (int i = 0; i < this.TeamsColumnsCheckedListBox.CheckedIndices.Count; ++i)
            {
                index = this.TeamsColumnsCheckedListBox.CheckedIndices[i];
                teamsColumns.Add(index);
            }
            this.mainForm.setTeamsDataTableColumns(ref teamsColumns);
        }

        internal void setShortlistColumns()
        {
            shortlistColumns.Clear();
            int index = 0;
            for (int i = 0; i < this.ShortlistColumnsCheckedListBox.CheckedIndices.Count; ++i)
            {
                index = this.ShortlistColumnsCheckedListBox.CheckedIndices[i];
                shortlistColumns.Add(index);
            }
            this.mainForm.setShortlistDataTableColumns(ref shortlistColumns);
        }

        private void SaveChangesButton_Click(object sender, EventArgs e)
        {
            setTheme();

            if (!curLanguage.Equals(this.LanguageComboBox.SelectedItem.ToString()))
            {
                this.mainForm.setLanguage(this.LanguageComboBox.SelectedItem.ToString());
                curLanguage = this.LanguageComboBox.SelectedItem.ToString();
            }

            setComboBoxes();

            setColumns();

            this.mainForm.setLabels();

            this.Hide();
        }

        private void CancelChangesButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void PlayerColumnsSelectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.PlayersColumnsCheckedListBox.Items.Count; ++i)
                this.PlayersColumnsCheckedListBox.SetItemChecked(i, true);
        }

        private void PlayerColumnsClearButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.PlayersColumnsCheckedListBox.Items.Count; ++i)
                this.PlayersColumnsCheckedListBox.SetItemChecked(i, false);
        }

        private void PlayerColumnsDefaultButton_Click(object sender, EventArgs e)
        {
            PlayerColumnsClearButton_Click(sender, e);
            setPlayersDefaultColumns();
        }

        private void StaffColumnsSelectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.StaffColumnsCheckedListBox.Items.Count; ++i)
                this.StaffColumnsCheckedListBox.SetItemChecked(i, true);
        }

        private void StaffColumnsClearButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.StaffColumnsCheckedListBox.Items.Count; ++i)
                this.StaffColumnsCheckedListBox.SetItemChecked(i, false);
        }

        private void StaffColumnsDefaultButton_Click(object sender, EventArgs e)
        {
            StaffColumnsClearButton_Click(sender, e);
            setStaffDefaultColumns();
        }

        private void TeamColumnsSelectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.TeamsColumnsCheckedListBox.Items.Count; ++i)
                this.TeamsColumnsCheckedListBox.SetItemChecked(i, true);
        }

        private void TeamColumnsClearButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.TeamsColumnsCheckedListBox.Items.Count; ++i)
                this.TeamsColumnsCheckedListBox.SetItemChecked(i, false);
        }

        private void TeamColumnsDefaultButton_Click(object sender, EventArgs e)
        {
            TeamColumnsClearButton_Click(sender, e);
            setTeamsDefaultColumns();
        }

        private void ShortlistColumnsSelectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.ShortlistColumnsCheckedListBox.Items.Count; ++i)
                this.ShortlistColumnsCheckedListBox.SetItemChecked(i, true);
        }

        private void ShortlistColumnsClearButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.ShortlistColumnsCheckedListBox.Items.Count; ++i)
                this.ShortlistColumnsCheckedListBox.SetItemChecked(i, false);
        }

        private void ShortlistColumnsDefaultButton_Click(object sender, EventArgs e)
        {
            ShortlistColumnsClearButton_Click(sender, e);
            setShortlistDefaultColumns();
        }

        public string CurrencyName
        {
            get { return this.CurrencyComboBox.SelectedItem.ToString(); }
        }

        public float Currency
        {
            get { return currency; }
        }

        public IFormatProvider CurrencyFormat
        {
            get { return cultureList[this.CurrencyComboBox.SelectedIndex]; }
        }

        public string WagesName
        {
            get { return this.WagesComboBox.SelectedItem.ToString(); }
        }

        public int WagesMultiplier
        {
            get { return wagesMultiplier; }
        }

        public string WagesString
        {
            get { return wagesString; }
        }

        public string HeightName
        {
            get { return this.HeightComboBox.SelectedItem.ToString(); }
        }

        public float HeightMultiplier
        {
            get { return heightMultiplier; }
        }

        public string HeightString
        {
            get { return heightString; }
        }

        public string WeightName
        {
            get { return this.WeightComboBox.SelectedItem.ToString(); }
        }

        public float WeightMultiplier
        {
            get { return weightMultiplier; }
        }

        public string WeightString
        {
            get { return weightString; }
        }

        public string NumberFormat
        {
            get { return numberFormat; }
        }

        public bool AllowEditing
        {
            get { return allowEditing; }
        }

        public int WonderkidsMaxAge
        {
            get { return wonderkidsMaxAge; }
        }

        public int WonderkidsMinPA
        {
            get { return wonderkidsMinPA; }
        }

        public int WonderstaffMinPA
        {
            get { return wonderstaffMinPA; }
        }

        public int WonderteamsMinRep
        {
            get { return wonderteamsMinRep; }
        }

        public CheckedListBox PlayersColumnsCheckedList
        {
            get { return PlayersColumnsCheckedListBox; }
        }

        public CheckedListBox StaffColumnsCheckedList
        {
            get { return StaffColumnsCheckedListBox; }
        }

        public CheckedListBox TeamsColumnsCheckedList
        {
            get { return TeamsColumnsCheckedListBox; }
        }

        public CheckedListBox ShortlistColumnsCheckedList
        {
            get { return ShortlistColumnsCheckedListBox; }
        }

        private void SystemSettingsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string setting = this.SystemSettingsComboBox.SelectedItem.ToString();
            int index = this.SystemSettingsComboBox.Items.IndexOf(setting);
            if (index >= 0 && settingsList.Count != 0)
            {
                curSettings = this.settingsList[index];
                setSettings();
            }
            this.SaveSettingsButton.Enabled = true;
            if (setting.Equals("Default"))
            {
                this.ClearSystemSettingsButton.Enabled = false;
                this.ClearSystemSettingsButton.BackgroundImage = global::FMScout.Properties.Resources.deleteDisabled;
            }
            else
            {
                this.ClearSystemSettingsButton.Enabled = true;
                this.ClearSystemSettingsButton.BackgroundImage = global::FMScout.Properties.Resources.Delete;
            }
        }

        private void ThemesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ThemesComboBox.SelectedItem.ToString().Equals("Custom"))
            {
                this.SaveThemeButton.Enabled = true;
                this.ClearColorThemeButton.Enabled = false;
                this.ClearColorThemeButton.BackgroundImage = global::FMScout.Properties.Resources.deleteDisabled;
            }
            else
            {
                string color = this.ThemesComboBox.SelectedItem.ToString();
                int index = this.ThemesComboBox.Items.IndexOf(color);
                if (index >= 0 && themeList.Count != 0)
                {
                    int i = -1;
                    this.FileMenuColorLabel.BackColor = themeList[index].color[++i];
                    this.FileMenuTextColorLabel.BackColor = themeList[index].color[++i];
                    this.TreeColorLabel.BackColor = themeList[index].color[++i];
                    this.TreeTextColorLabel.BackColor = themeList[index].color[++i];
                    this.SettingsPanelColorLabel.BackColor = themeList[index].color[++i];
                    this.SettingsPanelTextColorLabel.BackColor = themeList[index].color[++i];
                    this.TableColorLabel.BackColor = themeList[index].color[++i];
                    this.TableTextColorLabel.BackColor = themeList[index].color[++i];
                    this.SearchFieldsColorLabel.BackColor = themeList[index].color[++i];
                    this.SearchFieldsTextColorLabel.BackColor = themeList[index].color[++i];
                    this.ProfileFieldsColor1Label.BackColor = themeList[index].color[++i];
                    this.ProfileFieldsColor2Label.BackColor = themeList[index].color[++i];
                    this.ProfileFieldsTextColorLabel.BackColor = themeList[index].color[++i];
                    this.ProfileFieldsOnEditColorLabel.BackColor = themeList[index].color[++i];
                    this.ProfileFieldsOnEditTextColorLabel.BackColor = themeList[index].color[++i];
                    this.MainColorLabel.BackColor = themeList[index].color[++i];
                    this.MainTextColorLabel.BackColor = themeList[index].color[++i];
                    this.LowAttributeTextColorLabel.BackColor = themeList[index].color[++i];
                    this.MediumAttributeTextColorLabel.BackColor = themeList[index].color[++i];
                    this.GoodAttributeTextColorLabel.BackColor = themeList[index].color[++i];
                    this.ExcellentAttributeTextColorLabel.BackColor = themeList[index].color[++i];
                    this.FreePlayerColorLabel.BackColor = themeList[index].color[++i];
                    this.LoanPlayerColorLabel.BackColor = themeList[index].color[++i];
                    this.CoContractedColorLabel.BackColor = themeList[index].color[++i];
                    this.EUMemberColorLabel.BackColor = themeList[index].color[++i];
                    this.FreeStaffColorLabel.BackColor = themeList[index].color[++i];
                    this.NationalStaffColorLabel.BackColor = themeList[index].color[++i];
                    this.NationalTeamColorLabel.BackColor = themeList[index].color[++i];
                    this.MainToolbarColorLabel.BackColor = themeList[index].color[++i];
                    this.TreeToolbarColorLabel.BackColor = themeList[index].color[++i]; 
                    this.GameDateTextColorLabel.BackColor = themeList[index].color[++i];
                    this.TableGridColorLabel.BackColor = themeList[index].color[++i];
                    this.PanelsColorLabel.BackColor = themeList[index].color[++i];
                    this.ProfileHeadersColorLabel.BackColor = themeList[index].color[++i];
                    this.GroupBoxesColorLabel.BackColor = themeList[index].color[++i]; 
                    this.BordersColorsLabel.BackColor = themeList[index].color[++i];
                    this.BackgroundFontSelectLabel.Text = themeList[index].font.Name + ", " + (int)themeList[index].font.Size + ", " + themeList[index].font.Style.ToString();
                    this.GroupBoxesTransparencyTrackBar.Value = themeList[index].alpha;
                }
                this.SaveThemeButton.Enabled = false;
                if (color.Equals("Default") || color.Equals("Blue"))
                {
                    this.ClearColorThemeButton.Enabled = false;
                    this.ClearColorThemeButton.BackgroundImage = global::FMScout.Properties.Resources.deleteDisabled;
                }
                else
                {
                    this.ClearColorThemeButton.Enabled = true;
                    this.ClearColorThemeButton.BackgroundImage = global::FMScout.Properties.Resources.Delete;
                }
            }
        }

        private void SaveThemeButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = System.Environment.CurrentDirectory + "\\Themes";
            saveFileDialog.DefaultExt = "the";
            // The Filter property requires a search string after the pipe ( | )
            saveFileDialog.Filter = "Themes(*.the)|*.the";
            saveFileDialog.Title = "Save a theme file";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                string ext = saveFileDialog.FileName.Substring(saveFileDialog.FileName.LastIndexOf(".") + 1);
                // Saves the file via a FileStream created by the OpenFile method.
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    stream.SetLength(0);
                    using (StreamWriter sw = new StreamWriter(stream))
                    {
                        string strValue = string.Empty;

                        strValue += "Colors" + System.Environment.NewLine;
                        strValue += this.FileMenuColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.FileMenuTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;                        
                        strValue += this.TreeColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.TreeTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.SettingsPanelColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.SettingsPanelTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.TableColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.TableTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.SearchFieldsColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.SearchFieldsTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.ProfileFieldsColor1Label.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.ProfileFieldsColor2Label.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.ProfileFieldsTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.ProfileFieldsOnEditColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.ProfileFieldsOnEditTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.MainColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.MainTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.LowAttributeTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.MediumAttributeTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.GoodAttributeTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.ExcellentAttributeTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.FreePlayerColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.LoanPlayerColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.CoContractedColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.EUMemberColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.FreeStaffColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.NationalStaffColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.NationalTeamColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.MainToolbarColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.TreeToolbarColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine; 
                        strValue += this.GameDateTextColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.TableGridColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.PanelsColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.ProfileHeadersColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.GroupBoxesColorLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += this.BordersColorsLabel.BackColor.ToArgb();
                        strValue += System.Environment.NewLine;
                        strValue += "Alpha" + System.Environment.NewLine;
                        strValue += this.GroupBoxesTransparencyTrackBar.Value;
                        strValue += System.Environment.NewLine;
                        strValue += "Font" + System.Environment.NewLine;
                        strValue += this.BackgroundFontSelectLabel.Font.Name;
                        strValue += System.Environment.NewLine;
                        strValue += (int)this.BackgroundFontSelectLabel.Font.Size;
                        strValue += System.Environment.NewLine;
                        strValue += this.BackgroundFontSelectLabel.Font.Style;
                        sw.Write(strValue);
                        sw.Close();
                    }
                    stream.Close();
                }
                string name = saveFileDialog.FileName.Substring(saveFileDialog.FileName.LastIndexOf("\\") + 1, saveFileDialog.FileName.LastIndexOf(".") - saveFileDialog.FileName.LastIndexOf("\\") - 1);

                if (this.ThemesComboBox.Items.Contains(name))
                {
                    int index = this.ThemesComboBox.Items.IndexOf(name);
                    int i = -1;
                    themeList[index].color[++i] = this.FileMenuColorLabel.BackColor;
                    themeList[index].color[++i] = this.FileMenuTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.TreeColorLabel.BackColor;
                    themeList[index].color[++i] = this.TreeTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.SettingsPanelColorLabel.BackColor;
                    themeList[index].color[++i] = this.SettingsPanelTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.TableColorLabel.BackColor;
                    themeList[index].color[++i] = this.TableTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.SearchFieldsColorLabel.BackColor;
                    themeList[index].color[++i] = this.SearchFieldsTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.ProfileFieldsColor1Label.BackColor;
                    themeList[index].color[++i] = this.ProfileFieldsColor2Label.BackColor;
                    themeList[index].color[++i] = this.ProfileFieldsTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.ProfileFieldsOnEditColorLabel.BackColor;
                    themeList[index].color[++i] = this.ProfileFieldsOnEditTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.MainColorLabel.BackColor;
                    themeList[index].color[++i] = this.MainTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.LowAttributeTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.MediumAttributeTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.GoodAttributeTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.ExcellentAttributeTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.FreePlayerColorLabel.BackColor;
                    themeList[index].color[++i] = this.LoanPlayerColorLabel.BackColor;
                    themeList[index].color[++i] = this.CoContractedColorLabel.BackColor;
                    themeList[index].color[++i] = this.EUMemberColorLabel.BackColor;
                    themeList[index].color[++i] = this.FreeStaffColorLabel.BackColor;
                    themeList[index].color[++i] = this.NationalStaffColorLabel.BackColor;
                    themeList[index].color[++i] = this.NationalTeamColorLabel.BackColor;
                    themeList[index].color[++i] = this.MainToolbarColorLabel.BackColor;
                    themeList[index].color[++i] = this.TreeToolbarColorLabel.BackColor; 
                    themeList[index].color[++i] = this.GameDateTextColorLabel.BackColor;
                    themeList[index].color[++i] = this.TableGridColorLabel.BackColor;
                    themeList[index].color[++i] = this.PanelsColorLabel.BackColor;
                    themeList[index].color[++i] = this.ProfileHeadersColorLabel.BackColor;
                    themeList[index].color[++i] = this.GroupBoxesColorLabel.BackColor; 
                    themeList[index].color[++i] = this.BordersColorsLabel.BackColor;
                    themeList[index].alpha = (int)this.GroupBoxesTransparencyTrackBar.Value;
                    char[] sep = { ',' };
                    string[] font = this.BackgroundFontSelectLabel.Text.Split(sep);
                    FontStyle fontStyle = FontStyle.Regular;
                    foreach (FontStyle item in Enum.GetValues(typeof(FontStyle)))
                    {
                        if (font[2].Equals(item.ToString()))
                            fontStyle = item;
                    }
                    themeList[index].font = new Font(font[0], float.Parse(font[1]), fontStyle);
                    this.ThemesComboBox.SelectedItem = name;
                }
                else
                {
                    this.themeList.Add(new Theme());
                    int index = this.themeList.Count - 1;
                    themeList[index].color.Add(this.FileMenuColorLabel.BackColor);
                    themeList[index].color.Add(this.FileMenuTextColorLabel.BackColor);       
                    themeList[index].color.Add(this.TreeColorLabel.BackColor);
                    themeList[index].color.Add(this.TreeTextColorLabel.BackColor);
                    themeList[index].color.Add(this.SettingsPanelColorLabel.BackColor);
                    themeList[index].color.Add(this.SettingsPanelTextColorLabel.BackColor);
                    themeList[index].color.Add(this.TableColorLabel.BackColor);
                    themeList[index].color.Add(this.TableTextColorLabel.BackColor);
                    themeList[index].color.Add(this.SearchFieldsColorLabel.BackColor);
                    themeList[index].color.Add(this.SearchFieldsTextColorLabel.BackColor);
                    themeList[index].color.Add(this.ProfileFieldsColor1Label.BackColor);
                    themeList[index].color.Add(this.ProfileFieldsColor2Label.BackColor);
                    themeList[index].color.Add(this.ProfileFieldsTextColorLabel.BackColor);
                    themeList[index].color.Add(this.ProfileFieldsOnEditColorLabel.BackColor);
                    themeList[index].color.Add(this.ProfileFieldsOnEditTextColorLabel.BackColor);
                    themeList[index].color.Add(this.MainColorLabel.BackColor);
                    themeList[index].color.Add(this.MainTextColorLabel.BackColor);
                    themeList[index].color.Add(this.LowAttributeTextColorLabel.BackColor);
                    themeList[index].color.Add(this.MediumAttributeTextColorLabel.BackColor);
                    themeList[index].color.Add(this.GoodAttributeTextColorLabel.BackColor);
                    themeList[index].color.Add(this.ExcellentAttributeTextColorLabel.BackColor);
                    themeList[index].color.Add(this.FreePlayerColorLabel.BackColor);
                    themeList[index].color.Add(this.LoanPlayerColorLabel.BackColor);
                    themeList[index].color.Add(this.CoContractedColorLabel.BackColor);
                    themeList[index].color.Add(this.EUMemberColorLabel.BackColor);
                    themeList[index].color.Add(this.FreeStaffColorLabel.BackColor);
                    themeList[index].color.Add(this.NationalStaffColorLabel.BackColor);
                    themeList[index].color.Add(this.NationalTeamColorLabel.BackColor);
                    themeList[index].color.Add(this.MainToolbarColorLabel.BackColor);
                    themeList[index].color.Add(this.TreeToolbarColorLabel.BackColor);
                    themeList[index].color.Add(this.GameDateTextColorLabel.BackColor); 
                    themeList[index].color.Add(this.TableGridColorLabel.BackColor);
                    themeList[index].color.Add(this.PanelsColorLabel.BackColor);
                    themeList[index].color.Add(this.ProfileHeadersColorLabel.BackColor);
                    themeList[index].color.Add(this.GroupBoxesColorLabel.BackColor);
                    themeList[index].color.Add(this.BordersColorsLabel.BackColor);
                    themeList[index].alpha = (int)this.GroupBoxesTransparencyTrackBar.Value;
                    char[] sep = { ',' };
                    string[] font = this.BackgroundFontSelectLabel.Text.Split(sep);
                    FontStyle fontStyle = FontStyle.Regular;
                    foreach (FontStyle item in Enum.GetValues(typeof(FontStyle)))
                    {
                        if (font[2].Equals(item.ToString()))
                            fontStyle = item;
                    }
                    themeList[index].font = new Font(font[0], float.Parse(font[1]), fontStyle);
                    this.ThemesComboBox.Items[this.ThemesComboBox.Items.Count - 1] = name;
                    this.ThemesComboBox.Items.Add("Custom");
                }
            }
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = System.Environment.CurrentDirectory + "\\Settings";
            saveFileDialog.DefaultExt = "set";
            // The Filter property requires a search string after the pipe ( | )
            saveFileDialog.Filter = "Settings(*.set)|*.set";
            saveFileDialog.Title = "Save a settings file";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog.FileName != "")
            {
                string filename = saveFileDialog.FileName;
                if (filename.Substring(filename.Length - 11, 11).ToLower().Equals("default.set"))
                {
                    filename = filename.Substring(0, filename.Length - 4);
                    filename += "1.str";
                }
                string name = filename.Substring(filename.LastIndexOf("\\") + 1, filename.LastIndexOf(".") - filename.LastIndexOf("\\") - 1);
                saveSettingsFile(name);
            }
        }

        public void saveSettingsFile(string name)
        {
            // Saves the file via a FileStream created by the OpenFile method.
            string file = System.Environment.CurrentDirectory + "\\Settings\\" + name + ".set";
            using (FileStream stream = new FileStream(file, FileMode.OpenOrCreate))
            {
                stream.SetLength(0);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    string strValue = string.Empty;

                    strValue += DateTime.Now.Ticks;
                    strValue += System.Environment.NewLine;
                    if (!this.ThemesComboBox.SelectedItem.Equals("Custom"))
                        strValue += "ColorTheme " + this.ThemesComboBox.SelectedItem;
                    else
                        strValue += "ColorTheme " + curSettings.defColorTheme;
                    strValue += System.Environment.NewLine;
                    strValue += "Language " + this.LanguageComboBox.SelectedItem;
                    strValue += System.Environment.NewLine;
                    strValue += "Currency " + this.CurrencyComboBox.SelectedItem;
                    strValue += System.Environment.NewLine;
                    strValue += "Wages " + this.WagesComboBox.SelectedItem;
                    strValue += System.Environment.NewLine;
                    strValue += "Height " + this.HeightComboBox.SelectedItem;
                    strValue += System.Environment.NewLine;
                    strValue += "Weight " + this.WeightComboBox.SelectedItem;
                    strValue += System.Environment.NewLine;
                    strValue += "Editing " + this.AllowEditingComboBox.SelectedItem;
                    strValue += System.Environment.NewLine;
                    strValue += "WonderkidsMaxAge " + this.WonderkidsMaxAgeNumericUpDown.Value;
                    strValue += System.Environment.NewLine;
                    strValue += "WonderkidsMinPA " + this.WonderkidsMinPANumericUpDown.Value;
                    strValue += System.Environment.NewLine;
                    strValue += "WonderstaffMinPA " + this.WonderkidsMaxAgeNumericUpDown.Value;
                    strValue += System.Environment.NewLine;
                    strValue += "WonderteamsMinRep " + this.WonderteamsMinRepNumericUpDown.Value;
                    strValue += System.Environment.NewLine;
                  
                    // save player columns
                    strValue += "PlayerColumns ";
                    for (int i = 0; i < this.PlayersColumnsCheckedListBox.CheckedIndices.Count; ++i)
                        strValue += this.PlayersColumnsCheckedListBox.CheckedIndices[i] + " ";
                    strValue += System.Environment.NewLine;

                    // save staff columns
                    strValue += "StaffColumns ";
                    for (int i = 0; i < this.StaffColumnsCheckedListBox.CheckedIndices.Count; ++i)
                        strValue += this.StaffColumnsCheckedListBox.CheckedIndices[i] + " ";
                    strValue += System.Environment.NewLine;

                    // save teams columns
                    strValue += "TeamColumns ";
                    for (int i = 0; i < this.TeamsColumnsCheckedListBox.CheckedIndices.Count; ++i)
                        strValue += this.TeamsColumnsCheckedListBox.CheckedIndices[i] + " ";
                    strValue += System.Environment.NewLine;

                    // save shortlist columns
                    strValue += "ShortlistColumns ";
                    for (int i = 0; i < this.ShortlistColumnsCheckedListBox.CheckedIndices.Count; ++i)
                        strValue += this.ShortlistColumnsCheckedListBox.CheckedIndices[i] + " ";

                    sw.Write(strValue);
                    sw.Close();
                }
                stream.Close();
            }
          
            if (this.SystemSettingsComboBox.Items.Contains(name))
            {
                int index = this.SystemSettingsComboBox.Items.IndexOf(name);
                curSettings = this.settingsList[index];
                this.SystemSettingsComboBox.SelectedItem = name;
                saveSettings();
                setTheme();
            }
            else
            {
                this.settingsList.Add(new Settings());
                int index = this.settingsList.Count - 1;
                curSettings = this.settingsList[index];
                saveSettings();
                this.SystemSettingsComboBox.Items.Add(name);
                this.SystemSettingsComboBox.SelectedIndex = index;
                setTheme();
            }
        }

        public void saveSettings()
        {
            curSettings.defColorTheme = this.ThemesComboBox.SelectedItem.ToString();
            curSettings.defCurrency = this.CurrencyComboBox.SelectedItem.ToString();
            curSettings.defEditing = this.AllowEditingComboBox.SelectedItem.ToString();
            curSettings.defHeight = this.HeightComboBox.SelectedItem.ToString();
            curSettings.defLanguage = this.LanguageComboBox.SelectedItem.ToString();
            curSettings.defWages = this.WagesComboBox.SelectedItem.ToString();
            curSettings.defWeight = this.WeightComboBox.SelectedItem.ToString();
            curSettings.wonderkidsMaxAge = (int)this.WonderkidsMaxAgeNumericUpDown.Value;
            curSettings.wonderkidsMinPA = (int)this.WonderkidsMinPANumericUpDown.Value;
            curSettings.wonderstaffMinPA = (int)this.WonderstaffMinPANumericUpDown.Value;
            curSettings.wonderteamsMinRep = (int)this.WonderteamsMinRepNumericUpDown.Value;
            saveDefaultColumns();
        }

        public void saveDefaultColumns()
        {
            savePlayerDefaultColumns();
            saveStaffDefaultColumns();
            saveTeamsDefaultColumns();
            saveShortlistDefaultColumns();
        }

        public void savePlayerDefaultColumns()
        {
            // save player default columns
            curSettings.playersDefColumns.Clear();
            for (int i = 0; i < this.PlayersColumnsCheckedListBox.CheckedIndices.Count; ++i)
                curSettings.playersDefColumns.Add(this.PlayersColumnsCheckedListBox.CheckedIndices[i]);
        }

        public void saveStaffDefaultColumns()
        {
            // save staff default columns
            curSettings.staffDefColumns.Clear();
            for (int i = 0; i < this.StaffColumnsCheckedListBox.CheckedIndices.Count; ++i)
                curSettings.staffDefColumns.Add(this.StaffColumnsCheckedListBox.CheckedIndices[i]);
        }

        public void saveTeamsDefaultColumns()
        {
            // save team default columns
            curSettings.teamsDefColumns.Clear();
            for (int i = 0; i < this.TeamsColumnsCheckedListBox.CheckedIndices.Count; ++i)
                curSettings.teamsDefColumns.Add(this.TeamsColumnsCheckedListBox.CheckedIndices[i]);
        }

        public void saveShortlistDefaultColumns()
        {
            // save shortlist default columns
            curSettings.shortlistDefColumns.Clear();
            for (int i = 0; i < this.ShortlistColumnsCheckedListBox.CheckedIndices.Count; ++i)
                curSettings.shortlistDefColumns.Add(this.ShortlistColumnsCheckedListBox.CheckedIndices[i]);
        }

        private void ClearSystemSettingsButton_Click(object sender, EventArgs e)
        {
            if (this.SystemSettingsComboBox.SelectedIndex > 0)
            {
                string name = System.Environment.CurrentDirectory + "\\Settings\\" + this.SystemSettingsComboBox.SelectedItem + ".set";
                int index = this.SystemSettingsComboBox.Items.IndexOf(this.SystemSettingsComboBox.SelectedItem);
                this.SystemSettingsComboBox.Items.RemoveAt(index);
                this.settingsList.RemoveAt(index);
                this.SystemSettingsComboBox.SelectedIndex = index - 1;
                curSettings = this.settingsList[this.SystemSettingsComboBox.SelectedIndex];
                setSettings();
                File.Delete(name);
                setTheme();
            }
        }

        private void ClearColorThemeButton_Click(object sender, EventArgs e)
        {
            if (this.ThemesComboBox.SelectedIndex > 1 && this.ThemesComboBox.SelectedIndex < this.ThemesComboBox.Items.Count - 1)
            {
                string name = System.Environment.CurrentDirectory + "\\Themes\\" + this.ThemesComboBox.SelectedItem + ".the";
                int index = this.ThemesComboBox.Items.IndexOf(this.ThemesComboBox.SelectedItem);
                this.ThemesComboBox.Items.RemoveAt(index);
                this.themeList.RemoveAt(index);
                this.ThemesComboBox.SelectedIndex = index - 1;
                int i = -1;
                this.FileMenuColorLabel.BackColor = themeList[index - 1].color[++i];
                this.FileMenuTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.TreeColorLabel.BackColor = themeList[index - 1].color[++i];
                this.TreeTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.SettingsPanelColorLabel.BackColor = themeList[index - 1].color[++i];
                this.SettingsPanelTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.TableColorLabel.BackColor = themeList[index - 1].color[++i];
                this.TableTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.SearchFieldsColorLabel.BackColor = themeList[index - 1].color[++i];
                this.SearchFieldsTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.ProfileFieldsColor1Label.BackColor = themeList[index - 1].color[++i];
                this.ProfileFieldsColor2Label.BackColor = themeList[index - 1].color[++i];
                this.ProfileFieldsTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.ProfileFieldsOnEditColorLabel.BackColor = themeList[index - 1].color[++i];
                this.ProfileFieldsOnEditTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.MainColorLabel.BackColor = themeList[index - 1].color[++i];
                this.MainTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.LowAttributeTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.MediumAttributeTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.GoodAttributeTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.ExcellentAttributeTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.FreePlayerColorLabel.BackColor = themeList[index - 1].color[++i];
                this.LoanPlayerColorLabel.BackColor = themeList[index - 1].color[++i];
                this.CoContractedColorLabel.BackColor = themeList[index - 1].color[++i];
                this.EUMemberColorLabel.BackColor = themeList[index - 1].color[++i];
                this.FreeStaffColorLabel.BackColor = themeList[index - 1].color[++i];
                this.NationalStaffColorLabel.BackColor = themeList[index - 1].color[++i];
                this.NationalTeamColorLabel.BackColor = themeList[index - 1].color[++i];
                this.MainToolbarColorLabel.BackColor = themeList[index - 1].color[++i];
                this.TreeToolbarColorLabel.BackColor = themeList[index - 1].color[++i];
                this.GameDateTextColorLabel.BackColor = themeList[index - 1].color[++i];
                this.TableGridColorLabel.BackColor = themeList[index - 1].color[++i];
                this.PanelsColorLabel.BackColor = themeList[index - 1].color[++i];
                this.ProfileHeadersColorLabel.BackColor = themeList[index - 1].color[++i];
                this.GroupBoxesColorLabel.BackColor = themeList[index - 1].color[++i];
                this.BordersColorsLabel.BackColor = themeList[index - 1].color[++i];
                this.BackgroundFontSelectLabel.Text = this.themeList[index - 1].font.Name + ", " + (int)this.themeList[index - 1].font.Size + ", " + this.themeList[index - 1].font.Style.ToString();
                this.GroupBoxesTransparencyTrackBar.Value = themeList[index - 1].alpha;
                File.Delete(name);
                setTheme();
            }
        }

        private void FileMenuColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref FileMenuColorLabel);
        }
        
        private void FileMenuTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref FileMenuTextColorLabel);
        }

        private void TreeColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref TreeColorLabel);
        }

        private void TreeTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref TreeTextColorLabel);
        }

        private void SettingsPanelColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref SettingsPanelColorLabel);
        }

        private void SettingsPanelTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref SettingsPanelTextColorLabel);
        }

        private void TableColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref TableColorLabel);
        }

        private void TableTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref TableTextColorLabel);
        }

        private void SearchFieldsColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref SearchFieldsColorLabel);
        }

        private void SearchFieldsTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref SearchFieldsTextColorLabel);
        }

        private void GroupBoxesColor1Label_Click(object sender, EventArgs e)
        {
            selectColor(ref ProfileFieldsColor1Label);
        }

        private void GroupBoxesColor2Label_Click(object sender, EventArgs e)
        {
            selectColor(ref ProfileFieldsColor2Label);
        }

        private void ProfileFieldsTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref ProfileFieldsTextColorLabel);
        }

        private void ProfileFieldsOnEditColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref ProfileFieldsOnEditColorLabel);
        }
        
        private void ProfileFieldsOnEditTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref ProfileFieldsOnEditTextColorLabel);
        }

        private void MainColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref MainColorLabel);
        }

        private void MainTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref MainTextColorLabel);
        }

        private void LowAttributeTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref LowAttributeTextColorLabel);
        }

        private void MediumAttributeTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref MediumAttributeTextColorLabel);
        }

        private void GoodAttributeTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref GoodAttributeTextColorLabel);
        }

        private void ExcellentAttributeTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref ExcellentAttributeTextColorLabel);
        }

        private void FreePlayerColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref FreePlayerColorLabel);
        }

        private void LoanPlayerColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref LoanPlayerColorLabel);
        }

        private void CoContractedColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref CoContractedColorLabel);
        }

        private void EUMemberColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref EUMemberColorLabel);
        }

        private void FreeStaffColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref FreeStaffColorLabel);
        }

        private void NationalStaffColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref NationalStaffColorLabel);
        }

        private void NationalTeamColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref NationalTeamColorLabel);
        }

        private void MainToolbarColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref MainToolbarColorLabel);
        }

        private void TreeToolbarColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref TreeToolbarColorLabel);
        }

        private void GameDateTextColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref GameDateTextColorLabel);
        }

        private void TableGridColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref TableGridColorLabel);
        }

        private void PanelsColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref PanelsColorLabel);
        }

        private void ProfileHeadersColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref ProfileHeadersColorLabel);
        }

        private void GroupBoxesColorLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref GroupBoxesColorLabel);
        }

        private void BordersColorsLabel_Click(object sender, EventArgs e)
        {
            selectColor(ref BordersColorsLabel);
        }

        private void GroupBoxesTransparencyTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.ThemesComboBox.SelectedItem.ToString().Equals("Custom"))
                this.ThemesComboBox.SelectedItem = "Custom";

            this.GroupBoxesTransparencyResultLabel.Text = ((TrackBar)sender).Value.ToString();

            setTheme();
        }

        private void GroupBoxesTransparencyTrackBar_KeyUp(object sender, KeyEventArgs e)
        {
            if (!this.ThemesComboBox.SelectedItem.ToString().Equals("Custom"))
                this.ThemesComboBox.SelectedItem = "Custom";

            this.GroupBoxesTransparencyResultLabel.Text = ((TrackBar)sender).Value.ToString();

            setTheme();
        }

        private void GroupBoxesTransparencyTrackBar_ValueChanged(object sender, EventArgs e)
        {
           // if (!this.ThemesComboBox.SelectedItem.ToString().Equals("Custom"))
             //   this.ThemesComboBox.SelectedItem = "Custom";

            this.GroupBoxesTransparencyResultLabel.Text = ((TrackBar)sender).Value.ToString();

            //setTheme();
        }

        private void FontSelectLabel_Click(object sender, EventArgs e)
        {
            if (!this.ThemesComboBox.SelectedItem.ToString().Equals("Custom"))
                this.ThemesComboBox.SelectedItem = "Custom";

            fontDialog.Font = this.mainForm.Font;
            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                BackgroundFontSelectLabel.Text = fontDialog.Font.Name + ", " + (int)fontDialog.Font.Size + ", " + fontDialog.Font.Style.ToString();
                curFont = fontDialog.Font;
                setTheme();
            }
        }

        private void selectColor(ref Label colorLabel)
        {
            if (!this.ThemesComboBox.SelectedItem.ToString().Equals("Custom"))
                this.ThemesComboBox.SelectedItem = "Custom";

            colorDialog.Color = colorLabel.BackColor;
            colorDialog.AnyColor = true;
            colorDialog.FullOpen = true;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorLabel.BackColor = colorDialog.Color;
                setTheme();
            }
        }
    }
}