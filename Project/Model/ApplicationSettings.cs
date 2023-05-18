using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    public class ApplicationSettings
    {
        #region Variables
        private bool closed = false;
        private bool firstRun = true;
        private static ApplicationSettings instance = null;
        #endregion

        #region Properties
        public bool Closed
        {
            get => closed;
            set => closed = value;      
        }
        public bool FirstRun
        {
            get => firstRun;
            set => firstRun = value;      
        }
        public static ApplicationSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ApplicationSettings();
                }
                return instance;
            }
        }
        #endregion

        #region Constructor
        private ApplicationSettings() { }
        #endregion
        
    }
}
