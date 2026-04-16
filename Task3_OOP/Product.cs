using System;

namespace ConsoleApp3
{
    // Класс теперь сам по себе, не внутри Program
    internal class Product
    {
        internal string name;
        internal string manufacturer;
        internal double price;
        internal DateTime expirationDate;
        internal DateTime productionDate;

        public string Name => name;
        public string Manufacturer => manufacturer;
        public double Price => price;
        public DateTime ExpirationDate => expirationDate;
        public DateTime ProductionDate => productionDate;

        public Product(string name, string manufacturer, double price, DateTime expirationDate, DateTime productionDate)
        {
            SetValue(name, v => this.name = v, v => !string.IsNullOrWhiteSpace(v));
            SetValue(manufacturer, v => this.manufacturer = v, v => !string.IsNullOrWhiteSpace(v));
            SetValue(price, v => this.price = v, v => v > 0);
            SetValue(productionDate, v => this.productionDate = v, v => expirationDate == default || v <= expirationDate);
            SetValue(expirationDate, v => this.expirationDate = v, v => productionDate == default || v > productionDate);
        }

        public void SetValue<T>(T value, Action<T> assign, Func<T, bool> validate = null)
        {
            if (validate == null || validate(value)) assign(value);
            else throw new ArgumentException("Некорректное значение.");
        }

        public override string ToString()
        {
            return $"Название: {Name}\nПроизводитель: {Manufacturer}\nЦена: {Price}\n" +
                   $"Произведено: {ProductionDate:yyyy-MM-dd}\nСрок до: {ExpirationDate:yyyy-MM-dd}";
        }
    }
}