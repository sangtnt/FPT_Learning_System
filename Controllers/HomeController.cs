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
    [Authorize]
    public class HomeController : Controller
    {
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
        public ActionResult Index()
        {
            if (User.IsInRole(Roles.ROLE_ADMIN.ToString())){
                return RedirectToAction("Index", "TrainingStaff", new { area="Manager"});
            } 
            else if (User.IsInRole(Roles.ROLE_TRAINING_STAFF.ToString()))
            {
                return RedirectToAction("Index", "Trainer", new { area = "Manager" });
            }
            else if (User.IsInRole(Roles.ROLE_TRAINER.ToString()))
            {
                return RedirectToAction("Index", "TrainerClient");
            }
            else
            {
                return RedirectToAction("Index", "TraineeClient");
            }
        }
    }
}