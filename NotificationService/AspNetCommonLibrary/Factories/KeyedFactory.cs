namespace AspNetCommonLibrary.Factories
{
    public class TypedKeyedFactory<TKey, T> : IKeyedFactory<TKey, T>
        where TKey : notnull
        where T : class
    {
        private IServiceProvider ServiceProvider { get; set; }
        private Dictionary<TKey, Type> TypesMap { get; set; }

        public TypedKeyedFactory(IServiceProvider serviceProvider, Dictionary<TKey, Type> typesMap)
        {
            ServiceProvider = serviceProvider;
            TypesMap = typesMap;
        }

        public T Get(TKey key)
        {
            if (!TypesMap.TryGetValue(key, out var type))
            {
                throw new KeyNotFoundException($"Ключ {key} не обнаружен в списке доступных для {GetType().Name}.");
            }

            var service = ServiceProvider.GetService(type);

            if (service is null)
            {
                throw new KeyNotFoundException($"Сервис с типом {type.Name} не обнаружен в списке зарегестрированных сервисов.");
            }

            if (service is not T)
            {
                throw new Exception($"Сервис с типом {service.GetType().Name} не соответствует ожидаемому типу {typeof(T).Name}.");
            }

            return (service as T)!;
        }
    }
}
