using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime.Models
{
    public class DbResult
    {
        [Key]
        public int id { get; set; }
        public string Message { get; set; }
    }
}
