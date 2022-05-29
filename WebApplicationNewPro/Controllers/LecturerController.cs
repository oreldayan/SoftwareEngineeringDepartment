using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationNewPro.Dal;
using WebApplicationNewPro.Models;
using WebApplicationNewPro.ViewModel;

namespace WebApplicationNewPro.Controllers
{
    public class LecturerController : Controller
    {
        // GET: Lecturer

        public ActionResult Lecturer()
        {
            return View();
        }
        
        public ActionResult ShowSchedule()
        {
            string uID = Session["Id"].ToString();
            CourseDal courseDal = new CourseDal();
            List<Course> course = new List<Course>();
            course.Add(courseDal.courses.Where(x => x.lecturerID.Equals(uID)).First());
            return View(course);
        }

        //------------------------------------------------------------------------------
        public ActionResult StudentsPerCourse()
        {
            return View();
        }
        public ActionResult SubmitStudentsPerCourse(StudentCoursesTests c)
        {
            string uID = Session["Id"].ToString();
            StudentCoursesTestsDal dbsct = new StudentCoursesTestsDal();
            CourseDal courseDal = new CourseDal();
            Course cor = courseDal.courses.Where(x => x.courseID == c.courseID).FirstOrDefault();
            if(cor.lecturerID != uID)
            {
                TempData["message"] = "המרצה לא מלמד את הקורס";
                return View("StudentsPerCourse");
            }
            List<StudentCoursesTests> listStuOfCourse = dbsct.StudentCoursesTestsD.Where(x => x.courseID == c.courseID).ToList<StudentCoursesTests>();
            return View(listStuOfCourse);
        }
        //-------------------------------------------------------------------------------
        //הצגת ציוני בחינות לאחר המועד
        public ActionResult GradeOfTests()
        {
            return View();
        }
        public ActionResult SubmitGradeOfTests(StudentCoursesTests c)
        {
            string uID = Session["Id"].ToString();
            StudentCoursesTestsDal dbsct = new StudentCoursesTestsDal();
            CourseDal courseDal = new CourseDal();
            Course cor = courseDal.courses.Where(x => x.courseID == c.courseID).FirstOrDefault();
            if(cor == null)
            {
                TempData["message"] = "הקורס לא קיים";
                return View("GradeOfTests");
            }
            if (cor.lecturerID != uID)
            {
                TempData["message"] = "המרצה לא מלמד את הקורס";
                return View("GradeOfTests");
            }

            List<StudentCoursesTests> listStuOfCourse = dbsct.StudentCoursesTestsD.Where(x => x.courseID == c.courseID).ToList<StudentCoursesTests>();
            return View(listStuOfCourse);
        }
        //------------------------------------------------------------------------------------------
        //עדכון ציון הקורס
        public ActionResult EditGradeOfStudent()
        {
            return View();
        }

        public ActionResult SubmitEditGradeOfStudent(StudentCoursesTests sct)
        {
            string uID = Session["Id"].ToString();
            StudentCoursesTestsDal dbsct = new StudentCoursesTestsDal();
            CourseDal courseDal = new CourseDal();
            Course cor = courseDal.courses.Where(x => x.courseID == sct.courseID).FirstOrDefault();


            CourseDal dbCourseDal = new CourseDal();
            Course course = dbCourseDal.courses.Where(x => x.courseID == sct.courseID).FirstOrDefault();
            if (course == null)
            {
                TempData["message"] = "The course does not exist !";
                return View("EditGradeOfStudent");
            }

            UsersDal dbusersDal = new UsersDal();
            Users usr = dbusersDal.LogIns.Where(y => y.Id == sct.studentID).FirstOrDefault();
            if (usr == null)
            {
                TempData["message"] = "The student does not exist !";
                return View("EditGradeOfStudent");
            }
            if (cor.lecturerID != uID)
            {
                TempData["message"] = "המרצה לא מלמד את הקורס";
                return View("EditGradeOfStudent");
            }
            StudentCoursesTestsDal SCDal = new StudentCoursesTestsDal();
            StudentCoursesTests stu = SCDal.StudentCoursesTestsD.Where(x => x.studentID == sct.studentID && x.courseID == sct.courseID).FirstOrDefault();

            if (stu == null)
            {
                TempData["message"] = "הסטודנט לא רשום לקורס";
                return View("EditGradeOfStudent");
            }

            if (course.dateMoedA > DateTime.Now && course.dateMoedB > DateTime.Now)
            {
                TempData["message"] = "התאריכים של מועד א ומועד ב עדיין לא עברו לכן אי אפשר לעדכן את הציון";
                return View("EditGradeOfStudent");
            }
            if (course.dateMoedA < DateTime.Now && course.dateMoedB > DateTime.Now)
            {
                return View("EditGradeMoedA", stu);
            }
            if (course.dateMoedA > DateTime.Now && course.dateMoedB < DateTime.Now)
            {
                return View("EditGradeMoedB", stu);
            }

            return View("EditGradeMoedA_MoedB", stu);

        }


        public ActionResult EditGradeMoedA(StudentCoursesTests stu)
        {

            return View();
        }

        public ActionResult SubmitEditGradeMoedA(StudentCoursesTests stu)
        {

            StudentCoursesTestsDal SCDal = new StudentCoursesTestsDal();
            StudentCoursesTests newSCT = SCDal.StudentCoursesTestsD.Where(x => x.studentID == stu.studentID && x.courseID == stu.courseID).FirstOrDefault();
            newSCT.gradeMoedA = stu.gradeMoedA;
            SCDal.SaveChanges();
            TempData["UpdateGradeMoedA"] = "ציון מבחן מועד א' עודכן בהצלחה";
            return View("Lecturer");
        }




        public ActionResult EditGradeMoedB(StudentCoursesTests stu)
        {

            return View();
        }

        public ActionResult SubmitEditGradeMoedB(StudentCoursesTests stu)
        {

            StudentCoursesTestsDal SCDal = new StudentCoursesTestsDal();
            StudentCoursesTests newSCT = SCDal.StudentCoursesTestsD.Where(x => x.studentID == stu.studentID && x.courseID == stu.courseID).FirstOrDefault();
            newSCT.gradeMoedB = stu.gradeMoedB;
            SCDal.SaveChanges();
            TempData["UpdateGradeMoedB"] = "ציון מבחן מועד ב' עודכן בהצלחה";
            return View("Lecturer");
        }

        public ActionResult EditGradeMoedA_MoedB(StudentCoursesTests stu)
        {

            return View();
        }

        public ActionResult SubmitEditGradeMoedA_MoedB(StudentCoursesTests stu)
        {

            StudentCoursesTestsDal SCDal = new StudentCoursesTestsDal();
            StudentCoursesTests newSCT = SCDal.StudentCoursesTestsD.Where(x => x.studentID == stu.studentID && x.courseID == stu.courseID).FirstOrDefault();
            newSCT.gradeMoedA = stu.gradeMoedA;
            newSCT.gradeMoedB = stu.gradeMoedB;
            SCDal.SaveChanges();
            TempData["UpdateGradeMoedA_MoedB"] = "הציונים של מועד א' ומועד ב' עודכנו בהצלחה";
            return View("Lecturer");
        }



    }
}