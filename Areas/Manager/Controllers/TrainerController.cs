using FPT_Learning_System.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FPT_Learning_System.Areas.Manager.Controllers
{
    [Authorize(Roles ="ROLE_TRAINING_STAFF, ROLE_ADMIN")]
    public class TrainerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
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
        // GET: Manager/Trainer
        public ActionResult Index()
        {
            var role = RoleManager.FindByName(Roles.ROLE_TRAINER.ToString());
            var trainers = UserManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id)).ToList();
            return View(trainers);
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateTrainerView model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = model.Email, 
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone,
                    DateOfBirth = model.DoB,
                    Type = model.Type,
                    WorkingPlace = model.WorkingPlace
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await UserManager.AddToRoleAsync(user.Id, Roles.ROLE_TRAINER.ToString());
                    return RedirectToAction("Details", new { id = user.Id });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {id} can not be found!";
                return RedirectToAction("Index");
            }
            else
            {
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                AddErrors(result);
                return RedirectToAction("Index");
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
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationUser model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {model.Id} can not be found!";
                return RedirectToAction("Index");
            }
            else
            {
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.DateOfBirth = model.DateOfBirth;
                user.Type = model.Type;
                user.WorkingPlace = model.WorkingPlace;
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Details", new {id=user.Id });
                }
                AddErrors(result);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {id} can not be found!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(user);
            }
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var user = UserManager.FindById(id);
            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_TRAINING_STAFF")]
        public ActionResult AssignTrainerToCourse(string id)
        {
            ViewBag.UserId = id;
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "CourseName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ROLE_TRAINING_STAFF")]
        public ActionResult AssignTrainerToCourse([Bind(Include = "UserId,CourseId")] UserCourse userCourse)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.UserCourses.Add(userCourse);
                    db.SaveChanges();
                    return RedirectToAction("ViewCourses", new { id = userCourse.UserId });
                }
                catch
                {
                    ViewBag.UserId = userCourse.UserId;
                    ViewBag.CourseId = new SelectList(db.Courses, "Id", "CourseName");
                    ViewBag.Message = "This course've been already assigned to User";
                    return View(userCourse);
                }
            }

            ViewBag.UserId = userCourse.UserId;
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "CourseName");
            return View(userCourse);
        }
        [HttpGet]
        public ActionResult ViewCourses(string id)
        {
            var courses = db.UserCourses.Where(uc => uc.UserId == id).ToList();
            var user = UserManager.FindById(id);
            ViewBag.User = user;
            return View(courses);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserCourse([Bind(Include = "UserId,CourseId")] UserCourse userCourse)
        {
            var target = db.UserCourses.Where(uc => uc.CourseId == userCourse.CourseId | uc.UserId == userCourse.UserId).FirstOrDefault();
            db.UserCourses.Remove(target);
            db.SaveChanges();
            return RedirectToAction("ViewCourses", "Trainer", new { id = userCourse.UserId });
        }
    }
}