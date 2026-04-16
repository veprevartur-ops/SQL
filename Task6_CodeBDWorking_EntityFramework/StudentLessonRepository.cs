using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Repository for working with the StudentLesson table.
    /// </summary>
    public class StudentLessonRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentLessonRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public StudentLessonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a student's attendance at a lesson.
        /// </summary>
        /// <param name="sl">The student lesson to add.</param>
        public void Create(StudentLesson sl)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Add the student lesson to the context.
                db.StudentLessons.Add(sl);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all student lessons.
        /// </summary>
        /// <returns>List of student lessons.</returns>
        public List<StudentLesson> GetAll()
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Return all student lessons as a list.
                return new List<StudentLesson>(db.StudentLessons);
            }
        }

        /// <summary>
        /// Updates an existing student lesson.
        /// </summary>
        /// <param name="sl">The student lesson to update.</param>
        public void Update(StudentLesson sl)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Update the student lesson in the context.
                db.StudentLessons.Update(sl);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a student lesson by student and lesson identifiers.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <param name="lessonId">The lesson identifier.</param>
        public void Delete(Guid studentId, Guid lessonId)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Find the student lesson by IDs.
                var sl = db.StudentLessons.Find(studentId, lessonId);

                // If the student lesson exists, remove it.
                if (sl != null)
                {
                    db.StudentLessons.Remove(sl);

                    // Save changes to the database.
                    db.SaveChanges();
                }
            }
        }
    }
}