/************************************* Module Header **************************************\
* Module Name:	AutoCompleteTextBox.cs
* Project:		CSWPFAutoCompleteTextBox
* Copyright (c) Microsoft Corporation.
* 
* This example demonstrates how to achieve AutoComplete TextBox in WPF Application.
* 
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* History:
* * 10/20/2009 3:00 PM Bruce Zhou Created
 * 
\******************************************************************************************/



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
using System.Collections.ObjectModel;

namespace FMScout.CustomControls
{

    // Achieve AutoComplete TextBox or ComboBox
    public class AutoCompleteTextBox : ComboBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompleteTextBox"/> class.
        /// </summary>
        ///
        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox), 
                new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));

            //TextProperty.OverrideMetadata(
            //    typeof(AutoCompleteTextBox),
            //    new FrameworkPropertyMetadata(new PropertyChangedCallback(TextPropertyChanged)));
        }

        public AutoCompleteTextBox()
        {
            //load and apply style to the ComboBox.
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("/" + this.GetType().Assembly.GetName().Name + 
                ";component/Themes/" +
                GlobalSettings.getSettings().currentThemePath +
                "/Contents/ComboBox.xaml",
                 UriKind.Relative);
            this.Resources = rd;
            //disable default Text Search Function
            this.IsTextSearchEnabled = false;
        }
 
        /// <summary>
        ///  override OnApplyTemplate method 
        /// get TextBox control out of Combobox control, and hook up TextChanged event.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //get the textbox control in the ComboBox control
            TextBox textBox = this.Template.FindName("PART_EditableTextBox", this) as TextBox;
            if (textBox != null)
            {
                //disable Autoword selection of the TextBox
                textBox.AutoWordSelection = false;
                //handle TextChanged event to dynamically add Combobox items.
                textBox.TextChanged += new TextChangedEventHandler(textBox_TextChanged);
            }
        }

        //The autosuggestionlist source.
        private ObservableCollection<string> _autoSuggestionList = new ObservableCollection<string>();

        /// <summary>
        /// Gets or sets the auto suggestion list.
        /// </summary>
        /// <value>The auto suggestion list.</value>
        public ObservableCollection<string> AutoSuggestionList
        {
            get { return _autoSuggestionList; }
            set { _autoSuggestionList = value; }
        }

        //The autosuggestionlist to use for display.
        private ObservableCollection<string> _displayautoSuggestionList = new ObservableCollection<string>();

        /// <summary>
        /// Gets or sets the auto suggestion list.
        /// </summary>
        /// <value>The auto suggestion list.</value>
        public ObservableCollection<string> DisplayAutoSuggestionList
        {
            get { return _displayautoSuggestionList; }
            set { _displayautoSuggestionList = value; }
        }

      
        /// <summary>
        /// main logic to generate auto suggestion list.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.TextChangedEventArgs"/> 
        /// instance containing the event data.</param>
        void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (GlobalSettings.getSettings().settingTheme) return;
            
            TextBox textBox = sender as TextBox;
            textBox.AutoWordSelection = false;
            // if the word in the textbox is selected, then don't change item collection
            if ((textBox.SelectionStart != 0 || textBox.Text.Length==0))
            {
                this.Items.Clear();
                if (textBox.Text.Length >= TextBoxInfoMinChars)
                {
                    //add new filtered items according the current TextBox input
                    if (!string.IsNullOrEmpty(textBox.Text))
                    {
                        String lowerText = textBox.Text.ToLower();
                        Globals.getGlobalFuncs().scoutTimer.start();
                        for (int i = 0; i < this._autoSuggestionList.Count; ++i)
                        {
                            String s = this._autoSuggestionList[i];
                            String s2 = this._autoSuggestionList[i];
                            if (this._displayautoSuggestionList.Count > 0)
                                s2 = this._displayautoSuggestionList[i];

                            int index = s.ToLower().IndexOf(lowerText);
                            if (index > -1)
                            {
                                AutoCompleteEntry entry = new AutoCompleteEntry();
                                if (index == 0)
                                {
                                    entry.addBold(s2.Substring(index, textBox.Text.Length));
                                    entry.addUnbold(s2.Substring(index + textBox.Text.Length));
                                }
                                else
                                {
                                    entry.addUnbold(s2.Substring(0, index));
                                    entry.addBold(s2.Substring(index, textBox.Text.Length));
                                    entry.addUnbold(s2.Substring(index + textBox.Text.Length));
                                }

                                TextBlock tb = entry.tb;
                                entry.Content = tb;
                                this.Items.Add(entry);
                            }
                            if (this.Items.Count >= 50) break;
                        }
                        Globals.getGlobalFuncs().scoutTimer.stop();
                        System.Diagnostics.Debug.WriteLine("Searching : " + textBox.Text + " took: " 
                            + Globals.getGlobalFuncs().scoutTimer.miliseconds()  + "ms and results: " + this.Items.Count);
                    }
                }
            }

            // open or close dropdown of the ComboBox according to whether there are items in the 
            // fitlered result.
            this.IsDropDownOpen = this.HasItems;

            //avoid auto selection
            textBox.Focus();
            textBox.SelectionStart = textBox.Text.Length;
        }


        public static readonly DependencyProperty TextBoxInfoMinCharsProperty = DependencyProperty.Register(
    "TextBoxInfoMinChars",
    typeof(int),
    typeof(AutoCompleteTextBox),
    new PropertyMetadata(0));

        public int TextBoxInfoMinChars
        {
            get { return (int)GetValue(TextBoxInfoMinCharsProperty); }
            set { SetValue(TextBoxInfoMinCharsProperty, value); }
        }

        public static readonly DependencyProperty TextBoxInfoProperty = DependencyProperty.Register(
    "TextBoxInfo",
    typeof(string),
    typeof(AutoCompleteTextBox),
    new PropertyMetadata(string.Empty));

        public string TextBoxInfo
        {
            get { return (string)GetValue(TextBoxInfoProperty); }
            set { SetValue(TextBoxInfoProperty, value); }
        }

        private static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
            "HasText",
            typeof(bool),
            typeof(AutoCompleteTextBox),
            new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        public bool HasText
        {
            get
            {
                return (bool)GetValue(HasTextProperty);
            }
        }

        static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            InfoTextBox itb = (InfoTextBox)sender;

            bool actuallyHasText = itb.Text.Length > 0;
            if (actuallyHasText != itb.HasText)
            {
                itb.SetValue(HasTextPropertyKey, actuallyHasText);
            }
        }
    }

    //class derived from ComboBoxItem.
    /// <summary>
    /// Extended ComboBox Item
    /// </summary>
    public class AutoCompleteEntry : ComboBoxItem
    {
        public TextBlock tb;

        //text of the item
        private String _text;

        /// <summary>
        /// Contrutor of AutoCompleteEntry class
        /// </summary>
        /// <param name="text">All the Text of the item </param>
        /// <param name="bold">The already entered part of the Text</param>
        /// <param name="unbold">The remained part of the Text</param>
        //public AutoCompleteEntry(string text, string bold, string unbold)
        //{
        //    //_text = text;
        //    tb = new TextBlock();
        //    //highlight the current input Text
        //    tb.Inlines.Add(new Run { Text = bold, FontWeight = FontWeights.Bold,
        //        Foreground = Globals.getGlobalFuncs().selectedAutoCompleteBrush});
        //    tb.Inlines.Add(new Run { Text = unbold });
        //    this.Content = tb;
        //}

        public AutoCompleteEntry()
        {
            _text = text;
            tb = new TextBlock();
        }

        public void addUnbold(String s)
        {
            tb.Inlines.Add(new Run { Text = s });
        }
        
        public void addBold(String s)
        {
            tb.Inlines.Add(new Run
            {
                Text = s,
                FontWeight = FontWeights.Bold,
                Foreground = Globals.getGlobalFuncs().selectedAutoCompleteBrush
            });
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string text
        {
            get { return _text; }
        }
    }
}