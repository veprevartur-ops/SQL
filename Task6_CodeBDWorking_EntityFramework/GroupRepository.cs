using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Repository for working with the Group table.
    /// </summary>
    public class GroupRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public GroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Adds a new group.
        /// </summary>
        /// <param name="group">The group to add.</param>
        public void Create(Group group)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Add the group to the context.
                db.Groups.Add(group);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all groups.
        /// </summary>
        /// <returns>List of groups.</returns>
        public List<Group> GetAll()
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Return all groups as a list.
                return new List<Group>(db.Groups);
            }
        }

        /// <summary>
        /// Updates an existing group.
        /// </summary>
        /// <param name="group">The group to update.</param>
        public void Update(Group group)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Update the group in the context.
                db.Groups.Update(group);

                // Save changes to the database.
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a group by its identifier.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        public void Delete(Guid groupId)
        {
            // Create a new database context.
            using (var db = new AppDbContext(_connectionString))
            {
                // Find the group by ID.
                var group = db.Groups.Find(groupId);

                // If the group exists, remove it.
                if (group != null)
                {
                    db.Groups.Remove(group);

                    // Save changes to the database.
                    db.SaveChanges();
                }
            }
        }
    }
}