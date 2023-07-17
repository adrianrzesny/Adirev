using Microsoft.Win32;
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
using Adirev.View;

namespace Adirev.Models
{
    public delegate void MainWindowModelEventHandler();
    public class MainWindowModel
    {
        #region Enum
        public enum ModeWindow
        {
            ExportDatabase,
            ExportServer
        }
        #endregion

        #region Variables
        private string functionsTextSearchFields;
        private string proceduresTextSearchFields;
        private string tablesTextSearchFields;
        private string triggersTextSearchFields;
        private string viewsTextSearchFields;
        private string databasesTextSearchFields;
        private string tiFunctionsName;
        private string tiProceduresName;
        private string tiTablesName;
        private string tiTriggersName;
        private string tiViewsName;
        private string tiDatabasesName;
        private bool isCheckedFunctions;
        private bool isCheckedProcedures;
        private bool isCheckedTables;
        private bool isCheckedTriggers;
        private bool isCheckedViews;
        private bool isCheckedDatabases;
        private List<string> entitiesDataBase;
        private string entityDataBaseSelected;
        private System.Windows.Visibility progressBarVisibility;
        private System.Windows.Visibility itemsDataBaseVisibility;
        private System.Windows.Visibility entitiesDataBaseVisibility;
        private System.Windows.Visibility tabEntitiesDataBaseVisibility;
        #endregion

        #region Objects
        private ServerManager ServerDatabase { get; set; }
        #endregion

