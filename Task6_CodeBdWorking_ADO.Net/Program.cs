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

            // Методы для получения первого существующего объекта, чтобы использовать их как foreign keys
            var groupId = groupRepo.GetAll().First().GroupID;
            var classroomId = classroomRepo.GetAll().First().ClassroomID;
            var computerID = computerRepo.GetAll().First().ComputerID;
            var lessonID = lessonRepo.GetAll().First().LessonID;
            var studentID = studentRepo.GetAll().First().StudentID;

            Console.WriteLine("\nДемонстрация CRUD для Группы");
            var group = new Group
            {
                GroupID = Guid.NewGuid(),
                GroupName = "Демо-группа",
                IsActive = true
            };
            groupRepo.DemoAllCrudOperations(group);

            Console.WriteLine("\nДемонстрация CRUD для Студента");
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

            Console.WriteLine("\nДемонстрация CRUD для Класса");
            var classroom = new Classroom
            {
                ClassroomID = Guid.NewGuid(),
                RoomName = "Демо-класс",
                Capacity = 30,
                IsActive = true
            };
            classroomRepo.DemoAllCrudOperations(classroom);

            Console.WriteLine("\nДемонстрация CRUD для Компьютера");
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

            Console.WriteLine("\nДемонстрация CRUD для Занятия");
            var lesson = new Lesson
            {
                LessonID = Guid.NewGuid(),
                Topic = "Демо-занятие",
                LessonDate = DateTime.Now,
                ClassroomID = classroomId,
                IsActive = true
            };
            lessonRepo.DemoAllCrudOperations(lesson);

            Console.WriteLine("\nДемонстрация CRUD для Посещения студента");
            var sl = new StudentLesson
            {
                StudentID = studentID,
                LessonID = lessonID,
                Mark = 5.0m,
                IsPresent = true
            };
            studentLessonRepo.DemoAllCrudOperations(sl);

            Console.WriteLine("\nДемонстрация CRUD для Использования компьютера");
            var cl = new ComputerLesson
            {
                ComputerID = computerID,
                LessonID = lessonID,
                IsUsed = true
            };
            computerLessonRepo.DemoAllCrudOperations(cl);

            Console.WriteLine("\nДемонстрация завершена. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}