using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseOnline.Models;

namespace CourseOnline.Service
{
    public class OrderService
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public bool createOrder(List<int> listCourseId, string paymentType, string amount, string firstName, string lastName, string email, string phone, string tinh, string quan, string xa, string address, string memberId)
        {
            if (listCourseId.Count == 0)
            {
                return false;
            }
            var order = new OrderInfo();
            order.OrderId = "DH" + DateTime.Now.Ticks.ToString();
            order.PaymentTypeId = Convert.ToInt16(paymentType);
            order.MemberId = memberId;
            order.firstName = firstName;
            order.lastName = lastName;
            order.email = email;
            order.phone = phone;
            order.tinh = tinh;
            order.quan = quan;
            order.xa = xa;
            order.address = address;

            order.Amount = Convert.ToDecimal(amount);

            var orderDetails = new List<OrderDetail>();
            bool existError = false;
            foreach (var item in listCourseId)
            {
                var course = db.Courses.Find(item);
                if (course == null)
                {
                    existError = true;
                    break;
                }
                var orderDetail = new OrderDetail();

                orderDetail.CourseId = item;
                orderDetail.OrderId = order.OrderId;
                orderDetail.Price = course.Price;
                orderDetails.Add(orderDetail);
            }
            if (!existError)
            {
                order.OrderDetails = orderDetails;
                order.Status = Convert.ToInt16(OrderInfo.OrderStatus.Pending);
                order.CreatedAt = DateTime.Now.ToString();
                db.OrderInfos.Add(order);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                return true;
            }
            return false;
        }
    }
}