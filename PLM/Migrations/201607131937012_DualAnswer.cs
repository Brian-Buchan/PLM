namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DualAnswer : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Answers", new[] { "ModuleID" });
            CreateIndex("dbo.Answers", new[] { "ModuleID", "AnswerID" }, unique: true, name: "IX_AnswerStringModuleId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Answers", "IX_AnswerStringModuleId");
            CreateIndex("dbo.Answers", "ModuleID");
        }
    }
}
