namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttributionLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pictures", "Attribution", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pictures", "Attribution", c => c.String(maxLength: 40));
        }
    }
}
