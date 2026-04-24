using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Репозиторий для работы с таблицей кабинетов.
    /// </summary>
    public class ClassroomRepository : RepositoryBase<Classroom>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор репозитория кабинетов.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public ClassroomRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавление нового кабинета.
        /// </summary>
        /// <param name="classroom">Кабинет для добавления.</param>
        public override void Create(Classroom classroom)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.Classrooms.Add(classroom);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Получение списка всех кабинетов.
        /// </summary>
        /// <returns>Список кабинетов.</returns>
        public override List<Classroom> GetAll()
        {
            using (var db = new AppDbContext(_connectionString))
            {
                return new List<Classroom>(db.Classrooms);
            }
        }

        /// <summary>
        /// Обновление существующего кабинета.
        /// </summary>
        /// <param name="classroom">Кабинет для обновления.</param>
        public override void Update(Classroom classroom)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.Classrooms.Update(classroom);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление кабинета по идентификатору.
        /// </summary>
        /// <param name="classroomId">Идентификатор кабинета.</param>
        public override void Delete(Guid classroomId, Guid id2 = default)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                var classroom = db.Classrooms.Find(classroomId);
                if (classroom != null)
                {
                    db.Classrooms.Remove(classroom);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Получение строкового представления кабинета.
        /// </summary>
        /// <param name="classroom">Кабинет.</param>
        /// <returns>Строковое представление кабинета.</returns>
        public override string ToString(Classroom classroom)
        {
            return $"{classroom.ClassroomID}\t{classroom.RoomName}\t{classroom.Capacity}\t{(classroom.IsActive ? "Активен" : "Неактивен")}";
        }

        /// <summary>
        /// Вывод всех кабинетов в консоль.
        /// </summary>
        public override void PrintAll()
        {
            var list = GetAll();
            Console.WriteLine("ClassroomID\tRoomName\tCapacity\tStatus");
            foreach (var classroom in list)
            {
                Console.WriteLine(ToString(classroom));
            }
        }
    }
}