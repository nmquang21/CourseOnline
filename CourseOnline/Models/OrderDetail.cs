using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class OrderDetail
    {
        public double Price { get; set; }
        [Key, Column(Order = 0)]
        public int CourseId { get; set; }
        [Key, Column(Order = 1)]
        public string OrderId { get; set; }
        public virtual Course Course { get; set; }
        public virtual OrderInfo OrderInfo { get; set; }
    }
}