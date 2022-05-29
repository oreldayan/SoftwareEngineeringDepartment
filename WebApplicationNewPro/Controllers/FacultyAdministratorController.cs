using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationNewPro.Dal;
using WebApplicationNewPro.Models;

namespace WebApplicationNewPro.Controllers
{
    public class FacultyAdministratorController : Controller
    {
        // GET: FacultyAdministrator
        public ActionResult FacultyAdministrator()
        {
            return View();
        }
        //--------------------------------------------------------------------------------------------------
        //כפתור הוספת קורס חדש 
        public ActionResult AddCourseToLecturer()
        {
            
            return View();
        }

        public ActionResult SubmitAddCourseToLecturer(Course c)
        {
            CourseDal dbcourse = new CourseDal();

            List<Course> currentCourses = dbcourse.courses.ToList<Course>();
            foreach (Course cor in currentCourses)
            {
                if(cor.beginningTime == c.beginningTime && cor.day == c.day && cor.placeCourse == c.placeCourse)
                {
                    TempData["addCourse"] = "הקורס החדש הוא באותו יום שעה וכיתה של קורס שכבר קיים";
                    return View("AddCourseToLecturer");
                }

                if(cor.dateMoedA == c.dateMoedA && cor.placeMoedA == c.placeMoedA)
                {
                    TempData["addCourse"] = "מועד א של הקורס החדש הוא באותו יום וכיתה של מועד א' של קורס שכבר קיים";
                    return View("AddCourseToLecturer");
                }
                if (cor.dateMoedB == c.dateMoedB && cor.placeMoedB == c.placeMoedB)
                {
                    TempData["addCourse"] = "מועד ב' של הקורס החדש הוא באותו יום וכיתה של מועד ב' של קורס שכבר קיים";
                    return View("AddCourseToLecturer");
                }

            }
            Course co = dbcourse.courses.Where(x => x.courseID == c.courseID || x.courseName == c.courseName).FirstOrDefault();
            if (co != null)
            {
                TempData["addCourse"] = "הקורס כבר קיים";
                return View("AddCourseToLecturer");
            }

            UsersDal usrD = new UsersDal();
            Users lec = usrD.LogIns.Where(x => x.Id == c.lecturerID).FirstOrDefault();
            if (lec == null)
            {
                TempData["addCourse"] = "ID of lecturer not exists";
                return View("AddCourseToLecturer");
            }

            if (lec.Permission != "2")
            {
                TempData["messageAssigning"] = "The user you entered is not a lecturer !";
                return View("AssigningStudentToCourse");
            }

            List<Course> coursesOfLect = dbcourse.courses.Where(x=> x.lecturerID == c.lecturerID).ToList<Course>();
            foreach(Course cL in coursesOfLect)
            {
                if(cL.day == c.day && cL.beginningTime == c.beginningTime)
                {
                    TempData["addCourse"] = "המרצה כבר מלמד בשעות אילו";
                    return View("AddCourseToLecturer");
                }

            }
            if (ModelState.IsValid)
            {
                dbcourse.courses.Add(c);
                dbcourse.SaveChanges();
                TempData["addCourse"] = "הקורס נוסף בהצלחה";
                return View("FacultyAdministrator");
            }
            else
            {
                return View("AddCourseToLecturer", c);
            }
        }
        //-----------------------------------------------------------------------------------------------
        //כפתור הקצאת סטודנט קיים לקורס קיים
        public ActionResult AssigningStudentToCourse()
        {
            return View();
        }

