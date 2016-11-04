using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
//TODO: COPY OVER
namespace PLM
{
    public class Repos
    {
        ApplicationDbContext _dc = new ApplicationDbContext();

        //public Module GetModule(int id)
        //{
        //    var module = _dc.Database.SqlQuery(Module,
        //        "Select from Modules")
        //}

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
                "select c.[CategoryName], c.[CategoryID], count(m.ModuleID) as ModuleCount from[dbo].[Categories] c left outer join[dbo].[Modules] m on m.CategoryId = c.CategoryID group by c.[CategoryName], c.[CategoryID] order by c.[CategoryName]"
            ).ToList<ModuleFilterMenuList>();
            return _List;
        }


    }
}