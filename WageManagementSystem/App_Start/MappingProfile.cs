using AutoMapper;
using WageManagementSystem.Dtos;
using WageManagementSystem.Models;

namespace WageManagementSystem.App_Start
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<EmployeePayroll, EmployeePayrollDto>();
            Mapper.CreateMap<EmployeePayrollDto, EmployeePayroll>();
        }
    }
}