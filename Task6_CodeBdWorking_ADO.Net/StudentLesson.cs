using System;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Представляет посещение студентом занятия.
    /// </summary>
    public class StudentLesson
    {
        /// <summary>Идентификатор студента.</summary>
        public Guid StudentID { get; set; }
        /// <summary>Идентификатор занятия.</summary>
        public Guid LessonID { get; set; }
        /// <summary>Оценка.</summary>
        public decimal? Mark { get; set; }
        /// <summary>Признак присутствия.</summary>
        public bool? IsPresent { get; set; }
    }
}