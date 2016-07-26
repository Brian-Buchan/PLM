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
                                       //|| m.Description.Contains(searchString)).ToList();
            }

            if(filterParam > 0)
            {
                modules = modules.Where(m => m.CategoryId == filterParam).ToList();
            }

            ViewBag.filterParam = filterParam;
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
             where p.CategoryId == 1 && p.isDisabled == false
             select p).Count();
            foreach (Module module in db.Modules)
            {
                if (module.Answers.Count() <= 5 && module.CategoryId == 1)
                {
                    try
                    {
                        Answer answer = module.Answers.ElementAt(0);
                        Picture picture = answer.Pictures.ElementAt(0);
                    }
                    catch
                    {
                        ViewBag.Cat1Count -= 1;
                    }
                }
            }
            
            ViewBag.Cat2Count =
                (from p in db.Modules
                 where p.CategoryId == 2 && p.isDisabled == false 
                 select p).Count();
            foreach (Module module in db.Modules)
            {
                if (module.Answers.Count() <= 5 && module.CategoryId == 2)
                {
                    try
                    {
                        Answer answer = module.Answers.ElementAt(0);
                        Picture picture = answer.Pictures.ElementAt(0);
                    }
                    catch
                    {
                        ViewBag.Cat2Count -= 1;
                    }
                }
            }
            ViewBag.Cat3Count =
                (from p in db.Modules
                 where p.CategoryId == 3 && p.isDisabled == false
                 select p).Count();
            foreach (Module module in db.Modules)
            {
                if (module.Answers.Count() <= 5 && module.CategoryId == 3)
                {
                    try
                    {
                        Answer answer = module.Answers.ElementAt(0);
                        Picture picture = answer.Pictures.ElementAt(0);
                    }
                    catch
                    {
                        ViewBag.Cat3Count -= 1;
                    }
                }
            }
            ViewBag.Cat4Count =
                (from p in db.Modules
                 where p.CategoryId == 4 && p.isDisabled == false// && p.Answers.ElementAt(0).Pictures.ElementAt(0).PictureID != null
                 select p).Count();
            foreach (Module module in db.Modules)
            {
                if (module.Answers.Count() <= 5 && module.CategoryId == 4)
                {
                    try
                    {
                        Answer answer = module.Answers.ElementAt(0);
                        Picture picture = answer.Pictures.ElementAt(0);
                    }
                    catch
                    {
                        ViewBag.Cat4Count -= 1;
                    }
                }
            }
            ViewBag.Cat5Count =
                (from p in db.Modules
                 where p.CategoryId == 5 && p.isDisabled == false
                 select p).Count();
            foreach (Module module in db.Modules)
            {
                if (module.Answers.Count() <= 5 && module.CategoryId == 5)
                {
                    try
                    {
                        Answer answer = module.Answers.ElementAt(0);
                        Picture picture = answer.Pictures.ElementAt(0);
                    }
                    catch
                    {
                        ViewBag.Cat5Count -= 1;
                    }
                }
            }
            ViewBag.Cat6Count =
                (from p in db.Modules
                 where p.CategoryId == 6 && p.isDisabled == false
                 select p).Count();
            foreach (Module module in db.Modules)
            {
                if (module.Answers.Count() <= 5 && module.CategoryId == 6)
                {
                    try
                    {
                        Answer answer = module.Answers.ElementAt(0);
                        Picture picture = answer.Pictures.ElementAt(0);
                    }
                    catch
                    {
                        ViewBag.Cat6Count -= 1;
                    }
                }
            }
            ViewBag.Cat7Count =
                (from p in db.Modules
                 where p.CategoryId == 7 && p.isDisabled == false
                 select p).Count();
            foreach (Module module in db.Modules)
            {
                if (module.Answers.Count() <= 5 && module.CategoryId == 7)
                {
                    try
                    {
                        Answer answer = module.Answers.ElementAt(0);
                        Picture picture = answer.Pictures.ElementAt(0);
                    }
                    catch
                    {
                        ViewBag.Cat7Count -= 1;
                    }
                }
            }
            ViewBag.Cat8Count =
                (from p in db.Modules
                 where p.CategoryId == 8 && p.isDisabled == false
                 select p).Count();
            foreach (Module module in db.Modules)
            {
                if (module.Answers.Count() <= 5 && module.CategoryId == 8)
                {
                    try
                    {
                        Answer answer = module.Answers.ElementAt(0);
                        Picture picture = answer.Pictures.ElementAt(0);
                    }
                    catch
                    {
                        ViewBag.Cat8Count -= 1;
                    }
                }
            }
           ViewBag.Cat9Count = 
                (from p in db.Modules
                 where p.CategoryId == 9 && p.isDisabled == false
             select p).Count();
           foreach (Module module in db.Modules)
           {
               if (module.Answers.Count() <= 5 && module.CategoryId == 9)
               {
                   try
                   {
                       Answer answer = module.Answers.ElementAt(0);
                       Picture picture = answer.Pictures.ElementAt(0);
                   }
                   catch
                   {
                       ViewBag.Cat9Count -= 1;
                   }
               }
           }
            ViewBag.Cat10Count = 
                (from p in db.Modules
                 where p.CategoryId == 10 && p.isDisabled == false
             select p).Count();
            foreach (Module module in db.Modules)
            {
                if (module.Answers.Count() <= 5 && module.CategoryId == 10)
                {
                    try
                    {
                        Answer answer = module.Answers.ElementAt(0);
                        Picture picture = answer.Pictures.ElementAt(0);
                    }
                    catch
                    {
                        ViewBag.Cat10Count -= 1;
                    }
                }
            }
            ViewBag.Cat11Count =
                (from p in db.Modules
                 where p.CategoryId == 11 && p.isDisabled == false
                 select p).Count();
            foreach (Module module in db.Modules)
            {
                if (module.Answers.Count() <= 5 && module.CategoryId == 11)
                {
                    try
                    {
                        Answer answer = module.Answers.ElementAt(0);
                        Picture picture = answer.Pictures.ElementAt(0);
                    }
                    catch
                    {
                        ViewBag.Cat11Count -= 1;
                    }
                }
            }

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