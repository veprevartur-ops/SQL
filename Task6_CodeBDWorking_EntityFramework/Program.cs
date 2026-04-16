using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace EntityFramework_Database
{

    /// <summary>
    /// The main class of the program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The entry point of the program.
        /// </summary>
        static void Main()
        {
            // Build configuration from application.json.
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("application.json")
                .Build();

            // Get the connection string from configuration.
            string connectionString = config.GetConnectionString("DefaultConnection");

            // Create repositories for all entities.
            var groupRepo = new GroupRepository(connectionString);
            var studentRepo = new StudentRepository(connectionString);
            var classroomRepo = new ClassroomRepository(connectionString);
            var computerRepo = new ComputerRepository(connectionString);
            var lessonRepo = new LessonRepository(connectionString);
            var studentLessonRepo = new StudentLessonRepository(connectionString);
            var computerLessonRepo = new ComputerLessonRepository(connectionString);

            while (true)
            {
                // Display the main menu.
                Console.WriteLine("\n1. Add group");
                Console.WriteLine("2. Show all groups");
                Console.WriteLine("3. Add student");
                Console.WriteLine("4. Show all students");
                Console.WriteLine("5. Add classroom");
                Console.WriteLine("6. Show all classrooms");
                Console.WriteLine("7. Add computer");
                Console.WriteLine("8. Show all computers");
                Console.WriteLine("9. Add lesson");
                Console.WriteLine("10. Show all lessons");
                Console.WriteLine("11. Add student attendance");
                Console.WriteLine("12. Show all student attendances");
                Console.WriteLine("13. Add computer usage");
                Console.WriteLine("14. Show all computer usages");
                Console.WriteLine("15. Update group");
                Console.WriteLine("16. Delete group");
                Console.WriteLine("17. Update student");
                Console.WriteLine("18. Delete student");
                Console.WriteLine("19. Update classroom");
                Console.WriteLine("20. Delete classroom");
                Console.WriteLine("21. Update computer");
                Console.WriteLine("22. Delete computer");
                Console.WriteLine("23. Update lesson");
                Console.WriteLine("24. Delete lesson");
                Console.WriteLine("25. Update student attendance");
                Console.WriteLine("26. Delete student attendance");
                Console.WriteLine("27. Update computer usage");
                Console.WriteLine("28. Delete computer usage");
                Console.WriteLine("0. Exit");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    // Add a new group.
                    var group = new Group();
                    group.GroupID = Guid.NewGuid();
                    Console.Write("Group name: ");
                    group.GroupName = Console.ReadLine();
                    Console.Write("IsActive (1/0): ");
                    group.IsActive = Console.ReadLine() == "1";
                    groupRepo.Create(group);
                    Console.WriteLine($"Group added. ID: {group.GroupID}");
                }
                else if (choice == "2")
                {
                    // Show all groups.
                    var groups = groupRepo.GetAll();
                    Console.WriteLine("GroupID\tGroupName\tIsActive");
                    foreach (var group in groups)
                        Console.WriteLine($"{group.GroupID}\t{group.GroupName}\t{group.IsActive}");
                }
                else if (choice == "3")
                {
                    // Add a new student.
                    var student = new Student();
                    student.StudentID = Guid.NewGuid();
                    Console.Write("Full name: ");
                    student.FullName = Console.ReadLine();
                    Console.Write("Birth date (yyyy-MM-dd): ");
                    var birth = Console.ReadLine();
                    student.BirthDate = string.IsNullOrWhiteSpace(birth) ? (DateTime?)null : DateTime.Parse(birth);
                    Console.Write("GPA: ");
                    var gpa = Console.ReadLine();
                    student.GPA = string.IsNullOrWhiteSpace(gpa) ? (decimal?)null : decimal.Parse(gpa);
                    Console.Write("GroupID: ");
                    student.GroupID = Guid.Parse(Console.ReadLine());
                    Console.Write("IsActive (1/0): ");
                    student.IsActive = Console.ReadLine() == "1";
                    studentRepo.Create(student);
                    Console.WriteLine($"Student added. ID: {student.StudentID}");
                }
                else if (choice == "4")
                {
                    // Show all students.
                    var students = studentRepo.GetAll();
                    Console.WriteLine("StudentID\tFullName\tBirthDate\tGPA\tGroupID\tIsActive");
                    foreach (var s in students)
                        Console.WriteLine($"{s.StudentID}\t{s.FullName}\t{s.BirthDate}\t{s.GPA}\t{s.GroupID}\t{s.IsActive}");
                }
                else if (choice == "5")
                {
                    // Add a new classroom.
                    var classroom = new Classroom();
                    classroom.ClassroomID = Guid.NewGuid();
                    Console.Write("Room name: ");
                    classroom.RoomName = Console.ReadLine();
                    Console.Write("Capacity: ");
                    var cap = Console.ReadLine();
                    classroom.Capacity = string.IsNullOrWhiteSpace(cap) ? (int?)null : int.Parse(cap);
                    Console.Write("IsActive (1/0): ");
                    classroom.IsActive = Console.ReadLine() == "1";
                    classroomRepo.Create(classroom);
                    Console.WriteLine($"Classroom added. ID: {classroom.ClassroomID}");
                }
                else if (choice == "6")
                {
                    // Show all classrooms.
                    var classrooms = classroomRepo.GetAll();
                    Console.WriteLine("ClassroomID\tRoomName\tCapacity\tIsActive");
                    foreach (var c in classrooms)
                        Console.WriteLine($"{c.ClassroomID}\t{c.RoomName}\t{c.Capacity}\t{c.IsActive}");
                }
                else if (choice == "7")
                {
                    // Add a new computer.
                    var computer = new Computer();
                    computer.ComputerID = Guid.NewGuid();
                    Console.Write("Inventory number: ");
                    computer.InventoryNumber = Console.ReadLine();
                    Console.Write("Brand: ");
                    computer.Brand = Console.ReadLine();
                    Console.Write("Purchase date (yyyy-MM-dd): ");
                    var pd = Console.ReadLine();
                    computer.PurchaseDate = string.IsNullOrWhiteSpace(pd) ? (DateTime?)null : DateTime.Parse(pd);
                    Console.Write("Price: ");
                    var price = Console.ReadLine();
                    computer.Price = string.IsNullOrWhiteSpace(price) ? (decimal?)null : decimal.Parse(price);
                    Console.Write("ClassroomID: ");
                    computer.ClassroomID = Guid.Parse(Console.ReadLine());
                    Console.Write("IsActive (1/0): ");
                    computer.IsActive = Console.ReadLine() == "1";
                    computerRepo.Create(computer);
                    Console.WriteLine($"Computer added. ID: {computer.ComputerID}");
                }
                else if (choice == "8")
                {
                    // Show all computers.
                    var computers = computerRepo.GetAll();
                    Console.WriteLine("ComputerID\tInventoryNumber\tBrand\tPurchaseDate\tPrice\tClassroomID\tIsActive");
                    foreach (var c in computers)
                        Console.WriteLine($"{c.ComputerID}\t{c.InventoryNumber}\t{c.Brand}\t{c.PurchaseDate}\t{c.Price}\t{c.ClassroomID}\t{c.IsActive}");
                }
                else if (choice == "9")
                {
                    // Add a new lesson.
                    var lesson = new Lesson();
                    lesson.LessonID = Guid.NewGuid();
                    Console.Write("Topic: ");
                    lesson.Topic = Console.ReadLine();
                    Console.Write("Lesson date (yyyy-MM-dd): ");
                    lesson.LessonDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("ClassroomID: ");
                    lesson.ClassroomID = Guid.Parse(Console.ReadLine());
                    Console.Write("IsActive (1/0): ");
                    lesson.IsActive = Console.ReadLine() == "1";
                    lessonRepo.Create(lesson);
                    Console.WriteLine($"Lesson added. ID: {lesson.LessonID}");
                }
                else if (choice == "10")
                {
                    // Show all lessons.
                    var lessons = lessonRepo.GetAll();
                    Console.WriteLine("LessonID\tTopic\tLessonDate\tClassroomID\tIsActive");
                    foreach (var l in lessons)
                        Console.WriteLine($"{l.LessonID}\t{l.Topic}\t{l.LessonDate}\t{l.ClassroomID}\t{l.IsActive}");
                }
                else if (choice == "11")
                {
                    // Add a student attendance.
                    var sl = new StudentLesson();
                    Console.Write("StudentID: ");
                    sl.StudentID = Guid.Parse(Console.ReadLine());
                    Console.Write("LessonID: ");
                    sl.LessonID = Guid.Parse(Console.ReadLine());
                    Console.Write("Mark: ");
                    var mark = Console.ReadLine();
                    sl.Mark = string.IsNullOrWhiteSpace(mark) ? (decimal?)null : decimal.Parse(mark);
                    Console.Write("Is present (1/0): ");
                    var present = Console.ReadLine();
                    sl.IsPresent = string.IsNullOrWhiteSpace(present) ? (bool?)null : present == "1";
                    studentLessonRepo.Create(sl);
                    Console.WriteLine("Attendance added.");
                }
                else if (choice == "12")
                {
                    // Show all student attendances.
                    var list = studentLessonRepo.GetAll();
                    Console.WriteLine("StudentID\tLessonID\tMark\tIsPresent");
                    foreach (var sl in list)
                        Console.WriteLine($"{sl.StudentID}\t{sl.LessonID}\t{sl.Mark}\t{sl.IsPresent}");
                }
                else if (choice == "13")
                {
                    // Add a computer usage.
                    var cl = new ComputerLesson();
                    Console.Write("ComputerID: ");
                    cl.ComputerID = Guid.Parse(Console.ReadLine());
                    Console.Write("LessonID: ");
                    cl.LessonID = Guid.Parse(Console.ReadLine());
                    Console.Write("Is used (1/0): ");
                    var used = Console.ReadLine();
                    cl.IsUsed = string.IsNullOrWhiteSpace(used) ? (bool?)null : used == "1";
                    computerLessonRepo.Create(cl);
                    Console.WriteLine("Computer usage added.");
                }
                else if (choice == "14")
                {
                    // Show all computer usages.
                    var list = computerLessonRepo.GetAll();
                    Console.WriteLine("ComputerID\tLessonID\tIsUsed");
                    foreach (var cl in list)
                        Console.WriteLine($"{cl.ComputerID}\t{cl.LessonID}\t{cl.IsUsed}");
                }
                else if (choice == "15")
                {
                    // Update a group.
                    Console.Write("Enter GroupID to update: ");
                    var id = Guid.Parse(Console.ReadLine());
                    var group = groupRepo.GetAll().Find(g => g.GroupID == id);
                    if (group == null)
                        Console.WriteLine("Group not found.");
                    else
                    {
                        Console.Write("New group name: ");
                        group.GroupName = Console.ReadLine();
                        Console.Write("IsActive (1/0): ");
                        group.IsActive = Console.ReadLine() == "1";
                        groupRepo.Update(group);
                        Console.WriteLine("Group updated.");
                    }
                }
                else if (choice == "16")
                {
                    // Delete a group.
                    Console.Write("Enter GroupID to delete: ");
                    var id = Guid.Parse(Console.ReadLine());
                    groupRepo.Delete(id);
                    Console.WriteLine("Group deleted (if existed).");
                }
                else if (choice == "17")
                {
                    // Update a student.
                    Console.Write("Enter StudentID to update: ");
                    var id = Guid.Parse(Console.ReadLine());
                    var student = studentRepo.GetAll().Find(s => s.StudentID == id);
                    if (student == null)
                        Console.WriteLine("Student not found.");
                    else
                    {
                        Console.Write("New full name: ");
                        student.FullName = Console.ReadLine();
                        Console.Write("Birth date (yyyy-MM-dd): ");
                        var birth = Console.ReadLine();
                        student.BirthDate = string.IsNullOrWhiteSpace(birth) ? (DateTime?)null : DateTime.Parse(birth);
                        Console.Write("GPA: ");
                        var gpa = Console.ReadLine();
                        student.GPA = string.IsNullOrWhiteSpace(gpa) ? (decimal?)null : decimal.Parse(gpa);
                        Console.Write("GroupID: ");
                        student.GroupID = Guid.Parse(Console.ReadLine());
                        Console.Write("IsActive (1/0): ");
                        student.IsActive = Console.ReadLine() == "1";
                        studentRepo.Update(student);
                        Console.WriteLine("Student updated.");
                    }
                }
                else if (choice == "18")
                {
                    // Delete a student.
                    Console.Write("Enter StudentID to delete: ");
                    var id = Guid.Parse(Console.ReadLine());
                    studentRepo.Delete(id);
                    Console.WriteLine("Student deleted (if existed).");
                }
                else if (choice == "19")
                {
                    // Update a classroom.
                    Console.Write("Enter ClassroomID to update: ");
                    var id = Guid.Parse(Console.ReadLine());
                    var classroom = classroomRepo.GetAll().Find(c => c.ClassroomID == id);
                    if (classroom == null)
                        Console.WriteLine("Classroom not found.");
                    else
                    {
                        Console.Write("New room name: ");
                        classroom.RoomName = Console.ReadLine();
                        Console.Write("Capacity: ");
                        var cap = Console.ReadLine();
                        classroom.Capacity = string.IsNullOrWhiteSpace(cap) ? (int?)null : int.Parse(cap);
                        Console.Write("IsActive (1/0): ");
                        classroom.IsActive = Console.ReadLine() == "1";
                        classroomRepo.Update(classroom);
                        Console.WriteLine("Classroom updated.");
                    }
                }
                else if (choice == "20")
                {
                    // Delete a classroom.
                    Console.Write("Enter ClassroomID to delete: ");
                    var id = Guid.Parse(Console.ReadLine());
                    classroomRepo.Delete(id);
                    Console.WriteLine("Classroom deleted (if existed).");
                }
                else if (choice == "21")
                {
                    // Update a computer.
                    Console.Write("Enter ComputerID to update: ");
                    var id = Guid.Parse(Console.ReadLine());
                    var computer = computerRepo.GetAll().Find(c => c.ComputerID == id);
                    if (computer == null)
                        Console.WriteLine("Computer not found.");
                    else
                    {
                        Console.Write("Inventory number: ");
                        computer.InventoryNumber = Console.ReadLine();
                        Console.Write("Brand: ");
                        computer.Brand = Console.ReadLine();
                        Console.Write("Purchase date (yyyy-MM-dd): ");
                        var pd = Console.ReadLine();
                        computer.PurchaseDate = string.IsNullOrWhiteSpace(pd) ? (DateTime?)null : DateTime.Parse(pd);
                        Console.Write("Price: ");
                        var price = Console.ReadLine();
                        computer.Price = string.IsNullOrWhiteSpace(price) ? (decimal?)null : decimal.Parse(price);
                        Console.Write("ClassroomID: ");
                        computer.ClassroomID = Guid.Parse(Console.ReadLine());
                        Console.Write("IsActive (1/0): ");
                        computer.IsActive = Console.ReadLine() == "1";
                        computerRepo.Update(computer);
                        Console.WriteLine("Computer updated.");
                    }
                }
                else if (choice == "22")
                {
                    // Delete a computer.
                    Console.Write("Enter ComputerID to delete: ");
                    var id = Guid.Parse(Console.ReadLine());
                    computerRepo.Delete(id);
                    Console.WriteLine("Computer deleted (if existed).");
                }
                else if (choice == "23")
                {
                    // Update a lesson.
                    Console.Write("Enter LessonID to update: ");
                    var id = Guid.Parse(Console.ReadLine());
                    var lesson = lessonRepo.GetAll().Find(l => l.LessonID == id);
                    if (lesson == null)
                        Console.WriteLine("Lesson not found.");
                    else
                    {
                        Console.Write("Topic: ");
                        lesson.Topic = Console.ReadLine();
                        Console.Write("Lesson date (yyyy-MM-dd): ");
                        lesson.LessonDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("ClassroomID: ");
                        lesson.ClassroomID = Guid.Parse(Console.ReadLine());
                        Console.Write("IsActive (1/0): ");
                        lesson.IsActive = Console.ReadLine() == "1";
                        lessonRepo.Update(lesson);
                        Console.WriteLine("Lesson updated.");
                    }
                }
                else if (choice == "24")
                {
                    // Delete a lesson.
                    Console.Write("Enter LessonID to delete: ");
                    var id = Guid.Parse(Console.ReadLine());
                    lessonRepo.Delete(id);
                    Console.WriteLine("Lesson deleted (if existed).");
                }
                else if (choice == "25")
                {
                    // Update a student attendance.
                    Console.Write("StudentID: ");
                    var studentId = Guid.Parse(Console.ReadLine());
                    Console.Write("LessonID: ");
                    var lessonId = Guid.Parse(Console.ReadLine());
                    var sl = studentLessonRepo.GetAll().Find(x => x.StudentID == studentId && x.LessonID == lessonId);
                    if (sl == null)
                        Console.WriteLine("Attendance not found.");
                    else
                    {
                        Console.Write("Mark: ");
                        var mark = Console.ReadLine();
                        sl.Mark = string.IsNullOrWhiteSpace(mark) ? (decimal?)null : decimal.Parse(mark);
                        Console.Write("Is present (1/0): ");
                        var present = Console.ReadLine();
                        sl.IsPresent = string.IsNullOrWhiteSpace(present) ? (bool?)null : present == "1";
                        studentLessonRepo.Update(sl);
                        Console.WriteLine("Attendance updated.");
                    }
                }
                else if (choice == "26")
                {
                    // Delete a student attendance.
                    Console.Write("StudentID: ");
                    var studentId = Guid.Parse(Console.ReadLine());
                    Console.Write("LessonID: ");
                    var lessonId = Guid.Parse(Console.ReadLine());
                    studentLessonRepo.Delete(studentId, lessonId);
                    Console.WriteLine("Attendance deleted (if existed).");
                }
                else if (choice == "27")
                {
                    // Update a computer usage.
                    Console.Write("ComputerID: ");
                    var computerId = Guid.Parse(Console.ReadLine());
                    Console.Write("LessonID: ");
                    var lessonId = Guid.Parse(Console.ReadLine());
                    var cl = computerLessonRepo.GetAll().Find(x => x.ComputerID == computerId && x.LessonID == lessonId);
                    if (cl == null)
                        Console.WriteLine("Computer usage not found.");
                    else
                    {
                        Console.Write("Is used (1/0): ");
                        var used = Console.ReadLine();
                        cl.IsUsed = string.IsNullOrWhiteSpace(used) ? (bool?)null : used == "1";
                        computerLessonRepo.Update(cl);
                        Console.WriteLine("Computer usage updated.");
                    }
                }
                else if (choice == "28")
                {
                    // Delete a computer usage.
                    Console.Write("ComputerID: ");
                    var computerId = Guid.Parse(Console.ReadLine());
                    Console.Write("LessonID: ");
                    var lessonId = Guid.Parse(Console.ReadLine());
                    computerLessonRepo.Delete(computerId, lessonId);
                    Console.WriteLine("Computer usage deleted (if existed).");
                }
                else if (choice == "0")
                {
                    // Exit the program.
                    break;
                }
            }
        }
    }
}