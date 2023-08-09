using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Adirev.Converter
{
    class AboutProgramInfoColorIconConverter : IValueConverter
    {
        #region ValueConverter
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string type = value != null ? value.ToString().ToUpper() : string.Empty;

            if (type.Contains("NEW"))
            { return new SolidColorBrush(Colors.Lime); }
            else if (type.Contains("IMPROVED"))
            { return new SolidColorBrush(Colors.DeepSkyBlue); }
            else if (type.Contains("FIXED"))
            { return new SolidColorBrush(Colors.LightCoral); }
            else 
            { return new SolidColorBrush(Colors.Transparent); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
