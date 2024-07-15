using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class Attachment
    {
        [Key]
        public int at_id { get; set; }
        public int at_fun_doc_id { get; set; }
        public int? at_doc_id { get; set; }

        [Display(Name = "Path")]
        public string? at_path { get; set; }

        [Display(Name = "Cre by")]
        public int at_cre_by { get; set; }
        [DataType(DataType.Date)]

        [NotMapped]
        [Display(Name = "Cre by Name")]
        public string? at_cre_by_name { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Cre Date")]
        public DateTime at_cre_date { get; set; }
    }
}
