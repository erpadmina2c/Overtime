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
        IEnumerable<List_Attendance> AttendanceDetailsBySearch(DateTime fromdate, DateTime todate, int u_id,int curr_user_id);
        DataTable getMyTodaysPunchInfos(int u_id);
        DbResult addManualPunching(int machine, int u_id);
        DataTable getAttendanceReport(DateTime fromdate, DateTime todate, int user, int searchby);
        List<MachineDetail> getMachineDetails();
    }
}
