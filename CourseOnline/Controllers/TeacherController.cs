using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseOnline.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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
            ViewBag.ListCategories = db.Categories.Where(c => c.DeletedAt == null).ToList();
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
            newCourse.CreatedAt = DateTime.Now;
            newCourse.ActiveCode = "AC" + DateTime.Now.Ticks.ToString();

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

                if (!String.IsNullOrEmpty(item))
                {
                    var benefit = new Benefit();
                    benefit.name = item;
                    listBenefits.Add(benefit);
                }
                
            }
            if (listBenefits.Count > 0)
            {
                newCourse.Benefits = listBenefits;
            }

            List<ResourceCourse> listResourceCourses = new List<ResourceCourse>();
            foreach (var item in listLesson)
            {
                if (item.Title != null && item.LinkVideo != null)
                {
                    var resourceCourse = new ResourceCourse();
                    resourceCourse.Index = item.Index;
                    resourceCourse.Title = item.Title;
                    resourceCourse.LinkVideo = item.LinkVideo;
                    listResourceCourses.Add(resourceCourse);
                }
            }

            if (listResourceCourses.Count > 0)
            {
                newCourse.ResourceCourses = listResourceCourses;
            }
            newCourse.CategoryCourses = listCategoryCourse;
            db.Courses.Add(newCourse);
            try
            {
                db.SaveChanges();
                TempData["AddSuccess"] = "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("AddCourse");
        }
        [Authorize(Roles = "teacher,admin")]
        public ActionResult UpdateCourse(int? id)
        {
            ViewBag.ListCategories = db.Categories.Where(c => c.DeletedAt == null).ToList();
            string currentUserId = User.Identity.GetUserId();

            var course = db.Courses.Find(id);
            if (course == null || course.DeletedAt != null || course.Status == (int)Course.CourseStatus.Deleted || course.ApplicationUser.Id != currentUserId)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(course);
        }
        [Authorize(Roles = "teacher,admin")]
        public ActionResult StoreUpdateCourse(int courseId, string courseName, List<int> listCategory, string introduceVideo, string price, string image, string description, List<string> listBenefit, List<LessonInfomation> listLesson)
        {
            string currentUserId = User.Identity.GetUserId();
            var currentCourse = db.Courses.Find(courseId);
            if (currentCourse == null || currentCourse.DeletedAt != null || currentCourse.Status == (int)Course.CourseStatus.Deleted || currentCourse.ApplicationUser.Id != currentUserId)
            {
                return RedirectToAction("NotFound", "Home");
            }
            currentCourse.CourseName = courseName;
            currentCourse.Price = Convert.ToDouble(price);
            currentCourse.Description = description;
            currentCourse.IntroduceVideo = introduceVideo;
            currentCourse.Image = image;
            currentCourse.Status = (int)Course.CourseStatus.Non_Active;
            currentCourse.UpdatedAt = DateTime.Now;

            db.CategoryCourses.RemoveRange(currentCourse.CategoryCourses);
            List<CategoryCourse> listCategoryCourse = new List<CategoryCourse>();
            foreach (var item in listCategory)
            {
                var categoryCourse = new CategoryCourse();
                categoryCourse.CategoryId = item;
                categoryCourse.CourseId = currentCourse.CourseId;
                listCategoryCourse.Add(categoryCourse);
            }

            db.Benefits.RemoveRange(currentCourse.Benefits);
            List<Benefit> listBenefits = new List<Benefit>();
            foreach (var item in listBenefit)
            {

                if (!String.IsNullOrEmpty(item))
                {
                    var benefit = new Benefit();
                    benefit.name = item;
                    listBenefits.Add(benefit);
                }

            }
            if (listBenefits.Count > 0)
            {
                currentCourse.Benefits = listBenefits;
            }

            db.ResourceCourses.RemoveRange(currentCourse.ResourceCourses);
            List<ResourceCourse> listResourceCourses = new List<ResourceCourse>();
            foreach (var item in listLesson)
            {
                if (item.Title != null && item.LinkVideo != null)
                {
                    var resourceCourse = new ResourceCourse();
                    resourceCourse.Index = item.Index;
                    resourceCourse.Title = item.Title;
                    resourceCourse.LinkVideo = item.LinkVideo;
                    listResourceCourses.Add(resourceCourse);
                }
            }

            if (listResourceCourses.Count > 0)
            {
                currentCourse.ResourceCourses = listResourceCourses;
            }
            currentCourse.CategoryCourses = listCategoryCourse;

            db.Courses.AddOrUpdate(currentCourse);
            try
            {
                db.SaveChanges();
                TempData["AddSuccess"] = "Success";
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