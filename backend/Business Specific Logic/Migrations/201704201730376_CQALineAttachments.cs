namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CQALineAttachments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CQALine", "AttachmentsFolder", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CQALine", "AttachmentsFolder");
        }
    }
}
