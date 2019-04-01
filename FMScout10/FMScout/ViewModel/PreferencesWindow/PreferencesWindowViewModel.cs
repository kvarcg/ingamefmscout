using System;
using FMScout.ControlContext;

namespace FMScout.ViewModel
{
    public class PreferencesWindowViewModel : CustomWindowViewModel
    {
        public LabeledComboBoxContext setting { get; set; }
        public ImageTextButtonContext ok { get; set; }
        public ImageTextButtonContext cancel { get; set; }
        public ImageButtonContext save { get; set; }
        public ImageButtonContext del { get; set; }
        public ImageButtonContext def { get; set; }
        public LabeledHeaderContext general { get; set; }
        public LabeledHeaderContext player { get; set; }
        public LabeledHeaderContext staff { get; set; }
        public LabeledHeaderContext team { get; set; }
        public LabeledHeaderContext shortlist { get; set; }
        public LabeledHeaderContext playercolumnsettings { get; set; }
        public LabeledHeaderContext staffcolumnsettings { get; set; }
        public LabeledHeaderContext teamcolumnsettings { get; set; }
        public LabeledHeaderContext shortlistcolumnsettings { get; set; }
        public LabeledHeaderContext selectedallcolumns { get; set; }
        public LabeledHeaderContext clearcolumns { get; set; }
        public LabeledHeaderContext defcolumns { get; set; }
        public LabeledHeaderContext buttonWarningTooltip { get; set; }
        public LabeledHeaderContext buttonSaveTooltip { get; set; }
        public LabeledHeaderContext buttonDeleteTooltip { get; set; }
        public LabeledHeaderContext buttonSetDefaultTooltip { get; set; }
        public LabeledHeaderContext buttonUseChangesTooltip { get; set; }
        public LabeledHeaderContext buttonCancelChangesTooltip { get; set; }
        public LabeledHeaderContext buttonUseChangesContent { get; set; }
        public LabeledHeaderContext buttonCancelChangesContent { get; set; }
    }
}
