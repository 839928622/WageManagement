using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using WageManagementSystem.Models;

namespace WageManagementSystem.Controllers
{
    public class EmployeePayrollsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
        public async Task<ActionResult> Edit([Bind(Include = "Id,PayrollDate,EmoloyeeNumber,EmployeeName,Gender,Attendance,OverTime,Salary,AttendanceDataSources,ComPosition,ComRank,EnrollMentDate,ResignationDate,Deadline,SchoolName,FeeType,ReleaseType")] EmployeePayroll employeePayroll)
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
