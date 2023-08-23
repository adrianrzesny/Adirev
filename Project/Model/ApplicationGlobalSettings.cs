using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    [Serializable]
    public class ApplicationGlobalSettings
    {
        #region Properties
        public bool Autorun { get; set; }
        public bool HideWork { get; set; }
        #endregion

        #region Construcotr
        public ApplicationGlobalSettings()
        {}
        #endregion
    }
}
