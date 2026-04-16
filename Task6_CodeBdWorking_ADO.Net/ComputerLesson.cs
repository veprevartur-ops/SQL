using System;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Представляет использование компьютера на занятии.
    /// </summary>
    public class ComputerLesson
    {
        /// <summary>Идентификатор компьютера.</summary>
        public Guid ComputerID { get; set; }
        /// <summary>Идентификатор занятия.</summary>
        public Guid LessonID { get; set; }
        /// <summary>Признак использования.</summary>
        public bool? IsUsed { get; set; }
    }
}