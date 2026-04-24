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

        public ComputerLessonRepository(string connectionString)
            => _connectionString = connectionString;

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
                    }
                }
            }
            return list;
        }

        public override void Update(ComputerLesson cl)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "UPDATE ComputerLesson " +
                    "SET IsUsed = @IsUsed " +
                    "WHERE ComputerID = @ComputerID AND LessonID = @LessonID", conn))
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

        public override void Delete(Guid computerId, Guid lessonId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "DELETE FROM ComputerLesson " +
                    "WHERE ComputerID = @ComputerID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", computerId);
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
        /// Возврат строкового представления использования компьютера на занятии.
        /// </summary>
        /// <param name="cl">Экземпляр использования компьютера.</param>
        /// <returns>Строковое представление.</returns>
        public override string ToString(ComputerLesson cl)
        {
            return $"{cl.ComputerID}\t{cl.LessonID}\t{(cl.IsUsed.HasValue ? cl.IsUsed.Value.ToString() : "")}";
        }

        /// <summary>
        /// Вывод в консоль всех занятий с компьютерами из таблицы ComputerLesson.
        /// </summary>
        public override void PrintAll()
        {
            var all = GetAll(); 
            Console.WriteLine("ComputerID\t\t\tLessonID\t\t\tIsUsed");
            foreach (var cl in all)
            {
                Console.WriteLine(ToString(cl));
            }
        }
    }
}