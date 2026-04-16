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

        /// <summary>
        /// Создаёт новый экземпляр репозитория посещений студентов.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public StudentLessonRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет посещение студентом занятия.
        /// </summary>
        /// <param name="sl">Посещение для добавления.</param>
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

        /// <summary>
        /// Получает список всех посещений студентов.
        /// </summary>
        /// <returns>Список посещений.</returns>
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
                    Console.WriteLine("StudentID\tLessonID\tMark\tIsPresent");
                    while (reader.Read())
                    {
                        var studentLesson = new StudentLesson
                        {
                            StudentID = (Guid)reader["StudentID"],
                            LessonID = (Guid)reader["LessonID"],
                            Mark = reader["Mark"] == DBNull.Value
                                ? null
                                : (decimal?)reader["Mark"],
                            IsPresent = reader["IsPresent"] == DBNull.Value
                                ? null
                                : (bool?)reader["IsPresent"]
                        };
                        list.Add(studentLesson);

                        string mark = studentLesson.Mark.HasValue ? studentLesson.Mark.Value.ToString("0.00") : "";
                        string isPresent = studentLesson.IsPresent.HasValue ? studentLesson.IsPresent.Value.ToString() : "";

                        Console.WriteLine($"{studentLesson.StudentID}\t{studentLesson.LessonID}\t{mark}\t{isPresent}");
                        Console.WriteLine();
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Обновляет посещение студентом занятия.
        /// </summary>
        /// <param name="sl">Посещение для обновления.</param>
        public override void Update(StudentLesson sl)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления посещения.
                using (var cmd = new SqlCommand(
                    "UPDATE StudentLesson " +
                    "SET Mark = @Mark, IsPresent = @IsPresent " +
                    "WHERE StudentID = @StudentID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", sl.StudentID);
                    cmd.Parameters.AddWithValue("@LessonID", sl.LessonID);
                    cmd.Parameters.AddWithValue("@Mark", (object)sl.Mark ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPresent", (object)sl.IsPresent ?? DBNull.Value);

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
        /// Удаляет посещение по идентификаторам студента и занятия.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор занятия.</param>
        public override void Delete(Guid studentId, Guid lessonId)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления посещения.
                using (var cmd = new SqlCommand(
                    "DELETE FROM StudentLesson " +
                    "WHERE StudentID = @StudentID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@LessonID", lessonId);

                    // Выполняем команду.
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