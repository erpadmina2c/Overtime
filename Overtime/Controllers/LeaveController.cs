using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Overtime.Models;
using Overtime.Services;

namespace Overtime.Controllers
{
    public class LeaveController : Controller
    {
        private readonly IRole irole;
        private readonly IMenu imenu;
        private readonly ILeave ileave;
        private readonly IDepartment idepartment;
        private readonly IDesignation idesignation;
        private readonly IUser iusers;
        private readonly ILeavetype ileavetype;
        private readonly IAttachment iattachment;
        private readonly IConfiguration configuration;
        private readonly IArchivedLeave iarchivedLeave;
        public LeaveController(IUser _iusers,IRole _irole, IMenu _imenu, ILeave _ileave, 
            IDepartment _idepartment, ILeavetype _ileavetype, IConfiguration configuration,
            IAttachment _iattachment, IArchivedLeave _iarchivedLeave,IDesignation _idesignation)
        {
            iusers = _iusers;
            irole = _irole;
            imenu = _imenu;
            ileave = _ileave;
            idepartment = _idepartment;
            ileavetype = _ileavetype;
            Configuration = configuration;
            iattachment = _iattachment;
            iarchivedLeave = _iarchivedLeave;
            idesignation = _idesignation;
        }
        public ActionResult Index()
        {
            User user = getCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.CurrentUser = user;
                ViewBag.LeaveRequestForOthers = imenu.getMenulistByRoleAndTypeAndLink(user.u_role_id, "Options", "LeaveRequestForOthers");
                ViewBag.Departments = idepartment.GetDepartments;
                ViewBag.Users = iusers.getActiveUsers();
                ViewBag.LeaveTypes = ileavetype.GetLeavetypes;
                ViewBag.Designations = idesignation.GetDesignations();
                return View();
            }
        }
        public IConfiguration Configuration { get; }
        public ActionResult getLeaveRequests()
        {
            List<Leave> leaves = new List<Leave>();
            User user = getCurrentUser();
            if (user == null)
            {

            }
            else
            {
                leaves = ileave.getLeaveRequests(user.u_id);
            }
            return View(leaves);
        }


        public Leave getLeave(int l_id)
        {
            Leave leave = new Leave();
            leave=ileave.getLeave(l_id);
            return leave;
        }

