using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManager.Model;

namespace EmployeeManager
{
    // DbContext para EF Core
    public class EmployeeManagerContext : DbContext
    {
        public static string dbConnection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EmployeeManager";
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(dbConnection);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
