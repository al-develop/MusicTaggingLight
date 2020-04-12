using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MusicTaggingLight.Logic
{
    public static class ImageResizer
    {
        public static byte[] ResizeImage(byte[] selectedImage)
        {
            using (var ms = new MemoryStream(selectedImage))
            {
                var image = System.Drawing.Image.FromStream(ms);

                var ratioX = (double)120 / image.Width;
                var ratioY = (double)120 / image.Height;
                var ratio = Math.Min(ratioX, ratioY);

                var width = (int)(image.Width * ratio);
                var height = (int)(image.Height * ratio);

                var newImage = new Bitmap(width, height);
                Graphics.FromImage(newImage).DrawImage(image, 0, 0, width, height);
                Bitmap bmp = new Bitmap(newImage);

                ImageConverter converter = new ImageConverter();

                selectedImage = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                return selectedImage;
            }
        }
    }
}
