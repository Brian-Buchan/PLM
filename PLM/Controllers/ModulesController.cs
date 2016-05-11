using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLM.Controllers
{
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ModuleViewModel ModuleModel = new ModuleViewModel();
        // GET: Profile
        public ActionResult Index()
        {
            ModuleModel.Cats = db.Categories.ToList();
            ModuleModel.Mods = db.Modules.ToList();
            //var query = (db.Modules
            //            .GroupBy(p => new
            //            {
            //                p.CategoryId
            //            })
            //            .Select(g => new
            //            {
            //                g.Key.CategoryId,
            //                Available = g.Count()
            //            }));
            //foreach(var x in ModuleModel.Cats)
            ViewBag.Cat1Count = 
                (from p in db.Modules
             where p.CategoryId == 1
             select p).Count();
            ViewBag.Cat2Count =
                (from p in db.Modules
                 where p.CategoryId == 2
                 select p).Count();
            ViewBag.Cat3Count =
                (from p in db.Modules
                 where p.CategoryId == 3
                 select p).Count();
            ViewBag.Cat4Count =
                (from p in db.Modules
                 where p.CategoryId == 4
                 select p).Count();
            ViewBag.Cat5Count =
                (from p in db.Modules
                 where p.CategoryId == 5
                 select p).Count();
            ViewBag.Cat6Count =
                (from p in db.Modules
                 where p.CategoryId == 6
                 select p).Count();
            ViewBag.Cat7Count =
                (from p in db.Modules
                 where p.CategoryId == 7
                 select p).Count();
            ViewBag.Cat8Count =
                (from p in db.Modules
                 where p.CategoryId == 8
                 select p).Count();
           ViewBag.Cat9Count = 
                (from p in db.Modules
             where p.CategoryId == 9
             select p).Count();
            ViewBag.Cat10Count = 
                (from p in db.Modules
             where p.CategoryId == 10
             select p).Count();
            ViewBag.Cat11Count =
                (from p in db.Modules
                 where p.CategoryId == 11
                 select p).Count();

            return View(ModuleModel);
        }

        public ActionResult AddModule()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddModule(FormCollection form)
        {
            if (form["operation"] == "Add")
            {
                return Redirect("~/Modules/AddImage");
            }

            if (form["operation"] == "Edit")
            {
                return Redirect("~/Modules/EditImage");
            }

            if (form["operation"] == "Remove")
            {
                return Redirect("~/Modules/RemoveImage");
            }

            if (form["operation"] == "Save")
            {
                return Redirect("~/Modules/SaveModule");
            }
            else
            {
                return Redirect("/Modules/AddModule");
            }
        }
    }
}