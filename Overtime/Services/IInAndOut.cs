using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface IInAndOut
    {
        IEnumerable<InAndOut> GetInAndOuts { get; }

        DataTable getInAndOutLogBySearch(DateTime from, DateTime to);
    }
}
