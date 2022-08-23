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
    public class UserShiftController : Controller
    {
        private readonly IRole irole;
        private readonly IMenu imenu;
        private readonly IUserShift iuserShift;
        private readonly IUser iuser;
        public UserShiftController(IRole _irole,IMenu _imenu, IUserShift _iuserShift, IUser _iuser)
        {
            irole = _irole;
            imenu = _imenu;
            iuserShift = _iuserShift;
            iuser = _iuser;
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
                ViewBag.UserList = iuser.GetUsersList();
                 string [] daynames ={ "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
                ViewBag.Daynames = daynames;
                return View(iuserShift.GetUserShifts);
            }
        }
        public DbResult Create(UserShift userShift)
        {
            DbResult result = new DbResult();
            User user = getCurrentUser();
            if (user != null)
            {
                userShift.us_cre_by = user.u_id;
                result = iuserShift.Add(userShift);
            }
            else
            {
                result.Message = "Session Expired ! Please Reload !!!";
            }

            return result;
        }

        [HttpPost]
        public ActionResult UserShiftData(int id,string reportrange)
        {
            User user = getCurrentUser();
            IEnumerable<List_UserShift> TrainingData = Enumerable.Empty<List_UserShift>();
            if (user == null)
            {
                ViewBag.Message = "Session Expired !! Please reload Page ";
            }
            else
            {
                if (reportrange!=null)
                {
                    String[] array = reportrange.Split('-');
                    DateTime from = DateTime.Parse(array[0]);
                    DateTime to = DateTime.Parse(array[1] + " 11:59:59 PM");
                    TrainingData = iuserShift.UserShiftData(from, to, id);
                }
            }
            return View(TrainingData);
        }

        [HttpPost]

        public Result FinishUserShift(UserShift _userShift)
        {
            Result result = new Result();
            if (getCurrentUser() == null)
            {
                result.Message = "Session Expired !! Please reload Page ";

            }
            else
            {
                UserShift userShift = iuserShift.GetUserShift(_userShift.us_id);
                if (userShift != null)
                {
                    if (_userShift.us_end_date >= userShift.us_start_date)
                    {
                        userShift.us_end_date = _userShift.us_end_date;
                        iuserShift.Update(userShift);

                        result.Message = "Success";
                    }else
                    {
                        result.Message = "End Date Should be greater than Start date ";
                    }
                }else
                {
                    result.Message = "Data Not Found!!!";
                }
            }
             return result;
        }




        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View(iuserShift.GetUserShift(id));
            }
        }

        // POST: Role/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Training training)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {

                    iuserShift.Remove(id);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
        }
        // GET: Role/Edit/5
        public ActionResult Edit(int id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View(iuserShift.GetUserShift(id));
            }
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserShift userShift)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    if (userShift.us_end_date >= userShift.us_start_date)
                    {
                        UserShift _userShift = iuserShift.GetUserShift(id);
                        _userShift.us_end_date = userShift.us_end_date;
                        _userShift.us_start_date = userShift.us_start_date;
                        _userShift.us_u_id = userShift.us_u_id;
                        iuserShift.Update(_userShift);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Message = "End Date Should be greater than start date !!";

                        return View();
                    }
                    
                }
                catch(Exception ex)
                {
                    ViewBag.Message = ex.Message + " " + ex.InnerException;
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