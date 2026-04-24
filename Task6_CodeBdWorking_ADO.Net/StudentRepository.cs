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

        public StudentRepository(string connectionString) => _connectionString = connectionString;

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
                    while (reader.Read())
                    {
                        var student = new Student
                        {
                            StudentID = (Guid)reader["StudentID"],
                            FullName = (string)reader["FullName"],
                            BirthDate = reader["BirthDate"] == DBNull.Value ? null : (DateTime?)reader["BirthDate"],
                            GPA = reader["GPA"] == DBNull.Value ? null : (decimal?)reader["GPA"],
                            GroupID = (Guid)reader["GroupID"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        students.Add(student);
                    }
                }
            }
            return students;
        }

        public override void Update(Student student)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
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

        public override void Delete(Guid studentId, Guid id2 = default)
        {
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

        /// <summary>
        /// Возвращает строковое представление студента.
        /// </summary>
        /// <param name="student">Экземпляр студента.</param>
        /// <returns>Строка для вывода.</returns>
        public override string ToString(Student student)
        {
            string birthDate = student.BirthDate.HasValue ? student.BirthDate.Value.ToShortDateString() : "";
            string gpa = student.GPA.HasValue ? student.GPA.Value.ToString("0.00") : "";
            return $"{student.StudentID}\t{student.FullName}\t{birthDate}\t{gpa}\t{student.GroupID}\t{(student.IsActive ? "Активен" : "Неактивен")}";
        }

        /// <summary>
        /// Вывод в консоль всех студентов из таблицы Student.
        /// </summary>
        public override void PrintAll()
        {
            var all = GetAll();
            Console.WriteLine("StudentID\t\t\tFullName\tBirthDate\tGPA\tGroupID\t\t\t\tIsActive");
            foreach (var student in all)
            {
                Console.WriteLine(ToString(student));
            }
        }
    }
}