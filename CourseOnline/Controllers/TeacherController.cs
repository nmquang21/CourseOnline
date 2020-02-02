using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseOnline.Models;
using Microsoft.AspNet.Identity;

namespace CourseOnline.Controllers
{
    public class TeacherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Teacher
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "teacher,admin")]
        public ActionResult Profile()
        {
            return View();
        }
        [Authorize(Roles = "teacher,admin")]
        public ActionResult AddCourse()
        {
            ViewBag.ListCategories = db.Categories.ToList();
            return View();
        }
        [Authorize(Roles = "teacher,admin")]
        public ActionResult StoreAddCourse(string courseName, List<int> listCategory, string introduceVideo, string price, string image, string description, List<string> listBenefit, List<LessonInfomation> listLesson)
        {
            var newCourse = new Course();
            newCourse.CourseName = courseName;
            newCourse.Price = Convert.ToDouble(price);
            newCourse.Description = description;
            newCourse.IntroduceVideo = introduceVideo;
            newCourse.Image = image;

            var id = User.Identity.GetUserId();
            ApplicationUser appUser = new ApplicationUser();
            appUser = db.Users.Find(id);
            newCourse.TeacherId = id;
            newCourse.ApplicationUser = appUser;
            newCourse.Status = (int)Course.CourseStatus.Non_Active;

            List<CategoryCourse> listCategoryCourse = new List<CategoryCourse>();
            foreach (var item in listCategory)
            {
                var categoryCourse = new CategoryCourse();
                categoryCourse.CategoryId = item;
                categoryCourse.CourseId = newCourse.CourseId;
                listCategoryCourse.Add(categoryCourse);
            }
          
            List<Benefit> listBenefits = new List<Benefit>();
            foreach (var item in listBenefit)
            {
                var benefit = new Benefit();
                benefit.name = item;
                listBenefits.Add(benefit);
            }
            List<ResourceCourse> listResourceCourses = new List<ResourceCourse>();
            foreach (var item in listLesson)
            {
                var resourceCourse = new ResourceCourse();
                resourceCourse.Index = item.Index;
                resourceCourse.Title = item.Title;
                resourceCourse.LinkVideo = item.LinkVideo;
                listResourceCourses.Add(resourceCourse);
            }

            newCourse.CategoryCourses = listCategoryCourse;
            newCourse.Benefits = listBenefits;
            newCourse.ResourceCourses = listResourceCourses;
            db.Courses.Add(newCourse);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("AddCourse");
        }
    }
}