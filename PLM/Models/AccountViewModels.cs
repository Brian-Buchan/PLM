using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PLM.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Institution")]
        public string Institution { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        public EditUserViewModel(ApplicationUser user)
        {
            Id = user.Id;   //todo: added status to EditUserViewModel constructor 4-8-17
            UserName        = user.UserName;
            FirstName       = user.FirstName;
            LastName        = user.LastName;
            Email           = user.Email;
            Status          = user.Status;  //todo: added status to EditUserViewModel constructor  4-8-17
            Institution     = user.Institution;
          //  this.Password = user.PasswordHash;
        }

        [Key]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)] //TODO: Added ID to EditUserViewModel ;allows for changing username  4-8-17
        public string Id { get; set; }

        
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Institution { get; set; }

        [Display(Name = "Account Status")]
        public ApplicationUser.AccountStatus Status { get; set; }
        /*TODO: commented out password worn editviewmodel. This needs to be seperate function 
        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }
        */

    }

    public class CngUserPwdViewModel  //todo: added CngUserPwdViewModel 4-8-17
    {

        public CngUserPwdViewModel() { }
        public CngUserPwdViewModel(ApplicationUser user)
        {
            Id = user.Id;   
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Status = user.Status;  
            Institution = user.Institution;
            this.Password = user.PasswordHash;
        }

        [Key]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)] 
        public string Id { get; set; }


        [ReadOnly(true)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [ReadOnly(true)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [ReadOnly(true)]
        public string Institution { get; set; }

        [Display(Name = "Account Status")]
        [ReadOnly(true)]
        public ApplicationUser.AccountStatus Status { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class DisableUserViewModel
    {
        public DisableUserViewModel() { }
        public DisableUserViewModel(ApplicationUser user)
        {
            this.UserName = user.UserName;
            this.DisableAccountReason = user.DisableAccountReason;
            this.DisableAccountNote = user.DisableAccountNote;
            this.Status = user.Status;
        }

        [Key]
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Reason")]
        public ApplicationUser.Reason DisableAccountReason{get; set;}

        [Display(Name = "Note")]
        public string DisableAccountNote { get; set; }

        [Display(Name = "Account Status")]
        public ApplicationUser.AccountStatus Status { get; set; }

    }

    public class InstructorRoleRequest
    {
        public InstructorRoleRequest() { }
        public InstructorRoleRequest(ApplicationUser user)
        {
            this.UserName = user.UserName;
            this.Status = user.Status;
        }
        [Key]
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Institution { get; set; }

        [Display(Name = "Account Status")]
        public ApplicationUser.AccountStatus Status { get; set; }

    }

    public class CreateUserViewModel
    {
        public CreateUserViewModel() { }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Institution { get; set; }
    }
}