using Adirev.Service;
using Adirev.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Adirev.Model
{
    class StartingSettingsWindowModel
    {
        #region Object
        private ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Instance;
        #endregion

        #region Properties
        public bool Autorun { get; set; }
        public bool HideWork { get; set; }
        #endregion

        #region Constructor
        public StartingSettingsWindowModel()
        {
            Autorun = ApplicationStatus.Settings.Autorun;
            HideWork = ApplicationStatus.Settings.HideWork;
        }
        #endregion

        #region Private Method
        private void AddToAutorun(bool isAutorun)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
           
            var path = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\Adirev.exe";

            if (isAutorun)
            { rk.SetValue("Adirev", path); }
            else
            { rk.DeleteValue("Adirev", false); }
        }
        #endregion

        #region Public Method
        public void Save(StartingSettingsWindow window)
        {
            ApplicationStatus.Settings.Autorun = Autorun;
            ApplicationStatus.Settings.HideWork = HideWork;

            string pathSettings = @$"{ApplicationGlobalSettings.PathSettingsApplication}\settings.{ApplicationGlobalSettings.Extension}";
            FileManager.CreateDirectory(ApplicationGlobalSettings.PathFolderApplication, ApplicationGlobalSettings.FolderSettings);
            FileManager.WriteToBinaryFile<ApplicationGlobalSettings>(pathSettings, ApplicationStatus.Settings);

            AddToAutorun(Autorun); 

            window.Close();
        }
        #endregion
    }
}
