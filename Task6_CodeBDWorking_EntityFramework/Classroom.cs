using System;

namespace EntityFramework_Database
{
    /// <summary>
    /// Represents a classroom.
    /// </summary>
    public class Classroom
    {
        /// <summary>
        /// Gets or sets the unique classroom identifier.
        /// </summary>
        public Guid ClassroomID { get; set; }

        /// <summary>
        /// Gets or sets the room name.
        /// </summary>
        public string RoomName { get; set; }

        /// <summary>
        /// Gets or sets the capacity of the classroom.
        /// </summary>
        public int? Capacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the classroom is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}