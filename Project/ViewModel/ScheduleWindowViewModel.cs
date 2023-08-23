using Adirev.Model;
using Adirev.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Adirev.ViewModel
{
    class ScheduleWindowViewModel : NotifyPropertyChanged
    {
        #region Objects
        private ScheduleWindowModel model = new ScheduleWindowModel();
        #endregion

        #region Properties
        public ObservableCollection<ScheduleItemControl> ScheduleItems
        {
            get => model.ScheduleItems;
            set
            {
                model.ScheduleItems = value;
                OnPropertyChanged(nameof(ScheduleItems));
            }
        }
        #endregion

        #region Private Command
        private ICommand addScheduleItem = null;
        private ICommand save = null;
        #endregion

        #region Public Command
        public ICommand AddScheduleItem
        {
            get
            {
                if (addScheduleItem == null) addScheduleItem = new RelayCommand((object o) =>
                {
                    model.AddScheduleItem();
                    OnPropertyChanged(nameof(ScheduleItems));
                });

                return addScheduleItem;
            }
        }
        public ICommand Save
        {
            get
            {
                if (save == null) save = new RelayCommand((object o) =>
                {
                    model.Save();
                    OnPropertyChanged(nameof(ScheduleItems));
                });

                return save;
            }
        }
        #endregion
    }
}
