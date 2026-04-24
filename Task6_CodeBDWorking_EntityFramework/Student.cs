using System;

namespace EntityFramework_Database
{
    /// <summary>
    /// Студент.
    /// </summary>
    public class Student : IUpdateble<Student>
    {
        /// <summary>
        /// Уникальный идентификатор студента.
        /// </summary>
        public Guid StudentID { get; set; }

        /// <summary>
        /// ФИО студента.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Средний балл.
        /// </summary>
        public decimal? GPA { get; set; }

        /// <summary>
        /// Идентификатор группы.
        /// </summary>
        public Guid GroupID { get; set; }

        /// <summary>
        /// Признак активности.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Обновление ФИО студента.
        /// </summary>
        /// <param name="entity">Студент со старым ФИО.</param>
        public void Update(Student entity)
        {
            FullName += " Updated";
        }
    }
}