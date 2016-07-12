using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PLM.Models;

namespace PLM
{
    public class ModuleViewModel
    {
        public List<Module> Mods = new List<Module>();
        public List<Category> Cats = new List<Category>();
    }

    public class DisableModuleViewModel
    {
        public DisableModuleViewModel() { }

        public DisableModuleViewModel(Module module)
        {
            this.Name = module.Name;
            this.DisableReason = module.DisableReason;
            this.DisableModuleNote = module.DisableModuleNote;
            this.isDisabled = module.isDisabled;
        }

        [Key]
        [Display(Name = "Module Name")]
        public string Name { get; set; }

        [Display(Name = "Is Disabled")]
        public bool isDisabled { get; set; }

        [Display(Name = "Note")]
        public string DisableModuleNote { get; set; }

        [Display(Name = "Reason")]
        public Module.DisableModuleReason DisableReason { get; set; }
    }
}