using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLM;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace PLM.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "Home");
                return View(db.Modules.ToList());
            }
            else
            { 
               
                ViewBag.UserID = User.Identity.Name;
                var name = User.Identity.GetUserName();
                ApplicationUser currentUser = (ApplicationUser)db.Users.Single(x => x.UserName == name);
                var modules = db.Modules.ToList();
                modules = (from m in modules
                               where m.User == currentUser
                               select m).ToList();
                ViewBag.location = currentUser.ProfilePicture;
                return View(modules);
            }
        }

        public ActionResult StatusRequest()
        {
            ViewBag.User = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StatusRequest(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                user.Status = ApplicationUser.AccountStatus.PendingInstrustorRole;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }  
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProfile()
        //[Bind(Include = "PictureID,Location,AnswerID")] Picture picture
        {
            if (ModelState.IsValid)
            {
                var name = User.Identity.GetUserName();
                ApplicationUser currentUser = (ApplicationUser)db.Users.Single(x => x.UserName == name);
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

            return RedirectToAction("Index");
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
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        string imageDirectory = (DevPro.baseFileDirectory + "Profiles/" + Session["upload"].ToString() + "/");
                        if (!Directory.Exists(imageDirectory))
                        {
                            Directory.CreateDirectory(imageDirectory);
                        }
                        path = imageDirectory + fName;
                        file.SaveAs(path);
                        string filetype = Path.GetExtension(path);
                        
                        relpath = (imageDirectory + "profilePicture" + filetype);
                        System.IO.File.Copy(path, relpath, true);
                        System.IO.File.Delete(path);
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
                return "error";
            }
        }
    }
}