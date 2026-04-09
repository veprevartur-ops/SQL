using System;

namespace ConsoleApp3
{
    internal class Program
    {
        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        /// <param name="args">Аргументы командной строки</param>
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

                // Ввод данных для товара со скидкой
                Console.WriteLine("\n=== Товар со скидкой ===");
                Product baseProduct = ReadProductFromConsole();
                double discountPercent = ReadValue(
                    "Введите размер скидки (%): ",
                    double.Parse,
                    v => v >= 0 && v <= 100,
                    "Скидка должна быть числом от 0 до 100."
                );

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

        /// <summary>
        /// Универсальный метод для безопасного ввода значения с консоли с валидацией.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения</typeparam>
        /// <param name="prompt">Сообщение для пользователя</param>
        /// <param name="parser">Функция преобразования строки в нужный тип</param>
        /// <param name="validate">Функция валидации значения</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        /// <returns>Корректное значение типа T</returns>
        static T ReadValue<T>(string prompt, Func<string, T> parser, Func<T, bool> validate = null, string errorMessage = "Ошибка ввода.")
        {
            while (true)
            {
                Console.Write(prompt);
                try
                {
                    T value = parser(Console.ReadLine());
                    if (validate == null || validate(value))
                        return value;
                }
                catch { }
                Console.WriteLine(errorMessage);
            }
        }

        /// <summary>
        /// Ввод всех свойств товара с консоли.
        /// </summary>
        /// <returns>Созданный объект Product</returns>
        static Product ReadProductFromConsole()
        {
            string name = ReadValue("Введите название товара: ", s => s, s => !string.IsNullOrWhiteSpace(s), "Строка не должна быть пустой.");
            string manufacturer = ReadValue("Введите производителя: ", s => s, s => !string.IsNullOrWhiteSpace(s), "Строка не должна быть пустой.");
            double price = ReadValue("Введите цену: ", double.Parse, v => v > 0, "Цена должна быть положительным числом.");
            DateTime productionDate = ReadValue("Введите дату производства (гггг-мм-дд): ", DateTime.Parse, null, "Ошибка: неверный формат даты.");
            DateTime expirationDate = ReadValue("Введите дату окончания срока годности (гггг-мм-дд): ", DateTime.Parse, null, "Ошибка: неверный формат даты.");

            return new Product(name, manufacturer, price, expirationDate, productionDate);
        }

        /// <summary>
        /// Меню изменения одного из свойств товара.
        /// </summary>
        /// <param name="product">Объект товара для изменения</param>
        static void ChangeProductProperty(Product product)
        {
            Console.WriteLine("Что изменить? (1 - название, 2 - производитель, 3 - цена, 4 - дата производства, 5 - срок годности)");
            switch (Console.ReadLine())
            {
                case "1":
                    product.SetValue(
                        ReadValue("Новое название: ", s => s, s => !string.IsNullOrWhiteSpace(s), "Строка не должна быть пустой."),
                        v => product.name = v,
                        v => !string.IsNullOrWhiteSpace(v),
                        "Название товара не может быть пустым."
                    );
                    break;
                case "2":
                    product.SetValue(
                        ReadValue("Новый производитель: ", s => s, s => !string.IsNullOrWhiteSpace(s), "Строка не должна быть пустой."),
                        v => product.manufacturer = v,
                        v => !string.IsNullOrWhiteSpace(v),
                        "Производитель не может быть пустым."
                    );
                    break;
                case "3":
                    product.SetValue(
                        ReadValue("Новая цена: ", double.Parse, v => v > 0, "Цена должна быть положительным числом."),
                        v => product.price = v,
                        v => v > 0,
                        "Цена должна быть положительным числом."
                    );
                    break;
                case "4":
                    product.SetValue(
                        ReadValue("Новая дата производства (гггг-мм-дд): ", DateTime.Parse, null, "Ошибка: неверный формат даты."),
                        v => product.productionDate = v,
                        v => product.expirationDate == default || v <= product.expirationDate,
                        "Дата производства не может быть позже даты окончания срока годности."
                    );
                    break;
                case "5":
                    product.SetValue(
                        ReadValue("Новая дата окончания срока годности (гггг-мм-дд): ", DateTime.Parse, null, "Ошибка: неверный формат даты."),
                        v => product.expirationDate = v,
                        v => product.productionDate == default || v > product.productionDate,
                        "Дата окончания срока годности должна быть позже даты производства."
                    );
                    break;
                default:
                    Console.WriteLine("Неизвестный выбор.");
                    break;
            }
        }

        /// <summary>
        /// Класс товара.
        /// </summary>
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

