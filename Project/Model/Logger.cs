﻿using Adirev.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    public delegate void LoggerEventHandler();
    public sealed class Logger
    {
        #region Variables
        private string logOperacion;
        private string logHistory;
        private static Logger instance = null;
        #endregion

        #region Events
        public event LoggerEventHandler LogTextChanged;
        #endregion

        #region Properties
        public string LogOperacion
        {
            get => logOperacion;
            private set
            {
                logOperacion = value;
                LogTextChanged?.Invoke();
            }
        }
        public string LogHistory
        {
            get => logHistory;
            private set
            {
                logHistory = value;
                LogTextChanged?.Invoke();
            }
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
        public void AddLog(string log, bool addToHistory = false)
        {
            LogOperacion = $" [{DateTime.Now}] {log}\n";

            if (addToHistory)
            { LogHistory = $" [{DateTime.Now}] {log}\n{LogHistory}"; }
        }

        public void ClearLogs()
        {
            LogOperacion = string.Empty;
            LogHistory = string.Empty;
        }

        public static void SaveError(string message, string innerMessage, string module, string additionalInformation = null)
        {
            additionalInformation = additionalInformation == null ? string.Empty : $"({additionalInformation})";
            string content = $"[{module}]{additionalInformation} {message} {innerMessage}";
            FileManager.CreateDirectory(ApplicationGlobalSettings.PathFolderApplication, ApplicationGlobalSettings.FolderLog);
            FileManager.SaveFile(ApplicationGlobalSettings.PathlogApplication, $"LOG{DateTime.Now}_{DateTime.Now.Millisecond}", "txt", content);
        }
        #endregion
    }
}
