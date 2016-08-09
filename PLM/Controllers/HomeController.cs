using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.AspNet.Identity;
using PLM.CutomAttributes;

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
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult FreeUseImages()
        {
            return View();
        }
    }
}