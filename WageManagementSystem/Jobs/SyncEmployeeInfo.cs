using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Quartz;
using WageManagementSystem.EmployeeAttendanceQuery;
using WageManagementSystem.GetEmployeeInfoBySchool;
using WageManagementSystem.Models;
using System.Text.RegularExpressions;
using Quartz.Xml.JobSchedulingData20;

namespace WageManagementSystem.Jobs
{
    public class SyncEmployeeInfo:IJob
        
    {
       
            public async Task<List<EmployeePayroll>> GetEmployeesInfo(string schoolName)
            {

                EmployeeService packData = new EmployeeService();

                SVCHDR svchdr = new SVCHDR();
                svchdr.SOURCEID = "ASP.NET MVC";
                svchdr.IPADDRESS = "192.168.1.1";
                svchdr.DESTINATIONID = "EIP";
                svchdr.BO = "临时工信息同步";
                svchdr.TYPE = "select";

                APPHDR apphdr = new APPHDR();
                SVCBODYS svcbodys = new SVCBODYS();
                SVCHDRS svchdrs = new SVCHDRS();



                APPBODY appbody = new APPBODY();
                appbody.SCHOOLNAME = schoolName;






                await Task.Run(() =>
                {
                    svcbodys = packData.SYNEMPLOYEE(svchdr, apphdr, appbody); //invoke it
                });

                //access success
                List<EmployeePayroll> list=new List<EmployeePayroll>();

                if (svcbodys.OAPPBODYS.EMPLOYEES != null) //什么都没有，则数组为null，
                {
                    foreach (var item in svcbodys.OAPPBODYS.EMPLOYEES)
                    {
                        if (DateTime.Now<=Convert.ToDateTime(item.DEADLINE).AddMonths(2))//当前日期小于等于输送期限月份+2
                        {

                            var res = new EmployeePayroll
                            {

                                PayrollDate = DateTime.Now.AddMonths(-1),//当前月份生成上个月份数据
                                EmployeeNumber = item.EMPLOYEEID,
                                EmployeeName = item.EMPLOYEENAME,
                                Gender = item.GENDER=="2"?true:false,//源数据 2：男，1：女
                                SchoolName = item.SCHOOLNAME,
                                ComPosition = item.COMPOSITION,
                                ComRank = Convert.ToInt32(item.COMRANK),
                                EnrollMentDate = Convert.ToDateTime(item.ENROLLMENTDATE),
                                ResignationDate = Convert.ToDateTime(item.RESIGNATIONDATE),
                                Deadline =Convert.ToDateTime(item.DEADLINE) ,
                                Department = item.DEPARTMENT



                            };

                        list.Add(res);
                        }
                        continue;

                    }
                }

                return list;
            }

            public async Task<double[]> Getworkday(string EmployeeCode, string StartTime, string EndTime,
                string AttendanceDataSources)
            {
                DONLIM_MCASHRMS_EMPLOYEEATTENDANCEQUERY_087
                    packData = new DONLIM_MCASHRMS_EMPLOYEEATTENDANCEQUERY_087();

                DONLIM_MCASHRMS_EMPLOYEEATTENDANCEQUERY_0871 objpackData =
                    new DONLIM_MCASHRMS_EMPLOYEEATTENDANCEQUERY_0871();

                SvcHdrTypes scvhdrtypes = new SvcHdrTypes();
                AppBodyTypes appbodytypes = new AppBodyTypes();
                AppBodyType appbodytype = new AppBodyType();
                QueryResultListTypes qurtylist = new QueryResultListTypes();
                QueryDataType querytype = new QueryDataType();



                objpackData.SvcHdr = new SvcHdrType();
                objpackData.AppBody = new AppBodyType();
                objpackData.AppHdr = new AppHdrType();


                objpackData.SvcHdr.SOURCEID = "EIP";
                objpackData.SvcHdr.IPADDRESS = "192.168.80.63";
                objpackData.SvcHdr.TYPE = "SELECT";
                objpackData.SvcHdr.BO = "EIP考勤查询";
                objpackData.SvcHdr.DESTINATIONID = AttendanceDataSources;


                objpackData.AppBody.QueryData_ITEM = new QueryDataType[1];
                objpackData.AppBody.QueryData_ITEM[0] = new QueryDataType();

                objpackData.AppBody.QueryData_ITEM[0].EmployeeCode = EmployeeCode; //722394   703035 HRMS 19-1~31

                objpackData.AppBody.QueryData_ITEM[0].StartTime = Convert.ToDateTime(StartTime);
                objpackData.AppBody.QueryData_ITEM[0].EndTime = Convert.ToDateTime(EndTime);


                DONLIM_MCASHRMS_EMPLOYEEATTENDANCEQUERY_087Response Attendance =
                    new DONLIM_MCASHRMS_EMPLOYEEATTENDANCEQUERY_087Response();

                await Task.Run(() =>
                {
                    Attendance = packData.CallDONLIM_MCASHRMS_EMPLOYEEATTENDANCEQUERY_087
                        (objpackData); //invoke it
                });

                //access success




                return scvhdrtypes.RCODE == "S" && Attendance.AppBodys.QueryResultList_ITEM != null
                    ? new double[]
                    {
                        (double) Attendance.AppBodys.QueryResultList_ITEM[0].Ycqts,
                        (double) Attendance.AppBodys.QueryResultList_ITEM[0].OverTime
                    }
                    : new double[] { };

            }
            private ApplicationDbContext db = new ApplicationDbContext();
        async  void IJob.Execute(IJobExecutionContext context)
        {

     
        

        #region 根据输送机构同步人员信息

        var schools = await db.Schools.Where(s => s.IsActive == true).Select(s => new { s.Name }).ToListAsync();
            foreach (var school in schools)
            {
                var employees = await GetEmployeesInfo(school.Name);
                foreach (var employee in employees)
                {
                    db.EmployeePayrolls.Add(employee);
                    db.SaveChanges();
                }
            }


            #endregion


            #region 同步考勤数据源
            var employDepartment = await db.EmployeePayrolls.ToArrayAsync();
            foreach (var item in employDepartment)
            {
                string department = item.Department;

                string[] depart = Regex.Split(department, "-", RegexOptions.IgnoreCase);

                item.AttendanceDataSources = db.AttendanceDataSourceses
                    .Where(t => t.Branch == depart[1] + depart[2])
                    .Select(s => s.AttendenceSources)
                    .SingleOrDefault();
                db.SaveChanges();
            }


            #endregion


            #region 生成发放记录



            var employeeSalary = await db.EmployeePayrolls.Where(
                    e =>
                     e.PayrollDate.AddMonths(1).Date.ToString("yyyy-MM") == DateTime.Now.ToString("yyyy-MM"))
                    .Select(s => new { s.Attendance, s.OverTime, s.Id, s.EmployeeNumber, s.Department, s.AttendanceDataSources })
                    .ToListAsync();//取出上个月中，考勤为空 发放日期的年月+1=当前日期的年月 的数据


            foreach (var item in employeeSalary)
            {
                var dataInDb = db.EmployeePayrolls.Find(item.Id);


                var attendence = await Getworkday(
                                         item.EmployeeNumber
                                         , DateTime.Now.AddMonths(-1).Date.ToString("yyyy-MM-01")//上个月第一天
                                         , DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")//上个月最后一天
                                         , item.AttendanceDataSources);

                if (attendence != null)
                    dataInDb.Attendance = attendence[0];
                dataInDb.OverTime = attendence[1];
                
                db.SaveChanges();
            }
            #endregion

            db.Dispose();
        }
    }
    }