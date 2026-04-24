using System;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Компьютер.
    /// </summary>
    public class Computer : IUpdateble<Computer>
    {
        /// <summary>
        /// Уникальный идентификатор компьютера.
        /// </summary>
        public Guid ComputerID { get; set; }

        /// <summary>
        /// Инвентарный номер.
        /// </summary>
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Бренд компьютера.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Дата покупки.
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Идентификатор класса.
        /// </summary>
        public Guid ClassroomID { get; set; }

        /// <summary>
        /// Признак активности.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Обновление бренда компьютера.
        /// </summary>
        /// <param name="entity">Компьютер со старым брендом.</param>
        public void Update(Computer entity)
        {
            Brand += " Updated";
        }
    }
}