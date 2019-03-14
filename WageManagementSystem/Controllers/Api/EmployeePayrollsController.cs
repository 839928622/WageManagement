using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Mvc;
using Quartz;
using WageManagementSystem.Dtos;
using WageManagementSystem.Jobs;
using WageManagementSystem.Models;

namespace WageManagementSystem.Controllers.Api
{
    
    public class EmployeePayrollsController : ApiController
    {
        //private IScheduler _scheduler;

        //public EmployeePayrollsController(IScheduler scheduler)
        //{
        //    _scheduler = scheduler;
        //}

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/EmployeePayrolls
        public IEnumerable<EmployeePayrollDto> GetEmployeePayrolls()
        {
            DateTime dt =Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")) ;
            var result = from tmp in db.EmployeePayrolls
                    .Where(e => e.PayrollDate
                                == dt)
                    select tmp;
              //  .ToList()
              //  .Select(Mapper.Map<EmployeePayroll,EmployeePayrollDto>);//show last month data/latest data
              return result.ToList()
                  .Select(Mapper.Map<EmployeePayroll, EmployeePayrollDto>);
        }

        // GET: api/EmployeePayrolls/5
        //  [ResponseType(typeof(EmployeePayroll))]
        public async Task<EmployeePayrollDto> GetEmployeePayroll(int id)
        {
            EmployeePayroll employeePayroll = await db.EmployeePayrolls.FindAsync(id);
            if (employeePayroll == null)
            {
                throw new  HttpResponseException(HttpStatusCode.NotFound);
            }

            return Mapper.Map<EmployeePayroll, EmployeePayrollDto>(employeePayroll);
        }

        // PUT: api/EmployeePayrolls/5
      //  [ResponseType(typeof(void))]
      [System.Web.Http.HttpPut]
        public void  PutEmployeePayroll(int id, EmployeePayrollDto employeePayrollDto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var employeePayrollInDb = db.EmployeePayrolls.SingleOrDefault(e => e.Id == id);
            if (employeePayrollInDb==null)
            {
                throw new  HttpResponseException(HttpStatusCode.NotFound);
            }

            Mapper.Map<EmployeePayrollDto, EmployeePayroll>(employeePayrollDto,employeePayrollInDb);

            db.SaveChanges();

            //try
            //{
            //    await db.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmployeePayrollExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/EmployeePayrolls
      //  [ResponseType(typeof(EmployeePayroll))]
      [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CreateEmployeePayroll(EmployeePayrollDto employeePayrollDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var employeePayroll = Mapper.Map<EmployeePayrollDto,EmployeePayroll>(employeePayrollDto);

            db.EmployeePayrolls.Add(employeePayroll);
            await db.SaveChangesAsync();

            employeePayrollDto.Id = employeePayroll.Id;
            return Created(new Uri(Request.RequestUri+"/"+employeePayroll.Id), employeePayrollDto);//first agument:URI string,second agument:the object that we created
            
            //return CreatedAtRoute("DefaultApi", new { id = employeePayroll.Id }, employeePayroll);
        }

        // DELETE: api/EmployeePayrolls/5
       // [ResponseType(typeof(EmployeePayrollDto))]
       [System.Web.Http.HttpDelete]
        public async Task<IHttpActionResult> DeleteEmployeePayroll(int id)
        {
            EmployeePayroll employeePayroll = await db.EmployeePayrolls.FindAsync(id);
            if (employeePayroll == null)
            {
                return NotFound();
            }

            db.EmployeePayrolls.Remove(employeePayroll);
            await db.SaveChangesAsync();

            return Ok(employeePayroll);
        }


        //public  IEnumerable<EmployeePayrollDto> GenerateEmployeeFee()
        //{
        //    IJobDetail SyncEmployeeInfo = JobBuilder.Create<Job>()
        //        .WithIdentity("SyncInfo", "Group1")
        //        .Build();

        //    ITrigger trigger = TriggerBuilder.Create()
        //        .WithIdentity("Triggle1", "Group1")
        //        .StartNow()
        //        .WithSimpleSchedule(x=>x.WithIntervalInSeconds(5).WithRepeatCount(5))
        //        .Build();


        //    return db.EmployeePayrolls
        //        .Where(c=>c.PayrollDate>=Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01")))//return current 
        //        .ToList()
        //        .Select(Mapper.Map<EmployeePayroll, EmployeePayrollDto>);
        //}


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        private bool EmployeePayrollExists(int id)
        {
            return db.EmployeePayrolls.Count(e => e.Id == id) > 0;
        }
    }
}