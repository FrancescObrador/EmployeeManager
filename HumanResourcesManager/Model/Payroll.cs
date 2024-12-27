using ExportPDF;
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
    public class Payroll : PayrollBase
    {
        // Atributos heredados de PayrollBase

        //[ForeignKey("employee_id")]
        public virtual Employee employee { get; set; }
    }
}
