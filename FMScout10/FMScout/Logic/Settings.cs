using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using FMScout.UserControls;
using FMScout.ControlContext;
using FMScout.View;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace FMScout
{
    public static class GlobalSettings
    {
        static Settings set = null;
        
        public static Settings getSettings()
        {
            if (set == null)
            {
                set = new Settings();
            }
            return set;
        }

        public static void setSettings(Settings newSet)
        {
            set = newSet;
        }
    }

	public class Settings
	{
        public String currentTheme = "Gray Power";
        public String currentThemePath = "DarkTheme";
        public String currentLanguage = "English";

        private GlobalFuncs globalFuncs = null;
        public List<Language> languages; 
        public List<Multiplier> currencies;
        public List<MultiplierExtended> wages;
        public List<MultiplierExtended> heights;
        public List<MultiplierExtended> weights;
        public List<int> playerColumnsWidth;
        public List<int> staffColumnsWidth;
        public List<int> teamColumnsWidth;
        public List<int> shortlistColumnsWidth;

        // settings to copy
        public ObservableCollection<String> settingNames = new ObservableCollection<String>();
        public bool usingCustomSettings = false;
        public String firstSettingName = "";
        public bool settingSettings = false;
        public bool changingLanguage = false;
        public List<PreferencesSettings> preferencesSettings;
        public PreferencesSettings curPreferencesSettings = null;
        public int curPreferencesSettingsIndex = -1; 
        public bool initScout = false;

		public Settings()
		{
            globalFuncs = Globals.getGlobalFuncs();
		}
		
		public void init()
        {
            initScout = true;
            playerColumnsWidth = new List<int>();
            staffColumnsWidth = new List<int>();
            teamColumnsWidth = new List<int>();
            shortlistColumnsWidth = new List<int>();

            languages = new List<Language>();
            currencies = new List<Multiplier>();
            wages = new List<MultiplierExtended>();
            heights = new List<MultiplierExtended>();
            weights = new List<MultiplierExtended>();
            currencies.Add(new Multiplier(1.0f, CultureInfo.CreateSpecificCulture("en-GB")));
            currencies.Add(new Multiplier(1.15f, CultureInfo.CreateSpecificCulture("el-GR")));
            currencies.Add(new Multiplier(1.65f, CultureInfo.CreateSpecificCulture("en-US")));
            wages.Add(new MultiplierExtended(1.0f, "Weekly", null));
            wages.Add(new MultiplierExtended(4.0f, "Monthly", null));
            wages.Add(new MultiplierExtended(52.0f, "Yearly", null));
            heights.Add(new MultiplierExtended(1.0f, "cm", null));
            heights.Add(new MultiplierExtended(0.01f, "m", null));
            weights.Add(new MultiplierExtended(1.0f, "kg", null));
            weights.Add(new MultiplierExtended(2.2f, "lbs", null));

            preferencesSettings = new List<PreferencesSettings>();
            preferencesSettings.Add(new PreferencesSettings("Initial"));

            // add Columns Default Width
            int[] playerWidths = { 30, 80, 140, 120, 140, 80, 80, 40, 40, 40, 40, 100, 60, 100, 100, 100, 100, 100, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60
                           };
            playerColumnsWidth.AddRange(playerWidths);

            int[] staffWidths = { 80, 140, 120, 140, 80, 40, 40, 40, 40, 100, 100, 100, 100, 100, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60,
                               60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60,
                               60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60
                           };
            staffColumnsWidth.AddRange(staffWidths);

            int[] teamWidths = { 80, 140, 120, 100, 100, 120, 100, 100, 100, 100, 100, 60, 60, 60, 100, 80, 80, 80, 80, 80, 80,
                                80, 80, 80, 80, 80};
            teamColumnsWidth.AddRange(teamWidths);

            int[] shortlistWidths = { 80, 140, 120, 140, 80, 80, 40, 40, 40, 40, 100, 60, 100, 100, 100, 100, 100, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 
                                     60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60
                           };

            shortlistColumnsWidth.AddRange(shortlistWidths);

            setInitialSettings();
            readSettings();
            readLanguages();

            findCurrentSetting();

            setTheme();
            setBrushes();
            setLanguage();
            initScout = false;
        }

        public bool settingTheme = false;
        public void setTheme()
        {
            if (curPreferencesSettings.theme.Equals(currentTheme)) return;
            settingTheme = true;
            String themePath = "";
            String theme = "";
            int themeIndex = -1;
            Collection<ResourceDictionary> mergedDictionaries = App.Current.Resources.MergedDictionaries;
            for (int i = 0; i < globalFuncs.themes.Count; ++i)
            {
                theme = globalFuncs.themes[i];
                themePath = globalFuncs.themesPath[i];
                if (theme.Equals(curPreferencesSettings.theme)) themeIndex = i;
                for (int j = 0; j < mergedDictionaries.Count; ++j)
                {
                    String source = mergedDictionaries[j].Source.ToString();
                    if (source.Contains(themePath))
                    {
                        mergedDictionaries.Remove(mergedDictionaries[j]);
                        break;
                    }
                }
            }

            ResourceDictionary d = new ResourceDictionary();
            String uriSource = "/" + this.GetType().Assembly.GetName().Name +
                ";component/Themes/" + globalFuncs.themesPath[themeIndex] + "/" + globalFuncs.themesPath[themeIndex] + ".xaml";
            d.Source = new System.Uri(uriSource, UriKind.Relative);
            mergedDictionaries.Add(d);   
         
            // set custom resources
            setBrushes();

            currentTheme = curPreferencesSettings.theme;
            currentThemePath = globalFuncs.themesPath[globalFuncs.themes.IndexOf(currentTheme)];

            settingTheme = false;
        }

        public void setBrushes()
        {
            globalFuncs.attributeBrushes.Clear();
            globalFuncs.defaultProfileTextBoxForeground = App.Current.Resources["DefaultTextBoxForeground"] as SolidColorBrush;
            globalFuncs.selectedAutoCompleteBrush = App.Current.Resources["SelectedAutoCompleteBrush"] as SolidColorBrush;
            globalFuncs.attributeBrushes.Add(App.Current.Resources["LowAttributeForeground"] as SolidColorBrush);
            globalFuncs.attributeBrushes.Add(App.Current.Resources["MediumAttributeForeground"] as SolidColorBrush);
            globalFuncs.attributeBrushes.Add(App.Current.Resources["HighAttributeForeground"] as SolidColorBrush);
            globalFuncs.attributeBrushes.Add(App.Current.Resources["VeryHighAttributeForeground"] as SolidColorBrush);
        }

        public void setLanguage()
        {
            if (!initScout)
            {
                if (curPreferencesSettings.language.Equals(currentLanguage)) return;
            }

            ScoutLocalization sl = globalFuncs.localization;
            FieldInfo[] slfieldInfos = typeof(ScoutLocalization).GetFields();
            Language l = null;
            for (int i = 0; i < languages.Count; ++i)
            {
                if (curPreferencesSettings.language.Equals(languages[i].name))
                {
                    l = languages[i];
                    break;
                }
            }
            if (l != null)
            {
                FieldInfo[] lfieldInfos = typeof(Language).GetFields();
                List<String> toAddEmpty = new List<String>()
                {
                    "YesNoEmpty",
                    "contractStatuses",
                    "regions",
                    "bestcrs",
                    "bestprs",
                    "ownerShips",
                    "roles",
                    "bestcrs",
                    "teamtypes",
                    "reputations"
                };

                changingLanguage = true;
                //int j = 0;
                for (int i = 0; i < lfieldInfos.Length; ++i)
                {
                    FieldInfo lfieldInfo = lfieldInfos[i];
                    if (lfieldInfo != null)
                    {
                        if (lfieldInfo.FieldType.Name.Contains("List"))
                        {
                            //++j;
                            //if (slfieldInfo.Name.Equals("YesNoEmpty")) --j;
                            FieldInfo slfieldInfo = null;
                            for (int j = 0; j < slfieldInfos.Length; ++j)
                            {
                                if (slfieldInfos[j].Name.ToLower().Equals(lfieldInfo.Name.ToLower()))
                                {
                                    slfieldInfo = slfieldInfos[j];
                                    break;
                                }
                            }
                            if (slfieldInfo != null)
                            {
                                ObservableCollection<String> sllist = (ObservableCollection<String>)slfieldInfo.GetValue(sl);
                                List<String> llist = (List<String>)lfieldInfo.GetValue(l);
                                //sllist.Clear();
                                int index = -1;

                                if (toAddEmpty.Contains(slfieldInfo.Name))
                                    ++index;

                                for (int k = 0; k < llist.Count; ++k)
                                    sllist[++index] = llist[k];
                                
                                if (lfieldInfo.Name.ToLower().Equals("yesno"))
                                {
                                    FieldInfo slfieldInfo2 = typeof(ScoutLocalization).GetField("YesNoEmpty");
                                    ObservableCollection<String> sllist2 = (ObservableCollection<String>)slfieldInfo2.GetValue(sl);
                                    for (int k = 0; k < llist.Count; ++k)
                                        sllist2[k + 1] = llist[k];
                                }
                            }
                            else
                                globalFuncs.logging.update("Error: Language " + curPreferencesSettings.language + " Reason: Could not apply " + slfieldInfo.Name);
                        }
                    }
                    else
                        globalFuncs.logging.update("Error: Language " + curPreferencesSettings.language + " Reason: Could not apply " + lfieldInfo.Name);
                }
                globalFuncs.windowMain.changeLanguage();
                changingLanguage = false;
                currentLanguage = curPreferencesSettings.language;
            }
            else
            {
                globalFuncs.logging.update("Error: Language " + curPreferencesSettings.language + " Reason: Could not be applied");
            }
        }

        public void addCustomSetting()
        {
            this.usingCustomSettings = true;
            PreferencesSettings oldPreferencesSettings = this.curPreferencesSettings;
            PreferencesSettings preferencesSettings = new PreferencesSettings("Custom");
            this.settingNames.Insert(0, preferencesSettings.name);
            this.preferencesSettings.Insert(0, preferencesSettings);
            preferencesSettings.set(oldPreferencesSettings);
        }

        public void findCurrentSetting()
        {
            int currentIndex = -1;
            for (int i = 0; i < preferencesSettings.Count; ++i)
            {
                PreferencesSettings p = (PreferencesSettings)preferencesSettings[i];
                if (p.isCurrent)
                {
                    currentIndex = i;
                    break;
                }
            }

            if (currentIndex == -1)
            {
                for (int i = 0; i < preferencesSettings.Count; ++i)
                {
                    PreferencesSettings p = (PreferencesSettings)preferencesSettings[i];
                    if (p.isDefault)
                    {
                        currentIndex = i;
                        break;
                    }
                }
            }

            if (currentIndex == -1)
            {
                currentIndex = 0;
                preferencesSettings[currentIndex].isDefault = true;
            }

            curPreferencesSettings = preferencesSettings[currentIndex];
            curPreferencesSettingsIndex = currentIndex;
        }

        public void setCurrencyMultiplier(ref PreferencesSettings settings)
        {
            for (int i = 0; i < globalFuncs.localization.currenciesNative.Count; ++i)
            {
                if (settings.currency.Equals(globalFuncs.localization.currenciesNative[i]))
                {
                    settings.currencyMultiplier.multiplier = currencies[i].multiplier;
                    settings.currencyMultiplier.format = currencies[i].format;
                    break;
                }
            }
        }

        public void setWageMultiplier(ref PreferencesSettings settings)
        {
            for (int i = 0; i < globalFuncs.localization.wagesNative.Count; ++i)
            {
                if (settings.wage.Equals(globalFuncs.localization.wagesNative[i]))
                {
                    settings.wageMultiplier.multiplier = wages[i].multiplier;
                    settings.wageMultiplier.extended = wages[i].extended;
                    break;
                }
            }
        }

        public void setHeightMultiplier(ref PreferencesSettings settings)
        {
            for (int i = 0; i < globalFuncs.localization.heightsNative.Count; ++i)
            {
                if (settings.height.Equals(globalFuncs.localization.heightsNative[i]))
                {
                    settings.heightMultiplier.multiplier = heights[i].multiplier;
                    settings.heightMultiplier.extended = heights[i].extended;
                    break;
                }
            }
        }

        public void setWeightMultiplier(ref PreferencesSettings settings)
        {
            for (int i = 0; i < globalFuncs.localization.weightsNative.Count; ++i)
            {
                if (settings.weight.Equals(globalFuncs.localization.weightsNative[i]))
                {
                    settings.weightMultiplier.multiplier = weights[i].multiplier;
                    settings.weightMultiplier.extended = weights[i].extended;
                }
            }
        }
        
        public void setMultipliers(ref PreferencesSettings settings)
        {
            setCurrencyMultiplier(ref settings);
            setWageMultiplier(ref settings);
            setHeightMultiplier(ref settings);
            setWeightMultiplier(ref settings);
        }

        public void setInitialSettings()
        {
            PreferencesSettings initialSettings = (PreferencesSettings)preferencesSettings[0];
            currentTheme = "Gray Power"; 
            currentLanguage = "English";

            initialSettings.currency = "Euro"; 
            initialSettings.wage = "Monthly";
            initialSettings.height = "Centimeters";
            initialSettings.weight = "Kilos";
            initialSettings.editing = "No";
            initialSettings.language = "English";
            initialSettings.theme = "Gray Power";
            setMultipliers(ref initialSettings);

            int[] playersDefCols = {0, 2, 3, 4, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            initialSettings.playerColumns.AddRange(playersDefCols);
            int[] staffDefCols = { 1, 2, 3, 4, 5, 6, 7, 9, 10, 12, 13, 14 };
            initialSettings.staffColumns.AddRange(staffDefCols);
            int[] teamsDefCols = { 1, 2, 3, 4, 5, 6, 9, 10 };
            initialSettings.teamColumns.AddRange(teamsDefCols);
            int[] shortlistDefCols = { 1, 2, 3, 5, 6, 9, 10, 11, 12, 13, 14, 15 };
            initialSettings.shortlistColumns.AddRange(shortlistDefCols);

            initialSettings.wonderkidsMaxAge = 20;
            initialSettings.wonderkidsMinPA = 170;
            initialSettings.wonderstaffMinPA = 170;
            initialSettings.wonderteamsMinRep = 8000;
        }

        public void readSettings()
        {
            string path = globalFuncs.applicationDirectory + "\\Settings";
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
                Directory.CreateDirectory(path);
            else
            {
                FileInfo[] rgFiles = di.GetFiles("*.set");
                int counter = 0;
                foreach (FileInfo fi in rgFiles)
                {
                    string name = fi.Name.Substring(0, fi.Name.Length - 4);
                    preferencesSettings.Add(new PreferencesSettings(name));
                    int index = this.preferencesSettings.Count - 1;
                    PreferencesSettings fileSettings = (PreferencesSettings)preferencesSettings[index];
                    using (FileStream stream = new FileStream(path + "\\" + fi.Name, FileMode.Open))
                    {
                        using (StreamReader sw = new StreamReader(stream))
                        {
                            string readLine;
                            char[] sep = { ' ' };

                            ++counter;
                            while (!sw.EndOfStream)
                            {
                                readLine = sw.ReadLine();

                                string[] ar = readLine.Split(sep);
                                if (ar.Length == 2 || ar.Length == 3)
                                {
                                    if (ar[0].Equals("Default"))
                                    {
                                        bool isDefault = false;
                                        if (ar[1].Equals("Yes")) isDefault = true;
                                        fileSettings.isDefault = isDefault;
                                    }
                                    else if (ar[0].Equals("Language"))
                                    {
                                        string concat = ar[1];
                                        if (ar.Length == 3)
                                            concat += " " + ar[2];
                                        fileSettings.language = concat;
                                    }
                                    else if (ar[0].Equals("Theme"))
                                    {
                                        string concat = ar[1];
                                        if (ar.Length == 3)
                                            concat += " " + ar[2];
                                        fileSettings.theme = concat;
                                    }
                                    else if (ar[0].Equals("Currency"))
                                    {
                                        string concat = ar[1];
                                        if (ar.Length == 3)
                                            concat += " " + ar[2];
                                        fileSettings.currency = concat;
                                    }
                                    else if (ar[0].Equals("Wage"))
                                    {
                                        fileSettings.wage = ar[1];
                                    }
                                    else if (ar[0].Equals("Height"))
                                    {
                                        fileSettings.height = ar[1];
                                    }
                                    else if (ar[0].Equals("Weight"))
                                    {
                                        fileSettings.weight = ar[1];
                                    }
                                    else if (ar[0].Equals("Editing"))
                                    {
                                        fileSettings.editing = ar[1];
                                    }
                                    else if (ar[0].Equals("WonderkidsMaxAge"))
                                    {
                                        fileSettings.wonderkidsMaxAge = Int32.Parse(ar[1]);
                                    }
                                    else if (ar[0].Equals("WonderkidsMinPA"))
                                    {
                                        fileSettings.wonderkidsMinPA = Int32.Parse(ar[1]);
                                    }
                                    else if (ar[0].Equals("WonderstaffMinPA"))
                                    {
                                        fileSettings.wonderstaffMinPA = Int32.Parse(ar[1]);
                                    }
                                    else if (ar[0].Equals("WonderteamsMinRep"))
                                    {
                                        fileSettings.wonderteamsMinRep = Int32.Parse(ar[1]);
                                    }
                                }
                                else if (ar.Length > 0)
                                {
                                    if (ar[0].Equals("PlayerColumns"))
                                    {
                                        // restore player columns
                                        for (int i = 1; i < ar.Length - 1; ++i)
                                            fileSettings.playerColumns.Add(Int32.Parse(ar[i]));
                                    }
                                    else if (ar[0].Equals("StaffColumns"))
                                    {
                                        // restore staff columns
                                        for (int i = 1; i < ar.Length - 1; ++i)
                                            fileSettings.staffColumns.Add(Int32.Parse(ar[i]));
                                    }
                                    if (ar[0].Equals("TeamColumns"))
                                    {
                                        // restore teams columns
                                        for (int i = 1; i < ar.Length - 1; ++i)
                                            fileSettings.teamColumns.Add(Int32.Parse(ar[i]));
                                    }
                                    if (ar[0].Equals("ShortlistColumns"))
                                    {
                                        // restore shortlist columns
                                        for (int i = 1; i < ar.Length - 1; ++i)
                                            fileSettings.shortlistColumns.Add(Int32.Parse(ar[i]));
                                    }
                                }
                                else
                                    return;
                            }
                            sw.Close();
                        }
                        stream.Close();
                    }
                    setMultipliers(ref fileSettings);
                }
            }
        }

        public void readLanguages()
        {
            string path = globalFuncs.applicationDirectory + "\\Languages\\";
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
                Directory.CreateDirectory(path);
            else
            {
                FileInfo[] rgFiles = di.GetFiles("*.lng");
                foreach (FileInfo fi in rgFiles)
                {
                    string name = fi.Name.Substring(0, fi.Name.Length - 4);
                    globalFuncs.languages.Add(name);
                    Language l = new Language();
                    l.name = name;
                    using (FileStream stream = new FileStream(path + fi.Name, FileMode.Open))
                    {
                        using (StreamReader sw = new StreamReader(stream, true))
                        {
                            String line;
                            while (!sw.EndOfStream)
                            {
                                line = sw.ReadLine();
                                // is header
                                if (line.Length > 0)
                                {
                                    if (line.StartsWith("#"))
                                    {
                                        String header = line.Substring(1);
                                        header = header.Trim();
                                        FieldInfo fieldInfo = typeof(Language).GetField(header);
                                        if (fieldInfo != null)
                                        {
                                            List<String> list = (List<String>)fieldInfo.GetValue(l);
                                            if (sw.EndOfStream) break;
                                            line = sw.ReadLine();
                                            while (!line.StartsWith("#") && line.Length > 0)
                                            {
                                                list.Add(line);
                                                if (sw.EndOfStream) break;
                                                line = sw.ReadLine();
                                            }
                                        }
                                        else
                                            globalFuncs.logging.update("Error: Language " + name + " Reason: Could not read " + header);

                                    }
                                }
                            }
                            sw.Close();
                        }
                        stream.Close();
                    }
                    languages.Add(l);
                }
            }
        }

        public void setDefault(int i)
        {
            PreferencesSettings set = preferencesSettings[i];
            if (set.name.Equals("Initial")) return;
            string file = globalFuncs.applicationDirectory + "\\Settings\\" + set.name + ".set";
            string contents = "";
            string defaultLine = "";
            using (FileStream stream = new FileStream(file, FileMode.Open))
            {
                string line = "";
                using (StreamReader sw = new StreamReader(stream))
                {
                    while (!sw.EndOfStream)
                    {
                        line = sw.ReadLine();
                        if (line.Contains("Default")) defaultLine = line;
                        contents += line;
                        contents += System.Environment.NewLine; ;
                    }
                    sw.Close();
                }
                stream.Close();
            }

            using (FileStream stream = new FileStream(file, FileMode.Open))
            {    
                stream.SetLength(0);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    string strValue = string.Empty;
                    strValue += "Default ";
                    if (set.isDefault) strValue += "Yes";
                    else strValue += "No";
                   
                    sw.Write(contents.Replace(defaultLine, strValue));
                    sw.Close();
                }
                stream.Close();
            }
        }

        public void saveSettings(View.WindowPreferences windowPreferences)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = globalFuncs.applicationDirectory + "\\Settings";
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
                saveSettingsFile(name, ref windowPreferences);
            }
        }

        public PreferencesSettings createCustomSettings(WindowPreferences windowPreferences)
        {
            PreferencesSettings customSettings = new PreferencesSettings("Custom");
            customSettings.currency = globalFuncs.localization.currenciesNative[windowPreferences.GeneralTabItem.currency.ComboBox.SelectedIndex];
            customSettings.wage = globalFuncs.localization.wagesNative[windowPreferences.GeneralTabItem.wage.ComboBox.SelectedIndex];
            customSettings.height = globalFuncs.localization.heightsNative[windowPreferences.GeneralTabItem.height.ComboBox.SelectedIndex];
            customSettings.weight = globalFuncs.localization.weightsNative[windowPreferences.GeneralTabItem.weight.ComboBox.SelectedIndex];
            customSettings.editing = globalFuncs.localization.editing[windowPreferences.GeneralTabItem.allowEditing.ComboBox.SelectedIndex];
            customSettings.language = (String)windowPreferences.GeneralTabItem.language.ComboBox.SelectedItem;
            customSettings.theme = (String)windowPreferences.GeneralTabItem.theme.ComboBox.SelectedItem;
            customSettings.wonderkidsMaxAge = (int)windowPreferences.GeneralTabItem.wonderkidsMaxAge.NumericUpDown.Value;
            customSettings.wonderkidsMinPA = (int)windowPreferences.GeneralTabItem.wonderkidsMinPA.NumericUpDown.Value;
            customSettings.wonderstaffMinPA = (int)windowPreferences.GeneralTabItem.wonderstaffMinPA.NumericUpDown.Value;
            customSettings.wonderteamsMinRep = (int)windowPreferences.GeneralTabItem.wonderteamsMinRep.NumericUpDown.Value;

            for (int i = 0; i < windowPreferences.PlayerColumnsTabItem.WrapPanelColumns.Children.Count; ++i)
            {
                CheckBox item = (CheckBox)windowPreferences.PlayerColumnsTabItem.WrapPanelColumns.Children[i];
                if (item.IsChecked == true) customSettings.playerColumns.Add(i);
            }
            for (int i = 0; i < windowPreferences.StaffColumnsTabItem.WrapPanelColumns.Children.Count; ++i)
            {
                CheckBox item = (CheckBox)windowPreferences.StaffColumnsTabItem.WrapPanelColumns.Children[i];
                if (item.IsChecked == true) customSettings.staffColumns.Add(i);
            }
            for (int i = 0; i < windowPreferences.TeamColumnsTabItem.WrapPanelColumns.Children.Count; ++i)
            {
                CheckBox item = (CheckBox)windowPreferences.TeamColumnsTabItem.WrapPanelColumns.Children[i];
                if (item.IsChecked == true) customSettings.teamColumns.Add(i);
            }
            for (int i = 0; i < windowPreferences.ShortlistColumnsTabItem.WrapPanelColumns.Children.Count; ++i)
            {
                CheckBox item = (CheckBox)windowPreferences.ShortlistColumnsTabItem.WrapPanelColumns.Children[i];
                if (item.IsChecked == true) customSettings.shortlistColumns.Add(i);
            }

            setMultipliers(ref customSettings);

            return customSettings;
        }

        private void saveSettingsFile(string name, ref WindowPreferences windowPreferences)
        {
            PreferencesSettings savedSettings = createCustomSettings(windowPreferences);

            // Saves the file via a FileStream created by the OpenFile method.
            string file = globalFuncs.applicationDirectory + "\\Settings\\" + name + ".set";
            using (FileStream stream = new FileStream(file, FileMode.OpenOrCreate))
            {
                stream.SetLength(0);
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    string strValue = string.Empty;

                    strValue += "Default No";
                    strValue += System.Environment.NewLine;
                    strValue += "Theme " + savedSettings.theme;
                    strValue += System.Environment.NewLine; 
                    strValue += "Language " + savedSettings.language;
                    strValue += System.Environment.NewLine;
                    strValue += "Currency " + savedSettings.currency;
                    strValue += System.Environment.NewLine;
                    strValue += "Wage " + savedSettings.wage;
                    strValue += System.Environment.NewLine;
                    strValue += "Height " + savedSettings.height;
                    strValue += System.Environment.NewLine;
                    strValue += "Weight " + savedSettings.weight;
                    strValue += System.Environment.NewLine;
                    strValue += "Editing " + savedSettings.editing;
                    strValue += System.Environment.NewLine;
                    strValue += "WonderkidsMaxAge " + savedSettings.wonderkidsMaxAge;
                    strValue += System.Environment.NewLine;
                    strValue += "WonderkidsMinPA " + savedSettings.wonderkidsMinPA;
                    strValue += System.Environment.NewLine;
                    strValue += "WonderstaffMinPA " + savedSettings.wonderstaffMinPA;
                    strValue += System.Environment.NewLine;
                    strValue += "WonderteamsMinRep " + savedSettings.wonderteamsMinRep;
                    strValue += System.Environment.NewLine;

                    // save player columns
                    strValue += "PlayerColumns ";
                    for (int i = 0; i < savedSettings.playerColumns.Count; ++i)
                        strValue += savedSettings.playerColumns[i] + " ";
                    strValue += System.Environment.NewLine;

                    // save staff columns
                    strValue += "StaffColumns ";
                    for (int i = 0; i < savedSettings.staffColumns.Count; ++i)
                        strValue += savedSettings.staffColumns[i] +  " ";
                    strValue += System.Environment.NewLine;

                    // save teams columns
                    strValue += "TeamColumns ";
                    for (int i = 0; i < savedSettings.teamColumns.Count; ++i)
                        strValue += savedSettings.teamColumns[i] + " ";
                    strValue += System.Environment.NewLine;

                    // save shortlist columns
                    strValue += "ShortlistColumns ";
                    for (int i = 0; i < savedSettings.shortlistColumns.Count; ++i)
                        strValue += savedSettings.shortlistColumns[i] + " ";

                    sw.Write(strValue);
                    sw.Close();
                }
                stream.Close();
            }

            bool contains = false;
            int index = -1;
            for (int i = 0; i < this.settingNames.Count; ++i)
            {
                if (this.settingNames[i] == name)
                {
                    contains = true;
                    index = i;
                    break;
                }
            }

            if (contains)
            {
                this.preferencesSettings[index].set(savedSettings);
                windowPreferences.removeCustomSetting();
                windowPreferences.vm.setting.ComboBoxSelectedIndex = index - 1;
            }
            else
            {
                PreferencesSettings set = new PreferencesSettings(name);
                set.set(savedSettings);
                this.preferencesSettings.Add(set);
                //this.preferencesSettings[this.settingNames.Count - 1].set(curPreferencesSettings);
                windowPreferences.removeCustomSetting();
                windowPreferences.vm.setting.ComboBoxSelectedIndex = this.settingNames.Count - 1;
            }
        }
	}
}