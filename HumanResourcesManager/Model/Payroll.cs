using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourcesManager.Model
{
    [Table("payroll")]
    public class Payroll
    {
        public int id { get; set; }
        public DateTime pay_date { get; set; }
        public decimal gross_salary { get; set; }
        public decimal deductions { get; set; }
        public decimal net_salary { get; set; }

        //[ForeignKey("employee_id")]
        public virtual Employee employee { get; set; }
    }
}
