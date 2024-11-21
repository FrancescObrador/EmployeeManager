using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace HumanResourcesManager.Utilities
{
    public class BitmapImageConverter : IValueConverter
    {
        public BitmapImage Convert(byte[] byteArray)
        {
            if (byteArray == null)
            {
                return new BitmapImage();
            }
            using (var ms = new MemoryStream(byteArray))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }

        public static byte[] Convert(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((byte[])value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
