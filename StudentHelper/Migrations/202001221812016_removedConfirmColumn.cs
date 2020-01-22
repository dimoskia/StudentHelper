namespace StudentHelper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedConfirmColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "Confirmed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Confirmed", c => c.Boolean(nullable: false));
        }
    }
}
