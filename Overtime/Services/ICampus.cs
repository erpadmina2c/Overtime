using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{

        public interface ICampus
        {
            List<Campus> getCampuses();
            Campus getCampus(int c_id);
            DbResult addOrUpdateCampus(Campus campus);
            DbResult deleteCampus(int c_id, int u_id);
        }
  
}
