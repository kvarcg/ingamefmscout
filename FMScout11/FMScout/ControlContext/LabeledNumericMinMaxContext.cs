using System.Collections;
using System.Windows;

namespace FMScout.ControlContext
{
    public class LabeledNumericMinMaxContext : LabeledContext
    {
        #region LabeledNumeric Properties

        #region ValueMin

        protected int _valueMin = (int)-1;
        public int ValueMin
        {
            get { return _valueMin; }
            set
            {
                _valueMin = value;
                RaisePropertyChanged("ValueMin");
            }
        }

        #endregion

        #region ValueMax

        protected int _valueMax = (int)0;
        public int ValueMax
        {
            get { return _valueMax; }
            set
            {
                _valueMax = value;
                RaisePropertyChanged("ValueMax");
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

        #region NumericUpDownMinMaxHeight

        protected double _numericUpDownMinMaxHeight = (double)20.0;
        public double NumericUpDownMinMaxHeight
        {
            get { return _numericUpDownMinMaxHeight; }
            set
            {
                _numericUpDownMinMaxHeight = value;
                RaisePropertyChanged("NumericUpDownMinMaxHeight");
            }
        }

        #endregion

        #region NumericUpDownMinMaxWidth

        protected double _numericUpDownMinMaxWidth = (double)50.0;
        public double NumericUpDownMinMaxWidth
        {
            get { return _numericUpDownMinMaxWidth; }
            set
            {
                _numericUpDownMinMaxWidth = value;
                RaisePropertyChanged("NumericUpDownMinMaxWidth");
            }
        }

        #endregion

        #region NumericUpDownMinMaxAlignment

        protected HorizontalAlignment _NumericUpDownMinMaxAlignment = HorizontalAlignment.Left;
        public HorizontalAlignment NumericUpDownMinMaxAlignment
        {
            get { return _NumericUpDownMinMaxAlignment; }
            set
            {
                _NumericUpDownMinMaxAlignment = value;
                RaisePropertyChanged("NumericUpDownMinMaxAlignment");
            }
        }

        #endregion

        #endregion LabeledNumeric Properties
    }
}
