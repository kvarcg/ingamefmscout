using System;
using System.ComponentModel;
using System.Windows;

namespace FMScout.ControlContext
{
    public class LabeledNumeric : LabeledContext
    {
        #region LabeledNumeric Properties

        #region Value

        protected int _value = (int)0;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
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

        protected int _maximum = (int)100;
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
