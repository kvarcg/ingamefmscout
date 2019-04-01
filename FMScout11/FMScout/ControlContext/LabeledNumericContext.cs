using System;
using System.ComponentModel;
using System.Windows;

namespace FMScout.ControlContext
{
    public class LabeledNumericContext : LabeledContext
    {
        #region LabeledNumeric Properties

        #region NumericValue

        protected int _numericValue = 0;
        public int NumericValue
        {
            get { return _numericValue; }
            set
            {
                _numericValue = value;
                RaisePropertyChanged("NumericValue");
            }
        }

        #endregion

        #region Minimum

        protected int _minimum = (int)0;
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                RaisePropertyChanged("Minimum");
            }
        }
        
        #endregion
        
        #region Maximum

        protected int _maximum = (int)1000000;
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                RaisePropertyChanged("Maximum");
            }
        }

        #endregion

        #region NumericUpDownHeight

        protected double _numericUpDownHeight = (double)20.0;
        public double NumericUpDownHeight
        {
            get { return _numericUpDownHeight; }
            set
            {
                _numericUpDownHeight = value;
                RaisePropertyChanged("NumericUpDownHeight");
            }
        }

        #endregion

        #region NumericUpDownWidth

        protected double _numericUpDownWidth = (double)50.0;
        public double NumericUpDownWidth
        {
            get { return _numericUpDownWidth; }
            set
            {
                _numericUpDownWidth = value;
                RaisePropertyChanged("NumericUpDownWidth");
            }
        }

        #endregion

        #region NumericUpDownAlignment

        protected HorizontalAlignment _NumericUpDownAlignment = HorizontalAlignment.Left;
        public HorizontalAlignment NumericUpDownAlignment
        {
            get { return _NumericUpDownAlignment; }
            set
            {
                _NumericUpDownAlignment = value;
                RaisePropertyChanged("NumericUpDownAlignment");
            }
        }

        #endregion
        
        #endregion LabeledNumeric Properties
    }
}
