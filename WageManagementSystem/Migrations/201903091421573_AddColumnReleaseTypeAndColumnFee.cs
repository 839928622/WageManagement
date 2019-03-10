namespace WageManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnReleaseTypeAndColumnFee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "ReleaseType", c => c.String());
            AddColumn("dbo.Schools", "Fee", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schools", "Fee");
            DropColumn("dbo.Schools", "ReleaseType");
        }
    }
}
