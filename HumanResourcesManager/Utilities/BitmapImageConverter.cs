using System;
using System.Collections;
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

        /// <summary>
        /// Used to convert a byte[] to an image
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is byte[])
            {
                try
                {
                    var memoryStream = new MemoryStream((byte[])value);
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = memoryStream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    return image;
                }
                catch (Exception ex) 
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Used to convert an already converted and stored file
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BitmapImage bitmapImage)
            {
                try
                {
                    var stream = new MemoryStream();
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(stream);
                    return stream.ToArray();
                    
                }
                catch
                {
                    return null; 
                }
            }
            return null;
        }

        public byte[] FromFile(string filePath)
        {
            if (filePath != null)
            {
                return File.ReadAllBytes(filePath);
            }

            return Array.Empty<byte>();
        }
    }
}
