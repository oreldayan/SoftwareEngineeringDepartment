using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationNewPro.Models
{
    public class Course
    {
        [Required]
        [RegularExpression("^[a-z]*$", ErrorMessage = "Course Name must countain only letters")]
        public string courseName { get; set; }
        [Key]
        [Required]
        [RegularExpression("^[0-9]{2}$", ErrorMessage = "The password must contain 3 digits")]
        public string courseID { get; set; }
        [Required]
        
        public string placeCourse { get; set; }
        [Required]
        public string day { get; set; }
        [Required]
        public int beginningTime { get; set; }
        [Required]
        
        public DateTime dateMoedA { get; set; }
        [Required]
        
        public DateTime dateMoedB { get; set; }
        [Required]
        public string placeMoedA { get; set; }
        [Required]
        public string placeMoedB { get; set; }

        [Required]
        public string lecturerID { get; set; }

    }
}