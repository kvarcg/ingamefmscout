using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using FMScout.ControlContext;

namespace FMScout.ViewModel
{
    public class ProfileWindowViewModel : CustomWindowViewModel
    {
        public LabeledHeaderContext objectlist { get; set; }
        public ImageSource profiletype { get; set; }
    }
}
