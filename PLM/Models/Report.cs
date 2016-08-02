using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PLM.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLM.Models
{
    public class Report
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Module Reported")]
        public int moduleID { get; set; }
        [Required]
        [MaxLength(200)]
        [Display(Name = "Report info")]
        public string description { get; set;}
        [Required]
        [Display(Name = "User who reported the issue")]
        public string userID { get; set; }
        [Display(Name = "Type of report")]
        public virtual ReportCategoryEnum.reportCategory category {get; set;}
    }
} 