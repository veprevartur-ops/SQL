using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Repository for working with the ComputerLesson table.
    /// </summary>
    public class ComputerLessonRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerLessonRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public ComputerLessonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a computer usage at a lesson.
        /// </summary>
        /// <param name="cl">The computer lesson to add.</param>
        public void Create(ComputerLesson cl)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Add the computer lesson to the context.
                db.ComputerLessons.Add(cl);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all computer lessons.
        /// </summary>
        /// <returns>List of computer lessons.</returns>
        public List<ComputerLesson> GetAll()
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Return all computer lessons as a list.
                return new List<ComputerLesson>(db.ComputerLessons);
            }
        }

        /// <summary>
        /// Updates an existing computer lesson.
        /// </summary>
        /// <param name="cl">The computer lesson to update.</param>
        public void Update(ComputerLesson cl)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Update the computer lesson in the context.
                db.ComputerLessons.Update(cl);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a computer lesson by computer and lesson identifiers.
        /// </summary>
        /// <param name="computerId">The computer identifier.</param>
        /// <param name="lessonId">The lesson identifier.</param>
        public void Delete(Guid computerId, Guid lessonId)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Find the computer lesson by IDs.
                var cl = db.ComputerLessons.Find(computerId, lessonId);

                // If the computer lesson exists, remove it.
                if (cl != null)
                {
                    db.ComputerLessons.Remove(cl);

                    // Save changes to the database.
                    db.SaveChanges();
                }
            }
        }
    }
}