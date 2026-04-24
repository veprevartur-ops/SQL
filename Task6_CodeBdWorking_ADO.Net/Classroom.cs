using System;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Компьютерный класс.
    /// </summary>
    public class Classroom : IUpdateble<Classroom>
    {
        /// <summary>
        /// Уникальный идентификатор класса.
        /// </summary>
        public Guid ClassroomID { get; set; }

        /// <summary>
        /// Название класса.
        /// </summary>
        public string RoomName { get; set; }

        /// <summary>
        /// Вместимость класса.
        /// </summary>
        public int? Capacity { get; set; }

        /// <summary>
        /// Признак активности.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Обновление названия класса.
        /// </summary>
        /// <param name="entity">Класс со старым названием.</param>
        public void Update(Classroom entity)
        {
            RoomName += " Updated";
        }
    }
}