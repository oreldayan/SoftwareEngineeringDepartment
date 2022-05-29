using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WebApplicationNewPro.Models
{
    public class Users
    {
        [Required]
        [RegularExpression("^[a-z]*$", ErrorMessage = "First Name must countain only letters")]
        public string Username { get; set; }
        [Required]
        [RegularExpression("^[0-9]{3}$", ErrorMessage = "The password must contain 3 digits")]
        public string Password { get; set; }
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Permission { get; set; }
    }
}