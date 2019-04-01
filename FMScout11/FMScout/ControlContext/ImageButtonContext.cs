using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Drawing;
using System.Windows;

namespace FMScout.ControlContext
{
    public class ImageButtonContext : INotifier
    {
        #region ImageProperties

        #region ImageStretch

        protected Stretch _imageStretch = Stretch.UniformToFill;
        public Stretch ImageStretch
        {
            get { return _imageStretch; }
            set
            {
                _imageStretch = value;
                RaisePropertyChanged("ImageStretch");
            }
        }

        #endregion

        #region ImageMargin

        protected Thickness _imageMargin = new Thickness(0, 0, 0, 0);
        public Thickness ImageMargin
        {
            get { return _imageMargin; }
            set
            {
                _imageMargin = value;
                RaisePropertyChanged("ImageMargin");
            }
        }

        #endregion

        #region ImageSource

        protected ImageSource _imageSource = null;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                RaisePropertyChanged("ImageSource");
            }
        }

        #endregion

        #region ImageWidth

        protected double _ImageWidth = Double.NaN;
        public double ImageWidth
        {
            get { return _ImageWidth; }
            set
            {
                _ImageWidth = value;
                RaisePropertyChanged("ImageWidth");
            }
        }

        #endregion

        #region ImageHeight

        protected double _ImageHeight = Double.NaN;
        public double ImageHeight
        {
            get { return _ImageHeight; }
            set
            {
                _ImageHeight = value;
                RaisePropertyChanged("ImageHeight");
            }
        }

        #endregion

        #endregion
    }
}
