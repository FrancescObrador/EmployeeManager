using HumanResourcesManager.View;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourcesManager.Model
{
    [Table("department")]
    public class Department
    {
        public int id {  get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public virtual List<Employee> Employees { get; set; }
    }
}
