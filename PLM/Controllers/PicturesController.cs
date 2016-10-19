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
using System.Drawing;
using System.Drawing.Imaging;
using PLM.Models;

namespace PLM.Controllers
{
    public class PicturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Picture pictureToSave;
        
        public ActionResult Index()
        {
            //ConvertAllPicturesToStringData();
            var pictures = db.Pictures.Include(p => p.Answer);
            return View(pictures.ToList());
        }
        
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

        public ActionResult PictureView(PictureToView pic)
        {
            return View(pic);
        }

        public ActionResult EditPictureView(int? id)
        {
            Picture picture = db.Pictures.Find(id);
            return View(picture);
        }

        //Convert all pictures saved in file
        //system to image data in database
        //public void ConvertAllPicturesToStringData()
        //{
        //    Image image;
        //    Picture picToChange;
        //    var pictures = db.Pictures.Where(p => p.PictureData == null).ToList();

        //    foreach (Picture pic in pictures)
        //    {
        //        picToChange = db.Pictures.Find(pic.PictureID);
        //        image = Image.FromFile("C:\\" + pic.Location.Replace("/", "\\").ToString());

        //        MemoryStream ms = new MemoryStream();
        //        image.Save(ms, ImageFormat.Png);
        //        byte[] imgArr = ms.ToArray();

        //        picToChange.PictureData = Convert.ToBase64String(imgArr);

        //        db.SaveChanges();
        //    }
        //}

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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Create([Bind(Include = "Attribution,PictureID")] Picture picture, int? id)
        {
            pictureToSave = new Picture();
            pictureToSave.AnswerID = (int)id;
            pictureToSave.Location = "NotYetConstructed";
            if (picture.Attribution == null)
                pictureToSave.Attribution = "";
            else
                pictureToSave.Attribution = picture.Attribution;

            ConvertImageToDataString(pictureToSave, Request.Files[0].InputStream);
            ViewBag.AnswerID = id;

            if (ModelState.IsValid)
            {
                db.Pictures.Add(pictureToSave);
                db.SaveChanges();
                return RedirectToAction("edit", new { controller = "Answers", id = pictureToSave.AnswerID });
            }

            ViewBag.AnswerID = new SelectList(db.Answers, "AnswerID", "AnswerString", pictureToSave.AnswerID);
            return View(pictureToSave);
        }

        [NonAction]
        public static void ConvertImageToDataString(Picture pictureToSave, Stream stream)
        {
            ImageConverter IC = new ImageConverter();
            Image image = Image.FromStream(stream);
            image = PictureResizer.ScaleImage(image, 600, 400);
            pictureToSave.PictureData = Convert.ToBase64String(PictureResizer.GetByteArrayFromImage(image));
        }

        public ActionResult InvalidImage(int id)
        {
            ViewBag.AnswerID = id;
            return View();
        }

        public ActionResult FileToLarge(int id)
        {
            ViewBag.AnswerID = id;
            return View();
        }

        public ActionResult UploadError(int id)
        {
            ViewBag.AnswerID = id;
            return View();
        }
        
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Edit([Bind(Include = "PictureID,Location,AnswerID,Attribution")] Picture picture)
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
            return RedirectToAction("edit", new { controller = "Answers", id = picture.AnswerID });
        }

        #region From Image Editor
        [HttpGet]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult ImageEditor(int? id)
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

        [HttpPost]
        [ActionName("ImageEditor")]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult ImageEditorPOST()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Confirm()
        {
            ConfirmViewModel model = (ConfirmViewModel)TempData["model"];

            //Disallows using back button to return to page after saving or discarding. Does not permit caching.
            //Taken from Kornel at http://stackoverflow.com/questions/49547/making-sure-a-web-page-is-not-cached-across-all-browsers
            Response.Cache.SetCacheability(HttpCacheability.NoCache);  // HTTP 1.1.
            Response.Cache.AppendCacheExtension("no-store, must-revalidate");
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "0"); // Proxies.

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        [ActionName("Confirm")]
        public ActionResult ConfirmPOST()
        {
            string newImgData = Request.Form.Get("newImgData");
            string originalImgData = Request.Form.Get("originalImgData");
            string imgID = Request.Form.Get("imageId");
            string answerID = Request.Form.Get("answerId");
            ConfirmViewModel model = new ConfirmViewModel(newImgData, originalImgData, imgID, answerID);
            TempData["model"] = model;
            return RedirectToAction("Confirm");
        }

        [HttpPost]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Save()
        {
            string newImgData = Request.Form.Get("newImgData");
            string answerId = Request.Form.Get("answerID");
            string imageID = Request.Form.Get("imageID");

            newImgData = newImgData.Replace("data:image/png;base64,", "");
            newImgData = newImgData.Replace("data:image/jpeg;base64,", "");

            Picture picture = db.Pictures.Find(int.Parse(imageID));
            picture.PictureData = newImgData;
            db.Entry(picture).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Edit", "Answers", new { id = answerId });
        }

        [HttpPost]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Discard()
        {
            string ansId = Request.Form.Get("answerID");
            return RedirectToAction("Edit", "Answers", new { id = ansId });
        }
        #endregion

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