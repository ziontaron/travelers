namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIsDocument : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Track", "User_CreatedByKey", "dbo.User");
            DropIndex("dbo.Track", new[] { "User_CreatedByKey" });
            AddColumn("dbo.User", "is_locked", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "document_status", c => c.String());
            AlterColumn("dbo.Track", "User_CreatedByKey", c => c.Int());
            CreateIndex("dbo.Track", "User_CreatedByKey");
            AddForeignKey("dbo.Track", "User_CreatedByKey", "dbo.User", "UserKey");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Track", "User_CreatedByKey", "dbo.User");
            DropIndex("dbo.Track", new[] { "User_CreatedByKey" });
            AlterColumn("dbo.Track", "User_CreatedByKey", c => c.Int(nullable: false));
            DropColumn("dbo.User", "document_status");
            DropColumn("dbo.User", "is_locked");
            CreateIndex("dbo.Track", "User_CreatedByKey");
            AddForeignKey("dbo.Track", "User_CreatedByKey", "dbo.User", "UserKey", cascadeDelete: true);
        }
    }
}
