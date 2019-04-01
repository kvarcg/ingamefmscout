using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMScout.ControlContext;

namespace FMScout.ViewModel
{
    public class DonateWindowViewModel : CustomWindowViewModel
    {
        public ImageTextButtonContext ok { get; set; }
        public ImageTextButtonContext cancel { get; set; }
    }
}
