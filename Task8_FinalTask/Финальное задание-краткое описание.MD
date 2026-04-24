1)В папке Models расположены классы Категория, Статья и Транзакция с их полями.
2)В папке Controllers расположены контроллеры для каждого из классов: Категория, Статья, Транзакция.
3)Каждая транзакция имеет внешний ключ-id статьи, а каждая статья имеет внешний ключ-id категории, у каждой категории есть коллекция(ICollection) статей, а у каждой статьи есть коллекция(ICollection) транзакций.
4)В файле Program.cs идет настройка сервисов(контроллеры, Swagger, CORS, DbContext) и запуск приложения.
5)Файл appsettings.json - конфигурационный файл, который содержит строку подключения к базе данных ExpenseDB(в которой находятся все данные по категориям и статьям расходов и по транзакциям) и другие настройки.
6)Файл AppDbContext.cs - контекст базы данных для EF Core, содержит DbSet для каждой сущности. Метод OnModelCreating для настройки связей, имён таблиц, типов данных.
7)В каждом контроллере есть методы GET(показ списка категорий, статей или транзакций), POST(добавление новых элементов в таблицы базы данных ExpenseDB), PUT(обновление какого-либо элемента одной из таблиц ExpenseDB), DELETE(удаление какого-либо элемента одной из таблиц ExpenseDB)
8)В контроллере ExpenseTransactionsController кроме того есть методы GET, с помощью которых можно увидеть список транзакций по дням и месяцам.
9)База данных ExpenseDB создается в SQL с помощью следующих команд: 
      //Создание ExpenseDB
      CREATE DATABASE ExpenseDb;
      GO
      USE ExpenseDb;
      GO

      //Создание таблицы Категорий
      CREATE TABLE ExpenseCategories (
      Id INT IDENTITY(1,1) PRIMARY KEY,
      Name NVARCHAR(100) NOT NULL,
      MonthlyBudget DECIMAL(18,2) NOT NULL,
      IsActive BIT NOT NULL DEFAULT 1
      );

      //Создание таблицы статей
      CREATE TABLE ExpenseItems (
      Id INT IDENTITY(1,1) PRIMARY KEY,
      Name NVARCHAR(100) NOT NULL,
      CategoryId INT NOT NULL,
      IsActive BIT NOT NULL DEFAULT 1,
      CONSTRAINT FK_ExpenseItems_Categories FOREIGN KEY (CategoryId)
      REFERENCES ExpenseCategories(Id) ON DELETE CASCADE

      //Создание таблицы транзакций
      CREATE TABLE ExpenseTransactions (
      Id INT IDENTITY(1,1) PRIMARY KEY,
      Date DATE NOT NULL,
      Amount DECIMAL(18,2) NOT NULL,
      Comment NVARCHAR(200),
      ExpenseItemId INT NOT NULL,
      CONSTRAINT FK_ExpenseTransactions_Items FOREIGN KEY (ExpenseItemId)
      REFERENCES ExpenseItems(Id) ON DELETE CASCADE
      );

10)Фронтенд (Vue.js):
        1.src/App.vue
        Главный компонент приложения.
        Содержит шапку, вкладки, формы и списки для всех сущностей.
        2.src/api.js
        Модуль для работы с API (axios).
        Функции для всех CRUD-операций и фильтрации.
        3.src/components/CategoryList.vue
        Компонент для работы с категориями (список, добавление, удаление, редактирование).
        4.src/components/ExpenseItemList.vue
        Компонент для работы со статьями расходов.
        5.src/components/TransactionList.vue
        Компонент для работы с транзакциями, фильтрация по дню/месяцу, визуализация стикеров.