        public ActionResult SubmitAssigningStudentToCourse(StudentCoursesTests s)
        {
            

            string studentName = s.studentName.ToString();
            string studentID = s.studentID.ToString();
            string courseName = s.courseName.ToString();
            string courseID = s.courseID.ToString();

            s.gradeMoedA = -1;
            s.gradeMoedB = -1;
            
            UsersDal dbusersDal = new UsersDal();
            Users usr = dbusersDal.LogIns.Where(y=> y.Id == studentID && y.Username == s.studentName).FirstOrDefault();
            if (usr == null)
            {
                TempData["messageAssigning"] = "The student does not exist !";
                return View("AssigningStudentToCourse");
            }
            if(usr.Permission != "3" )
            {
                TempData["messageAssigning"] = "The user you entered is not a student !";
                return View("AssigningStudentToCourse");
            }

            StudentCoursesTestsDal dbSCTD = new StudentCoursesTestsDal();
            
            StudentCoursesTests sctd = dbSCTD.StudentCoursesTestsD.Where(x => x.studentID == s.studentID && x.courseID == s.courseID).FirstOrDefault();
            if (sctd != null)
            {
                TempData["messageAssigning"] = "The student is already in the course !";
                return View("AssigningStudentToCourse");
            }

            CourseDal dbCourseDal = new CourseDal();
            Course courseToAdd = dbCourseDal.courses.Where(x => x.courseID == s.courseID && x.courseName == s.courseName).FirstOrDefault();
            if (courseToAdd == null)
            {
                TempData["messageAssigning"] = "The course does not exist !";
                return View("AssigningStudentToCourse");
            }
            
            List<StudentCoursesTests> courseOfStu = dbSCTD.StudentCoursesTestsD.Where(x => x.studentID == studentID ).ToList<StudentCoursesTests>();
            CourseDal c = new CourseDal();
            List<Course> course = new List<Course>();
            foreach (StudentCoursesTests i in courseOfStu)
            {
                course.Add(c.courses.Where(x => x.courseID.Equals(i.courseID)).First());
            }
            foreach(Course j in course)
            {
                if(j.beginningTime == courseToAdd.beginningTime && j.day == courseToAdd.day)
                {
                    TempData["messageAssigning"] = "השעות של הקורס שאני רוצה להוסיף לסטודנט חופפות עם קורס שקיים לסודנט כבר";
                    return View("AssigningStudentToCourse");
                }
            }

            dbSCTD.StudentCoursesTestsD.Add(s);
            dbSCTD.SaveChanges();
            TempData["messageAssigning"] = "The student was successfully assigned to the course";

            return View("FacultyAdministrator");

        }

        //----------------------------------------------------------------------------------------
        //כפתור עריכת לוח זמנים של קורס קורס
        
        public ActionResult FindCourse()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditCourseSchedule()
        {
            string c = Request.Form["courseID"].ToString();
            CourseDal dbCourseDal = new CourseDal();
            Course course = dbCourseDal.courses.Where(x => x.courseID == c).FirstOrDefault();
            if (course == null)
            {
                Session["message"] = "The course does not exist !";
                return View("FindCourse");
            }
            
            return View(course);
        }
        public ActionResult SaveCourse(Course c)
        {
            CourseDal cD = new CourseDal();
            Course oldcourse = cD.courses.Where(x => x.courseID == c.courseID).FirstOrDefault();
            List<Course> courses = cD.courses.Where(y=>y.courseID != c.courseID).ToList<Course>();
            if (c.dateMoedA >= c.dateMoedB)
            {
                TempData["message"] = "The Moed B exam can not take place before Moed A" ;
                return View("FindCourse", oldcourse);
            }
            
            foreach(Course co in courses)
            {
                if (co.placeCourse == c.placeCourse && co.day == c.day && co.beginningTime == c.beginningTime)
                {
                    TempData["message"] = "אי אפשר ששני שיעורים שמתקיימים באותו יום ובאותה שעה- יתקיימו גם באותה כיתה! ";
                    return View("FindCourse", oldcourse);
                }
            }
            

            //בדיקה עם המרצה שהשעות שערכתי לא מתנגשות לו עם קורסים אחרים שיש לו
            List<Course> listoflec = cD.courses.Where(x => x.lecturerID == c.lecturerID).ToList<Course>();
            foreach (Course courseoflec in listoflec)
            {
                if (courseoflec.day == c.day)
                {
                    if (courseoflec.beginningTime == c.beginningTime )
                    {
                        TempData["message"] = "היום והשעה של הקורס מתנגשים עם יום ושעה של קורס אחר שכבר קיים למרצה המלמד";
                        return View("FindCourse");
                    }
                }
            }


            cD.courses.Remove(oldcourse);
            cD.SaveChanges();
            cD.courses.Add(c);
            cD.SaveChanges();
            return View();
        }

        //----------------------------------------------------------------------------------------------------------
            



