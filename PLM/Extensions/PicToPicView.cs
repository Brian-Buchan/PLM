using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public static class PicToPicView
    {
        public static Models.PictureToView Convert(Picture picture)
        {
            Models.PictureToView pic;
            if (picture.Attribution == null)
            {
                pic = new Models.PictureToView(picture.PictureData);
            }
            else
            {
                pic = new Models.PictureToView(picture.PictureData, picture.Attribution);
            }
            return pic;
        }
    }
}