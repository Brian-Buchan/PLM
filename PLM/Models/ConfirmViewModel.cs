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

        public ConfirmViewModel(string b64Img, string origUrl)
        {
            Base64ImgData = b64Img;
            originalUrl = origUrl;
        }
    }
}