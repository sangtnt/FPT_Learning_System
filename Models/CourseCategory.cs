using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FPT_Learning_System.Models
{
    public class CourseCategory
    {
        [Key]
        [Display(Name="CategoryId")]
        public string CategoryId { set; get; }
        [Display(Name ="CategoryName")]
        public string CategoryName { set; get; }
        public virtual ICollection<Course> Courses { set; get; }
    }
}