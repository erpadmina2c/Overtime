using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class List_Attendance
    {
        [Key]
        [Display(Name = "Id")]
        public int a_id { get; set; }
        [Display(Name = "Department")]
        public string? a_department { get; set; }

        [Display(Name = "FullName")]
        public string? a_full_name { get; set; }

        [Display(Name = "Username")]
        public string? a_u_name { get; set; }

        [Display(Name = "Duty Start time")]
        public string? a_duty_start { get; set; }

        [Display(Name = "First Punch")]

        [DisplayFormat(DataFormatString = @"{0:dd-MM-yyyy hh:mm tt}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public DateTime? a_first_Punch { get; set; }

        [Display(Name = "First Punch Location")]
        public string? a_f_punch_loc { get; set; }

        [Display(Name = "Duty End Time")]
        public string? a_duty_ended_at { get; set; }

        [Display(Name = "Second Punch")]

        [DisplayFormat(DataFormatString = @"{0:dd-MM-yyyy hh:mm tt}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        public DateTime? a_second_punch { get; set; }

        [Display(Name = "Second Punch Location")]
        public string? a_s_punch_loc { get; set; }

        [Display(Name = "Worktime(Hour)")]

        [Column(TypeName = "decimal(10,2)")]
        public decimal? a_duration { get; set; }

        [Display(Name = "Status")]
        public string? a_status { get; set; }

        [Display(Name = "Remark")]
        public string? a_remark { get; set; }

        [Display(Name = "AttendanceDate")]
        [DataType(DataType.Date)]
        public DateTime a_attendance_date { get; set; }

    }
}
