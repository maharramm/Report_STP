using Microsoft.EntityFrameworkCore;
using ReportSTP.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportSTP.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LoginForUser>().HasNoKey();
            builder.Entity<LoginForUser>().ToTable("LoginUser");
            builder.Entity<LoginForUser>().Property(u => u.UserName).HasColumnName("Username");
            builder.Entity<LoginForUser>().Property(u => u.Password).HasColumnName("Password");

            base.OnModelCreating(builder);
        }

        public DbSet<LoginForUser> Users { get; set; }

    }
}
