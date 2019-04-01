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
using Microsoft.Windows.Controls;
using FMScout.ControlContext;
using FMScout.ViewModel;
using FMScout.Model;

namespace FMScout.View.MainWindow
{
	public partial class TeamsSearchTabItem : UserControl
	{
		public List<DataGridColumn> dataGridColumnList = null;
		
		public TeamsSearchTabItem()
		{
			this.InitializeComponent();
            setDataContext();
        }

        public void setDataContext()
        {
            TeamSearchTabItemViewModel vm = new TeamSearchTabItemViewModel();
            LabeledTextBoxContext name = new LabeledTextBoxContext();
            name.LabelContent = "Name";
            name.LabelWidth = 70;
            name.TextBoxText = "name";
            name.TextBoxWidth = 110;

            LabeledTextBoxContext nation = new LabeledTextBoxContext();
            nation.LabelContent = "Nation";
            nation.LabelWidth = 70;
            nation.TextBoxText = "nation";
            nation.TextBoxWidth = 110;

            LabeledTextBoxContext stadium = new LabeledTextBoxContext();
            stadium.LabelContent = "Stadium";
            stadium.LabelWidth = 70;
            stadium.TextBoxText = "stadium";
            stadium.TextBoxWidth = 110;

            LabeledComboBoxContext teamType = new LabeledComboBoxContext();
            teamType.LabelContent = "Team Type";
            teamType.LabelWidth = 70;
            teamType.ComboBoxWidth = 100;
            teamType.ComboBoxItems = Globals.teamTypes;

            LabeledComboBoxContext reputation = new LabeledComboBoxContext();
            reputation.LabelContent = "Reputation";
            reputation.LabelWidth = 70;
            reputation.ComboBoxWidth = 100;
            reputation.ComboBoxItems = Globals.reputations;

            LabeledComboBoxContext region = new LabeledComboBoxContext();
            region.LabelContent = "Region";
            region.LabelWidth = 70;
            region.ComboBoxWidth = 100;
            region.ComboBoxItems = Globals.regions;

            LabeledNumericMinMaxContext transferBudget = new LabeledNumericMinMaxContext();
            transferBudget.LabelContent = "Transfer Budget";
            transferBudget.LabelWidth = 100;
            transferBudget.ValueMax = 200000000;
            transferBudget.Maximum = 200000000;
            transferBudget.NumericUpDownMinMaxWidth = 86;

            LabeledNumericMinMaxContext wageBudget = new LabeledNumericMinMaxContext();
            wageBudget.LabelContent = "Wage Budget";
            wageBudget.LabelWidth = 100;
            wageBudget.ValueMax = 200000000;
            wageBudget.Maximum = 200000000;
            wageBudget.NumericUpDownMinMaxWidth = 86;

            vm.name = name;
            vm.nation = nation;
            vm.stadium = stadium;
            vm.teamType = teamType;
            vm.reputation = reputation;
            vm.region = region;
            vm.transferBudget = transferBudget;
            vm.wageBudget = wageBudget;

            this.DataContext = vm;
        }
		
		public void initDataTableColumns(ref List<int> columnsWidth)
        {
            this.dataGrid.Columns.Clear();
            this.dataGridColumnList = new List<DataGridColumn>(columnsWidth.Count);
            for (int i = 0; i < columnsWidth.Count; ++i)
            {
                this.dataGridColumnList.Add(new DataGridTextColumn());
                this.dataGrid.Columns.Add(dataGridColumnList[i]);
                DataGridColumn dc = this.dataGrid.Columns[i];
                dc.Width = columnsWidth[i];
                dc.Header = CustomDataColumns.teamsColumns[i];
            }
		}
	}
}