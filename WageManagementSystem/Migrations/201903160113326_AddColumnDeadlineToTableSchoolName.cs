namespace WageManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnDeadlineToTableSchoolName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schools", "Deadline", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Schools", "Deadline");
        }
    }
}
