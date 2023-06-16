using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Adirev.Interface;
using Adirev.Model;
using Adirev.View;
using Adirev.ViewModel;

namespace Adirev.Service
{
    public class DatabaseManager
    {
        #region Enum
        public enum TypeScript
        {
            FN,
            TR,
            P,
            V,
            U
        }
        public enum OpcionExport
        {
            ALL,
            CHECKED
        }
        #endregion

        #region Objects
        private Logger LoggerApplication { get; set; } = Logger.Instance;
        private ApplicationSettings Settings { get; set; } = ApplicationSettings.Instance;
        #endregion

        #region Properties
        public string System { get; set; } = null;
        public string Server { get; set; } = null;
        public string DatabaseEntity { get; set; } = null;
        public string Login { get; set; } = null;
        public string Password { get; set; } = null;
        public List<string> DatabaseFunctions { get; set; }
        public List<string> DatabaseProcedures { get; set; }
        public List<string> DatabaseTables { get; set; }
        public List<string> DatabaseTriggers { get; set; }
        public List<string> DatabaseViews { get; set; }
        #endregion

        #region Public Methods
        public static string GetNameTypeScript(DatabaseManager.TypeScript type)
        {
            switch (type)
            {
                case DatabaseManager.TypeScript.FN:
                    return "Functions";
                case DatabaseManager.TypeScript.TR:
                    return "Triggers";
                case DatabaseManager.TypeScript.P:
                    return "Procedures";
                case DatabaseManager.TypeScript.V:
                    return "Views";
                case DatabaseManager.TypeScript.U:
                    return "Tables";
                default:
                    return "X";
            }
        }

        public bool LoadItems()
        {
            bool result = true;

            try
            {
                IDatabase db = GetDatabaseObject(this.System);

                if (this.DatabaseEntity != null && db != null && result == true)
                {
                    DatabaseFunctions = db.GetItems(DatabaseManager.TypeScript.FN, this);
                    DatabaseProcedures = db.GetItems(DatabaseManager.TypeScript.P, this);
                    DatabaseTables = db.GetItems(DatabaseManager.TypeScript.U, this);
                    DatabaseTriggers = db.GetItems(DatabaseManager.TypeScript.TR, this);
                    DatabaseViews = db.GetItems(DatabaseManager.TypeScript.V, this);
                }
                else
                { result = false; }
            }
            catch (Exception ex)
            {
                result = false;

                if (ServerManager.Ping(Server) && ex.Message.Contains("Login failed"))
                { LoginToDatabase(() => { result = LoadItems(); }); }
                else
                { LoggerApplication.AddLog(ex.Message, true); }
            }

            return result;
        }

        public List<DatabaseItem> GetItemsContents(DatabaseManager.TypeScript type, DatabaseManager.OpcionExport opcionExport, List<string> listToDownload = null)
        {
            List<DatabaseItem> listDatabaseItem = new List<DatabaseItem>();

            try
            {
                IDatabase db = GetDatabaseObject(this.System);

                if (this.DatabaseEntity != null && db != null)
                { listDatabaseItem = db.GetItemsContents(type, this, opcionExport, listToDownload); }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Login failed"))
                { LoginToDatabase(() => { GetItemsContents(type, opcionExport, listToDownload); }); }
                else
                { LoggerApplication.AddLog(ex.Message, true); }
            }

            return listDatabaseItem;
        }

        public static IDatabase GetDatabaseObject(string system)
        {
            IDatabase db;
            switch (system)
            {
                case "Microsoft SQL Server":
                    db = new MSDatabase();
                    break;
                default:
                    db = null;
                    break;
            }

            return db;
        }
        #endregion

        #region Private Method
        private void LoginToDatabase(Action action)
        {
            if (string.IsNullOrEmpty(Login) && string.IsNullOrEmpty(Password) && Settings.Closed == false && Settings.FirstRun == false)
            {
                CredentialsWindow cw = new CredentialsWindow(this.DatabaseEntity);

                Application curApp = Application.Current;
                Window mainWindow = curApp.MainWindow;
                cw.Left = mainWindow.Left + ((mainWindow.Width - cw.Width) / 2);
                cw.Top = mainWindow.Top + ((mainWindow.Height - cw.Height) / 2);

                bool? resultWindow = cw.ShowDialog();
                CredentialsWindowViewModel viewModel = (CredentialsWindowViewModel)cw.DataContext;
                Login = viewModel.Login.Length == 0 ? "#" : viewModel.Login;
                Password = viewModel.Password.Length == 0 ? "#" : viewModel.Password;
                action?.Invoke();
            }
            else
            {
                Login = null;
                Password = null;
                LoggerApplication.AddLog($"Login failed {this.Server}.{this.DatabaseEntity} - Check login and password", true);
            }
        }
        #endregion
    }
}
