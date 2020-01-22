namespace StudentHelper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addConfirmationCodeColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ConfirmationCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ConfirmationCode");
        }
    }
}
