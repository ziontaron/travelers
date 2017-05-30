namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TravelerHeader", "CreatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TravelerHeader", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
