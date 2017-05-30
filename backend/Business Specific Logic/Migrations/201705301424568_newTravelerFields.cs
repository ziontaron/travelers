namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTravelerFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelerHeader", "PartNumberRev", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.TravelerHeader", "MOOrder", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.TravelerHeader", "MOLine", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.TravelerHeader", "MOQty", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TravelerHeader", "MOQty");
            DropColumn("dbo.TravelerHeader", "MOLine");
            DropColumn("dbo.TravelerHeader", "MOOrder");
            DropColumn("dbo.TravelerHeader", "PartNumberRev");
        }
    }
}
