using System;

namespace DataAccessLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
