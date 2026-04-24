using System;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Учебная группа.
    /// </summary>
    public class Group : IUpdateble<Group>
    {
        /// <summary>
        /// Уникальный идентификатор группы.
        /// </summary>
        public Guid GroupID { get; set; }

        /// <summary>
        /// Название группы.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Признак активности группы.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Обновление названия группы.
        /// </summary>
        /// <param name="entity">Группа с новым названием.</param>
        public void Update(Group entity)
        {
            GroupName += " Updated";
        }
    }
}