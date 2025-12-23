using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class MachineDetail
    {
        [Key]
        [Display(Name = "MachineId")]
        public int machine_Id { get; set; }

        [Display(Name = "Machine Name")]
        public string? machine_name { get; set; }

        [Display(Name = "Port Number")]
        public int? port_number { get; set; }

        [Display(Name = "IP Address")]
        public string? ip_address { get; set; }

        [Display(Name = "Machine Type")]
        public string? machine_type { get; set; }

        [Display(Name = "Campus")]
        public int? campus_id { get; set; }

        [NotMapped]
        [Display(Name = "Campus")]
        public string? campus_name { get; set; }

        [Display(Name = "CreatedDate")]
        [DataType(DataType.Date)]
        public DateTime? created_at { get; set; }
    }
}
