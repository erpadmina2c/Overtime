using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface IArchivedLeave
    {
        List<ArchivedLeave> getArchivedLeaves();
        DbResult createArchivedLeave(int u_id, int leave_days, int cre_by);
        DbResult updateArchivedLeave(int al_id, int leave_days, int u_id);
        ArchivedLeave getEmployeeArchivedLeave(int u_id);
    }
}
