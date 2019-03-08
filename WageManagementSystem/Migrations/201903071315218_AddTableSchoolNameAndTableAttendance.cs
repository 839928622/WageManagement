namespace WageManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableSchoolNameAndTableAttendance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AttendanceDataSources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Branch = c.String(),
                        AttendenceSources = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AttendanceDataSources");
        }
    }
}
