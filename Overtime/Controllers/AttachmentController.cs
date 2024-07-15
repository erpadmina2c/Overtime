using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Overtime.Models;
using Overtime.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Overtime.Controllers
{
    public class AttachmentController : Controller
    {
        private readonly IUser iuser;
        private readonly IMenu imenu;
        private readonly IAttachment iattachment;
        private readonly IConfiguration configuration ;

        public AttachmentController(IUser _iuser, IMenu _imenu, IAttachment _iattachment, IConfiguration configuration)
        {
            iuser = _iuser;
            imenu = _imenu;
            iattachment = _iattachment;
            Configuration = configuration;

        }
        public IConfiguration Configuration { get; }
        // GET: DocumentImageController
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult GetAttachmentsByDocument(int fun_doc_id, int doc_id)
        //{
        //    List<Object> obj = new List<object>();
        //    Result result = new Result();

        //    try
        //    {
        //        String FileLocation = Configuration.GetConnectionString("FileServer");
        //        IEnumerable<Attachment> attachments = iattachment.GetAttachmentsByDocument(fun_doc_id, doc_id);

        //        obj.Add(new { FileLocation = FileLocation, Attachments = attachments });
             
        //        result.Objects = obj;
        //        result.Message = "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Objects = null;
        //        result.Message = ex.Message;
        //    }

        //    return new JsonResult(result);
        //}

        public ActionResult GetAttachmentsByDocument(int fun_doc_id, int doc_id)
        {
            IEnumerable<Attachment> attachments = Enumerable.Empty<Attachment>();
            List<Object> obj = new List<object>();
            Result result = new Result();

            try
            {
                String FileLocation = Configuration.GetConnectionString("FileServer");
                attachments = iattachment.GetAttachmentsByDocument(fun_doc_id, doc_id);

                obj.Add(new { FileLocation = FileLocation, Attachments = attachments });
                ViewBag.FileLocation = FileLocation;
                result.Objects = obj;
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                result.Objects = null;
                result.Message = ex.Message;
            }

            return View(attachments);
        }


        // GET: DocumentImageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DocumentImageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DocumentImageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: DocumentImageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DocumentImageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: DocumentImageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DocumentImageController/Delete/5
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
    }
}
