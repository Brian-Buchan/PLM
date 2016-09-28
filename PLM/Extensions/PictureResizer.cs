using System;
using System.Drawing;
using System.IO;

namespace PLM
{
    public static class PictureResizer
    {
        public static int GetImageWidth(System.Drawing.Image image)
        {
            int rc = 0;
            try
            {
                rc = image.Width;
            }
            catch
            { // (Exception e) {

            }
            return rc;
        }

        public static int GetImageHeight(System.Drawing.Image image)
        {
            int rc = 0;
            try
            {
                rc = image.Height;
            }
            catch
            { // (Exception e) {

            }
            return rc;
        }

        public static Image GetImageFromByteArray(byte[] bitmap)
        {

            Image rc;

            using (var origImageStream = new MemoryStream(bitmap))
            using (var pngImageStream = new MemoryStream())
            {
                var pngImage = System.Drawing.Image.FromStream(origImageStream);
                pngImage.Save(pngImageStream, System.Drawing.Imaging.ImageFormat.Png);
                //
                rc = Image.FromStream(pngImageStream);
                pngImage.Dispose();
                pngImageStream.Dispose();
                return rc;
            }

        }

        public static Image ConvertImageToPng(Image image)
        {

            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();
                var byteArray = stream.ToArray();
                stream.Dispose();
                return GetImageFromByteArray(byteArray);
            }

        }

        public static byte[] GetByteArrayFromImage(Image image)
        {

            ImageConverter converter = new ImageConverter();
            byte[] imgArray = (byte[])converter.ConvertTo(image, typeof(byte[]));
            //TODO: check to see if this needs a dispose
            return imgArray;
        }

        public static void GetDimensions(byte[] imgByteArray, out int w, out int h)
        {
            w = 0;
            h = 0;
            Image img = GetImageFromByteArray(imgByteArray);
            float fWidth = img.PhysicalDimension.Width;

            float fHeight = img.PhysicalDimension.Height;
            w = (int)fWidth;
            h = (int)fHeight;
        }



        public static byte[] GetScaledDownByteArray(byte[] byteImage, int maxWidth, int maxHeight)
        {
            byte[] rc;
            Image image = GetImageFromByteArray(byteImage);
            Image img = ScaleImage(image, maxWidth, maxHeight);
            rc = GetByteArrayFromImage(img);
            img.Dispose();
            image.Dispose();
            return rc;
        }

        public static Image ScaleImage(System.Drawing.Image image, int maxWidth, int maxHeight)
        {
            try
            {
                var ratioX = (double)maxWidth / image.Width;
                var ratioY = (double)maxHeight / image.Height;
                var ratio = Math.Min(ratioX, ratioY);
                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);
                var newImage = new Bitmap(newWidth, newHeight);
                Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);

                return ConvertImageToPng(newImage);
            }
            catch
            {
                return image;
            }
        }
        /*
                     byte[] jpgImageBytes = null;
             using (var origImageStream = new MemoryStream(image))
             using (var jpgImageStream = new MemoryStream())
             {
                 var jpgImage = System.Drawing.Image.FromStream(origImageStream);
                 jpgImage.Save(jpgImageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                 jpgImageBytes = jpgImageStream.ToArray();
                 jpgImage.Dispose();
             }
                         */
    }
}