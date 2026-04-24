using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Репозиторий для работы с таблицей Computer.
    /// </summary>
    public class ComputerRepository : RepositoryBase<Computer>
    {
        private readonly string _connectionString;

        public ComputerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

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
                    while (reader.Read())
                    {
                        var computer = new Computer
                        {
                            ComputerID = (Guid)reader["ComputerID"],
                            InventoryNumber = reader["InventoryNumber"] == DBNull.Value ? null : (string)reader["InventoryNumber"],
                            Brand = reader["Brand"] == DBNull.Value ? null : (string)reader["Brand"],
                            PurchaseDate = reader["PurchaseDate"] == DBNull.Value ? null : (DateTime?)reader["PurchaseDate"],
                            Price = reader["Price"] == DBNull.Value ? null : (decimal?)reader["Price"],
                            ClassroomID = (Guid)reader["ClassroomID"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        computers.Add(computer);
                    }
                }
            }
            return computers;
        }

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

        /// <summary>
        /// Возвращает строковое представление компьютера.
        /// </summary>
        /// <param name="computer">Экземпляр компьютера.</param>
        /// <returns>Строка для вывода.</returns>
        public override string ToString(Computer computer)
        {
            return $"{computer.ComputerID}\t{computer.InventoryNumber}\t{computer.Brand}\t" +
                   $"{(computer.PurchaseDate.HasValue ? computer.PurchaseDate.Value.ToShortDateString() : "")}\t" +
                   $"{(computer.Price.HasValue ? computer.Price.Value.ToString("0.00") : "")}\t" +
                   $"{computer.ClassroomID}\t{(computer.IsActive ? "Активен" : "Неактивен")}";
        }

        /// <summary>
        /// Вывод в консоль всех компьютеров из таблицы Computer.
        /// </summary>
        public override void PrintAll()
        {
            var all = GetAll();
            Console.WriteLine("ComputerID\tInventoryNumber\tBrand\tPurchaseDate\tPrice\tClassroomID\tIsActive");
            foreach (var computer in all)
            {
                Console.WriteLine(ToString(computer));
            }
        }
    }
}