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
using System.Collections;

namespace FMScout.UserControls
{
    /// <summary>
    /// Interaction logic for LabeledComboBox.xaml
    /// </summary>
    public partial class LabeledComboBox : UserControl
    {
        public LabeledComboBox()
        {
            InitializeComponent();
            //this.LayoutRoot.DataContext = this;
        }
        /*
        #region StackPanel Properties
        
        #region StackPanelOrientation

        /// <summary>
        /// StackPanelOrientation Dependency Property
        /// </summary>
        public static readonly DependencyProperty StackPanelOrientationProperty =
            DependencyProperty.Register("StackPanelOrientation", typeof(Orientation), typeof(LabeledComboBox),
                new FrameworkPropertyMetadata((Orientation)Orientation.Horizontal));

        /// <summary>
        /// Gets or sets the StackPanelOrientation property.
        /// </summary>
        public Orientation StackPanelOrientation
        {
            get { return (Orientation)GetValue(StackPanelOrientationProperty); }
            set { SetValue(StackPanelOrientationProperty, value); }
        }

        #endregion

        #endregion StackPanel Properties

        #region Label Properties

        #region LabelContent

        /// <summary>
        /// LabelContent Dependency Property
        /// </summary>
        public static readonly DependencyProperty LabelContentProperty =
            DependencyProperty.Register("LabelContent", typeof(object), typeof(LabeledComboBox),
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
            DependencyProperty.Register("LabelHeight", typeof(double), typeof(LabeledComboBox),
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
            DependencyProperty.Register("LabelWidth", typeof(double), typeof(LabeledComboBox),
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
            DependencyProperty.Register("LabelAlignment", typeof(HorizontalAlignment), typeof(LabeledComboBox),
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

        #region ComboBox Properties

        #region ComboBoxHeight

        /// <summary>
        /// ComboBoxWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty ComboBoxHeightProperty =
            DependencyProperty.Register("ComboBoxHeight", typeof(double), typeof(LabeledComboBox),
                new FrameworkPropertyMetadata((double)20.0));

        /// <summary>
        /// Gets or sets the ComboBoxWidth property.
        /// </summary>
        public double ComboBoxHeight
        {
            get { return (double)GetValue(ComboBoxHeightProperty); }
            set { SetValue(ComboBoxHeightProperty, value); }
        }

        #endregion

        #region ComboBoxWidth

        /// <summary>
        /// ComboBoxWidth Dependency Property
        /// </summary>
        public static readonly DependencyProperty ComboBoxWidthProperty =
            DependencyProperty.Register("ComboBoxWidth", typeof(double), typeof(LabeledComboBox),
                new FrameworkPropertyMetadata((double)80.0));

        /// <summary>
        /// Gets or sets the ComboBoxWidth property.
        /// </summary>
        public double ComboBoxWidth
        {
            get { return (double)GetValue(ComboBoxWidthProperty); }
            set { SetValue(ComboBoxWidthProperty, value); }
        }

        #endregion

        #region ComboBoxAlignment

        /// <summary>
        /// ComboBoxAlignment Dependency Property
        /// </summary>
        public static readonly DependencyProperty ComboBoxAlignmentProperty =
            DependencyProperty.Register("ComboBoxAlignment", typeof(HorizontalAlignment), typeof(LabeledComboBox),
                new FrameworkPropertyMetadata((HorizontalAlignment)HorizontalAlignment.Left));

        /// <summary>
        /// Gets or sets the ComboBoxAlignment property.
        /// </summary>
        public HorizontalAlignment ComboBoxAlignment
        {
            get { return (HorizontalAlignment)GetValue(ComboBoxAlignmentProperty); }
            set { SetValue(ComboBoxAlignmentProperty, value); }
        }

        #endregion

        #region ComboBoxItems

        /// <summary>
        /// ComboBoxItems Dependency Property
        /// </summary>
        public static readonly DependencyProperty ComboBoxItemsProperty =
            DependencyProperty.Register("ComboBoxItems", typeof(IEnumerable), typeof(LabeledComboBox),
                new FrameworkPropertyMetadata((IEnumerable)null));

        /// <summary>
        /// Gets or sets the ComboBoxItems property.
        /// </summary>
        public IEnumerable ComboBoxItems
        {
            get { return (IEnumerable)GetValue(ComboBoxItemsProperty); }
            set { SetValue(ComboBoxItemsProperty, value); }
        }

        #endregion

        #region ComboBoxSelectedItem

        /// <summary>
        /// ComboBoxSelectedItem Dependency Property
        /// </summary>
        /// 
        public static readonly DependencyProperty ComboBoxSelectedItemProperty =
            DependencyProperty.Register("ComboBoxSelectedItem", typeof(object), typeof(LabeledComboBox),
                new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets or sets the ComboBoxSelectedItem property.
        /// </summary>
        public object ComboBoxSelectedItem
        {
            get { return (object)GetValue(ComboBoxSelectedItemProperty); }
            set { SetValue(ComboBoxSelectedItemProperty, value); }
        }

        #endregion

        #region ComboBoxItemTemplate

        /// <summary>
        /// ComboBoxItemTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty ComboBoxItemTemplateProperty =
            DependencyProperty.Register("ComboBoxItemTemplate", typeof(DataTemplate), typeof(LabeledComboBox),
                new FrameworkPropertyMetadata((DataTemplate)null));

        /// <summary>
        /// Gets or sets the ComboBoxItemTemplate property.
        /// </summary>
        public DataTemplate ComboBoxItemTemplate
        {
            get { return (DataTemplate)GetValue(ComboBoxItemTemplateProperty); }
            set { SetValue(ComboBoxItemTemplateProperty, value); }
        }

        #endregion

        #endregion ComboBox Properties
        */
        public Label Label
        {
            get { return this.MyLabel; }
            set { this.MyLabel = value; }
        }

        public ComboBox ComboBox
        {
            get { return this.MyComboBox; }
            set { this.MyComboBox = value; }
        }		
    }
}
