using System;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Представляет компьютер.
    /// </summary>
    public class Computer
    {
        /// <summary>Уникальный идентификатор компьютера.</summary>
        public Guid ComputerID { get; set; }
        /// <summary>Инвентарный номер.</summary>
        public string InventoryNumber { get; set; }
        /// <summary>Бренд компьютера.</summary>
        public string Brand { get; set; }
        /// <summary>Дата покупки.</summary>
        public DateTime? PurchaseDate { get; set; }
        /// <summary>Цена.</summary>
        public decimal? Price { get; set; }
        /// <summary>Идентификатор класса.</summary>
        public Guid ClassroomID { get; set; }
        /// <summary>Признак активности.</summary>
        public bool IsActive { get; set; }
    }
}