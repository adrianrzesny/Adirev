﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Adirev.Class;
using Adirev.Model;
using Adirev.Service;
using Adirev.ViewModel;
using System.Threading.Tasks;

namespace Adirev.Models
{
    public delegate void MainWindowModelEventHandler();
    public class MainWindowModel
    {
        #region Variables
        private string tiFunctionsName;
        private string tiProceduresName;
        private string tiTablesName;
        private string tiTriggersName;
        private string tiViewsName;
        private bool isCheckedFunctions;
        private bool isCheckedProcedures;
        private bool isCheckedTables;
        private bool isCheckedTriggers;
        private bool isCheckedViews;
        #endregion

        #region Objects
        private ServerManager ServerDatabase;
        #endregion

        #region Properties
        public ObservableCollection<object> MenuItems { get; set; }
        public bool Connected { get; set; }
        public bool ServerIsEnabled { get; set; }
        public bool DatabaseIsEnabled { get; set; }
        public bool PathIsEnabled { get; set; }
        public ObservableCollection<CheckBoxItem> DatabaseFunctions { get; set; }
        public ObservableCollection<CheckBoxItem> DatabaseProcedures { get; set; }
        public ObservableCollection<CheckBoxItem> DatabaseTables { get; set; }
        public ObservableCollection<CheckBoxItem> DatabaseTriggers { get; set; }
        public ObservableCollection<CheckBoxItem> DatabaseViews { get; set; }

        public string TIFunctionsName
        {
            get => tiFunctionsName;
            set
            {
                tiFunctionsName = value;
                FunctionsNameChanged?.Invoke();
            }
        }
        public string TIProceduresName
        {
            get => tiProceduresName;
            set
            {
                tiProceduresName = value;
                ProceduresNameChanged?.Invoke();
            }
        }
        public string TITablesName
        {
            get => tiTablesName;
            set
            {
                tiTablesName = value;
                TablesNameChanged?.Invoke();
            }
        }
        public string TITriggersName
        {
            get => tiTriggersName;
            set
            {
                tiTriggersName = value;
                TriggersNameChanged?.Invoke();
            }
        }
        public string TIViewsName
        {
            get => tiViewsName;
            set
            {
                tiViewsName = value;
                ViewsNameChanged?.Invoke();
            }
        }
        public bool IsCheckedFunctions
        {
            get => isCheckedFunctions;
            set
            {
                isCheckedFunctions = value;
                StatusCheckedFunctionsChanged?.Invoke();
            }
        }
        public bool IsCheckedProcedures
        {
            get => isCheckedProcedures;
            set
            {
                isCheckedProcedures = value;
                StatusCheckedProceduresChanged?.Invoke();
            }
        }
        public bool IsCheckedTables
        {
            get => isCheckedTables;
            set
            {
                isCheckedTables = value;
                StatusCheckedTablesChanged?.Invoke();
            }
        }
        public bool IsCheckedTriggers
        {
            get => isCheckedTriggers;
            set
            {
                isCheckedTriggers = value;
                StatusCheckedTriggersChanged?.Invoke();
            }
        }
        public bool IsCheckedViews
        {
            get => isCheckedViews;
            set
            {
                isCheckedViews = value;
                StatusCheckedViewsChanged?.Invoke();
            }
        }
        public string SystemDataBaseSelected
        {
            get => ServerDatabase.System;
            set
            {
                ServerDatabase.System = value;
                if (ServerDatabase.System != null)
                { ServerIsEnabled = true; }
            }
        }
        public string Server
        {
            get => ServerDatabase.ServerDatabase;
            set => ServerDatabase.ServerDatabase = value;
        }
        public string EntityDataBaseSelected { get; set; }
        public List<string> EntitiesDataBase { get; set; }
        public List<string> Systems { get => new List<string>() { "Microsoft SQL Server" }; }
        public string ServerButtonChar { get => Connected == true ? "X" : "⮧"; }
        public string Path { get; set; }
        #endregion

