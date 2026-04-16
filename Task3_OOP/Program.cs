using System;

namespace ConsoleApp3
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Теперь эти методы вызываются внутри Program
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
                double discountPercent = ReadValue("Введите размер скидки (%): ", double.Parse, v => v >= 0 && v <= 100);

                DiscountedProduct discountedProduct = new DiscountedProduct(
                    baseProduct.Name, baseProduct.Manufacturer, baseProduct.Price,
                    baseProduct.ExpirationDate, baseProduct.ProductionDate, discountPercent);

                Console.WriteLine("\nИнформация о товаре со скидкой:");
                Console.WriteLine(discountedProduct);
            }
            catch (ArgumentException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            catch (FormatException) { Console.WriteLine("Ошибка формата."); }
        }

        static T ReadValue<T>(string prompt, Func<string, T> parser, Func<T, bool> validate = null)
        {
            while (true)
            {
                Console.Write(prompt);
                try
                {
                    T value = parser(Console.ReadLine());
                    if (validate == null || validate(value)) return value;
                    Console.WriteLine("Некорректное значение.");
                }
                catch { Console.WriteLine("Ошибка формата."); }
            }
        }

        // МЕТОДЫ ПЕРЕНЕСЕНЫ СЮДА ИЗ PRODUCT.CS
        static Product ReadProductFromConsole()
        {
            string name = ReadValue("Введите название: ", s => s, s => !string.IsNullOrWhiteSpace(s));
            double price = ReadValue("Введите цену: ", double.Parse, v => v > 0);
            string manufacturer = ReadValue("Введите производителя: ", s => s, s => !string.IsNullOrWhiteSpace(s));
            DateTime prodDate = ReadValue("Введите дату производства (гггг-мм-дд): ", DateTime.Parse);
            DateTime expDate = ReadValue("Введите дату окончания срока годности (гггг-мм-дд): ", DateTime.Parse);

            return new Product(name, manufacturer, price, expDate, prodDate);
        }

        static void ChangeProductProperty(Product product)
        {
            Console.WriteLine("Что изменить? (1-название, 2-производитель, 3-цена, 4-производство, 5-срок)");
            switch (Console.ReadLine())
            {
                case "1": product.SetValue(ReadValue("Новое: ", s => s, s => !string.IsNullOrWhiteSpace(s)), v => product.name = v); break;
                case "2": product.SetValue(ReadValue("Новый: ", s => s, s => !string.IsNullOrWhiteSpace(s)), v => product.manufacturer = v); break;
                case "3": product.SetValue(ReadValue("Цена: ", double.Parse, v => v > 0), v => product.price = v); break;
                case "4": product.SetValue(ReadValue("Дата (гггг-мм-дд): ", DateTime.Parse), v => product.productionDate = v, v => product.expirationDate == default || v <= product.expirationDate); break;
                case "5": product.SetValue(ReadValue("Срок (гггг-мм-дд): ", DateTime.Parse), v => product.expirationDate = v, v => product.productionDate == default || v > product.productionDate); break;
                default: Console.WriteLine("Неизвестно."); break;
            }
        }
    }
}