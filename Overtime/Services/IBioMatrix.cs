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
        public IEnumerable<Attendance> GetAttendance();
        void Add(MachineDetail machine);
        MachineDetail GetMachine(int machine_Id);
        DataTable GetMonthReport();
    }
}
