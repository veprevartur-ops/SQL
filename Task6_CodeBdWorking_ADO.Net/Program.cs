using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Главный класс программы. Запускает демонстрацию CRUD-операций для различных сущностей.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Точка входа в программу. Позволяет выбрать сущность и выполнить для неё демонстрацию CRUD-операций.
        /// </summary>
        static void Main()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("application.json")
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            var groupRepo = new GroupRepository(connectionString);
            var studentRepo = new StudentRepository(connectionString);
            var classroomRepo = new ClassroomRepository(connectionString);
            var computerRepo = new ComputerRepository(connectionString);
            var lessonRepo = new LessonRepository(connectionString);
            var studentLessonRepo = new StudentLessonRepository(connectionString);
            var computerLessonRepo = new ComputerLessonRepository(connectionString);

            // Методы для получения первого существующего объекта
            var groupId = groupRepo.GetAll().First().GroupID;
            var classroomId = classroomRepo.GetAll().First().ClassroomID;
            var computerID = computerRepo.GetAll().First().ComputerID;
            var lessonID = lessonRepo.GetAll().First().LessonID;
            var studentID = studentRepo.GetAll().First().StudentID;


            while (true)
            {
                Console.WriteLine("\n=== Демонстрация CRUD-операций ===");
                Console.WriteLine("1. Группа");
                Console.WriteLine("2. Студент");
                Console.WriteLine("3. Класс");
                Console.WriteLine("4. Компьютер");
                Console.WriteLine("5. Занятие");
                Console.WriteLine("6. Посещение студента");
                Console.WriteLine("7. Использование компьютера");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите сущность для демонстрации: ");
                var choice = Console.ReadLine();

                // Демонстрация CRUD-операций для группы
                if (choice == "1")
                {
                    var group = new Group
                    {
                        GroupID = Guid.NewGuid(),
                        GroupName = "Демо-группа",
                        IsActive = true
                    };
                    groupRepo.DemoAllCrudOperations(group);
                }
                // Демонстрация CRUD-операций для студента
                else if (choice == "2")
                {
                    var student = new Student
                    {
                        StudentID = Guid.NewGuid(),
                        FullName = "Демо Студент",
                        BirthDate = DateTime.Now.AddYears(-20),
                        GPA = 4.5m,
                        GroupID = groupId,
                        IsActive = true
                    };
                    studentRepo.DemoAllCrudOperations(student);
                }
                // Демонстрация CRUD-операций для класса
                else if (choice == "3")
                {
                    var classroom = new Classroom
                    {
                        ClassroomID = Guid.NewGuid(),
                        RoomName = "Демо-класс",
                        Capacity = 30,
                        IsActive = true
                    };
                    classroomRepo.DemoAllCrudOperations(classroom);
                }
                // Демонстрация CRUD-операций для компьютера
                else if (choice == "4")
                {
                    var computer = new Computer
                    {
                        ComputerID = Guid.NewGuid(),
                        InventoryNumber = "DEMO-001",
                        Brand = "DemoBrand",
                        PurchaseDate = DateTime.Now.AddYears(-1),
                        Price = 50000,
                        ClassroomID = classroomId,
                        IsActive = true
                    };
                    computerRepo.DemoAllCrudOperations(computer);
                }
                // Демонстрация CRUD-операций для занятия
                else if (choice == "5")
                {
                    var lesson = new Lesson
                    {
                        LessonID = Guid.NewGuid(),
                        Topic = "Демо-занятие",
                        LessonDate = DateTime.Now,
                        ClassroomID = classroomId,
                        IsActive = true
                    };
                    lessonRepo.DemoAllCrudOperations(lesson);
                }
                // Демонстрация CRUD-операций для посещения студентом занятия
                else if (choice == "6")
                {
                    var sl = new StudentLesson
                    {
                        StudentID = studentID,
                        LessonID = lessonID,
                        Mark = 5.0m,
                        IsPresent = true
                    };
                    studentLessonRepo.DemoAllCrudOperations(sl);
                }
                // Демонстрация CRUD-операций для использования компьютера на уроке
                else if (choice == "7")
                {
                    var cl = new ComputerLesson
                    {
                        ComputerID = computerID,
                        LessonID = lessonID,
                        IsUsed = true
                    };
                    computerLessonRepo.DemoAllCrudOperations(cl);
                }
                // Завершение работы программы
                else if (choice == "0")
                {
                    break;
                }
            }
        }
    }
}