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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FMScout.CustomControls;

namespace FMScout.UserControls
{
	/// <summary>
	/// Interaction logic for LabeledNumericUpDownMinMaxAttributes.xaml
	/// </summary>
	public partial class LabeledNumericUpDownMinMaxAttributes : UserControl
	{
		        public LabeledNumericUpDownMinMaxAttributes()
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
            DependencyProperty.Register("LabelContent", typeof(object), typeof(LabeledNumericUpDownMinMax),
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
            DependencyProperty.Register("LabelHeight", typeof(double), typeof(LabeledNumericUpDownMinMax),
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
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(LabeledNumericUpDownMinMax),
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
            DependencyProperty.Register("LabelAlignment", typeof(HorizontalAlignment), typeof(LabeledNumericUpDownMinMax),
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

        #region ValueMin

        /// <summary>
        /// NumericUpDownValueMin Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueMinProperty =
            DependencyProperty.Register("ValueMin", typeof(int), typeof(LabeledNumericUpDownMinMax),
                new FrameworkPropertyMetadata(0));

        /// <summary>
        /// Gets or sets the ValueMin property.
        /// </summary>
        public int ValueMin
        {
            get { return (int)GetValue(ValueMinProperty); }
            set { SetValue(ValueMinProperty, value); }
        }

        #endregion

        #region ValueMax

        /// <summary>
        /// NumericUpDownValueMax Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueMaxProperty =
            DependencyProperty.Register("ValueMax", typeof(int), typeof(LabeledNumericUpDownMinMax),
                new FrameworkPropertyMetadata(0));

        /// <summary>
        /// Gets or sets the ValueMax property.
        /// </summary>
        public int ValueMax
        {
            get { return (int)GetValue(ValueMaxProperty); }
            set { SetValue(ValueMaxProperty, value); }
        }

        #endregion

        #region Minimum

        /// <summary>
        /// Minimum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(LabeledNumericUpDownMinMax),
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
            DependencyProperty.Register("Maximum", typeof(int), typeof(LabeledNumericUpDownMinMax),
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

        #region NumericUpDownMinMaxHeight

        /// <summary>
        /// NumericUpDownMinMaxWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty NumericUpDownMinMaxHeightProperty =
            DependencyProperty.Register("NumericUpDownMinMaxHeight", typeof(double), typeof(LabeledNumericUpDownMinMax),
                new FrameworkPropertyMetadata((double)20.0));

        /// <summary>
        /// Gets or sets the NumericUpDownMinMaxWidth property.
        /// </summary>
        public double NumericUpDownMinMaxHeight
        {
            get { return (double)GetValue(NumericUpDownMinMaxHeightProperty); }
            set { SetValue(NumericUpDownMinMaxHeightProperty, value); }
        }

        #endregion

        #region NumericUpDownMinMaxWidth

        /// <summary>
        /// NumericUpDownMinMaxWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty NumericUpDownMinMaxWidthProperty =
            DependencyProperty.Register("NumericUpDownMinMaxWidth", typeof(double), typeof(LabeledNumericUpDownMinMax),
                new FrameworkPropertyMetadata((double)50.0));

        /// <summary>
        /// Gets or sets the NumericUpDownMinMaxWidth property.
        /// </summary>
        public double NumericUpDownMinMaxWidth
        {
            get { return (double)GetValue(NumericUpDownMinMaxWidthProperty); }
            set { SetValue(NumericUpDownMinMaxWidthProperty, value); }
        }

        #endregion

        #region NumericUpDownMinMaxAlignmentProperty

        /// <summary>
        /// NumericUpDownMinMaxAlignment Dependency Property
        /// </summary>
        public static readonly DependencyProperty NumericUpDownMinMaxAlignmentProperty =
            DependencyProperty.Register("NumericUpDownMinMaxAlignment", typeof(HorizontalAlignment), typeof(LabeledNumericUpDownMinMax),
                new FrameworkPropertyMetadata((HorizontalAlignment)HorizontalAlignment.Center));

        /// <summary>
        /// Gets or sets the NumericUpDownMinMaxAlignment property.
        /// </summary>
        public HorizontalAlignment NumericUpDownMinMaxAlignment
        {
            get { return (HorizontalAlignment)GetValue(NumericUpDownMinMaxAlignmentProperty); }
            set { SetValue(NumericUpDownMinMaxAlignmentProperty, value); }
        }

        #endregion

        #endregion NumericUpDown Properties
        */
        private void MyNumericUpDownMin_ValueChanged(object obj, RoutedPropertyChangedEventArgs<decimal> e)
        {
            NumericUpDown v1 = this.MyNumericUpDownMin;
            NumericUpDown v2 = this.MyNumericUpDownMax;
            if (v1.Value >= v2.Value)
            {
                v2.Value = v1.Value + 1;
            }
        }

        private void MyNumericUpDownMax_ValueChanged(object obj, RoutedPropertyChangedEventArgs<decimal> e)
        {
            NumericUpDown v1 = this.MyNumericUpDownMin;
            NumericUpDown v2 = this.MyNumericUpDownMax;
            if (v2.Value <= v1.Value)
            {
                v1.Value = v2.Value - 1;
            }
        }

        private void MyNumericUpDownMin_MinimumChanged(object obj, RoutedPropertyChangedEventArgs<decimal> e)
        {
            NumericUpDown v1 = this.MyNumericUpDownMin;
            NumericUpDown v2 = this.MyNumericUpDownMax;
            v2.Minimum = v1.Minimum + 1;
        }

        private void MyNumericUpDownMax_MaximumChanged(object obj, RoutedPropertyChangedEventArgs<decimal> e)
        {
            NumericUpDown v1 = this.MyNumericUpDownMin;
            NumericUpDown v2 = this.MyNumericUpDownMax;
            v1.Maximum = v2.Maximum - 1;
        }

        public Label Label
        {
            get { return this.MyLabel; }
            set { this.MyLabel = value; }
        }

        public NumericUpDown NumericUpDownMin
        {
            get { return this.MyNumericUpDownMin; }
            set { this.MyNumericUpDownMin = value; }
        }

        public NumericUpDown NumericUpDownMax
        {
            get { return this.MyNumericUpDownMax; }
            set { this.MyNumericUpDownMax = value; }
        }
	}
}