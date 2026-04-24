using EntityFramework_Database;
using System;
using System.Collections.Generic;

namespace EntityFramework_Database
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
        /// Возвращает строковое представление объекта сущности.
        /// </summary>
        /// <param name="entity">Экземпляр сущности.</param>
        /// <returns>Строковое представление.</returns>
        public abstract string ToString(T entity);


        /// <summary>
        /// Печатает все строки данной таблицы
        /// </summary>
        public abstract void PrintAll();

        /// <summary>
        /// Демонстрирация выполнения всех CRUD-операций для сущности.
        /// </summary>
        /// <param name="entity">Экземпляр сущности.</param>
        public void DemoAllCrudOperations(T entity)
        {
            // 1. Получить и вывести все элементы
            Console.WriteLine("\nТекущий список:");
            PrintAll();

            // 2. Добавить новый элемент
            Create(entity);
            Console.WriteLine("\nДобавлен новый элемент:");
            Console.WriteLine(entity.ToString());
            Console.WriteLine("\n\n");
            PrintAll();

            // 3. Обновить сущность через её собственный Update
            if (entity is IUpdateble<T> up)
            {
                up.Update(entity); // Меняет поля объекта (например, добавляет суффикс " Updated")
                Console.WriteLine("\nИзменённый (локально) объект:");
                Console.WriteLine(ToString(entity));
            }
            else
            {
                Console.WriteLine("Обновление не поддерживается для этого типа.");
            }

            // 4. Перезаписать обновлённую сущность в базу через Update()
            Update(entity);
            Console.WriteLine("Объект обновлён в базе данных.");
            PrintAll();

            // 5. Удалить этот же объект из базы  
            // Универсально ищем свойство с Guid-ключом по типу (ClassroomID, GroupID и т.д.)
            var keyProp = typeof(T).GetProperty(typeof(T).Name + "ID");
            if (keyProp == null)
            {
                Console.WriteLine("Не удалось идентифицировать ключ объекта для удаления.");
                return;
            }
            var keyValue = (Guid)keyProp.GetValue(entity);

            // Удаление (у большинства репозиториев второй Guid не нужен)
            Delete(keyValue);
            Console.WriteLine($"Объект с ключом Id = {keyValue} удалён.");

            // 6. Показать финальный список
            Console.WriteLine("Финальный список:");
            PrintAll();
        }
    }
}