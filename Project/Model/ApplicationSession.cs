using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    [Serializable]
    public class ApplicationSession
    {
        #region Variables
        private static string folderApplication = "Adirev";
        private static string folderHistoryApplication = "History";
        private static string extension = "ars";
        #endregion

        #region Properties
        public static string PathApplication { get => Environment.GetEnvironmentVariable("USERPROFILE"); }
        public static string PathFolderApplication { get => $@"{Environment.GetEnvironmentVariable("USERPROFILE")}\{FolderApplication}"; }
        public static string PathHistoryApplication { get => $@"{Environment.GetEnvironmentVariable("USERPROFILE")}\{FolderApplication}\{FolderHistoryApplication}"; }
        public static string FolderApplication { get => folderApplication; }
        public static string FolderHistoryApplication { get => folderHistoryApplication; }
        public static string Extension { get => extension; }
        public string System { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string Path { get; set; }
        #endregion

    }
}
