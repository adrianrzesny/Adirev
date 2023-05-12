using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Adirev.Model;

namespace Adirev.ViewModel
{
    public class MenuItemViewModel : NotifyPropertyChanged
    {
        #region Objects
        private readonly ICommand _command;
        private MenuItemModel model = new MenuItemModel();
        #endregion

        #region Constructor
        public MenuItemViewModel(Action<string> execute, Action eventClick, string param)
        {
            _command = new CommandViewModel(execute, eventClick, param);
        }

        public MenuItemViewModel(Action execute, Action eventClick)
        {
            _command = new CommandViewModel(execute, eventClick);
        }
        #endregion

        #region Properties
        public string Header
        {
            get => model.Header;
            set
            {
                model.Header = value;
                OnPropertyChanged(nameof(Header));
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

        public ICommand Command
        {
            get => _command;
        }
        #endregion
    }

    public class CommandViewModel : ICommand
    {
        #region Objects
        private readonly Action _action = null;
        private readonly Action _actionClick = null;
        private readonly Action<string> _action_s = null;
        private readonly string _param;
        #endregion

        #region Constructor
        public CommandViewModel(Action action, Action actionClick)
        {
            _action = action;
            _actionClick = actionClick;
        }

        public CommandViewModel(Action<string> action, Action actionClick, string param)
        {
            _action_s = action;
            _actionClick = actionClick;
            _param = param;
        }
        #endregion

        #region Public Methods
        public void Execute(object o)
        {
            if (_action_s != null)
            { _action_s(_param); }

            if (_action != null)
            { _action(); }

            _actionClick();
        }

        public bool CanExecute(object o)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
        #endregion
    }
}
