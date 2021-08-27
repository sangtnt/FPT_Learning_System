using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FPT_Learning_System.Models
{
    public class Course
    {
        [Key]
        [Display(Name ="CourseId")]
        public string CourseId { set; get; }
        [Display(Name ="CourseName")]
        public string CourseName { set; get; }
        public CourseCategory CourseCategory { set; get; }
        public virtual ICollection<ApplicationUser> Users { set; get; }
    }
}