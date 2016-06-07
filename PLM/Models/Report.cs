using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLM.Models
{
    public class Report
    {
        [Key]
        public int ID { get; set; }
        public int moduleID { get; set; }
        public string description { get; set;}
        public int userID { get; set; }
    }
}