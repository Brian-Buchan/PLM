using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PLM;
using PLM.CutomAttributes;
//TODO: COPY OVER - Copy Views Categories Folder
namespace PLM.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult Index()
        {
            IEnumerable<Category> cats;
            using (Repos repo = new Repos())
            {
                cats = repo.GetCategoryList();
            }

            return View(cats);
        }

        // GET: Categories/Details/5
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int ID = id ?? 0;
            Category category;
            using (Repos repo = new Repos())
            {
                category = repo.GetCategoryByID(ID);
            }
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (Repos repo = new Repos())
                    {
                        repo.AddCategory(category);
                    }
                    return RedirectToAction("Index");
                }
                return View(category);
            }
            catch (Exception) { }

            //TODO - error when adding new category
            //return RedirectToAction("Create", new { error = "You cannot add duplicate answers" });
            return RedirectToAction("Create");
        }

        // GET: Categories/Edit/5
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int ID = id ?? 0;
            Category category;
            using (Repos repo = new Repos())
            {
                category = repo.GetCategoryByID(ID);
            }
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (Repos repo = new Repos())
                    {
                        repo.AddCategory(category);
                    }
                    return View(category);
                }
            }
            catch (Exception) { }
            return RedirectToAction("Index");
        }

        // GET: Categories/Delete/5
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            int ID = id ?? 0;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category;
            using (Repos repo = new Repos())
            {
                category = repo.GetCategoryByID(ID);
            }
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category;
            using (Repos repo = new Repos())
            {
                category = repo.GetCategoryByID(id);
                repo.DeleteCategory(category.CategoryID);
            }
            return RedirectToAction("Index");
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
