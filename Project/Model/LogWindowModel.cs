using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    class LogWindowModel
    {
        #region Properties
        public Logger LoggerApplication { get; set; } = Logger.Instance;
        public string Logs { get => LoggerApplication.LogHistory; }
        #endregion
    }
}
