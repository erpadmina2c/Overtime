using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime.Models
{
    public class DbResultWithObject
    {
        [Key]
        public int id { get; set; }
        public String? Message { get; set; }
        public int? Obj { get; set; }
    }
}
