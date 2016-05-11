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