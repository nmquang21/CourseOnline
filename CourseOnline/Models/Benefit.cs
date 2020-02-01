using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class Benefit
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public virtual Course Course { get; set; }
    }
}