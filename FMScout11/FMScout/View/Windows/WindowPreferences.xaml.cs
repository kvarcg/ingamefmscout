using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using FMScout.UserControls;
using FMScout.CustomControls;
using FMScout.ControlContext;
using FMScout.ViewModel;

namespace FMScout.View
{
    public partial class WindowPreferences : Window
    {
        public PreferencesWindowViewModel vm;
        public GeneralTabItemViewModel generalTabItemViewModel;
        private Settings settings = null;
        private GlobalFuncs globalFuncs = null;

        public WindowPreferences()
        {
            this.InitializeComponent();

            this.ButtonClose.Click += new RoutedEventHandler(ButtonClose_Click);
            this.MouseDown += new System.Windows.Input.MouseButtonEventHandler(WindowPreferences_MouseDown);

            settings = GlobalSettings.getSettings();
            globalFuncs = Globals.getGlobalFuncs(); 

            setDataContext();

            init();

            if (settings.usingCustomSettings)
                this.ButtonWarning.Opacity = 1;
            else
                this.ButtonWarning.Opacity = 0;

            this.ButtonWarning.Click += new System.Windows.RoutedEventHandler(ButtonWarning_Click);
            this.ButtonSaveSettings.Click += new RoutedEventHandler(ButtonSaveSettings_Click);
            this.ButtonDeleteSettings.Click += new RoutedEventHandler(ButtonDeleteSettings_Click);
            this.ButtonSetDefaultSettings.Click += new RoutedEventHandler(ButtonSetDefaultSettings_Click);
            this.setting.ComboBox.SelectionChanged += new SelectionChangedEventHandler(settingComboBox_SelectionChanged);
            this.ButtonUseChanges.Click += new RoutedEventHandler(ButtonUseChanges_Click);
            this.ButtonCancelChanges.Click += new RoutedEventHandler(ButtonCancelChanges_Click);
            this.GeneralTabItem.currency.ComboBox.SelectionChanged +=
                delegate(object _sender, SelectionChangedEventArgs e) { selectionChanged(); };
            this.GeneralTabItem.wage.ComboBox.SelectionChanged +=
                delegate(object _sender, SelectionChangedEventArgs e) { selectionChanged(); };
            this.GeneralTabItem.height.ComboBox.SelectionChanged +=
                delegate(object _sender, SelectionChangedEventArgs e) { selectionChanged(); };
            this.GeneralTabItem.weight.ComboBox.SelectionChanged +=
                delegate(object _sender, SelectionChangedEventArgs e) { selectionChanged(); };
            this.GeneralTabItem.allowEditing.ComboBox.SelectionChanged +=
                delegate(object _sender, SelectionChangedEventArgs e) { selectionChanged(); };
            this.GeneralTabItem.language.ComboBox.SelectionChanged +=
                delegate(object _sender, SelectionChangedEventArgs e) { selectionChanged(); };
            this.GeneralTabItem.theme.ComboBox.SelectionChanged +=
                delegate(object _sender, SelectionChangedEventArgs e) { selectionChanged(); }; 

            this.GeneralTabItem.wonderkidsMaxAge.NumericUpDown.ValueChanged +=
                delegate(object _sender, RoutedPropertyChangedEventArgs<decimal> e) { selectionChanged(); };
            this.GeneralTabItem.wonderkidsMinPA.NumericUpDown.ValueChanged +=
                delegate(object _sender, RoutedPropertyChangedEventArgs<decimal> e) { selectionChanged(); };
            this.GeneralTabItem.wonderstaffMinPA.NumericUpDown.ValueChanged +=
                delegate(object _sender, RoutedPropertyChangedEventArgs<decimal> e) { selectionChanged(); };
            this.GeneralTabItem.wonderteamsMinRep.NumericUpDown.ValueChanged +=
                delegate(object _sender, RoutedPropertyChangedEventArgs<decimal> e) { selectionChanged(); };

            setCheckBoxEvents(ref this.PlayerColumnsTabItem.WrapPanelColumns);
            setCheckBoxEvents(ref this.StaffColumnsTabItem.WrapPanelColumns);
            setCheckBoxEvents(ref this.TeamColumnsTabItem.WrapPanelColumns);
            setCheckBoxEvents(ref this.ShortlistColumnsTabItem.WrapPanelColumns);
        }

