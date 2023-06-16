using Adirev.ViewModel;
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
using System.Windows.Shapes;

namespace Adirev.View
{
    public partial class CredentialsWindow : Window
    {
        public CredentialsWindow(string title)
        {
            InitializeComponent();
            CredentialsWindowViewModel modelView = (CredentialsWindowViewModel)this.DataContext;
            modelView.Title = title;
        }
    }
}
