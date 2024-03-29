﻿using Adirev.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    [Serializable]
    public class ApplicationSession
    {
        #region Properties
        public string System { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string LoginHash { get; set; }
        public string PasswordHash { get; set; }
        public string Path { get; set; }
        private string Login
        {
            get => Cryptography.Decrypt(LoginHash);
            set => LoginHash = Cryptography.Encrypt(value);
        }
        private string Password 
        {
            get => Cryptography.Decrypt(PasswordHash);
            set => PasswordHash = Cryptography.Encrypt(value);
        }
        #endregion

        #region Public Method
        public string GetLogin()
        { return Login; }
        
        public void SetLogin(string login)
        { Login = login; }

        public string GetPassword()
        { return Password; }

        public void SetPassword(string passwod)
        { Password = passwod; }
        #endregion
    }
}
