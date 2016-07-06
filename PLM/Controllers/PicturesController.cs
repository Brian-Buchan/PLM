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
using PLM.Extensions;
using PLM.CutomAttributes;

namespace PLM.Controllers
{
    public class PicturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Pictures/
        public ActionResult Index()
        {
            var pictures = db.Pictures.Include(p => p.Answer);
            return View(pictures.ToList());
        }

        // GET: /Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: /Pictures/Create
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Create(int? id)
        {
            ViewBag.AnswerID = id;
            Picture picture = new Picture();
            picture.Answer = db.Answers
                    .Where(a => a.AnswerID == id)
                    .ToList().First();

            return View(picture);
        }

        // POST: /Pictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Create([Bind(Include = "Attribution,PictureID")] Picture picture, int? id) 
        {
            ViewBag.AnswerID = id;
            if (ModelState.IsValid)
            {
                //db.Entry(picture).State = EntityState.Modified;
                picture = new Picture();
                picture.Answer = db.Answers
                    .Where(a => a.AnswerID == id)
                    .ToList().First();
                //picture.Attribution = attribution;

                picture.AnswerID = (int)id;

                picture.Location = "";
                db.Pictures.Add(picture);

                var location = SaveUploadedFile(picture);
         
                if (location == "")
                {
                    //error
                }
                else
                {
                    picture.Location = location;
                }

                db.SaveChanges();
                return RedirectToAction("edit", new { controller = "Answers", id = picture.AnswerID });
            }

            ViewBag.AnswerID = new SelectList(db.Answers, "AnswerID", "AnswerString", picture.AnswerID);
            return View(picture);

        }

        // GET: /Pictures/Edit/5
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnswerID = new SelectList(db.Answers, "AnswerID", "AnswerString", picture.AnswerID);
            return View(picture);
        }

        // POST: /Pictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Edit([Bind(Include = "PictureID,Location,AnswerID")] Picture picture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(picture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("edit", new { controller = "Answers", id = picture.AnswerID });
            }
            ViewBag.AnswerID = new SelectList(db.Answers, "AnswerID", "AnswerString", picture.AnswerID);
            return View(picture);
        }

        // GET: /Pictures/Delete/5
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }

            
            return View(picture);
        }

        // POST: /Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Pictures.Find(id);
            db.Pictures.Remove(picture);
            db.SaveChanges();
            System.IO.File.Delete(picture.Location);
            return RedirectToAction("edit", new { controller = "Answers", id = picture.AnswerID});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Save a picture to the server. Returns the relative path if successful, otherwise returns "FAILED"
        /// </summary>
        /// <param name="picture">The picture object to be saved</param>
        /// <returns>string</returns>
        public string SaveUploadedFile(Picture picture)
        {
            Session["upload"] = picture.Answer.Module.Name;
            bool isSavedSuccessfully = true;
            string fName = "";
            string path = "";
            string relpath = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;
                    picture.Answer.PictureCount++;
                    if (file.ContentLength >= 10971520)
                    {
                        RedirectToAction("InvalidImage","Pictures");
                    }else if(!((file.ContentType=="image/jpeg")||(file.ContentType=="image/bmp")||(file.ContentType=="image/png"))){
                        RedirectToAction("InvalidImage","Pictures");
                    }
                    else
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string moduleDirectory = (DevPro.baseFileDirectory + "PLM/" + Session["upload"].ToString() + "/");
                            if (!Directory.Exists(moduleDirectory))
                            {
                                Directory.CreateDirectory(moduleDirectory);
                            }
                            path = moduleDirectory + fName;
                            // Saves the file through the HttpPostedFileBase class
                            file.SaveAs(path);
                            string filetype = Path.GetExtension(path);

                            // Then renames that image to the correct name based off the answer
                            // And number of picturs per answer, then deletes the old picture
                            string newfName = (picture.Answer.AnswerString + "-" + picture.Answer.PictureCount.ToString() + filetype);
                            relpath = (moduleDirectory + newfName);
                            System.IO.File.Copy(path, relpath);
                            System.IO.File.Delete(path);

                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
                
            }

            if (isSavedSuccessfully)
            {
                return relpath;
            }
            else
            {
                return "FAILED";
            }
        }

        public ActionResult DropzoneTest()
        {
            return View();
        }
    }
}
