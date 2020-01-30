using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class SubjectCourse
    {
        [Key]
        public int SubjectCourseId { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}