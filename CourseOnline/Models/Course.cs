using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter phone course name")]
        [DisplayName("Course Name")]
        public string CourseName { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter phone price")]
        public double Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter phone description")]
        public string Description { get; set; }

        public string Image { get; set; }
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
        public virtual ICollection<SubjectCourse> SubjectCourses { get; set; }
        public virtual ICollection<ResourceCourse> ResourceCourses  { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


    }
}