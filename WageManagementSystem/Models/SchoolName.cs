using System.ComponentModel.DataAnnotations;

namespace WageManagementSystem.Models
{
    public class School
    {
        public int Id { get; set; }

        [Display(Name = "输送机构")]
        public string Name { get; set; }

        [Display(Name = "费用类型")]
        public string ReleaseType { get; set; }

        [Display(Name = "费用")]
        public double Fee { get; set; }

        [Display(Name = "是否启用")]
        public bool IsActive { get; set; }



    }
}