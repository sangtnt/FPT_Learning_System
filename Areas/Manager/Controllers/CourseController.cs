using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FPT_Learning_System.Models;

namespace FPT_Learning_System.Areas.Manager.Controllers
{
    public class CourseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Manager/Course
        public ActionResult Index()
        {
            var courses = db.Courses.Include(c => c.CourseCategory);
            return View(courses.ToList());
        }

        // GET: Manager/Course/Create
        public ActionResult Create()
        {
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "Id", "CategoryName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CourseName,CourseCategoryId")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.Id = Guid.NewGuid();
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "Id", "CategoryName", course.CourseCategoryId);
            return View(course);
        }
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "Id", "CategoryName", course.CourseCategoryId);
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseName,CourseCategoryId")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseCategoryId = new SelectList(db.CourseCategories, "Id", "CategoryName", course.CourseCategoryId);
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
