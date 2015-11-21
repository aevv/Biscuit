using System;
using DapperExtensions.Mapper;

namespace DataAccessLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
        public KeyType KeyType { get; set; }
    }
}
