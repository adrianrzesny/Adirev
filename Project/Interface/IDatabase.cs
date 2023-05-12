using System;
using System.Collections.Generic;
using System.Text;
using Adirev.Model;
using Adirev.Service;

namespace Adirev.Interface
{
    public interface IDatabase
    {
        public List<string> GetItems(DatabaseManager.TypeScript type, DatabaseManager database);
        public List<string> GetDatabases(ServerManager server);
        public List<DatabaseItem> GetItemsContents(DatabaseManager.TypeScript type, DatabaseManager database, DatabaseManager.OpcionExport opcionExport, List<string> listToDownload = null);
    }
}
