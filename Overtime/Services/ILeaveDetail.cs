using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface ILeaveDetail
    {
        IEnumerable<LeaveDetail> GetLeavetypes { get; }

        public LeaveDetail GetLeavetype(int id);

        string AddUpdateLeave(LeaveDetail leavetype,string flag,int UserId);

        string DeleteLeave(int LeaveId);
        void Update(LeaveDetail leavetype);
      
    }
}
