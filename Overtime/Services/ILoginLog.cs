using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface ILoginLog
    {
        LoginLog GetLoginLog(int id);

        IEnumerable<LoginLog> GetLoginLogs { get; }

        void Add(LoginLog loginLog);

        void Remove(int id);

        void Update(LoginLog loginlog);
        public IEnumerable<LoginLog> GetLoginLogsBySearch(int id, DateTime start_time, DateTime end_time);
    }
}
