using System;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Использование компьютера на занятии.
    /// </summary>
    public class ComputerLesson : IUpdateble<ComputerLesson>
    {
        /// <summary>
        /// Идентификатор компьютера.
        /// </summary>
        public Guid ComputerID { get; set; }

        /// <summary>
        /// Идентификатор занятия.
        /// </summary>
        public Guid LessonID { get; set; }

        /// <summary>
        /// Признак использования.
        /// </summary>
        public bool? IsUsed { get; set; }

        /// <summary>
        /// Обновление признака использования.
        /// </summary>
        /// <param name="entity">Урок со старыми данными об использовании.</param>
        public void Update(ComputerLesson entity)
        {
            IsUsed = !(IsUsed ?? false);
        }
    }
}