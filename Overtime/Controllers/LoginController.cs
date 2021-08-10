using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class LoginController : Controller
    {
        private readonly IUser iuser;
        private readonly ILoginLog iLoginLog;
        public LoginController(IUser _iuser,ILoginLog _iloginLog)
        {
            iuser = _iuser;
            iLoginLog = _iloginLog;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            var key = "shdfg2323g3g4j3879sdfh2j3237w8eh";
            try
            {
                if (user.u_name!=null&&user.u_password!=null)
                {
                    
                  
                    User newuser = iuser.getUserbyUsername(user.u_name);
                    if (newuser != null)
                    {
                        var newPassword = AesOperaions.DecryptString(key, newuser.u_password);
                        
                        if (user.u_password.ToString().Equals(newPassword.ToString()))
                        {
                            newuser.u_password = null;
                            string JsonStr = JsonConvert.SerializeObject(newuser);
                            HttpContext.Session.SetString("User", JsonStr);

                            LoginLog loginLog = new LoginLog();
                            loginLog.ll_cre_by = newuser.u_id;
                            loginLog.ll_login_time = DateTime.Now;
                            loginLog.ll_cre_date = DateTime.Now;
                            loginLog.ll_cre_by_name = newuser.u_name;
                            iLoginLog.Add(loginLog);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.Message = "User Name and Password are incorrect!!!";
                            return View("Index");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "User Name and Password are incorrect!!!";
                        return View("Index");
                    }
                }else
                {
                    ViewBag.Message = "Please enter username and Password";
                    return View("Index");
                }
                
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message+ " " +ex.InnerException;
                return View("Index");
            }
        }
        [HttpPost]
        public ResultData LoginAPI(string Username,string Password,string Token)
        {
            ResultData result = new ResultData();
            var ACT_Tocken = "DFG5DF65GFGD5TERTB6FZZSFREGCV546";
            var key = "shdfg2323g3g4j3879sdfh2j3237w8eh";
            try
            {
                if (Username != null && Password != null&& !Username.Equals("")&& !Password.Equals("")&& Token == ACT_Tocken)
                {


                    User newuser = iuser.getUserbyUsername(Username);
                    if (newuser != null)
                    {
                        var newPassword = AesOperaions.DecryptString(key, newuser.u_password);
                        if (Password.ToString().Equals(newPassword.ToString()))
                        {
                            newuser.u_password = null;
                            string JsonStr = JsonConvert.SerializeObject(newuser);
                            //HttpContext.Session.SetString("User", JsonStr);

                            LoginLog loginLog = new LoginLog();
                            loginLog.ll_cre_by = newuser.u_id;
                            loginLog.ll_login_time = DateTime.Now;
                            loginLog.ll_cre_date = DateTime.Now;
                            loginLog.ll_cre_by_name = newuser.u_name;
                            iLoginLog.Add(loginLog);
                            result.successData = newuser;
                            result.Message = "Success";
                            result.hasError = false;


                        }
                        else
                        {
                            result.Message = "User Name and Password are incorrect!!!";
                            result.successData = null;
                            result.hasError = true;
                        }
                    }
                    else
                    {

                        result.Message = "User Name and Password are incorrect!!!";
                        result.successData = null;
                        result.hasError = true;
                    }
                }
                else
                {

                    result.Message = "User Name and Password are incorrect!!!";
                    result.successData = null;
                    result.hasError = true;
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
               // result.Message = "User Name and Password are incrrect!!!";
                result.successData = null;
                result.hasError = true;
            }
            return result;
        }
    }
}