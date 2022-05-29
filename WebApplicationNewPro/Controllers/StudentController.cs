using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationNewPro.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using WebApplicationNewPro.Dal;

namespace WebApplicationNewPro.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Student()
        {

            return View("Student");

        }
        public ActionResult ShowCoursesSchedule()
        {
            string uID = Session["Id"].ToString();
            StudentCoursesTestsDal course_of_student_Dal = new StudentCoursesTestsDal();
            CourseDal courseDal = new CourseDal();
            List<StudentCoursesTests> sct = new List<StudentCoursesTests>();
            List<Course> course = new List<Course>();
            List<string> courses = new List<string>();

            courses = course_of_student_Dal.StudentCoursesTestsD.Where(x => x.studentID.Equals(uID)).Select(y => y.courseID).ToList();
            
            foreach(string c in courses)
            {
                course.Add(courseDal.courses.Where(x => x.courseID.Equals(c)).First());
            }

            return View(course);
        }
        public ActionResult ShowTests()
        {
            string uID = Session["Id"].ToString();
            StudentCoursesTestsDal course_of_student_Dal = new StudentCoursesTestsDal();
            CourseDal courseDal = new CourseDal();
            List<StudentCoursesTests> sct = new List<StudentCoursesTests>();
            List<Course> course = new List<Course>();
            List<string> courses = new List<string>();

            courses = course_of_student_Dal.StudentCoursesTestsD.Where(x => x.studentID.Equals(uID)).Select(y => y.courseID).ToList();

            foreach (string c in courses)
            {
                course.Add(courseDal.courses.Where(x => x.courseID.Equals(c)).First());
            }
            return View(course);
        }


    }
}