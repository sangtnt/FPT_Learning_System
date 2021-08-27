using FPT_Learning_System.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FPT_Learning_System.Areas.Manager.Controllers
{
    [Authorize(Roles = "ROLE_TRAINING_STAFF, ROLE_ADMIN")]
    public class TraineeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
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
            var role = RoleManager.FindByName(Roles.ROLE_TRAINEE.ToString());
            var trainees = UserManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id)).ToList();
            return View(trainees);
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            List < SelectListItem > roles = new List<SelectListItem>();
            foreach(var role in RoleManager.Roles)
            {
                roles.Add(new SelectListItem() { 
                    Value = role.Name,
                    Text = role.Name
                });
            }
            ViewBag.Roles = roles;
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null){
                ViewBag.ErrorMessage = $"User with {id} can not be found!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(user);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateTraineeView model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone,
                    DateOfBirth = model.DoB,
                    Education = model.Education,
                    MainProgrammingLanguages = model.MainProgrammingLanguage,
                    ToeicScore = model.ToeicScore,
                    ExperienceDetails = model.ExperienceDetails,
                    Location = model.Location
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await UserManager.AddToRoleAsync(user.Id, Roles.ROLE_TRAINEE.ToString());
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
                user.ToeicScore = model.ToeicScore;
                user.Education = model.Education;
                user.MainProgrammingLanguages = model.MainProgrammingLanguages;
                user.Location = model.Location;
                user.ExperienceDetails = model.ExperienceDetails;
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Details", new { id = user.Id });
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var user = UserManager.FindById(id);
            return View(user);
        }

        [HttpGet]
        [Authorize(Roles ="ROLE_TRAINING_STAFF")]
        public ActionResult AssignTraineeToCourse()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "CourseName");
            var role = RoleManager.FindByName(Roles.ROLE_TRAINEE.ToString());
            var trainers = UserManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));
            ViewBag.UserId = new SelectList(trainers, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ROLE_TRAINING_STAFF")]
        public ActionResult AssignTraineeToCourse([Bind(Include = "UserId,CourseId")] UserCourse userCourse)
        {
            if (ModelState.IsValid)
            {
                db.UserCourses.Add(userCourse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "Id", "CourseName");
            var role = RoleManager.FindByName(Roles.ROLE_TRAINEE.ToString());
            var trainers = UserManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id));
            ViewBag.UserId = new SelectList(trainers, "Id", "Email");
            return View(userCourse);
        }
    }
}