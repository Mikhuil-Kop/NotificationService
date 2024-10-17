using AspNetCommonLibrary.Workers;

namespace AspNetCommonLibrary.Services
{
    /// <summary>
    /// Сервис, который отвечает за поддержание беcперебойной работы фонового процесса.
    /// Постоянно получает и запускает Worker с заданой паузой между вызовами.
    /// 
    /// В случае ошибки работы Worker-а - также задает паузу, после чего пробует еще раз.
    /// 
    /// Если Worker не удалось получить - возвращает критическую ошибку и завершает работу.
    /// </summary>
    public interface IRecurringWorkerService
    {
        Task ExecuteAsync<T>(Func<IServiceProvider, T> workerSelector, int normalRecurringTime, int errorRecurringTime, CancellationToken stoppingToken) where T : IRecurringWorker;
    }
}