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
        /// <summary>
        /// Base 64 image data
        /// </summary>
        public string Base64ImgData { get; set; }
        /// <summary>
        /// The original image's url
        /// </summary>
        public string originalUrl { get; set; }
        /// <summary>
        /// The ID of the image in the database
        /// </summary>
        public string imageID { get; set; }
        /// <summary>
        /// The ID of the answer in the database
        /// </summary>
        public string answerID { get; set; }
        /// <summary>
        /// The absolute path to the image file in the temp folder
        /// </summary>
        public string tempUrl { get; set; }

        public ConfirmViewModel(string b64Img, string origUrl, string imgID, string ansID, string tmpUrl)
        {
            Base64ImgData = b64Img;
            originalUrl = origUrl;
            imageID = imgID;
            answerID = ansID;
            tempUrl = tmpUrl;
        }
    }
}