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

        #region From Image Editor

        //Image Editor flow:
        //
        //User goes to the Image Editor (ImgEd) page
        //
        //AJAX call from ImgEd page saves image data
        //
        //User hits "Save" button, which is in fact a form submit button that POSTs data 
        //to the ConfirmPOST() action.
        //
        //This action redirects the user to the Confirm GET action after processing the nessecary data
        //
        //The user selects either "Save" or "Discard" on the Confirm page, 
        //which POSTs to either the Save() or Discard() actions, respectively
        //
        //Users are then returned to the Index page of the Home controller.


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
            //Image Editor post string format:
            //If the image is saved as a jpeg, the post results in: "data:image/jpeg;base64,[IMAGEDATA]", 
            //where "[IMAGEDATA]" is a base64 string that converts to a jpeg image.
            //Otherwise, if the image is saved as a png, the post results in: "data:image/png;base64,[IMAGEDATA]",
            //where "[IMAGEDATA]" is a base64 string that converts to a png image.

            string imgId = Request.Form.Get("imgId");
            string answerId = Request.Form.Get("answerId");

            string result = SaveImage(Request.Form.Get("imgData"), imgId, answerId);
            
            if (result == "FAILED")
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                        "Something went wrong with your request. \nContact an administrator.");
            }
            else if (result=="TOO LARGE")
            {
                return new HttpStatusCodeResult(HttpStatusCode.RequestEntityTooLarge, 
                "Image file size larger than 200 KB. \nTry lowering the quality when you save," + 
                " \nor resize the image to a smaller size.");
            }

            //return View();
            return new HttpStatusCodeResult(HttpStatusCode.OK, result);
        }

        [HttpGet]
        //[RequireHttps] //Ensures http-headers work
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
            string b64Img = Request.Form.Get("imgData");
            string origUrl = Request.Form.Get("origUrl");
            string imgID = Request.Form.Get("imgId");
            string answerID = Request.Form.Get("answerId");
            string tempUrl = Request.Form.Get("tempUrl");
            ConfirmViewModel model = new ConfirmViewModel(b64Img, origUrl, imgID, answerID, tempUrl);
            TempData["model"] = model;
            return RedirectToAction("Confirm");
        }

        [HttpPost]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Save()
        {
            string origUrl = Request.Form.Get("origUrl");
            origUrl = HttpUtility.HtmlDecode(origUrl);
            string tempUrl = Request.Form.Get("tempUrl");
            tempUrl = HttpUtility.HtmlDecode(tempUrl);
            string temporaryFileName = Path.GetFileName(tempUrl);
            //string newFileName = Path.GetFileNameWithoutExtension(origUrl);

            string result = PermaSave(temporaryFileName, origUrl);

            return RedirectToAction("Index", "Home", new {actResult = result });
        }

        [HttpPost]
        [AuthorizeOrRedirectAttribute(Roles = "Instructor")]
        public ActionResult Discard()
        {
            string tempUrl = Request.Form.Get("tempUrl");
            tempUrl = HttpUtility.HtmlDecode(tempUrl);
            //string noRedirect = Request.Form.Get("noRedirect");
            string temporaryFileName = Path.GetFileName(tempUrl);

            DiscardChanges(temporaryFileName);

            //if (noRedirect == Boolean.TrueString)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.OK);
            //}

            return RedirectToAction("Index", "Home");
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
        /// <param name="imgId">The id of the image that was edited. 
        /// Will be used to discriminate which image to overwrite.</param>
        /// <param name="answerId">The id of the answer. Used to select which answer the image belongs to.</param>
        /// <returns>string</returns>
        [NonAction]
        private string SaveImage(string fromPost, string imgId, string answerId)
        {
            try
            {
                string dirPath = (Path.Combine(Server.MapPath("~/Content/Images/tempUploads/")));

                if (!(Directory.Exists(dirPath)))
                {
                    Directory.CreateDirectory(dirPath);
                }

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

                //add the image ID, with discriminating curly braces ("{" and "}")
                TempFileName = "{" + imgId + "}" + TempFileName;

                //add the answerID, with discriminating brackets ("[" and "]")
                TempFileName = "[" + answerId + "]" + TempFileName;

                //add the file extension
                TempFileName = TempFileName + "." + imageFormat;

                //The filename for "answer 1 image 3", for example, would thus look like: 
                // "[1]{3}khsial3ihvbsliuygal.png"

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
        /// Permanently save the file with the given name in the tempUpload folder
        /// to a different folder, with a new name. Overwrites files with the same name that are already there.
        /// If there are multiple files found with the same name for whatever reason,
        /// takes the last one found.
        /// Returns "SAVED" if successful, 
        /// "BAD LOCATION" if the file could not be found within the temp folder 
        /// (possibly indicating that the temp folder is missing),
        /// "NO FILES FOUND" if GetFiles failed to match,
        /// "BAD MOVE ON RENAME" if TryRenameFile fails,
        /// "BAD MOVE DURING TRANSFER" if the filesMove array is nulled,
        /// or "FAILED ON FILE MOVE" otherwise.
        /// </summary>
        /// <param name="filename">The file to permanently save from the tempUpload folder. Expects only 
        /// the filename and its extension, not a path</param>
        /// <param name="toNewFilePath">The new name for the saved file, with the new path. 
        /// Expects the full path</param>
        /// <returns>string</returns>
        [NonAction]
        private string PermaSave(string filename, string toNewFilePath)
        {

            List<string> filesToMove = new List<string>();
            string dirPath = (Path.Combine(Server.MapPath("~/Content/Images/tempUploads/")));
            //string dirPath = DevPro.baseFileDirectory + "tempUploads";
            string newDirPath = Path.GetDirectoryName(Server.MapPath(toNewFilePath));
            //string newDirPath = (Path.Combine(Server.MapPath("~/Content/Images/permUploads/")));
            string newFileName = Path.GetFileNameWithoutExtension(toNewFilePath);
            string result = "";

            throw new ArgumentException(toNewFilePath + " " + newDirPath);

            return result;

            ////if the selected file doesn't exist in the temp folder
            //if (!(System.IO.File.Exists(dirPath + filename)))
            //{
            //    //If this code is reached, the passed in filename could not be accessed. 
            //    //It may have been moved, deleted, or did not exist in the first place.
            //    //The passed in filename may have also contained illegal characters, 
            //    //referenced a location on a failing/missing disk, 
            //    //or the program might not have read permissions for that specific file.
            //    result = "BAD LOCATION:" + dirPath + filename;
            //    return result;
            //}

            //string[] filesToSave = Directory.GetFiles(dirPath, filename);

            //if (filesToSave.Length == 0)
            //{
            //    //If this code is reached, GetFiles didn't find any matching files.
            //    return "NO FILES FOUND";
            //}

            ////for each file to be saved, 
            //foreach (string filePath in filesToSave)
            //{
            //    string newFilePath;
            //    if (FileManipExtensions.TryRenameFile(filePath, newFileName, out newFilePath, true))
            //    {
            //        filesToMove.Add(newFilePath);
            //    }
            //}

            ////Make sure that filepaths were actually moved to the filesToMove list.
            //if (filesToMove.Count == 0)
            //{
            //    //If this code is reached, there is a problem within the TryRenameFile method.
            //    //Another process may have been accessing the file at the time of the renaming.
            //    return "BAD MOVE ON RENAME";
            //}

            //string[] filesMove = filesToMove.ToArray();

            ////Verify that all the filepaths were moved to the array intact

            ////if filesMove.Length is either zero or unequal to filesToMove.Count
            //if (filesMove.Length == 0 || filesMove.Length != filesToMove.Count)
            //{
            //    //If this code is reached, something happened that nulled 
            //    //or removed some or all elements from the filesMove array
            //    return "BAD MOVE DURING TRANSFER";
            //}

            //if (FileManipExtensions.MoveSpecificFiles(filesMove, newDirPath, true))
            //{
            //    return "SAVED";
            //}
            //else return "FAILED ON FILE MOVE";
        }

        /// <summary>
        /// Discard all the images in the temp folder with the given filename.
        /// Returns "DONE" if successful, "ERROR" if the deletion fails at any point.
        /// </summary>
        /// <param name="filename">The name of the file to delete</param>
        /// <returns>string</returns>
        [NonAction]
        private string DiscardChanges(string filename)
        {
            string dirPath = (Path.Combine(Server.MapPath("~/Content/Images/tempUploads/")));
            string[] filesToDiscard = Directory.GetFiles(dirPath, filename);
            if (FileManipExtensions.DeleteSpecificFiles(filesToDiscard))
            {
                return "DONE";
            }
            else return "ERROR";
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

        /// <summary>
        /// Save a picture to the server. Returns the relative path if successful, otherwise returns "FAILED"
        /// </summary>
        /// <param name="picture">The picture object to be saved</param>
        /// <returns>string</returns>
        [NonAction]
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
                            if ((filetype == ".bmp") || (filetype == ".jpg") || (filetype == ".jpeg") || (filetype == ".png"))
                            {
                                // Then renames that image to the correct name based off the answer
                                // And number of picturs per answer, then deletes the old picture
                                string newfName = (picture.Answer.AnswerString + "-" + picture.Answer.PictureCount.ToString() + filetype);
                                relpath = (moduleDirectory + newfName);
                                System.IO.File.Copy(path, relpath);
                                System.IO.File.Delete(path);

                                db.SaveChanges();
                            }
                            else RedirectToAction("InvalidImage", "Pictures");
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
        public ActionResult InvalidImage()
        {
            return View();
        }
        public ActionResult DropzoneTest()
        {
            return View();
        }
    }
}
