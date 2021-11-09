using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class LeaveDetail
    {
        [Key]
        public int? LeaveId { get; set; }
        public string? EmpName { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Leave Days")]
        public int LeaveDays { get; set; }
        public string? MarkedBy { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? LastModfiedDate { get; set; }
        public string? MarkedDate { get; set; }
        public string? LeaveType { get; set; }
        public string? CurrentStatus { get; set; }
    }
}
