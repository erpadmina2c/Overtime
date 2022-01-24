using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Overtime.Services;
using System.Data;

namespace Overtime.Controllers
{
    public class FoodController : Controller
    {
        private readonly IMenu imenu;
        private readonly IFoodSchedule ifoodschedule;
        public FoodController(IFoodSchedule _ifoodschedule, IMenu _imenu)
        {
            imenu = _imenu;
            ifoodschedule = _ifoodschedule;
        }
        public IActionResult Index()
        {
            User user=getCurrentUser();
            if (user== null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                Result result = new Result();
                FoodSchedule foodSchedule = new FoodSchedule();
                foodSchedule.F_userid = user.u_id;
                foodSchedule.f_date = DateTime.Now;
                result = CheckUserFoodDetails(foodSchedule);
                if (result.Message == "Exist")
                {
                    return RedirectToAction("Index", "Home");
                }

                return View();
            }
           
        }
        [HttpPost]
        public Result AddFoodDetails(FoodSchedule foodSchedule)
        {
            User user = getCurrentUser();
            Result result = new Result();
            if (user == null)
            {
                result.Message = "session timeout !!!";
               
            }
            else
            {
                foodSchedule.F_userid = user.u_id;
                foodSchedule.f_date = DateTime.Now;
                result = CheckUserFoodDetails(foodSchedule);
                if (result.Message != "Exist")
                {
                    foodSchedule.F_FeedbackDate = DateTime.Now.AddDays(-1);
                    result = ifoodschedule.AddFoodSchedules(foodSchedule);
                }
               
            }

            return result;
        }
        public Result CheckUserFoodDetails(FoodSchedule foodSchedule) 
        {
            User user = getCurrentUser();
            Result result = new Result();
            if (user == null)
            {
                result.Message = "session timeout !!!";
            }
            else
            {
                result = ifoodschedule.GetUserFoodDetails(foodSchedule);
            }
            return result;
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
        public IActionResult FeedBackReport()
        {
            User user = getCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }

        }
        public  IActionResult GetFoodFeedBackReportByDate(string date)
        {
            DataTable dataTable = new DataTable();
            User user = getCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                dataTable= ifoodschedule.GetFoodFeedBackReportByDate(date, user.u_id);
            }
            return View(dataTable);
        }
    }
}