        public async Task<DbResultWithObject> createOrUpdateLeaveAsync(Leave leave, List<IFormFile> files)
        {
            DbResultWithObject result = new DbResultWithObject();
            User user = getCurrentUser();
            if (user != null)
            {

                String FileLocation = Configuration.GetConnectionString("FileLocation");
                var urls = new List<string>();
                if (files != null || files.Count != 0)
                {
                    long size = files.Sum(f => f.Length);
                    var filePaths = new List<string>();

                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0)
                        {
                            var myUniqueFileName = string.Format(@"{0}" + Path.GetExtension(formFile.FileName), DateTime.Now.Ticks);
                            // full path to file in temp location
                            String strDate = DateTime.Now.ToString("dd-MM-yyyy");

                            var filePath = Path.Combine(FileLocation, "Overtime");
                            filePath = Path.Combine(filePath, strDate);
                            bool exists = System.IO.Directory.Exists(filePath);
                            if (!exists)
                                System.IO.Directory.CreateDirectory(filePath);
                            filePaths.Add(filePath);

                            var fileNameWithPath = string.Concat(filePath, "\\", myUniqueFileName);
                            urls.Add(string.Concat(strDate, "\\", myUniqueFileName));

                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                        }
                    }
                }
                leave.l_cre_by = user.u_id;
                result = ileave.createOrUpdateLeave(leave);
                if (result.Message == "Success")
                {
                    try
                    {
                        if (urls != null || urls.Count != 0)
                        {
                            foreach (var path in urls)
                            {
                                Attachment attachment = new Attachment();
                                attachment.at_doc_id = 2;
                                attachment.at_path = path;
                                attachment.at_cre_date = DateTime.Now;
                                attachment.at_cre_by = user.u_id;
                                attachment.at_fun_doc_id = Convert.ToInt32(result.Obj.ToString());
                                iattachment.Add(attachment);
                            }
                        }
                        result.Message = "Success";
                    }
                    catch(Exception ex)
                    {
                        result.Message = ex.Message+" "+ex.InnerException;
                    }
                   
                }
               
            }
            else
            {
                result.Message = "Session Expired !! Please Reload Page!!!";
            }

            return result;

        }


        public ActionResult ReviewLeaveApplications()
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




        public ActionResult getViewLeaveDetail(int l_id)
        {
            ViewBag.Departments = idepartment.GetDepartments;
            ViewBag.Users = iusers.GetUsers;
            ViewBag.LeaveTypes = ileavetype.GetLeavetypes;
            ViewBag.Designations = idesignation.GetDesignations();
            Leave leave = ileave.getLeave(l_id);
            return View(leave);
        }
      

        public ActionResult getLeaveApplicationsForReview()
        {
            List<Leave> leaves = new List<Leave>();
            User user = getCurrentUser();
            if (user == null)
            {

            }
            else
            {
                leaves = ileave.getLeaveApplicationsForReview(user.u_id);
            }
            return View(leaves);
        }

        public DbResult AuthorizeLeave(int l_id, string l_authorization)
        {
            DbResult result = new DbResult();
            User user = getCurrentUser();
            if (user != null)
            {
               
                result = ileave.AuthorizeLeave(l_id,l_authorization, user.u_id);
            }
            else
            {
                result.Message = "Session Expired !! Please Reload Page!!!";
            }

            return result;
        }

        public ActionResult LeaveApproval()
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


        public ActionResult getLeaveApplicationsForHrApproval()
        {
            List<Leave> leaves = new List<Leave>();
            User user = getCurrentUser();
            if (user == null)
            {

            }
            else
            {
                leaves = ileave.getLeaveApplicationsForHrApproval(user.u_id);
            }
            return View(leaves);
        }


        public DbResult ApproveLeave(int id,string type)                        
        {                                                                       
            DbResult dbResult = new DbResult();                                 
            User user = getCurrentUser();                                       
            if (user != null)                                                   
            {                                                                   

                dbResult = ileave.ApproveLeave(id, type, user.u_id);            
            }                                                                   
            else                                                                
            {                                                                   
                dbResult.Message = "Session Expired !! Please Reload Page!!!";  
            }                                                                   
            return dbResult;                                                    
        }

        public DbResult deleteLeaveApplication(int l_id)
        {
            DbResult dbResult = new DbResult();
            User user = getCurrentUser();
            if (user != null)
            {

                dbResult = ileave.deleteLeaveApplication(l_id,  user.u_id);
            }
            else
            {
                dbResult.Message = "Session Expired !! Please Reload Page!!!";
            }
            return dbResult;
        }

        public ActionResult LeaveReport()
        {
            User user = getCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Users = iusers.GetUsersList();
                return View();
            }
        }

        public ActionResult getLeaveReport(int u_id, string reportrange, string type, string fullhistory)
        {
            DataTable dataTable = new DataTable();

            if (!reportrange.Equals("undefined"))
            {
                String[] array = reportrange.Split('-');

                DateTime from = DateTime.Parse(array[0]);
                DateTime to = DateTime.Parse(array[1] + " 11:59:59 PM");

                dataTable = ileave.getLeaveReport(u_id, from, to, type, fullhistory);
            }

            return View(dataTable);
        }


        public ActionResult LeaveCalculator()
        {
            User user = getCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Users = iusers.GetUsersList();
                return View();
            }
        }

        public ActionResult getRemainingLeave( int u_id)
        {

            ViewBag.Employee = iusers.GetUser(u_id);
            List<Leave>  leaves= ileave.getLeaveDetailsOfAEmployee(u_id);
            RemainingLeave remainingLeave = ileave.getRemainingLeave(u_id);
            ViewBag.remainingLeave = remainingLeave;
            
            return View(leaves);
        } 
        public ActionResult getRemainingLeaveOnly( int u_id)
        {
            RemainingLeave remainingLeave = ileave.getRemainingLeave(u_id);
            ViewBag.remainingLeave = remainingLeave;
            
            return View(remainingLeave);
        }

        public ActionResult ArchivedLeaves()
        {
            User user = getCurrentUser();
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Users = iusers.GetUsersList();
                return View();
            }
        }
        public ActionResult getArchivedLeaves()
        {
            List<ArchivedLeave> archivedLeaves = new List<ArchivedLeave>();
            archivedLeaves=iarchivedLeave.getArchivedLeaves();
            return View(archivedLeaves);
        }

        public DbResult createArchivedLeave(int u_id, int leave_days)
        {
            DbResult dbResult = new DbResult();
            User user = getCurrentUser();
            if (user == null)
            {
                dbResult.Message = "Session Expired !!";
            }
            else
            {
                dbResult = iarchivedLeave.createArchivedLeave(u_id, leave_days, user.u_id);
            }
            return dbResult;
        } 
        public DbResult updateArchivedLeave(int al_id, int leave_days)
        {
            DbResult dbResult = new DbResult();
            User user = getCurrentUser();
            if (user == null)
            {
                dbResult.Message = "Session Expired !!";
            }
            else
            {
                dbResult = iarchivedLeave.updateArchivedLeave(al_id, leave_days, user.u_id);
            }
            return dbResult;
        }


        private User getCurrentUser()
        {
            try
            {
                if (HttpContext.Session.GetString("User") == null)
                {
                    return null ;
                }
                else
                {
                    User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"))?? new User();
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