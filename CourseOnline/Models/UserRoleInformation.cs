using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class UserRoleInformation
    {
        [Key]
        public int Id { get; set; }
        public string UserRoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}