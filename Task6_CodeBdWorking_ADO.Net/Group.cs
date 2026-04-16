using System;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Представляет учебную группу.
    /// </summary>
    public class Group
    {
        /// <summary>Уникальный идентификатор группы.</summary>
        public Guid GroupID { get; set; }
        /// <summary>Название группы.</summary>
        public string GroupName { get; set; }
        /// <summary>Признак активности группы.</summary>
        public bool IsActive { get; set; }
    }
}