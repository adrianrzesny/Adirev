using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    public class ApplicationStatus
    {
        #region Variables
        private bool closed = false;
        private bool firstRun = true;
        private static ApplicationStatus instance = null;
        ApplicationGlobalSettings settings = null;
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
        public static ApplicationStatus Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ApplicationStatus();
                }
                return instance;
            }
        }
        public ApplicationGlobalSettings Settings
        {
            get => settings;
            set => settings = value;
        }
        #endregion

        #region Constructor
        private ApplicationStatus() 
        {
            Settings = new ApplicationGlobalSettings();
        }
        #endregion
        
    }
}
