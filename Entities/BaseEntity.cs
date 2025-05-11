using System;

namespace BusBuddy.Entities
{
    /// <summary>
    /// Base abstract class for all entities
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// The unique identifier for the entity
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Date and time when the entity was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