        private void setCheckBoxEvents(ref WrapPanel wrap)
        {
            UIElementCollection collection = wrap.Children;
            for (int i = 0; i < collection.Count; ++i)
            {
                ((CheckBox)collection[i]).Checked +=
                delegate(object _sender, RoutedEventArgs e) { selectionChanged(); };
                ((CheckBox)collection[i]).Unchecked +=
                delegate(object _sender, RoutedEventArgs e) { selectionChanged(); };
            }
        }

        private void selectionChanged()
        {
            if (!settings.usingCustomSettings && settings.settingSettings) addCustomSetting();
        }

        public void setDataContext()
        {
            vm = new PreferencesWindowViewModel();
            generalTabItemViewModel = this.GeneralTabItem.vm;

            ImageButtonContext close = new ImageButtonContext();
            close.ImageSource = TryFindResource("close") as ImageSource;
            ImageTextButtonContext ok = new ImageTextButtonContext();
            ok.ImageSource = TryFindResource("yes") as ImageSource;
            ImageTextButtonContext cancel = new ImageTextButtonContext();
            cancel.ImageSource = TryFindResource("cancel") as ImageSource;
            ImageButtonContext save = new ImageButtonContext();
            save.ImageSource = TryFindResource("save") as ImageSource;
            ImageButtonContext del = new ImageButtonContext();
            del.ImageSource = TryFindResource("delete") as ImageSource;
            ImageButtonContext def = new ImageButtonContext();
            def.ImageSource = TryFindResource("default") as ImageSource;

            LabeledComboBoxContext setting = new LabeledComboBoxContext();
            setting.LabelWidth = 60;
            setting.ComboBoxWidth = 120;

            vm.close = close;
            vm.ok = ok;
            vm.cancel = cancel;
            vm.save = save;
            vm.del = del;
            vm.def = def;
            vm.setting = setting;
            
            vm.setting.ComboBoxItems = settings.settingNames;

            vm.header = new LabeledHeaderContext();
            vm.general = new LabeledHeaderContext();
            vm.player = new LabeledHeaderContext();
            vm.staff = new LabeledHeaderContext();
            vm.team = new LabeledHeaderContext();
            vm.shortlist = new LabeledHeaderContext();
            vm.playercolumnsettings = new LabeledHeaderContext();
            vm.staffcolumnsettings = new LabeledHeaderContext();
            vm.teamcolumnsettings = new LabeledHeaderContext();
            vm.shortlistcolumnsettings = new LabeledHeaderContext();
            vm.selectedallcolumns = new LabeledHeaderContext();
            vm.clearcolumns = new LabeledHeaderContext();
            vm.defcolumns = new LabeledHeaderContext();
            vm.buttonWarningTooltip = new LabeledHeaderContext();
            vm.buttonSaveTooltip = new LabeledHeaderContext();
            vm.buttonDeleteTooltip = new LabeledHeaderContext();
            vm.buttonSetDefaultTooltip = new LabeledHeaderContext();
            vm.buttonUseChangesTooltip = new LabeledHeaderContext();
            vm.buttonCancelChangesTooltip = new LabeledHeaderContext();   

            setControlValues();
            setLocalization();

            this.DataContext = vm;
        }

        public void setControlValues()
        {

        }

