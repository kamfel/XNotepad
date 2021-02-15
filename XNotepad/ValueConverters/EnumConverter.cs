using System;
using System.Globalization;
using XNotepad.UI.Resources;

namespace XNotepad.UI.ValueConverters
{
    public class EnumConverter : BaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resourceManager = EnumStrings.ResourceManager;
            var typeName = value.GetType().Name;
            return resourceManager.GetString($"{typeName}_{value.ToString()}");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new object();
        }
    }
}
