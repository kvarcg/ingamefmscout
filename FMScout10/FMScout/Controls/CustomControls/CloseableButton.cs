using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FMScout.CustomControls
{
    public class CloseableButton : ToggleButton
    {
        #region Commands

        private static RoutedCommand _ButtonPressed;

        public static RoutedCommand ButtonPressed
        {
            get
            {
                return _ButtonPressed;
            }
        }

        private static void InitializeCommands()
        {
            _ButtonPressed = new RoutedCommand("ButtonPressed", typeof(ToggleButton));
            CommandManager.RegisterClassCommandBinding(typeof(ToggleButton), 
                new CommandBinding(_ButtonPressed, OnButtonPressed));
            CommandManager.RegisterClassInputBinding(typeof(ToggleButton), 
                new InputBinding(_ButtonPressed, new MouseGesture(MouseAction.LeftClick)));
        }

        #endregion

        private static void OnButtonPressed(object sender, ExecutedRoutedEventArgs e)
        {
            Button control = sender as Button;
         
        }
    }
}
