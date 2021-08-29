using FPT_Learning_System.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FPT_Learning_System.Controllers
{
    [Authorize(Roles ="ROLE_TRAINER")]
    public class TrainerClientController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
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
        // GET: TrainerClient
        public ActionResult Index()
        {
            var userEmail = User.Identity.GetUserName();
            var courses = db.UserCourses.Where(uc => uc.User.UserName== userEmail).ToList();
            return View(courses);
        }
        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            var email = User.Identity.GetUserName();
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {email} can not be found!";
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
                    return RedirectToAction("Details");
                }
                AddErrors(result);
            }
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> Details()
        {
            var email = User.Identity.GetUserName();
            var user = await UserManager.FindByEmailAsync(email);
            return View(user);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}