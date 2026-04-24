using Microsoft.EntityFrameworkCore;

namespace EntityFramework_Database
{
    /// <summary>
    /// Контекст базы данных EF Core.
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор контекста базы данных.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Таблица групп.
        /// </summary>
        public DbSet<Group> Groups { get; set; }

        /// <summary>
        /// Таблица студентов.
        /// </summary>
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// Таблица кабинетов.
        /// </summary>
        public DbSet<Classroom> Classrooms { get; set; }

        /// <summary>
        /// Таблица компьютеров.
        /// </summary>
        public DbSet<Computer> Computers { get; set; }

        /// <summary>
        /// Таблица занятий.
        /// </summary>
        public DbSet<Lesson> Lessons { get; set; }

        /// <summary>
        /// Таблица посещений студентами занятий.
        /// </summary>
        public DbSet<StudentLesson> StudentLessons { get; set; }

        /// <summary>
        /// Таблица использования компьютеров на занятиях.
        /// </summary>
        public DbSet<ComputerLesson> ComputerLessons { get; set; }

        /// <summary>
        /// Настройка строки подключения к базе данных.
        /// </summary>
        /// <param name="optionsBuilder">Построитель параметров.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Строка подключения к SQL Server.
            optionsBuilder.UseSqlServer(_connectionString);
        }

        /// <summary>
        /// Настройка модели базы данных.
        /// </summary>
        /// <param name="modelBuilder">Построитель модели.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройки ключей и названий таблиц.
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