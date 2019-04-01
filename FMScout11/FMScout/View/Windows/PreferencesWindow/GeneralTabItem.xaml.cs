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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FMScout.ControlContext;
using FMScout.ViewModel;
using System.Collections.ObjectModel;

namespace FMScout.View.PreferencesWindow
{
	public partial class GeneralTabItem : UserControl
	{
        public GeneralTabItemViewModel vm = null;
        private GlobalFuncs globalFuncs = null;

        public GeneralTabItem()
		{
			this.InitializeComponent();

            this.allowEditing.Visibility = Visibility.Collapsed;
            //this.theme.Visibility = Visibility.Collapsed;

            globalFuncs = Globals.getGlobalFuncs(); 
            
            setDataContext();
        }

        public void setSettingsComboContext(ref LabeledComboBoxContext box)
        {
            box.LabelWidth = 110;
            box.ComboBoxWidth = 120;
        }

        public void setNumericsContext(ref LabeledNumericContext numeric)
        {
            numeric.LabelWidth = 140;
            numeric.NumericValue = 0;
            numeric.Minimum = 0;
            numeric.NumericUpDownWidth = 56;
        }

        public void setDataContext()
        {
            Settings s = GlobalSettings.getSettings();

            LabeledComboBoxContext currency = new LabeledComboBoxContext();
            setSettingsComboContext(ref currency);
            LabeledComboBoxContext wage = new LabeledComboBoxContext();
            setSettingsComboContext(ref wage);
            LabeledComboBoxContext height = new LabeledComboBoxContext();
            setSettingsComboContext(ref height);
            LabeledComboBoxContext weight = new LabeledComboBoxContext();
            setSettingsComboContext(ref weight);
            LabeledComboBoxContext allowEditing = new LabeledComboBoxContext();
            setSettingsComboContext(ref allowEditing);
            LabeledComboBoxContext language = new LabeledComboBoxContext();
            setSettingsComboContext(ref language);
            LabeledComboBoxContext theme = new LabeledComboBoxContext();
            setSettingsComboContext(ref theme);

            LabeledNumericContext wonderkidsMaxAge = new LabeledNumericContext();
            setNumericsContext(ref wonderkidsMaxAge);
            wonderkidsMaxAge.Maximum = 200;
            LabeledNumericContext wonderkidsMinPA = new LabeledNumericContext();
            setNumericsContext(ref wonderkidsMinPA);
            LabeledNumericContext wonderstaffMinPA = new LabeledNumericContext();
            wonderkidsMinPA.Maximum = 200;
            setNumericsContext(ref wonderstaffMinPA);
            wonderstaffMinPA.Maximum = 200;
            LabeledNumericContext wonderteamsMinRep = new LabeledNumericContext();
            setNumericsContext(ref wonderteamsMinRep);
            wonderteamsMinRep.Maximum = 10000;

            vm = new GeneralTabItemViewModel();
            vm.generalsettings = new LabeledHeaderContext();
            vm.currency = currency;
            vm.wage = wage;
            vm.height = height;
            vm.weight = weight;
            vm.allowEditing = allowEditing;
            vm.language = language;
            vm.theme = theme;
            vm.wonderkidsMaxAge = wonderkidsMaxAge;
            vm.wonderkidsMinPA = wonderkidsMinPA;
            vm.wonderstaffMinPA = wonderstaffMinPA;
            vm.wonderteamsMinRep = wonderteamsMinRep;

            ScoutLocalization localization = globalFuncs.localization;
            vm.currency.ComboBoxItems = localization.currencies;
            vm.wage.ComboBoxItems = localization.wages;
            vm.height.ComboBoxItems = localization.heights;
            vm.weight.ComboBoxItems = localization.weights;
            vm.allowEditing.ComboBoxItems = localization.YesNo;
            vm.language.ComboBoxItems = globalFuncs.languages;
            vm.theme.ComboBoxItems = globalFuncs.themes;

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
            ObservableCollection<string> WindowGeneralLabels = localization.WindowGeneralLabels;
            int index = -1;
            vm.generalsettings.Header = WindowGeneralLabels[++index];
            vm.currency.LabelContent = WindowGeneralLabels[++index];
            vm.wage.LabelContent = WindowGeneralLabels[++index];
            vm.height.LabelContent = WindowGeneralLabels[++index];
            vm.weight.LabelContent = WindowGeneralLabels[++index];
            vm.allowEditing.LabelContent = WindowGeneralLabels[++index];
            vm.language.LabelContent = WindowGeneralLabels[++index];
            vm.theme.LabelContent = WindowGeneralLabels[++index];
            vm.wonderkidsMaxAge.LabelContent = WindowGeneralLabels[++index];
            vm.wonderkidsMinPA.LabelContent = WindowGeneralLabels[++index];
            vm.wonderstaffMinPA.LabelContent = WindowGeneralLabels[++index];
            vm.wonderteamsMinRep.LabelContent = WindowGeneralLabels[++index];
        }
	}
}