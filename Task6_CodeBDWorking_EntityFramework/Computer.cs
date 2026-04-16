using System;

namespace EntityFramework_Database
{
    /// <summary>
    /// Represents a computer.
    /// </summary>
    public class Computer
    {
        /// <summary>
        /// Gets or sets the unique computer identifier.
        /// </summary>
        public Guid ComputerID { get; set; }

        /// <summary>
        /// Gets or sets the inventory number.
        /// </summary>
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Gets or sets the brand of the computer.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the purchase date.
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the classroom identifier.
        /// </summary>
        public Guid ClassroomID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the computer is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}