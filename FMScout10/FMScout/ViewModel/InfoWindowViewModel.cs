using System;
using FMScout.ControlContext;

namespace FMScout.ViewModel
{
    public class InfoWindowViewModel : CustomWindowViewModel
    {
        public LabeledHeaderContext gamesettings { get; set; }
        public LabeledHeaderContext gamestatus { get; set; }
        public LabeledHeaderContext gamestatusinfo { get; set; }
        public LabeledHeaderContext gameversion { get; set; }
        public LabeledHeaderContext gameversioninfo { get; set; }
        public LabeledHeaderContext players { get; set; }
        public LabeledHeaderContext playersinfo { get; set; }
        public LabeledHeaderContext staff { get; set; }
        public LabeledHeaderContext staffinfo { get; set; }
        public LabeledHeaderContext teams { get; set; }
        public LabeledHeaderContext teamsinfo { get; set; }
        public LabeledHeaderContext scoutsettings { get; set; }
        public LabeledHeaderContext settings { get; set; }
        public LabeledHeaderContext settingsinfo { get; set; }
        public LabeledHeaderContext language { get; set; }
        public LabeledHeaderContext languageinfo { get; set; }
        public LabeledHeaderContext theme { get; set; }
        public LabeledHeaderContext themeinfo { get; set; }
        public LabeledHeaderContext currency { get; set; }
        public LabeledHeaderContext currencyinfo { get; set; }
        public LabeledHeaderContext wage { get; set; }
        public LabeledHeaderContext wageinfo { get; set; }
        public LabeledHeaderContext heightdistance { get; set; }
        public LabeledHeaderContext heightdistanceinfo { get; set; }
        public LabeledHeaderContext weight { get; set; }
        public LabeledHeaderContext weightinfo { get; set; }
        public LabeledHeaderContext editorsettings { get; set; }
        public LabeledHeaderContext editing { get; set; }
        public LabeledHeaderContext editinginfo { get; set; }
    }
}
