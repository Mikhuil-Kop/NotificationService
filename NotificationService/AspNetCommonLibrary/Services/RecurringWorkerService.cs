using AspNetCommonLibrary.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCommonLibrary.Services
{
    public class RecurringWorkerService : IRecurringWorkerService
    {
        private ILogger Logger { get; set; }
        private IServiceProvider ServiceProvider { get; set; }

        public RecurringWorkerService(
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            ServiceProvider = serviceProvider;
        }

        public async Task ExecuteAsync<T>(Func<IServiceProvider, T> workerSelector, int normalRecurringTime, int errorRecurringTime, CancellationToken stoppingToken) where T : IRecurringWorker
        {
            try
            {
                Logger.LogInformation("RepetitiveWorkersBackgroundService - Запуск фонового процесса {typeName}", typeof(T).Name);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var executionSuccessful = await TryWorkAsync(workerSelector, stoppingToken);

                    if (executionSuccessful)
                    {
                        await Task.Delay(normalRecurringTime, stoppingToken);
                    }
                    else
                    {
                        await Task.Delay(errorRecurringTime, stoppingToken);
                    }
                }

                Logger.LogInformation("RepetitiveWorkersBackgroundService - Остановка фонового процесса {typeName}", typeof(T).Name);
            }
            catch (TaskCanceledException)
            {
                Logger.LogInformation("RepetitiveWorkersBackgroundService - Остановка фонового процесса {typeName}", typeof(T).Name);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "RepetitiveWorkersBackgroundService - Ошибка при запуске фонового процесса {typeName} | {ex}", typeof(T).Name, ex);
            }
        }

        private async Task<bool> TryWorkAsync<T>(Func<IServiceProvider, T> workerSelector, CancellationToken stoppingToken) where T : IRecurringWorker
        {
            using var scope = ServiceProvider.CreateAsyncScope();
            var worker = workerSelector.Invoke(scope.ServiceProvider);

            try
            {
                Logger.LogInformation("RepetitiveWorkersBackgroundService - Начало выполнения процесса {typeName}", typeof(T).Name);

                await worker.WorkAsync(stoppingToken);

                Logger.LogInformation("RepetitiveWorkersBackgroundService - Завершение выполнения процесса {typeName}", typeof(T).Name);

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "RepetitiveWorkersBackgroundService - Ошибка выполнения процесса {typeName} | {ex}", typeof(T).Name, ex);

                return false;
            }
        }
    }
}