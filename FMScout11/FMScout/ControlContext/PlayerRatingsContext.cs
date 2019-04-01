using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FMScout.ControlContext
{
	public class PlayerRatingsContext : INotifier
    {
        #region PlayerRatingsProperties

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
        
        #region LabelForeground

        protected Brush _LabelForeground = App.Current.Resources["ProfileRatingBackground"] as SolidColorBrush;
        public Brush LabelForeground
        {
            get { return _LabelForeground; }
            set
            {
                _LabelForeground = value;
                RaisePropertyChanged("LabelForeground");
            }
        }

        #endregion
		
		#region FontWeight

        protected FontWeight _LabelFontWeight = FontWeights.Normal;
        public FontWeight LabelFontWeight
        {
            get { return _LabelFontWeight; }
            set
            {
                _LabelFontWeight = value;
                RaisePropertyChanged("LabelFontWeight");
            }
        }

        #endregion

        #endregion
    }
}