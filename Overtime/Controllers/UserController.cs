using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Overtime.Models;
using Overtime.Repository;
using Overtime.Services;

namespace Overtime.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private readonly IUser iuser;
        private readonly IRole irole;
        private readonly IDepartment idepartment;
        private readonly IMenu imenu;
        private readonly ILoginLog iloginLog;
        private readonly IUserReportingHierarchy iuserReportingHierarchy;


        public UserController(IUser _iuser,IRole _irole, IDepartment _idepartment,IMenu _imenu,ILoginLog _loginLog, IUserReportingHierarchy _iuserReportingHierarchy)
        {
            iuser=_iuser;
            irole = _irole;
            idepartment = _idepartment;
            imenu = _imenu;
            iloginLog = _loginLog;
            iuserReportingHierarchy = _iuserReportingHierarchy;
        }
        
        public ActionResult Index()
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {

                return View(iuser.GetUsersList());
            }
            
        }

        public ActionResult UserReportingHeirarchy()
        {
            User user = getCurrentUser();
            ViewBag.Users = iuser.GetUsersList();
            if (user != null)
            {
               
                  
            }
            else
            {
               
                ViewBag.Message = "Session Already Expired!!! Please reload !!";
            }
            return View();
        }
        [HttpPost]
        public ActionResult getUserReportingHeirarchy(UserReportingHierarchy userReportingHierarchy)
        {
            User user = getCurrentUser();
            IEnumerable<List_UserReportingHierarchy> list_UserReportingHierarchies = Enumerable.Empty<List_UserReportingHierarchy>();
            if (user != null)
            {
                list_UserReportingHierarchies = iuserReportingHierarchy.GetUserReportingHierarchysByuser(userReportingHierarchy);
            }
            else
            {

                ViewBag.Message = "Session Expired !! Please reload Page !!";
            }

            return View(list_UserReportingHierarchies);
        }

        public Result deleteEmployeeReportingHirarchy(int urh_id)
        {
            Result result = new Result();
            User user = getCurrentUser();
            if(user!=null)
            {
                UserReportingHierarchy userReportingHeirarchy = iuserReportingHierarchy.GetUserReportingHierarchy(urh_id);
                if (userReportingHeirarchy != null)
                {
                    iuserReportingHierarchy.Remove(urh_id);
                    result.Message = "Success";
                }else
                {
                    result.Message = "Data Not Found!!!";
                }

            }
            else
            {
                result.Message = "Session Expired !! Please Reload Page!!!";
            }

            return result;

        }



        [HttpPost]
        public DbResult AddUserReportingHeirarchy(UserReportingHierarchy userReportingHierarchy)
        {
            DbResult result = new DbResult();
            User user = getCurrentUser();
            if (user != null)
            {
                result = iuserReportingHierarchy.addUserReportingHierarchy(userReportingHierarchy);
            }
            else
            {

                result.Message = "Session Expired !! Please reload Page !!";
            }
            return result;
        }


        // GET: User/Details/5
        public ActionResult Details(int id)
        {

            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {

                ViewBag.RoleList = (irole.GetRoles);
                ViewBag.DepartmentList = (idepartment.GetDepartments);
            }
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    if (user.u_role_id != 0)
                    {
                        User usercheck = iuser.getUserbyUsername(user.u_name);
                        if (usercheck == null)
                        {
                            var key = "shdfg2323g3g4j3879sdfh2j3237w8eh";
                            var encryptedString = AesOperaions.EncryptString(key, user.u_password);
                            user.u_password = encryptedString.ToString();
                            user.u_cre_by = getCurrentUser().u_id;
                            user.u_cre_date = DateTime.Now;
                            user.u_active_yn = "Y";
                            iuser.Add(user);
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ViewBag.RoleList = (irole.GetRoles);
                            ViewBag.DepartmentList = (idepartment.GetDepartments);
                            ViewBag.Message = "Username already exsist";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Please enter all column";
                        return View();
                    }

                    
                }
                catch(Exception ex)
                {
                    ViewBag.RoleList = (irole.GetRoles);
                    ViewBag.DepartmentList = (idepartment.GetDepartments);
                    ViewBag.Message = ex.Message;
                    return View();
                }
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.RoleList = (irole.GetRoles);
                ViewBag.DepartmentList = (idepartment.GetDepartments);
                User user = iuser.GetUser(id);
                user.u_password = null;
                return View(user);
            }
        }
        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User user)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    User temp_user = iuser.GetUser(id);
                    
                    var key = "shdfg2323g3g4j3879sdfh2j3237w8eh";
                   
                    temp_user.u_full_name = user.u_full_name;
                    temp_user.u_name = user.u_name;
                    temp_user.u_is_admin = user.u_is_admin;
                    temp_user.u_role_id = user.u_role_id;
                    temp_user.u_active_yn = user.u_active_yn;
                    temp_user.u_email = user.u_email;
                    temp_user.u_allocation_yn= user.u_allocation_yn;
                    if(user.u_password!=null)
                    {
                        var encryptedString = AesOperaions.EncryptString(key, user.u_password);
                        temp_user.u_password = encryptedString.ToString();
                    }
                    else
                    {
                        temp_user.u_password = temp_user.u_password;
                    }
                    
                   
                    iuser.Update(temp_user);

                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ViewBag.RoleList = (irole.GetRoles);
                    ViewBag.DepartmentList = (idepartment.GetDepartments);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    ViewBag.Message = ex.Message ;
                    return View();
                }
               
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View(iuser.GetUser(id));
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id,User user)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {

                    user = iuser.GetUser(id);
                    iuser.Remove(id);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {

                    return View();
                }
            }
        }

        // GET: User/Delete/5
        public ActionResult UserLoginHistory(int id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.UserList = iuser.GetUsersList();
                return View();
            }
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult UserLoginHistoryBySearch(int id,string reportrange)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    String[] array = reportrange.Split('-');

                    DateTime start_time = DateTime.Parse(array[0]);
                    DateTime end_time = DateTime.Parse(array[1] + " 11:59:59 PM");

                    IEnumerable<LoginLog> loginLogs= iloginLog.GetLoginLogsBySearch(id,start_time,end_time);

                    return View(loginLogs);

                }
                catch
                {

                    return View();
                }
            }
        }

        public ActionResult EmployeeInformation()
        {
            User user = getCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                ViewBag.UserList = iuser.GetUsersList();
            }
            return View();
        }
        public ActionResult getEmployeeInformation(int u_id,string Type)
        {
            List<List_User> list_Users = new List<List_User>();
            User user = getCurrentUser();
            if (user != null)
            {
                list_Users = iuser.getEmployeeInformation(u_id,Type);
            }
            else
            {
                ViewBag.Message = "Session Timeout !! Please Reload the Page !!";
            }
            return View(list_Users);
        }

        public DbResult CancelEmployee(int u_id, string u_cancelation_date)
        {
            DbResult dbResult = new DbResult();
            User user = getCurrentUser();
            if (user != null)
            {
                dbResult = iuser.CancelEmployee(u_id, u_cancelation_date,user.u_id);
            }
            else
            {
                ViewBag.Message = "Session Timeout !! Please Reload the Page !!";
            }
            return dbResult;
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
                   
                    if (user.u_role_id ==999) ViewBag.isMonitor = "Y";
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