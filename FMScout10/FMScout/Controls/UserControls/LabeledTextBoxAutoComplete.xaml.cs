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
using FMScout.CustomControls;

namespace FMScout.UserControls
{
    /// <summary>
    /// Interaction logic for LabeledTextBoxAutoComplete.xaml
    /// </summary>
    public partial class LabeledTextBoxAutoComplete : UserControl
    {
        public LabeledTextBoxAutoComplete()
        {
            InitializeComponent();
            //this.LayoutRoot.DataContext = this;
        }
        /*
        #region Label Properties

        #region LabelContent

        /// <summary>
        /// LabelContent Dependency Property
        /// </summary>
        public static readonly DependencyProperty LabelContentProperty =
            DependencyProperty.Register("LabelContent", typeof(object), typeof(LabeledTextBox),
                new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets or sets the LabelContent property.
        /// </summary>
        public object LabelContent
        {
            get { return (object)GetValue(LabelContentProperty); }
            set { SetValue(LabelContentProperty, value); }
        }

        #endregion

        #region LabelHeight

        /// <summary>
        /// LabelWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty LabelHeightProperty =
            DependencyProperty.Register("LabelHeight", typeof(double), typeof(LabeledTextBox),
                new FrameworkPropertyMetadata((double)30.0));

        /// <summary>
        /// Gets or sets the LabelWidth property.
        /// </summary>
        public double LabelHeight
        {
            get { return (double)GetValue(LabelHeightProperty); }
            set { SetValue(LabelHeightProperty, value); }
        }

        #endregion

        #region LabelWidth

        /// <summary>
        /// LabelWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(LabeledTextBox),
                new FrameworkPropertyMetadata((double)80.0));

        /// <summary>
        /// Gets or sets the LabelWidth property.
        /// </summary>
        public double LabelWidth
        {
            get { return (double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

        #endregion

        #region LabelAlignment

        /// <summary>
        /// LabelAlignment Dependency Property
        /// </summary>
        public static readonly DependencyProperty LabelAlignmentProperty =
            DependencyProperty.Register("LabelAlignment", typeof(HorizontalAlignment), typeof(LabeledTextBox),
                new FrameworkPropertyMetadata((HorizontalAlignment)HorizontalAlignment.Left));

        /// <summary>
        /// Gets or sets the LabelAlignment property.
        /// </summary>
        public HorizontalAlignment LabelAlignment
        {
            get { return (HorizontalAlignment)GetValue(LabelAlignmentProperty); }
            set { SetValue(LabelAlignmentProperty, value); }
        }

        #endregion

        #endregion Label Properties

        #region TextBox Properties

        #region TextBoxText

        /// <summary>
        /// TextBoxText Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextBoxTextProperty =
            DependencyProperty.Register("TextBoxText", typeof(object), typeof(LabeledTextBox),
                new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets or sets the TextBoxText property.
        /// </summary>
        public object TextBoxText
        {
            get { return (object)GetValue(TextBoxTextProperty); }
            set { SetValue(TextBoxTextProperty, value); }
        }

        #endregion

        #region TextBoxHeight

        /// <summary>
        /// TextBoxWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextBoxHeightProperty =
            DependencyProperty.Register("TextBoxHeight", typeof(double), typeof(LabeledTextBox),
                new FrameworkPropertyMetadata((double)20.0));

        /// <summary>
        /// Gets or sets the TextBoxWidth property.
        /// </summary>
        public double TextBoxHeight
        {
            get { return (double)GetValue(TextBoxHeightProperty); }
            set { SetValue(TextBoxHeightProperty, value); }
        }

        #endregion

        #region TextBoxWidth

        /// <summary>
        /// TextBoxWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextBoxWidthProperty =
            DependencyProperty.Register("TextBoxWidth", typeof(double), typeof(LabeledTextBox),
                new FrameworkPropertyMetadata((double)80.0));

        /// <summary>
        /// Gets or sets the TextBoxWidth property.
        /// </summary>
        public double TextBoxWidth
        {
            get { return (double)GetValue(TextBoxWidthProperty); }
            set { SetValue(TextBoxWidthProperty, value); }
        }

        #endregion

        #region TextBoxAlignment

        /// <summary>
        /// TextBoxAlignment Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextBoxAlignmentProperty =
            DependencyProperty.Register("TextBoxAlignment", typeof(HorizontalAlignment), typeof(LabeledTextBox),
                new FrameworkPropertyMetadata((HorizontalAlignment)HorizontalAlignment.Left));

        /// <summary>
        /// Gets or sets the TextBoxAlignment property.
        /// </summary>
        public HorizontalAlignment TextBoxAlignment
        {
            get { return (HorizontalAlignment)GetValue(TextBoxAlignmentProperty); }
            set { SetValue(TextBoxAlignmentProperty, value); }
        }

        #endregion

        #endregion TextBox Properties
        */
        public Label Label
        {
            get { return this.MyLabel; }
            set { this.MyLabel = value; }
        }

        public AutoCompleteTextBox TextBox
        {
            get { return this.MyTextBox; }
            set { this.MyTextBox = value; }
        }
    }
}
