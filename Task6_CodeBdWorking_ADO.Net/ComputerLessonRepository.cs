using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Репозиторий для работы с таблицей ComputerLesson.
    /// </summary>
    public class ComputerLessonRepository : RepositoryBase<ComputerLesson>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория использования компьютеров.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public ComputerLessonRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет использование компьютера на занятии.
        /// </summary>
        /// <param name="cl">Использование для добавления.</param>
        public override void Create(ComputerLesson cl)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO ComputerLesson " +
                    "(ComputerID, LessonID, IsUsed) " +
                    "VALUES (@ComputerID, @LessonID, @IsUsed)", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", cl.ComputerID);
                    cmd.Parameters.AddWithValue("@LessonID", cl.LessonID);
                    cmd.Parameters.AddWithValue("@IsUsed", (object)cl.IsUsed ?? DBNull.Value);

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
        /// Получает список всех использований компьютеров.
        /// </summary>
        /// <returns>Список использований.</returns>
        public override List<ComputerLesson> GetAll()
        {
            var list = new List<ComputerLesson>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT ComputerID, LessonID, IsUsed FROM ComputerLesson", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("ComputerID\tLessonID\tIsUsed");
                    while (reader.Read())
                    {
                        var computerLesson = new ComputerLesson
                        {
                            ComputerID = (Guid)reader["ComputerID"],
                            LessonID = (Guid)reader["LessonID"],
                            IsUsed = reader["IsUsed"] == DBNull.Value
                                ? null
                                : (bool?)reader["IsUsed"]
                        };
                        list.Add(computerLesson);

                        // Печать содержимого строки
                        Console.WriteLine($"{computerLesson.ComputerID}\t{computerLesson.LessonID}\t{(computerLesson.IsUsed.HasValue ? computerLesson.IsUsed.Value.ToString() : "")}");
                        Console.WriteLine();
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Обновляет использование компьютера на занятии.
        /// </summary>
        /// <param name="cl">Использование для обновления.</param>
        public override void Update(ComputerLesson cl)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления использования компьютера.
                using (var cmd = new SqlCommand(
                    "UPDATE ComputerLesson " +
                    "SET IsUsed = @IsUsed " +
                    "WHERE ComputerID = @ComputerID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", cl.ComputerID);
                    cmd.Parameters.AddWithValue("@LessonID", cl.LessonID);
                    cmd.Parameters.AddWithValue("@IsUsed", (object)cl.IsUsed ?? DBNull.Value);

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
        /// Удаляет использование компьютера по идентификаторам компьютера и занятия.
        /// </summary>
        /// <param name="computerId">Идентификатор компьютера.</param>
        /// <param name="lessonId">Идентификатор занятия.</param>
        public override void Delete(Guid computerId, Guid lessonId)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления использования компьютера.
                using (var cmd = new SqlCommand(
                    "DELETE FROM ComputerLesson " +
                    "WHERE ComputerID = @ComputerID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", computerId);
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