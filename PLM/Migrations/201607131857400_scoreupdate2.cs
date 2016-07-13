namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scoreupdate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Scores", "Module_ModuleID", "dbo.Modules");
            DropIndex("dbo.Scores", new[] { "Module_ModuleID" });
            AddColumn("dbo.Scores", "ModuleID", c => c.Int(nullable: false));
            DropColumn("dbo.Scores", "Module_ModuleID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Scores", "Module_ModuleID", c => c.Int());
            DropColumn("dbo.Scores", "ModuleID");
            CreateIndex("dbo.Scores", "Module_ModuleID");
            AddForeignKey("dbo.Scores", "Module_ModuleID", "dbo.Modules", "ModuleID");
        }
    }
}
