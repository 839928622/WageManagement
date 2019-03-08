namespace WageManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableEmployeePayrolls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeePayrolls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayrollDate = c.DateTime(nullable: false),
                        EmoloyeeNumber = c.String(),
                        EmployeeName = c.String(),
                        Gender = c.Boolean(nullable: false),
                        Attendance = c.Double(nullable: true),
                        OverTime = c.Double(nullable: true),
                        Salary = c.Double(nullable: true),
                        AttendanceDataSources = c.String(),
                        ComPosition = c.String(),
                        ComRank = c.Int(nullable: true),
                        EnrollMentDate = c.DateTime(nullable: true),
                        ResignationDate = c.DateTime(nullable: true),
                        Deadline = c.DateTime(nullable: false),
                        SchoolName = c.String(),
                        FeeType = c.String(),
                        ReleaseType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmployeePayrolls");
        }
    }
}
