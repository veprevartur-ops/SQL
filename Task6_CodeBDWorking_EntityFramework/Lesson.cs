using System;

namespace EntityFramework_Database
{
    /// <summary>
    /// Занятие.
    /// </summary>
    public class Lesson : IUpdateble<Lesson>
    {
        /// <summary>
        /// Уникальный идентификатор занятия.
        /// </summary>
        public Guid LessonID { get; set; }

        /// <summary>
        /// Тема занятия.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Дата занятия.
        /// </summary>
        public DateTime LessonDate { get; set; }

        /// <summary>
        /// Идентификатор класса.
        /// </summary>
        public Guid ClassroomID { get; set; }

        /// <summary>
        /// Признак активности.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Обновление темы занятия.
        /// </summary>
        /// <param name="entity">Занятие со старой темой.</param>
        public void Update(Lesson entity)
        {
            Topic += " Updated";
            LessonDate = LessonDate.AddDays(1);
        }
    }
}