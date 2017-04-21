namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CQAHeader", "CustomerNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CQAHeader", "CustomerNumber");
        }
    }
}
