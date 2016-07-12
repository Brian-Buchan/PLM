namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisableModuleChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "DisableReason", c => c.Int(nullable: false));
            AddColumn("dbo.DisableModuleViewModels", "Reason", c => c.Int(nullable: false));
            DropColumn("dbo.Modules", "DisableModuleReason");
            DropColumn("dbo.DisableModuleViewModels", "DisableModuleReason");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DisableModuleViewModels", "DisableModuleReason", c => c.Int(nullable: false));
            AddColumn("dbo.Modules", "DisableModuleReason", c => c.Int(nullable: false));
            DropColumn("dbo.DisableModuleViewModels", "Reason");
            DropColumn("dbo.Modules", "DisableReason");
        }
    }
}
