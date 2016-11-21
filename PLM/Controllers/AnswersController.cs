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
        public ActionResult Index(int id = 0)
        {
            IEnumerable<Answer> answers;
            using (Repos repos = new Repos())
            {
                answers = repos.GetAnswerList(id);
            }
            return View(answers);
        }
        // GET: /Answers/Details/5
        public ActionResult Details(int? id)
        {
            int ID = id ?? 0;
            Answer answer;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                using (Repos repo = new Repos())
                {
                    answer = repo.GetAnswerByID(ID);
                }
                if (answer == null)
                {
                    return HttpNotFound();
                }
                return View(answer);
            }
        }

        // GET: /Answers/Create
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Create(int ID, string error)
        {
            Module module;
            try
            {
                using (Repos repo = new Repos())
                {
                    module = repo.GetModuleByID(ID);
                    ViewBag.ModuleID = module.ModuleID;
                    ViewBag.ModuleName = module.Name;
                    ViewBag.ModuleAnsList = repo.GetAnswerList(module.ModuleID);
                }
            }
            catch (Exception)
            {
                ViewBag.Error = error;
            }
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
                    using (Repos repo = new Repos())
                    {
                        repo.AddAnswer(answer);
                    }
                    return RedirectToAction("Create", new { id = answer.ModuleID });
                }
            }
            catch (Exception) { }
            return RedirectToAction("Create", new { error = "You cannot add duplicate answers" });
        }

        // GET: /Answers/Edit/5
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Edit(int? id)
        {
            int ID = id ?? 0;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "-1"); // Proxies.
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer;
            using (Repos repo = new Repos())
            {
                answer = repo.GetAnswerByID(ID);
                ViewBag.Pictures = repo.GetViewBagPictureList(answer.AnswerID);
            }
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
                using (Repos repo = new Repos())
                {
                    repo.UpdateAnswer(answer);
                    ViewBag.Pictures = repo.GetViewBagPictureList(answer.AnswerID);
                }
                
                return RedirectToAction("Create", new { controller = "Answers", id = answer.ModuleID });
            }
            ViewBag.ModuleID = new SelectList(db.Modules, "ModuleID", "Name");
            return View(answer);
        }
        // GET: /Answers/Delete/5
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Delete(int? id)
        {
            int ID = id ?? 0;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer;
            using (Repos repo = new Repos())
            {
                answer = repo.GetAnswerByID(ID);
            }
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
            Answer answer;
            using (Repos repo = new Repos())
            {
                answer = repo.GetAnswerByID(id);
                repo.DeleteAnswer(answer.AnswerID);
            }
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