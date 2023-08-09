using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Adirev.Converter
{
    class AboutProgramInfoTextConverter : IValueConverter
    {
        #region ValueConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = value != null ? value.ToString().ToUpper() : string.Empty;

            if (text.Contains("#"))
            { text = string.Empty; }

            return text.Replace("[NEW]", "").Replace("[IMPROVED]", "").Replace("[FIXED]", "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
