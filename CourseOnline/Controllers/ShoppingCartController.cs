using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseOnline.Models;
using CourseOnline.Service;
using log4net;
using Microsoft.AspNet.Identity;
using LogManager = log4net.LogManager;
namespace CourseOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private OrderService orderService = new OrderService();

        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult VnPay(string amount, string description, List<int> listCourseId)
        {
            //Get Config Info
            string vnp_Returnurl = "https://localhost:44374/Home/Cart"; //URL nhan ket qua tra ve 
            string vnp_Url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = "P1V8JH37"; //Ma website
            string vnp_HashSecret = "XAUJIMFNKYUUWWNWOLLNIHJCUGLOIGEF"; //Chuoi bi mat

            //Get payment input
            OrderInfo order = new OrderInfo();
            //Save order to db
            order.OrderId = "DH" + DateTime.Now.Ticks.ToString();
            TempData["OrderId"] = order.OrderId;
            var orderDetails = new List<OrderDetail>();
            foreach (var item in listCourseId)
            {
                var course = db.Courses.Find(item);
                if (course == null)
                {
                    break;
                }
                var orderDetail = new OrderDetail();

                orderDetail.CourseId = item;
                orderDetail.OrderId = order.OrderId;
                orderDetail.Price = course.Price;
                orderDetails.Add(orderDetail);
            }
            order.OrderDetails = orderDetails;
            order.Status = Convert.ToInt16(OrderInfo.OrderStatus.Pending);
            order.MemberId = User.Identity.GetUserId();
            order.PaymentTypeId = Convert.ToInt16(OrderInfo.PaymentType.DirectTransfer);
            order.Amount = Convert.ToDecimal(amount);
            if (description == null)
            {
                description = "NA";
            }
            order.OrderDescription = description;
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
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.0.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_BankCode", "NCB");
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", order.OrderDescription);
            vnpay.AddRequestData("vnp_OrderType", "190002"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            //Response.Redirect(paymentUrl);
            Debug.WriteLine(paymentUrl);
            Console.WriteLine(paymentUrl);
            return Redirect(paymentUrl);
        }

        public ActionResult CODPay(List<int> listCourseId, string paymentType, string amount, string firstName, string lastName, string email, string phone, string tinh, string quan, string xa, string address)
        {
            var memberId = User.Identity.GetUserId();
            if (orderService.createOrder(listCourseId, paymentType,amount, firstName, lastName, email, phone, tinh, quan, xa, address, memberId))
            {
                TempData["msg"] = "Order success!";
                //Xoa session:
                Debug.WriteLine("Da xoa session");
                Session["shoppingCart"] = null;
            }
            return Redirect("/Home/Cart");
        }
    }
}