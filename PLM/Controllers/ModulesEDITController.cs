using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PLM;

namespace PLM.Controllers
{
    public class ModulesEDITController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /ModulesEDIT/
        public ActionResult Index()
        {
            return View(db.Modules.ToList());
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ModulesEDIT/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ModuleID,Name,CategoryId,Description,DefaultNumAnswers,DefaultTime,DefaultNumPictures,isPrivate")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Index", new { controller = "Profile"});
            }

            return View(module);
        }

        // GET: /ModulesEDIT/Edit/5
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
            return View(module);
        }

        // POST: /ModulesEDIT/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ModuleID,Name,CategoryId,Description,DefaultNumAnswers,DefaultTime,DefaultNumPictures,isPrivate")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { controller = "Profile" });
            }
            return View(module);
        }

        // GET: /ModulesEDIT/Delete/5
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
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();
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
