using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Overtime.Models;
using Overtime.Services;

namespace Overtime.Controllers
{
    public class CampusController : Controller
    {
        private readonly ICampus icampus;
        private readonly IMenu imenu;

        public CampusController(ICampus _icampus, IMenu _imenu)
        {
            icampus = _icampus;
            imenu = _imenu;
        }

        // GET: Campus
        public ActionResult Index()
        {
            if (GetCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        // 🔹 Get campuses by name
        public ActionResult getCampuses()
        {
            List<Campus> campuses = new List<Campus>();

            if (GetCurrentUser() == null)
            {
                ViewBag.message = "Session Expired !!";
            }
            else
            {
                campuses = icampus.getCampuses();
            }

            return View(campuses);
        }

        public Campus getCampus(int c_id)
        {
            Campus campus = new Campus();

            if (GetCurrentUser() == null)
            {
                ViewBag.message = "Session Expired !!";
            }
            else
            {
                campus = icampus.getCampus(c_id);
            }

            return campus ;
        }

        // 🔹 Add or Update Campus
        [HttpPost]
        public DbResult addOrUpdateCampus(Campus campus)
        {
            DbResult dbResult = new DbResult();
            User user = GetCurrentUser();

            if (user == null)
            {
                dbResult.Message = "Session Expired !!";
                return dbResult;
            }

            try
            {
                if (campus.c_id == 0)
                {
                    campus.c_active_yn = "Y";
                    campus.c_cre_by = user.u_id;
                }

                dbResult = icampus.addOrUpdateCampus(campus);
            }
            catch (Exception ex)
            {
                dbResult.Message = ex.Message + " " + ex.InnerException;
            }

            return dbResult;
        }

        // 🔹 Delete Campus
        [HttpPost]
        public DbResult deleteCampus(int c_id)
        {
            DbResult dbResult = new DbResult();
            User user = GetCurrentUser();

            if (user == null)
            {
                dbResult.Message = "Session Expired !!";
                return dbResult;
            }

            try
            {
                dbResult = icampus.deleteCampus(c_id, user.u_id);
                dbResult.Message = "Success";
            }
            catch (Exception ex)
            {
                dbResult.Message = ex.Message + " " + ex.InnerException;
            }

            return dbResult;
        }

        // 🔹 Get current logged-in user
        private User GetCurrentUser()
        {
            try
            {
                if (HttpContext.Session.GetString("User") == null)
                    return null;

                User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));

                ViewBag.Name = user.u_full_name;
                ViewBag.isAdmin = user.u_is_admin;

                List<MenuItems> menulist = new List<MenuItems>();

                IEnumerable<Menu> menus =
                    imenu.getMenulistByRoleAndType(user.u_role_id, "Menu");

                foreach (var menu in menus)
                {
                    menulist.Add(new MenuItems
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
                        menuItem = imenu.getMenulistByRoleAndTypeAndParrent(
                            user.u_role_id, "MenuItem", menu.m_id)
                    });
                }

                ViewBag.MenuList = menulist;
                ViewBag.isMonitor = user.u_role_description == "Monitor" ? "Y" : "N";

                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}