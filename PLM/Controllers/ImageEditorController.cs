using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using PLM.Extensions;
using PLM.Models;

namespace PLM.Controllers
{
    public class ImageEditorController : Controller
    {
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
        [NonAction]
        public ActionResult ImageEditor()
        {
            return View();
        }
        
        [HttpPost]
        [NonAction]
        [ActionName("ImageEditor")]
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
        [NonAction]
        //[RequireHttps] //Ensures http-headers work
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
        [NonAction]
        [ValidateAntiForgeryToken]
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
        [NonAction]
        public ActionResult Save()
        {
            string origUrl = Request.Form.Get("origUrl");
            string tempUrl = Request.Form.Get("tempUrl");
            string temporaryFileName = Path.GetFileName(tempUrl);
            string newFileName = Path.GetFileNameWithoutExtension(origUrl);

            PermaSave(temporaryFileName, newFileName);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [NonAction]
        public ActionResult Discard()
        {
            string tempUrl = Request.Form.Get("tempUrl");
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
        /// to the permUploads folder, with a new name.
        /// If there are multiple files found with the same name for whatever reason,
        /// takes the last one found.
        /// Returns "SAVED" if successful, 
        /// "NO FILES" if there were no files found, 
        /// or "FAILED" otherwise.
        /// </summary>
        /// <param name="filename">The file to permanently save. Expects only 
        /// the filename and its extension, not a path</param>
        /// <param name="newFileName">The new name for the saved file. 
        /// Expects only the name, not the path or extension</param>
        /// <returns>string</returns>
        [NonAction]
        private string PermaSave(string filename, string newFileName)
        {
            List<string> filesToMove = new List<string>();
            string dirPath = (Path.Combine(Server.MapPath("~/Content/Images/tempUploads/")));
            string newDirPath = (Path.Combine(Server.MapPath("~/Content/Images/permUploads/")));

            //if the selected file doesn't exist in the temp folder
            if (!(System.IO.File.Exists(dirPath + filename)))
            {
                //If this code is reached, the passed in filename could not be accessed. 
                //It may have been moved, deleted, or did not exist in the first place.
                //The passed in filename may have also contained illegal characters, 
                //referenced a location on a failing/missing disk, 
                //or the program might not have read permissions for that specific file.
                return "BAD LOCATION";
            }

            string[] filesToSave = Directory.GetFiles(dirPath, filename);

            if (filesToSave.Length == 0)
            {
                //If this code is reached, GetFiles didn't find any matching files.
                return "NO FILES FOUND";
            }

            //for each file to be saved, 
            foreach (string filePath in filesToSave)
            {
                string newFilePath;
                if (FileManipExtensions.TryRenameFile(filePath, newFileName, out newFilePath, true))
                {
                    filesToMove.Add(newFilePath);
                }
            }

            //Make sure that filepaths were actually moved to the filesToMove list.
            if (filesToMove.Count == 0)
            {
                //If this code is reached, there is a problem within the TryRenameFile method.
                //Another process may have been accessing the file at the time of the renaming.
                return "BAD MOVE ON RENAME";
            }

            string[] filesMove = filesToMove.ToArray();

            //Verify that all the filepaths were moved to the array intact

            //if filesMove.Length is either zero or unequal to filesToMove.Count
            if (filesMove.Length == 0 || filesMove.Length != filesToMove.Count)
            {
                //If this code is reached, something happened that nulled 
                //or removed some or all elements from the filesMove array
                return "BAD MOVE DURING TRANSFER";
            }

            if (FileManipExtensions.MoveSpecificFiles(filesMove, newDirPath, true))
            {
                return "SAVED";
            }
            else return "FAILED ON FILE MOVE";
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
    }
}