        #region Properties
        public Logger LoggerApplication { get; set; } = Logger.Instance;
        public ApplicationSettings Settings { get; set; } = ApplicationSettings.Instance;
        public ModeWindow ModeMainWindow { get; set; }
        public ObservableCollection<object> MenuItems { get; set; }
        public bool Connected { get; set; }
        public bool IsEnabledWindow { get; set; }
        public bool ServerIsEnabled { get; set; }
        public bool DatabaseIsEnabled { get; set; }
        public bool PathIsEnabled { get; set; }
        public ObservableCollection<CheckBoxItem> DatabaseFunctions { get; set; }
        public ObservableCollection<CheckBoxItem> DatabaseProcedures { get; set; }
        public ObservableCollection<CheckBoxItem> DatabaseTables { get; set; }
        public ObservableCollection<CheckBoxItem> DatabaseTriggers { get; set; }
        public ObservableCollection<CheckBoxItem> DatabaseViews { get; set; }
        public ObservableCollection<CheckBoxItem> DatabasesEntities { get; set; }
        public List<string> Systems { get => new List<string>() { "Microsoft SQL Server" }; }
        public string ServerButtonChar { get => Connected == true ? "X" : "⮧"; }
        public string Path { get; set; }
        public List<string> EntitiesDataBase
        {
            get => entitiesDataBase;
            set
            {
                entitiesDataBase = value;
                EntitiesDataBaseChanged?.Invoke();
            }
        }
        public string EntityDataBaseSelected
        {
            get => entityDataBaseSelected;
            set
            {
                entityDataBaseSelected = value;
                EntityDataBaseSelectedChanged?.Invoke();
            }
        }
        public System.Windows.Visibility ProgressBarVisibility
        {
            get => progressBarVisibility;
            set
            {
                progressBarVisibility = value;
                IsEnabledWindow = progressBarVisibility != Visibility.Visible;
                ProgressBarVisibilityChanged?.Invoke();
            }
        }
        public System.Windows.Visibility ItemsDataBaseVisibility
        {
            get => itemsDataBaseVisibility;
            set => itemsDataBaseVisibility = value;
        }
        public System.Windows.Visibility EntitiesDataBaseVisibility
        {
            get => entitiesDataBaseVisibility;
            set => entitiesDataBaseVisibility = value;
        }
        public System.Windows.Visibility TabEntitiesDataBaseVisibility
        {
            get => tabEntitiesDataBaseVisibility;
            set => tabEntitiesDataBaseVisibility = value;
        }
        public string TextLog
        {
            get => LoggerApplication.LogOperacion;
            set => LoggerApplication.AddLog(value);
        }
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
        public string TIDatabasesName
        {
            get => tiDatabasesName;
            set
            {
                tiDatabasesName = value;
                DatabasesNameChanged?.Invoke();
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
        public bool TIFunctionsIsSelected { get; set; }
        public bool TIDatabasesIsSelected { get; set; }
        public bool IsCheckedDatabases
        {
            get => isCheckedDatabases;
            set
            {
                isCheckedDatabases = value;
                StatusCheckedDatabasesChanged?.Invoke();
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
        public string FunctionsTextSearchFields
        {
            get => functionsTextSearchFields;
            set
            {
                functionsTextSearchFields = value;
                DatabaseFunctions = SearchItems(DatabaseFunctions, value);
            }
        }
        public string ProceduresTextSearchFields
        {
            get => proceduresTextSearchFields;
            set
            {
                proceduresTextSearchFields = value;
                DatabaseProcedures = SearchItems(DatabaseProcedures, value);
            }
        }
        public string TriggersTextSearchFields
        {
            get => triggersTextSearchFields;
            set
            {
                triggersTextSearchFields = value;
                DatabaseTriggers = SearchItems(DatabaseTriggers, value);
            }
        }
        public string TablesTextSearchFields
        {
            get => tablesTextSearchFields;
            set
            {
                tablesTextSearchFields = value;
                DatabaseTables = SearchItems(DatabaseTables, value);
            }
        }
        public string ViewsTextSearchFields
        {
            get => viewsTextSearchFields;
            set
            {
                viewsTextSearchFields = value;
                DatabaseViews = SearchItems(DatabaseViews, value);
            }
        }
        public string DatabasesTextSearchFields
        {
            get => databasesTextSearchFields;
            set
            {
                databasesTextSearchFields = value;
                DatabasesEntities = SearchItems(DatabasesEntities, value);
            }
        }
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
        public event MainWindowModelEventHandler DatabasesNameChanged;
        public event MainWindowModelEventHandler StatusCheckedViewsChanged;
        public event MainWindowModelEventHandler StatusCheckedDatabasesChanged;
        public event MainWindowModelEventHandler ClickMenuItem;
        public event MainWindowModelEventHandler ProgressBarVisibilityChanged;
        public event MainWindowModelEventHandler EntitiesDataBaseChanged;
        public event MainWindowModelEventHandler EntityDataBaseSelectedChanged;
        #endregion

        #region Constructor
        public MainWindowModel()
        {
            ModeMainWindow = ModeWindow.ExportDatabase;
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
            TIDatabasesName = "Databases 0/0";
            IsCheckedDatabases = true;

            DatabaseFunctions = new ObservableCollection<CheckBoxItem>();
            DatabaseProcedures = new ObservableCollection<CheckBoxItem>();
            DatabaseTables = new ObservableCollection<CheckBoxItem>();
            DatabaseTriggers = new ObservableCollection<CheckBoxItem>();
            DatabaseViews = new ObservableCollection<CheckBoxItem>();
            DatabasesEntities = new ObservableCollection<CheckBoxItem>();

            EntitiesDataBase = new List<string>();
            ServerDatabase = new ServerManager();

            IsEnabledWindow = true;
            ServerIsEnabled = false;
            DatabaseIsEnabled = false;
            PathIsEnabled = false;
            Connected = false;
            TIFunctionsIsSelected = true;
            TIDatabasesIsSelected = false;

            ProgressBarVisibility = System.Windows.Visibility.Hidden;
            ItemsDataBaseVisibility = System.Windows.Visibility.Visible;
            EntitiesDataBaseVisibility = System.Windows.Visibility.Visible;
            TabEntitiesDataBaseVisibility = System.Windows.Visibility.Collapsed;

            LoadMenuItem();

            LoadSession($@"{ApplicationSession.PathHistoryApplication}\lastsesion.{ApplicationSession.Extension}");

            Settings.FirstRun = false;
        }
        #endregion

        #region Prvate Methods
        private void DeleteHistory()
        {
            var listDirectories = FileManager.GetDirectories(ApplicationSession.PathHistoryApplication);
            foreach (var item in listDirectories)
            { FileManager.DeleteDirectory(@$"{ApplicationSession.PathHistoryApplication}\{FileManager.DeleteInvalidFileNameChars(item)}"); }

            LoadMenuItem();
        }

        private void LoadMenuItem()
        {
            ObservableCollection<object> historyItems = new ObservableCollection<object>();

            #region Menu item history
            var listDirectories = FileManager.GetDirectories(ApplicationSession.PathHistoryApplication);
            foreach (var item in listDirectories)
            {
                string pathFileSession = @$"{ApplicationSession.PathHistoryApplication}\{FileManager.DeleteInvalidFileNameChars(item)}\{FileManager.DeleteInvalidFileNameChars(item)}.{ApplicationSession.Extension}";
                historyItems.Add(new MenuItemViewModel(LoadSession, MenuItemClickInvoke, pathFileSession) { Header = item });
            }

            if (listDirectories?.Count == 0)
            { historyItems.Add(new MenuItemViewModel((string s1) => { }, () => { }, String.Empty) { Header = "History empty" }); }
            #endregion

            #region Menu item separator
            try
            { historyItems.Add(new Separator()); }
            catch (Exception ex)
            { }
            #endregion

            #region Menu item delete history
            try
            {
                historyItems.Add(new MenuItemViewModel(DeleteHistory, MenuItemClickInvoke) { Header = "Clear history" });
            }
            catch (Exception ex)
            { }
            #endregion

            #region Menu item 
            try
            {
                MenuItems = new ObservableCollection<object>
                {
                    new MenuItemViewModel((string s1) => { }, () => { }, String.Empty) { Header = "Mode" ,
                        MenuItems = new ObservableCollection<object>
                        {
                            new MenuItemViewModel(SetModeWindowDatabase, MenuItemClickInvoke) { Header = "Export SQL Script Database" },
                            new MenuItemViewModel(SetModeWindowServer, MenuItemClickInvoke) { Header = "Export SQL Script Server" }
                        }
                    },
                    new MenuItemViewModel((string s1) => { }, () => { }, String.Empty) { Header = "History",
                        MenuItems = historyItems
                    },
                    new MenuItemViewModel((string s1) => { }, () => { }, String.Empty) { Header = "Logs" ,
                        MenuItems = new ObservableCollection<object>
                        {
                            new MenuItemViewModel(OpenLogWindow, MenuItemClickInvoke) { Header = "Show logs" },
                            new MenuItemViewModel(ClearLogs, MenuItemClickInvoke) { Header = "Clear logs" }
                        }
                    }
                };
            }
            catch (Exception ex)
            { }
            #endregion
        }

        private void SetModeWindowDatabase()
        {
            ModeMainWindow = ModeWindow.ExportDatabase;

            ItemsDataBaseVisibility = System.Windows.Visibility.Visible;
            EntitiesDataBaseVisibility = System.Windows.Visibility.Visible;
            TabEntitiesDataBaseVisibility = System.Windows.Visibility.Collapsed;
            TIFunctionsIsSelected = true;
            TIDatabasesIsSelected = false;
        }

        private void SetModeWindowServer()
        {
            ModeMainWindow = ModeWindow.ExportServer;

            ItemsDataBaseVisibility = System.Windows.Visibility.Collapsed;
            EntitiesDataBaseVisibility = System.Windows.Visibility.Collapsed;
            TabEntitiesDataBaseVisibility = System.Windows.Visibility.Visible;
            TIFunctionsIsSelected = false;
            TIDatabasesIsSelected = true;
        }

        private void SaveScript(DatabaseManager.TypeScript type, DatabaseManager.OpcionExport opcionExport, string databaseName, List<string> listToDownload = null)
        {
            string path = Path;

            string ItemType = DatabaseManager.GetNameTypeScript(type);
            LoggerApplication.AddLog($"Export ( {Server}.{databaseName}->{ItemType} )", true);

            DatabaseManager db = ServerDatabase.Databases.Where(x => x.DatabaseEntity == databaseName).FirstOrDefault();
            if (listToDownload?.Count > 0 || opcionExport == DatabaseManager.OpcionExport.ALL)
            {
                List<DatabaseItem> list = db.GetItemsContents(type, opcionExport, listToDownload);

                if (opcionExport == DatabaseManager.OpcionExport.ALL)
                {
                    path += $@"\{FileManager.DeleteInvalidFileNameChars(db.DatabaseEntity)}";
                    FileManager.CreateDirectory(Path, FileManager.DeleteInvalidFileNameChars(db.DatabaseEntity));
                    FileManager.CreateDirectory(path, DatabaseManager.GetNameTypeScript(type));
                }
                else
                {
                    FileManager.CreateDirectory(path, DatabaseManager.GetNameTypeScript(type));
                }

                foreach (var item in list)
                { FileManager.SaveFileSQL(path, item.Name, item.Contents, type); }
            }
        }

        private ObservableCollection<CheckBoxItem> ConvertToListCheckBoxItem(List<string> list, CheckBoxItemEventHandler myMethodName)
        {
            ObservableCollection<CheckBoxItem> listCheckBoxItem = new ObservableCollection<CheckBoxItem>();

            if (list != null)
            {
                foreach (var item in list)
                {
                    CheckBoxItem checkBoxItem = new CheckBoxItem { IsSelected = true, Name = item };
                    checkBoxItem.StatusChanged += myMethodName;
                    listCheckBoxItem.Add(checkBoxItem);
                }
            }

            return listCheckBoxItem;
        }

        private void LoadSession(string path)
        {
            SetModeWindowDatabase();

            if (Connected)
            { LoadDatabases(); }

            ApplicationSession applicationSession = new ApplicationSession();

            applicationSession = FileManager.ReadFromBinaryFile<ApplicationSession>(path);

            ServerDatabase.Login = applicationSession.GetLogin();
            ServerDatabase.Password = applicationSession.GetPassword();
            SystemDataBaseSelected = applicationSession.System;
            Server = applicationSession.Server;

            if (ServerManager.Ping(Server))
            {
                LoadDatabases();
                EntityDataBaseSelected = applicationSession.Database;
                LoadDatabaseItems();
            }

            Path = applicationSession.Path;
        }

        private void MenuItemClickInvoke()
        {
            ClickMenuItem?.Invoke();
        }

        private void SaveScriptsDatabase()
        {
            SetVisibleProgressBar(true);

            SaveScript(DatabaseManager.TypeScript.FN, DatabaseManager.OpcionExport.CHECKED, EntityDataBaseSelected, DatabaseFunctions.Where(x => x.IsSelected).Select(x => x.Name).ToList());
            SaveScript(DatabaseManager.TypeScript.P, DatabaseManager.OpcionExport.CHECKED, EntityDataBaseSelected, DatabaseProcedures.Where(x => x.IsSelected).Select(x => x.Name).ToList());
            SaveScript(DatabaseManager.TypeScript.U, DatabaseManager.OpcionExport.CHECKED, EntityDataBaseSelected, DatabaseTables.Where(x => x.IsSelected).Select(x => x.Name).ToList());
            SaveScript(DatabaseManager.TypeScript.TR, DatabaseManager.OpcionExport.CHECKED, EntityDataBaseSelected, DatabaseTriggers.Where(x => x.IsSelected).Select(x => x.Name).ToList());
            SaveScript(DatabaseManager.TypeScript.V, DatabaseManager.OpcionExport.CHECKED, EntityDataBaseSelected, DatabaseViews.Where(x => x.IsSelected).Select(x => x.Name).ToList());

            SetVisibleProgressBar(false);
            LoggerApplication.AddLog($"Export completed ( {Server}.{EntityDataBaseSelected} -> {Path} )", true);

            SaveCurentSesion();
            LoadMenuItem();
        }

        private void SaveScriptsServer()
        {
            SetVisibleProgressBar(true);

            foreach (var item in DatabasesEntities.Where(x => x.IsSelected == true).Select(x => x.Name))
            {
                SaveScript(DatabaseManager.TypeScript.FN, DatabaseManager.OpcionExport.ALL, item);
                SaveScript(DatabaseManager.TypeScript.P, DatabaseManager.OpcionExport.ALL, item);
                SaveScript(DatabaseManager.TypeScript.U, DatabaseManager.OpcionExport.ALL, item);
                SaveScript(DatabaseManager.TypeScript.TR, DatabaseManager.OpcionExport.ALL, item);
                SaveScript(DatabaseManager.TypeScript.V, DatabaseManager.OpcionExport.ALL, item);
            }

            SetVisibleProgressBar(false);
            LoggerApplication.AddLog($"Export completed ( {Server} -> {Path} )", true);
        }

        private void SaveCurentSesion()
        {
            string path = ApplicationSession.PathHistoryApplication;
            FileManager.CreateDirectory(path, @$"{FileManager.DeleteInvalidFileNameChars(Server)}.{FileManager.DeleteInvalidFileNameChars(EntityDataBaseSelected)}");

            ApplicationSession applicationSession = new ApplicationSession() { System = this.SystemDataBaseSelected, Server = this.Server, Database = this.EntityDataBaseSelected, Path = this.Path };
            applicationSession.SetLogin(ServerDatabase.Databases.Where(x => x.DatabaseEntity == EntityDataBaseSelected).FirstOrDefault()?.Login);
            applicationSession.SetPassword(ServerDatabase.Databases.Where(x => x.DatabaseEntity == EntityDataBaseSelected).FirstOrDefault()?.Password);

            path += @$"\{FileManager.DeleteInvalidFileNameChars(Server)}.{FileManager.DeleteInvalidFileNameChars(EntityDataBaseSelected)}\{FileManager.DeleteInvalidFileNameChars(Server)}.{FileManager.DeleteInvalidFileNameChars(EntityDataBaseSelected)}.{ApplicationSession.Extension}";

            FileManager.WriteToBinaryFile<ApplicationSession>(path, applicationSession);
        }

        private void UpdateTabNameFunctions()
        {
            TIFunctionsName = $"Functions {DatabaseFunctions.Where(x => x.IsSelected).Count()}/{DatabaseFunctions.Count}";
            IsCheckedFunctions = DatabaseFunctions.Where(x => x.IsSelected).Count() == DatabaseFunctions.Count;
        }

        private void UpdateTabNameProcedures()
        {
            TIProceduresName = $"Procedures {DatabaseProcedures.Where(x => x.IsSelected).Count()}/{DatabaseProcedures.Count}";
            IsCheckedProcedures = DatabaseProcedures.Where(x => x.IsSelected).Count() == DatabaseProcedures.Count;
        }

        private void UpdateTabNameTables()
        {
            TITablesName = $"Tables {DatabaseTables.Where(x => x.IsSelected).Count()}/{DatabaseTables.Count}";
            IsCheckedTables = DatabaseTables.Where(x => x.IsSelected).Count() == DatabaseTables.Count;
        }

        private void UpdateTabNameTriggers()
        {
            TITriggersName = $"Triggers {DatabaseTriggers.Where(x => x.IsSelected).Count()}/{DatabaseTriggers.Count}";
            IsCheckedTriggers = DatabaseTriggers.Where(x => x.IsSelected).Count() == DatabaseTriggers.Count;
        }

        private void UpdateTabNameViews()
        {
            TIViewsName = $"Views {DatabaseViews.Where(x => x.IsSelected).Count()}/{DatabaseViews.Count}";
            IsCheckedViews = DatabaseViews.Where(x => x.IsSelected).Count() == DatabaseViews.Count;
        }
        private void UpdateTabNameDatabases()
        {
            TIDatabasesName = $"Databases {DatabasesEntities.Where(x => x.IsSelected).Count()}/{DatabasesEntities.Count}";
            IsCheckedDatabases = DatabasesEntities.Where(x => x.IsSelected).Count() == DatabasesEntities.Count;
        }
        private void ClearLogs()
        {
            LoggerApplication.ClearLogs();
        }

        #endregion

        #region Public Methods
        public void ChangeSelectionAll(ObservableCollection<CheckBoxItem> listItems, bool isChecked)
        {
            foreach (var item in listItems.Where(x => x.Visibility == Visibility.Visible))
            { item.IsSelected = isChecked; }
        }

        public void SetVisibleProgressBar(bool visible)
        {
            ProgressBarVisibility = visible == true ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        public void LoadDatabases()
        {
            if (!Connected)
            {
                if (ServerDatabase.LoadDatabases())
                {
                    EntitiesDataBase = ServerDatabase.Databases.Select(x => x.DatabaseEntity).ToList();
                    DatabasesEntities = ConvertToListCheckBoxItem(ServerDatabase.Databases.Select(x => x.DatabaseEntity).ToList(), UpdateTabNameDatabases);
                    DatabaseIsEnabled = true;
                    PathIsEnabled = true;
                    Connected = true;
                }
            }
            else
            {
                LoggerApplication.AddLog($"Server disconnected -> {Server}", true);

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
                DatabasesEntities.Clear();

                ServerDatabase.Login = String.Empty;
                ServerDatabase.Password = String.Empty;

            }

            UpdateTabNameDatabases();
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
                DatabasesEntities = ConvertToListCheckBoxItem(ServerDatabase.Databases.Select(x => x.DatabaseEntity).ToList(), UpdateTabNameDatabases);
            }
            else
            {
                if (Settings.FirstRun)
                {
                    EntitiesDataBase.Clear();
                    EntityDataBaseSelected = null;
                    DatabaseIsEnabled = false;
                    PathIsEnabled = false;
                    Connected = false;
                    Path = "";

                    DatabaseFunctions.Clear();
                    DatabaseProcedures.Clear();
                    DatabaseTables.Clear();
                    DatabaseTriggers.Clear();
                    DatabaseViews.Clear();
                    DatabasesEntities.Clear();
                }
            }

            UpdateTabNameFunctions();
            UpdateTabNameProcedures();
            UpdateTabNameTables();
            UpdateTabNameTriggers();
            UpdateTabNameViews();
            UpdateTabNameDatabases();
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
            if (ModeMainWindow == ModeWindow.ExportDatabase)
            { SaveScriptsDatabase(); }
            else
            { SaveScriptsServer(); }
        }

        public void SaveLastSesion()
        {
            Settings.Closed = true;

            ApplicationSession applicationSession = new ApplicationSession() { System = this.SystemDataBaseSelected, Server = this.Server, Database = this.EntityDataBaseSelected, Path = this.Path };
            applicationSession.SetLogin(ServerDatabase.Databases.Where(x => x.DatabaseEntity == EntityDataBaseSelected).FirstOrDefault()?.Login);
            applicationSession.SetPassword(ServerDatabase.Databases.Where(x => x.DatabaseEntity == EntityDataBaseSelected).FirstOrDefault()?.Password);

            FileManager.CreateDirectory(ApplicationSession.PathApplication, ApplicationSession.FolderApplication);
            FileManager.CreateDirectory(ApplicationSession.PathApplication, ApplicationSession.FolderHistoryApplication);
            string path = @$"{ApplicationSession.PathHistoryApplication}\lastsesion.{ApplicationSession.Extension}";

            FileManager.WriteToBinaryFile<ApplicationSession>(path, applicationSession);
        }

        public void OpenLogWindow()
        {
            LogWindow lw = new LogWindow();

            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            lw.Left = mainWindow.Left + ((mainWindow.Width - lw.Width) / 2);
            lw.Top = mainWindow.Top + ((mainWindow.Height - lw.Height) / 2);

            lw.ShowDialog();
        }

        private ObservableCollection<CheckBoxItem> SearchItems(ObservableCollection<CheckBoxItem> items, string searchText)
        {
            foreach (var x in items)
            { x.Visibility = Visibility.Visible; }

            foreach (var x in items.Where(x => x.Name.ToLower().Contains(searchText.ToLower()) == false).ToList())
            { x.Visibility = Visibility.Hidden; }

            return new ObservableCollection<CheckBoxItem>(items.OrderBy(a => a.Visibility).ThenBy(x => x.Name));
        }

        #endregion
    }
}
