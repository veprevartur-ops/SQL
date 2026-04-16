using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Repository for working with the Student table.
    /// </summary>
    public class StudentRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a new student.
        /// </summary>
        /// <param name="student">The student to add.</param>
        public void Create(Student student)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Add the student to the context.
                db.Students.Add(student);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all students.
        /// </summary>
        /// <returns>List of students.</returns>
        public List<Student> GetAll()
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Return all students as a list.
                return new List<Student>(db.Students);
            }
        }

        /// <summary>
        /// Updates an existing student.
        /// </summary>
        /// <param name="student">The student to update.</param>
        public void Update(Student student)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Update the student in the context.
                db.Students.Update(student);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a student by its identifier.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        public void Delete(Guid studentId)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Find the student by ID.
                var student = db.Students.Find(studentId);

                // If the student exists, remove it.
                if (student != null)
                {
                    db.Students.Remove(student);

                    // Save changes to the database.
                    db.SaveChanges();
                }
            }
        }
    }
}