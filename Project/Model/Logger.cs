using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    public sealed class Logger
    {
        #region Variables
        private string logOperacion;
        private static Logger instance = null;
        #endregion

        #region Properties
        public string LogOperacion
        {
            get => logOperacion;
            private set => logOperacion = value;
        }
        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }
        #endregion

        #region Constructor
        private Logger() { }
        #endregion

        #region Public Method
        public void AddLog(string log)
        {
            LogOperacion += $"{log}\n";
        }
        #endregion
    }
}
