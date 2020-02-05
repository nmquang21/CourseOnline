using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class OrderInfo
    {
        [Key]
        public string OrderId { get; set; }
        public string MemberId { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string DeletedAt { get; set; }
        public string OrderDescription { get; set; }
        public string BankCode { get; set; }
        public decimal vnp_TransactionNo { get; set; }
        public string vpn_Message { get; set; }
        public string vpn_TxnResponseCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string tinh { get; set; }
        public string quan { get; set; }
        public string xa { get; set; }
        public string address { get; set; }
        public int orderType { get; set; }
        public enum PaymentType
        {
            Cod = 1,
            InternetBanking = 2,
            DirectTransfer = 3
        }
        public enum OrderStatus
        {
            Pending = 5,
            Confirmed = 4,
            Shipping = 3,
            Paid = 2,
            Done = 1,
            Cancel = 0,
            Deleted = -1
        }
        public enum OrderType
        {
            BuyCourse = 1,
            MemberShip = 2
        }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}