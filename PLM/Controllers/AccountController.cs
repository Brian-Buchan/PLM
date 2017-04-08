using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using PLM.Models;
using System.Net;
using System.Data.Entity;
using System.Configuration;
using System.Diagnostics;
using PLM.Extensions;
using PLM.CutomAttributes;
using System.Text.RegularExpressions;
using System.Globalization;
namespace PLM.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();



        public AccountController()
        {
        }
        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [Authorize(Roles ="Admin")]
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.UsernameSortParam = String.IsNullOrEmpty(sortOrder) ? "username_asc" : "";
            ViewBag.NameSortParam = sortOrder == "first_asc" ? "first_desc" : "first_asc";
            ViewBag.LastSortParam = sortOrder == "last_asc" ? "last_desc" : "last_asc";
            ViewBag.StatusSortParam = sortOrder == "status_asc" ? "status_desc" : "status_asc";
            ViewBag.InstitutionSortParam = sortOrder == "inst_asc" ? "inst_desc" : "inst_asc";
            //
            var db = new ApplicationDbContext();
            var users = from u in db.Users
                        where u.Status != ApplicationUser.AccountStatus.Disabled
                        select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.LastName.Contains(searchString)
                                       || u.FirstName.Contains(searchString)
                                       && u.Status != ApplicationUser.AccountStatus.Disabled);
            }
            switch (sortOrder)
            {
                case "inst_asc":
                    users = users.OrderBy(u => u.Institution);
                    break;
                case "inst_desc":
                    users = users.OrderByDescending(u => u.Institution);
                    break;
                case "status_asc":
                    users = users.OrderBy(u => u.Status);
                    break;
                case "status_desc":
                    users = users.OrderByDescending(u => u.Status);
                    break;

                case "username_asc":
                    users = users.OrderBy(u => u.UserName);
                    break;
                case "first_desc":
                    users = users.OrderByDescending(u => u.FirstName);
                    break;
                case "last_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "first_asc":
                    users = users.OrderBy(u => u.FirstName);
                    break;
                case "last_asc":
                    users = users.OrderBy(u => u.LastName);
                    break;
            }

            var model = new System.Collections.Generic.List<EditUserViewModel>();

            foreach (var user in users)
            {
                var u = new EditUserViewModel(user);
                model.Add(u);
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DisabledUsersList(string sortOrder, string searchString)
        {
            ViewBag.UsernameSortParam = String.IsNullOrEmpty(sortOrder) ? "username_asc" : "";
            ViewBag.NameSortParam = sortOrder == "first_asc" ? "first_desc" : "first_asc";
            ViewBag.LastSortParam = sortOrder == "last_asc" ? "last_desc" : "last_asc";

            var db = new ApplicationDbContext();
            var users = from u in db.Users
                        where u.Status == ApplicationUser.AccountStatus.Disabled
                        select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.LastName.Contains(searchString)
                                       || u.FirstName.Contains(searchString)
                                       && u.Status == ApplicationUser.AccountStatus.Disabled);
            }
            switch (sortOrder)
            {
                case "username_asc":
                    users = users.OrderBy(u => u.UserName);
                    break;
                case "first_desc":
                    users = users.OrderByDescending(u => u.FirstName);
                    break;
                case "last_desc":
                    users = users.OrderByDescending(u => u.LastName);
                    break;
                case "first_asc":
                    users = users.OrderBy(u => u.FirstName);
                    break;
                case "last_asc":
                    users = users.OrderBy(u => u.LastName);
                    break;
            }
            return View(users);
        }

        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult RoleRequest()
        {
            var db = new ApplicationDbContext();
            var users = from u in db.Users
                        where u.Status == ApplicationUser.AccountStatus.PendingInstrustorRole
                        select u;

            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        [ValidateAntiForgeryToken]
        public ActionResult RoleRequest(string userID)
        {
            ApplicationUser user = db.Users.First(x => x.Id == userID);
            user.Status = ApplicationUser.AccountStatus.Active;
            UserManager.AddToRole(user.Id, "Instructor");
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("RoleRequest", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        [ValidateAntiForgeryToken]
        public ActionResult DenyRequest(string userID)
        {
            ApplicationUser user = db.Users.First(x => x.Id == userID);
            user.Status = ApplicationUser.AccountStatus.Active;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("RoleRequest", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveALLRequests()
        {
            var users = (from u in db.Users
                        where u.Status == ApplicationUser.AccountStatus.PendingInstrustorRole
                        select u).ToList();

            for (int i = 0; i < users.Count; i++)
            {
                users[i].Status = ApplicationUser.AccountStatus.Active;
                UserManager.AddToRole(users[i].Id, "Instructor");
                db.Entry(users[i]).State = EntityState.Modified;
                db.SaveChanges();
            }    
            
            return RedirectToAction("RoleRequest", "Account");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Sets user Account Type to Free and Account Status to Active
                var user = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Institution = model.Institution,
                    Type = ApplicationUser.AccountType.Free,
                    Status = ApplicationUser.AccountStatus.Active
                };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Learner");
                    return RedirectToAction("Index", "Account");
                }
                AddErrors(result);
            }
            return View(model);
        }


        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult Edit(string userName = null)
        {
            if (userName == null)
            {
                return RedirectToAction("Index", "Account");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            var user = db.Users.First(u => u.UserName == userName);
            var model = new EditUserViewModel(user);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult Edit([Bind(Include = "Id,UserName, LastName, FirstName, Institution, Email, Status")] EditUserViewModel userModel)
        {
            //, Password, ConfirmPassword
            if (ModelState.IsValid)
            {
              
                var user = db.Users.First(u => u.Id == userModel.Id);
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Email = userModel.Email;
                user.Status =  userModel.Status;
                user.Institution = userModel.Institution;
          //TODO: Add change password button and process to reset passwords. No need to know user password.
          //      user.UserName = userModel.UserName;
          //      PasswordHasher ph = new PasswordHasher();
          //      user.PasswordHash = ph.HashPassword(userModel.Password);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userModel);
        }


        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult CngUserPwd(string id = null)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var user = db.Users.First(u => u.Id == id);
            var model = new CngUserPwdViewModel();
            ViewBag.LastName = user.LastName;
            ViewBag.FirstName = user.FirstName;
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult CngUserPwd([Bind(Include = "Id,Password,ConfirmPassword")] CngUserPwdViewModel userModel)
        {
          

            var user = db.Users.First(u => u.Id == userModel.Id);


            PasswordHasher ph = new PasswordHasher();
            user.PasswordHash = ph.HashPassword(userModel.Password);
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            

            return RedirectToAction("Index");
           
            //return View(userModel);
        }

        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult Delete(string userName = null)
        {
            var db = new ApplicationDbContext();
            var user = db.Users.First(u => u.UserName == userName);
            var model = new EditUserViewModel(user);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult DeleteConfirmed(string userName)
        {
            var db = new ApplicationDbContext();
            var user = db.Users.First(u => u.UserName == userName);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult ViewUserRoles(string userName = null)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                List<string> userRoles;

                using (var context = new ApplicationDbContext())
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);

                    var user = userManager.FindByName(userName);
                    if (user == null)
                    {
                        throw new Exception("User not found!");
                    }

                    var userRoleIds = (from r in user.Roles select r.RoleId);
                    userRoles = (from id in userRoleIds
                                 let r = roleManager.FindById(id)
                                 select r.Name).ToList();
                }
                ViewBag.UserName = userName;
                ViewBag.RolesForUser = userRoles;
            }
            return View();
        }


        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult DeleteRoleForUser(string userName = null, string roleName = null)
        {
            if ((!string.IsNullOrWhiteSpace(userName)) || (!string.IsNullOrWhiteSpace(roleName)))
            {
                List<string> userRoles;

                using (var context = new ApplicationDbContext())
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);
                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);
                    var user = userManager.FindByName(userName);

                    if (user == null)
                    {
                        throw new Exception("User not found!");
                    }

                    if (userManager.IsInRole(user.Id, roleName))
                    {
                        userManager.RemoveFromRole(user.Id, roleName);
                        context.SaveChanges();
                    }

                    var userRoleIds = (from r in user.Roles select r.RoleId);
                    userRoles = (from id in userRoleIds
                                 let r = roleManager.FindById(id)
                                 select r.Name).ToList();
                }
                ViewBag.UserName = userName;
                ViewBag.RolesForUser = userRoles;
                return View("ViewUserRoles");
            }

            else
            {
                return View("Index");
            }
        }

        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult AddRoleToUser(string userName = null)
        {
            List<string> roles;

            using (var context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                roles = (from r in roleManager.Roles select r.Name).ToList();
            }

            ViewBag.Roles = new SelectList(roles);
            ViewBag.UserName = userName;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult AddRoleToUser(string roleName, string userName)
        {
            List<string> roles;

            using (var context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(userName);

                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                if (roleManager == null)
                {
                    throw new Exception("Roles not found!");
                }

                var role = roleManager.FindByName(roleName);
                if (userManager.IsInRole(user.Id, role.Name))
                {
                    ViewBag.ErrorMessage = "This user already has the role specified!";
                    roles = (from r in roleManager.Roles select r.Name).ToList();
                    ViewBag.Roles = new SelectList(roles);
                    ViewBag.UserName = userName;
                    return View();
                }
                else
                {
                    userManager.AddToRole(user.Id, role.Name);
                    context.SaveChanges();
                    List<string> userRoles;
                    var userRoleIds = (from r in user.Roles select r.RoleId);
                    userRoles = (from id in userRoleIds
                                 let r = roleManager.FindById(id)
                                 select r.Name).ToList();

                    ViewBag.UserName = userName;
                    ViewBag.RolesForUser = userRoles;
                    return View("ViewUserRoles");
                }
            }
        }

        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try { 
                    var user = await UserManager.FindAsync(model.Email, model.Password);
                    if (user.Status == ApplicationUser.AccountStatus.Disabled)
                    {
                        return RedirectToAction("AccountDisabled");
                    }
                    if (user != null)
                    {
                        ViewBag.errorMessage = "You must have a confirmed email to log on.";
                        await SignInAsync(user, model.RememberMe);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
                catch(NullReferenceException)
                {
                    RedirectToAction("Login");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //User View
        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult AccountDisabled()
        {
            return View();
        }

        //Admin options page
        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult AccountDisable(string userName = null)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var db = new ApplicationDbContext();
            var user = db.Users.First(u => u.UserName == userName);
            var model = new DisableUserViewModel(user);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] //TODO: added authorizations 3-8-17
        public ActionResult AccountDisable([Bind(Include = "UserName, DisableAccountReason, DisableAccountNote, Status")] DisableUserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                var db = new ApplicationDbContext();
                var user = db.Users.First(u => u.UserName == userModel.UserName);

                user.DisableAccountReason = userModel.DisableAccountReason;
                user.DisableAccountNote = userModel.DisableAccountNote;
                user.Status = userModel.Status;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userModel);
        }
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                RegexUtilities emailcheck = new RegexUtilities();
                string email = model.Email;
                var user = new ApplicationUser();
                if (emailcheck.IsValidEmail(email)) // Checks whether email is valid in order to fix an error we had
                {
                    user = new ApplicationUser()
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Institution = model.Institution,
                        Type = ApplicationUser.AccountType.Free,
                        Status = ApplicationUser.AccountStatus.Active
                    };
                }
                else
                {
                    user = new ApplicationUser()
                    {
                        UserName = "Validemail@nmc.edu",//sets up a dummy model so it doesn't crash before showing validation message and error to user
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Institution = model.Institution,
                        Type = ApplicationUser.AccountType.Free,
                        Status = ApplicationUser.AccountStatus.Active
                    };
                }
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Learner");
                    ViewBag.Message = "Check your email and confirm your account, you must be confirmed "
                        + "before you can log in.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                    RedirectToAction("Index", "Profile");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
        // GET: /Account/ConfirmEmail

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }

        
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        
        
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View();
                }
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        
        
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            return View();
        }
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        
        
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }


        // GET: /Account/Manage
        [AllowAnonymous]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }
        
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            { 
                if(!(error.Contains("Name")&& error.Contains("is already taken.")))//Checks for name X is already taken and removes the error because it is a duplicate
                {
                ModelState.AddModelError("", error);
                }
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }
            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }
            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject,
               "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
            return callbackUrl;
        }
        #endregion
    }
}