using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface IAccomadation
    {
        IEnumerable<Accomadation> GetAccomadationslist { get; }
        Accomadation GetAccomadation(int id);

        void Add(Accomadation accomadation);


        void Remove(int id);
        void Update(Accomadation accomadation);
        IEnumerable<Accomadation> GetAccomadations(string name);
        DbResult AddOrUpdateAccomodation(Accomadation accomadation);
        DbResult DeleteAccomodation(int id, int u_id);
    }
}
