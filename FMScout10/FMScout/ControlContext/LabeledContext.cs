using System;
using System.ComponentModel;
using System.Windows;

namespace FMScout.ControlContext
{
    public class LabeledContext : INotifier
    {
        #region LabelContext Properties

        #region LabelContent

        protected object _labelContent = (String)"";
        public object LabelContent
        {
            get { return _labelContent; }
            set
            {
                _labelContent = value;
                RaisePropertyChanged("LabelContent");
            }
        }

        #endregion

        #region LabelWidth

        protected double _labelWidth = (double)80.0;
        public double LabelWidth
        {
            get { return _labelWidth; }
            set
            {
                _labelWidth = value;
                RaisePropertyChanged("LabelWidth");
            }
        }

        #endregion

        #region LabelHeight

        protected double _labelHeight = (double)22.0;
        public double LabelHeight
        {
            get { return _labelHeight; }
            set
            {
                _labelHeight = value;
                RaisePropertyChanged("LabelHeight");
            }
        }

        #endregion

        #region LabelAlignment

        protected HorizontalAlignment _labelAlignment = HorizontalAlignment.Left;
        public HorizontalAlignment LabelAlignment
        {
            get { return _labelAlignment; }
            set
            {
                _labelAlignment = value;
                RaisePropertyChanged("LabelAlignment");
            }
        }

        #endregion
		
		#region LabelFontWeight

        protected FontWeight _labelFontWeight = FontWeights.Normal;
        public FontWeight LabelFontWeight
        {
            get { return _labelFontWeight; }
            set
            {
                _labelFontWeight = value;
                RaisePropertyChanged("LabelFontWeight");
            }
        }

        #endregion

        #endregion LabelContext Properties
    }
}
