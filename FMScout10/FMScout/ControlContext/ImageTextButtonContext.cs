using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace FMScout.ControlContext
{
    public class ImageTextButtonContext : ImageButtonContext
    {
        #region TextBlockProperties

        #region TextBlockText

        protected String _TextBlockText = "";
        public String TextBlockText
        {
            get { return _TextBlockText; }
            set
            {
                _TextBlockText = value;
                RaisePropertyChanged("TextBlockText");
            }
        }

        #endregion

        #region TextBlockWidth

        protected double _TextBlockWidth = Double.NaN;
        public double TextBlockWidth
        {
            get { return _TextBlockWidth; }
            set
            {
                _TextBlockWidth = value;
                RaisePropertyChanged("TextBlockWidth");
            }
        }

        #endregion

        #region TextBlockHeight

        protected double _TextBlockHeight = Double.NaN;
        public double TextBlockHeight
        {
            get { return _TextBlockHeight; }
            set
            {
                _TextBlockHeight = value;
                RaisePropertyChanged("TextBlockHeight");
            }
        }

        #endregion

        #endregion
    }
}
