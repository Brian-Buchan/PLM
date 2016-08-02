namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisableModuleAccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DisableModuleViewModels",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        isDisabled = c.Boolean(nullable: false),
                        DisableModuleNote = c.String(),
                        DisableModuleReason = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            AddColumn("dbo.Modules", "DisableModuleNote", c => c.String());
            AddColumn("dbo.Modules", "DisableModuleReason", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "DisableAccountNote", c => c.String());
            AddColumn("dbo.AspNetUsers", "DisableAccountReason", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DisableAccountReason");
            DropColumn("dbo.AspNetUsers", "DisableAccountNote");
            DropColumn("dbo.Modules", "DisableModuleReason");
            DropColumn("dbo.Modules", "DisableModuleNote");
            DropTable("dbo.DisableModuleViewModels");
        }
    }
}
