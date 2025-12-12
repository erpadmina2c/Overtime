using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Overtime.Models;
using Overtime.Services;

namespace Overtime.Controllers
{
    public class SalaryController : Controller
    {
        private readonly ISalary isalary;
        private readonly IMenu imenu;
        private readonly IExcel iexcel;
        private readonly IUser iuser;

        public SalaryController(ISalary _isalary, IMenu _imenu,IExcel _iexcel, IUser _iuser)
        {
            isalary = _isalary;
            imenu = _imenu;
            iexcel = _iexcel;
            iuser = _iuser;
        }

        // GET: Salary
        public ActionResult Index()
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.AccessForSalaryUpload = imenu.getMenulistByRoleAndTypeAndLink(getCurrentUser().u_role_id, "Options", "AccessForSalaryUpload");
                ViewBag.users = iuser.GetUsersList();
                return View();
            }
        }


        public ActionResult getSalaryOftheMonth(string yearMonth)
        {
            DataTable dataTable = new DataTable();
            User curruser = getCurrentUser();
            if (curruser == null)
            {
                ViewBag.message = "Session Expired !!";
            }
            else
            {
                ViewBag.AccessForSalaryUpload = imenu.getMenulistByRoleAndTypeAndLink(getCurrentUser().u_role_id, "Options", "AccessForSalaryUpload");
                dataTable = isalary.getSalaryOftheMonth(yearMonth, curruser.u_id);
            }
            return View(dataTable);
         
        }

        [HttpPost]
        public async Task<IActionResult> UploadSalary(string yearMonth, IFormFile file)
        {
            DataTable dataTable = new DataTable();
            try
            {
                List<Salary> salaries = await iexcel.ReadSalaryExcelAsync(file);
                string jsonData = JsonConvert.SerializeObject(salaries);
                dataTable = isalary.uploadSalary(yearMonth, jsonData);
              
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message+ex.InnerException);
            }
            return View(dataTable);
        }


        [HttpPost]
        public DbResult deleteSalary(int id)
        {
            DbResult dbResult = new DbResult();
            try
            {
                User curruser = getCurrentUser();
                if (curruser != null)
                {
                    dbResult = isalary.deleteSalary(id, curruser.u_id);
                }else
                {
                    dbResult.Message = "Session Expired !!";
                }
            }
            catch (Exception ex)
            {
                dbResult.Message=ex.Message;
            }
            return dbResult;
        }


        public DbResult deleteSalaryOftheMonth(string yearMonth)
        {
            DbResult dbResult = new DbResult();
            try
            {
                User curruser = getCurrentUser();
                if (curruser != null)
                {
                    dbResult = isalary.deleteSalaryOftheMonth(yearMonth, curruser.u_id);
                }
                else
                {
                    dbResult.Message = "Session Expired !!";
                }
            }
            catch (Exception ex)
            {
                dbResult.Message = ex.Message;
            }
            return dbResult;
        }


        public ActionResult getPaySlip(int id)
        {
            Salary salary = new Salary();
            salary = isalary.GetSalary(id);
            return View(salary);
        }

            // GET: Salary/Details/5
        public ActionResult Details(int id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View(isalary.GetSalary(id));
            }
        }

        // GET: Salary/Create
        public ActionResult Create()
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }
        }

        // POST: Salary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Salary salary)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    salary.s_cre_by = getCurrentUser().u_id;
                    salary.s_cre_date = DateTime.Now;
                    isalary.Add(salary);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
        }

        // GET: Salary/Edit/5
        public ActionResult Edit(int id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View(isalary.GetSalary(id));
            }
        }

        // POST: Salary/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Salary salary)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    Salary existingSalary = isalary.GetSalary(id);
                    if (existingSalary != null)
                    {
                        existingSalary.s_year_month = salary.s_year_month;
                        existingSalary.s_emp_code = salary.s_emp_code;
                        existingSalary.s_emp_name = salary.s_emp_name;
                        existingSalary.s_category = salary.s_category;
                        existingSalary.s_department = salary.s_department;
                        existingSalary.s_basic_salary = salary.s_basic_salary;
                        existingSalary.s_total_salary = salary.s_total_salary;
                        existingSalary.s_overtime = salary.s_overtime;
                        existingSalary.s_allowance = salary.s_allowance;
                        existingSalary.s_deduction = salary.s_deduction;
                        existingSalary.s_advance = salary.s_advance;
                        existingSalary.s_payable = salary.s_payable;
                        existingSalary.s_Message = salary.s_Message;

                        isalary.Update(existingSalary);
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
        }

        // GET: Salary/Delete/5
        public ActionResult Delete(int id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View(isalary.GetSalary(id));
            }
        }

        // POST: Salary/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Salary salary)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    isalary.Remove(id);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
        }

        private User getCurrentUser()
        {
            try
            {
                if (HttpContext.Session.GetString("User") == null)
                {
                    return null;
                }
                else
                {
                    User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
                    ViewBag.Name = user.u_full_name;
                    ViewBag.isAdmin = user.u_is_admin;

                    List<MenuItems> menulist = new List<MenuItems>();

                    IEnumerable<Menu> menus = imenu.getMenulistByRoleAndType(user.u_role_id, "Menu");

                    foreach (var menu in menus)
                    {
                        MenuItems menuItems = new MenuItems
                        {
                            m_id = menu.m_id,
                            m_description = menu.m_description,
                            m_desc_to_show = menu.m_desc_to_show,
                            m_link = menu.m_link,
                            m_parrent_id = menu.m_parrent_id,
                            m_type = menu.m_type,
                            m_cre_by = menu.m_cre_by,
                            m_active_yn = menu.m_active_yn,
                            m_cre_date = menu.m_cre_date,
                            menuItem = imenu.getMenulistByRoleAndTypeAndParrent(user.u_role_id, "MenuItem", menu.m_id)
                        };
                        menulist.Add(menuItems);
                    }

                    ViewBag.MenuList = menulist;
                    ViewBag.isMonitor = user.u_role_description.Equals("Monitor") ? "Y" : "N";
                    return user;
                }

            }
            catch
            {
                return null;
            }
        }
    }
}
