using Newtonsoft.Json;
using Reusable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessSpecificLogic.FS
{
    public abstract class FSEntity : BaseEntity
    {
        [NotMapped]
        [JsonIgnore]
        public abstract string sqlGetAll { get; }

        [NotMapped]
        [JsonIgnore]
        public abstract string sqlGetById { get; }
    }
}
