using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Репозиторий для работы с таблицей Student.
    /// </summary>
    public class StudentRepository : RepositoryBase<Student>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория студентов.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public StudentRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет нового студента в базу данных.
        /// </summary>
        /// <param name="student">Студент для добавления.</param>
        public override void Create(Student student)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO Student" +
                    "(StudentID, FullName, BirthDate, GPA, GroupID, IsActive) " +
                    "VALUES (@StudentID, @FullName, @BirthDate, @GPA, @GroupID, @IsActive)", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
                    cmd.Parameters.AddWithValue("@FullName", student.FullName);
                    cmd.Parameters.AddWithValue("@BirthDate", (object)student.BirthDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GPA", (object)student.GPA ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GroupID", student.GroupID);
                    cmd.Parameters.AddWithValue("@IsActive", student.IsActive);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Message.Contains("FOREIGN KEY"))
                        {
                            throw new InvalidOperationException("Ошибка: указан несуществующий внешний ключ GroupID для студента.", ex);
                        }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Получает список всех студентов.
        /// </summary>
        /// <returns>Список студентов.</returns>
        public override List<Student> GetAll()
        {
            var students = new List<Student>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT StudentID, FullName, BirthDate, GPA, GroupID, IsActive FROM Student", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("StudentID\tFullName\tBirthDate\tGPA\tGroupID\tIsActive");
                    while (reader.Read())
                    {
                        var student = new Student
                        {
                            StudentID = (Guid)reader["StudentID"],
                            FullName = (string)reader["FullName"],
                            BirthDate = reader["BirthDate"] == DBNull.Value
                                ? null
                                : (DateTime?)reader["BirthDate"],
                            GPA = reader["GPA"] == DBNull.Value
                                ? null
                                : (decimal?)reader["GPA"],
                            GroupID = (Guid)reader["GroupID"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        students.Add(student);

                        string birthDate = student.BirthDate.HasValue ? student.BirthDate.Value.ToShortDateString() : "";
                        string gpa = student.GPA.HasValue ? student.GPA.Value.ToString("0.00") : "";

                        Console.WriteLine($"{student.StudentID}\t{student.FullName}\t{birthDate}\t{gpa}\t{student.GroupID}\t{student.IsActive}");
                        Console.WriteLine();
                    }
                }
            }
            return students;
        }

        /// <summary>
        /// Обновляет данные студента.
        /// </summary>
        /// <param name="student">Студент для обновления.</param>
        public override void Update(Student student)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления студента.
                using (var cmd = new SqlCommand(
                    "UPDATE Student " +
                    "SET FullName = @FullName, BirthDate = @BirthDate, GPA = @GPA, GroupID = @GroupID, IsActive = @IsActive " +
                    "WHERE StudentID = @StudentID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
                    cmd.Parameters.AddWithValue("@FullName", student.FullName);
                    cmd.Parameters.AddWithValue("@BirthDate", (object)student.BirthDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GPA", (object)student.GPA ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GroupID", student.GroupID);
                    cmd.Parameters.AddWithValue("@IsActive", student.IsActive);

                    // Выполняем команду.
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Message.Contains("FOREIGN KEY"))
                        {
                            throw new InvalidOperationException("Ошибка: указан несуществующий внешний ключ GroupID для студента.", ex);
                        }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Удаляет студента по идентификатору.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        public override void Delete(Guid studentId, Guid id2 = default)
        {
            // id2 игнорируется, потому что для Student нужен только один идентификатор
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(
                    "DELETE FROM Student " +
                    "WHERE StudentID = @StudentID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    int affected = cmd.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        throw new InvalidOperationException("Удаление не выполнено: запись с таким ключом не найдена.");
                    }
                }
            }
        }
    }
}