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
        public ActionResult Create(int ID)
        {
            ViewBag.ModuleID = ID;

            var modules = db.Modules.ToList();

            ViewBag.ModuleName = modules.Find(x => x.ModuleID == ID).Name;

            var answers = db.Answers.ToList();
            ViewBag.ModuleAnsList = (from a in answers
                                     where a.ModuleID == ID
                                     select a).ToList();
            //ViewBag.ModuleID = new SelectList(db.Modules, "ModuleID", "Name");
            return View();
        }

        // POST: /Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnswerID,AnswerString,ModuleID")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Answers.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Create", new {id = answer.ModuleID });
            }

            ViewBag.ModuleID = new SelectList(db.Modules, "ModuleID", "Name");
            return View(answer);
        }

        // GET: /Answers/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ModuleID = new SelectList(db.Modules, "ModuleID", "Name");
            return View(answer);
        }

        // POST: /Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnswerID,AnswerString,ModuleID")] Answer answer, int? ModuleID)
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

        [HttpPost]
        public ActionResult UpdateImage()
        {
            //Image Editor post string format:
            //If the image is saved as a jpeg, the post results in: "data:image/jpeg;base64,[IMAGEDATA]", 
            //where "[IMAGEDATA]" is a base64 string that converts to a jpeg image.
            //Otherwise, if the image is saved as a png, the post results in: "data:image/png;base64,[IMAGEDATA]",
            //where "[IMAGEDATA]" is a base64 string that converts to a png image.

            string imgId = Request.Form.Get("imgId");

            string result = SaveImage(Request.Form.Get("imgData"), imgId);

            if (result == "FAILED")
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                        "Something went wrong with your request. \nContact an administrator.");
            }
            else if (result == "TOO LARGE")
            {
                return new HttpStatusCodeResult(HttpStatusCode.RequestEntityTooLarge,
                "Image file size larger than 200 KB. \nTry lowering the quality when you save," +
                " \nor resize the image to a smaller size.");
            }

            //return View();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult Save()
        {
            bool willSave;
            string valueFromPost = Request.Form.Get("willSave");

            //try and parse the value sent from the form
            if (Boolean.TryParse(valueFromPost, out willSave))
            {
                if (willSave)
                {
                    string result = PermaSave(HttpContext.Session.SessionID);

                    switch (result)
                    {
                        case "NO FILES":
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest,
                                "There are no files to save.");

                        case "SAVED":
                            return new HttpStatusCodeResult(HttpStatusCode.OK,
                                "Files Saved. Refreshing page.");

                        case "FAILED":
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                                "There was an error processing your request. \nContact an administrator.");
                        default:
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                                "Something went wrong, and we're not sure what. \nContact an administrator immediately.");
                    }
                }
                else
                {
                    string result = DiscardChanges(HttpContext.Session.SessionID);

                    switch (result)
                    {
                        case "DONE":
                            return new HttpStatusCodeResult(HttpStatusCode.OK,
                                "Changes Discarded. Refreshing page.");
                        case "ERROR":
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                                "There was an error processing your request. \nContact an administrator.");

                        default:
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                                "Something went wrong, and we're not sure what. \nContact an administrator immediately.");
                    }
                }
            }
            else return new HttpStatusCodeResult(HttpStatusCode.BadRequest,
                "There was an error processing your request. \nContact an administrator.");
        }
        
        // GET: /Answers/Delete/5
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
        public ActionResult DeleteConfirmed(int id)
        {
            Answer answer = db.Answers.Find(id);

            CascadeDeleter.DeleteAnswer(id);
            return RedirectToAction("edit", new { controller = "ModulesEdit", id = answer.ModuleID});
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
        /// Saves an image from data obtained from a POST.
        /// Returns the filepath if it succeeds.
        /// Returns "FAILED" if there is an exception, or the specified file format isn't supported.
        /// Returns "TOO LARGE" if the image size is greater that 200000 bytes (200KB).
        /// </summary>
        /// <param name="fromPost">The POST data in the following format: 
        /// "data:image/[FILEEXTENSION];base64,[IMAGEDATA]",
        /// where [FILEEXTENSION] is either "jpeg" or "png", and 
        /// [IMAGEDATA] is an image in Base64 encoding.</param>
        /// <param name="id">The id of the image that was edited. 
        /// Will be used to discriminate which image to overwrite.</param>
        /// <returns>string</returns>
        [NonAction]
        private string SaveImage(string fromPost, string id)
        {
            try
            {
                string dirPath = (Path.Combine(Server.MapPath("~/Content/Images/tempUploads/")));

                //gets the post data
                string imageBase64 = fromPost;

                //gets the image format from the post
                string imageFormat = imageBase64.Substring(imageBase64.IndexOf('/') + 1, imageBase64.IndexOf(';') - 11);

                //gets the file data as a Base64 string
                imageBase64 = imageBase64.Substring(imageBase64.LastIndexOf(',') + 1);

                //converts the file data to a byte array
                byte[] img = Convert.FromBase64String(imageBase64);

                //if the image is greater than 200KB, reject it and return a response.
                if (img.Length > 200000)
                {
                    return "TOO LARGE";
                }

                //sets up the filename, guid part taken from Mark Synowiec at http://stackoverflow.com/questions/730268/unique-random-string-generation
                Guid g = Guid.NewGuid();
                string TempFileName = Convert.ToBase64String(g.ToByteArray());
                //replace invalid characters with valid ones.
                TempFileName = TempFileName.Replace("=", "");
                TempFileName = TempFileName.Replace("+", "");
                TempFileName = TempFileName.Replace(@"/", "");

                //add the image ID, with a discriminating exclamation point (!)
                TempFileName = TempFileName + "!" + id;

                //add the user's sessionID, with a discriminating caret (^)
                TempFileName = HttpContext.Session.SessionID + "^" + TempFileName;

                //add the file extension
                TempFileName = TempFileName + "." + imageFormat;

                using (MemoryStream ms = new MemoryStream(img, 0, img.Length))
                {
                    Image image;
                    image = Image.FromStream(ms, true);

                    if (imageFormat == "jpeg")
                    {
                        image.Save(dirPath + TempFileName, ImageFormat.Jpeg);
                        image.Dispose();
                    }
                    else if (imageFormat == "png")
                    {
                        image.Save(dirPath + TempFileName, ImageFormat.Png);
                        image.Dispose();
                    }
                    else
                    {
                        return "FAILED";
                    }
                }
                return dirPath + TempFileName;
            }
            catch (Exception ex)
            {
                return "FAILED";
            }
        }

        /// <summary>
        /// Permanently save files in the tempUpload folder that contain 
        /// the given session ID to the permUploads folder.
        /// Returns "SAVED" if successful, 
        /// "NO FILES" if there were no files found, 
        /// or "FAILED" otherwise.
        /// </summary>
        /// <param name="sessionId">The session ID of the user.</param>
        /// <returns>string</returns>
        [NonAction]
        private string PermaSave(string sessionId)
        {
            string dirPath = (Path.Combine(Server.MapPath("~/Content/Images/tempUploads/")));
            string newDirPath = (Path.Combine(Server.MapPath("~/Content/Images/permUploads/")));

            string[] filesToSave = Directory.GetFiles(dirPath, "*" + sessionId + "*");

            if (filesToSave.Length == 0)
            {
                return "NO FILES";
            }

            if (FileManipExtensions.MoveSpecificFiles(filesToSave, newDirPath, true))
            {
                return "SAVED";
            }
            else return "FAILED";
        }

        /// <summary>
        /// Discard all the images in the temp folder with the given sessionID.
        /// Returns "DONE" if successful, "ERROR" if the deletion fails at any point.
        /// </summary>
        /// <param name="sessionId">The sessionID to use when deleting files</param>
        /// <returns>string</returns>
        [NonAction]
        private string DiscardChanges(string sessionId)
        {
            string dirPath = (Path.Combine(Server.MapPath("~/Content/Images/tempUploads/")));
            string[] filesToDiscard = Directory.GetFiles(dirPath, "*" + sessionId + "*");
            if (FileManipExtensions.DeleteSpecificFiles(filesToDiscard))
            {
                return "DONE";
            }
            else return "ERROR";
        }
    }
}
