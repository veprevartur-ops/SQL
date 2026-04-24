using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Репозиторий для работы с таблицей компьютеров.
    /// </summary>
    public class ComputerRepository : RepositoryBase<Computer>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор репозитория компьютеров.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public ComputerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавление нового компьютера.
        /// </summary>
        /// <param name="computer">Компьютер для добавления.</param>
        public override void Create(Computer computer)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.Computers.Add(computer);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Получение списка всех компьютеров.
        /// </summary>
        /// <returns>Список компьютеров.</returns>
        public override List<Computer> GetAll()
        {
            using (var db = new AppDbContext(_connectionString))
            {
                return new List<Computer>(db.Computers);
            }
        }

        /// <summary>
        /// Обновление существующего компьютера.
        /// </summary>
        /// <param name="computer">Компьютер для обновления.</param>
        public override void Update(Computer computer)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.Computers.Update(computer);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление компьютера по идентификатору.
        /// </summary>
        /// <param name="computerId">Идентификатор компьютера.</param>
        public override void Delete(Guid computerId, Guid id2 = default)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                var computer = db.Computers.Find(computerId);
                if (computer != null)
                {
                    db.Computers.Remove(computer);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Строковое представление компьютера.
        /// </summary>
        /// <param name="computer">Компьютер.</param>
        /// <returns>Строка с параметрами компьютера.</returns>
        public override string ToString(Computer computer)
        {
            return $"{computer.ComputerID}\t{computer.InventoryNumber}\t{computer.Brand}\t" +
                   $"{(computer.PurchaseDate.HasValue ? computer.PurchaseDate.Value.ToShortDateString() : "")}\t" +
                   $"{(computer.Price.HasValue ? computer.Price.Value.ToString("0.00") : "")}\t" +
                   $"{computer.ClassroomID}\t{(computer.IsActive ? "Активен" : "Неактивен")}";
        }

        /// <summary>
        /// Вывод в консоль всех компьютеров.
        /// </summary>
        public override void PrintAll()
        {
            var list = GetAll();
            Console.WriteLine("ComputerID\tInventoryNumber\tBrand\tPurchaseDate\tPrice\tClassroomID\tStatus");
            foreach (var computer in list)
            {
                Console.WriteLine(ToString(computer));
            }
        }
    }
}