using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Context
{
    public class MVCApp01DbContext:IdentityDbContext<ApplicationUser>
    {
        public MVCApp01DbContext(DbContextOptions<MVCApp01DbContext>options):base(options)
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=Omnia;Database=MNCApp01;Trusted_Connection=true");
        //}
        public DbSet<Department> Departments{ get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
