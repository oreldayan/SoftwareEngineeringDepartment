using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplicationNewPro.Models;

namespace WebApplicationNewPro.Dal
{
    public class StudentCoursesTestsDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StudentCoursesTests>().ToTable("tblStudentCoursesTests");
        }
        public DbSet<StudentCoursesTests> StudentCoursesTestsD { get; set; }
    }
}
