using Adirev.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.ViewModel
{
    class LogWindowViewModel : NotifyPropertyChanged
    {
        #region Objects 
        private LogWindowModel model = new LogWindowModel();
        #endregion

        #region Properties
        public string Logs { get => model.Logs; }
        #endregion

        #region Constructor
        public LogWindowViewModel()
        {
            model.LoggerApplication.LogTextChanged += RefreshLogs;
        }
        #endregion

        #region Public Method
        public void RefreshLogs()
        {
            OnPropertyChanged(nameof(Logs));
        }
        #endregion
    }
}
