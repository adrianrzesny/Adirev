using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using Adirev.Model;

namespace Adirev.Class
{
    public delegate void CheckBoxItemEventHandler();
    public class CheckBoxItem : NotifyPropertyChanged
    {
        #region Variables
        private CheckBoxItemModel model = new CheckBoxItemModel();
        #endregion

        #region Events
        public event CheckBoxItemEventHandler StatusChanged;
        #endregion

        #region Properties
        public string Name
        {
            get => model.Name;
            set
            {
                model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool IsSelected
        {
            get => model.IsSelected;
            set
            {
                model.IsSelected = value;
                OnPropertyChanged(nameof(IsSelected));
                StatusChanged?.Invoke();
            }
        }

        public Visibility Visibility
        {
            get => model.Visibility;
            set
            {
                model.Visibility = value;
                OnPropertyChanged(nameof(Visibility));
                StatusChanged?.Invoke();
            }
        }
        #endregion
    }
}
