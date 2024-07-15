using Overtime.Models;
using Overtime.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class AttachmentRepository : IAttachment
    {
        private DBContext db;

        public AttachmentRepository(DBContext _db)
        {
            db = _db;
        }
        public IEnumerable<Attachment> GetAttachments => db.Attachments;

        public void Add(Attachment attachment)
        {
            db.Attachments.Add(attachment);
            db.SaveChanges();
        }

        public Attachment GetAttachment(int id)
        {
            Attachment attachment = db.Attachments.Find(id);
            return attachment;
        }

        public IEnumerable<Attachment> GetAttachmentsByDocument(int rowid, int doc_id)
        {

            var _rowid = new SqlParameter("rowid", rowid + "");
            var _doc_id = new SqlParameter("doc_id", doc_id + "");

            var dbResults = db.Attachments.FromSqlRaw<Attachment>("EXECUTE dbo.GetAttachmentsByDocument @rowid,@doc_id", _rowid, _doc_id).ToList();
            return dbResults;

          
        }

        public void Remove(int id)
        {
            Attachment attachment = db.Attachments.Find(id);
            db.Attachments.Remove(attachment);
            db.SaveChanges();
        }

        public void Update(Attachment attachment)
        {
            db.Attachments.Update(attachment);
            db.SaveChanges();
        }
    }
}
