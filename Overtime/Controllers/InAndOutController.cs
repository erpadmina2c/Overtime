using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Overtime.Models;
using Overtime.Services;

namespace Overtime.Controllers
{
    public class InAndOutController : Controller
    {
        private readonly IInAndOut iinAndOut;
        private readonly IMenu imenu;
        private readonly IUser iuser;
        private readonly IAccomadation iaccomadation;
        private readonly ICampus icampus;
        public InAndOutController(IInAndOut _iinAndOut, IMenu _imenu,IUser _iuser, IAccomadation _iaccomadation, ICampus _icampus)
        {
            iinAndOut = _iinAndOut;
            imenu = _imenu;
            iuser = _iuser;
            iaccomadation = _iaccomadation;
            icampus = _icampus;
        }
        // GET: Role
        public ActionResult Index()
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
        public ActionResult InAndOutLog()
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.CampusList = icampus.getCampuses();
                return View();
            }
          
        }
        public ActionResult getInAndOutLogBySearch(int campus ,string reportrange)
        {
            DataTable dataTable = new DataTable();
            if (getCurrentUser() == null)
            {
                ViewBag.message = "Session Expired !!";
            }
            else
            {

                if (!reportrange.Equals("undefined"))
                {
                    String[] array = reportrange.Split('-');

                    DateTime from = DateTime.Parse(array[0]);
                    DateTime to = DateTime.Parse(array[1] + " 11:59:59 PM");


                    dataTable = iinAndOut.getInAndOutLogBySearch(campus,from, to);
                }
            }
            return View(dataTable);
        }


        public ActionResult InAndOutReport()
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Accomodation = (iaccomadation.GetAccomadationslist);
                ViewBag.UserList = (iuser.getActiveUsers());
                ViewBag.CampusList = icampus.getCampuses();
                return View();
            }

        }

        public ActionResult getInAndOutReport(int campus,int u_id ,int ac_id,string reportrange)
        {
            DataTable dataTable = new DataTable();
            if (getCurrentUser() == null)
            {
                ViewBag.message = "Session Expired !!";
            }
            else
            {

                if (!reportrange.Equals("undefined"))
                {
                    String[] array = reportrange.Split('-');

                    DateTime from = DateTime.Parse(array[0]);
                    DateTime to = DateTime.Parse(array[1] + " 11:59:59 PM");
                    dataTable = iinAndOut.getInAndOutReport(campus,u_id, ac_id, from, to);
                }
            }
            return View(dataTable);
        }

        public DbResult AddInAndOut(InAndOut inAndOut)
        {
            DbResult dataTable = new DbResult();
            User user = getCurrentUser();
            if (user == null)
            {
                ViewBag.message = "Session Expired !!";
            }
            else
            {
                inAndOut.io_created_by = user.u_id;
                dataTable = iinAndOut.AddInAndOut(inAndOut);
            }
            return dataTable;
        }

        public DbResult updateInAndOutPunchType(InAndOut inAndOut)
        {
            DbResult dataTable = new DbResult();
            User user = getCurrentUser();
            if (user == null)
            {
                ViewBag.message = "Session Expired !!";
            }
            else
            {
                inAndOut.io_modified_by = user.u_id;
                dataTable = iinAndOut.updateInAndOutPunchType(inAndOut);
            }
            return dataTable;
        }
        
        
        public DbResult updateInAndOutPunchTypeUserWise(InAndOut inAndOut)
        {
            DbResult dataTable = new DbResult();
            User user = getCurrentUser();
            if (user == null)
            {
                ViewBag.message = "Session Expired !!";
            }
            else
            {
                inAndOut.io_created_by = user.u_id;
                dataTable = iinAndOut.updateInAndOutPunchTypeUserWise(inAndOut);
            }
            return dataTable;
        }

        public ActionResult campusWiseInAndOut()
        {
            User user = getCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Accomodation = (iaccomadation.GetAccomadationslist);
                ViewBag.CampusList = icampus.getCampuses();
                return View();
            }
          
        }

        public ActionResult getCampusWiseInAndOut(int campus,int ac_id ,string status)
        {
            DataTable dataTable = new DataTable();
            User user = getCurrentUser();
            if (user == null)
            {
                ViewBag.message = "Session Expired !!";
            }
            else
            {
                //habeebtest
                dataTable = iinAndOut.getCampusWiseInAndOut(campus, ac_id, status,user.u_id);
            }
            ViewBag.Total = dataTable.AsEnumerable().Count();
            ViewBag.In = dataTable.AsEnumerable().Where(row => row.Field<string>("CurrentStatus").Equals("In")).Count();
            ViewBag.Out = dataTable.AsEnumerable().Where(row => row.Field<string>("CurrentStatus").Equals("Out")).Count();
            return View(dataTable);
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