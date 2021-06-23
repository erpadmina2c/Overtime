using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class TrainingRepository : ITraining
    {
        private readonly DBContext db;

        public TrainingRepository(DBContext _db)
        {
            db = _db;
        }

        public IEnumerable<Training> GetTrainings => db.Training;

        public DbResult Add(Training training)
        {
            var user = new SqlParameter("id", training.tr_u_id + "");
            var from_date = new SqlParameter("from", training.tr_start_date + "");
            var result = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.createTraining @id,@from", user, from_date).ToList().FirstOrDefault();
            return result;
        }

        public Training GetTraining(int id)
        {
            Training training = db.Training.Find(id);
            return training;
        }

        public void Remove(int id)
        {
            Training training = db.Training.Find(id);
            db.Training.Remove(training);
            db.SaveChanges();
        }

        public IEnumerable<List_Training> TrainingData(DateTime from, DateTime to, int id)
        {
            var user = new SqlParameter("id",id + "");
            var from_date = new SqlParameter("from", from + "");
            var to_date = new SqlParameter("to", to + "");
            var pellets = db.List_Training.FromSqlRaw<List_Training>("EXECUTE dbo.TrainingData @id,@from,@to", user, from_date, to_date).ToList();
            return pellets;
        }

        public void Update(Training training)
        {
            db.Training.Update(training);
            db.SaveChanges();
        }
    }
}
