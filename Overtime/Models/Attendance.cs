using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class Attendance
    {
        [Key]
        [Display(Name = "Id")]
        public int id { get; set; }
        [Display(Name = "Department")]
        public string? u_department { get; set; }

        [Display(Name = "FullName")]
        public string? u_full_name { get; set; }

        [Display(Name = "Username")]
        public string? u_name { get; set; }

        [Display(Name = "Email")]
        public string? u_email { get; set; }

        [Display(Name = "Status")]
        public string? u_status { get; set; }

        [Display(Name = "Remark")]
        public string? u_remark { get; set; }

        [Display(Name = "AttendanceDate")]
        [DataType(DataType.Date)]
        public DateTime u_attendance_date { get; set; }
    }
}
