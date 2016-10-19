using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace PLM.Models
{
    public class ConfirmViewModel
    {
        public string Base64ImgData { get; set; }

        public string originalBase64ImgData { get; set; }

        public string imageID { get; set; }

        public string answerID { get; set; }

        public ConfirmViewModel(string b64Img, string originalb64Img, string imgID, string ansID)
        {
            Base64ImgData = CleanPictureData(b64Img);
            originalBase64ImgData = CleanPictureData(originalb64Img);
            imageID = imgID;
            answerID = ansID;
        }

        private static string CleanPictureData(string pictureData)
        {
            pictureData = pictureData.Replace("data:image/png;base64,", "");
            pictureData = pictureData.Replace("data:image/jpeg;base64,", "");
            return pictureData;
        }
    }
}