using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMScout.ControlContext;

namespace FMScout.ViewModel
{
    public class SearchViewModel
    {
        public object dataGrid { get; set; }
        public LabeledHeaderContext groupboxsearch { get; set; }
        public LabeledHeaderContext groupboxresults { get; set; }
        public LabeledHeaderContext customizecolumns { get; set; }
        public TextBlockContext results { get; set; }
        public ImageTextButtonContext search { get; set; }
        public ImageTextButtonContext wonder { get; set; }
    }
}
