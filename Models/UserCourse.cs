using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FPT_Learning_System.Models
{
    public class UserCourse
    {
        [Key] 
        [Required]
        [Display(Name ="User")]
        [Column(Order =0)]
        public string UserId { set; get; }
        public virtual ApplicationUser User { set; get; }
        [Key]
        [Column(Order = 1)]
        [Required]
        [Display(Name ="Course")]
        public Guid CourseId { set; get; }
        public virtual Course Course { set; get; }
    }
}