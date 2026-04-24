using System;

namespace ConsoleApp3
{
    /// <summary>
    /// Класс, представляющий товар.
    /// </summary>
    public class Product
    {
        private string name;
        private string manufacturer;
        private double price;
        private DateTime expirationDate;
        private DateTime productionDate;

        /// <summary>
        /// Название товара.
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название товара не может быть пустым.");
                name = value;
            }
        }

        /// <summary>
        /// Производитель.
        /// </summary>
        public string Manufacturer
        {
            get => manufacturer;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Производитель не может быть пустым.");
                manufacturer = value;
            }
        }

        /// <summary>
        /// Цена.
        /// </summary>
        public double Price
        {
            get => price;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Цена должна быть положительным числом.");
                price = value;
            }
        }

        /// <summary>
        /// Дата окончания срока годности.
        /// </summary>
        public DateTime ExpirationDate
        {
            get => expirationDate;
            set
            {
                if (productionDate != default && value <= productionDate)
                    throw new ArgumentException("Дата окончания срока годности должна быть позже даты производства.");
                expirationDate = value;
            }
        }

        /// <summary>
        /// Дата производства.
        /// </summary>
        public DateTime ProductionDate
        {
            get => productionDate;
            set
            {
                if (expirationDate != default && value > expirationDate)
                    throw new ArgumentException("Дата производства не может быть позже даты окончания срока годности.");
                productionDate = value;
            }
        }

        public Product(string name, string manufacturer, double price, DateTime expirationDate, DateTime productionDate)
        {
            Name = name;
            Manufacturer = manufacturer;
            Price = price;
            ProductionDate = productionDate;
            ExpirationDate = expirationDate;
        }

        public override string ToString()
        {
            return $"Название: {Name}\nПроизводитель: {Manufacturer}\nЦена: {Price}\n" +
                   $"Произведено: {ProductionDate:yyyy-MM-dd}\nСрок до: {ExpirationDate:yyyy-MM-dd}";
        }
    }
}