using FPT_Learning_System.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using System;
using System.Web;

[assembly: OwinStartupAttribute(typeof(FPT_Learning_System.Startup))]
namespace FPT_Learning_System
{
    public enum Roles
    {
        ROLE_ADMIN,
        ROLE_TRAINER,
        ROLE_TRAINEE,
        ROLE_TRAINING_STAFF
    }
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;
        
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }
        public void CreateDefaultRolesAndUsers()
        {
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            CreateRole(Roles.ROLE_ADMIN.ToString());
            CreateRole(Roles.ROLE_TRAINEE.ToString());
            CreateRole(Roles.ROLE_TRAINER.ToString());
            CreateRole(Roles.ROLE_TRAINING_STAFF.ToString());
            CreateDefaultAdmin();
        }
        void CreateRole(string roleName)
        {
            IdentityRole role = new IdentityRole();
            if (!roleManager.RoleExists(roleName))
            {
                role.Name = roleName;
                roleManager.Create(role);
            }
        }
        void CreateDefaultAdmin()
        {
            var user = new ApplicationUser {
                Email = "sangtntgcs190019@fpt.edu.vn",
                UserName = "sangtntgcs190019@fpt.edu.vn",
                DateOfBirth = DateTime.ParseExact("22/09/2001", "dd/MM/yyyy", null)
            };
            var result = userManager.Create(user, "Sang22092001*");
            if (result.Succeeded)
            {
                userManager.AddToRole(user.Id, Roles.ROLE_ADMIN.ToString());
            }
        }
    }
}
