using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    class CredentialsWindowModel
    {
        #region Variables
        private string title = String.Empty;
        private string login = String.Empty;
        private string password = String.Empty;
        #endregion

        #region Properties
        public string Title 
        {
            get => title; 
            set => title = value; 
        }
        public string Login
        {
            get => login;
            set => login = value;
        }
        public string Password
        {
            get => password; 
            set => password = value;
        }
        #endregion

        #region Construcotr
        public CredentialsWindowModel()
        {
            Title = "Login Window";
        }
        #endregion
    }
}
