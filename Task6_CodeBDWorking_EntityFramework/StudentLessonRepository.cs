using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Репозиторий для работы с таблицей посещений занятий студентами.
    /// </summary>
    public class StudentLessonRepository : RepositoryBase<StudentLesson>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор репозитория посещений занятий студентами.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public StudentLessonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавление посещения студентом занятия.
        /// </summary>
        /// <param name="sl">Посещение для добавления.</param>
        public override void Create(StudentLesson sl)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.StudentLessons.Add(sl);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Получение списка всех посещений занятий студентами.
        /// </summary>
        /// <returns>Список посещений.</returns>
        public override List<StudentLesson> GetAll()
        {
            using (var db = new AppDbContext(_connectionString))
            {
                return new List<StudentLesson>(db.StudentLessons);
            }
        }

        /// <summary>
        /// Обновление информации о посещении.
        /// </summary>
        /// <param name="sl">Посещение для обновления.</param>
        public override void Update(StudentLesson sl)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.StudentLessons.Update(sl);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление посещения по идентификаторам студента и занятия.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор занятия.</param>
        public override void Delete(Guid studentId, Guid lessonId)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                var sl = db.StudentLessons.Find(studentId, lessonId);
                if (sl != null)
                {
                    db.StudentLessons.Remove(sl);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Строковое представление посещения занятия студентом.
        /// </summary>
        /// <param name="sl">Экземпляр посещения.</param>
        /// <returns>Строка с параметрами посещения.</returns>
        public override string ToString(StudentLesson sl)
        {
            string mark = sl.Mark.HasValue ? sl.Mark.Value.ToString("0.00") : "";
            string isPresent = sl.IsPresent.HasValue ? (sl.IsPresent.Value ? "Присутствовал" : "Отсутствовал") : "Нет данных";
            return $"{sl.StudentID}\t{sl.LessonID}\t{mark}\t{isPresent}";
        }

        /// <summary>
        /// Вывод всех посещений занятий в консоль.
        /// </summary>
        public override void PrintAll()
        {
            var list = GetAll();
            Console.WriteLine("StudentID\t\t\tLessonID\t\t\tОценка\tПосещение");
            foreach (var sl in list)
            {
                Console.WriteLine(ToString(sl));
            }
        }
    }
}