using System;

namespace EntityFramework_Database
{
    /// <summary>
    /// Represents the use of a computer during a lesson.
    /// </summary>
    public class ComputerLesson
    {
        /// <summary>
        /// Gets or sets the computer identifier.
        /// </summary>
        public Guid ComputerID { get; set; }

        /// <summary>
        /// Gets or sets the lesson identifier.
        /// </summary>
        public Guid LessonID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the computer was used.
        /// </summary>
        public bool? IsUsed { get; set; }
    }
}