using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLM.Models
{
    public class PictureToView
    {
        public string PictureData { get; set; }
        public string Attribution { get; set; }
        
        public PictureToView(string pictureData, string attribution)
        {
            PictureData = pictureData;
            Attribution = attribution;
        }

        public PictureToView(string pictureData)
        {
            PictureData = pictureData;
            Attribution = "";
        }

        public PictureToView()
        {

        }
    }
}