using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface IUserShift
    {
        IEnumerable<UserShift> GetUserShifts { get; }
        UserShift GetUserShift(int id);

        DbResult Add(UserShift userShift);

        void Remove(int id);

        void Update(UserShift userShift);
        IEnumerable<List_UserShift> UserShiftData(DateTime from, DateTime to, int id);
    }
}
