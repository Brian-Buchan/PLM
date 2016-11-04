using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public class GameModule
    {
        public int ModuleID { get; set; }
        
        public string Name { get; set; }
        
        public int DefaultNumAnswers { get; set; }
        
        public int DefaultTime { get; set; }
        
        public int DefaultNumQuestions { get; set; }

        public virtual List<Answer> Answers { get; set; }
        
        public string rightAnswerString { get; set; }
        
        public string wrongAnswerString { get; set; }

        public GameModule()
        {

        }

        public GameModule(Module module)
        {
            ModuleID = module.ModuleID;
            Name = module.Name;
            DefaultNumAnswers = module.DefaultNumAnswers;
            DefaultNumQuestions = module.DefaultNumQuestions;
            DefaultTime = module.DefaultTime;
            Answers = module.Answers;
            rightAnswerString = module.rightAnswerString;
            wrongAnswerString = module.wrongAnswerString;
        }
    }
}