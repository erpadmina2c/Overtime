using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface ITraining  
    {
        IEnumerable<Training> GetTrainings { get; }
        Training GetTraining(int id);

        DbResult Add(Training training);

        void Remove(int id);

        void Update(Training training);
        IEnumerable<List_Training> TrainingData(DateTime from, DateTime to, int id);
    }
}
