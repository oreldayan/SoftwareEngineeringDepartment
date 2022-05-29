using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationNewPro.Models
{
    public class StudentCoursesTests
    {
        [Key]
        [Required]
        public string studentID { get; set; }
        [Required]
        public string courseID { get; set; }
        [Required]
        public string courseName { get; set; }
        public string studentName { get; set; }
        public int gradeMoedA { get; set; }
        public int gradeMoedB { get; set; }
    }
}