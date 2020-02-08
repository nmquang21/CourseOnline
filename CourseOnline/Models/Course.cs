using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

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
        public string IntroduceVideo { get; set; }
        public string Image { get; set; }
        public string TeacherId { get; set; }
        public int Status { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<SubjectCourse> SubjectCourses { get; set; }
        public virtual ICollection<ResourceCourse> ResourceCourses  { get; set; }
        public virtual ICollection<Benefit> Benefits { get; set; }
        public virtual ICollection<CategoryCourse> CategoryCourses { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public enum CourseStatus
        {
            Updated = 3,
            Non_Active = 2,
            Actived = 1,
            Deleted = 0
        }

    }
}