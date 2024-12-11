using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesManager.Model;

namespace HumanResourcesManager
{
    // DbContext para EF Core
    public class HumanResourcesManagerContext : DbContext
    {
        public static string dbConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HumanResourcesManager";
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(dbConnection);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
