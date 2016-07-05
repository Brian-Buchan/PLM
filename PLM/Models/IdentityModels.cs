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
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Institution")]
        public string Institution { get; set; }

        [Display(Name = "Avatar Url")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Modules List")]
        public List <Module> ModuleList { get; set; }

        [Display(Name = "Score List")]
        public List<Score> ScoreList { get; set; }

        [Display(Name = "Enrolled Courses")]
        public List<Course> EnrolledCourses { get; set; }

        [Display(Name = "Instructed Courses")]
        public List<Course> InstructedCourses { get; set; }

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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim("FirstName", this.FirstName));
            userIdentity.AddClaim(new Claim("LastName", this.LastName));
            userIdentity.AddClaim(new Claim("Instution", this.Institution));
            //userIdentity.AddClaim(new Claim("ProfilePicture", this.ProfilePicture));
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            //Production
            : base("Production", throwIfV1Schema: false)
        //Development
        //: base("Development", throwIfV1Schema: false)
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
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseWork> CourseWork { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<PLM.Models.Report> Reports { get; set; }
    }
}