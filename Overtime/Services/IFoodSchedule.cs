using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
   public interface IFoodSchedule
    {
        Result AddFoodSchedules(FoodSchedule foodSchedule);
        Result GetUserFoodDetails(FoodSchedule foodSchedule);
        DataTable GetFoodFeedBackReportByDate(DateTime startDate,DateTime endDate, int u_id);
    }
}
