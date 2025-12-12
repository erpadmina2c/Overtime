using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class ExcelRepository : IExcel
    {
        private readonly DBContext db;

        public ExcelRepository(DBContext _db)
        {
            db = _db;
        }

        public async Task<List<Salary>> ReadSalaryExcelAsync(IFormFile file)
        {
            List<Salary> list = new List<Salary>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var workbook = new XLWorkbook(stream))
                {
                    var ws = workbook.Worksheets.First();
                    var rows = ws.RowsUsed().Skip(1); // skip header row

                    foreach (var row in rows)
                    {
                        // Skip empty row
                        if (row.Cells().All(c => string.IsNullOrWhiteSpace(c.GetString())))
                            continue;

                        var s = new Salary
                        {
                            s_emp_code = GetCellString(row, 1),
                            s_emp_name = GetCellString(row, 2),
                            s_category = GetCellString(row, 3),
                            s_department = GetCellString(row, 4),
                            s_basic_salary = GetCellInt(row, 5),
                            s_daily_ot = GetCellInt(row, 6),
                            s_total_salary = GetCellInt(row, 7),
                            s_overtime = GetCellInt(row, 8),
                            s_special_bonus = GetCellInt(row, 9),
                            s_allowance = GetCellInt(row, 10),
                            s_aditional_bonus = GetCellInt(row, 11),
                            s_other_payment = GetCellInt(row, 12),
                            s_traveling_allowance = GetCellInt(row, 13),
                            s_performance_bonus = GetCellInt(row, 14),
                            s_deduction = GetCellInt(row, 15),
                            s_recovery = GetCellInt(row, 16),
                            s_advance = GetCellInt(row, 17),
                            s_payable = GetCellInt(row, 18),
                        };

                        list.Add(s);
                    }
                }
            }

            return list;
        }

        private string GetCellString(IXLRow row, int col)
        {
            return row.Cell(col).IsEmpty() ? "" : row.Cell(col).GetString().Trim();
        }

        private int? GetCellInt(IXLRow row, int col)
        {
            var cellValue = row.Cell(col).GetString().Trim();

            if (string.IsNullOrWhiteSpace(cellValue))
                return null;

            // Remove commas
            cellValue = cellValue.Replace(",", "");

            // Try integer parse
            if (int.TryParse(cellValue, out int result))
                return result;

            // Try decimal parse (round or convert to int)
            if (decimal.TryParse(cellValue, out decimal dec))
                return (int)dec; // or Math.Round(dec)

            return null;
        }

    }
}
