using CourseOnline.Models;
using LinqKit;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CourseOnline.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
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
                predicate = predicate.Or(c => c.Author.AuthorName.Contains(search));
            }
            predicate = predicate.And(c => c.DeletedAt == null);
            listCourse = listCourse.Where(predicate).OrderBy(c => c.CourseName);

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(listCourse.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ProductDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        public ActionResult YourCourses()
        {
            return View();
        }
        public ActionResult MemberShip()
        {
            return View();
        }
        
        public ActionResult Cart()
        {
            return View();
        }
        public ActionResult CheckOut()
        {
            return View();
        }
        public ActionResult OrderReceived()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}