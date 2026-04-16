using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Repository for working with the Classroom table.
    /// </summary>
    public class ClassroomRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassroomRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public ClassroomRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a new classroom.
        /// </summary>
        /// <param name="classroom">The classroom to add.</param>
        public void Create(Classroom classroom)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Add the classroom to the context.
                db.Classrooms.Add(classroom);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all classrooms.
        /// </summary>
        /// <returns>List of classrooms.</returns>
        public List<Classroom> GetAll()
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Return all classrooms as a list.
                return new List<Classroom>(db.Classrooms);
            }
        }

        /// <summary>
        /// Updates an existing classroom.
        /// </summary>
        /// <param name="classroom">The classroom to update.</param>
        public void Update(Classroom classroom)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Update the classroom in the context.
                db.Classrooms.Update(classroom);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a classroom by its identifier.
        /// </summary>
        /// <param name="classroomId">The classroom identifier.</param>
        public void Delete(Guid classroomId)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Find the classroom by ID.
                var classroom = db.Classrooms.Find(classroomId);

                // If the classroom exists, remove it.
                if (classroom != null)
                {
                    db.Classrooms.Remove(classroom);

                    // Save changes to the database.
                    db.SaveChanges();
                }
            }
        }
    }
}