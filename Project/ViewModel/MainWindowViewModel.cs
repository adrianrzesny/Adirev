using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Adirev.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Adirev.Class;
    using Adirev.Model;
    using Adirev.Models;

    public class MainWindowViewModel : NotifyPropertyChanged
    {
        #region Objects 
        private MainWindowModel model = new MainWindowModel();
        #endregion

        #region Properties
        public System.Windows.Forms.NotifyIcon NotifyIcon
        {
            get => model.NotifyIcon;
            set => model.NotifyIcon = value;
        }
        public string ServerButtonChar { get => model.ServerButtonChar; }
        public string TextLog { get => model.TextLog; }
        public Logger LoggerApplication { get => model.LoggerApplication; }

        public bool IsEnabledWindow
        {
            get => model.IsEnabledWindow;
            set
            {
                model.IsEnabledWindow = value;
                OnPropertyChanged(nameof(IsEnabledWindow));
            }
        }
        public System.Windows.Visibility ProgressBarVisibility
        {
            get => model.ProgressBarVisibility;
            set
            {
                model.ProgressBarVisibility = value;
                OnPropertyChanged(nameof(ProgressBarVisibility));
            }
        }
        public ObservableCollection<object> MenuItems
        {
            get => model.MenuItems;
            set
            {
                model.MenuItems = value;
                OnPropertyChanged(nameof(MenuItems));
            }
        }
        public List<string> Systems { get => model.Systems; }
        public bool ServerIsEnabled
        {
            get => model.ServerIsEnabled;
            set
            {
                model.ServerIsEnabled = value;
                OnPropertyChanged(nameof(ServerIsEnabled));
            }
        }

        public bool DatabaseIsEnabled
        {
            get => model.DatabaseIsEnabled;
            set
            {
                model.DatabaseIsEnabled = value;
                OnPropertyChanged(nameof(DatabaseIsEnabled));
            }
        }

        public bool PathIsEnabled
        {
            get => model.PathIsEnabled;
            set
            {
                model.ServerIsEnabled = value;
                OnPropertyChanged(nameof(PathIsEnabled));
            }
        }

        public string SystemDataBaseSelected
        {
            get => model.SystemDataBaseSelected;
            set
            {
                model.SystemDataBaseSelected = value;
                OnPropertyChanged(nameof(SystemDataBaseSelected));
                OnPropertyChanged(nameof(ServerIsEnabled));
            }
        }

        public string EntityDataBaseSelected
        {
            get => model.EntityDataBaseSelected;
            set
            {
                model.EntityDataBaseSelected = value;
                OnPropertyChanged(nameof(EntityDataBaseSelected));
            }
        }

        public List<string> EntitiesDataBase
        {
            get => model.EntitiesDataBase;
            set
            {
                model.EntitiesDataBase = value;
                OnPropertyChanged(nameof(EntitiesDataBase));
            }
        }

        public string Path
        {
            get => model.Path;
            set
            {
                model.Path = value;
                OnPropertyChanged(nameof(Path));
            }
        }

        public string Server
        {
            get => model.Server;
            set
            {
                model.Server = value;
                OnPropertyChanged(nameof(Server));
            }
        }

        public ObservableCollection<CheckBoxItem> DatabaseFunctions
        {
            get => model.DatabaseFunctions;
            set
            {
                model.DatabaseFunctions = value;
                OnPropertyChanged(nameof(DatabaseFunctions));
            }
        }

        public ObservableCollection<CheckBoxItem> DatabaseProcedures
        {
            get => model.DatabaseProcedures;
            set
            {
                model.DatabaseFunctions = value;
                OnPropertyChanged(nameof(DatabaseProcedures));
            }
        }

        public ObservableCollection<CheckBoxItem> DatabaseTables
        {
            get => model.DatabaseTables;
            set
            {
                model.DatabaseFunctions = value;
                OnPropertyChanged(nameof(DatabaseTables));
            }
        }

        public ObservableCollection<CheckBoxItem> DatabaseTriggers
        {
            get => model.DatabaseTriggers;
            set
            {
                model.DatabaseFunctions = value;
                OnPropertyChanged(nameof(DatabaseTriggers));
            }
        }

        public ObservableCollection<CheckBoxItem> DatabaseViews
        {
            get => model.DatabaseViews;
            set
            {
                model.DatabaseFunctions = value;
                OnPropertyChanged(nameof(DatabaseViews));
            }
        }
        
        public ObservableCollection<CheckBoxItem> DatabasesEntities
        {
            get => model.DatabasesEntities;
            set
            {
                model.DatabasesEntities = value;
                OnPropertyChanged(nameof(DatabasesEntities));
            }
        }

        public string TIFunctionsName
        {
            get => model.TIFunctionsName;
            set
            {
                model.TIFunctionsName = value;
                OnPropertyChanged(nameof(TIFunctionsName));
            }
        }
        public bool IsCheckedFunctions
        {
            get => model.IsCheckedFunctions;
            set
            {
                model.IsCheckedFunctions = value;
                OnPropertyChanged(nameof(IsCheckedFunctions));
            }
        }
        public string TIProceduresName
        {
            get => model.TIProceduresName;
            set
            {
                model.TIProceduresName = value;
                OnPropertyChanged(nameof(TIProceduresName));
            }
        }
        public bool IsCheckedProcedures
        {
            get => model.IsCheckedProcedures;
            set
            {
                model.IsCheckedProcedures = value;
                OnPropertyChanged(nameof(IsCheckedProcedures));
            }
        }
        public string TITablesName
        {
            get => model.TITablesName;
            set
            {
                model.TITablesName = value;
                OnPropertyChanged(nameof(TITablesName));
            }
        }
        public bool IsCheckedTables
        {
            get => model.IsCheckedTables;
            set
            {
                model.IsCheckedTables = value;
                OnPropertyChanged(nameof(IsCheckedTables));
            }
        }
        public string TITriggersName
        {
            get => model.TITriggersName;
            set
            {
                model.TITriggersName = value;
                OnPropertyChanged(nameof(TITriggersName));
            }
        }
        public bool IsCheckedTriggers
        {
            get => model.IsCheckedTriggers;
            set
            {
                model.IsCheckedTriggers = value;
                OnPropertyChanged(nameof(IsCheckedTriggers));
            }
        }
        public string TIViewsName
        {
            get => model.TIViewsName;
            set
            {
                model.TIViewsName = value;
                OnPropertyChanged(nameof(TIViewsName));
            }
        }
        public string TIDatabasesName
        {
            get => model.TIDatabasesName;
            set
            {
                model.TIViewsName = value;
                OnPropertyChanged(nameof(TIDatabasesName));
            }
        }
        public bool TIFunctionsIsSelected
        {
            get => model.TIFunctionsIsSelected;
            set
            {
                model.TIFunctionsIsSelected = value;
                OnPropertyChanged(nameof(TIFunctionsIsSelected));
            }
        }
        public bool TIDatabasesIsSelected
        {
            get => model.TIDatabasesIsSelected;
            set
            {
                model.TIDatabasesIsSelected = value;
                OnPropertyChanged(nameof(TIDatabasesIsSelected));
            }
        }
        public bool IsCheckedViews
        {
            get => model.IsCheckedViews;
            set
            {
                model.IsCheckedViews = value;
                OnPropertyChanged(nameof(IsCheckedViews));
            }
        }
        public bool IsCheckedDatabases
        {
            get => model.IsCheckedDatabases;
            set
            {
                model.IsCheckedDatabases = value;
                OnPropertyChanged(nameof(IsCheckedDatabases));
            }
        }
        public System.Windows.Visibility ItemsDataBaseVisibility
        {
            get => model.ItemsDataBaseVisibility;
            set => model.ItemsDataBaseVisibility = value;
        }
        public System.Windows.Visibility TabEntitiesDataBaseVisibility
        {
            get => model.TabEntitiesDataBaseVisibility;
            set => model.TabEntitiesDataBaseVisibility = value;
        }
        public System.Windows.Visibility EntitiesDataBaseVisibility
        {
            get => model.EntitiesDataBaseVisibility;
            set => model.EntitiesDataBaseVisibility = value;
        }
        public string FunctionsTextSearchFields
        {
            get => model.FunctionsTextSearchFields;
            set 
            {
                model.FunctionsTextSearchFields = value;
                OnPropertyChanged(nameof(FunctionsTextSearchFields));
                OnPropertyChanged(nameof(DatabaseFunctions));
            }
        }
        public string ProceduresTextSearchFields
        {
            get => model.ProceduresTextSearchFields;
            set 
            {
                model.ProceduresTextSearchFields = value;
                OnPropertyChanged(nameof(ProceduresTextSearchFields));
                OnPropertyChanged(nameof(DatabaseProcedures));
            }
        }
        public string TablesTextSearchFields
        {
            get => model.TablesTextSearchFields;
            set 
            {
                model.TablesTextSearchFields = value;
                OnPropertyChanged(nameof(TablesTextSearchFields));
                OnPropertyChanged(nameof(DatabaseTables));
            }
        }
        public string TriggersTextSearchFields
        {
            get => model.TriggersTextSearchFields;
            set 
            {
                model.TriggersTextSearchFields = value;
                OnPropertyChanged(nameof(TriggersTextSearchFields));
                OnPropertyChanged(nameof(DatabaseTriggers));
            }
        }
        public string ViewsTextSearchFields
        {
            get => model.ViewsTextSearchFields;
            set 
            {
                model.ViewsTextSearchFields = value;
                OnPropertyChanged(nameof(ViewsTextSearchFields));
                OnPropertyChanged(nameof(DatabaseViews));
            }
        }
        public string DatabasesTextSearchFields
        {
            get => model.DatabasesTextSearchFields;
            set 
            {
                model.DatabasesTextSearchFields = value;
                OnPropertyChanged(nameof(DatabasesTextSearchFields));
                OnPropertyChanged(nameof(DatabasesEntities));
            }
        }
        #endregion

        #region Constructor 
        public MainWindowViewModel()
        {
            model.FunctionsNameChanged += RefreshFunctionName;
            model.StatusCheckedFunctionsChanged += RefreshCheckboxSelectionAllFunctions;

            model.ProceduresNameChanged += RefreshProceduresName;
            model.StatusCheckedProceduresChanged += RefreshCheckboxSelectionAllProcedures;

            model.TablesNameChanged += RefreshTablesName;
            model.StatusCheckedTablesChanged += RefreshCheckboxSelectionAllTables;

            model.TriggersNameChanged += RefreshTriggersName;
            model.StatusCheckedTriggersChanged += RefreshCheckboxSelectionAllTriggers;

            model.ViewsNameChanged += RefreshViewsName;
            model.StatusCheckedViewsChanged += RefreshCheckboxSelectionAllViews;

            model.DatabasesNameChanged += RefreshDatabaseName;
            model.StatusCheckedDatabasesChanged += RefreshCheckboxSelectionAllDatabases;

            model.ClickMenuItem += Refresh;
            model.ProgressBarVisibilityChanged += RefreshVisibleProgressBar;

            model.EntitiesDataBaseChanged += RefreshEntitiesDataBase;
            model.EntityDataBaseSelectedChanged += RefreshEntityDataBaseSelected;

            LoggerApplication.LogTextChanged += RefreshTextLog;
        }
        #endregion

        #region Private Methods
        private void Refresh()
        {
            OnPropertyChanged(nameof(Server));
            OnPropertyChanged(nameof(EntitiesDataBase));
            OnPropertyChanged(nameof(EntityDataBaseSelected));
            OnPropertyChanged(nameof(Path));
            OnPropertyChanged(nameof(ServerButtonChar));
            OnPropertyChanged(nameof(DatabaseFunctions));
            OnPropertyChanged(nameof(DatabaseProcedures));
            OnPropertyChanged(nameof(DatabaseTables));
            OnPropertyChanged(nameof(DatabaseTriggers));
            OnPropertyChanged(nameof(DatabaseViews));
            OnPropertyChanged(nameof(DatabasesEntities));
            OnPropertyChanged(nameof(MenuItems));
            OnPropertyChanged(nameof(ItemsDataBaseVisibility));
            OnPropertyChanged(nameof(TabEntitiesDataBaseVisibility));
            OnPropertyChanged(nameof(EntitiesDataBaseVisibility));
            OnPropertyChanged(nameof(DatabaseIsEnabled));
            OnPropertyChanged(nameof(PathIsEnabled));
            OnPropertyChanged(nameof(TIDatabasesIsSelected));
            OnPropertyChanged(nameof(TIFunctionsIsSelected));
        }

        private void RefreshVisibleProgressBar()
        {
            OnPropertyChanged(nameof(ProgressBarVisibility));
            OnPropertyChanged(nameof(IsEnabledWindow));
        }
        
        private void RefreshEntitiesDataBase()
        {
            OnPropertyChanged(nameof(EntitiesDataBase));
        }
        
        private void RefreshEntityDataBaseSelected()
        {
            OnPropertyChanged(nameof(EntityDataBaseSelected));
        }

        private void RefreshTextLog()
        {
            OnPropertyChanged(nameof(TextLog));
        }

        private void RefreshCheckboxSelectionAllViews()
        {
            OnPropertyChanged(nameof(IsCheckedViews));
        }
        
        private void RefreshCheckboxSelectionAllDatabases()
        {
            OnPropertyChanged(nameof(IsCheckedDatabases));
        }

        private void RefreshViewsName()
        {
            OnPropertyChanged(nameof(TIViewsName));
        }

        private void RefreshDatabaseName()
        {
            OnPropertyChanged(nameof(TIDatabasesName));
        }

        private void RefreshCheckboxSelectionAllTriggers()
        {
            OnPropertyChanged(nameof(IsCheckedTriggers));
        }

        private void RefreshTriggersName()
        {
            OnPropertyChanged(nameof(TITriggersName));
        }

        private void RefreshCheckboxSelectionAllTables()
        {
            OnPropertyChanged(nameof(IsCheckedTables));
        }

        private void RefreshTablesName()
        {
            OnPropertyChanged(nameof(TITablesName));
        }

        public void RefreshFunctionName()
        {
            OnPropertyChanged(nameof(TIFunctionsName));
        }

        public void RefreshCheckboxSelectionAllFunctions()
        {
            OnPropertyChanged(nameof(IsCheckedFunctions));
        }

        public void RefreshProceduresName()
        {
            OnPropertyChanged(nameof(TIProceduresName));
        }

        public void RefreshCheckboxSelectionAllProcedures()
        {
            OnPropertyChanged(nameof(IsCheckedProcedures));
        }
        #endregion

        #region Private Command
        private ICommand loadDatabases = null;
        private ICommand loadDatabaseItems = null;
        private ICommand changeSelectionAllFunction;
        private ICommand changeSelectionAllProcedures;
        private ICommand changeSelectionAllTables;
        private ICommand changeSelectionAllTriggers;
        private ICommand changeSelectionAllViews;
        private ICommand changeSelectionAllDatabases;
        private ICommand loadPath;
        private ICommand saveScripts;
        private ICommand saveLastSesion;
        private ICommand openLogWindow;
        private ICommand windowClosing;
        #endregion

        #region Public Command
        public ICommand WindowClosing
        {
            get
            {
                if (windowClosing == null) windowClosing = new RelayCommand((object o) =>
                {
                    model.WindowClosing((CancelEventArgs)o);
                });

                return windowClosing;
            }
        }
        public ICommand LoadDatabases
        {
            get
            {
                if (loadDatabases == null) loadDatabases = new RelayCommand((object o) =>
                {
                    model.LoadDatabases();

                    OnPropertyChanged(nameof(EntitiesDataBase));
                    OnPropertyChanged(nameof(EntityDataBaseSelected));
                    OnPropertyChanged(nameof(DatabaseIsEnabled));
                    OnPropertyChanged(nameof(PathIsEnabled));
                    OnPropertyChanged(nameof(Path));
                    OnPropertyChanged(nameof(Server));
                    OnPropertyChanged(nameof(ServerButtonChar));
                    OnPropertyChanged(nameof(DatabaseFunctions));
                    OnPropertyChanged(nameof(DatabaseProcedures));
                    OnPropertyChanged(nameof(DatabaseTables));
                    OnPropertyChanged(nameof(DatabaseTriggers));
                    OnPropertyChanged(nameof(DatabaseViews));
                    OnPropertyChanged(nameof(DatabasesEntities));
                });

                return loadDatabases;
            }
        }

        public ICommand LoadDatabaseItems
        {
            get
            {
                if (loadDatabaseItems == null) loadDatabaseItems = new RelayCommand((object o) =>
                {
                    model.LoadDatabaseItems();

                    OnPropertyChanged(nameof(DatabaseFunctions));
                    OnPropertyChanged(nameof(DatabaseProcedures));
                    OnPropertyChanged(nameof(DatabaseTables));
                    OnPropertyChanged(nameof(DatabaseTriggers));
                    OnPropertyChanged(nameof(DatabaseViews));
                    OnPropertyChanged(nameof(DatabasesEntities));
                });

                return loadDatabaseItems;
            }
        }

        public ICommand ChangeSelectionAllFunction
        {

            get
            {
                if (changeSelectionAllFunction == null) changeSelectionAllFunction = new RelayCommand((object o) =>
                {
                    model.ChangeSelectionAll(DatabaseFunctions, IsCheckedFunctions);
                    OnPropertyChanged(nameof(DatabaseFunctions));
                });

                return changeSelectionAllFunction;
            }
        }

        public ICommand ChangeSelectionAllProcedures
        {

            get
            {
                if (changeSelectionAllProcedures == null) changeSelectionAllProcedures = new RelayCommand((object o) =>
                {
                    model.ChangeSelectionAll(DatabaseProcedures, IsCheckedProcedures);
                    OnPropertyChanged(nameof(DatabaseProcedures));
                });

                return changeSelectionAllProcedures;
            }
        }

        public ICommand ChangeSelectionAllTables
        {

            get
            {
                if (changeSelectionAllTables == null) changeSelectionAllTables = new RelayCommand((object o) =>
                {
                    model.ChangeSelectionAll(DatabaseTables, IsCheckedTables);
                    OnPropertyChanged(nameof(DatabaseTables));
                });

                return changeSelectionAllTables;
            }
        }

        public ICommand ChangeSelectionAllTriggers
        {

            get
            {
                if (changeSelectionAllTriggers == null) changeSelectionAllTriggers = new RelayCommand((object o) =>
                {
                    model.ChangeSelectionAll(DatabaseTriggers, IsCheckedTriggers);
                    OnPropertyChanged(nameof(DatabaseTriggers));
                });

                return changeSelectionAllTriggers;
            }
        }

        public ICommand ChangeSelectionAllViews
        {

            get
            {
                if (changeSelectionAllViews == null) changeSelectionAllViews = new RelayCommand((object o) =>
                {
                    model.ChangeSelectionAll(DatabaseViews, IsCheckedViews);
                    OnPropertyChanged(nameof(DatabaseViews));
                });

                return changeSelectionAllViews;
            }
        }
        
        public ICommand ChangeSelectionAllDatabases
        {

            get
            {
                if (changeSelectionAllDatabases == null) changeSelectionAllDatabases = new RelayCommand((object o) =>
                {
                    model.ChangeSelectionAll(DatabasesEntities, IsCheckedDatabases);
                    OnPropertyChanged(nameof(DatabasesEntities));
                });

                return changeSelectionAllDatabases;
            }
        }

        public ICommand LoadPath
        {

            get
            {
                if (loadPath == null) loadPath = new RelayCommand((object o) =>
                {
                    model.LoadPath();
                    OnPropertyChanged(nameof(Path));
                });

                return loadPath;
            }
        }

        public ICommand SaveScripts
        {

            get
            {
                if (saveScripts == null) saveScripts = new RelayCommand(async (object o) =>
                {
                    Task.Run(() =>
                    {
                        model.SetVisibleProgressBar(true);
                        OnPropertyChanged(nameof(ProgressBarVisibility));

                        model.SaveScripts();
                        OnPropertyChanged(nameof(MenuItems));
                        OnPropertyChanged(nameof(ProgressBarVisibility));
                    });
                });

                return saveScripts;
            }
        }

        public ICommand SaveLastSesion
        {

            get
            {
                if (saveLastSesion == null) saveLastSesion = new RelayCommand((object o) =>
                {
                    model.SaveLastSesion();
                });

                return saveLastSesion;
            }
        }

        public ICommand OpenLogWindow
        {
            get
            {
                if (openLogWindow == null) openLogWindow = new RelayCommand((object o) =>
                {
                    model.OpenLogWindow();
                });

                return openLogWindow;
            }
        }
        #endregion
    }
}
