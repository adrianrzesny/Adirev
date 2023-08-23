using Adirev.Model;
using Adirev.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Adirev.ViewModel
{
    class StartingSettingsWindowViewModel : NotifyPropertyChanged
    {
        #region Object
        private StartingSettingsWindowModel model = new StartingSettingsWindowModel();
        #endregion

        #region Properties

        public bool Autorun
        {
            get => model.Autorun;
            set
            {
                model.Autorun = value;
                OnPropertyChanged(nameof(Autorun));
            }
        }
        public bool HideWork
        {
            get => model.HideWork;
            set
            {
                model.HideWork = value;
                OnPropertyChanged(nameof(HideWork));
            }
        }
        #endregion

        #region Private Command
        private ICommand save = null;
        #endregion

        #region Public Command

        public ICommand Save
        {
            get
            {
                if (save == null) save = new RelayCommand((object o) =>
                {
                    model.Save((StartingSettingsWindow)o);
                });

                return save;
            }
        }
        #endregion
    }
}
