using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Repository for working with the Lesson table.
    /// </summary>
    public class LessonRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public LessonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a new lesson.
        /// </summary>
        /// <param name="lesson">The lesson to add.</param>
        public void Create(Lesson lesson)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Add the lesson to the context.
                db.Lessons.Add(lesson);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all lessons.
        /// </summary>
        /// <returns>List of lessons.</returns>
        public List<Lesson> GetAll()
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Return all lessons as a list.
                return new List<Lesson>(db.Lessons);
            }
        }

        /// <summary>
        /// Updates an existing lesson.
        /// </summary>
        /// <param name="lesson">The lesson to update.</param>
        public void Update(Lesson lesson)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Update the lesson in the context.
                db.Lessons.Update(lesson);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a lesson by its identifier.
        /// </summary>
        /// <param name="lessonId">The lesson identifier.</param>
        public void Delete(Guid lessonId)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Find the lesson by ID.
                var lesson = db.Lessons.Find(lessonId);

                // If the lesson exists, remove it.
                if (lesson != null)
                {
                    db.Lessons.Remove(lesson);

                    // Save changes to the database.
                    db.SaveChanges();
                }
            }
        }
    }
}