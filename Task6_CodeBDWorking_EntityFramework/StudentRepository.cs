using System;
using System.Collections.Generic;

namespace EntityFramework_Database
{
    /// <summary>
    /// Репозиторий для работы с таблицей студентов.
    /// </summary>
    public class StudentRepository : RepositoryBase<Student>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Конструктор репозитория студентов.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавление нового студента.
        /// </summary>
        /// <param name="student">Студент для добавления.</param>
        public override void Create(Student student)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.Students.Add(student);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Получение списка всех студентов.
        /// </summary>
        /// <returns>Список студентов.</returns>
        public override List<Student> GetAll()
        {
            using (var db = new AppDbContext(_connectionString))
            {
                return new List<Student>(db.Students);
            }
        }

        /// <summary>
        /// Обновление существующего студента.
        /// </summary>
        /// <param name="student">Студент для обновления.</param>
        public override void Update(Student student)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                db.Students.Update(student);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление студента по идентификатору.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        public override void Delete(Guid studentId, Guid id2=default)
        {
            using (var db = new AppDbContext(_connectionString))
            {
                var student = db.Students.Find(studentId);
                if (student != null)
                {
                    db.Students.Remove(student);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Строковое представление студента.
        /// </summary>
        /// <param name="student">Студент.</param>
        /// <returns>Строка с параметрами студента.</returns>
        public override string ToString(Student student)
        {
            string birthDate = student.BirthDate.HasValue ? student.BirthDate.Value.ToShortDateString() : "";
            string gpa = student.GPA.HasValue ? student.GPA.Value.ToString("0.00") : "";
            return $"{student.StudentID}\t{student.FullName}\t{birthDate}\t{gpa}\t{student.GroupID}\t{(student.IsActive ? "Активен" : "Неактивен")}";
        }

        /// <summary>
        /// Вывод всех студентов в консоль.
        /// </summary>
        public override void PrintAll()
        {
            var list = GetAll();
            Console.WriteLine("StudentID\tFullName\tBirthDate\tGPA\tGroupID\t\t\t\tСтатус");
            foreach (var student in list)
            {
                Console.WriteLine(ToString(student));
            }
        }
    }
}