        #region Events
        public event MainWindowModelEventHandler FunctionsNameChanged;
        public event MainWindowModelEventHandler StatusCheckedFunctionsChanged;
        public event MainWindowModelEventHandler ProceduresNameChanged;
        public event MainWindowModelEventHandler StatusCheckedProceduresChanged;
        public event MainWindowModelEventHandler TablesNameChanged;
        public event MainWindowModelEventHandler StatusCheckedTablesChanged;
        public event MainWindowModelEventHandler TriggersNameChanged;
        public event MainWindowModelEventHandler StatusCheckedTriggersChanged;
        public event MainWindowModelEventHandler ViewsNameChanged;
        public event MainWindowModelEventHandler StatusCheckedViewsChanged;
        public event MainWindowModelEventHandler ClickMenuItem;
        #endregion

        #region Constructor
        public MainWindowModel()
        {
            TIFunctionsName = "Functions 0/0";
            IsCheckedFunctions = true;
            TIProceduresName = "Procedures 0/0";
            IsCheckedProcedures = true;
            TITablesName = "Tables 0/0";
            IsCheckedTables = true;
            TITriggersName = "Triggers 0/0";
            IsCheckedTriggers = true;
            TIViewsName = "Views 0/0";
            IsCheckedViews = true;

            DatabaseFunctions = new ObservableCollection<CheckBoxItem>();
            DatabaseProcedures = new ObservableCollection<CheckBoxItem>();
            DatabaseTables = new ObservableCollection<CheckBoxItem>();
            DatabaseTriggers = new ObservableCollection<CheckBoxItem>();
            DatabaseViews = new ObservableCollection<CheckBoxItem>();

            EntitiesDataBase = new List<string>();
            ServerDatabase = new ServerManager();

            ServerIsEnabled = false;
            DatabaseIsEnabled = false;
            PathIsEnabled = false;
            Connected = false;

            LoadMenuItem();

            LoadSession($@"{ApplicationSession.PathHistoryApplication}\lastsesion.{ApplicationSession.Extension}");
        }
        #endregion

        #region Prvate Methods
        private void DeleteHistory()
        {
            var listDirectories = FileManager.GetDirectories(ApplicationSession.PathHistoryApplication);
            foreach (var item in listDirectories)
            { FileManager.DeleteDirectory(@$"{ApplicationSession.PathHistoryApplication}\{item}"); }

            LoadMenuItem();
        }

        private void LoadMenuItem()
        {
            ObservableCollection<object> historyItems = new ObservableCollection<object>();
            historyItems.Add(new MenuItemViewModel(DeleteHistory, MenuItemClickInvoke) { Header = "Clear history" });
            historyItems.Add(new Separator());

            var listDirectories = FileManager.GetDirectories(ApplicationSession.PathHistoryApplication);
            foreach (var item in listDirectories)
            {
                string pathFileSession = @$"{ApplicationSession.PathHistoryApplication}\{item}\{item}.{ApplicationSession.Extension}";
                historyItems.Add(new MenuItemViewModel(LoadSession, MenuItemClickInvoke, pathFileSession) { Header = item });
            }

            if (listDirectories?.Count == 0)
            { historyItems.Add(new MenuItemViewModel((string s1) => { }, () => { }, String.Empty) { Header = "History empty" }); }

            MenuItems = new ObservableCollection<object>
            {
                new MenuItemViewModel((string s1) => { }, () => { }, String.Empty) { Header = "File" ,
                    MenuItems = new ObservableCollection<object>
                    {
                        new MenuItemViewModel(SaveScripts, MenuItemClickInvoke) { Header = "Export SQL Script Database" },
                        new MenuItemViewModel(SaveScriptsServer, MenuItemClickInvoke) { Header = "Export SQL Script Server" }
                    }
                },
                new MenuItemViewModel((string s1) => { }, () => { }, String.Empty) { Header = "History",
                    MenuItems = historyItems
                },
            };
        }

        private void SaveScript(DatabaseManager.TypeScript type, DatabaseManager.OpcionExport opcionExport, string databaseName, List<string> downloadList = null)
        {
            string path = Path;

            FileManager.CreateDirectory(path, "Functions");
            FileManager.CreateDirectory(path, "Triggers");
            FileManager.CreateDirectory(path, "Procedures");
            FileManager.CreateDirectory(path, "Views");
            FileManager.CreateDirectory(path, "Tables");

            DatabaseManager db = ServerDatabase.Databases.Where(x => x.DatabaseEntity == databaseName).FirstOrDefault();
            if (downloadList?.Count > 0 || opcionExport == DatabaseManager.OpcionExport.ALL)
            {
                List<DatabaseItem> list = db.GetItemsContents(type, opcionExport, downloadList);

                if (opcionExport == DatabaseManager.OpcionExport.ALL)
                {
                    FileManager.CreateDirectory(Path, db.DatabaseEntity);
                    path += $@"\{db.DatabaseEntity}";
                }

                foreach (var item in list)
                { FileManager.SaveFileSQL(path, item.Name, item.Contents, type); }
            }
        }

