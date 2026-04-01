using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Product product = ReadProductFromConsole();

                Console.WriteLine("\nТовар успешно создан!");
                Console.WriteLine(product);

                Console.WriteLine("\nХотите изменить свойства товара? (да/нет)");
                string answer = Console.ReadLine().ToLower();
                if (answer == "да")
                {
                    Console.WriteLine("Что изменить? (1 - название, 2 - производитель, 3 - цена, 4 - дата производства, 5 - срок годности)");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Write("Новое название: ");
                            product.SetName(Console.ReadLine());
                            break;
                        case "2":
                            Console.Write("Новый производитель: ");
                            product.SetManufacturer(Console.ReadLine());
                            break;
                        case "3":
                            Console.Write("Новая цена: ");
                            product.SetPrice(double.Parse(Console.ReadLine()));
                            break;
                        case "4":
                            Console.Write("Новая дата производства (гггг-мм-дд): ");
                            product.SetProductionDate(DateTime.Parse(Console.ReadLine()));
                            break;
                        case "5":
                            Console.Write("Новая дата окончания срока годности (гггг-мм-дд): ");
                            product.SetExpirationDate(DateTime.Parse(Console.ReadLine()));
                            break;
                        default:
                            Console.WriteLine("Неизвестный выбор.");
                            break;
                    }

                    Console.WriteLine("\nОбновлённая информация о товаре:");
                    Console.WriteLine(product);
                }

                // Ввод данных для товара со скидкой
                Console.WriteLine("\n=== Товар со скидкой ===");
                Product baseProduct = ReadProductFromConsole();
                Console.Write("Введите размер скидки (%): ");
                double discountPercent = double.Parse(Console.ReadLine());

                DiscountedProduct discountedProduct = new DiscountedProduct(
                    baseProduct.Name, baseProduct.Manufacturer, baseProduct.Price,
                    baseProduct.ExpirationDate, baseProduct.ProductionDate, discountPercent);

                Console.WriteLine("\nИнформация о товаре со скидкой:");
                Console.WriteLine(discountedProduct);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: введён неверный формат числа или даты.");
            }
        }

        static Product ReadProductFromConsole()
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();

            Console.Write("Введите производителя: ");
            string manufacturer = Console.ReadLine();

            Console.Write("Введите цену: ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Введите дату производства (гггг-мм-дд): ");
            DateTime productionDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Введите дату окончания срока годности (гггг-мм-дд): ");
            DateTime expirationDate = DateTime.Parse(Console.ReadLine());

            return new Product(name, manufacturer, price, expirationDate, productionDate);
        }

        internal class Product
        {
            string name;
            string manufacturer;
            double price;
            DateTime expirationDate;
            DateTime productionDate;

            public string Name => name;
            public string Manufacturer => manufacturer;
            public double Price => price;
            public DateTime ExpirationDate => expirationDate;
            public DateTime ProductionDate => productionDate;

            public Product(string name, string manufacturer, double price, DateTime expirationDate, DateTime productionDate)
            {
                SetName(name);
                SetManufacturer(manufacturer);
                SetPrice(price);
                SetProductionDate(productionDate);
                SetExpirationDate(expirationDate);
            }

            public void SetName(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Название товара не может быть пустым.", nameof(name));
                this.name = name;
            }

            public void SetManufacturer(string manufacturer)
            {
                if (string.IsNullOrWhiteSpace(manufacturer))
                    throw new ArgumentException("Производитель не может быть пустым.", nameof(manufacturer));
                this.manufacturer = manufacturer;
            }

            public void SetPrice(double price)
            {
                if (price <= 0)
                    throw new ArgumentException("Цена должна быть положительным числом.", nameof(price));
                this.price = price;
            }

            public void SetProductionDate(DateTime productionDate)
            {
                if (expirationDate != default && productionDate > expirationDate)
                    throw new ArgumentException("Дата производства не может быть позже даты окончания срока годности.");
                this.productionDate = productionDate;
            }

            public void SetExpirationDate(DateTime expirationDate)
            {
                if (productionDate != default && expirationDate <= productionDate)
                    throw new ArgumentException("Дата окончания срока годности должна быть позже даты производства.");
                this.expirationDate = expirationDate;
            }

            public override string ToString()
            {
                return $"Название: {Name}\n" +
                       $"Производитель: {Manufacturer}\n" +
                       $"Цена: {Price}\n" +
                       $"Дата производства: {ProductionDate:yyyy-MM-dd}\n" +
                       $"Срок годности до: {ExpirationDate:yyyy-MM-dd}";
            }
        }

        internal class DiscountedProduct : Product
        {
            private double discountPercent;
            public double DiscountPercent
            {
                get => discountPercent;
                set
                {
                    if (value < 0 || value > 100)
                        throw new ArgumentException("Размер скидки должен быть от 0 до 100 процентов.");
                    discountPercent = value;
                }
            }

            public double DiscountedPrice
            {
                get
                {
                    return Math.Round(Price * (1 - DiscountPercent / 100), 2);
                }
            }

            public DiscountedProduct(
                string name,
                string manufacturer,
                double price,
                DateTime expirationDate,
                DateTime productionDate,
                double discountPercent)
                : base(name, manufacturer, price, expirationDate, productionDate)
            {
                DiscountPercent = discountPercent;
            }

            public override string ToString()
            {
                return base.ToString() +
                       $"Скидка: {DiscountPercent}%\n" +
                       $"Акционная цена: {DiscountedPrice}\n";
            }
        }
    }
}


