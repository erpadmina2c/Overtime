using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface IUserReportingHierarchy
    {
        public IEnumerable<UserReportingHierarchy> GetUserReportingHierarchys { get; }
        public UserReportingHierarchy GetUserReportingHierarchy(int id);

        void Add(UserReportingHierarchy UserReportingHierarchy);

        void Remove(int id);

        public IEnumerable<List_UserReportingHierarchy> GetUserReportingHierarchysByuser(UserReportingHierarchy userReportingHierarchy);
        DbResult addUserReportingHierarchy(UserReportingHierarchy userReportingHierarchy);
    }
}
