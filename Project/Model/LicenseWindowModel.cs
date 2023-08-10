using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace Adirev.Model
{
    class LicenseWindowModel
    {
        #region Properties
        public string License { get; set; }
        public Visibility ContentLicenseVisibility { get; set; }
        #endregion

        #region Constructor
        public LicenseWindowModel()
        {
            ContentLicenseVisibility = Visibility.Collapsed;
            LoadLicense();
        }
        #endregion

        #region Private Methods
        private void LoadLicense()
        {
            try
            {
                string path = File.Exists("Attachments/LICENSE.md") ? "Attachments/LICENSE.md" : "Project/Attachments/LICENSE.md";
                using (StreamReader sr = File.OpenText(path))
                {
                    string line = String.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        { License += line + "\n"; }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Public Method
        public void ShowLicense()
        {
            ContentLicenseVisibility = ContentLicenseVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion
    }
}
