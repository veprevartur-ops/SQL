using System;

namespace ConsoleApp3
{
    // Наследуется напрямую от Product
    internal class DiscountedProduct : Product
    {
        private double discountPercent;

        public double DiscountPercent
        {
            get => discountPercent;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Скидка 0-100%.");
                discountPercent = value;
            }
        }

        public double DiscountedPrice => Math.Round(Price * (1 - DiscountPercent / 100), 2);

        public DiscountedProduct(string name, string manufacturer, double price, DateTime exp, DateTime prod, double discount)
            : base(name, manufacturer, price, exp, prod)
        {
            DiscountPercent = discount;
        }

        public override string ToString()
        {
            return base.ToString() + $"\nСкидка: {DiscountPercent}%\nАкционная цена: {DiscountedPrice}";
        }
    }
}