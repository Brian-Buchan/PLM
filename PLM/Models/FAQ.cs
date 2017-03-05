using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLM
{
    public class FAQ
    {
        [StringLength(255)]
        public string Question { get; set; }
        [StringLength(8000)]
        public string Answer { get; set; }
        public int? SortOrder { get; set; }
        [Key]
        public int Id { get; set; }
    }
}