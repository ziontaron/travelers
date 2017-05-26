namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TravelerIsaDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelerHeader", "sys_active", c => c.Boolean(nullable: false));
            AddColumn("dbo.TravelerHeader", "is_locked", c => c.Boolean(nullable: false));
            AddColumn("dbo.TravelerHeader", "document_status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TravelerHeader", "document_status");
            DropColumn("dbo.TravelerHeader", "is_locked");
            DropColumn("dbo.TravelerHeader", "sys_active");
        }
    }
}
