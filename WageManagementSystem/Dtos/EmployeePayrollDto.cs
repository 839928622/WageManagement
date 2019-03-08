using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WageManagementSystem.Dtos
{
    public class EmployeePayrollDto
    {
        public int Id { get; set; }
        public DateTime PayrollDate { get; set; }
        public string EmoloyeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public bool Gender { get; set; }
        public double Attendance { get; set; }
        public double OverTime { get; set; }
        public double Salary { get; set; }

        public string AttendanceDataSources { get; set; }
        public string ComPosition { get; set; }
        public int ComRank { get; set; }
        public DateTime EnrollMentDate { get; set; }
        public DateTime ResignationDate { get; set; }
        public DateTime Deadline { get; set; }
        public string SchoolName { get; set; }
        public string FeeType { get; set; }
        public string ReleaseType { get; set; }

    }
}