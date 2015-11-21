using System;

namespace DataAccessLayer.Interfaces
{
    public interface IEntity
    {
        /// <summary>
        /// Entity Id
        /// </summary>
        Guid Id { get; }
    }
}