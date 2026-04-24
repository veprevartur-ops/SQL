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

        public GroupRepository(string connectionString) => _connectionString = connectionString;

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
                    while (reader.Read())
                    {
                        var group = new Group
                        {
                            GroupID = (Guid)reader["GroupID"],
                            GroupName = (string)reader["GroupName"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        groups.Add(group);
                    }
                }
            }
            return groups;
        }


        public override void Update(Group group)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "UPDATE [Group] " +
                    "SET GroupName = @GroupName, IsActive = @IsActive " +
                    "WHERE GroupID = @GroupID", conn))
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

        public override void Delete(Guid groupId, Guid id2 = default)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "DELETE FROM [Group] WHERE GroupID = @GroupID", conn))
                {
                    cmd.Parameters.AddWithValue("@GroupID", groupId);

                    int affected = cmd.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        throw new InvalidOperationException("Удаление не выполнено: запись с таким ключом не найдена.");
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает строковое представление группы.
        /// </summary>
        /// <param name="group">Экземпляр группы.</param>
        /// <returns>Строка для вывода.</returns>
        public override string ToString(Group group)
        {
            return $"{group.GroupID}\t{group.GroupName}\t{(group.IsActive ? "Активна" : "Неактивна")}";
        }

        /// <summary>
        /// Вывод в консоль всех групп из таблицы Group.
        /// </summary>
        public override void PrintAll()
        {
            var all = GetAll();
            Console.WriteLine("GroupID\t\t\t\t\tGroupName\tIsActive");
            foreach (var group in all)
            {
                Console.WriteLine(ToString(group));
            }
        }
    }
}