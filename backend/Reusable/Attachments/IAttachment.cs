using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable.Attachments
{
    public interface IAttachment
    {
        string AttachmentsFolder { get; set; }

        [NotMapped]
        List<Attachment> Attachments { get; set; }
    }
}
