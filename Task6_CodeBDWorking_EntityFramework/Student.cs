using System;

namespace EntityFramework_Database
{
    /// <summary>
    /// Represents a student.
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Gets or sets the unique student identifier.
        /// </summary>
        public Guid StudentID { get; set; }

        /// <summary>
        /// Gets or sets the full name of the student.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the birth date of the student.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the GPA of the student.
        /// </summary>
        public decimal? GPA { get; set; }

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        public Guid GroupID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the student is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}