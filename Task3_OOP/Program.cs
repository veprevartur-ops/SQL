using System;

namespace ConsoleApp3
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Product product = ReadProductFromConsole();

                Console.WriteLine("\nТовар успешно создан!");
                Console.WriteLine(product);

                Console.WriteLine("\nХотите изменить свойства товара? (да/нет)");
                if (Console.ReadLine().Trim().ToLower() == "да")
                {
                    ChangeProductProperty(product);
                    Console.WriteLine("\nОбновлённая информация о товаре:");
                    Console.WriteLine(product);
                }

                Console.WriteLine("\n=== Товар со скидкой ===");
                Product baseProduct = ReadProductFromConsole();
                double discountPercent = ReadValue("Введите размер скидки (%): ", double.Parse);

                DiscountedProduct discountedProduct = new DiscountedProduct(
                    baseProduct.Name, baseProduct.Manufacturer, baseProduct.Price,
                    baseProduct.ExpirationDate, baseProduct.ProductionDate, discountPercent);

                Console.WriteLine("\nИнформация о товаре со скидкой:");
                Console.WriteLine(discountedProduct);
            }
            catch (ArgumentException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            catch (FormatException) { Console.WriteLine("Ошибка формата."); }
        }

        /// <summary>
        /// Универсальный метод для безопасного ввода значения с консоли.
        /// Вся валидация происходит в сеттере свойства.
        /// </summary>
        static T ReadValue<T>(string prompt, Func<string, T> parser)
        {
            while (true)
            {
                Console.Write(prompt);
                try
                {
                    T value = parser(Console.ReadLine());
                    return value;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                catch
                {
                    Console.WriteLine("Ошибка формата.");
                }
            }
        }

        /// <summary>
        /// Ввод всех свойств товара с консоли.
        /// </summary>
        static Product ReadProductFromConsole()
        {
            string name = ReadValue("Введите название: ", s => s);
            string manufacturer = ReadValue("Введите производителя: ", s => s);
            double price = ReadValue("Введите цену: ", double.Parse);
            DateTime prodDate = ReadValue("Введите дату производства (гггг-мм-дд): ", DateTime.Parse);
            DateTime expDate = ReadValue("Введите дату окончания срока годности (гггг-мм-дд): ", DateTime.Parse);

            return new Product(name, manufacturer, price, expDate, prodDate);
        }

        /// <summary>
        /// Меню изменения одного из свойств товара.
        /// </summary>
        static void ChangeProductProperty(Product product)
        {
            Console.WriteLine("Что изменить? (1 - название, 2 - производитель, 3 - цена, 4 - производство, 5 - срок)");
            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        product.Name = ReadValue("Новое название: ", s => s);
                        break;
                    case "2":
                        product.Manufacturer = ReadValue("Новый производитель: ", s => s);
                        break;
                    case "3":
                        product.Price = ReadValue("Цена: ", double.Parse);
                        break;
                    case "4":
                        product.ProductionDate = ReadValue("Дата производства (гггг-мм-дд): ", DateTime.Parse);
                        break;
                    case "5":
                        product.ExpirationDate = ReadValue("Срок годности (гггг-мм-дд): ", DateTime.Parse);
                        break;
                    default:
                        Console.WriteLine("Неизвестно.");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}