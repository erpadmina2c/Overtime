using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
    public interface IAttachment
    {
        Attachment GetAttachment(int id);

        IEnumerable<Attachment> GetAttachments { get; }

        void Add(Attachment attachment);

        void Remove(int id);

        void Update(Attachment attachment);

        IEnumerable<Attachment> GetAttachmentsByDocument(int rowid, int doc_id);
    }
}
