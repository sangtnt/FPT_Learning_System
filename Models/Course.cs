using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FPT_Learning_System.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { set; get; }
        [Display(Name ="Course Name")]
        public string CourseName { set; get; }

        public Guid CourseCategoryId { set; get; }
        public virtual CourseCategory CourseCategory { set; get; }
        public virtual ICollection<UserCourse> Users { set; get; }
    }
}