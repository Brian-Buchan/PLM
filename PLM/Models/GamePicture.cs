using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLM.Models
{
    public class GamePicture
    {
        public string PictureData { get; set; }
        public string Attribution { get; set; }
        public bool Usable { get; set; }
        
        public GamePicture(Picture picture)
        {
            Usable = true;
            PictureData = picture.PictureData;
            Attribution = picture.Attribution;
        }

        public GamePicture()
        {

        }
    }
}