using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace PLM
{
    public class Module
    {
        public int ModuleID { get; set; }
        [Display(Name = "Module name")]
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public virtual int CategoryId { get; set; }

        [Display(Name="Default Number of Answers")]
        public int DefaultNumAnswers { get; set; }

        [Display(Name = "Default Time in minutes")]
        public int DefaultTime { get; set; }

        [Display(Name = "Default Number of Questions")]
        public int DefaultNumQuestions { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool isPrivate { get; set; }
        [Display(Name = "Response for correct answer")]
        public string rightAnswerString { get; set; }
        [Display(Name = "Response for incorrect answer")]
        public string wrongAnswerString { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Is Disabled")]
        public bool isDisabled { get; set; }

        [Display(Name = "Note")]
        public string DisableModuleNote { get; set; }

        public enum DisableModuleReason
        {
            [Description("Account Not Paid")]
            AccountNotPaid,
            [Description("Against Terms of Use")]
            AgainstTermsOfUse,
            [Description("Copywrite Infringment")]
            CopyWriteInfringment,
            [Description("Inappropiate Content")]
            InappropriateContent,
            Other
        }

        [Display(Name = "Reason")]
        public DisableModuleReason DisableReason { get; set; }

    }

    
}