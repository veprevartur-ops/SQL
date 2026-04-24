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

        public LessonRepository(string connectionString) => _connectionString = connectionString;

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
                    }
                }
            }
            return lessons;
        }

        public override void Update(Lesson lesson)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(
                    "UPDATE Lesson " +
                    "SET Topic = @Topic, LessonDate = @LessonDate, ClassroomID = @ClassroomID, IsActive = @IsActive WHERE LessonID = @LessonID", conn))
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

        public override void Delete(Guid lessonId, Guid id2 = default)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "DELETE FROM Lesson " +
                    "WHERE LessonID = @LessonID", conn))
                {
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
        /// Возвращает строковое представление занятия.
        /// </summary>
        /// <param name="lesson">Экземпляр занятия.</param>
        /// <returns>Строка для вывода.</returns>
        public override string ToString(Lesson lesson)
        {
            return $"{lesson.LessonID}\t{lesson.Topic}\t{lesson.LessonDate:yyyy-MM-dd HH:mm:ss}\t{lesson.ClassroomID}\t{(lesson.IsActive ? "Активно" : "Неактивно")}";
        }

        /// <summary>
        /// Вывод в консоль всех занятий из таблицы Lesson.
        /// </summary>
        public override void PrintAll()
        {
            var all = GetAll();
            Console.WriteLine("LessonID\t\t\t\t\tTopic\tLessonDate\t\tClassroomID\t\t\t\t\tIsActive");
            foreach (var lesson in all)
            {
                Console.WriteLine(ToString(lesson));
            }
        }
    }
}