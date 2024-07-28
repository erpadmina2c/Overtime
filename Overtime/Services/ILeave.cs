using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface ILeave
    {
        List<Leave> getLeaveRequests(int u_id);
        DbResultWithObject createOrUpdateLeave(Leave leave);
        List<Leave> getLeaveApplicationsForReview(int u_id);
        Leave getLeave(int l_id);
        DbResult AuthorizeLeave(int l_id, string l_authorization,int u_id);
        List<Leave> getLeaveApplicationsForHrApproval(int u_id);
        DbResult ApproveLeave(int id, string type,string ticket, int u_id);
        DbResult deleteLeaveApplication(int l_id, int u_id);
        DataTable getLeaveReport(int u_id, DateTime from, DateTime to, string type, string fullhistory);
        List<Leave> getLeaveDetailsOfAEmployee(int u_id);
        RemainingLeave getRemainingLeave(int u_id);
        DbResult SendToAnotherSupervisor(int l_id, int supervisor,int u_id);
    }
}
