using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PLM;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using PLM.CutomAttributes;

namespace PLM.Controllers
{
    public class AnswersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Answers/
        public ActionResult Index()
        {
            var answers = db.Answers.Include(a => a.Module);
            return View(answers.ToList());
        }

        // GET: /Answers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: /Answers/Create
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Create(int ID, string error)
        {
            try
            {
                ViewBag.ModuleID = ID;

                var modules = db.Modules.ToList();

                ViewBag.ModuleName = modules.Find(x => x.ModuleID == ID).Name;

                var answers = db.Answers.ToList();

                ViewBag.ModuleAnsList = (from a in answers
                                         where a.ModuleID == ID
                                         select a).ToList();

            }
            catch (Exception)
            {
                ViewBag.Error = "You cannot add duplicate answers";
            }
            ViewBag.Error = error;
            return View();
        }

        // POST: /Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Create([Bind(Include = "AnswerID,AnswerString,ModuleID,PictureCount")] Answer answer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Answers.Add(answer);
                    db.SaveChanges();
                    return RedirectToAction("Create", new { id = answer.ModuleID });
                }
            }
            catch (Exception)
            {

            }
            //answer.Module = db.Modules.Find(answer.ModuleID);
            return RedirectToAction("Create", new { error = "You cannot add duplicate answers" });
        }

        // GET: /Answers/Edit/5
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Edit(int? id)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "-1"); // Proxies.
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModuleID = new SelectList(db.Modules, "ModuleID", "Name");
            return View(answer);
        }

        // POST: /Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Edit([Bind(Include = "AnswerID,AnswerString,ModuleID,PictureCount")] Answer answer, int? ModuleID)
        {
            if (ModelState.IsValid)
            {

                db.Entry(answer).State = EntityState.Modified;
                if (ModuleID != null)
                {
                    answer.ModuleID = (int)ModuleID;
                }
                db.SaveChanges();
                return RedirectToAction("Create", new { controller = "Answers", id = answer.ModuleID });
            }
            ViewBag.ModuleID = new SelectList(db.Modules, "ModuleID", "Name");
            return View(answer);
        }



        // GET: /Answers/Delete/5
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: /Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer answer = db.Answers.Find(id);

            DirectoryHandler.DeleteAnswer(id);
            return RedirectToAction("Create", new { id = answer.ModuleID });
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
