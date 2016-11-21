namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModuleIDinDisableModuleViewModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisableModuleViewModels", "ModuleID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DisableModuleViewModels", "ModuleID");
        }
    }
}
