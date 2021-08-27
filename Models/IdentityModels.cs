using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace FPT_Learning_System.Models
{
    public enum TrainerType
    {
        INTERNAL, EXTERNAL
    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { set; get; }
        [Display(Name = "Last Name")]
        public string LastName { set; get; }
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { set; get; }
        [Display(Name = "Avartar")]
        //Properties of Trainee
        public string Avartar { set; get; }
        [Display(Name ="Education")]
        public string Education { set; get; }
        [Display(Name = "Main Programming Languages")]
        public string MainProgrammingLanguages { set; get; }
        [Display(Name ="Toeic Score")]
        public float ToeicScore { set; get; }
        [Display(Name ="Experience Details")]
        [DataType(DataType.MultilineText)]
        public string ExperienceDetails { set; get; }
        [Display(Name ="Location")]
        [DataType(DataType.MultilineText)]
        public string Location { set; get; }
        // Properties of Trainer
        [Display(Name ="Type")]
        public string Type { set; get; }
        [Display(Name ="Working Place")]
        [DataType(DataType.MultilineText)]
        public string WorkingPlace { set; get; }
        public virtual ICollection<UserCourse> Courses { set; get; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<FPT_Learning_System.Models.Course> Courses { get; set; }

        public System.Data.Entity.DbSet<FPT_Learning_System.Models.CourseCategory> CourseCategories { get; set; }
        public System.Data.Entity.DbSet<FPT_Learning_System.Models.UserCourse> UserCourses { get; set; }
    }
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
        : base(roleStore) { }

        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationRoleManager(
                new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
            return manager;
        }
    }
}