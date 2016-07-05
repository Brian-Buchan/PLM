using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace PLM
{
    public class Score
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public virtual Module Module { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Correct Answers")]
        public int CorrectAnswers { get; set; }

        [Display(Name = "Total Answers")]
        public int TotalAnswers { get; set; }

        [Display(Name = "Time Completed")]
        public DateTime TimeStamp { get; set; }

        public Score()
        {
            TimeStamp = DateTime.Now;
        }
    }
}