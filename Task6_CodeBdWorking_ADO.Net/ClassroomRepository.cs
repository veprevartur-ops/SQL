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

        public ClassroomRepository(string connectionString) => _connectionString = connectionString;

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
        /// Получает список всех классов.
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
                    }
                }
            }
            return classrooms;
        }

        public override void Update(Classroom classroom)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "UPDATE Classroom " +
                    "SET RoomName = @RoomName, Capacity = @Capacity, IsActive = @IsActive " +
                    "WHERE ClassroomID = @ClassroomID", conn))
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

        public override void Delete(Guid classroomId, Guid id2 = default)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "DELETE FROM Classroom " +
                    "WHERE ClassroomID = @ClassroomID", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassroomID", classroomId);
                    int affected = cmd.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        throw new InvalidOperationException("Удаление не выполнено: запись с таким ключом не найдена.");
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает строковое представление объекта Classroom.
        /// </summary>
        /// <param name="classroom">Класс.</param>
        /// <returns>Строка с данными о классе.</returns>
        public override string ToString(Classroom classroom)
        {
            return $"{classroom.ClassroomID}\t{classroom.RoomName}\t{classroom.Capacity}\t{(classroom.IsActive ? "Активен" : "Неактивен")}";
        }

        /// <summary>
        /// Вывод в консоль всех классов из таблицы Classroom.
        /// </summary>
        public override void PrintAll()
        {
            var classrooms = GetAll();
            foreach (var classroom in classrooms)
            {
                Console.WriteLine(ToString(classroom));
            }
        }
    }
}