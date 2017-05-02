using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reusable
{
    public interface IRecursiveEntity : IEntity
    {
        [NotMapped]
        List<IRecursiveEntity> nodes { get; set; }
    }
}
