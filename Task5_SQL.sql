// Создание базы данных

CREATE DATABASE InformaticsLessons;
GO

USE InformaticsLessons;
GO

// Создание таблиц

// Группа
CREATE TABLE [Group]
(
    GroupID     UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    GroupName   NVARCHAR(100) NOT NULL,
    IsActive    BIT NOT NULL DEFAULT 1
);

// Студент
CREATE TABLE [Student]
(
    StudentID   UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FullName    NVARCHAR(100) NOT NULL,
    BirthDate   DATE,
    GPA         DECIMAL(3,2),
    GroupID     UNIQUEIDENTIFIER NOT NULL,
    IsActive    BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (GroupID) REFERENCES [Group](GroupID)
);

// Компьютерный класс
CREATE TABLE [Classroom]
(
    ClassroomID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RoomName    NVARCHAR(50) NOT NULL,
    Capacity    INT,
    IsActive    BIT NOT NULL DEFAULT 1
);

// Компьютер
CREATE TABLE [Computer]
(
    ComputerID      UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    InventoryNumber NVARCHAR(50),
    Brand           NVARCHAR(50),
    PurchaseDate    DATETIME,
    Price           DECIMAL(10,2),
    ClassroomID     UNIQUEIDENTIFIER NOT NULL,
    IsActive        BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (ClassroomID) REFERENCES [Classroom](ClassroomID)
);

// Занятие
CREATE TABLE [Lesson]
(
    LessonID    UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Topic       NVARCHAR(200),
    LessonDate  DATETIME NOT NULL,
    ClassroomID UNIQUEIDENTIFIER NOT NULL,
    IsActive    BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (ClassroomID) REFERENCES [Classroom](ClassroomID)
);

// Связь Студент — Занятие
CREATE TABLE [StudentLesson]
(
    StudentID   UNIQUEIDENTIFIER NOT NULL,
    LessonID    UNIQUEIDENTIFIER NOT NULL,
    Mark        DECIMAL(4,2),
    IsPresent   BIT,
    PRIMARY KEY (StudentID, LessonID),
    FOREIGN KEY (StudentID) REFERENCES [Student](StudentID),
    FOREIGN KEY (LessonID)  REFERENCES [Lesson](LessonID)
);

// Связь Компьютер — Занятие
CREATE TABLE [ComputerLesson]
(
    ComputerID  UNIQUEIDENTIFIER NOT NULL,
    LessonID    UNIQUEIDENTIFIER NOT NULL,
    IsUsed      BIT,
    PRIMARY KEY (ComputerID, LessonID),
    FOREIGN KEY (ComputerID) REFERENCES [Computer](ComputerID),
    FOREIGN KEY (LessonID)   REFERENCES [Lesson](LessonID)
);

// Вставка тестовых данных

INSERT INTO [Group] (GroupID, GroupName, IsActive)
VALUES 
    (NEWID(), N'ПИ-101', 1)
   ,(NEWID(), N'ПИ-102', 1)
   ,(NEWID(), N'ПИ-103', 0);

INSERT INTO [Student] (StudentID, FullName, BirthDate, GPA, GroupID, IsActive)
VALUES 
    (NEWID(), N'Иванов Иван Иванович',   '2003-05-12', 4.50, 
        (SELECT TOP 1 GroupID 
         FROM [Group] 
         WHERE GroupName = N'ПИ-101'), 1)
   ,(NEWID(), N'Петров Петр Петрович',   '2002-11-23', 3.80, 
        (SELECT TOP 1 GroupID 
         FROM [Group] 
         WHERE GroupName = N'ПИ-102'), 1)
   ,(NEWID(), N'Сидорова Анна Сергеевна','2004-01-30', 4.90, 
        (SELECT TOP 1 GroupID 
         FROM [Group] 
         WHERE GroupName = N'ПИ-101'), 0);

INSERT INTO [Classroom] (ClassroomID, RoomName, Capacity, IsActive)
VALUES 
     (NEWID(), N'101', 20, 1)
    ,(NEWID(), N'102', 25, 1)
    ,(NEWID(), N'103', 15, 0);

INSERT INTO [Computer] (ComputerID, InventoryNumber, Brand, PurchaseDate, Price, ClassroomID, IsActive)
VALUES 
     (NEWID(), N'INV-001', N'HP',   '20220901', 45000.00, 
        (SELECT TOP 1 ClassroomID 
         FROM [Classroom] 
         WHERE RoomName = N'101'), 1)
    ,(NEWID(), N'INV-002', N'Dell', '20210815', 42000.00, 
        (SELECT TOP 1 ClassroomID 
         FROM [Classroom] 
         WHERE RoomName = N'102'), 1)
    ,(NEWID(), N'INV-003', N'Acer', '20200520', 39000.00, 
        (SELECT TOP 1 ClassroomID 
         FROM [Classroom] 
         WHERE RoomName = N'101'), 0);

