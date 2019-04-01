using FMScout.ControlContext;

namespace FMScout.ViewModel
{
    public class GeneralTabItemViewModel
    {
        public LabeledHeaderContext generalsettings { get; set; }
        public LabeledComboBoxContext currency { get; set; }
        public LabeledComboBoxContext wage { get; set; }
        public LabeledComboBoxContext height { get; set; }
        public LabeledComboBoxContext weight { get; set; }
        public LabeledComboBoxContext allowEditing { get; set; }
        public LabeledComboBoxContext language { get; set; }
        public LabeledComboBoxContext theme { get; set; }
        public LabeledNumericContext wonderkidsMaxAge { get; set; }
        public LabeledNumericContext wonderkidsMinPA { get; set; }
        public LabeledNumericContext wonderstaffMinPA { get; set; }
        public LabeledNumericContext wonderteamsMinRep { get; set; }
    }
}