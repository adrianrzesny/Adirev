using Adirev.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Adirev.ViewModel
{
    class LicenseWindowViewModel : NotifyPropertyChanged
    {
        #region Objects 
        private LicenseWindowModel model = new LicenseWindowModel();
        #endregion

        #region Properties
        public string Licence
        {
            get => model.License;
            set => model.License = value;
        }

        public Visibility ContentLicenseVisibility
        {
            get => model.ContentLicenseVisibility;
            set => model.ContentLicenseVisibility = value;
        }
        #endregion


        #region Private Command
        private ICommand showLicense = null;
        #endregion

        #region Public Command
        public ICommand ShowLicense
        {
            get
            {
                if (showLicense == null) showLicense = new RelayCommand((object o) =>
                {
                    model.ShowLicense();
                    OnPropertyChanged(nameof(ContentLicenseVisibility));
                });

                return showLicense;
            }
        }
        #endregion
    }
}
