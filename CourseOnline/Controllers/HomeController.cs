using CourseOnline.Models;
using LinqKit;
using log4net;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebGrease;


namespace CourseOnline.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Shop(string search, int? page)
        {
            var predicate = PredicateBuilder.New<Course>(true);
            if (search != null)
            {
                page = 1;
            }
            ViewBag.Search = search;
            var listCourse = from c in db.Courses select c;

            if (!String.IsNullOrEmpty(search))
            {
                predicate = predicate.Or(c => c.CourseName.Contains(search));
                predicate = predicate.Or(c => c.ApplicationUser.firstName.Contains(search));
                predicate = predicate.Or(c => c.ApplicationUser.lastName.Contains(search));
            }
            predicate = predicate.And(c => c.Status == (int)Course.CourseStatus.Actived);
            predicate = predicate.And(c => c.DeletedAt == null);
            listCourse = listCourse.Where(predicate).OrderBy(c => c.CourseName);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            //Danh sach id khoa hoc member da mua:
            ViewBag.MyPaidCourse = myPaidCourses();
            return View(listCourse.ToPagedList(pageNumber, pageSize));
        }
        [AllowAnonymous]
        public ActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return RedirectToAction("NotFound");
            }
            ViewBag.Benefits = db.Benefits.Where(b => b.Course.CourseId == id).ToList();

            //Danh sach id khoa hoc member da mua:
            ViewBag.MyPaidCourse = myPaidCourses();
            return View(course);
        }
        [Authorize]
        public ActionResult YourCourses()
        {
            return View();
        }
        [Authorize]
        public ActionResult MemberShip()
        {
            return View();
        }
        [Authorize]
        public ActionResult LearnCourse(int? id)
        {
            string currentUserId = User.Identity.GetUserId();
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Course course = db.Courses.Find(id);
            //Kiem tra khoa hoc da duoc mua boi user da dang nhap:
            StudentCourse studentCourse = db.StudentCourses.Find(id, currentUserId);
            
            if (studentCourse == null)
            {
                return RedirectToAction("NotFound");
            }
            if (course == null)
            {
                return RedirectToAction("NotFound");
            }
            //Danh sach id khoa hoc member da mua:
            ViewBag.MyPaidCourse = myPaidCourses();
            return View(course);
        }
        [AllowAnonymous]
        public ActionResult Cart()
        {
            Uri myUri = new Uri(Request.Url.ToString());
            string code = HttpUtility.ParseQueryString(myUri.Query).Get("vnp_ResponseCode");
            string BankCode = HttpUtility.ParseQueryString(myUri.Query).Get("vnp_BankCode");
            Debug.WriteLine("OrderId = " + TempData["OrderId"]);
            if (code == "00")
            {
                ViewBag.ThanhToan = "Success!";
                string orderID = (string)TempData["OrderId"];
                var order = db.OrderInfos.Find(orderID);
                List<OrderDetail> listCourse = db.OrderDetails.Where(od => od.OrderId == orderID).ToList();
                if (order == null)
                {
                    return RedirectToAction("NotFound");
                }
                order.Status = (int)OrderInfo.OrderStatus.Paid;
                order.BankCode = BankCode;
                foreach (var item in listCourse)
                {
                    //Debug.WriteLine("courseID= "+item.CourseId);
                    var studentCourse = new StudentCourse();
                    studentCourse.MemberId = order.MemberId;
                    studentCourse.CourseId = item.CourseId;
                    db.StudentCourses.Add(studentCourse);
                    db.SaveChanges();
                }
                db.OrderInfos.AddOrUpdate(order);
                db.SaveChanges();
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult CheckOut()
        {
            Debug.WriteLine("is authorize");
            Debug.WriteLine(Request.IsAuthenticated);
            var id = User.Identity.GetUserId();
            ApplicationUser appUser = new ApplicationUser();
            appUser = db.Users.Find(id);
            if (appUser != null)
            {
                ViewData["currentUser"] = appUser;
            }
            return View();
        }
        public ActionResult OrderReceived()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
        public List<int> myPaidCourses()
        {
            var myCourse = new List<int>();
            var memberId = User.Identity.GetUserId();
            if (Request.IsAuthenticated)
            {
                myCourse = db.StudentCourses.Where(sc => sc.MemberId == memberId).Select(sc=>sc.CourseId).ToList();
            }
            return myCourse;
        }
    }
}