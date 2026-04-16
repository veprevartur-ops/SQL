using System;

namespace EntityFramework_Database
{
    /// <summary>
    /// Represents a student's attendance at a lesson.
    /// </summary>
    public class StudentLesson
    {
        /// <summary>
        /// Gets or sets the student identifier.
        /// </summary>
        public Guid StudentID { get; set; }

        /// <summary>
        /// Gets or sets the lesson identifier.
        /// </summary>
        public Guid LessonID { get; set; }

        /// <summary>
        /// Gets or sets the mark.
        /// </summary>
        public decimal? Mark { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the student was present.
        /// </summary>
        public bool? IsPresent { get; set; }
    }
}