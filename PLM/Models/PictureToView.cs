using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLM
{
    public class PictureToView
    {
        [Key]
        public int PictureID { get; set; }
        public string PictureData { get; set; }
        public string Attribution { get; set; }
    }
}