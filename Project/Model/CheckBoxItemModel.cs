using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Adirev.Class;

namespace Adirev.Model
{
    class CheckBoxItemModel
    {
        #region Variables
        private string name;
        private bool isSelected;
        private Visibility visibility = Visibility.Visible;
        #endregion

        #region Properties
        public string Name
        {
            get => name;
            set => name = value;
        }

        public bool IsSelected
        {
            get => isSelected;
            set => isSelected = value;
        }

        public Visibility Visibility
        {
            get => visibility;
            set => visibility = value;
        }
        #endregion
    }
}
