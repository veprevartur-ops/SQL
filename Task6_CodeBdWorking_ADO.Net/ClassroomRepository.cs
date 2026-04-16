using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Репозиторий для работы с таблицей Classroom.
    /// </summary>
    public class ClassroomRepository : RepositoryBase<Classroom>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория классов.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public ClassroomRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет новый класс в базу данных.
        /// </summary>
        /// <param name="classroom">Класс для добавления.</param>
        public override void Create(Classroom classroom)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO Classroom " +
                    "(ClassroomID, RoomName, Capacity, IsActive) " +
                    "VALUES (@ClassroomID, @RoomName, @Capacity, @IsActive)", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassroomID", classroom.ClassroomID);
                    cmd.Parameters.AddWithValue("@RoomName", classroom.RoomName);
                    cmd.Parameters.AddWithValue("@Capacity", (object)classroom.Capacity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", classroom.IsActive);

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
        /// Получает список всех классов и печатает их содержимое.
        /// </summary>
        /// <returns>Список классов.</returns>
        public override List<Classroom> GetAll()
        {
            var classrooms = new List<Classroom>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT ClassroomID, RoomName, Capacity, IsActive FROM Classroom", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("ClassroomID\tRoomName\tCapacity\tIsActive");
                    while (reader.Read())
                    {
                        var classroom = new Classroom
                        {
                            ClassroomID = (Guid)reader["ClassroomID"],
                            RoomName = (string)reader["RoomName"],
                            Capacity = reader["Capacity"] == DBNull.Value
                                ? null
                                : (int?)reader["Capacity"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        classrooms.Add(classroom);

                        // Печать содержимого строки
                        Console.WriteLine($"{classroom.ClassroomID}\t{classroom.RoomName}\t{classroom.Capacity}\t{classroom.IsActive}");
                        Console.WriteLine();
                    }
                }
            }
            return classrooms;
        }

        /// <summary>
        /// Обновляет данные класса.
        /// </summary>
        /// <param name="classroom">Класс для обновления.</param>
        public override void Update(Classroom classroom)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления класса.
                using (var cmd = new SqlCommand(
                    "UPDATE Classroom " +
                    "SET RoomName = @RoomName, Capacity = @Capacity, IsActive = @IsActive " +
                    "WHERE ClassroomID = @ClassroomID", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassroomID", classroom.ClassroomID);
                    cmd.Parameters.AddWithValue("@RoomName", classroom.RoomName);
                    cmd.Parameters.AddWithValue("@Capacity", (object)classroom.Capacity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", classroom.IsActive);

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
        /// Удаляет класс по идентификатору.
        /// </summary>
        /// <param name="classroomId">Идентификатор класса.</param>
        public override void Delete(Guid classroomId, Guid id2 = default)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления класса.
                using (var cmd = new SqlCommand(
                    "DELETE FROM Classroom " +
                    "WHERE ClassroomID = @ClassroomID", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassroomID", classroomId);

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