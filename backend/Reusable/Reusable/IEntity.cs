using System;

namespace Reusable
{
    public interface IEntity : ICloneable
    {
        int id { get; }

        string AAA_EntityName { get; }
    }
}
