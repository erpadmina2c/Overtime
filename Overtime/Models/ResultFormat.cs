using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class ResultData
    {
      
        public Boolean hasError { get; set; }
        public String Message { get; set; }

        public Object successData { get; set; }
    }
}
