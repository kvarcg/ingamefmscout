using System;
using FMScout.ControlContext;

namespace FMScout.ViewModel
{
    public class TeamSearchTabItemViewModel : SearchViewModel
    {
        public LabeledTextBoxContext name { get; set; }
        public LabeledTextBoxContext nation { get; set; }
        public LabeledTextBoxContext stadium { get; set; }
        public LabeledComboBoxContext teamtype { get; set; }
        public LabeledComboBoxContext reputation { get; set; }
        public LabeledComboBoxContext region { get; set; }
        public LabeledNumericMinMaxContext transferBudget { get; set; }
        public LabeledNumericMinMaxContext wageBudget { get; set; }
    }

    public class TeamGridViewModel : GridViewModel
    {
        public TEAMSTATE TeamState { get; set; }
        public String Name { get; set; }
        public String Nation { get; set; }
        public int Reputation { get; set; }
        public String Status { get; set; }
        public String Stadium { get; set; }
        public int TransferBudget { get; set; }
        public int RemainingBudget { get; set; }
        public int WageBudget { get; set; }
        public int WageUsed { get; set; }
        public int BudgetBalance { get; set; }
        public int TransferRevenueAvailable { get; set; }
        public int CurrentAffiliatedClubs { get; set; }
        public int MaxAffiliatedClubs { get; set; }
        public int TrainingFacilities { get; set; }
        public int YouthFacilities { get; set; }
        public int MaximumAttendance { get; set; }
        public int AverageAttendance { get; set; }
        public int MinimumAttendance { get; set; }
        public int Decay { get; set; }
        public int FieldWidth { get; set; }
        public int FieldLength { get; set; }
        public int CurrentCapacity { get; set; }
        public int SeatingCapacity { get; set; }
        public int ExpansionCapacity { get; set; }
        public int UsedCapacity { get; set; }
    }
}