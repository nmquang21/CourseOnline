using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class Membership
    {
        [Key]
        public int ID { get; set; }
        //public int MembershipRole { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual Member Member { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        //public enum MembershipRoleEnum
        //{
        //    SilverMember = 1,
        //    GoldMember = 2,
        //    PlatinumMember = 3
        //}
    }
}