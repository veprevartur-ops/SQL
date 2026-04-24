using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Репозиторий для работы с таблицей использования компьютера на занятии.
    /// </summary>
    public class ComputerLessonRepository : RepositoryBase<ComputerLesson>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор репозитория использования компьютера на занятии.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public ComputerLessonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавление использования компьютера на занятии.
        /// </summary>
        /// <param name="cl">Экземпляр использования для добавления.</param>
        public override void Create(ComputerLesson cl)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.ComputerLessons.Add(cl);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Получение списка всех использований компьютеров на занятиях.
        /// </summary>
        /// <returns>Список использований.</returns>
        public override List<ComputerLesson> GetAll()
        {
            using (var db = new AppDbContext(_connectionString))
            {
                return new List<ComputerLesson>(db.ComputerLessons);
            }
        }

        /// <summary>
        /// Обновление использования компьютера на занятии.
        /// </summary>
        /// <param name="cl">Экземпляр использования для обновления.</param>
        public override void Update(ComputerLesson cl)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.ComputerLessons.Update(cl);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление использования компьютера на занятии по идентификаторам.
        /// </summary>
        /// <param name="computerId">Идентификатор компьютера.</param>
        /// <param name="lessonId">Идентификатор занятия.</param>
        public override void Delete(Guid computerId, Guid lessonId)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                var cl = db.ComputerLessons.Find(computerId, lessonId);
                if (cl != null)
                {
                    db.ComputerLessons.Remove(cl);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Строковое представление использования компьютера на занятии.
        /// </summary>
        /// <param name="cl">Использование компьютера на занятии.</param>
        /// <returns>Строка с описанием.</returns>
        public override string ToString(ComputerLesson cl)
        {
            return $"{cl.ComputerID}\t{cl.LessonID}\t{(cl.IsUsed.HasValue ? (cl.IsUsed.Value ? "Использовался" : "Не использовался") : "Нет данных")}";
        }

        /// <summary>
        /// Вывод всех использований компьютера на занятиях в консоль.
        /// </summary>
        public override void PrintAll()
        {
            var list = GetAll();
            Console.WriteLine("ComputerID\t\t\tLessonID\t\t\tСтатус использования");
            foreach (var cl in list)
            {
                Console.WriteLine(ToString(cl));
            }
        }
    }
}