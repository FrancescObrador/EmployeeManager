using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourcesManager.Model
{
    [Table("employee_project")]
    public class EmployeeProject
    {

        public string role { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }

        [Key]
        public int employee_id { get; set; }  // Clave foránea explícita
        public int project_id { get; set; }   // Clave foránea explícita

        [ForeignKey("employee_id")]
        public virtual Employee employee { get; set; }
        [ForeignKey("project_id")]
        public virtual Project project { get; set; }
    }
}
