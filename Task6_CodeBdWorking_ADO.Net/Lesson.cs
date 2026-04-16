using System;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Представляет занятие.
    /// </summary>
    public class Lesson
    {
        /// <summary>Уникальный идентификатор занятия.</summary>
        public Guid LessonID { get; set; }
        /// <summary>Тема занятия.</summary>
        public string Topic { get; set; }
        /// <summary>Дата занятия.</summary>
        public DateTime LessonDate { get; set; }
        /// <summary>Идентификатор класса.</summary>
        public Guid ClassroomID { get; set; }
        /// <summary>Признак активности.</summary>
        public bool IsActive { get; set; }
    }
}