            /// <summary>
            /// Конструктор с валидацией всех полей через универсальный SetValue.
            /// </summary>
            /// <param name="name">Название товара</param>
            /// <param name="manufacturer">Производитель</param>
            /// <param name="price">Цена</param>
            /// <param name="expirationDate">Дата окончания срока годности</param>
            /// <param name="productionDate">Дата производства</param>
            public Product(string name, string manufacturer, double price, DateTime expirationDate, DateTime productionDate)
            {
                SetValue(name, v => this.name = v, v => !string.IsNullOrWhiteSpace(v), "Название товара не может быть пустым.");
                SetValue(manufacturer, v => this.manufacturer = v, v => !string.IsNullOrWhiteSpace(v), "Производитель не может быть пустым.");
                SetValue(price, v => this.price = v, v => v > 0, "Цена должна быть положительным числом.");
                SetValue(productionDate, v => this.productionDate = v, v => expirationDate == default || v <= expirationDate, "Дата производства не может быть позже даты окончания срока годности.");
                SetValue(expirationDate, v => this.expirationDate = v, v => productionDate == default || v > productionDate, "Дата окончания срока годности должна быть позже даты производства.");
            }

            /// <summary>
            /// Универсальный сеттер с валидацией.
            /// </summary>
            /// <typeparam name="T">Тип значения</typeparam>
            /// <param name="value">Значение</param>
            /// <param name="assign">Действие присваивания</param>
            /// <param name="validate">Функция валидации</param>
            /// <param name="errorMessage">Сообщение об ошибке</param>
            public void SetValue<T>(T value, Action<T> assign, Func<T, bool> validate, string errorMessage)
            {
                if (validate == null || validate(value))
                    assign(value);
                else
                    throw new ArgumentException(errorMessage);
            }

            /// <summary>
            /// Возвращает строковое представление товара.
            /// </summary>
            /// <returns>Строка с информацией о товаре</returns>
            public override string ToString()
            {
                return $"Название: {Name}\n" +
                       $"Производитель: {Manufacturer}\n" +
                       $"Цена: {Price}\n" +
                       $"Дата производства: {ProductionDate:yyyy-MM-dd}\n" +
                       $"Срок годности до: {ExpirationDate:yyyy-MM-dd}";
            }
        }

        /// <summary>
        /// Класс товара со скидкой (наследник Product).
        /// </summary>
        internal class DiscountedProduct : Product
        {
            private double discountPercent;

            /// <summary>
            /// Размер скидки в процентах.
            /// </summary>
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

            /// <summary>
            /// Возвращает цену товара с учётом скидки.
            /// </summary>
            /// <returns>Цена со скидкой</returns>
            public double DiscountedPrice => Math.Round(Price * (1 - DiscountPercent / 100), 2);

            /// <summary>
            /// Конструктор товара со скидкой.
            /// </summary>
            /// <param name="name">Название товара</param>
            /// <param name="manufacturer">Производитель</param>
            /// <param name="price">Цена</param>
            /// <param name="expirationDate">Дата окончания срока годности</param>
            /// <param name="productionDate">Дата производства</param>
            /// <param name="discountPercent">Размер скидки</param>
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

            /// <summary>
            /// Возвращает строковое представление товара со скидкой.
            /// </summary>
            /// <returns>Строка с информацией о товаре со скидкой</returns>
            public override string ToString()
            {
                return base.ToString() +
                       $"Скидка: {DiscountPercent}%\n" +
                       $"Акционная цена: {DiscountedPrice}\n";
            }
        }
    }
}


//namespace ConsoleApp3
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            try
//            {
//                Product product = ReadProductFromConsole();

//                Console.WriteLine("\nТовар успешно создан!");
//                Console.WriteLine(product);

//                Console.WriteLine("\nХотите изменить свойства товара? (да/нет)");
//                string answer = Console.ReadLine().ToLower();
//                if (answer == "да")
//                {
//                    Console.WriteLine("Что изменить? (1 - название, 2 - производитель, 3 - цена, 4 - дата производства, 5 - срок годности)");
//                    string choice = Console.ReadLine();

//                    switch (choice)
//                    {
//                        case "1":
//                            Console.Write("Новое название: ");
//                            product.SetName(Console.ReadLine());
//                            break;
//                        case "2":
//                            Console.Write("Новый производитель: ");
//                            product.SetManufacturer(Console.ReadLine());
//                            break;
//                        case "3":
//                            Console.Write("Новая цена: ");
//                            product.SetPrice(double.Parse(Console.ReadLine()));
//                            break;
//                        case "4":
//                            Console.Write("Новая дата производства (гггг-мм-дд): ");
//                            product.SetProductionDate(DateTime.Parse(Console.ReadLine()));
//                            break;
//                        case "5":
//                            Console.Write("Новая дата окончания срока годности (гггг-мм-дд): ");
//                            product.SetExpirationDate(DateTime.Parse(Console.ReadLine()));
//                            break;
//                        default:
//                            Console.WriteLine("Неизвестный выбор.");
//                            break;
//                    }

//                    Console.WriteLine("\nОбновлённая информация о товаре:");
//                    Console.WriteLine(product);
//                }

