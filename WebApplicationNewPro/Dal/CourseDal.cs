using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplicationNewPro.Models;

namespace WebApplicationNewPro.Dal
{
    public class CourseDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CourseDal>(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().ToTable("tblCourse");
        }
        public DbSet<Course> courses { get; set; }
    }
}