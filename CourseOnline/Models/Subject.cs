using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId  { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<SubjectCourse> SubjectCourses { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}