using Adirev.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Adirev.ViewModel
{
    class AboutProgramInfoWindowViewModel : NotifyPropertyChanged
    {
        #region Objects 
        private AboutProgramInfoWindowModel model = new AboutProgramInfoWindowModel();
        #endregion

        #region Properties
        public ObservableCollection<AboutProgramPositionModel> AboutProgramPositions
        { 
            get => model.AboutProgramPositions; 
            set=> model.AboutProgramPositions = value; 
        }
        #endregion
    }
}
