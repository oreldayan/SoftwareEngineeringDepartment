using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplicationNewPro.Models;

namespace WebApplicationNewPro.Dal
{
    public class UsersDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>().ToTable("tblUsers");
        }
        public DbSet<Users> LogIns { get; set; }
    }
}