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
        public ActionResult TOU()
        {
            return View();
        }
                
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

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.Message = "Please feel free to contact me!";

            return View();
        }
    }
}