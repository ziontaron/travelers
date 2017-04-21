using Reusable;
using Reusable.Attachments;
using System.Collections.Generic;

namespace BusinessSpecificLogic.EF
{
    public partial class CQALine : BaseDocument, IAttachment
    {
        public CQALine()
        {
            sys_active = true;
        }

        public string AttachmentsFolder { get; set; }
        public List<Attachment> Attachments { get; set; }

        public override int id
        {
            get
            {
                return CQALinelKey;
            }
        }
    }
}
