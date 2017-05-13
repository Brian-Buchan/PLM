namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                {
                    AnswerID = c.Int(nullable: false, identity: true),
                    AnswerString = c.String(nullable: false, maxLength: 25),
                    ModuleID = c.Int(nullable: false),
                    PictureCount = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.AnswerID)
                .ForeignKey("dbo.Modules", t => t.ModuleID, cascadeDelete: true)
                .Index(t => new { t.ModuleID, t.AnswerString }, unique: true, name: "IX_AnswerStringModuleId");

            CreateTable(
                "dbo.Modules",
                c => new
                {
                    ModuleID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 25),
                    Description = c.String(maxLength: 200),
                    CategoryId = c.Int(nullable: false),
                    DefaultNumAnswers = c.Int(nullable: false),
                    DefaultTime = c.Int(nullable: false),
                    DefaultNumQuestions = c.Int(nullable: false),
                    isPrivate = c.Boolean(nullable: false),
                    rightAnswerString = c.String(),
                    wrongAnswerString = c.String(),
                    isDisabled = c.Boolean(nullable: false),
                    DisableModuleNote = c.String(),
                    DisableReason = c.Int(nullable: false),
                    User_Id = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.ModuleID)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.User_Id);

            CreateTable(
                "dbo.Categories",
                c => new
                {
                    CategoryID = c.Int(nullable: false, identity: true),
                    CategoryName = c.String(),
                })
                .PrimaryKey(t => t.CategoryID);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    FirstName = c.String(nullable: false, maxLength: 25),
                    LastName = c.String(nullable: false, maxLength: 25),
                    Institution = c.String(nullable: false, maxLength: 40),
                    ProfilePicture = c.String(),
                    OverrideNumberOfAnswers = c.Int(nullable: false),
                    Type = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    DisableAccountNote = c.String(),
                    DisableAccountReason = c.Int(nullable: false),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    RoleId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.Pictures",
                c => new
                {
                    PictureID = c.Int(nullable: false, identity: true),
                    Location = c.String(),
                    AnswerID = c.Int(nullable: false),
                    Attribution = c.String(maxLength: 300),
                    PictureData = c.String(),
                })
                .PrimaryKey(t => t.PictureID)
                .ForeignKey("dbo.Answers", t => t.AnswerID, cascadeDelete: true)
                .Index(t => t.AnswerID);

            CreateTable(
                "dbo.FAQs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Question = c.String(maxLength: 255),
                    Answer = c.String(),
                    SortOrder = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Reports",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    moduleID = c.Int(nullable: false),
                    description = c.String(nullable: false, maxLength: 200),
                    userID = c.String(nullable: false),
                    category = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.Scores",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    ModuleID = c.Int(nullable: false),
                    UserID = c.String(),
                    CorrectAnswers = c.Int(nullable: false),
                    TotalAnswers = c.Int(nullable: false),
                    TimeStamp = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.ID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Pictures", "AnswerID", "dbo.Answers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Modules", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Modules", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Answers", "ModuleID", "dbo.Modules");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Pictures", new[] { "AnswerID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Modules", new[] { "User_Id" });
            DropIndex("dbo.Modules", new[] { "CategoryId" });
            DropIndex("dbo.Answers", "IX_AnswerStringModuleId");
            DropTable("dbo.Scores");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Reports");
            DropTable("dbo.FAQs");
            DropTable("dbo.Pictures");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Categories");
            DropTable("dbo.Modules");
            DropTable("dbo.Answers");
        }
    }
}
