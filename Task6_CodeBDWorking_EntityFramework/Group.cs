using System;

namespace EntityFramework_Database
{
    /// <summary>
    /// Represents a student group.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets or sets the unique group identifier.
        /// </summary>
        public Guid GroupID { get; set; }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the group is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}