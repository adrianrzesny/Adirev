using Adirev.Service;
using Adirev.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Adirev.View
{
    public partial class ScheduleItemControl : UserControl, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Variables
        private Visibility visibilityForm;
        private Visibility visibilityWindow;
        private Visibility visibilityListDay;
        private Visibility visibilityLastExecutionTime;
        private bool isCheckedHour;
        private bool isCheckedDay;
        private bool isCheckedWeek;
        private string title;
        private string serverButtonChar;
        private bool isValid;
        private bool serverIsEnabled;
        private Brush borderFormColor;
        private string entityDataBaseSelected;
        public DateTime realLastExecutionTime;
        #endregion

        #region DependencyProperty
        public static readonly DependencyProperty NameScheduleProperty = DependencyProperty.Register(
           "NameSchedule", typeof(string), typeof(ScheduleItemControl), new PropertyMetadata(""));

        public static readonly DependencyProperty SystemProperty = DependencyProperty.Register(
           "SystemDatabase", typeof(string), typeof(ScheduleItemControl), new PropertyMetadata(""));

        public static readonly DependencyProperty ServerProperty = DependencyProperty.Register(
           "Server", typeof(string), typeof(ScheduleItemControl), new PropertyMetadata(""));

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
           "Path", typeof(string), typeof(ScheduleItemControl), new PropertyMetadata(""));

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
           "Time", typeof(string), typeof(ScheduleItemControl), new PropertyMetadata(""));

        public static readonly DependencyProperty DayProperty = DependencyProperty.Register(
           "Day", typeof(string), typeof(ScheduleItemControl), new PropertyMetadata(""));

        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
           "IsActive", typeof(bool), typeof(ScheduleItemControl), new PropertyMetadata(false));
        #endregion

        #region Properties

        #region Private
        private bool IsValidSystemDatabase { get; set; } = true;
        private bool IsValidServer { get; set; } = true;
        private bool IsValidEntityDataBaseSelected { get; set; } = true;
        private bool IsValidPath { get; set; } = true;
        private bool IsValidTime { get; set; } = true;
        private bool IsValidDay { get; set; } = true;
        private List<string> entitiesDataBase;
        #endregion

        #region Public
        public bool Connected { get; set; } = false;
        public bool IsEnabled { get => VisibilityWindow == Visibility.Visible ? true : false; }
        public bool IsValid { get => isValid; }
        public List<string> SystemsList { get => new List<string>() { "Microsoft SQL Server" }; }
        public DateTime LastExecutionTime { get; set; }
        public ServerManager ServerDatabase { get; set; }
        public string Error => null;
        public string this[string columnName] { get => Validation(columnName); }
        public string NameSchedule
        {
            get { return (string)GetValue(NameScheduleProperty); }
            set { SetValue(NameScheduleProperty, value); }
        }
        public string SystemDatabase
        {
            get { return (string)GetValue(SystemProperty); }
            set { SetValue(SystemProperty, value); }
        }
        public string Server
        {
            get { return (string)GetValue(ServerProperty); }
            set
            {
                SetValue(ServerProperty, value);
                ServerDatabase.ServerDatabase = value;
            }
        }
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public string Time
        {
            get { return (string)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        public string Day
        {
            get { return (string)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public DateTime RealLastExecutionTime 
        { 
            get => realLastExecutionTime; 
            set
            {
                realLastExecutionTime = value;
                VisibilityLastExecutionTime = value != DateTime.MinValue ? Visibility.Visible : Visibility.Hidden;
                OnPropertyChanged(nameof(RealLastExecutionTime));
            }
        }
        public List<string> EntitiesDataBase
        {
            get => entitiesDataBase;
            set
            {
                entitiesDataBase = value;
                OnPropertyChanged(nameof(EntitiesDataBase));
            }
        }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public string ServerButtonChar
        {
            get => serverButtonChar;
            set
            {
                serverButtonChar = value;
                OnPropertyChanged(nameof(ServerButtonChar));
            }
        }
        public Visibility VisibilityLastExecutionTime
        {
            get => visibilityLastExecutionTime;
            set
            {
                visibilityLastExecutionTime = value;
                OnPropertyChanged(nameof(VisibilityLastExecutionTime));
            }
        }
        public Visibility VisibilityListDay
        {
            get => visibilityListDay;
            set
            {
                visibilityListDay = value;
                OnPropertyChanged(nameof(VisibilityListDay));
            }
        }
        public Visibility VisibilityWindow
        {
            get => visibilityWindow;
            set
            {
                visibilityWindow = value;
                OnPropertyChanged(nameof(VisibilityWindow));
            }
        }
        public Visibility VisibilityForm
        {
            get => visibilityForm;
            set
            {
                visibilityForm = value;
                OnPropertyChanged(nameof(VisibilityForm));
            }
        }
        public bool IsCheckedHour
        {
            get => isCheckedHour;
            set
            {
                isCheckedHour = value;
                if (value)
                { VisibilityListDay = Visibility.Hidden; }
                OnPropertyChanged(nameof(IsCheckedHour));
            }
        }
        public bool IsCheckedDay
        {
            get => isCheckedDay;
            set
            {
                isCheckedDay = value;
                if (value)
                { VisibilityListDay = Visibility.Hidden; }
                OnPropertyChanged(nameof(IsCheckedDay));
            }
        }
        public bool IsCheckedWeek
        {
            get => isCheckedWeek;
            set
            {
                isCheckedWeek = value;
                if (value)
                { VisibilityListDay = Visibility.Visible; }
                OnPropertyChanged(nameof(IsCheckedWeek));
            }
        }
        public Brush BorderFormColor
        {
            get => borderFormColor;
            set
            {
                borderFormColor = value;
                OnPropertyChanged(nameof(BorderFormColor));
            }
        }
        public bool ServerIsEnabled
        {
            get => serverIsEnabled;
            set
            {
                serverIsEnabled = value;
                OnPropertyChanged(nameof(ServerIsEnabled));
            }
        }
        public string EntityDataBaseSelected
        {
            get => entityDataBaseSelected;
            set
            {
                entityDataBaseSelected = value;
                OnPropertyChanged(nameof(EntityDataBaseSelected));
            }
        }
        #endregion

        #endregion

        #region Constructor
        public ScheduleItemControl()
        {
            InitializeComponent();
            ServerDatabase = new ServerManager();
            VisibilityForm = Visibility.Collapsed;
            IsCheckedDay = true;
            NameSchedule = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Title = NameSchedule;
            Time = "00:00:00";
            Day = DateTime.Now.ToString("yyyy-MM-dd"); 
            LastExecutionTime = DateTime.MinValue;
            ServerIsEnabled = true;
            ServerButtonChar = Connected == true ? "X" : "⮧";
            isValid = false;
            BorderFormColor = new SolidColorBrush(Colors.Black);
            VisibilityLastExecutionTime = Visibility.Hidden;

            RefreshTitle();
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Public methods
        public void RefreshTitle()
        {
            Title = $"[{NameSchedule}] {Server}->{EntityDataBaseSelected} [{(IsActive ? "Active" : "Inactive")}]";
        }
        public void LoadDatabases(object sender, RoutedEventArgs e)
        {
            if (!Connected)
            {
                ServerDatabase.System = SystemDatabase;
                ServerDatabase.ServerDatabase = Server;
                if (ServerDatabase.LoadDatabases())
                {
                    EntitiesDataBase = ServerDatabase.Databases.Select(x => x.DatabaseEntity).ToList();
                    Connected = true;
                    ServerIsEnabled = false;
                    ServerButtonChar = Connected == true ? "X" : "⮧";
                }
            }
            else
            {
                EntitiesDataBase.Clear();
                EntityDataBaseSelected = null;
                Connected = false;
                ServerIsEnabled = true;
                Server = "";
                Path = "";
                ServerButtonChar = Connected == true ? "X" : "⮧";


                ServerDatabase.Login = String.Empty;
                ServerDatabase.Password = String.Empty;
            }
        }
        #endregion

        #region Private methods
        private void ShowForm(object sender, RoutedEventArgs e)
        {
            VisibilityForm = VisibilityForm == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
        private void HideWindow(object sender, RoutedEventArgs e)
        {
            VisibilityWindow = Visibility.Collapsed;
        }
        private string Validation(string columnName)
        {
            isValid = true;
            string msg = string.Empty;

            if (columnName == "SystemDatabase")
            {
                if (string.IsNullOrEmpty(SystemDatabase))
                {
                    IsValidSystemDatabase = false;
                    msg = "The field cannot be empty.";
                }
                else
                {
                    IsValidSystemDatabase = true;
                }
            }
            if (columnName == "Server")
            {
                if (string.IsNullOrEmpty(Server))
                {
                    IsValidServer = false;
                    msg = "The field cannot be empty.";
                }
                else
                {
                    IsValidServer = true;
                }
            }
            if (columnName == "EntityDataBaseSelected")
            {
                if (string.IsNullOrEmpty(EntityDataBaseSelected))
                {
                    IsValidEntityDataBaseSelected = false;
                    msg = "The field cannot be empty.";
                }
                else
                {
                    IsValidEntityDataBaseSelected = true;
                }
            }
            if (columnName == "Path")
            {
                if (string.IsNullOrEmpty(Path))
                {
                    IsValidPath = false;
                    msg = "The field cannot be empty.";
                }
                else
                {
                    IsValidPath = true;
                }
            }
            if (columnName == "Time")
            {
                string pattern = @"^(?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d$";
                if (!Regex.IsMatch(Time, pattern))
                {
                    IsValidTime = false;
                    msg = "Invalid format.";
                }
                else
                {
                    IsValidTime = true;
                }
            }
            if (columnName == "Day" && IsCheckedWeek)
            {
                string pattern = @"^\d{4}-\d{2}-\d{2}$";
                if (!Regex.IsMatch(Day, pattern))
                {
                    IsValidDay = false;
                    msg = "Invalid format.";
                }
                else
                {
                    IsValidDay = true;
                }
            }

            if (!IsValidSystemDatabase || !IsValidServer || !IsValidEntityDataBaseSelected || !IsValidPath || !IsValidTime || !IsValidDay)
            {
                isValid = false;
                BorderFormColor = new SolidColorBrush(Colors.Red);
                return msg;
            }
            else
            {
                BorderFormColor = new SolidColorBrush(Colors.Black);
            }

            return null;
        }
        private void LoadPath(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                { Path = dialog.SelectedPath; }
            }
        }
        private void LoadCredentials(object sender, RoutedEventArgs e)
        {
            CredentialsWindow cw = new CredentialsWindow(Server);

            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            cw.Left = mainWindow.Left + ((mainWindow.Width - cw.Width) / 2);
            cw.Top = mainWindow.Top + ((mainWindow.Height - cw.Height) / 2);

            bool? resultWindow = cw.ShowDialog();
            CredentialsWindowViewModel viewModel = (CredentialsWindowViewModel)cw.DataContext;
            ServerDatabase.Login = viewModel.Login.Length == 0 ? "#" : viewModel.Login;
            ServerDatabase.Password = viewModel.Password.Length == 0 ? "#" : viewModel.Password;
        }
        #endregion
    }
}

