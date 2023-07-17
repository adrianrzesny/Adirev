using Adirev.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Adirev.View
{
    public partial class Placeholder : UserControl
    {
        #region Properties
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                if (!isFocus && Value?.Length == 0)
                { PlaceholderTextBox.Text = PlaceholderText; }
            }
        }
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set 
            { 
                SetValue(PlaceholderTextProperty, value);
                PlaceholderTextBox.Text = PlaceholderText;
            }
        }
        #endregion

        #region Variables
        private bool isFocus = false;
        #endregion

        #region DependencyProperty
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value), typeof(string), typeof(Placeholder),
                new PropertyMetadata(
                    string.Empty));

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(Placeholder), typeof(string), typeof(Placeholder),
                new PropertyMetadata(
                    string.Empty));
        #endregion

        #region Constructor
        public Placeholder()
        {
            InitializeComponent();
            Value = string.Empty;
        }
        #endregion

        #region Events
        private void PlaceholderTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            isFocus = true;

            if (PlaceholderTextBox.Text == PlaceholderText)
            {
                Value = string.Empty;
                PlaceholderTextBox.Text = string.Empty;
            }
        }

        private void PlaceholderTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            isFocus = false;
            Value = PlaceholderTextBox.Text;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Value = string.Empty;
            PlaceholderTextBox.Text = PlaceholderText;
        }

        private void PlaceholderTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Value = ((TextBox)sender).Text;

            if (PlaceholderTextBox.Text == PlaceholderText)
            { Value = string.Empty; }
        }
        #endregion
    }
}
