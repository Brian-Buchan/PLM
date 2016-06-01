using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PLM
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Display(Name="Category Name")]
        public string CategoryName { get; set; }

        public virtual List <Module> ModuleList { get; set; }
    }
}