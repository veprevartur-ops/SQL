using System;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Представляет компьютерный класс.
    /// </summary>
    public class Classroom
    {
        /// <summary>Уникальный идентификатор класса.</summary>
        public Guid ClassroomID { get; set; }
        /// <summary>Название класса.</summary>
        public string RoomName { get; set; }
        /// <summary>Вместимость класса.</summary>
        public int? Capacity { get; set; }
        /// <summary>Признак активности.</summary>
        public bool IsActive { get; set; }
    }
}