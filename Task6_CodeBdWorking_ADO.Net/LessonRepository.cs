using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Репозиторий для работы с таблицей Lesson.
    /// </summary>
    public class LessonRepository : RepositoryBase<Lesson>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория занятий.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public LessonRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет новое занятие в базу данных.
        /// </summary>
        /// <param name="lesson">Занятие для добавления.</param>
        public override void Create(Lesson lesson)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO Lesson " +
                    "(LessonID, Topic, LessonDate, ClassroomID, IsActive) " +
                    "VALUES (@LessonID, @Topic, @LessonDate, @ClassroomID, @IsActive)", conn))
                {
                    cmd.Parameters.AddWithValue("@LessonID", lesson.LessonID);
                    cmd.Parameters.AddWithValue("@Topic", (object)lesson.Topic ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LessonDate", lesson.LessonDate);
                    cmd.Parameters.AddWithValue("@ClassroomID", lesson.ClassroomID);
                    cmd.Parameters.AddWithValue("@IsActive", lesson.IsActive);

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
        /// Получает список всех занятий.
        /// </summary>
        /// <returns>Список занятий.</returns>
        public override List<Lesson> GetAll()
        {
            var lessons = new List<Lesson>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT LessonID, Topic, LessonDate, ClassroomID, IsActive FROM Lesson", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("LessonID\tTopic\tLessonDate\tClassroomID\tIsActive");
                    while (reader.Read())
                    {
                        var lesson = new Lesson
                        {
                            LessonID = (Guid)reader["LessonID"],
                            Topic = reader["Topic"] == DBNull.Value
                                ? null
                                : (string)reader["Topic"],
                            LessonDate = (DateTime)reader["LessonDate"],
                            ClassroomID = (Guid)reader["ClassroomID"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        lessons.Add(lesson);

                        // Печать содержимого строки
                        string lessonDate = lesson.LessonDate.ToString("yyyy-MM-dd HH:mm:ss");
                        Console.WriteLine($"{lesson.LessonID}\t{lesson.Topic}\t{lessonDate}\t{lesson.ClassroomID}\t{lesson.IsActive}");
                        Console.WriteLine();
                    }
                }
            }
            return lessons;
        }

        /// <summary>
        /// Обновляет данные занятия.
        /// </summary>
        /// <param name="lesson">Занятие для обновления.</param>
        public override void Update(Lesson lesson)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления занятия.
                using (var cmd = new SqlCommand(
                    "UPDATE Lesson " +
                    "SET Topic = @Topic, LessonDate = @LessonDate, ClassroomID = @ClassroomID, IsActive = @IsActive WHERE LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@LessonID", lesson.LessonID);
                    cmd.Parameters.AddWithValue("@Topic", (object)lesson.Topic ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LessonDate", lesson.LessonDate);
                    cmd.Parameters.AddWithValue("@ClassroomID", lesson.ClassroomID);
                    cmd.Parameters.AddWithValue("@IsActive", lesson.IsActive);

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
        /// Удаляет занятие по идентификатору.
        /// </summary>
        /// <param name="lessonId">Идентификатор занятия.</param>
        public override void Delete(Guid lessonId, Guid id2 = default)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления занятия.
                using (var cmd = new SqlCommand(
                    "DELETE FROM Lesson " +
                    "WHERE LessonID = @LessonID", conn))
                {
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