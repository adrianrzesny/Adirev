using Adirev.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.ViewModel
{
    class CredentialsWindowViewModel : NotifyPropertyChanged
    {
        #region Objects
        private CredentialsWindowModel model = new CredentialsWindowModel();
        #endregion

        #region Properties

        public string Title 
        { 
            get => model.Title; 
            set
            {
                model.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public string Login
        {
            get => model.Login;
            set
            {
                model.Login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password
        {
            get => model.Password;
            set
            {
                model.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        #endregion
    }
}
