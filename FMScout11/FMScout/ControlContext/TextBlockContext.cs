using System;
using System.ComponentModel;
using System.Windows;
using System.Collections;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace FMScout.ControlContext
{
    public class TextBlockContext : INotifier
    {
        #region Text

        protected string _Text = null;
        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                RaisePropertyChanged("Text");
            }
        }

        #endregion
    }
}