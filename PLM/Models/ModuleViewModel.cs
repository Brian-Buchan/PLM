using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLM
{
    public class ModuleViewModel
    {
        public List<Module> Mods = new List<Module>();
        public List<Category> Cats = new List<Category>();
    }

    public class DisableModuleViewModel
    {
        [Key]
        [Display(Name = "Module Name")]
        public string Name { get; set; }

        [Display(Name = "Is Disabled")]
        public bool isDisabled { get; set; }

        [Display(Name = "Note")]
        public string DisableModuleNote { get; set; }

        public enum DisableReason
        {
            AccountNotPaid,
            AgainstTermsOfUse,
            CopyWriteInfringment,
            InappropriateContent,
            Other
        }

        [Display(Name = "Reason")]
        public DisableReason DisableModuleReason { get; set; }
    }
}