        public void setLocalization()
        {
            ScoutLocalization localization = globalFuncs.localization;
            ObservableCollection<String> WindowPreferencesLabels = globalFuncs.localization.WindowPreferencesLabels;
            int index = -1;

            vm.header.Header = WindowPreferencesLabels[++index];
            vm.setting.LabelContent = WindowPreferencesLabels[++index];
            vm.general.Header = WindowPreferencesLabels[++index];
            vm.player.Header = WindowPreferencesLabels[++index];
            vm.staff.Header = WindowPreferencesLabels[++index];
            vm.team.Header = WindowPreferencesLabels[++index];
            vm.shortlist.Header = WindowPreferencesLabels[++index];
            vm.playercolumnsettings.Header = WindowPreferencesLabels[++index];
            vm.staffcolumnsettings.Header = WindowPreferencesLabels[++index];
            vm.teamcolumnsettings.Header = WindowPreferencesLabels[++index];
            vm.shortlistcolumnsettings.Header = WindowPreferencesLabels[++index];
            vm.selectedallcolumns.Header = WindowPreferencesLabels[++index];
            vm.clearcolumns.Header = WindowPreferencesLabels[++index];
            vm.defcolumns.Header = WindowPreferencesLabels[++index];
            vm.ok.TextBlockText = WindowPreferencesLabels[++index];
            vm.cancel.TextBlockText = WindowPreferencesLabels[++index];
            vm.buttonWarningTooltip.Header = WindowPreferencesLabels[++index] + Environment.NewLine + WindowPreferencesLabels[++index];
            vm.buttonSaveTooltip.Header = WindowPreferencesLabels[++index];
            vm.buttonDeleteTooltip.Header = WindowPreferencesLabels[++index];
            vm.buttonSetDefaultTooltip.Header = WindowPreferencesLabels[++index];
            vm.buttonUseChangesTooltip.Header = WindowPreferencesLabels[++index];
            vm.buttonCancelChangesTooltip.Header = WindowPreferencesLabels[++index];
        }

        public void init()
        {
            setCheckBoxes(ref globalFuncs.localization.playerColumns, ref this.PlayerColumnsTabItem.WrapPanelColumns, ref settings.playerColumnsWidth);
            setCheckBoxes(ref globalFuncs.localization.staffColumns, ref this.StaffColumnsTabItem.WrapPanelColumns, ref settings.staffColumnsWidth);
            setCheckBoxes(ref globalFuncs.localization.teamColumns, ref this.TeamColumnsTabItem.WrapPanelColumns, ref settings.teamColumnsWidth);
            setCheckBoxes(ref globalFuncs.localization.shortlistColumns, ref this.ShortlistColumnsTabItem.WrapPanelColumns, ref settings.shortlistColumnsWidth);
            setDefaults();
        }

        public void setCheckBoxes(ref ObservableCollection<String> content, ref WrapPanel wrapPanel, ref List<int> columnWidth)
        {
            for (int i = 0; i < columnWidth.Count; ++i)
            {
                CheckBox item = new CheckBox();
                item.Style = App.Current.Resources["CheckBox"] as Style;
                item.Content = content[i];
                item.Margin = new Thickness(0, 0, 2, 2);
                wrapPanel.Children.Add(item);
            }
        }

        public void setDefaults()
        {
            settings.findCurrentSetting();
            setSettings();
        }

        public void setSettings()
        {
            ScoutLocalization localization = globalFuncs.localization;
            PreferencesSettings curSettings = settings.curPreferencesSettings;

            settings.settingSettings = false;
            vm.setting.ComboBoxSelectedIndex = globalFuncs.getElementIndex(ref settings.settingNames, curSettings.name);
            generalTabItemViewModel.currency.ComboBoxSelectedIndex = globalFuncs.localization.currenciesNative.IndexOf(curSettings.currency);
            generalTabItemViewModel.wage.ComboBoxSelectedIndex = globalFuncs.localization.wagesNative.IndexOf(curSettings.wage);
            generalTabItemViewModel.height.ComboBoxSelectedIndex = globalFuncs.localization.heightsNative.IndexOf(curSettings.height);
            generalTabItemViewModel.weight.ComboBoxSelectedIndex = globalFuncs.localization.weightsNative.IndexOf(curSettings.weight);
            generalTabItemViewModel.allowEditing.ComboBoxSelectedIndex = globalFuncs.localization.editing.IndexOf(curSettings.editing);
            generalTabItemViewModel.language.ComboBoxSelectedIndex = globalFuncs.languages.IndexOf(curSettings.language);
            generalTabItemViewModel.theme.ComboBoxSelectedIndex = globalFuncs.themes.IndexOf(curSettings.theme);
            generalTabItemViewModel.wonderkidsMaxAge.NumericValue = curSettings.wonderkidsMaxAge;
            generalTabItemViewModel.wonderkidsMinPA.NumericValue = curSettings.wonderkidsMinPA;
            generalTabItemViewModel.wonderstaffMinPA.NumericValue = curSettings.wonderstaffMinPA;
            generalTabItemViewModel.wonderteamsMinRep.NumericValue = curSettings.wonderteamsMinRep;
            //this.GeneralTabItem.wonderkidsMaxAge.NumericUpDown.Value = curSettings.wonderkidsMaxAge;
            //this.GeneralTabItem.wonderkidsMinPA.NumericUpDown.Value = curSettings.wonderkidsMinPA;
            //this.GeneralTabItem.wonderstaffMinPA.NumericUpDown.Value = curSettings.wonderstaffMinPA;
            //this.GeneralTabItem.wonderteamsMinRep.NumericUpDown.Value = curSettings.wonderteamsMinRep;

            this.PlayerColumnsTabItem.ButtonColumnsDefault_Click(null, null);
            this.StaffColumnsTabItem.ButtonColumnsDefault_Click(null, null);
            this.TeamColumnsTabItem.ButtonColumnsDefault_Click(null, null);
            this.ShortlistColumnsTabItem.ButtonColumnsDefault_Click(null, null);
            settings.settingSettings = true;
        }

