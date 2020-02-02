using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class CategoryCourse
    {
        [Key]
        public int CategoryCourseId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter phone Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}