using System;
using System.Collections.Generic;
using System.Text;
using Adirev.Interface;

namespace Adirev.Service
{
    public class ServerManager
    {
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
                IDatabase db = DatabaseManager.GetDatabaseObject(this.System);

                if (db != null)
                {
                    List<string> listDatabases = db.GetDatabases(this);
                    Databases.Clear();

                    foreach (var item in listDatabases)
                    {
                        Databases.Add(new DatabaseManager() { DatabaseEntity = item, Login = this.Login, Password = this.Password, Server = this.ServerDatabase, System = this.System });
                    }

                    if (Databases?.Count > 0)
                    { result = true; }
                }
            }
            catch (Exception ex)
            { }

            return result;
        }
        #endregion
    }
}