        private void setCurrentSettings()
        {
            String selectedItem = settings.settingNames[this.vm.setting.ComboBoxSelectedIndex];
            for (int i = 0; i < settings.settingNames.Count; ++i)
            {
                if (settings.settingNames[i].Equals(selectedItem))
                    settings.preferencesSettings[i].isCurrent = true;
                else
                    settings.preferencesSettings[i].isCurrent = false;
            }
        }

        private void addCustomSetting()
        {
            globalFuncs.FadeInElement(this.ButtonWarning, globalFuncs.elementDuration, globalFuncs.elementFinalOpacity, true);
            settings.addCustomSetting();
            settings.settingSettings = false;
            PreferencesSettings preferencesSettings = settings.preferencesSettings[0]; 
            vm.setting.ComboBoxSelectedIndex = globalFuncs.getElementIndex(ref settings.settingNames, preferencesSettings.name);
            settings.settingSettings = true;
        }

        public void removeCustomSetting()
        {
            globalFuncs.FadeOutElement(this.ButtonWarning, globalFuncs.elementDuration, globalFuncs.elementFinalOpacity);
            for (int i = 0; i < settings.preferencesSettings.Count; ++i)
            {
                if (settings.preferencesSettings[i].name.Equals("Custom"))
                {
                    settings.usingCustomSettings = false;
                    settings.preferencesSettings.RemoveAt(i);
                    settings.settingNames.RemoveAt(i);
                    String selection = settings.preferencesSettings[i].name;
                    settings.settingSettings = false;
                    vm.setting.ComboBoxSelectedIndex = globalFuncs.getElementIndex(ref settings.settingNames, selection);
                    settings.settingSettings = true;
                    break;
                }
            }
        }

        private void ButtonUseChanges_Click(object sender, RoutedEventArgs e)
        {
            String selection = (String)this.setting.ComboBox.SelectedItem;

            for (int i = 0; i < settings.settingNames.Count; ++i)
            {
                if (!settings.preferencesSettings[i].name.Equals(selection))
                    settings.preferencesSettings[i].isCurrent = false;
                else
                {
                    settings.firstSettingName = settings.preferencesSettings[i].name;
                    settings.preferencesSettings[i].isCurrent = true;
                    if (settings.preferencesSettings[i].name.Equals("Custom"))
                        settings.preferencesSettings[i].set(settings.createCustomSettings(this));
                }
            }

            if (settings.usingCustomSettings && !settings.firstSettingName.Equals("Custom"))
                removeCustomSetting();

            ((WindowMain)this.Owner).PlayerSearch.setColumns(ref this.PlayerColumnsTabItem.WrapPanelColumns);
            ((WindowMain)this.Owner).StaffSearch.setColumns(ref this.StaffColumnsTabItem.WrapPanelColumns);
            ((WindowMain)this.Owner).TeamSearch.setColumns(ref this.TeamColumnsTabItem.WrapPanelColumns);
            ((WindowMain)this.Owner).Shortlist.setColumns(ref this.ShortlistColumnsTabItem.WrapPanelColumns);

            settings.findCurrentSetting();
            settings.setTheme();
            settings.setLanguage();
            globalFuncs.closeWindow(this);
        }

