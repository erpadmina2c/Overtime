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

        DataTable getInAndOutLogBySearch(int campus,DateTime from, DateTime to);
        DataTable getInAndOutReport(int campus,int u_id,int ac_id, DateTime from, DateTime to);
        DbResult AddInAndOut(InAndOut inAndOut);
        DbResult updateInAndOutPunchType(InAndOut inAndOut);
        DataTable getCampusWiseInAndOut(int campus,int ac,string status,int u_id);
        DbResult updateInAndOutPunchTypeUserWise(InAndOut inAndOut);
    }
}
