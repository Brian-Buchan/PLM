namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usr : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.DisableModuleViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DisableModuleViewModels",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        ModuleID = c.Int(nullable: false),
                        isDisabled = c.Boolean(nullable: false),
                        DisableModuleNote = c.String(),
                        DisableReason = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
        }
    }
}
