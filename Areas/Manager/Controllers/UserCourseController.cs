using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FPT_Learning_System.Models;
using Microsoft.AspNet.Identity.Owin;

namespace FPT_Learning_System.Areas.Manager.Controllers
{
    public class UserCourseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index()
        {
            var userCourses = db.UserCourses.Include(u => u.Course).Include(u => u.User);
            return View(userCourses.ToList());
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCourse userCourse = db.UserCourses.Find(id);
            if (userCourse == null)
            {
                return HttpNotFound();
            }
            return View(userCourse);
        }
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "CourseName");
            ViewBag.UserId = new SelectList(UserManager.Users, "Id", "FirstName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,CourseId")] UserCourse userCourse)
        {
            if (ModelState.IsValid)
            {
                db.UserCourses.Add(userCourse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "CourseName", userCourse.CourseId);
            ViewBag.UserId = new SelectList(UserManager.Users, "Id", "FirstName", userCourse.UserId);
            return View(userCourse);
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCourse userCourse = db.UserCourses.Find(id);
            if (userCourse == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "CourseName", userCourse.CourseId);
            ViewBag.UserId = new SelectList(UserManager.Users, "Id", "FirstName", userCourse.UserId);
            return View(userCourse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,CourseId")] UserCourse userCourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userCourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "CourseName", userCourse.CourseId);
            ViewBag.UserId = new SelectList(UserManager.Users, "Id", "FirstName", userCourse.UserId);
            return View(userCourse);
        }
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCourse userCourse = db.UserCourses.Find(id);
            if (userCourse == null)
            {
                return HttpNotFound();
            }
            return View(userCourse);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserCourse userCourse = db.UserCourses.Find(id);
            db.UserCourses.Remove(userCourse);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
