namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameNormalization : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Modules", "DefaultNumPictures", "DefaultNumQuestions");
        }

        public override void Down()
        {
            RenameColumn("dbo.Modules", "DefaultNumQuestions", "DefaultNumPictures");
        }
    }
}
