using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;

namespace PLM
{
    public class CourseWork
    {
        public int ID { get; set; }
        public List<Assignment> Assignments { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }

        [Display(Name = "Override Number of Answers")]
        public int OverrideNumOfAnswers { get; set; }

        [Display(Name = "Max Attempts")]
        public int MaxAttempts { get; set; }
    }
}