using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public static class PicToPicView
    {
        public static PLM.Models.PictureToView Convert(Picture picture)
        {
            PLM.Models.PictureToView pic = new PLM.Models.PictureToView(picture.PictureData, picture.Attribution);
            return pic;
        }
    }
}