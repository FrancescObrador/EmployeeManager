using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace HumanResourcesManager.Model
{
    [Table("employee")]
    public class Employee
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public DateTime hire_date { get; set; }
        public decimal salary { get; set; }
        public string position { get; set; }
        public DateTime date_of_birth { get; set; }
        public byte[]? picture { get; set; }

        [ForeignKey("department_id")]
        public virtual Department department { get; set; }

        [ForeignKey("employee_id")]
        public virtual List<Payroll> Payrolls { get; set; }

        public virtual List<EmployeeProject> EmployeeProjects { get; set; }

        public Employee()
        {

        }

        public string FullName => $"{first_name} {last_name}";
    }
}
