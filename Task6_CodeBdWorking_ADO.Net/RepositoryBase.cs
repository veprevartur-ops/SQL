using System;
using System.Collections.Generic;


namespace ConsoleAdoDatabase
{
    public abstract class RepositoryBase<T>
        {
            /// <summary>
            /// Добавляет новую сущность.
            /// </summary>
            /// <param name="entity">Сущность для добавления.</param>
            public abstract void Create(T entity);

            /// <summary>
            /// Получает все сущности.
            /// </summary>
            /// <returns>Список сущностей.</returns>
            public abstract List<T> GetAll();

            /// <summary>
            /// Обновляет сущность.
            /// </summary>
            /// <param name="entity">Сущность для обновления.</param>
            public abstract void Update(T entity);

            /// <summary>
            /// Удаляет сущность по одному или двум идентификаторам.
            /// </summary>
            /// <param name="id1">Первый идентификатор (обязательный).</param>
            /// <param name="id2">Второй идентификатор (обязательный только для составного ключа).</param>
            public abstract void Delete(Guid id1, Guid id2 = default);


            /// <summary>
            /// Демонстрирует выполнение всех CRUD-операций для сущности.
            /// </summary>
            /// <param name="entity">Экземпляр сущности.</param>
            public void DemoAllCrudOperations(T entity)
            {
                GetAll();

                Create(entity);
                Update(entity);

                Console.Write("Введите Guid для удаления: ");
                string input = Console.ReadLine();
                if (Guid.TryParse(input, out Guid id))
                {
                    Delete(id);
                    Console.WriteLine($"Сущность с Id = {id} удалена.");
                }
                else
                {
                    Console.WriteLine("Некорректный Guid.");
                }

                GetAll();
            }
    }
}