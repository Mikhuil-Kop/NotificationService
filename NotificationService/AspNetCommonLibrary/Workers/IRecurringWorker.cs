namespace AspNetCommonLibrary.Workers
{
    /// <summary>
    /// Интерфейс Worker-а, который будет выполняться на фоне в отдельном потоке.
    /// Этот Worker существует, пока не выполнит свою задачу, после чего уничтожается.
    /// Он вновь создается спустя некоторое время, чтобы выполнить свою работу.
    /// </summary>
    public interface IRecurringWorker
    {
        Task WorkAsync(CancellationToken cancellationToken);
    }
}