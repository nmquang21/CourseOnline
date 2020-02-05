using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class Member
    {
        public  int Id { get; set; }
        public  string MemberType { get; set; }
        public  decimal Price { get; set; }
        public int ExpiredMonths { get; set; }
        public int ExpiredYears { get; set; }
    }
}