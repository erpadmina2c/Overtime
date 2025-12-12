using Microsoft.AspNetCore.Http;
using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public  interface IExcel
    {
        Task<List<Salary>> ReadSalaryExcelAsync(IFormFile file);
    }
}