INSERT INTO [Lesson] (LessonID, Topic, LessonDate, ClassroomID, IsActive)
VALUES 
     (NEWID(), N'Основы программирования', '20240220 09:00:00', 
        (SELECT TOP 1 ClassroomID 
         FROM [Classroom] 
         WHERE RoomName = N'101'), 1)
    ,(NEWID(), N'Базы данных',             '20240221 11:00:00', 
        (SELECT TOP 1 ClassroomID 
         FROM [Classroom] 
         WHERE RoomName = N'102'), 1)
    ,(NEWID(), N'Компьютерные сети',       '20240222 13:00:00', 
        (SELECT TOP 1 ClassroomID 
         FROM [Classroom] 
         WHERE RoomName = N'103'), 0);

INSERT INTO [StudentLesson] (StudentID, LessonID, Mark, IsPresent)
VALUES 
     ((SELECT TOP 1 StudentID 
       FROM [Student] 
       WHERE FullName = N'Иванов Иван Иванович'), 
      (SELECT TOP 1 LessonID 
       FROM [Lesson] 
       WHERE Topic = N'Основы программирования'), 5.00, 1)
    
    ,((SELECT TOP 1 StudentID 
        FROM [Student] 
        WHERE FullName = N'Петров Петр Петрович'), 
      (SELECT TOP 1 LessonID 
        FROM [Lesson] 
        WHERE Topic = N'Базы данных'), 4.50, 1)
    
    ,((SELECT TOP 1 StudentID 
        FROM [Student] 
        WHERE FullName = N'Сидорова Анна Сергеевна'), 
      (SELECT TOP 1 LessonID 
        FROM [Lesson] 
        WHERE Topic = N'Компьютерные сети'), 4.80, 0);

INSERT INTO [ComputerLesson] (ComputerID, LessonID, IsUsed)
VALUES 
     ((SELECT TOP 1 ComputerID 
       FROM [Computer] 
       WHERE InventoryNumber = N'INV-001'), 
      (SELECT TOP 1 LessonID 
       FROM [Lesson] 
       WHERE Topic = N'Основы программирования'), 1)
    
    ,((SELECT TOP 1 ComputerID 
        FROM [Computer] 
        WHERE InventoryNumber = N'INV-002'), 
      (SELECT TOP 1 LessonID 
        FROM [Lesson] 
        WHERE Topic = N'Базы данных'), 1)
    
    ,((SELECT TOP 1 ComputerID 
        FROM [Computer] 
        WHERE InventoryNumber = N'INV-003'), 
      (SELECT TOP 1 LessonID 
        FROM [Lesson] 
        WHERE Topic = N'Компьютерные сети'), 0);

// Работа с данными

// Все активные студенты из групп ПИ-101 и ПИ-102
SELECT TOP 50
     s.FullName
    ,s.BirthDate
    ,s.GPA
FROM 
    Student s
JOIN 
    [Group] g ON s.GroupID = g.GroupID
WHERE 
    (g.GroupName = N'ПИ-101' OR g.GroupName = N'ПИ-102')
    AND s.IsActive = 1
ORDER BY 
    s.FullName;

// Удаление записей неактивных студентов
DELETE 
FROM 
    [StudentLesson]
WHERE 
    StudentID IN 
        (SELECT StudentID 
         FROM [Student] 
         WHERE IsActive = 0);

DELETE 
FROM 
    [Student]
WHERE 
    IsActive = 0;

// Изменить оценку студента
UPDATE 
    [StudentLesson]
SET 
    Mark = 5.00
WHERE 
    StudentID = (SELECT TOP 1 StudentID 
                 FROM [Student] 
                 WHERE FullName = N'Иванов Иван Иванович')
    AND LessonID = (SELECT TOP 1 LessonID 
                    FROM [Lesson] 
                    WHERE Topic = N'Основы программирования');

// Средний балл по каждой группе
SELECT TOP 50
    g.GroupName,
    AVG(s.GPA) AS AvgGPA
FROM 
    [Student] s
JOIN 
    [Group] g ON s.GroupID = g.GroupID
GROUP BY 
    g.GroupName
ORDER BY 
    AvgGPA DESC;

// Все студенты и их оценки на занятиях
SELECT 
    s.FullName,
    l.Topic,
    sl.Mark
FROM 
    [Student] s
LEFT JOIN 
    [StudentLesson] sl ON s.StudentID = sl.StudentID
LEFT JOIN 
    [Lesson] l ON sl.LessonID = l.LessonID
ORDER BY 
    s.FullName, l.LessonDate;

// Все занятия и студенты, которые их посещали
SELECT TOP 50
    l.Topic,
    l.LessonDate,
    s.FullName
FROM 
    [Lesson] l
RIGHT JOIN 
    [StudentLesson] sl ON l.LessonID = sl.LessonID
RIGHT JOIN 
    [Student] s ON sl.StudentID = s.StudentID
ORDER BY 
    l.LessonDate, s.FullName;

// Студенты, получившие оценку выше 4
SELECT TOP 50
    s.FullName,
    l.Topic,
    sl.Mark
FROM 
    [Student] s
INNER JOIN 
    [StudentLesson] sl ON s.StudentID = sl.StudentID
INNER JOIN 
    [Lesson] l ON sl.LessonID = l.LessonID
WHERE 
    sl.Mark > 4
ORDER BY 
    sl.Mark DESC;