Скрины работы Swagger:
<img width="1395" height="932" alt="image" src="https://github.com/user-attachments/assets/5e38509c-47fe-4d9f-b99d-d1076684cbad" />
<img width="1342" height="920" alt="image" src="https://github.com/user-attachments/assets/2193e263-dace-48c5-a452-be7d9219a04d" />
<img width="1298" height="900" alt="image" src="https://github.com/user-attachments/assets/95641e35-e711-4920-8bd0-af2083f73751" />
<img width="892" height="848" alt="image" src="https://github.com/user-attachments/assets/47ba7a28-b642-4ee3-a3f2-8dc215f22731" />
<img width="892" height="552" alt="image" src="https://github.com/user-attachments/assets/dece229f-c161-4d8c-8c28-864b2749f91f" />
<img width="902" height="687" alt="image" src="https://github.com/user-attachments/assets/85ebb0c5-3974-4dc0-9bd1-fc617846c283" />
<img width="893" height="863" alt="image" src="https://github.com/user-attachments/assets/eaf625d2-7cae-4143-9993-752006535187" />
<img width="891" height="657" alt="image" src="https://github.com/user-attachments/assets/31b122c5-c4f6-4c6f-966d-1e91b2220b1d" />
<img width="890" height="782" alt="image" src="https://github.com/user-attachments/assets/dd85aa5e-6d9d-4e49-bd2f-f7e64f40a4ad" />
<img width="901" height="556" alt="image" src="https://github.com/user-attachments/assets/c978a336-7398-4a7a-a060-38e91475fa42" />
<img width="898" height="710" alt="image" src="https://github.com/user-attachments/assets/c3eab2ec-9cdf-4e9d-8ba9-de2e6e4bbcdc" />
<img width="900" height="862" alt="image" src="https://github.com/user-attachments/assets/7306327b-fa74-4183-b63c-07719607a25e" />
<img width="908" height="758" alt="image" src="https://github.com/user-attachments/assets/42b85a6a-229f-4ba2-b7dc-5bf803ba81fe" />
<img width="912" height="702" alt="image" src="https://github.com/user-attachments/assets/c6623d87-5b9c-4b63-a743-edbe4a384a9a" />
<img width="908" height="866" alt="image" src="https://github.com/user-attachments/assets/1cb6b16a-9685-4927-9a05-bedef9602601" />
<img width="911" height="486" alt="image" src="https://github.com/user-attachments/assets/f7621858-a9b7-4a01-a6d2-4b091d4fd68a" />

Скрины работы Insomnia: 
<img width="1590" height="902" alt="image" src="https://github.com/user-attachments/assets/1fdd4086-1300-4e60-8916-86cdae3a5594" />
<img width="1592" height="907" alt="image" src="https://github.com/user-attachments/assets/9092d58d-9564-4324-b65b-6b0d9f8438ae" />
<img width="1587" height="900" alt="image" src="https://github.com/user-attachments/assets/7d9141a2-0787-4726-ad8e-dbf8b52156da" />
<img width="1595" height="907" alt="image" src="https://github.com/user-attachments/assets/29ec102d-8c5b-48d4-a712-c60d07cfcb84" />
<img width="1587" height="905" alt="image" src="https://github.com/user-attachments/assets/95cef050-a797-4360-8209-0196b7fa6fce" />
<img width="1613" height="901" alt="image" src="https://github.com/user-attachments/assets/f5ad2605-524f-43d9-89f9-deac377e32c5" />
<img width="1612" height="898" alt="image" src="https://github.com/user-attachments/assets/bd5b1287-96e9-42d1-a7c6-245ca7a4c835" />
<img width="1615" height="902" alt="image" src="https://github.com/user-attachments/assets/83252bcf-0fb4-4a7a-a626-10ca83a5511a" />
<img width="1615" height="906" alt="image" src="https://github.com/user-attachments/assets/0cd0b749-764b-4f7c-bf39-eed590623df1" />
<img width="1610" height="902" alt="image" src="https://github.com/user-attachments/assets/17809248-1bfb-44ed-b780-4a16a0d26c5c" />
<img width="1612" height="907" alt="image" src="https://github.com/user-attachments/assets/bf6cf53b-2c6a-42d0-8028-5792d31c9c4d" />
<img width="1618" height="907" alt="image" src="https://github.com/user-attachments/assets/b738a382-8a62-454d-8040-d276a3fcf737" />
<img width="1610" height="903" alt="image" src="https://github.com/user-attachments/assets/1255a8b9-dec5-4bf1-8b93-c0d00321ceab" />
<img width="1613" height="902" alt="image" src="https://github.com/user-attachments/assets/63877204-163d-4725-abc0-f27df91c49ce" />
<img width="1602" height="867" alt="image" src="https://github.com/user-attachments/assets/6aca9222-602e-430f-80f6-526b9239db89" />
<img width="1606" height="892" alt="image" src="https://github.com/user-attachments/assets/42b5362a-f4d5-4d76-b838-fc5a92c0d67e" />
























      
