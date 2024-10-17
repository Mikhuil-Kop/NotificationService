using AspNetCommonLibrary.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCommonLibrary.Extensions
{
    public static class DIExtensions
    {
        #region Регистрация фабрик

        public static void AddSingletonKeyedFactory<TKey, T>(this IServiceCollection collection, Func<IServiceProvider, TKey, T> createFunction) where TKey : notnull where T : class
        {
            collection.AddSingleton<IKeyedFactory<TKey, T>>((s) =>
            {
                return new DelegateKeyedFactory<TKey, T>(s, createFunction);
            });
        }

        public static void AddScopedKeyedFactory<TKey, T>(this IServiceCollection collection, Func<IServiceProvider, TKey, T> createFunction) where TKey : notnull where T : class
        {
            collection.AddScoped<IKeyedFactory<TKey, T>>((s) =>
            {
                return new DelegateKeyedFactory<TKey, T>(s, createFunction);
            });
        }

        public static void AddTransientKeyedFactory<TKey, T>(this IServiceCollection collection, Func<IServiceProvider, TKey, T> createFunction) where TKey : notnull where T : class
        {
            collection.AddTransient<IKeyedFactory<TKey, T>>((s) =>
            {
                return new DelegateKeyedFactory<TKey, T>(s, createFunction);
            });
        }

        public static void AddSingletonKeyedFactory<TKey, T>(this IServiceCollection collection, Dictionary<TKey, Type> typesMap) where TKey : notnull where T : class
        {
            collection.AddSingleton<IKeyedFactory<TKey, T>>((s) =>
            {
                return new TypedKeyedFactory<TKey, T>(s, typesMap);
            });
        }

        public static void AddScopedKeyedFactory<TKey, T>(this IServiceCollection collection, Dictionary<TKey, Type> typesMap) where TKey : notnull where T : class
        {
            collection.AddScoped<IKeyedFactory<TKey, T>>((s) =>
            {
                return new TypedKeyedFactory<TKey, T>(s, typesMap);
            });
        }

        public static void AddTransientKeyedFactory<TKey, T>(this IServiceCollection collection, Dictionary<TKey, Type> typesMap) where TKey : notnull where T : class
        {
            collection.AddTransient<IKeyedFactory<TKey, T>>((s) =>
            {
                return new TypedKeyedFactory<TKey, T>(s, typesMap);
            });
        }

        #endregion

        #region Получение из фабрик

        public static T GetRequiredServiceByKey<TKey, T>(this IServiceProvider serviceProvider, TKey key) where TKey : notnull where T : class
        {
            return serviceProvider.GetRequiredService<IKeyedFactory<TKey, T>>().Get(key);
        }

        #endregion
    }
}
