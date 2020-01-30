using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class ResourceCourse
    {
        [Key]
        public int ResourceCourseId { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public int Type { get; set; }
        public int Index { get; set; }
        public string LinkVideo { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}