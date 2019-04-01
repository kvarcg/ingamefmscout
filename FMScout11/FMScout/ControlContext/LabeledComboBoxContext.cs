using System;
using System.ComponentModel;
using System.Windows;
using System.Collections;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace FMScout.ControlContext
{
    public class LabeledComboBoxContext : LabeledContext
    {
        #region StackPanel Properties

        #region StackPanelOrientation

        protected Orientation _stackPanelOrientation = Orientation.Horizontal;
        public Orientation StackPanelOrientation
        {
            get { return _stackPanelOrientation; }
            set
            {
                _stackPanelOrientation = value;
                RaisePropertyChanged("StackPanelOrientation");
            }
        }

        #endregion

        #endregion StackPanel Properties

        #region LabeledComboBoxContext Properties

        #region ComboBoxHeight

        protected double _comboBoxHeight = (double)22.0;
        public double ComboBoxHeight
        {
            get { return _comboBoxHeight; }
            set
            {
                _comboBoxHeight = value;
                RaisePropertyChanged("ComboBoxHeight");
            }
        }
        
        #endregion
        
        #region ComboBoxWidth

        protected double _comboBoxWidth = (double)30.0;
        public double ComboBoxWidth
        {
            get { return _comboBoxWidth; }
            set
            {
                _comboBoxWidth = value;
                RaisePropertyChanged("ComboBoxWidth");
            }
        }
        
        #endregion

        #region ComboBoxAlignment

        protected HorizontalAlignment _comboBoxAlignment = HorizontalAlignment.Center;
        public HorizontalAlignment ComboBoxAlignment
        {
            get { return _comboBoxAlignment; }
            set
            {
                _comboBoxAlignment = value;
                RaisePropertyChanged("ComboBoxAlignment");
            }
        }

        #endregion

        #region ComboBoxItems

        protected IEnumerable _comboBoxItems = null;
        public IEnumerable ComboBoxItems
        {
            get { return _comboBoxItems; }
            set
            {
                _comboBoxItems = value;
                RaisePropertyChanged("ComboBoxItems");
            }
        }

        #endregion

        #region ComboBoxSelectedIndex

        protected int _comboBoxSelectedIndex = -1;
        public int ComboBoxSelectedIndex
        {
            get { return _comboBoxSelectedIndex; }
            set
            {
                if (!GlobalSettings.getSettings().changingLanguage)
                {
                    _comboBoxSelectedIndex = value;
                    RaisePropertyChanged("ComboBoxSelectedIndex");
                    //System.Diagnostics.Debug.WriteLine("Changed to: " + _comboBoxSelectedIndex);
                    //if (_comboBoxSelectedIndex != -1)
                     //   System.Diagnostics.Debug.WriteLine("Item: " + ((ObservableCollection<String>)_comboBoxItems)[_comboBoxSelectedIndex]);
                }
            }
        }

        #endregion

        //#region ComboBoxSelectedItem

        //protected object _ComboBoxSelectedIndex = -1;
        //public object ComboBoxSelectedItem
        //{
        //    get { return _comboBoxSelectedItem; }
        //    set
        //    {
        //        _comboBoxSelectedItem = value;
        //        RaisePropertyChanged("ComboBoxSelectedItem");
        //    }
        //}

        //#endregion

        #region ComboBoxItemTemplate

        protected DataTemplate _comboBoxItemTemplate = null;
        public DataTemplate ComboBoxItemTemplate
        {
            get { return _comboBoxItemTemplate; }
            set
            {
                _comboBoxItemTemplate = value;
                RaisePropertyChanged("ComboBoxItemTemplate");
            }
        }

        #endregion

        #endregion
    }
}
