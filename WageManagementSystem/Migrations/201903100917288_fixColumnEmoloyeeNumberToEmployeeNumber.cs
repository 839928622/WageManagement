namespace WageManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixColumnEmoloyeeNumberToEmployeeNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmployeePayrolls", "EmployeeNumber", c => c.String());
            DropColumn("dbo.EmployeePayrolls", "EmoloyeeNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EmployeePayrolls", "EmoloyeeNumber", c => c.String());
            DropColumn("dbo.EmployeePayrolls", "EmployeeNumber");
        }
    }
}
