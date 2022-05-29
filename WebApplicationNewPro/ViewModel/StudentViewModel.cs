using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationNewPro.Models;

namespace WebApplicationNewPro.ViewModel
{
    public class StudentViewModel
    {
        public StudentCoursesTests student { get; set; }
        public List<StudentCoursesTests> students { get; set;}
    }
}