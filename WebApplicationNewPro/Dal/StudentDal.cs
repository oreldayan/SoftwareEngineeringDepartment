using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplicationNewPro.Models;

namespace WebApplicationNewPro.Dal
{
    public class StudentDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>().ToTable("tblStudent");
        }
        public DbSet<Student> Students { get; set; }
    }
}