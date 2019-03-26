using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WageManagementSystem.Models
{
    public class EmployeePayroll
    {
        public int Id { get; set; }

        [Display(Name = "发放日期")]
       // [DataType(DataType.Date)]
        public DateTime PayrollDate { get; set; }
        [Display(Name = "工号")]
        public string EmployeeNumber { get; set; }
        [Display(Name = "姓名")]
        public string EmployeeName { get; set; }
        [Display(Name = "性别")]
        public bool Gender { get; set; }
        [Display(Name = "出勤数（天）")]
        public double Attendance { get; set; }
        [Display(Name = "加班数（小时）")]
        public double OverTime { get; set; }
        [Display(Name = "费用")]
        public double Salary { get; set; }
        [Display(Name = "考勤数据源")]
        public string AttendanceDataSources { get; set; }
        [Display(Name = "职务")]
        public string ComPosition { get; set; }
        [Display(Name = "职级")]
        public int ComRank { get; set; }
        [Display(Name = "入职日期")]
        public DateTime EnrollMentDate { get; set; }
        [Display(Name = "离职日期")]
        public DateTime ResignationDate { get; set; }
        [Display(Name = "输送期限")]
        public DateTime Deadline { get; set; }
        [Display(Name = "输送机构")]
        public string SchoolName { get; set; }
        [Display(Name = "费用类型")]
        public string FeeType { get; set; }
        [Display(Name = "发放类型")]
        public string ReleaseType { get; set; }
        [Display(Name = "部门")]
        public string Department { get; set; }

        //public static implicit operator EmployeePayroll(EmployeePayroll v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}