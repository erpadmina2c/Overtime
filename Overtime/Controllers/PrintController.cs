using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Overtime.Models;
using Overtime.Services;

namespace Overtime.Controllers
{
    public class PrintController : Controller
    {
        private readonly IMenu imenu;
        private readonly ILeave ileave;
        private readonly IWebHostEnvironment iwebHostEnvironment;
        public PrintController(IMenu _imenu, IWebHostEnvironment _iwebHostEnvironment,ILeave _ileave)
        {
            imenu = _imenu;
            iwebHostEnvironment = _iwebHostEnvironment;
            ileave = _ileave;
        }
        public ActionResult PrintSalaryRequestForm(int id)
        {
            string mimtype = "";
            int extension = 1;
            var path = $"{iwebHostEnvironment.WebRootPath}\\Reports\\LeaveSalaryRequest1.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            Leave leave = ileave.getLeave(id);
            LocalReport localReport = new LocalReport(path);
            //localReport.AddDataSource("DataSet1", leave);
          //  parameters.Add("InvNo", id.ToString());
            parameters.Add("EmployeeName", leave.l_leave_for_name.ToString());
            parameters.Add("EmployeeCode", leave.l_leave_for_name.ToString());
            parameters.Add("Department", leave.l_dep_name.ToString());
            parameters.Add("SalaryOfMonth", Convert.ToDateTime(leave.l_salary_month.ToString()).ToString("MMM yyyy"));
            parameters.Add("Designation", leave.l_designation_name.ToString());
            parameters.Add("AmountRequired", leave.l_required_amount.ToString());
            parameters.Add("RequiredByDate", Convert.ToDateTime(leave.l_required_date).ToString("dd-MM-yyyy"));
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);

            return File(result.MainStream, "application/pdf");
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
                        MenuItems menuItems = new MenuItems();
                        menuItems.m_id = menu.m_id;
                        menuItems.m_description = menu.m_description;
                        menuItems.m_desc_to_show = menu.m_desc_to_show;
                        menuItems.m_link = menu.m_link;
                        menuItems.m_parrent_id = menu.m_parrent_id;
                        menuItems.m_type = menu.m_type;
                        menuItems.m_cre_by = menu.m_cre_by;
                        menuItems.m_active_yn = menu.m_active_yn;
                        menuItems.m_cre_date = menu.m_cre_date;
                        menuItems.menuItem = imenu.getMenulistByRoleAndTypeAndParrent(user.u_role_id, "MenuItem", menu.m_id);
                        menulist.Add(menuItems);
                    }

                    ViewBag.MenuList = menulist;
                  
                    if (user.u_role_description.Equals("Monitor")) ViewBag.isMonitor = "Y";
                    else
                    {
                        ViewBag.isMonitor = "N";
                    }
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