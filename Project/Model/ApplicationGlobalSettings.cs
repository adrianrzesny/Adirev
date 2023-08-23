using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    [Serializable]
    public class ApplicationGlobalSettings
    {
        #region Variables
        private static string folderApplication = "Adirev";
        private static string folderSettings = "Settings";
        private static string folderHistory = "History";
        private static string folderLog = "Log";
        private static string extension = "ars";
        #endregion

        #region Properties
        public bool Autorun { get; set; }
        public bool HideWork { get; set; }
        public static string PathApplication { get => Environment.GetEnvironmentVariable("USERPROFILE"); }
        public static string PathFolderApplication { get => $@"{Environment.GetEnvironmentVariable("USERPROFILE")}\{FolderApplication}"; }
        public static string PathHistoryApplication { get => $@"{Environment.GetEnvironmentVariable("USERPROFILE")}\{FolderApplication}\{FolderHistory}"; }
        public static string PathlogApplication { get => $@"{Environment.GetEnvironmentVariable("USERPROFILE")}\{FolderApplication}\{folderLog}"; }
        public static string PathSettingsApplication { get => $@"{Environment.GetEnvironmentVariable("USERPROFILE")}\{FolderApplication}\{FolderSettings}"; }
        public static string FolderApplication { get => folderApplication; }
        public static string FolderSettings { get => folderSettings; }
        public static string FolderHistory { get => folderHistory; }
        public static string FolderLog { get => folderLog; }
        public static string Extension { get => extension; }
        #endregion

        #region Construcotr
        public ApplicationGlobalSettings()
        { }
        #endregion
    }
}
