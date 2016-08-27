using System.Collections.Generic;

namespace QueueExtensions
{
    /// <summary>
    /// Контейнер для стандартной очереди .NET (System.Collections.Generic.Queue). Предоставляет возможности получения объекта коллекции, синхронизации и обновление коллекции
    /// </summary>
    /// <typeparam name="T">Тип элементов очереди</typeparam>
    public interface IQueueContainer<T>
    {
        /// <summary>
        /// Возвращает ссылку на содержащуюся в контейнере коллекцию
        /// </summary>
        /// <returns>Ссылка на коллекцию</returns>
        Queue<T> GetQueue();
        /// <summary>
        /// Возвращает объект синхронизации доступа к коллекции, следует всегда использовать блокировку при попытке получения доступа к очереди, хранящейся в контейнере (особенно при использовании метода <see cref="SetQueue"/>)
        /// </summary>
        /// <returns>Объект синхронизации доступа к коллекции</returns>
        object GetSyncRoot();
        /// <summary>
        /// Заменяет содержующуюся ссылку на коллекцию, новая коллекция заменяет старую
        /// </summary>
        /// <param name="queue">Новая коллекция</param>
        void SetQueue(Queue<T> queue);
    }
}
