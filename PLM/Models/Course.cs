using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace PLM
{
    public class Course
    {
        public int ID { get; set; }
        public ApplicationUser Instructor { get; set; }
        public List<ApplicationUser> Students { get; set; }
        public List<Module> Modules { get; set; }

        [Display(Name = "Course Work")]
        public List<CourseWork> CourseWork { get; set; }

    }
}