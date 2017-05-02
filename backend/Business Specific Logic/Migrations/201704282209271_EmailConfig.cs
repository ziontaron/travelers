namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "EmailPassword", c => c.String());
            AddColumn("dbo.User", "EmailServer", c => c.String());
            AddColumn("dbo.User", "EmailPort", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "EmailPort");
            DropColumn("dbo.User", "EmailServer");
            DropColumn("dbo.User", "EmailPassword");
        }
    }
}
