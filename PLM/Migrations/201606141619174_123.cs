namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _123 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reports", "userID", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reports", "userID", c => c.Int(nullable: false));
        }
    }
}
