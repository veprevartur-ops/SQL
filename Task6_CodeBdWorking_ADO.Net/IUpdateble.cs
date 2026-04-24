namespace ConsoleAdoDatabase
{
    /// <summary>
    /// Интерфейс обновления сущности.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public interface IUpdateble<T>
    {
        /// <summary>
        /// Обновление имени или основного поля сущности.
        /// </summary>
        /// <param name="entity">Сущность с новыми данными.</param>
        void Update(T entity);
    }
}