namespace AspNetCommonLibrary.Factories
{
    public abstract class KeyedFactoryBase<TKey, T> : IDisposable, IKeyedFactory<TKey, T>
        where TKey : notnull
        where T : class
    {
        private IServiceProvider ServiceProvider { get; }
        private Dictionary<TKey, T> CreatedServices { get; }

        public KeyedFactoryBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            CreatedServices = new Dictionary<TKey, T>();
        }

        protected abstract T Create(IServiceProvider serviceProvider, TKey key);

        public T Get(TKey key)
        {
            try
            {
                lock (CreatedServices)
                {
                    if (!CreatedServices.TryGetValue(key, out var service))
                    {
                        service = Create(ServiceProvider, key);
                        CreatedServices.Add(key, service);
                    }

                    return service;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении объекта с типом " + typeof(T).Name, ex);
            }
        }

        public void Dispose()
        {
            foreach (var value in CreatedServices.Values)
            {
                if (value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
