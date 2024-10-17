namespace AspNetCommonLibrary.Workers
{
    /// <summary>
    /// Интерфейс Worker-а, который будет выполняться на фоне в отдельном потоке.
    /// Этот Worker должен сам поддерживать свою постоянную работу.
    /// </summary>
    public interface ISingletonWorker
    {
        Task WorkAsync(CancellationToken cancellationToken);
    }
}