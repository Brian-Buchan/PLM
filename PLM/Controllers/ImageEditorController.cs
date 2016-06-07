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
        public ActionResult ImageEditor(string imageBase64)
        {
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