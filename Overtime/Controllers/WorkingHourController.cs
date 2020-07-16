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
    public class WorkingHourController : Controller
    {
        private readonly IRole irole;
        private readonly IMenu imenu;
        private readonly IWorkingHour iworkingHour;
        private readonly IOverTimeRequest ioverTimeRequest;
        public WorkingHourController(IRole _irole, IMenu _imenu,IWorkingHour _iworkingHour, IOverTimeRequest _ioverTimeRequest)
        {
            irole = _irole;
            imenu = _imenu;
            iworkingHour = _iworkingHour;
            ioverTimeRequest = _ioverTimeRequest;

        }
        [HttpPost]
        public ActionResult Index(int id, int doc_id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.workingHour = Math.Round(ioverTimeRequest.getWorkingHourByDocument(id, doc_id), 2);
                ViewBag.deduction = Math.Round(iworkingHour.GetWorkingHourConsolidateByDocument(id, doc_id), 2);
                return View(iworkingHour.GetWorkingHourByDocument(id, doc_id));
            }
        }

        // GET: WorkingHour/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WorkingHour/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkingHour/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                     if (Convert.ToDecimal(collection["wh_hours"]) > 0)
                        {
                            decimal workinghour = ioverTimeRequest.getWorkingHourByDocument(Convert.ToInt32(collection["id"]),
                                Convert.ToInt32(collection["doc_id"]));
                            decimal additionalworkinghour = iworkingHour.GetWorkingHourConsolidateByDocument(Convert.ToInt32(collection["id"]),
                                Convert.ToInt32(collection["doc_id"]));
                            decimal totalexpected = additionalworkinghour + Convert.ToDecimal(collection["wh_hours"]);

                            ViewBag.workingHour = Math.Round(workinghour, 2);
                            ViewBag.deduction = Math.Round(additionalworkinghour, 2);
                            if ((workinghour - totalexpected) >= 0)
                            {
                                WorkingHour workingHour = new WorkingHour();
                                workingHour.wh_fun_doc_id = Convert.ToInt32(collection["id"]);
                                workingHour.wh_doc_id = Convert.ToInt32(collection["doc_id"]);
                                workingHour.wh_remarks = Convert.ToString(collection["wh_remarks"]);
                                workingHour.wh_hours = Convert.ToDecimal(collection["wh_hours"]);
                                workingHour.wh_cre_by = getCurrentUser().u_id;
                                workingHour.wh_cre_date = DateTime.Now;
                                iworkingHour.Add(workingHour);
                            }
                            else
                            {
                                TempData["errorMessage"] = "Deduction amount greater than actual Working hour!! " + (workinghour - totalexpected);
                            }
                        }
                        else
                        {
                            TempData["errorMessage"] = "Deduction cannot be Zero!!";
                        }
                    
                    }catch (Exception ex)
                {
                    TempData["errorMessage"] = "Error:" + ex.Message;

                }
            
                
                return View(iworkingHour.GetWorkingHourByDocument(Convert.ToInt32(collection["id"]), Convert.ToInt32(collection["doc_id"])));
            }
        }

        // GET: WorkingHour/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WorkingHour/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WorkingHour/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WorkingHour/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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