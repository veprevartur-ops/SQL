using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Repository for working with the Computer table.
    /// </summary>
    public class ComputerRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public ComputerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a new computer.
        /// </summary>
        /// <param name="computer">The computer to add.</param>
        public void Create(Computer computer)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Add the computer to the context.
                db.Computers.Add(computer);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all computers.
        /// </summary>
        /// <returns>List of computers.</returns>
        public List<Computer> GetAll()
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Return all computers as a list.
                return new List<Computer>(db.Computers);
            }
        }

        /// <summary>
        /// Updates an existing computer.
        /// </summary>
        /// <param name="computer">The computer to update.</param>
        public void Update(Computer computer)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Update the computer in the context.
                db.Computers.Update(computer);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a computer by its identifier.
        /// </summary>
        /// <param name="computerId">The computer identifier.</param>
        public void Delete(Guid computerId)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Find the computer by ID.
                var computer = db.Computers.Find(computerId);

                // If the computer exists, remove it.
                if (computer != null)
                {
                    db.Computers.Remove(computer);

                    // Save changes to the database.
                    db.SaveChanges();
                }
            }
        }
    }
}