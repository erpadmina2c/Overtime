using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
     public  interface IWorkingHour
    {
        WorkingHour GetWorkingHour(int id);

        IEnumerable<WorkingHour> GetWorkingHours { get; }

        void Add(WorkingHour workingHour);

        void Remove(int id);

        void Update(WorkingHour workingHour);

        IEnumerable<WorkingHour> GetWorkingHourByDocument(int rowid, int doc_id);
        decimal GetWorkingHourConsolidateByDocument(int v1, int v2);
    }
}
