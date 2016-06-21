namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reports : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "category", c => c.Int(nullable: false));
            AlterColumn("dbo.Reports", "description", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reports", "description", c => c.String(nullable: false));
            DropColumn("dbo.Reports", "category");
        }
    }
}
