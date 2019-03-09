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
using System.Web.Http.Description;
using WageManagementSystem.Models;

namespace WageManagementSystem.Controllers.Api
{
    public class EmployeePayrollsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/EmployeePayrolls
        public IQueryable<EmployeePayroll> GetEmployeePayrolls()
        {
            return db.EmployeePayrolls;
        }

        // GET: api/EmployeePayrolls/5
        [ResponseType(typeof(EmployeePayroll))]
        public async Task<IHttpActionResult> GetEmployeePayroll(int id)
        {
            EmployeePayroll employeePayroll = await db.EmployeePayrolls.FindAsync(id);
            if (employeePayroll == null)
            {
                return NotFound();
            }

            return Ok(employeePayroll);
        }

        // PUT: api/EmployeePayrolls/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmployeePayroll(int id, EmployeePayroll employeePayroll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeePayroll.Id)
            {
                return BadRequest();
            }

            db.Entry(employeePayroll).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeePayrollExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/EmployeePayrolls
        [ResponseType(typeof(EmployeePayroll))]
        public async Task<IHttpActionResult> PostEmployeePayroll(EmployeePayroll employeePayroll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EmployeePayrolls.Add(employeePayroll);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = employeePayroll.Id }, employeePayroll);
        }

        // DELETE: api/EmployeePayrolls/5
        [ResponseType(typeof(EmployeePayroll))]
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