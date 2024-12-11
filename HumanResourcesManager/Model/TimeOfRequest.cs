using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourcesManager.Model
{
    [Table("time_off_request")]
    public class TimeOfRequest
    {
        public int id {  get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime request_date { get; set; }
        public RequestType type { get; set; }

        //[ForeignKey("employee_id")]
        public virtual Employee employee { get; set; }
    }

    public enum RequestType { vacation, sick_leave }
}
