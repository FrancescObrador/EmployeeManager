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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar clave primaria compuesta para EmployeeProject
            modelBuilder.Entity<EmployeeProject>()
                .HasKey(ep => new { ep.employee_id, ep.project_id });

            // Configurar relación Employee -> EmployeeProject con eliminación en cascada
            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.employee)
                .WithMany(e => e.EmployeeProjects)
                .HasForeignKey(ep => ep.employee_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar relación Project -> EmployeeProject con eliminación en cascada
            modelBuilder.Entity<EmployeeProject>()
                .HasOne(ep => ep.project)
                .WithMany(p => p.EmployeeProjects)
                .HasForeignKey(ep => ep.project_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Configurar auto-inclusión de EmployeeProjects en Project
            modelBuilder.Entity<Project>()
                .Navigation(p => p.EmployeeProjects)
                .AutoInclude();
        }

    }
}
