namespace WageManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnDepartmentToTableEmployeePayroll : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeePayrolls", "Department", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmployeePayrolls", "Department");
        }
    }
}
