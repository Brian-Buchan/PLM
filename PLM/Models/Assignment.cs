using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace PLM
{
    public class Assignment
    {
        public int ID { get; set; }

        [Display(Name = "Course Work")]
        public virtual CourseWork CourseWork { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Score List")]
        public List<Score> ScoreList { get; set; }

        [Display(Name = "Number of Attempts")]
        public int NumOfAttempts { get; set; }
    }
}