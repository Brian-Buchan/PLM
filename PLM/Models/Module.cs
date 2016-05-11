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
       // public string CategoryId { get; set; }  - Shane: commented out. Switched to Category.

        public virtual int CategoryId { get; set; }

        public virtual List<Answer> Answers { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}