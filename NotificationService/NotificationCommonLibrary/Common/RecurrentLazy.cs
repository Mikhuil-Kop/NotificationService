namespace NotificationCommonLibrary.Common
{
    /// <summary>
    /// Реализует ленивую загрузку данных, а также при их запросе запускает фоновый процесс,
    /// раз в N милисекунд выполняет повторную загрузку. Также отслеживает ошибки при инициализации данных.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class RecurrentLazy<T> : IDisposable where T : class
    {
        private Func<Task<T>> ValueFactory { get; }
        public int RepeatingTime { get; }
        private CancellationTokenSource CancellationTokenSource { get; }
        private object Locker { get; }

        private T? LoadValue { get; set; }
        private Exception? LoadException { get; set; }
        private Task? LoadTask { get; set; }
        private bool Loaded { get; set; }

        public T Value
        {
            get
            {
                lock (Locker)
                {
                    if (!Loaded)
                    {
                        try
                        {
                            var value = ValueFactory.Invoke().Result;

                            LoadValue = value;
                            LoadException = null;
                        }
                        catch (Exception e)
                        {
                            LoadValue = null;
                            LoadException = e;
                        }

                        LoadTask = Task.Run(() => ReloadAsync(RepeatingTime, CancellationTokenSource.Token));

                        Loaded = true;
                    }

                    if (LoadException != null)
                    {
                        throw new Exception("Ошибка загрузки данных", LoadException);
                    }

                    return LoadValue!;
                }
            }
        }

        public RecurrentLazy(Func<Task<T>> valueFactory, int repeatingTime)
        {
            ValueFactory = valueFactory;
            RepeatingTime = repeatingTime;

            CancellationTokenSource = new CancellationTokenSource();
            Locker = new object();
        }

        private async Task ReloadAsync(int repeatingTime, CancellationToken token)
        {
            await Task.Delay(repeatingTime, token);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var value = await ValueFactory.Invoke();

                    lock (Locker)
                    {
                        LoadValue = value;
                        LoadException = null;
                        Loaded = true;
                    }
                }
                catch (Exception e)
                {
                    lock (Locker)
                    {
                        LoadValue = null;
                        LoadException = e;
                        Loaded = true;
                    }
                }

                await Task.Delay(repeatingTime, token);
            }
        }
 
        public void Dispose()
        {
            CancellationTokenSource.Cancel();
        }
    }
}
