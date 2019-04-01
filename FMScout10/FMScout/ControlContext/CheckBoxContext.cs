using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMScout.ControlContext
{
    public class CheckBoxContext : INotifier
    {
        #region IsChecked

        protected bool? _IsChecked = false;
        public bool? IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }

        #endregion

        #region Content

        protected String _Content = "";
        public String Content
        {
            get { return _Content; }
            set
            {
                _Content = value;
                RaisePropertyChanged("Content");
            }
        }

        #endregion

        #region IsSealed

        protected bool _IsSealed = false;
        public bool IsSealed
        {
            get { return _IsSealed; }
            set
            {
                _IsSealed = value;
                RaisePropertyChanged("IsSealed");
            }
        }

        #endregion
    }
}
