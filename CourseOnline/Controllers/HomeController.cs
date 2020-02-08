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
using CourseOnline.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebGrease;
using WebGrease.Css.Extensions;


namespace CourseOnline.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private MemberService memberService = new MemberService();

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.ListCategories = db.Categories.Where(c => c.DeletedAt == null).ToList();
            ViewBag.HomeCourse = db.Courses.Where(c => c.DeletedAt == null && c.Status == (int)Course.CourseStatus.Actived).Take(2).ToList();
            ViewBag.TotalCourse = db.Courses.Where(c=>c.DeletedAt == null && c.Status == (int)Course.CourseStatus.Actived).ToList().Count();
            ViewBag.TotalMember = db.Users.Count();
            ViewBag.TotalAuthor = db.Users.Where(u => u.Roles.Select(r => r.RoleId).ToList().Contains("31278a9d-e45f-4026-8d6d-8a8c8f787992")).ToList().Count();
            ViewBag.TotalSubject = db.Categories.Count();
            ViewBag.ThanhToan = "";
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Check expired member ship:
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId();
                var user = UserManager.FindById(id);
                if (memberService.CheckExpiredMember(user) == true)
                {
                    TempData["Expired"] = "Expired!";
                }
            }

            //Vn pay:
            Uri myUri = new Uri(Request.Url.ToString());
            string code = HttpUtility.ParseQueryString(myUri.Query).Get("vnp_ResponseCode");
            string BankCode = HttpUtility.ParseQueryString(myUri.Query).Get("vnp_BankCode");
            Debug.WriteLine("OrderId = " + TempData["OrderId"]);
            if (code == "00")
            {
                ViewBag.ThanhToan = "Success";
                string orderID = (string)TempData["OrderId"];
                var order = db.OrderInfos.Find(orderID);
                if (order == null)
                {
                    return RedirectToAction("NotFound");
                }
                order.Status = (int)OrderInfo.OrderStatus.Paid;
                order.BankCode = BankCode;
                db.OrderInfos.AddOrUpdate(order);

                if (User.Identity.IsAuthenticated)
                {
                    //Luu membership
                    var listMemberType = db.Members.ToList();
                    var memberShip = new Membership();
                    foreach (var memberType in listMemberType)
                    {
                        if (order.Amount == memberType.Price && order.OrderDescription == memberType.MemberType)
                        {
                            memberShip.Member = memberType;
                        }
                    }
                    var id = User.Identity.GetUserId();
                    memberShip.ApplicationUser = UserManager.FindById(id);
                    memberShip.CreatedAt = DateTime.Now;
                    db.Memberships.Add(memberShip);
                    string roleMember = memberShip.Member.MemberType.ToString();
                    //Add role member:
                    UserManager.AddToRole(id, roleMember);
                }

                db.SaveChanges();
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult Shop(string search, int? page, int? category, string fee, string sortOrder)
        {
            ViewBag.ListCategories = db.Categories.Where(c => c.DeletedAt == null).ToList();
            ViewBag.CurrentCatalog = category;
            ViewBag.Free = fee;
            ViewBag.Search = search;
            ViewBag.CurrentSort = sortOrder;
            var predicate = PredicateBuilder.New<Course>(true);
            if (search != null)
            {
                page = 1;
            }
            var listCourse = from c in db.Courses select c;

            if (!String.IsNullOrEmpty(search))
            {
                predicate = predicate.Or(c => c.CourseName.Contains(search));
                predicate = predicate.Or(c => c.ApplicationUser.firstName.Contains(search));
                predicate = predicate.Or(c => c.ApplicationUser.lastName.Contains(search));
            }
            if (category > 0)
            {
                predicate = predicate.Or(c => c.CategoryCourses.Select(cc => cc.CategoryId).ToList().Contains((int)category));
            }
            if (fee == "paid")
            {
                predicate = predicate.And(c => c.Price > 0);
            }
            if (fee == "free")
            {
                predicate = predicate.And(c => c.Price == 0);
            }


            predicate = predicate.And(c => c.Status == (int)Course.CourseStatus.Actived);
            predicate = predicate.And(c => c.DeletedAt == null);
            listCourse = listCourse.Where(predicate);

            switch (sortOrder)
            {
                case "buy":
                    listCourse = listCourse.OrderByDescending(s => s.StudentCourses.Count);
                    break;
                case "date":
                    listCourse = listCourse.OrderByDescending(s => s.CreatedAt);
                    break;
                case "price":
                    listCourse = listCourse.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    listCourse = listCourse.OrderByDescending(s => s.Price);
                    break;
                default:  // Name ascending 
                    listCourse = listCourse.OrderBy(s => s.CourseName);
                    break;
            }

            int pageSize = 9;
            int pageNumber = (page ?? 1);
            //Danh sach id khoa hoc member da mua:
            ViewBag.MyPaidCourse = myPaidCourses();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Check expired member ship:
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = UserManager.FindById(userId);
                if (memberService.CheckExpiredMember(user) == true)
                {
                    TempData["Expired"] = "Expired!";
                }
                if (UserManager.IsInRole(userId, "Silver") || UserManager.IsInRole(userId, "Gold") || UserManager.IsInRole(userId, "Platium"))
                {
                    ViewBag.StillMember = true;
                }
            }
            return View(listCourse.ToPagedList(pageNumber, pageSize));
        }
        [AllowAnonymous]
        public ActionResult ProductDetail(int? id)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Check expired member ship:
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = UserManager.FindById(userId);
                if (memberService.CheckExpiredMember(user) == true)
                {
                    TempData["Expired"] = "Expired!";
                }

                if (UserManager.IsInRole(userId, "Silver") || UserManager.IsInRole(userId, "Gold") || UserManager.IsInRole(userId, "Platium"))
                {
                    ViewBag.StillMember = true;
                }
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null || course.DeletedAt != null || course.Status != (int)Course.CourseStatus.Actived)
            {
                return RedirectToAction("NotFound");
            }
            ViewBag.Benefits = db.Benefits.Where(b => b.Course.CourseId == id).ToList();

            //Danh sach id khoa hoc member da mua:
            ViewBag.MyPaidCourse = myPaidCourses();
            return View(course);
        }
        [Authorize]
        public ActionResult YourCourses(string search, int? page, int? category)
        {
            ViewBag.ListCategories = db.Categories.Where(c => c.DeletedAt == null).ToList();
            ViewBag.CurrentCatalog = category;
            ViewBag.Key = search;

            var predicate = PredicateBuilder.New<Course>(true);
            if (search != null)
            {
                page = 1;
            }

            if (!String.IsNullOrEmpty(search))
            {
                predicate = predicate.Or(c => c.CourseName.Contains(search));
                predicate = predicate.Or(c => c.ApplicationUser.firstName.Contains(search));
                predicate = predicate.Or(c => c.ApplicationUser.lastName.Contains(search));
            }
            if (category > 0)
            {
                predicate = predicate.Or(c => c.CategoryCourses.Select(cc => cc.CategoryId).ToList().Contains((int)category));
            }

            predicate = predicate.And(c => c.Status == (int)Course.CourseStatus.Actived);
            predicate = predicate.And(c => c.DeletedAt == null);
            //Khoa hoc da mua:
            var id = User.Identity.GetUserId();
            predicate = predicate.And(c => c.StudentCourses.Select(sc => sc.MemberId).Contains(id));

            var listCourse = from c in db.Courses select c;
            listCourse = listCourse.Where(predicate).OrderBy(c => c.CourseName);
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(listCourse.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "member,teacher,admin")]
        public ActionResult MemberShip()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //Check expired member ship:
            var id = User.Identity.GetUserId();
            var user = UserManager.FindById(id);
            if (memberService.CheckExpiredMember(user) == false)
            {
                ViewBag.UserMemberShip = db.Memberships.Where(m => m.ApplicationUser.Id == id).ToList()[0];
            }

            ViewBag.ListMemberType = db.Members.ToList();
            return View();
        }
        [Authorize]
        public ActionResult LearnCourse(int? id)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            string currentUserId = User.Identity.GetUserId();
            var user = UserManager.FindById(currentUserId);
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            Course course = db.Courses.Find(id);

            //Kiem tra khoa hoc da duoc mua boi user da dang nhap hoac user da mua member:
            StudentCourse studentCourse = db.StudentCourses.Find(id, currentUserId);
            
            if (studentCourse == null && memberService.CheckExpiredMember(user) == true && course.Price > 0)
            {
                return RedirectToAction("NotFound");
            }
            if (course == null || course.DeletedAt != null || course.Status != (int)Course.CourseStatus.Actived)
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
                myCourse = db.StudentCourses.Where(sc => sc.MemberId == memberId && sc.Course.DeletedAt != null && sc.Course.Status == (int)Course.CourseStatus.Actived).Select(sc=>sc.CourseId).ToList();
            }
            return myCourse;
        }
    }
}