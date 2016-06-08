namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class g : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reports", "description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reports", "description", c => c.String());
        }
    }
}
