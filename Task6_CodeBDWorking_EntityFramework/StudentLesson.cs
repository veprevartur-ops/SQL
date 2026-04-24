using System;

namespace EntityFramework_Database
{
    /// <summary>
    /// Посещение студентом занятия.
    /// </summary>
    public class StudentLesson : IUpdateble<StudentLesson>
    {
        /// <summary>
        /// Идентификатор студента.
        /// </summary>
        public Guid StudentID { get; set; }

        /// <summary>
        /// Идентификатор занятия.
        /// </summary>
        public Guid LessonID { get; set; }

        /// <summary>
        /// Оценка.
        /// </summary>
        public decimal? Mark { get; set; }

        /// <summary>
        /// Признак присутствия.
        /// </summary>
        public bool? IsPresent { get; set; }

        /// <summary>
        /// Обновление оценки.
        /// </summary>
        /// <param name="entity">Посещение со старыми данными.</param>
        public void Update(StudentLesson entity)
        {
            // Нет имени — меняем Mark
            if (Mark.HasValue)
                Mark += 0.25m;
            else
                Mark = 5.0m;
        }
    }
}