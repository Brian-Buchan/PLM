using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;

namespace PLM.Controllers
{
    public class ImageEditorController : Controller
    {
        // GET: ImageEditor
        [HttpGet]
        public ActionResult ImageEditor()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ImageEditor")]
        public ActionResult ImageEditorPOST()
        {
            string imageBase64 = Request.Form.Get("imgData");
            imageBase64 = imageBase64.Substring(imageBase64.LastIndexOf(',') + 1);
            byte[] img = Convert.FromBase64String(imageBase64);
            using (MemoryStream ms = new MemoryStream(img,0,img.Length))
            {
                Image image = Image.FromStream(ms, true);
                //image.Save("~/~")
            }
            return View();
        }
    }
}