using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface IBioMatrix
    {
        public IEnumerable<Attendance> GetAttendance(string date);
        public IEnumerable<List_Attendance> GetAttendanceByDate(string date, int u_id);
        void Add(MachineDetail machine);
        MachineDetail GetMachine(int machine_Id);
        DataTable GetMonthReport(DateTime rq_start_time, DateTime rq_end_time);

        public IEnumerable<LeaveDetail> GetLeaveDetail();
    }
}