        //----------------------------------------------------------------------------------------------------------
        //כפתור עדכון ציון קורס
        public ActionResult UpdateGrade()
        {
            return View();
        }
        public ActionResult SubmitUpdateGrade(StudentCoursesTests sct)
        {

            CourseDal dbCourseDal = new CourseDal();
            Course course = dbCourseDal.courses.Where(x => x.courseID == sct.courseID).FirstOrDefault();
            if (course == null)
            {
                TempData["message"] = "The course does not exist !";
                return View("UpdateGrade");
            }

            UsersDal dbusersDal = new UsersDal();
            Users usr = dbusersDal.LogIns.Where(y => y.Id == sct.studentID ).FirstOrDefault();
            if (usr == null)
            {
                TempData["message"] = "The student does not exist !";
                return View("UpdateGrade");
            }

            StudentCoursesTestsDal SCDal = new StudentCoursesTestsDal();
            StudentCoursesTests stu = SCDal.StudentCoursesTestsD.Where(x => x.studentID == sct.studentID && x.courseID == sct.courseID).FirstOrDefault();

            if (stu == null)
            {
                TempData["message"] = "הסטודנט לא רשום לקורס";
                return View("UpdateGrade");
            }

            if(course.dateMoedA > DateTime.Now && course.dateMoedB > DateTime.Now)
            {
                TempData["message"] = "התאריכים של מועד א ומועד ב עדיין לא עברו לכן אי אפשר לעדכן את הציון";
                return View("UpdateGrade");
            }
            if (course.dateMoedA < DateTime.Now && course.dateMoedB > DateTime.Now)
            {
                return View("UpdateGradeMoedA", stu);
            }
            if (course.dateMoedA > DateTime.Now && course.dateMoedB < DateTime.Now)
            {
                return View("UpdateGradeMoedB", stu);
            }

            return View("UpdateGradeMoedA_MoedB", stu);
            
        }


        public ActionResult UpdateGradeMoedA(StudentCoursesTests stu)
        {

            return View();
        }

        public ActionResult SubmitUpdateGradeMoedA(StudentCoursesTests stu)
        {

            StudentCoursesTestsDal SCDal = new StudentCoursesTestsDal();
            StudentCoursesTests newSCT = SCDal.StudentCoursesTestsD.Where(x => x.studentID == stu.studentID && x.courseID == stu.courseID).FirstOrDefault();
            newSCT.gradeMoedA = stu.gradeMoedA;
            SCDal.SaveChanges();
            TempData["UpdateGradeMoedA"] = "Update yes";
            return View("FacultyAdministrator");
        }

        public ActionResult UpdateGradeMoedB(StudentCoursesTests stu)
        {

            return View();
        }

        public ActionResult SubmitUpdateGradeMoedB(StudentCoursesTests stu)
        {

            StudentCoursesTestsDal SCDal = new StudentCoursesTestsDal();
            StudentCoursesTests newSCT = SCDal.StudentCoursesTestsD.Where(x => x.studentID == stu.studentID && x.courseID == stu.courseID).FirstOrDefault();
            newSCT.gradeMoedB = stu.gradeMoedB;
            SCDal.SaveChanges();
            TempData["UpdateGradeMoedB"] = "ציון מבחן מועד ב' עודכן בהצלחה";
            return View("FacultyAdministrator");
        }

        public ActionResult UpdateGradeMoedA_MoedB(StudentCoursesTests stu)
        {

            return View();
        }

        public ActionResult SubmitUpdateGradeMoedA_MoedB(StudentCoursesTests stu)
        {

            StudentCoursesTestsDal SCDal = new StudentCoursesTestsDal();
            StudentCoursesTests newSCT = SCDal.StudentCoursesTestsD.Where(x => x.studentID == stu.studentID && x.courseID == stu.courseID).FirstOrDefault();
            newSCT.gradeMoedA = stu.gradeMoedA;
            newSCT.gradeMoedB = stu.gradeMoedB;
            SCDal.SaveChanges();
            TempData["UpdateGradeMoedA_MoedB"] = "הציונים של מועד א' ומועד ב' עודכנו בהצלחה";
            return View("FacultyAdministrator");
        }

    }
}