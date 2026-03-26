using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Представляет учебную группу.
    /// </summary>
    public class Group
    {
        /// <summary>Уникальный идентификатор группы.</summary>
        public Guid GroupID { get; set; }
        /// <summary>Название группы.</summary>
        public string GroupName { get; set; }
        /// <summary>Признак активности группы.</summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Представляет студента.
    /// </summary>
    public class Student
    {
        /// <summary>Уникальный идентификатор студента.</summary>
        public Guid StudentID { get; set; }
        /// <summary>ФИО студента.</summary>
        public string FullName { get; set; }
        /// <summary>Дата рождения.</summary>
        public DateTime? BirthDate { get; set; }
        /// <summary>Средний балл.</summary>
        public decimal? GPA { get; set; }
        /// <summary>Идентификатор группы.</summary>
        public Guid GroupID { get; set; }
        /// <summary>Признак активности.</summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Представляет компьютерный класс.
    /// </summary>
    public class Classroom
    {
        /// <summary>Уникальный идентификатор класса.</summary>
        public Guid ClassroomID { get; set; }
        /// <summary>Название класса.</summary>
        public string RoomName { get; set; }
        /// <summary>Вместимость класса.</summary>
        public int? Capacity { get; set; }
        /// <summary>Признак активности.</summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Представляет компьютер.
    /// </summary>
    public class Computer
    {
        /// <summary>Уникальный идентификатор компьютера.</summary>
        public Guid ComputerID { get; set; }
        /// <summary>Инвентарный номер.</summary>
        public string InventoryNumber { get; set; }
        /// <summary>Бренд компьютера.</summary>
        public string Brand { get; set; }
        /// <summary>Дата покупки.</summary>
        public DateTime? PurchaseDate { get; set; }
        /// <summary>Цена.</summary>
        public decimal? Price { get; set; }
        /// <summary>Идентификатор класса.</summary>
        public Guid ClassroomID { get; set; }
        /// <summary>Признак активности.</summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Представляет занятие.
    /// </summary>
    public class Lesson
    {
        /// <summary>Уникальный идентификатор занятия.</summary>
        public Guid LessonID { get; set; }
        /// <summary>Тема занятия.</summary>
        public string Topic { get; set; }
        /// <summary>Дата занятия.</summary>
        public DateTime LessonDate { get; set; }
        /// <summary>Идентификатор класса.</summary>
        public Guid ClassroomID { get; set; }
        /// <summary>Признак активности.</summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Представляет посещение студентом занятия.
    /// </summary>
    public class StudentLesson
    {
        /// <summary>Идентификатор студента.</summary>
        public Guid StudentID { get; set; }
        /// <summary>Идентификатор занятия.</summary>
        public Guid LessonID { get; set; }
        /// <summary>Оценка.</summary>
        public decimal? Mark { get; set; }
        /// <summary>Признак присутствия.</summary>
        public bool? IsPresent { get; set; }
    }

    /// <summary>
    /// Представляет использование компьютера на занятии.
    /// </summary>
    public class ComputerLesson
    {
        /// <summary>Идентификатор компьютера.</summary>
        public Guid ComputerID { get; set; }
        /// <summary>Идентификатор занятия.</summary>
        public Guid LessonID { get; set; }
        /// <summary>Признак использования.</summary>
        public bool? IsUsed { get; set; }
    }

    /// <summary>
    /// Репозиторий для работы с таблицей Group.
    /// </summary>
    public class GroupRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория групп.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public GroupRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет новую группу в базу данных.
        /// </summary>
        /// <param name="group">Группа для добавления.</param>
        public void Create(Group group)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO [Group] (GroupID, GroupName, IsActive) VALUES (@GroupID, @GroupName, @IsActive)", conn))
                {
                    cmd.Parameters.AddWithValue("@GroupID", group.GroupID);
                    cmd.Parameters.AddWithValue("@GroupName", group.GroupName);
                    cmd.Parameters.AddWithValue("@IsActive", group.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает список всех групп.
        /// </summary>
        /// <returns>Список групп.</returns>
        public List<Group> GetAll()
        {
            var groups = new List<Group>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM [Group]", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(new Group
                        {
                            GroupID = (Guid)reader["GroupID"],
                            GroupName = (string)reader["GroupName"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return groups;
        }

        /// <summary>
        /// Обновляет данные группы.
        /// </summary>
        /// <param name="group">Группа для обновления.</param>
        public void Update(Group group)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления группы.
                using (var cmd = new SqlCommand(
                    "UPDATE [Group] SET GroupName = @GroupName, IsActive = @IsActive WHERE GroupID = @GroupID", conn))
                {
                    cmd.Parameters.AddWithValue("@GroupID", group.GroupID);
                    cmd.Parameters.AddWithValue("@GroupName", group.GroupName);
                    cmd.Parameters.AddWithValue("@IsActive", group.IsActive);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет группу по идентификатору.
        /// </summary>
        /// <param name="groupId">Идентификатор группы.</param>
        public void Delete(Guid groupId)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления группы.
                using (var cmd = new SqlCommand(
                    "DELETE FROM [Group] WHERE GroupID = @GroupID", conn))
                {
                    cmd.Parameters.AddWithValue("@GroupID", groupId);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// Репозиторий для работы с таблицей Student.
    /// </summary>
    public class StudentRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория студентов.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public StudentRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет нового студента в базу данных.
        /// </summary>
        /// <param name="student">Студент для добавления.</param>
        public void Create(Student student)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO Student (StudentID, FullName, BirthDate, GPA, GroupID, IsActive) VALUES (@StudentID, @FullName, @BirthDate, @GPA, @GroupID, @IsActive)", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
                    cmd.Parameters.AddWithValue("@FullName", student.FullName);
                    cmd.Parameters.AddWithValue("@BirthDate", (object)student.BirthDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GPA", (object)student.GPA ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GroupID", student.GroupID);
                    cmd.Parameters.AddWithValue("@IsActive", student.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает список всех студентов.
        /// </summary>
        /// <returns>Список студентов.</returns>
        public List<Student> GetAll()
        {
            var students = new List<Student>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Student", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            StudentID = (Guid)reader["StudentID"],
                            FullName = (string)reader["FullName"],
                            BirthDate = reader["BirthDate"] == DBNull.Value ? null : (DateTime?)reader["BirthDate"],
                            GPA = reader["GPA"] == DBNull.Value ? null : (decimal?)reader["GPA"],
                            GroupID = (Guid)reader["GroupID"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return students;
        }

        /// <summary>
        /// Обновляет данные студента.
        /// </summary>
        /// <param name="student">Студент для обновления.</param>
        public void Update(Student student)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления студента.
                using (var cmd = new SqlCommand(
                    "UPDATE Student SET FullName = @FullName, BirthDate = @BirthDate, GPA = @GPA, GroupID = @GroupID, IsActive = @IsActive WHERE StudentID = @StudentID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
                    cmd.Parameters.AddWithValue("@FullName", student.FullName);
                    cmd.Parameters.AddWithValue("@BirthDate", (object)student.BirthDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GPA", (object)student.GPA ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@GroupID", student.GroupID);
                    cmd.Parameters.AddWithValue("@IsActive", student.IsActive);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет студента по идентификатору.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        public void Delete(Guid studentId)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления студента.
                using (var cmd = new SqlCommand(
                    "DELETE FROM Student WHERE StudentID = @StudentID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// Репозиторий для работы с таблицей Classroom.
    /// </summary>
    public class ClassroomRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория классов.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public ClassroomRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет новый класс в базу данных.
        /// </summary>
        /// <param name="classroom">Класс для добавления.</param>
        public void Create(Classroom classroom)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO Classroom (ClassroomID, RoomName, Capacity, IsActive) VALUES (@ClassroomID, @RoomName, @Capacity, @IsActive)", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassroomID", classroom.ClassroomID);
                    cmd.Parameters.AddWithValue("@RoomName", classroom.RoomName);
                    cmd.Parameters.AddWithValue("@Capacity", (object)classroom.Capacity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", classroom.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает список всех классов.
        /// </summary>
        /// <returns>Список классов.</returns>
        public List<Classroom> GetAll()
        {
            var classrooms = new List<Classroom>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Classroom", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        classrooms.Add(new Classroom
                        {
                            ClassroomID = (Guid)reader["ClassroomID"],
                            RoomName = (string)reader["RoomName"],
                            Capacity = reader["Capacity"] == DBNull.Value ? null : (int?)reader["Capacity"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return classrooms;
        }

        /// <summary>
        /// Обновляет данные класса.
        /// </summary>
        /// <param name="classroom">Класс для обновления.</param>
        public void Update(Classroom classroom)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления класса.
                using (var cmd = new SqlCommand(
                    "UPDATE Classroom SET RoomName = @RoomName, Capacity = @Capacity, IsActive = @IsActive WHERE ClassroomID = @ClassroomID", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassroomID", classroom.ClassroomID);
                    cmd.Parameters.AddWithValue("@RoomName", classroom.RoomName);
                    cmd.Parameters.AddWithValue("@Capacity", (object)classroom.Capacity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", classroom.IsActive);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет класс по идентификатору.
        /// </summary>
        /// <param name="classroomId">Идентификатор класса.</param>
        public void Delete(Guid classroomId)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления класса.
                using (var cmd = new SqlCommand(
                    "DELETE FROM Classroom WHERE ClassroomID = @ClassroomID", conn))
                {
                    cmd.Parameters.AddWithValue("@ClassroomID", classroomId);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// Репозиторий для работы с таблицей Computer.
    /// </summary>
    public class ComputerRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория компьютеров.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public ComputerRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет новый компьютер в базу данных.
        /// </summary>
        /// <param name="computer">Компьютер для добавления.</param>
        public void Create(Computer computer)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO Computer (ComputerID, InventoryNumber, Brand, PurchaseDate, Price, ClassroomID, IsActive) VALUES (@ComputerID, @InventoryNumber, @Brand, @PurchaseDate, @Price, @ClassroomID, @IsActive)", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", computer.ComputerID);
                    cmd.Parameters.AddWithValue("@InventoryNumber", (object)computer.InventoryNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Brand", (object)computer.Brand ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PurchaseDate", (object)computer.PurchaseDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", (object)computer.Price ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ClassroomID", computer.ClassroomID);
                    cmd.Parameters.AddWithValue("@IsActive", computer.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает список всех компьютеров.
        /// </summary>
        /// <returns>Список компьютеров.</returns>
        public List<Computer> GetAll()
        {
            var computers = new List<Computer>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Computer", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        computers.Add(new Computer
                        {
                            ComputerID = (Guid)reader["ComputerID"],
                            InventoryNumber = reader["InventoryNumber"] == DBNull.Value ? null : (string)reader["InventoryNumber"],
                            Brand = reader["Brand"] == DBNull.Value ? null : (string)reader["Brand"],
                            PurchaseDate = reader["PurchaseDate"] == DBNull.Value ? null : (DateTime?)reader["PurchaseDate"],
                            Price = reader["Price"] == DBNull.Value ? null : (decimal?)reader["Price"],
                            ClassroomID = (Guid)reader["ClassroomID"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return computers;
        }

        /// <summary>
        /// Обновляет данные компьютера.
        /// </summary>
        /// <param name="computer">Компьютер для обновления.</param>
        public void Update(Computer computer)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления компьютера.
                using (var cmd = new SqlCommand(
                    "UPDATE Computer SET InventoryNumber = @InventoryNumber, Brand = @Brand, PurchaseDate = @PurchaseDate, Price = @Price, ClassroomID = @ClassroomID, IsActive = @IsActive WHERE ComputerID = @ComputerID", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", computer.ComputerID);
                    cmd.Parameters.AddWithValue("@InventoryNumber", (object)computer.InventoryNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Brand", (object)computer.Brand ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PurchaseDate", (object)computer.PurchaseDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", (object)computer.Price ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ClassroomID", computer.ClassroomID);
                    cmd.Parameters.AddWithValue("@IsActive", computer.IsActive);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет компьютер по идентификатору.
        /// </summary>
        /// <param name="computerId">Идентификатор компьютера.</param>
        public void Delete(Guid computerId)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления компьютера.
                using (var cmd = new SqlCommand(
                    "DELETE FROM Computer WHERE ComputerID = @ComputerID", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", computerId);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// Репозиторий для работы с таблицей Lesson.
    /// </summary>
    public class LessonRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория занятий.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public LessonRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет новое занятие в базу данных.
        /// </summary>
        /// <param name="lesson">Занятие для добавления.</param>
        public void Create(Lesson lesson)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO Lesson (LessonID, Topic, LessonDate, ClassroomID, IsActive) VALUES (@LessonID, @Topic, @LessonDate, @ClassroomID, @IsActive)", conn))
                {
                    cmd.Parameters.AddWithValue("@LessonID", lesson.LessonID);
                    cmd.Parameters.AddWithValue("@Topic", (object)lesson.Topic ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LessonDate", lesson.LessonDate);
                    cmd.Parameters.AddWithValue("@ClassroomID", lesson.ClassroomID);
                    cmd.Parameters.AddWithValue("@IsActive", lesson.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает список всех занятий.
        /// </summary>
        /// <returns>Список занятий.</returns>
        public List<Lesson> GetAll()
        {
            var lessons = new List<Lesson>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Lesson", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lessons.Add(new Lesson
                        {
                            LessonID = (Guid)reader["LessonID"],
                            Topic = reader["Topic"] == DBNull.Value ? null : (string)reader["Topic"],
                            LessonDate = (DateTime)reader["LessonDate"],
                            ClassroomID = (Guid)reader["ClassroomID"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
            }
            return lessons;
        }

        /// <summary>
        /// Обновляет данные занятия.
        /// </summary>
        /// <param name="lesson">Занятие для обновления.</param>
        public void Update(Lesson lesson)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления занятия.
                using (var cmd = new SqlCommand(
                    "UPDATE Lesson SET Topic = @Topic, LessonDate = @LessonDate, ClassroomID = @ClassroomID, IsActive = @IsActive WHERE LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@LessonID", lesson.LessonID);
                    cmd.Parameters.AddWithValue("@Topic", (object)lesson.Topic ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@LessonDate", lesson.LessonDate);
                    cmd.Parameters.AddWithValue("@ClassroomID", lesson.ClassroomID);
                    cmd.Parameters.AddWithValue("@IsActive", lesson.IsActive);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет занятие по идентификатору.
        /// </summary>
        /// <param name="lessonId">Идентификатор занятия.</param>
        public void Delete(Guid lessonId)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления занятия.
                using (var cmd = new SqlCommand(
                    "DELETE FROM Lesson WHERE LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@LessonID", lessonId);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// Репозиторий для работы с таблицей StudentLesson.
    /// </summary>
    public class StudentLessonRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория посещений студентов.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public StudentLessonRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет посещение студентом занятия.
        /// </summary>
        /// <param name="sl">Посещение для добавления.</param>
        public void Create(StudentLesson sl)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO StudentLesson (StudentID, LessonID, Mark, IsPresent) VALUES (@StudentID, @LessonID, @Mark, @IsPresent)", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", sl.StudentID);
                    cmd.Parameters.AddWithValue("@LessonID", sl.LessonID);
                    cmd.Parameters.AddWithValue("@Mark", (object)sl.Mark ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPresent", (object)sl.IsPresent ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает список всех посещений студентов.
        /// </summary>
        /// <returns>Список посещений.</returns>
        public List<StudentLesson> GetAll()
        {
            var list = new List<StudentLesson>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM StudentLesson", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new StudentLesson
                        {
                            StudentID = (Guid)reader["StudentID"],
                            LessonID = (Guid)reader["LessonID"],
                            Mark = reader["Mark"] == DBNull.Value ? null : (decimal?)reader["Mark"],
                            IsPresent = reader["IsPresent"] == DBNull.Value ? null : (bool?)reader["IsPresent"]
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Обновляет посещение студентом занятия.
        /// </summary>
        /// <param name="sl">Посещение для обновления.</param>
        public void Update(StudentLesson sl)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления посещения.
                using (var cmd = new SqlCommand(
                    "UPDATE StudentLesson SET Mark = @Mark, IsPresent = @IsPresent WHERE StudentID = @StudentID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", sl.StudentID);
                    cmd.Parameters.AddWithValue("@LessonID", sl.LessonID);
                    cmd.Parameters.AddWithValue("@Mark", (object)sl.Mark ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsPresent", (object)sl.IsPresent ?? DBNull.Value);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет посещение по идентификаторам студента и занятия.
        /// </summary>
        /// <param name="studentId">Идентификатор студента.</param>
        /// <param name="lessonId">Идентификатор занятия.</param>
        public void Delete(Guid studentId, Guid lessonId)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления посещения.
                using (var cmd = new SqlCommand(
                    "DELETE FROM StudentLesson WHERE StudentID = @StudentID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@LessonID", lessonId);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// Репозиторий для работы с таблицей ComputerLesson.
    /// </summary>
    public class ComputerLessonRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Создаёт новый экземпляр репозитория использования компьютеров.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public ComputerLessonRepository(string connectionString) => _connectionString = connectionString;

        /// <summary>
        /// Добавляет использование компьютера на занятии.
        /// </summary>
        /// <param name="cl">Использование для добавления.</param>
        public void Create(ComputerLesson cl)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO ComputerLesson (ComputerID, LessonID, IsUsed) VALUES (@ComputerID, @LessonID, @IsUsed)", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", cl.ComputerID);
                    cmd.Parameters.AddWithValue("@LessonID", cl.LessonID);
                    cmd.Parameters.AddWithValue("@IsUsed", (object)cl.IsUsed ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Получает список всех использований компьютеров.
        /// </summary>
        /// <returns>Список использований.</returns>
        public List<ComputerLesson> GetAll()
        {
            var list = new List<ComputerLesson>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM ComputerLesson", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ComputerLesson
                        {
                            ComputerID = (Guid)reader["ComputerID"],
                            LessonID = (Guid)reader["LessonID"],
                            IsUsed = reader["IsUsed"] == DBNull.Value ? null : (bool?)reader["IsUsed"]
                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Обновляет использование компьютера на занятии.
        /// </summary>
        /// <param name="cl">Использование для обновления.</param>
        public void Update(ComputerLesson cl)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления использования компьютера.
                using (var cmd = new SqlCommand(
                    "UPDATE ComputerLesson SET IsUsed = @IsUsed WHERE ComputerID = @ComputerID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", cl.ComputerID);
                    cmd.Parameters.AddWithValue("@LessonID", cl.LessonID);
                    cmd.Parameters.AddWithValue("@IsUsed", (object)cl.IsUsed ?? DBNull.Value);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Удаляет использование компьютера по идентификаторам компьютера и занятия.
        /// </summary>
        /// <param name="computerId">Идентификатор компьютера.</param>
        /// <param name="lessonId">Идентификатор занятия.</param>
        public void Delete(Guid computerId, Guid lessonId)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления использования компьютера.
                using (var cmd = new SqlCommand(
                    "DELETE FROM ComputerLesson WHERE ComputerID = @ComputerID AND LessonID = @LessonID", conn))
                {
                    cmd.Parameters.AddWithValue("@ComputerID", computerId);
                    cmd.Parameters.AddWithValue("@LessonID", lessonId);

                    // Выполняем команду.
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// Главный класс программы.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Точка входа в программу.
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

            while (true)
            {
                Console.WriteLine("\n1. Добавить группу");
                Console.WriteLine("2. Показать все группы");
                Console.WriteLine("3. Обновить группу");
                Console.WriteLine("4. Удалить группу");
                Console.WriteLine("5. Добавить студента");
                Console.WriteLine("6. Показать всех студентов");
                Console.WriteLine("7. Обновить студента");
                Console.WriteLine("8. Удалить студента");
                Console.WriteLine("9. Добавить класс");
                Console.WriteLine("10. Показать все классы");
                Console.WriteLine("11. Обновить класс");
                Console.WriteLine("12. Удалить класс");
                Console.WriteLine("13. Добавить компьютер");
                Console.WriteLine("14. Показать все компьютеры");
                Console.WriteLine("15. Обновить компьютер");
                Console.WriteLine("16. Удалить компьютер");
                Console.WriteLine("17. Добавить занятие");
                Console.WriteLine("18. Показать все занятия");
                Console.WriteLine("19. Обновить занятие");
                Console.WriteLine("20. Удалить занятие");
                Console.WriteLine("21. Добавить посещение студентом занятия");
                Console.WriteLine("22. Показать все посещения студентов");
                Console.WriteLine("23. Обновить посещение");
                Console.WriteLine("24. Удалить посещение");
                Console.WriteLine("25. Добавить использование компьютера на занятии");
                Console.WriteLine("26. Показать все использования компьютеров");
                Console.WriteLine("27. Обновить использование компьютера");
                Console.WriteLine("28. Удалить использование компьютера");
                Console.WriteLine("0. Выход");
                var choice = Console.ReadLine();

                if (choice == "1")
                {
                    // Добавить группу.
                    var group = new Group();
                    group.GroupID = Guid.NewGuid();
                    Console.Write("Название группы (например, ИВТ-21): ");
                    group.GroupName = Console.ReadLine();
                    Console.Write("Активна? (1 — да, 0 — нет): ");
                    group.IsActive = Console.ReadLine() == "1";
                    groupRepo.Create(group);
                    Console.WriteLine($"Группа добавлена. ID: {group.GroupID}");
                }
                else if (choice == "2")
                {
                    // Показать все группы.
                    var groups = groupRepo.GetAll();
                    Console.WriteLine("GroupID\tGroupName\tIsActive");
                    foreach (var group in groups)
                        Console.WriteLine($"{group.GroupID}\t{group.GroupName}\t{group.IsActive}");
                }
                else if (choice == "3")
                {
                    // Обновить группу.
                    Console.Write("Введите ID группы для обновления: ");
                    var id = Guid.Parse(Console.ReadLine());
                    var group = groupRepo.GetAll().Find(g => g.GroupID == id);
                    if (group == null)
                    {
                        Console.WriteLine("Группа не найдена.");
                    }
                    else
                    {
                        Console.Write("Новое название группы: ");
                        group.GroupName = Console.ReadLine();
                        Console.Write("Активна? (1 — да, 0 — нет): ");
                        group.IsActive = Console.ReadLine() == "1";
                        groupRepo.Update(group);
                        Console.WriteLine("Группа обновлена.");
                    }
                }
                else if (choice == "4")
                {
                    // Удалить группу.
                    Console.Write("Введите ID группы для удаления: ");
                    var id = Guid.Parse(Console.ReadLine());
                    groupRepo.Delete(id);
                    Console.WriteLine("Группа удалена.");
                }
                else if (choice == "5")
                {
                    // Добавить студента.
                    var student = new Student();
                    student.StudentID = Guid.NewGuid();
                    Console.Write("ФИО (например, Иванов Иван Иванович): ");
                    student.FullName = Console.ReadLine();
                    Console.Write("Дата рождения (формат: ГГГГ-ММ-ДД, например, 2000-12-31): ");
                    var birth = Console.ReadLine();
                    student.BirthDate = string.IsNullOrWhiteSpace(birth) ? (DateTime?)null : DateTime.Parse(birth);
                    Console.Write("Средний балл GPA (например, 4.75): ");
                    var gpa = Console.ReadLine();
                    student.GPA = string.IsNullOrWhiteSpace(gpa) ? (decimal?)null : decimal.Parse(gpa);
                    Console.Write("ID группы (формат: GUID, например, 123e4567-e89b-12d3-a456-426614174000): ");
                    student.GroupID = Guid.Parse(Console.ReadLine());
                    Console.Write("Активен? (1 — да, 0 — нет): ");
                    student.IsActive = Console.ReadLine() == "1";
                    studentRepo.Create(student);
                    Console.WriteLine($"Студент добавлен. ID: {student.StudentID}");
                }
                else if (choice == "6")
                {
                    // Показать всех студентов.
                    var students = studentRepo.GetAll();
                    Console.WriteLine("StudentID\tFullName\tBirthDate\tGPA\tGroupID\tIsActive");
                    foreach (var s in students)
                        Console.WriteLine($"{s.StudentID}\t{s.FullName}\t{s.BirthDate}\t{s.GPA}\t{s.GroupID}\t{s.IsActive}");
                }
                else if (choice == "7")
                {
                    // Обновить студента.
                    Console.Write("Введите ID студента для обновления: ");
                    var id = Guid.Parse(Console.ReadLine());
                    var student = studentRepo.GetAll().Find(s => s.StudentID == id);
                    if (student == null)
                    {
                        Console.WriteLine("Студент не найден.");
                    }
                    else
                    {
                        Console.Write("Новое ФИО: ");
                        student.FullName = Console.ReadLine();
                        Console.Write("Новая дата рождения (ГГГГ-ММ-ДД): ");
                        var birth = Console.ReadLine();
                        student.BirthDate = string.IsNullOrWhiteSpace(birth) ? (DateTime?)null : DateTime.Parse(birth);
                        Console.Write("Новый GPA: ");
                        var gpa = Console.ReadLine();
                        student.GPA = string.IsNullOrWhiteSpace(gpa) ? (decimal?)null : decimal.Parse(gpa);
                        Console.Write("Новый ID группы (GUID): ");
                        student.GroupID = Guid.Parse(Console.ReadLine());
                        Console.Write("Активен? (1 — да, 0 — нет): ");
                        student.IsActive = Console.ReadLine() == "1";
                        studentRepo.Update(student);
                        Console.WriteLine("Студент обновлен.");
                    }
                }
                else if (choice == "8")
                {
                    // Удалить студента.
                    Console.Write("Введите ID студента для удаления: ");
                    var id = Guid.Parse(Console.ReadLine());
                    studentRepo.Delete(id);
                    Console.WriteLine("Студент удален.");
                }
                else if (choice == "9")
                {
                    // Добавить класс.
                    var classroom = new Classroom();
                    classroom.ClassroomID = Guid.NewGuid();
                    Console.Write("Название класса (например, 101): ");
                    classroom.RoomName = Console.ReadLine();
                    Console.Write("Вместимость (целое число, например, 30): ");
                    var cap = Console.ReadLine();
                    classroom.Capacity = string.IsNullOrWhiteSpace(cap) ? (int?)null : int.Parse(cap);
                    Console.Write("Активен? (1 — да, 0 — нет): ");
                    classroom.IsActive = Console.ReadLine() == "1";
                    classroomRepo.Create(classroom);
                    Console.WriteLine($"Класс добавлен. ID: {classroom.ClassroomID}");
                }
                else if (choice == "10")
                {
                    // Показать все классы.
                    var classrooms = classroomRepo.GetAll();
                    Console.WriteLine("ClassroomID\tRoomName\tCapacity\tIsActive");
                    foreach (var c in classrooms)
                        Console.WriteLine($"{c.ClassroomID}\t{c.RoomName}\t{c.Capacity}\t{c.IsActive}");
                }
                else if (choice == "11")
                {
                    // Обновить класс.
                    Console.Write("Введите ID класса для обновления: ");
                    var id = Guid.Parse(Console.ReadLine());
                    var classroom = classroomRepo.GetAll().Find(c => c.ClassroomID == id);
                    if (classroom == null)
                    {
                        Console.WriteLine("Класс не найден.");
                    }
                    else
                    {
                        Console.Write("Новое название класса: ");
                        classroom.RoomName = Console.ReadLine();
                        Console.Write("Новая вместимость: ");
                        var cap = Console.ReadLine();
                        classroom.Capacity = string.IsNullOrWhiteSpace(cap) ? (int?)null : int.Parse(cap);
                        Console.Write("Активен? (1 — да, 0 — нет): ");
                        classroom.IsActive = Console.ReadLine() == "1";
                        classroomRepo.Update(classroom);
                        Console.WriteLine("Класс обновлен.");
                    }
                }
                else if (choice == "12")
                {
                    // Удалить класс.
                    Console.Write("Введите ID класса для удаления: ");
                    var id = Guid.Parse(Console.ReadLine());
                    classroomRepo.Delete(id);
                    Console.WriteLine("Класс удален.");
                }
                else if (choice == "13")
                {
                    // Добавить компьютер.
                    var computer = new Computer();
                    computer.ComputerID = Guid.NewGuid();
                    Console.Write("Инвентарный номер (например, PC-001): ");
                    computer.InventoryNumber = Console.ReadLine();
                    Console.Write("Бренд (например, Lenovo): ");
                    computer.Brand = Console.ReadLine();
                    Console.Write("Дата покупки (формат: ГГГГ-ММ-ДД, например, 2022-09-01): ");
                    var pd = Console.ReadLine();
                    computer.PurchaseDate = string.IsNullOrWhiteSpace(pd) ? (DateTime?)null : DateTime.Parse(pd);
                    Console.Write("Цена (например, 45000.50): ");
                    var price = Console.ReadLine();
                    computer.Price = string.IsNullOrWhiteSpace(price) ? (decimal?)null : decimal.Parse(price);
                    Console.Write("ID класса (GUID, например, 123e4567-e89b-12d3-a456-426614174000): ");
                    computer.ClassroomID = Guid.Parse(Console.ReadLine());
                    Console.Write("Активен? (1 — да, 0 — нет): ");
                    computer.IsActive = Console.ReadLine() == "1";
                    computerRepo.Create(computer);
                    Console.WriteLine($"Компьютер добавлен. ID: {computer.ComputerID}");
                }
                else if (choice == "14")
                {
                    // Показать все компьютеры.
                    var computers = computerRepo.GetAll();
                    Console.WriteLine("ComputerID\tInventoryNumber\tBrand\tPurchaseDate\tPrice\tClassroomID\tIsActive");
                    foreach (var c in computers)
                        Console.WriteLine($"{c.ComputerID}\t{c.InventoryNumber}\t{c.Brand}\t{c.PurchaseDate}\t{c.Price}\t{c.ClassroomID}\t{c.IsActive}");
                }
                else if (choice == "15")
                {
                    // Обновить компьютер.
                    Console.Write("Введите ID компьютера для обновления: ");
                    var id = Guid.Parse(Console.ReadLine());
                    var computer = computerRepo.GetAll().Find(c => c.ComputerID == id);
                    if (computer == null)
                    {
                        Console.WriteLine("Компьютер не найден.");
                    }
                    else
                    {
                        Console.Write("Новый инвентарный номер: ");
                        computer.InventoryNumber = Console.ReadLine();
                        Console.Write("Новый бренд: ");
                        computer.Brand = Console.ReadLine();
                        Console.Write("Новая дата покупки (ГГГГ-ММ-ДД): ");
                        var pd = Console.ReadLine();
                        computer.PurchaseDate = string.IsNullOrWhiteSpace(pd) ? (DateTime?)null : DateTime.Parse(pd);
                        Console.Write("Новая цена: ");
                        var price = Console.ReadLine();
                        computer.Price = string.IsNullOrWhiteSpace(price) ? (decimal?)null : decimal.Parse(price);
                        Console.Write("Новый ID класса (GUID): ");
                        computer.ClassroomID = Guid.Parse(Console.ReadLine());
                        Console.Write("Активен? (1 — да, 0 — нет): ");
                        computer.IsActive = Console.ReadLine() == "1";
                        computerRepo.Update(computer);
                        Console.WriteLine("Компьютер обновлен.");
                    }
                }
                else if (choice == "16")
                {
                    // Удалить компьютер.
                    Console.Write("Введите ID компьютера для удаления: ");
                    var id = Guid.Parse(Console.ReadLine());
                    computerRepo.Delete(id);
                    Console.WriteLine("Компьютер удален.");
                }
                else if (choice == "17")
                {
                    // Добавить занятие.
                    var lesson = new Lesson();
                    lesson.LessonID = Guid.NewGuid();
                    Console.Write("Тема занятия (например, Основы C#): ");
                    lesson.Topic = Console.ReadLine();
                    Console.Write("Дата занятия (формат: ГГГГ-ММ-ДД, например, 2023-03-15): ");
                    lesson.LessonDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("ID класса (GUID, например, 123e4567-e89b-12d3-a456-426614174000): ");
                    lesson.ClassroomID = Guid.Parse(Console.ReadLine());
                    Console.Write("Активно? (1 — да, 0 — нет): ");
                    lesson.IsActive = Console.ReadLine() == "1";
                    lessonRepo.Create(lesson);
                    Console.WriteLine($"Занятие добавлено. ID: {lesson.LessonID}");
                }
                else if (choice == "18")
                {
                    // Показать все занятия.
                    var lessons = lessonRepo.GetAll();
                    Console.WriteLine("LessonID\tTopic\tLessonDate\tClassroomID\tIsActive");
                    foreach (var l in lessons)
                        Console.WriteLine($"{l.LessonID}\t{l.Topic}\t{l.LessonDate}\t{l.ClassroomID}\t{l.IsActive}");
                }
                else if (choice == "19")
                {
                    // Обновить занятие.
                    Console.Write("Введите ID занятия для обновления: ");
                    var id = Guid.Parse(Console.ReadLine());
                    var lesson = lessonRepo.GetAll().Find(l => l.LessonID == id);
                    if (lesson == null)
                    {
                        Console.WriteLine("Занятие не найдено.");
                    }
                    else
                    {
                        Console.Write("Новая тема: ");
                        lesson.Topic = Console.ReadLine();
                        Console.Write("Новая дата занятия (ГГГГ-ММ-ДД): ");
                        lesson.LessonDate = DateTime.Parse(Console.ReadLine());
                        Console.Write("Новый ID класса (GUID): ");
                        lesson.ClassroomID = Guid.Parse(Console.ReadLine());
                        Console.Write("Активно? (1 — да, 0 — нет): ");
                        lesson.IsActive = Console.ReadLine() == "1";
                        lessonRepo.Update(lesson);
                        Console.WriteLine("Занятие обновлено.");
                    }
                }
                else if (choice == "20")
                {
                    // Удалить занятие.
                    Console.Write("Введите ID занятия для удаления: ");
                    var id = Guid.Parse(Console.ReadLine());
                    lessonRepo.Delete(id);
                    Console.WriteLine("Занятие удалено.");
                }
                else if (choice == "21")
                {
                    // Добавить посещение студентом занятия.
                    var sl = new StudentLesson();
                    Console.Write("ID студента (GUID, например, 123e4567-e89b-12d3-a456-426614174000): ");
                    sl.StudentID = Guid.Parse(Console.ReadLine());
                    Console.Write("ID занятия (GUID, например, 123e4567-e89b-12d3-a456-426614174000): ");
                    sl.LessonID = Guid.Parse(Console.ReadLine());
                    Console.Write("Оценка (например, 5.00): ");
                    var mark = Console.ReadLine();
                    sl.Mark = string.IsNullOrWhiteSpace(mark) ? (decimal?)null : decimal.Parse(mark);
                    Console.Write("Присутствовал? (1 — да, 0 — нет): ");
                    var present = Console.ReadLine();
                    sl.IsPresent = string.IsNullOrWhiteSpace(present) ? (bool?)null : present == "1";
                    studentLessonRepo.Create(sl);
                    Console.WriteLine("Посещение добавлено.");
                }
                else if (choice == "22")
                {
                    // Показать все посещения студентов.
                    var list = studentLessonRepo.GetAll();
                    Console.WriteLine("StudentID\tLessonID\tMark\tIsPresent");
                    foreach (var sl in list)
                        Console.WriteLine($"{sl.StudentID}\t{sl.LessonID}\t{sl.Mark}\t{sl.IsPresent}");
                }
                else if (choice == "23")
                {
                    // Обновить посещение.
                    Console.Write("Введите ID студента: ");
                    var studentId = Guid.Parse(Console.ReadLine());
                    Console.Write("Введите ID занятия: ");
                    var lessonId = Guid.Parse(Console.ReadLine());
                    var sl = studentLessonRepo.GetAll().Find(x => x.StudentID == studentId && x.LessonID == lessonId);
                    if (sl == null)
                    {
                        Console.WriteLine("Посещение не найдено.");
                    }
                    else
                    {
                        Console.Write("Новая оценка: ");
                        var mark = Console.ReadLine();
                        sl.Mark = string.IsNullOrWhiteSpace(mark) ? (decimal?)null : decimal.Parse(mark);
                        Console.Write("Присутствовал? (1 — да, 0 — нет): ");
                        var present = Console.ReadLine();
                        sl.IsPresent = string.IsNullOrWhiteSpace(present) ? (bool?)null : present == "1";
                        studentLessonRepo.Update(sl);
                        Console.WriteLine("Посещение обновлено.");
                    }
                }
                else if (choice == "24")
                {
                    // Удалить посещение.
                    Console.Write("Введите ID студента: ");
                    var studentId = Guid.Parse(Console.ReadLine());
                    Console.Write("Введите ID занятия: ");
                    var lessonId = Guid.Parse(Console.ReadLine());
                    studentLessonRepo.Delete(studentId, lessonId);
                    Console.WriteLine("Посещение удалено.");
                }
                else if (choice == "25")
                {
                    // Добавить использование компьютера на занятии.
                    var cl = new ComputerLesson();
                    Console.Write("ID компьютера (GUID, например, 123e4567-e89b-12d3-a456-426614174000): ");
                    cl.ComputerID = Guid.Parse(Console.ReadLine());
                    Console.Write("ID занятия (GUID, например, 123e4567-e89b-12d3-a456-426614174000): ");
                    cl.LessonID = Guid.Parse(Console.ReadLine());
                    Console.Write("Использовался? (1 — да, 0 — нет): ");
                    var used = Console.ReadLine();
                    cl.IsUsed = string.IsNullOrWhiteSpace(used) ? (bool?)null : used == "1";
                    computerLessonRepo.Create(cl);
                    Console.WriteLine("Использование добавлено.");
                }
                else if (choice == "26")
                {
                    // Показать все использования компьютеров.
                    var list = computerLessonRepo.GetAll();
                    Console.WriteLine("ComputerID\tLessonID\tIsUsed");
                    foreach (var cl in list)
                        Console.WriteLine($"{cl.ComputerID}\t{cl.LessonID}\t{cl.IsUsed}");
                }
                else if (choice == "27")
                {
                    // Обновить использование компьютера.
                    Console.Write("Введите ID компьютера: ");
                    var computerId = Guid.Parse(Console.ReadLine());
                    Console.Write("Введите ID занятия: ");
                    var lessonId = Guid.Parse(Console.ReadLine());
                    var cl = computerLessonRepo.GetAll().Find(x => x.ComputerID == computerId && x.LessonID == lessonId);
                    if (cl == null)
                    {
                        Console.WriteLine("Использование не найдено.");
                    }
                    else
                    {
                        Console.Write("Использовался? (1 — да, 0 — нет): ");
                        var used = Console.ReadLine();
                        cl.IsUsed = string.IsNullOrWhiteSpace(used) ? (bool?)null : used == "1";
                        computerLessonRepo.Update(cl);
                        Console.WriteLine("Использование обновлено.");
                    }
                }
                else if (choice == "28")
                {
                    // Удалить использование компьютера.
                    Console.Write("Введите ID компьютера: ");
                    var computerId = Guid.Parse(Console.ReadLine());
                    Console.Write("Введите ID занятия: ");
                    var lessonId = Guid.Parse(Console.ReadLine());
                    computerLessonRepo.Delete(computerId, lessonId);
                    Console.WriteLine("Использование удалено.");
                }
                else if (choice == "0")
                {
                    // Завершение работы программы.
                    break;
                }
            }
        }
    }
}