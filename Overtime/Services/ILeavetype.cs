using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface ILeavetype
    {
        IEnumerable<Leavetypetbl> GetLeavetypes { get; }

        public Leavetypetbl GetLeavetype(int id);

        void Add(Leavetypetbl leavetype);

        void Remove(int id);
        void Update(Leavetypetbl leavetype);

      
    }
}
