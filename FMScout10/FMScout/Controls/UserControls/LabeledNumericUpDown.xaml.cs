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
    /// Interaction logic for LabeledNumericUpDown.xaml
    /// </summary>
    public partial class LabeledNumericUpDown : UserControl
    {
        public LabeledNumericUpDown()
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
            DependencyProperty.Register("LabelContent", typeof(object), typeof(LabeledNumericUpDown),
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
            DependencyProperty.Register("LabelHeight", typeof(double), typeof(LabeledNumericUpDown),
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
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(LabeledNumericUpDown),
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
            DependencyProperty.Register("LabelAlignment", typeof(HorizontalAlignment), typeof(LabeledNumericUpDown),
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

        #region NumericUpDown Properties

        #region Value

        /// <summary>
        /// NumericUpDownValue Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(LabeledNumericUpDown),
                new FrameworkPropertyMetadata(0));

        /// <summary>
        /// Gets or sets the LabelContent property.
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region Minimum

        /// <summary>
        /// Minimum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(LabeledNumericUpDown),
                new FrameworkPropertyMetadata(0));

        /// <summary>
        /// Gets or sets the LabelWidth property.
        /// </summary>
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        #endregion

        #region Maximum

        /// <summary>
        /// Maximum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(LabeledNumericUpDown),
                new FrameworkPropertyMetadata(0));

        /// <summary>
        /// Gets or sets the Maximum property.
        /// </summary>
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        #endregion

        #region NumericUpDownHeight

        /// <summary>
        /// NumericUpDownWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty NumericUpDownHeightProperty =
            DependencyProperty.Register("NumericUpDownHeight", typeof(double), typeof(LabeledNumericUpDown),
                new FrameworkPropertyMetadata((double)20.0));

        /// <summary>
        /// Gets or sets the NumericUpDownWidth property.
        /// </summary>
        public double NumericUpDownHeight
        {
            get { return (double)GetValue(NumericUpDownHeightProperty); }
            set { SetValue(NumericUpDownHeightProperty, value); }
        }

        #endregion

        #region NumericUpDownWidth

        /// <summary>
        /// NumericUpDownWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty NumericUpDownWidthProperty =
            DependencyProperty.Register("NumericUpDownWidth", typeof(double), typeof(LabeledNumericUpDown),
                new FrameworkPropertyMetadata((double)50.0));

        /// <summary>
        /// Gets or sets the NumericUpDownWidth property.
        /// </summary>
        public double NumericUpDownWidth
        {
            get { return (double)GetValue(NumericUpDownWidthProperty); }
            set { SetValue(NumericUpDownWidthProperty, value); }
        }

        #endregion

        #region NumericUpDownAlignmentProperty

        /// <summary>
        /// NumericUpDownAlignment Dependency Property
        /// </summary>
        public static readonly DependencyProperty NumericUpDownAlignmentProperty =
            DependencyProperty.Register("NumericUpDownAlignment", typeof(HorizontalAlignment), typeof(LabeledNumericUpDown),
                new FrameworkPropertyMetadata((HorizontalAlignment)HorizontalAlignment.Center));

        /// <summary>
        /// Gets or sets the NumericUpDownAlignment property.
        /// </summary>
        public HorizontalAlignment NumericUpDownAlignment
        {
            get { return (HorizontalAlignment)GetValue(NumericUpDownAlignmentProperty); }
            set { SetValue(NumericUpDownAlignmentProperty, value); }
        }

        #endregion

        #endregion NumericUpDown Properties
        */

        public Label Label
        {
            get { return this.MyLabel; }
            set { this.MyLabel = value; }
        }

        public NumericUpDown NumericUpDown
        {
            get { return this.MyNumericUpDown; }
            set { this.MyNumericUpDown = value; }
        }
    }
}
