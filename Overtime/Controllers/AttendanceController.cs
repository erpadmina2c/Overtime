using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Overtime.Models;
using Overtime.Services;
using Newtonsoft.Json;
using Overtime.Repository;
using System.Data;

namespace Overtime.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IMenu imenu;
        private readonly IBioMatrix ibio;
        private readonly DBContext context;
        private readonly IUser iuser;
        private readonly ILeavetype ileavetype;
        private readonly ILeaveDetail ileavedetail;
        public AttendanceController(DBContext _context, IMenu _imenu, IBioMatrix _ibio, IUser _iuser, ILeavetype _ileavetype,ILeaveDetail _leaveDetail)
        {
            imenu = _imenu;
            ibio = _ibio;
            context = _context;
            iuser = _iuser;
            ileavetype = _ileavetype;
            ileavedetail = _leaveDetail;
        }
        // GET: AttendanceController
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
        [HttpPost]
        public ActionResult GetDailyAttendance(string date)
        {
           
          
                return View(ibio.GetAttendance(date));
           
        }

        [HttpPost]
        public ActionResult GetDailyAttendanceByDate(string date)
        {
            IEnumerable<List_Attendance> list_Attendances = Enumerable.Empty<List_Attendance>();
            User user = getCurrentUser();
            if (user != null)
            {
                list_Attendances = ibio.GetAttendanceByDate(date,user.u_id);
                return View(list_Attendances);
            }
            else
            {
                ViewBag.Message = "Session Expired !! Please Reload Page !!";
                return View(list_Attendances);
            }
            
              
            
        }
        public ActionResult EmailSetting()
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

        public ActionResult MachineDetail()
        {
            var values =  context.MachineDetails.ToList();
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View(values);
            }

        }

        // GET: AttendanceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AttendanceController/Create
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

        // POST: AttendanceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MachineDetail machine)
        {
            machine.created_at = DateTime.Now;
            ibio.Add(machine);
            return RedirectToAction(nameof(MachineDetail));
        }

        // GET: AttendanceController/Edit/5
        public ActionResult Edit(int id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                MachineDetail machine = ibio.GetMachine(id);
                return View(machine);
            }
        }

        // POST: AttendanceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MachineDetail machine)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var entity = context.MachineDetails.FirstOrDefault(item => item.machine_Id == id);

                if (entity != null)
                {
                    entity.machine_name = machine.machine_name;
                    entity.port_number = machine.port_number;
                    entity.ip_address = machine.ip_address;
                    context.SaveChanges();
                }
                return RedirectToAction(nameof(MachineDetail));
            }
        }
        public ActionResult MonthReport()
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


        [HttpPost]
        public ActionResult GetMonthReport(string reportrange)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                String[] array = reportrange.Split('-');

                DateTime rq_start_time = DateTime.Parse(array[0]);
                DateTime rq_end_time = DateTime.Parse(array[1] + " 11:59:59 PM");

                DataTable dataTable = new DataTable();
                dataTable = ibio.GetMonthReport(rq_start_time, rq_end_time);


                return View(dataTable);
            }
        }


        // GET: AttendanceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttendanceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
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


        // GET: AttendanceController/Create
        public ActionResult AddLeave()
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.UserList = (iuser.GetUsersList());
                ViewBag.LeaveType = ileavetype.GetLeavetypes;

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLeave(LeaveDetail leaveDetail)
        {
            User user = getCurrentUser();
            if (user == null)
            {
                ViewBag.UserList = (iuser.GetUsersList());
                ViewBag.LeaveType = ileavetype.GetLeavetypes;
                ViewBag.Message = "Session Expired !! Please Reload !!";
                return View();
            }
            else
            {
                var flag = "Add";
                var result = ileavedetail.AddUpdateLeave(leaveDetail, flag, user.u_id);
                if(result.Equals("Successfully added"))
                {
                    return RedirectToAction("LeaveDetail");
                }
                else
                {
                    ViewBag.UserList = (iuser.GetUsersList());
                    ViewBag.LeaveType = ileavetype.GetLeavetypes;
                    ViewBag.Message =result;
                    return View();
                }
               
            }
        }

        public ActionResult LeaveEdit(int id)
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.UserList = (iuser.GetUsersList());
                ViewBag.LeaveType = ileavetype.GetLeavetypes;
                LeaveDetail leaveobj = ibio.GetLeaveDetail().Where(m=>m.LeaveId== id).FirstOrDefault();
                return  View(leaveobj);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LeaveEdit(LeaveDetail leaveDetail)
        {
            User user = getCurrentUser();
            if (user == null)
            {
                ViewBag.UserList = (iuser.GetUsersList());
                ViewBag.LeaveType = ileavetype.GetLeavetypes;
                ViewBag.Message = "Session Expired !! Please Reload !!";
                return View();
            }
            else
            {
                var flag = "Update";
                var result = ileavedetail.AddUpdateLeave(leaveDetail, flag, user.u_id);
                if (result.Equals("Successfully added"))
                {
                    return RedirectToAction("LeaveDetail");
                }
                else
                {
                    ViewBag.UserList = (iuser.GetUsersList());
                    ViewBag.LeaveType = ileavetype.GetLeavetypes;
                    ViewBag.Message = result;
                    return View();
                }

            }
        }

        public ActionResult LeaveDetail()
        {
            if (getCurrentUser() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var values = ibio.GetLeaveDetail();
                return View(values);
            }

        }

        public ActionResult DeleteLeave(int LeaveId)
        {
           
           var result = ileavedetail.DeleteLeave(LeaveId);

            return new JsonResult(result);

        }
    }
}
