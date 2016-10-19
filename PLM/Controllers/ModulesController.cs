using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace PLM.Controllers
{
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ModuleViewModel ModuleModel = new ModuleViewModel();
      
        ///<summary>
        /// Counts the number of modules in a category that are usable and not disabled
        /// </summary>
        /// <param name="cat">Id of the category to count</param>
        public int categoryCount(int cat)//Method that counts the number of valid modules in a category
        {
            //int count = (from p in db.Modules
            //             where p.CategoryId == cat && p.isDisabled == false
            //             select p).Count();
            //foreach (Module module in db.Modules)
            //{
            //    if (module.Answers.Count() <= 5 && module.CategoryId == cat)
            //    {
            //        try
            //        {
            //            Answer answer = module.Answers.ElementAt(0);
            //            Picture picture = answer.Pictures.ElementAt(0);
            //        }
            //        catch
            //        {
            //            count -= 1;
            //        }
            //    }
            //}
            int count = 0;
            return (count);
        }  
        //GET: Profile
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? filterParam, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var modules = db.Modules.ToList();
            modules = (from m in modules
                            where m.isPrivate == false && m.isDisabled == false 
                            select m).ToList();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString.ToLower();
                modules = modules.Where(m => m.Name.ToLower().Contains(searchString)).ToList();
            }
            if (filterParam > 0)
            {
                modules = modules.Where(m => m.CategoryId == filterParam).ToList();
            }
            ViewBag.filterParam = filterParam;
            //Call the category count function to get category coutns
            ViewBag.Cat1Count = categoryCount(1);
            ViewBag.Cat2Count = categoryCount(2);
            ViewBag.Cat3Count = categoryCount(3);
            ViewBag.Cat4Count = categoryCount(4);
            ViewBag.Cat5Count = categoryCount(5);
            ViewBag.Cat6Count = categoryCount(6);
            ViewBag.Cat7Count = categoryCount(7);
            ViewBag.Cat8Count = categoryCount(8);
            ViewBag.Cat9Count = categoryCount(9);
            ViewBag.Cat10Count = categoryCount(10);
            ViewBag.Cat11Count = categoryCount(11);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(modules.ToPagedList(pageNumber, pageSize));
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