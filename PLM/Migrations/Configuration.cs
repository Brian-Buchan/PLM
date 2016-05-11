namespace PLM.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PLM.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PLM.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

              //You can use the DbSet<T>.AddOrUpdate() helper extension method 
              //to avoid creating duplicate seed data. E.g.
            
                //context.Modules.AddOrUpdate(
                //  m => m.Name,
                //  new Module { Name = "Games", CategoryId = "Games", ModuleID = 1 },
                //  new Module { Name = "Food", CategoryId = "Food", ModuleID = 2 },
                //  new Module { Name = "Shows", CategoryId = "Shows", ModuleID = 3 }
                //);
        }
    }
}
