using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface IUserBioDepartment
    {
        IEnumerable<UserBioDepartment> GetUserBioDepartments();
    
        UserBioDepartment GetUserBioDepartment(int id);

        void Add(UserBioDepartment userBioDepartment);

        void Remove(int id);

        void Update(UserBioDepartment userBioDepartment);
        bool getIsExistOrNot(int ud_id, int ud_depart_id);
    }
}
