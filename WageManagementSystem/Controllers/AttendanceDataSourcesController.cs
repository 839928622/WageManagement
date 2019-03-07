using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WageManagementSystem.Models;

namespace WageManagementSystem.Controllers
{
    public class AttendanceDataSourcesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AttendanceDataSources
        public ActionResult Index()
        {
            return View(db.AttendanceDataSourceses.ToList());
        }

        // GET: AttendanceDataSources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendanceDataSources attendanceDataSources = db.AttendanceDataSourceses.Find(id);
            if (attendanceDataSources == null)
            {
                return HttpNotFound();
            }
            return View(attendanceDataSources);
        }

        // GET: AttendanceDataSources/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttendanceDataSources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Branch,AttendenceSources")] AttendanceDataSources attendanceDataSources)
        {
            if (ModelState.IsValid)
            {
                db.AttendanceDataSourceses.Add(attendanceDataSources);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(attendanceDataSources);
        }

        // GET: AttendanceDataSources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendanceDataSources attendanceDataSources = db.AttendanceDataSourceses.Find(id);
            if (attendanceDataSources == null)
            {
                return HttpNotFound();
            }
            return View(attendanceDataSources);
        }

        // POST: AttendanceDataSources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Branch,AttendenceSources")] AttendanceDataSources attendanceDataSources)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendanceDataSources).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attendanceDataSources);
        }

        // GET: AttendanceDataSources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttendanceDataSources attendanceDataSources = db.AttendanceDataSourceses.Find(id);
            if (attendanceDataSources == null)
            {
                return HttpNotFound();
            }
            return View(attendanceDataSources);
        }

        // POST: AttendanceDataSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AttendanceDataSources attendanceDataSources = db.AttendanceDataSourceses.Find(id);
            db.AttendanceDataSourceses.Remove(attendanceDataSources);
            db.SaveChanges();
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