        private ObservableCollection<CheckBoxItem> ConvertToListCheckBoxItem(List<string> list, CheckBoxItemEventHandler myMethodName)
        {
            ObservableCollection<CheckBoxItem> listCheckBoxItem = new ObservableCollection<CheckBoxItem>();

            foreach (var item in list)
            {
                CheckBoxItem checkBoxItem = new CheckBoxItem { IsSelected = true, Name = item };
                checkBoxItem.StatusChanged += myMethodName;
                listCheckBoxItem.Add(checkBoxItem);
            }

            return listCheckBoxItem;
        }

        private void LoadSession(string path)
        {
            if (Connected)
            { LoadDatabases(); }

            ApplicationSession applicationSession = new ApplicationSession();

            applicationSession = FileManager.ReadFromBinaryFile<ApplicationSession>(path);

            SystemDataBaseSelected = applicationSession.System;
            Server = applicationSession.Server;
            LoadDatabases();
            EntityDataBaseSelected = applicationSession.Database;
            LoadDatabaseItems();
            Path = applicationSession.Path;
        }

        private void MenuItemClickInvoke()
        {
            ClickMenuItem?.Invoke();
        }

        #endregion

        #region Public Methods
        public void ChangeSelectionAll(ObservableCollection<CheckBoxItem> listItems, bool isChecked)
        {
            foreach (var item in listItems)
            { item.IsSelected = isChecked; }
        }

        public void UpdateTabNameFunctions()
        {
            TIFunctionsName = $"Functions {DatabaseFunctions.Where(x => x.IsSelected).Count()}/{DatabaseFunctions.Count}";
            IsCheckedFunctions = DatabaseFunctions.Where(x => x.IsSelected).Count() == DatabaseFunctions.Count;
        }

        public void UpdateTabNameProcedures()
        {
            TIProceduresName = $"Procedures {DatabaseProcedures.Where(x => x.IsSelected).Count()}/{DatabaseProcedures.Count}";
            IsCheckedProcedures = DatabaseProcedures.Where(x => x.IsSelected).Count() == DatabaseProcedures.Count;
        }

        public void UpdateTabNameTables()
        {
            TITablesName = $"Tables {DatabaseTables.Where(x => x.IsSelected).Count()}/{DatabaseTables.Count}";
            IsCheckedTables = DatabaseTables.Where(x => x.IsSelected).Count() == DatabaseTables.Count;
        }

        public void UpdateTabNameTriggers()
        {
            TITriggersName = $"Triggers {DatabaseTriggers.Where(x => x.IsSelected).Count()}/{DatabaseTriggers.Count}";
            IsCheckedTriggers = DatabaseTriggers.Where(x => x.IsSelected).Count() == DatabaseTriggers.Count;
        }

        public void UpdateTabNameViews()
        {
            TIViewsName = $"Views {DatabaseViews.Where(x => x.IsSelected).Count()}/{DatabaseViews.Count}";
            IsCheckedViews = DatabaseViews.Where(x => x.IsSelected).Count() == DatabaseViews.Count;
        }

        public void LoadDatabases()
        {
            if (!Connected)
            {
                if (ServerDatabase.LoadDatabases())
                {
                    EntitiesDataBase = ServerDatabase.Databases.Select(x => x.DatabaseEntity).ToList();
                    DatabaseIsEnabled = true;
                    PathIsEnabled = true;
                    Connected = true;
                }
            }
            else
            {
                EntitiesDataBase.Clear();
                EntityDataBaseSelected = null;
                DatabaseIsEnabled = false;
                PathIsEnabled = false;
                Connected = false;
                Server = "";
                Path = "";

                DatabaseFunctions.Clear();
                DatabaseProcedures.Clear();
                DatabaseTables.Clear();
                DatabaseTriggers.Clear();
                DatabaseViews.Clear();
            }
        }

