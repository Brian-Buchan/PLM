namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryRequired : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisableModuleViewModels", "DisableReason", c => c.Int(nullable: false));
            DropColumn("dbo.DisableModuleViewModels", "Reason");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DisableModuleViewModels", "Reason", c => c.Int(nullable: false));
            DropColumn("dbo.DisableModuleViewModels", "DisableReason");
        }
    }
}
