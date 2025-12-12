using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface ISalary
    {
        List<Salary> GetSalaries();
        Salary GetSalary(int id);
        void Add(Salary salary);
        void Remove(int id);
        void Update(Salary salary);
        DataTable getSalaryOftheMonth(string yearMonth,int u_id);
        DataTable uploadSalary(string yearMonth, string jsonData);
        DbResult deleteSalary(int id,int u_id);
        DbResult deleteSalaryOftheMonth(string yearMonth, int u_id);
    }
}

