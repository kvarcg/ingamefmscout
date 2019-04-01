using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMScout.ControlContext;

namespace FMScout.ViewModel.MainWindow
{
    public class MainWindowViewModel
    {
        public LabeledHeaderContext menuload { get; set; }
        public LabeledHeaderContext menuloadfm { get; set; }
        public LabeledHeaderContext menuloadexit { get; set; }
        public LabeledHeaderContext menushortlist { get; set; }
        public LabeledHeaderContext menushortlistimport { get; set; }
        public LabeledHeaderContext menushortlistexport { get; set; }
        public LabeledHeaderContext menushortlistexportsel { get; set; }
        public LabeledHeaderContext menushortlistadd { get; set; }
        public LabeledHeaderContext menusearch { get; set; }
        public LabeledHeaderContext menusearchplayers { get; set; }
        public LabeledHeaderContext menusearchstaff { get; set; }
        public LabeledHeaderContext menusearchteams { get; set; }
        public LabeledHeaderContext menusearchshortlist { get; set; }
        public LabeledHeaderContext menusearchnow { get; set; }
        public LabeledHeaderContext menuclear { get; set; }
        public LabeledHeaderContext menuclearplayers { get; set; }
        public LabeledHeaderContext menuclearstaff { get; set; }
        public LabeledHeaderContext menuclearteams { get; set; }
        public LabeledHeaderContext menuclearshortlist { get; set; }
        public LabeledHeaderContext menuclearall { get; set; }
        public LabeledHeaderContext menutools { get; set; }
        public LabeledHeaderContext menutoolspref { get; set; }
        public LabeledHeaderContext menuhelp { get; set; }
        public LabeledHeaderContext menuhelpabout { get; set; }
        public LabeledHeaderContext menuhelpdonate { get; set; }
        public LabeledHeaderContext currentgamedate { get; set; }
        public ImageTextButtonContext currentscreen { get; set; }
        public ImageTextButtonContext tabplayers { get; set; }
        public ImageTextButtonContext tabstaff { get; set; }
        public ImageTextButtonContext tabteams { get; set; }
        public ImageTextButtonContext tabshortlist { get; set; }
        public LabeledHeaderContext general { get; set; }
        public LabeledHeaderContext attributes { get; set; }
        public ImageTextButtonContext showinfo { get; set; }
    }
}
