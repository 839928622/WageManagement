using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using Quartz;
using WageManagementSystem.Dtos;
using WageManagementSystem.Jobs;
using WageManagementSystem.Models;
using WageManagementSystem.EmployeeAttendanceQuery;
using System.Collections.Specialized;
using Quartz.Impl;

namespace WageManagementSystem.Controllers
{
    public class EmployeePayrollsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

      

        public   RedirectToRouteResult GenerateEmployeeFee()
        {


            NameValueCollection props = new NameValueCollection
          {
                             { "quartz.serializer.type", "binary" }
                       };
                StdSchedulerFactory factory = new StdSchedulerFactory(props);
           
         // get a scheduler
         IScheduler sched =  factory.GetScheduler();
                 sched.Start();

            IJobDetail SyncEmployeeInfo = JobBuilder.Create<SyncEmployeeInfo>()
                .WithIdentity("SyncInfo", "Group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("Triggle1", "Group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(360000).WithRepeatCount(1))
                .Build();

            try
            {
                sched.ScheduleJob(SyncEmployeeInfo, trigger);
            }
            catch (Exception)
            {

                throw;
            }
            

            return RedirectToAction("Index");
            //return  db.EmployeePayrolls
            //       .Where(c => c.PayrollDate >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01")))//return current 
            //       .ToList()
            //       .Select(Mapper.Map<EmployeePayroll, EmployeePayrollDto>);
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

        [HttpPost]
        public async Task<ActionResult> getworkdays(string code,string attendenceSourse)
        {
            var start = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01");//上个月第一天
            var startdatetime = Convert.ToDateTime(start);
            var end = startdatetime.AddDays(1 - startdatetime.Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");//上个月最后一天
         double[] result= await Getworkday(code,start,end, attendenceSourse);

            EmployeePayroll ep = new EmployeePayroll();
            if (result!=null)
            {
                ep.Attendance = 25;//result[0];
                ep.OverTime = 0;// result[1];
            }
            
            return Json(ep,JsonRequestBehavior.AllowGet);

        }

        // GET: EmployeePayrolls
        public async Task<ActionResult> Index()
        {
           //var dataSources=  from data in db.EmployeePayrolls
                            //.OrderBy(e=>e.PayrollDate.Year)
                            //select data;
       //  ViewBag.dist = db.EmployeePayrolls.Select(c => c.PayrollDate).Distinct();
       ViewBag.result=db.EmployeePayrolls.DistinctBy(e => e.PayrollDate.Year);
            return View(await db.EmployeePayrolls.Distinct().ToListAsync());
        }

        // GET: EmployeePayrolls/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePayroll employeePayroll = await db.EmployeePayrolls.FindAsync(id);
            if (employeePayroll == null)
            {
                return HttpNotFound();
            }
            return View(employeePayroll);
        }

        // GET: EmployeePayrolls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeePayrolls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PayrollDate,EmoloyeeNumber,EmployeeName,Gender,Attendance,OverTime,Salary,AttendanceDataSources,ComPosition,ComRank,EnrollMentDate,ResignationDate,Deadline,SchoolName,FeeType,ReleaseType")] EmployeePayroll employeePayroll)
        {
            if (ModelState.IsValid)
            {
                db.EmployeePayrolls.Add(employeePayroll);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employeePayroll);
        }

        // GET: EmployeePayrolls/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePayroll employeePayroll = await db.EmployeePayrolls.FindAsync(id);
            if (employeePayroll == null)
            {
                return HttpNotFound();
            }
            return View(employeePayroll);
        }

        // POST: EmployeePayrolls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PayrollDate,EmployeeNumber,EmployeeName,Gender,Attendance,OverTime,Salary,AttendanceDataSources,ComPosition,ComRank,EnrollMentDate,ResignationDate,Deadline,SchoolName,FeeType,ReleaseType")] EmployeePayroll employeePayroll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeePayroll).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employeePayroll);
        }

        // GET: EmployeePayrolls/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeePayroll employeePayroll = await db.EmployeePayrolls.FindAsync(id);
            if (employeePayroll == null)
            {
                return HttpNotFound();
            }
            return View(employeePayroll);
        }

        // POST: EmployeePayrolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            EmployeePayroll employeePayroll = await db.EmployeePayrolls.FindAsync(id);
            db.EmployeePayrolls.Remove(employeePayroll);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
