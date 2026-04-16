using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Database
{
    /// <summary>
    /// The EF Core database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Gets or sets the groups table.
        /// </summary>
        public DbSet<Group> Groups { get; set; }

        /// <summary>
        /// Gets or sets the students table.
        /// </summary>
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// Gets or sets the classrooms table.
        /// </summary>
        public DbSet<Classroom> Classrooms { get; set; }

        /// <summary>
        /// Gets or sets the computers table.
        /// </summary>
        public DbSet<Computer> Computers { get; set; }

        /// <summary>
        /// Gets or sets the lessons table.
        /// </summary>
        public DbSet<Lesson> Lessons { get; set; }

        /// <summary>
        /// Gets or sets the student lessons table.
        /// </summary>
        public DbSet<StudentLesson> StudentLessons { get; set; }

        /// <summary>
        /// Gets or sets the computer lessons table.
        /// </summary>
        public DbSet<ComputerLesson> ComputerLessons { get; set; }

        /// <summary>
        /// Configures the database connection.
        /// </summary>
        /// <param name="optionsBuilder">The options builder.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Set the SQL Server connection string.
            optionsBuilder.UseSqlServer(_connectionString);
        }

        /// <summary>
        /// Configures the database model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity keys and table names.
            modelBuilder.Entity<Group>().ToTable("Group").HasKey(g => g.GroupID);
            modelBuilder.Entity<Student>().ToTable("Student").HasKey(s => s.StudentID);
            modelBuilder.Entity<Classroom>().ToTable("Classroom").HasKey(c => c.ClassroomID);
            modelBuilder.Entity<Computer>().ToTable("Computer").HasKey(c => c.ComputerID);
            modelBuilder.Entity<Lesson>().ToTable("Lesson").HasKey(l => l.LessonID);
            modelBuilder.Entity<StudentLesson>().ToTable("StudentLesson").HasKey(sl => new { sl.StudentID, sl.LessonID });
            modelBuilder.Entity<ComputerLesson>().ToTable("ComputerLesson").HasKey(cl => new { cl.ComputerID, cl.LessonID });
        }
    }
}