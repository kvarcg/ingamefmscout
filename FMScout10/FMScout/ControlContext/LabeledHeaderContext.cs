using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMScout.ControlContext
{
    public class LabeledHeaderContext : INotifier
    {
        #region Header

        protected object _Header = (String)"";
        public object Header
        {
            get { return _Header; }
            set
            {
                _Header = value;
                RaisePropertyChanged("Header");
            }
        }

        #endregion
    }
}
