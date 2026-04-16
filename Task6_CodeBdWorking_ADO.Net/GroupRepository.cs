using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Репозиторий для работы с таблицей Group.
    /// </summary>
    public class GroupRepository : RepositoryBase<Group>
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
        public override void Create(Group group)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "INSERT INTO [Group]" +
                    "(GroupID, GroupName, IsActive)" +
                    "VALUES (@GroupID, @GroupName, @IsActive)", conn))
                {
                    cmd.Parameters.AddWithValue("@GroupID", group.GroupID);
                    cmd.Parameters.AddWithValue("@GroupName", group.GroupName);
                    cmd.Parameters.AddWithValue("@IsActive", group.IsActive);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Message.Contains("FOREIGN KEY"))
                        {
                            throw new InvalidOperationException("Ошибка: указан несуществующий внешний ключ GroupID для студента.", ex);
                        }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Получает список всех групп.
        /// </summary>
        /// <returns>Список групп.</returns>
        public override List<Group> GetAll()
        {
            var groups = new List<Group>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT GroupID, GroupName, IsActive FROM [Group]", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("GroupID\tGroupName\tIsActive");
                    while (reader.Read())
                    {
                        var group = new Group
                        {
                            GroupID = (Guid)reader["GroupID"],
                            GroupName = (string)reader["GroupName"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        groups.Add(group);

                        // Печать содержимого строки
                        Console.WriteLine($"{group.GroupID}\t{group.GroupName}\t{group.IsActive}");
                        Console.WriteLine();
                    }
                }
            }
            return groups;
        }


        /// <summary>
        /// Обновляет данные группы.
        /// </summary>
        /// <param name="group">Группа для обновления.</param>
        public override void Update(Group group)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для обновления группы.
                using (var cmd = new SqlCommand(
                    "UPDATE [Group] " +
                    "SET GroupName = @GroupName, IsActive = @IsActive" +
                    "WHERE GroupID = @GroupID", conn))
                {
                    cmd.Parameters.AddWithValue("@GroupID", group.GroupID);
                    cmd.Parameters.AddWithValue("@GroupName", group.GroupName);
                    cmd.Parameters.AddWithValue("@IsActive", group.IsActive);

                    // Выполняем команду.
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Message.Contains("FOREIGN KEY"))
                        {
                            throw new InvalidOperationException("Ошибка: указан несуществующий внешний ключ GroupID для студента.", ex);
                        }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Удаляет группу по идентификатору.
        /// </summary>
        /// <param name="groupId">Идентификатор группы.</param>
        public override void Delete(Guid groupId, Guid id2 = default)
        {
            // Открываем соединение с базой данных.
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Готовим SQL-команду для удаления группы.
                using (var cmd = new SqlCommand(
                    "DELETE FROM [Group]" +
                    "WHERE GroupID = @GroupID", conn))
                {
                    cmd.Parameters.AddWithValue("@GroupID", groupId);

                    // Выполняем команду.
                    int affected = cmd.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        throw new InvalidOperationException("Удаление не выполнено: запись с таким ключом не найдена.");
                    }
                }
            }
        }
    }
}