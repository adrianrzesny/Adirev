using System;
using System.Collections.Generic;
using System.Text;
using Adirev.Class;

namespace Adirev.Model
{
    class CheckBoxItemModel
    {
        #region Variables
        private string name;
        private bool isSelected;
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
        #endregion
    }
}
