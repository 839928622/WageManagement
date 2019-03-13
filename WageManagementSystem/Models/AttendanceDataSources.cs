using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WageManagementSystem.Models
{
    public class AttendanceDataSources
    {

        public int Id { get; set; }
        [Display(Name = "分公司名称")]
        public string Branch { get; set; }
        [Display(Name = "考勤数据源")]
        public string AttendenceSources { get; set; }

    }
}