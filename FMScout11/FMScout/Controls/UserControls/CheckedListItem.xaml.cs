using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FMScout.UserControls
{
    /// <summary>
    /// Interaction logic for CheckedListItem.xaml
    /// </summary>
    public partial class CheckedListItem : UserControl
    {
        public CheckedListItem()
        {
            InitializeComponent();
            this.MyCheckBox.DataContext = this;
        }
        
        #region CheckedListItem Properties

        #region CheckBoxContent

        /// <summary>
        /// checkBoxContent Dependency Property
        /// </summary>
        public static readonly DependencyProperty CheckBoxContentProperty =
            DependencyProperty.Register("CheckBoxContent", typeof(object), typeof(CheckedListItem),
                new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Gets or sets the CheckBoxContent property.
        /// </summary>
        public object CheckBoxContent
        {
            get { return (object)GetValue(CheckBoxContentProperty); }
            set { SetValue(CheckBoxContentProperty, value); }
        }

        #endregion
        
        #region isChecked

        /// <summary>
        /// isChecked Dependency Property
        /// </summary>
        public static readonly DependencyProperty isCheckedProperty =
            DependencyProperty.Register("isChecked", typeof(bool), typeof(CheckedListItem),
                new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Gets or sets the isChecked property.
        /// </summary>
        public bool isChecked
        {
            get { return (bool)GetValue(isCheckedProperty); }
            set { SetValue(isCheckedProperty, value); }
        }

        #endregion

        #endregion CheckedListItem Properties
		
        public CheckBox CheckBox
        {
            get { return this.MyCheckBox; }
            set { this.MyCheckBox = value; }
        }
    }
}
