using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Adirev.Converter
{
    public class AboutProgramInfoTextIconConverter : IValueConverter
    {
        #region ValueConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value != null ? value.ToString().ToUpper() : string.Empty;

            if (text.Contains("NEW"))
            { return "NEW"; }
            else if (text.Contains("IMPROVED"))
            { return "IMPROVED"; }
            else if (text.Contains("FIXED"))
            { return "FIXED"; }
            else
            { return String.Empty; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
