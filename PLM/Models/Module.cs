using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLM
{
    public class Module
    {
        public int ModuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Category")]
        public virtual int CategoryId { get; set; }

        [Display(Name="Default Number of Answers")]
        public int DefaultNumAnswers { get; set; }

        [Display(Name = "Default Time")]
        public int DefaultTime { get; set; }

        [Display(Name = "Default Number of Pictures")]
        public int DefaultNumPictures { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool isPrivate { get; set; }

        public virtual Category Category { get; set; }
    }
}