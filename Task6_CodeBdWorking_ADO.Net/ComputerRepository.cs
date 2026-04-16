using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Репозиторий для работы с таблицей Computer.
    /// Предоставляет методы для создания, получения, обновления и удаления записей о компьютерах.
    /// </summary>
    public class ComputerRepository : RepositoryBase<Computer>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория компьютеров.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public ComputerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавляет новый компьютер в базу данных.
        /// </summary>
        /// <param name="entity">Экземпляр компьютера для добавления.</param>
        public override void Create(Computer entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO Computer " +
                    "(ComputerID, InventoryNumber, Brand, PurchaseDate, Price, ClassroomID, IsActive) " +
                    "VALUES (@ComputerID, @InventoryNumber, @Brand, @PurchaseDate, @Price, @ClassroomID, @IsActive)", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", entity.ComputerID);
                    cmd.Parameters.AddWithValue("@InventoryNumber", (object)entity.InventoryNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Brand", (object)entity.Brand ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PurchaseDate", (object)entity.PurchaseDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", (object)entity.Price ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ClassroomID", entity.ClassroomID);
                    cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает список всех компьютеров из базы данных.
        /// </summary>
        /// <returns>Список объектов <see cref="Computer"/>.</returns>
        public override List<Computer> GetAll()
        {
            var computers = new List<Computer>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT ComputerID, InventoryNumber, Brand, PurchaseDate, Price, ClassroomID, IsActive FROM Computer", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("ComputerID\tInventoryNumber\tBrand\tPurchaseDate\tPrice\tClassroomID\tIsActive");
                    while (reader.Read())
                    {
                        var computer = new Computer
                        {
                            ComputerID = (Guid)reader["ComputerID"],
                            InventoryNumber = reader["InventoryNumber"] == DBNull.Value
                                ? null
                                : (string)reader["InventoryNumber"],
                            Brand = reader["Brand"] == DBNull.Value
                                ? null
                                : (string)reader["Brand"],
                            PurchaseDate = reader["PurchaseDate"] == DBNull.Value
                                ? null
                                : (DateTime?)reader["PurchaseDate"],
                            Price = reader["Price"] == DBNull.Value
                                ? null
                                : (decimal?)reader["Price"],
                            ClassroomID = (Guid)reader["ClassroomID"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        computers.Add(computer);

                        // Печать содержимого строки
                        Console.WriteLine($"{computer.ComputerID}\t{computer.InventoryNumber}\t{computer.Brand}\t{(computer.PurchaseDate.HasValue ? computer.PurchaseDate.Value.ToShortDateString() : "")}\t{(computer.Price.HasValue ? computer.Price.Value.ToString("0.00") : "")}\t{computer.ClassroomID}\t{computer.IsActive}");
                        Console.WriteLine();
                    }
                }
            }
            return computers;
        }

        /// <summary>
        /// Обновляет данные о компьютере в базе данных.
        /// </summary>
        /// <param name="entity">Экземпляр компьютера с обновлёнными данными.</param>
        public override void Update(Computer entity)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "UPDATE Computer " +
                    "SET InventoryNumber = @InventoryNumber, Brand = @Brand, PurchaseDate = @PurchaseDate, Price = @Price, ClassroomID = @ClassroomID, IsActive = @IsActive " +
                    "WHERE ComputerID = @ComputerID", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", entity.ComputerID);
                    cmd.Parameters.AddWithValue("@InventoryNumber", (object)entity.InventoryNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Brand", (object)entity.Brand ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PurchaseDate", (object)entity.PurchaseDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", (object)entity.Price ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ClassroomID", entity.ClassroomID);
                    cmd.Parameters.AddWithValue("@IsActive", entity.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет компьютер по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор компьютера для удаления.</param>
        /// <param name="id2">Не используется. Для совместимости с базовым классом.</param>
        public override void Delete(Guid id, Guid id2 = default)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "DELETE FROM Computer WHERE ComputerID = @ComputerID", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", id);

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