using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace PLM
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(25)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Institution")]
        [MaxLength(40)]
        public string Institution { get; set; }

        [Display(Name = "Avatar Url")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Modules List")]
        public List <Module> ModuleList { get; set; }
        public int OverrideNumberOfAnswers { get; set; }

        public enum AccountType
        {
            Free,
            Premium            
        }

        public enum AccountStatus
        {
            Pending,
            PendingInstrustorRole,
            Active,
            Disabled
        }

        [Display(Name = "Account Type")]
        public AccountType Type { get; set; }

        [Display(Name = "Account Status")]
        public AccountStatus Status { get; set; }

        [Display(Name = "Note")]
        public string DisableAccountNote { get; set; }
        public enum Reason
        {
            None,
            AccountNotPaid, 
            AgainstTermsOfUser,
            Other
        }

        [Display(Name = "Reason")]
        public Reason DisableAccountReason { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim("FirstName", this.FirstName));
            userIdentity.AddClaim(new Claim("LastName", this.LastName));
            userIdentity.AddClaim(new Claim("Instution", this.Institution));
            return userIdentity;
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base(DevPro.connectionStringName, throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Score> Scores { get; set; }
        public DbSet<PLM.Models.Report> Reports { get; set; }
        public DbSet<FAQ> FAQs { get; set; }


        public System.Data.Entity.DbSet<PLM.DisableModuleViewModel> DisableModuleViewModels { get; set; }
    }
}