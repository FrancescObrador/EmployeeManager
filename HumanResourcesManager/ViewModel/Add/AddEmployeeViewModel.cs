using HumanResourcesManager.Model;
using HumanResourcesManager.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourcesManager.ViewModel.Add
{
    public class AddEmployeeViewModel
    {
        private HumanResourcesManagerContext _context;
        private Logger logger;

        public AddEmployeeViewModel()
        {
            logger = new Logger("AddEmployeeViewModel");

            _context = new HumanResourcesManagerContext();
            _context.Set<Employee>().Load();

            logger.LogInfo("AddEmployeeViewModel created");
        }

        public void Create(Employee newItem)
        {
            try
            {
                _context.Set<Employee>().Add(newItem);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }

        public List<Project> GetProjects()
        {
            return _context.Set<Project>().ToList();
        }

        public List<Department> GetDepartments()
        {
            return _context.Set<Department>().ToList();
        }

        public Department GetDepartment(int id)
        {
            return _context.Departments.FirstOrDefault(d => d.id == id);
        }
    }
}
