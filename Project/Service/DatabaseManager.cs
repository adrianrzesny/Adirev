using System;
using System.Collections.Generic;
using System.Text;
using Adirev.Interface;
using Adirev.Model;

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
        public bool LoadItems()
        {
            bool result = true;

            try
            {
                IDatabase db = GetDatabaseObject(this.System);

                if (this.DatabaseEntity != null && db != null)
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
            { }

            return result;
        }

        public List<DatabaseItem> GetItemsContents(DatabaseManager.TypeScript type, DatabaseManager.OpcionExport opcionExport, List<string> downloadList = null)
        {
            List<DatabaseItem> listDatabaseItem = null;

            try
            {
                IDatabase db = GetDatabaseObject(this.System);

                if (this.DatabaseEntity != null && db != null)
                { listDatabaseItem = db.GetItemsContents(type, this, opcionExport, downloadList); }
            }
            catch (Exception ex)
            { }

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
    }
}
