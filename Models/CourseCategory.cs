using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FPT_Learning_System.Models
{
    public class CourseCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { set; get; }
        [Display(Name ="Category")]
        public string CategoryName { set; get; }

        public virtual ICollection<Course> Courses { set; get; }
    }
}