namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModuleNameLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Modules", "Name", c => c.String(nullable: false, maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Modules", "Name", c => c.String(maxLength: 25));
        }
    }
}
