using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
//TODO: COPY OVER
namespace PLM
{
    public class Repos
    {
        ApplicationDbContext _dc = new ApplicationDbContext();

        public IEnumerable<Module> GetModuleList()
        {
            // IngredientListView ingredients = new IngredientListView();
            //var idParam = new SqlParameter
            //{
            //    ParameterName = "recipeid",
            //    Value = recipeID
            //};
            var _List = _dc.Database.SqlQuery<Module>(
                "Select * from modules"
            ).ToList<Module>();
            return _List;
        }

        public IEnumerable<AnswerViewModel> GetAnswerList(int moduleID = 0)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "moduleID",
                Value = moduleID > 0 ? moduleID : SqlInt32.Null
            };
            var _List = _dc.Database.SqlQuery<AnswerViewModel>(
                "Select AnswerID, AnswerString, Answers.ModuleID, Modules.Name as ModuleName from Answers join Modules on Answers.ModuleID = Modules.ModuleID Where Answers.ModuleID = @moduleID or @moduleID is null", idParam 
            ).ToList<AnswerViewModel>();
            return _List;
        }
        
        public IEnumerable<Picture> GetPictureList(int answerID = 0)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "answerID",
                Value = answerID > 0 ? answerID : SqlInt32.Null
            };
            var _List = _dc.Database.SqlQuery<Picture>(
                "Select PictureID, AnswerID, Attribution, PictureData, Answers.AnswerString as AnswerString from Pictures join Answer on Picture.AnswerID = Answers.AnswerID Where Picture.AnswerID = @answerID or @answerID is null", idParam
            ).ToList<Picture>();
            return _List;
        }

        public IEnumerable<Module> GetCategoryList()
        {
            var _List = _dc.Database.SqlQuery<Module>(
                "Select * from Categories"
            ).ToList<Module>();
            return _List;
        }

        public IEnumerable<ModuleFilterMenuList> GetModuleFilterMenuList()
        {
            var _List = _dc.Database.SqlQuery<ModuleFilterMenuList>(
                "select c.[CategoryName], c.[CategoryID], count(m.ModuleID) as ModuleCount from[dbo].[Categories] as c left outer join[dbo].[Modules] as m on m.CategoryId = c.CategoryID group by c.[CategoryName], c.[CategoryID] order by c.[CategoryName]"
            ).ToList<ModuleFilterMenuList>();
            return _List;
        }


    }
}