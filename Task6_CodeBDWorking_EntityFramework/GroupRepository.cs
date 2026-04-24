using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Репозиторий для работы с таблицей групп.
    /// </summary>
    public class GroupRepository : RepositoryBase<Group>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор репозитория групп.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public GroupRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавление новой группы.
        /// </summary>
        /// <param name="group">Группа для добавления.</param>
        public override void Create(Group group)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.Groups.Add(group);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Получение списка всех групп.
        /// </summary>
        /// <returns>Список групп.</returns>
        public override List<Group> GetAll()
        {
            using (var db = new AppDbContext(_connectionString))
            {
                return new List<Group>(db.Groups);
            }
        }

        /// <summary>
        /// Обновление существующей группы.
        /// </summary>
        /// <param name="group">Группа для обновления.</param>
        public override void Update(Group group)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.Groups.Update(group);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление группы по идентификатору.
        /// </summary>
        /// <param name="groupId">Идентификатор группы.</param>
        public override void Delete(Guid groupId, Guid id2 = default)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                var group = db.Groups.Find(groupId);
                if (group != null)
                {
                    db.Groups.Remove(group);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Строковое представление группы.
        /// </summary>
        /// <param name="group">Группа.</param>
        /// <returns>Строка с информацией о группе.</returns>
        public override string ToString(Group group)
        {
            return $"{group.GroupID}\t{group.GroupName}\t{(group.IsActive ? "Активна" : "Неактивна")}";
        }

        /// <summary>
        /// Вывод всех групп в консоль.
        /// </summary>
        public override void PrintAll()
        {
            var list = GetAll();
            Console.WriteLine("GroupID\t\t\t\t\tGroupName\tStatus");
            foreach (var group in list)
            {
                Console.WriteLine(ToString(group));
            }
        }
    }
}