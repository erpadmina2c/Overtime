using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface IHourRate
    {
        IEnumerable<HourRate> GetHourRates { get; }
        public HourRate GetHourRate(int id);

        void Add(HourRate hourRate);

        void Remove(int id);
        void Update(HourRate HourRate);
    }
}