        private void ButtonCancelChanges_Click(object sender, RoutedEventArgs e)
        {
            if (settings.usingCustomSettings && !settings.firstSettingName.Equals("Custom"))
                removeCustomSetting();

            globalFuncs.closeWindow(this);
        }

        private void ButtonSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            settings.saveSettings(this);
            this.setting.ComboBox.Items.Refresh();
        }

        private void ButtonDeleteSettings_Click(object sender, RoutedEventArgs e)
        {
            String selectedItem = settings.settingNames[this.vm.setting.ComboBoxSelectedIndex];
            int index = -1;
            for (int i = 0; i < settings.settingNames.Count; ++i)
            {
                if (settings.settingNames[i].Equals(selectedItem))
                {
                    index = i;
                    break;
                }
            }


            if (!selectedItem.Equals("Custom"))
            {
                int selectedIndex = -1;
                settings.settingNames.RemoveAt(index);
                settings.preferencesSettings.RemoveAt(index);
                if (index < settings.settingNames.Count) selectedIndex = index;
                else selectedIndex = settings.settingNames.Count - 1;
                this.vm.setting.ComboBoxSelectedIndex = selectedIndex;
                string file = globalFuncs.applicationDirectory + "\\Settings\\" + selectedItem + ".set"; 
                if (File.Exists(file))
                    File.Delete(file);
            }
            else
                removeCustomSetting();

            this.setting.ComboBox.Items.Refresh();
        }

        private void ButtonSetDefaultSettings_Click(object sender, RoutedEventArgs e)
        {
            String selectedItem = settings.settingNames[this.vm.setting.ComboBoxSelectedIndex];
            List<int> changed = new List<int>();
            for (int i = 0; i < settings.settingNames.Count; ++i)
            {
                PreferencesSettings set = settings.preferencesSettings[i];
                if (!selectedItem.Equals(set.name))
                {
                    if (set.isDefault) changed.Add(i);
                    set.isDefault = false;
                }
                else
                {
                    if (!set.isDefault) changed.Add(i);
                    set.isDefault = true;
                }
            }

            setDefaultButtons(ref selectedItem);
            this.ButtonSetDefaultSettings.IsEnabled = false;

            for (int i = 0; i < changed.Count; ++i)
                settings.setDefault(changed[i]);
        }

        private void settingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1) return;
            String settingName = (String)e.AddedItems[0];
            int index = -1;
            for (int i = 0; i < settings.settingNames.Count; ++i)
            {
                if (settings.settingNames[i].Equals(settingName))
                {
                    index = i;
                    break;
                }
            }

            settings.curPreferencesSettings = (PreferencesSettings)settings.preferencesSettings[index];
            settings.curPreferencesSettingsIndex = index;

            setDefaultButtons(ref settingName);

            if (settings.curPreferencesSettings.isDefault)
                this.ButtonSetDefaultSettings.IsEnabled = false;

            if (settings.settingSettings) setSettings();
        }

        private void setDefaultButtons(ref String name)
        {
            this.ButtonSetDefaultSettings.IsEnabled = true;
            this.ButtonSaveSettings.IsEnabled = true;
            this.ButtonDeleteSettings.IsEnabled = true;

            if (name.Equals("Initial"))
            {
                this.ButtonSaveSettings.IsEnabled = false;
                this.ButtonDeleteSettings.IsEnabled = false;
                return;
            }

            if (name.Equals("Custom"))
            {
                this.ButtonSetDefaultSettings.IsEnabled = false;
                this.ButtonDeleteSettings.IsEnabled = false;
                return;
            }

            this.ButtonSaveSettings.IsEnabled = false;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            globalFuncs.closeWindow(this);
        }

        private void WindowPreferences_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonWarning_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            removeCustomSetting();
            setSettings();
        }
    }
}