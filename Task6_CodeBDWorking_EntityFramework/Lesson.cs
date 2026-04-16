using System;

namespace EntityFramework_Database
{
    /// <summary>
    /// Represents a lesson.
    /// </summary>
    public class Lesson
    {
        /// <summary>
        /// Gets or sets the unique lesson identifier.
        /// </summary>
        public Guid LessonID { get; set; }

        /// <summary>
        /// Gets or sets the topic of the lesson.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Gets or sets the lesson date.
        /// </summary>
        public DateTime LessonDate { get; set; }

        /// <summary>
        /// Gets or sets the classroom identifier.
        /// </summary>
        public Guid ClassroomID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the lesson is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}