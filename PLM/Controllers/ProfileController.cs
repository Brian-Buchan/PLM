using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLM;
using Microsoft.AspNet.Identity;

namespace PLM.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Profile
        //public ActionResult Index()
        //{
        //    return View(db.Modules.ToList());
        //}
        public ActionResult Index()
        {
            ViewBag.UserID = User.Identity.Name;
            //ViewBag.ImageUrl = imageUrl;
            return View(db.Modules.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProfile()
        //[Bind(Include = "PictureID,Location,AnswerID")] Picture picture
        {
            if (ModelState.IsValid)
            {
                var name = User.Identity.GetUserId();
                var test = db.Users.Single(x => x.FirstName == User.Identity.GetFirstName());
                ApplicationUser currentUser = (ApplicationUser)db.Users.Select(x => x.Id == User.Identity.GetUserId());
                var location = SaveUploadedFileProfile(currentUser.Id);

                if (location == "")
                {
                    //error
                }
                else
                {
                  currentUser.ProfilePicture = location;
                }

                db.SaveChanges(); 
            }

            //ViewBag.AnswerID = new SelectList(db.Answers, "AnswerID", "AnswerString", picture.AnswerID);
            return View();

        }
        public string SaveUploadedFileProfile(string UserId)
        {
            Session["upload"] = UserId;
            bool isSavedSuccessfully = true;
            string fName = "";
            string path = "";
            string relpath = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here

                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        string moduleDirectory = (Path.Combine(Server.MapPath("~/Content/Images/PLM/" + Session["upload"].ToString() + "/")));
                        if (!Directory.Exists(moduleDirectory))
                        {
                            Directory.CreateDirectory(moduleDirectory);
                        }
                        //String[] substrings = fName.Split('/');
                        //fName = substrings[Array.LastIndexOf(substrings, ".")];
                        path = moduleDirectory + fName;

                        relpath = ("/Content/Images/PLM/" + Session["upload"].ToString() + "/" + fName);
                        file.SaveAs(path);
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






        //public ActionResult PictureUpload(string UserName)
        //{
        //    var location = SaveUploadedFile(picture);

        //    if (location == "")
        //    {
        //        //error
        //    }
        //    else
        //    {
        //        picture.Location = location;
        //    }
        //}
        //public ActionResult FileUpload(HttpPostedFileBase file)
        //{
        //    if (file != null)
        //    {
        //        string pic = System.IO.Path.GetFileName(file.FileName);
        //        string path = System.IO.Path.Combine(
        //                               Server.MapPath("~/Content/Images/Profile"), pic);
        //        // file is uploaded
        //        file.SaveAs(path);

        //        // save the image path path to the database or you can send image 
        //        // directly to database
        //        // in-case if you want to store byte[] ie. for DB
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            file.InputStream.CopyTo(ms);
        //            byte[] array = ms.GetBuffer();
        //        }
               
        //        return RedirectToAction("Index", "Profile", new { imageurl = path });
        //    }
        //    return View();
        //}
    }
}