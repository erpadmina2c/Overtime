using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class HourRateRepository : IHourRate
    {
        private DBContext db;

        public HourRateRepository(DBContext _db)
        {
            db = _db;
        }
        public IEnumerable<HourRate> GetHourRates => db.HourRates;

        public void Add(HourRate hourRate)
        {
            db.HourRates.Add(hourRate);
            db.SaveChanges();
        }

        public HourRate GetHourRate(int id)
        {
            HourRate hourRate = db.HourRates.Find(id);
            return hourRate;
        }

        public void Remove(int id)
        {
            HourRate hourRate = db.HourRates.Find(id);
            db.HourRates.Remove(hourRate);
            db.SaveChanges();
        }

        public void Update(HourRate HourRate)
        {
            db.HourRates.Update(HourRate);
            db.SaveChanges();
        }
    }
}
