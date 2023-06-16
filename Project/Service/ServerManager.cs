using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using Adirev.Interface;
using Adirev.Model;
using Adirev.View;
using Adirev.ViewModel;

namespace Adirev.Service
{
    public class ServerManager
    {
        #region Objects
        private Logger LoggerApplication { get; set; } = Logger.Instance;
        private ApplicationSettings Settings { get; set; } = ApplicationSettings.Instance;
        #endregion

        #region Properties
        public string System { get; set; }
        public string ServerDatabase { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<DatabaseManager> Databases { get; set; }
        #endregion

        #region Constructor
        public ServerManager()
        {
            Databases = new List<DatabaseManager>();
        }
        #endregion

        #region Public Methods
        public bool LoadDatabases()
        {
            bool result = false;

            try
            {
                if (Ping(ServerDatabase))
                {
                    IDatabase db = DatabaseManager.GetDatabaseObject(this.System);

                    List<string> listDatabases = db.GetDatabases(this);
                    Databases.Clear();

                    foreach (var item in listDatabases)
                    {
                        Databases.Add(new DatabaseManager() { DatabaseEntity = item, Login = this.Login, Password = this.Password, Server = this.ServerDatabase, System = this.System });
                    }

                    if (Databases?.Count > 0)
                    {
                        result = true;
                        LoggerApplication.AddLog($"Server connected -> {ServerDatabase}", true);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;

                if (ex.Message.Contains("Login failed"))
                { LoginToDatabase(() => { result = LoadDatabases(); }); }
                else
                { LoggerApplication.AddLog(ex.Message, true); }
            }

            return result;
        }

        public static bool Ping(string server)
        {
            bool result = false;

            var numberSearchChar = server.IndexOf('\\');

            if (numberSearchChar == -1)
            { numberSearchChar = server.IndexOf('/'); }

            if (numberSearchChar != -1)
            { server = server.Substring(0, numberSearchChar); }

            Ping pinger = null;
            try
            {
                if (server.Length != 0)
                {
                    pinger = new Ping();
                    PingReply reply = pinger.Send(server);
                    if (reply.Status == IPStatus.Success)
                    { result = true; }
                }
            }
            catch (PingException)
            { }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            if (result == false && server.Length != 0)
            {
                Logger la = Logger.Instance;
                la.AddLog($"Server connection error -> {server}", true);
            }

            return result;
        }
        #endregion

        #region Private Method
        private void LoginToDatabase(Action action)
        {
            if (Login == null && Password == null && Settings.Closed == false && Settings.FirstRun == false)
            {
                CredentialsWindow cw = new CredentialsWindow(ServerDatabase);

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
                LoggerApplication.AddLog($"Login failed {ServerDatabase} - Check login and password", true);
            }
        }
        #endregion
    }
}