        public void LoadDatabaseItems()
        {
            DatabaseManager db = ServerDatabase.Databases.Where(x => x.DatabaseEntity == EntityDataBaseSelected).FirstOrDefault();

            if (db != null && db.LoadItems())
            {
                DatabaseFunctions = ConvertToListCheckBoxItem(db.DatabaseFunctions, UpdateTabNameFunctions);
                DatabaseProcedures = ConvertToListCheckBoxItem(db.DatabaseProcedures, UpdateTabNameProcedures);
                DatabaseTables = ConvertToListCheckBoxItem(db.DatabaseTables, UpdateTabNameTables);
                DatabaseTriggers = ConvertToListCheckBoxItem(db.DatabaseTriggers, UpdateTabNameTriggers);
                DatabaseViews = ConvertToListCheckBoxItem(db.DatabaseViews, UpdateTabNameViews);
            }

            UpdateTabNameFunctions();
            UpdateTabNameProcedures();
            UpdateTabNameTables();
            UpdateTabNameTriggers();
            UpdateTabNameViews();
        }

        public void LoadPath()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                { Path = dialog.SelectedPath; }
            }
        }

        public void SaveScripts()
        {
            SaveScript(DatabaseManager.TypeScript.FN, DatabaseManager.OpcionExport.CHECKED, EntityDataBaseSelected, DatabaseFunctions.Where(x => x.IsSelected).Select(x => x.Name).ToList());
            SaveScript(DatabaseManager.TypeScript.P, DatabaseManager.OpcionExport.CHECKED, EntityDataBaseSelected, DatabaseProcedures.Where(x => x.IsSelected).Select(x => x.Name).ToList());
            SaveScript(DatabaseManager.TypeScript.U, DatabaseManager.OpcionExport.CHECKED, EntityDataBaseSelected, DatabaseTables.Where(x => x.IsSelected).Select(x => x.Name).ToList());
            SaveScript(DatabaseManager.TypeScript.TR, DatabaseManager.OpcionExport.CHECKED, EntityDataBaseSelected, DatabaseTriggers.Where(x => x.IsSelected).Select(x => x.Name).ToList());
            SaveScript(DatabaseManager.TypeScript.V, DatabaseManager.OpcionExport.CHECKED, EntityDataBaseSelected, DatabaseViews.Where(x => x.IsSelected).Select(x => x.Name).ToList());

            SaveCurentSesion();
            LoadMenuItem();
        }

        public void SaveScriptsServer()
        {
            foreach (var item in ServerDatabase.Databases.Select(x => x.DatabaseEntity))
            {
                SaveScript(DatabaseManager.TypeScript.FN, DatabaseManager.OpcionExport.ALL, item);
                SaveScript(DatabaseManager.TypeScript.P, DatabaseManager.OpcionExport.ALL, item);
                SaveScript(DatabaseManager.TypeScript.U, DatabaseManager.OpcionExport.ALL, item);
                SaveScript(DatabaseManager.TypeScript.TR, DatabaseManager.OpcionExport.ALL, item);
                SaveScript(DatabaseManager.TypeScript.V, DatabaseManager.OpcionExport.ALL, item);
            }
        }

        public void SaveLastSesion()
        {
            ApplicationSession applicationSession = new ApplicationSession() { System = this.SystemDataBaseSelected, Server = this.Server, Database = this.EntityDataBaseSelected, Path = this.Path };

            FileManager.CreateDirectory(ApplicationSession.PathApplication, ApplicationSession.FolderApplication);
            FileManager.CreateDirectory(ApplicationSession.PathApplication, ApplicationSession.FolderHistoryApplication);
            string path = @$"{ApplicationSession.PathHistoryApplication}\lastsesion.{ApplicationSession.Extension}";

            FileManager.WriteToBinaryFile<ApplicationSession>(path, applicationSession);
        }

        public void SaveCurentSesion()
        {
            string path = ApplicationSession.PathHistoryApplication;
            FileManager.CreateDirectory(path, @$"\{Server}.{EntityDataBaseSelected}");

            ApplicationSession applicationSession = new ApplicationSession() { System = this.SystemDataBaseSelected, Server = this.Server, Database = this.EntityDataBaseSelected, Path = this.Path };

            path += @$"\{Server}.{EntityDataBaseSelected}\{Server}.{EntityDataBaseSelected}.{ApplicationSession.Extension}";

            FileManager.WriteToBinaryFile<ApplicationSession>(path, applicationSession);
        }

        #endregion
    }
}
