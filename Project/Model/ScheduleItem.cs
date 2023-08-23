using Adirev.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    [Serializable]
    class ScheduleItem
    {
        #region Properties
        public string Name { get; set; }
        public string System { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string LoginHash { get; set; }
        public string PasswordHash { get; set; }
        public string Path { get; set; }
        public bool IsCheckedHour { get; set; }
        public bool IsCheckedDay { get; set; }
        public bool IsCheckedWeek { get; set; }
        public string Time { get; set; }
        public string Day { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastExecutionTime { get; set; }
        public DateTime RealLastExecutionTime { get; set; }
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

        #region Constructor
        public ScheduleItem()
        { }

        public ScheduleItem(string login, string password)
        {
            SetLogin(login);
            SetPassword(password);
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
