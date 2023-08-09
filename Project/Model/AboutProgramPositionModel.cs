using System;
using System.Collections.Generic;
using System.Text;

namespace Adirev.Model
{
    class AboutProgramPositionModel
    {
        #region Properties
        public string Text { get; set; }
        public string Version { get; set; }
        #endregion

        #region Constructor
        public AboutProgramPositionModel()
        { }

        public AboutProgramPositionModel(string text, bool isVersion)
        {            
            if (isVersion)
            { Version = text; }
            else
            { Text = text; }
        }
        #endregion
    }
}
