using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PLM;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Infrastructure;
using PLM.CutomAttributes;

namespace PLM.Controllers
{

    public class ModulesEDITController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ModulesEDIT/
       [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string searchString, string userSearchString)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";

            var db = new ApplicationDbContext();
            var modules = from u in db.Modules
                          select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                modules = modules.Where(m => m.Name.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(userSearchString))
            {
                modules = modules.Where(m => m.User.UserName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_asc":
                    modules = modules.OrderBy(m => m.Name);
                    break;
            }

            return View(modules);
        }

        public ActionResult ProfanityCheck()
        {
            var modules = from m in db.Modules
                          select m;

            return View(modules);
        }

        // GET: /ModulesEDIT/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: /ModulesEDIT/Create
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Create()
        {
            PopulateCategoryDropDownList();
            return View();
        }

        // POST: /ModulesEDIT/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Create([Bind(Include = "ModuleID,Name,CategoryId,Description,DefaultNumAnswers,DefaultTime,DefaultNumQuestions,isPrivate")] Module module)
        {
            if (ModelState.IsValid)
            {
                //**********NOTE****************//
                //Make sure that a user is logged in to access this page
                if (((User.IsInRole("Learner")) || (User.IsInRole("Admin"))))
                {
                    var userID = User.Identity.GetUserId();
                    ApplicationUser currentUser = (ApplicationUser)db.Users.Single(x => x.Id == userID);
                    module.User = currentUser;
                }

                db.Modules.Add(module);
                db.SaveChanges();
                PopulateCategoryDropDownList(module.CategoryId);
                if (module.CategoryId != null) { return RedirectToAction("Create", "Answers", new { id = module.ModuleID }); }
                else{return RedirectToAction("Create", "ModulesEDIT");}

            }
            return View(module);
        }

        // GET: /ModulesEDIT/Edit/5
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            PopulateCategoryDropDownList(module.CategoryId);
            return View(module);
        }

        // POST: /ModulesEDIT/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Edit([Bind(Include = "ModuleID,Name,CategoryId,Description,DefaultNumAnswers,DefaultTime,DefaultNumQuestions,isPrivate")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { controller = "Profile" });
            }
            PopulateCategoryDropDownList(module.CategoryId);
            return View(module);
        }

        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult DisableModule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            var model = new DisableModuleViewModel(module);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: /ModulesEDIT/ModuleDisable/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult DisableModule([Bind(Include = "Name, isDisabled, DisableModuleNote, DisableReason")] DisableModuleViewModel userModule)
        {
            if (ModelState.IsValid)
            {
                var db = new ApplicationDbContext();
                Module module = db.Modules.First(m => m.Name == userModule.Name);

                module.isDisabled = userModule.isDisabled;
                module.DisableModuleNote = userModule.DisableModuleNote;
                module.DisableReason = userModule.DisableReason;

                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { controller = "ModulesEdit" });
            }
            return View(userModule);
        }

        private void PopulateCategoryDropDownList(object selectedCategory = null)
        {
            var categoryQuery = from c in db.Categories
                                select c;
            ViewBag.CategoryId = new SelectList(categoryQuery.Distinct().ToList(), "CategoryId", "CategoryName", selectedCategory);
        }

        // GET: /ModulesEDIT/Delete/5
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: /ModulesEDIT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult DeleteConfirmed(int id)
        {
            CascadeDeleter.DeleteModule(id);

            return RedirectToAction("Index", new { controller = "Profile" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
