using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Репозиторий для работы с таблицей занятий.
    /// </summary>
    public class LessonRepository : RepositoryBase<Lesson>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор репозитория занятий.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public LessonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавление нового занятия.
        /// </summary>
        /// <param name="lesson">Занятие для добавления.</param>
        public override void Create(Lesson lesson)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.Lessons.Add(lesson);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Получение списка всех занятий.
        /// </summary>
        /// <returns>Список занятий.</returns>
        public override List<Lesson> GetAll()
        {
            using (var db = new AppDbContext(_connectionString))
            {
                return new List<Lesson>(db.Lessons);
            }
        }

        /// <summary>
        /// Обновление существующего занятия.
        /// </summary>
        /// <param name="lesson">Занятие для обновления.</param>
        public override void Update(Lesson lesson)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.Lessons.Update(lesson);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление занятия по идентификатору.
        /// </summary>
        /// <param name="lessonId">Идентификатор занятия.</param>
        public override void Delete(Guid lessonId, Guid id2 = default)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                var lesson = db.Lessons.Find(lessonId);
                if (lesson != null)
                {
                    db.Lessons.Remove(lesson);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Строковое представление занятия.
        /// </summary>
        /// <param name="lesson">Занятие.</param>
        /// <returns>Строка с информацией о занятии.</returns>
        public override string ToString(Lesson lesson)
        {
            return $"{lesson.LessonID}\t{lesson.Topic}\t{lesson.LessonDate:yyyy-MM-dd HH:mm}\t{lesson.ClassroomID}\t{(lesson.IsActive ? "Активно" : "Неактивно")}";
        }

        /// <summary>
        /// Вывод всех занятий в консоль.
        /// </summary>
        public override void PrintAll()
        {
            var list = GetAll();
            Console.WriteLine("LessonID\t\t\tTopic\t\tДата и время\t\tClassroomID\t\t\t\tСтатус");
            foreach (var lesson in list)
            {
                Console.WriteLine(ToString(lesson));
            }
        }
    }
}