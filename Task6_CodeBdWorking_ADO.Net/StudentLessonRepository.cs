using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Репозиторий для работы с таблицей StudentLesson.
    /// </summary>
    public class StudentLessonRepository : RepositoryBase<StudentLesson>
    {
        private readonly string _connectionString;

        public StudentLessonRepository(string connectionString) => _connectionString = connectionString;

        public override void Create(StudentLesson sl)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO StudentLesson " +
                    "(StudentID, LessonID, Mark, IsPresent) " +
                    "VALUES (@StudentID, @LessonID, @Mark, @IsPresent)", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", sl.StudentID);
                    cmd.Parameters.AddWithValue("@LessonID", sl.LessonID);
                    cmd.Parameters.AddWithValue("@Mark", (object)sl.Mark ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPresent", (object)sl.IsPresent ?? DBNull.Value);

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

        public override List<StudentLesson> GetAll()
        {
            var list = new List<StudentLesson>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT StudentID, LessonID, Mark, IsPresent FROM StudentLesson", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var studentLesson = new StudentLesson
                        {
                            StudentID = (Guid)reader["StudentID"],
                            LessonID = (Guid)reader["LessonID"],
                            Mark = reader["Mark"] == DBNull.Value ? null : (decimal?)reader["Mark"],
                            IsPresent = reader["IsPresent"] == DBNull.Value ? null : (bool?)reader["IsPresent"]
                        };
                        list.Add(studentLesson);
                    }
                }
            }
            return list;
        }

        public override void Update(StudentLesson sl)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "UPDATE StudentLesson " +
                    "SET Mark = @Mark, IsPresent = @IsPresent " +
                    "WHERE StudentID = @StudentID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", sl.StudentID);
                    cmd.Parameters.AddWithValue("@LessonID", sl.LessonID);
                    cmd.Parameters.AddWithValue("@Mark", (object)sl.Mark ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPresent", (object)sl.IsPresent ?? DBNull.Value);

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

        public override void Delete(Guid studentId, Guid lessonId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "DELETE FROM StudentLesson " +
                    "WHERE StudentID = @StudentID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@LessonID", lessonId);

                    int affected = cmd.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        throw new InvalidOperationException("Удаление не выполнено: запись с таким ключом не найдена.");
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает строковое представление посещения студентом занятия.
        /// </summary>
        /// <param name="sl">Экземпляр посещения.</param>
        /// <returns>Строка для вывода.</returns>
        public override string ToString(StudentLesson sl)
        {
            string mark = sl.Mark.HasValue ? sl.Mark.Value.ToString("0.00") : "";
            string isPresent = sl.IsPresent.HasValue ? (sl.IsPresent.Value ? "Присутствовал" : "Отсутствовал") : "";
            return $"{sl.StudentID}\t{sl.LessonID}\t{mark}\t{isPresent}";
        }

        /// <summary>
        /// Вывод в консоль всех посещений занятий студентами из таблицы StudentLesson.
        /// </summary>
        public override void PrintAll()
        {
            var all = GetAll();
            Console.WriteLine("StudentID\t\t\tLessonID\t\t\tMark\tIsPresent");
            foreach (var sl in all)
            {
                Console.WriteLine(ToString(sl));
            }
        }
    }
}