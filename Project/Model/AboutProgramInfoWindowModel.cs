using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Adirev.Model
{
    class AboutProgramInfoWindowModel
    {
        #region Properties
        public ObservableCollection<AboutProgramPositionModel> AboutProgramPositions { get; set; }
        #endregion

        #region Constructor
        public AboutProgramInfoWindowModel()
        {
            AboutProgramPositions = new ObservableCollection<AboutProgramPositionModel>();
            LoadInfoVersion();
        }
        #endregion

        #region Private Methods
        private void LoadInfoVersion()
        {
            try
            {
                string path = File.Exists("Changes/CHANGELOG.md") ? "Changes/CHANGELOG.md" : "Project/Changes/CHANGELOG.md";
                using (StreamReader sr = File.OpenText(path))
                {
                    string line = String.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        { AboutProgramPositions.Add(new AboutProgramPositionModel(line.Replace("#", "Adirev"), line.Contains("#"))); }
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion
    }
}
