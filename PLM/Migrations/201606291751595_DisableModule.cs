namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DisableModule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "isDisabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Modules", "isDisabled");
        }
    }
}
