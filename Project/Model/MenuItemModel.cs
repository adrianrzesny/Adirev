using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Adirev.Model
{
    public class MenuItemModel
    {
        #region Properties
        public string Header { get; set; }
        public ObservableCollection<object> MenuItems { get; set; }
        #endregion
    }
}
