using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace PLM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FAQ()
        {
            return View();
        }
        //public ActionResult FileUpload(HttpPostedFileBase file)
        //{
        //    if (file != null)
        //    {
        //        string pic = System.IO.Path.GetFileName(file.FileName);
        //        string path = System.IO.Path.Combine(
        //                               Server.MapPath("~/Content/Images/Profile"), pic);
        //        // file is uploaded
        //        file.SaveAs(path);
                
        //        // save the image path path to the database or you can send image 
        //        // directly to database
        //        // in-case if you want to store byte[] ie. for DB
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            file.InputStream.CopyTo(ms);
        //            byte[] array = ms.GetBuffer();
        //        }
        //        return RedirectToAction("Index", "Profile", new { imageurl = path });
        //    }
        //    return View();
        //}
        public ActionResult AboutUs()
        {
            ViewBag.Message = "Learning Without Thinking";

            return View();
        }

        public ActionResult ContactUs()
        {
            ViewBag.Message = "Please feel free to contact me!";

            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.Message = "Please feel free to contact me!";

            return View();
        }
    }
}