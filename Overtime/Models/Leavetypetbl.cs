using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class Leavetypetbl
    {
        [Key]
        [Display(Name = "LeaveTypeId")]
        public int LeavetypeId { get; set; }

        [Display(Name = "Leave Type")]
        public string? Leavetype { get; set; }

        [Display(Name = "Fly Ticket")]
        public string? FlyTicket { get; set; }

        [Display(Name = "Attachment Required")]
        public string? AttachmentRequired { get; set; }

    }
}
