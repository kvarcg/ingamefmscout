using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMScout.ControlContext;
using FMScout.UserControls;

namespace FMScout.ViewModel
{
    public abstract class ProfileViewModel
    {
        public int ID { get; set; }
        public CheckBoxContext SelectionButton { get; set; }

        protected virtual void setProfileContext(LabeledTextBoxContext num)
        {
            num.LabelWidth = 110;
            num.LabelHeight = 22;
            num.TextBoxWidth = 110;
            num.TextBoxHeight = 22;
        }

        protected virtual void setAttributeContext(ref LabeledTextBoxContext num)
        {
            num.LabelWidth = 100;
            num.LabelHeight = 19;
            num.TextBoxWidth = 38;
            num.TextBoxHeight = 19;
            num.TextBoxAlignment = HorizontalAlignment.Center;
        }

        protected virtual void setAttributeContext(LabeledTextBoxContext num)
        {
            num.LabelWidth = 100;
            num.LabelHeight = 19;
            num.TextBoxWidth = 38;
            num.TextBoxHeight = 19;
            num.TextBoxAlignment = HorizontalAlignment.Center;
        }
    }
}
