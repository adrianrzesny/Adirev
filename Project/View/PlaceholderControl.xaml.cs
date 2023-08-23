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
    public partial class Placeholder : UserControl, INotifyPropertyChanged
    {
        #region Variables
        private string placeholderText;
        #endregion

        #region DependencyProperty
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value), typeof(string), typeof(Placeholder),
                new PropertyMetadata(
                    string.Empty));
        #endregion

        #region Properties
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set
            {
                if (value != "Search..")
                { SetValue(ValueProperty, value); }
                else
                { SetValue(ValueProperty, string.Empty); }
            }
        }
        public string PlaceholderText
        {
            get => placeholderText;
            set 
            {
                placeholderText = value;
                OnPropertyChanged(nameof(PlaceholderText));
            }
        }
        #endregion

        #region Constructor
        public Placeholder()
        {
            InitializeComponent();

            Value = string.Empty;
            PlaceholderText = "Search..";
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void PlaceholderTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PlaceholderText == "Search..")
            {
                Value = string.Empty;
                PlaceholderText = string.Empty;
            }
        }

        private void PlaceholderTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Value.Length == 0)
            { PlaceholderText = "Search.."; }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Value = string.Empty;
            PlaceholderText = "Search.."; 
        }

        private void PlaceholderTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Value = ((TextBox)sender).Text;
        }
        #endregion
    }
}
