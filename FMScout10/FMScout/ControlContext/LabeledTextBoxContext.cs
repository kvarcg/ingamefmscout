using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace FMScout.ControlContext
{
    public class LabeledTextBoxContext : LabeledContext
    {
        #region TextBoxInfoText

        protected object _TextBoxInfoText = null;
        public object TextBoxInfoText
        {
            get { return _TextBoxInfoText; }
            set
            {
                _TextBoxInfoText = value;
                RaisePropertyChanged("TextBoxInfoText");
            }
        }

        #endregion

        #region TextBoxText

        protected object _TextBoxText = null;
        public object TextBoxText
        {
            get { return _TextBoxText; }
            set
            {
                _TextBoxText = value;
                RaisePropertyChanged("TextBoxText");
            }
        }

        #endregion

        #region TextBoxWidth

        protected double _TextBoxWidth = (double)80.0;
        public double TextBoxWidth
        {
            get { return _TextBoxWidth; }
            set
            {
                _TextBoxWidth = value;
                RaisePropertyChanged("TextBoxWidth");
            }
        }

        #endregion

        #region TextBoxHeight

        protected double _TextBoxHeight = (double)22.0;
        public double TextBoxHeight
        {
            get { return _TextBoxHeight; }
            set
            {
                _TextBoxHeight = value;
                RaisePropertyChanged("TextBoxHeight");
            }
        }

        #endregion

        #region TextBoxAlignment

        protected HorizontalAlignment _TextBoxAlignment = HorizontalAlignment.Left;
        public HorizontalAlignment TextBoxAlignment
        {
            get { return _TextBoxAlignment; }
            set
            {
                _TextBoxAlignment = value;
                RaisePropertyChanged("TextBoxAlignment");
            }
        }

        #endregion

        #region TextBoxForeground

        protected Brush _TextBoxForeground = Globals.getGlobalFuncs().defaultProfileTextBoxForeground;
        public Brush TextBoxForeground
        {
            get { return _TextBoxForeground; }
            set
            {
                _TextBoxForeground = value;
                RaisePropertyChanged("TextBoxForeground");
            }
        }

        #endregion
    }
}