//                // Ввод данных для товара со скидкой
//                Console.WriteLine("\n=== Товар со скидкой ===");
//                Product baseProduct = ReadProductFromConsole();
//                Console.Write("Введите размер скидки (%): ");
//                double discountPercent = double.Parse(Console.ReadLine());

//                DiscountedProduct discountedProduct = new DiscountedProduct(
//                    baseProduct.Name, baseProduct.Manufacturer, baseProduct.Price,
//                    baseProduct.ExpirationDate, baseProduct.ProductionDate, discountPercent);

//                Console.WriteLine("\nИнформация о товаре со скидкой:");
//                Console.WriteLine(discountedProduct);
//            }
//            catch (ArgumentException ex)
//            {
//                Console.WriteLine($"Ошибка: {ex.Message}");
//            }
//            catch (FormatException)
//            {
//                Console.WriteLine("Ошибка: введён неверный формат числа или даты.");
//            }
//        }

//        static Product ReadProductFromConsole()
//        {
//            Console.Write("Введите название товара: ");
//            string name = Console.ReadLine();

//            Console.Write("Введите производителя: ");
//            string manufacturer = Console.ReadLine();

//            Console.Write("Введите цену: ");
//            double price = double.Parse(Console.ReadLine());

//            Console.Write("Введите дату производства (гггг-мм-дд): ");
//            DateTime productionDate = DateTime.Parse(Console.ReadLine());

//            Console.Write("Введите дату окончания срока годности (гггг-мм-дд): ");
//            DateTime expirationDate = DateTime.Parse(Console.ReadLine());

//            return new Product(name, manufacturer, price, expirationDate, productionDate);
//        }

//        internal class Product
//        {
//            string name;
//            string manufacturer;
//            double price;
//            DateTime expirationDate;
//            DateTime productionDate;

//            public string Name => name;
//            public string Manufacturer => manufacturer;
//            public double Price => price;
//            public DateTime ExpirationDate => expirationDate;
//            public DateTime ProductionDate => productionDate;

//            public Product(string name, string manufacturer, double price, DateTime expirationDate, DateTime productionDate)
//            {
//                SetName(name);
//                SetManufacturer(manufacturer);
//                SetPrice(price);
//                SetProductionDate(productionDate);
//                SetExpirationDate(expirationDate);
//            }

//            public void SetName(string name)
//            {
//                if (string.IsNullOrWhiteSpace(name))
//                    throw new ArgumentException("Название товара не может быть пустым.", nameof(name));
//                this.name = name;
//            }

//            public void SetManufacturer(string manufacturer)
//            {
//                if (string.IsNullOrWhiteSpace(manufacturer))
//                    throw new ArgumentException("Производитель не может быть пустым.", nameof(manufacturer));
//                this.manufacturer = manufacturer;
//            }

//            public void SetPrice(double price)
//            {
//                if (price <= 0)
//                    throw new ArgumentException("Цена должна быть положительным числом.", nameof(price));
//                this.price = price;
//            }

//            public void SetProductionDate(DateTime productionDate)
//            {
//                if (expirationDate != default && productionDate > expirationDate)
//                    throw new ArgumentException("Дата производства не может быть позже даты окончания срока годности.");
//                this.productionDate = productionDate;
//            }

//            public void SetExpirationDate(DateTime expirationDate)
//            {
//                if (productionDate != default && expirationDate <= productionDate)
//                    throw new ArgumentException("Дата окончания срока годности должна быть позже даты производства.");
//                this.expirationDate = expirationDate;
//            }

//            public override string ToString()
//            {
//                return $"Название: {Name}\n" +
//                       $"Производитель: {Manufacturer}\n" +
//                       $"Цена: {Price}\n" +
//                       $"Дата производства: {ProductionDate:yyyy-MM-dd}\n" +
//                       $"Срок годности до: {ExpirationDate:yyyy-MM-dd}";
//            }
//        }

//        internal class DiscountedProduct : Product
//        {
//            private double discountPercent;
//            public double DiscountPercent
//            {
//                get => discountPercent;
//                set
//                {
//                    if (value < 0 || value > 100)
//                        throw new ArgumentException("Размер скидки должен быть от 0 до 100 процентов.");
//                    discountPercent = value;
//                }
//            }

//            public double DiscountedPrice
//            {
//                get
//                {
//                    return Math.Round(Price * (1 - DiscountPercent / 100), 2);
//                }
//            }

//            public DiscountedProduct(
//                string name,
//                string manufacturer,
//                double price,
//                DateTime expirationDate,
//                DateTime productionDate,
//                double discountPercent)
//                : base(name, manufacturer, price, expirationDate, productionDate)
//            {
//                DiscountPercent = discountPercent;
//            }

//            public override string ToString()
//            {
//                return base.ToString() +
//                       $"Скидка: {DiscountPercent}%\n" +
//                       $"Акционная цена: {DiscountedPrice}\n";
//            }
//        }
//    }
//}

