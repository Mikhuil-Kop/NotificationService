namespace AspNetCommonLibrary.Factories
{
    public class DelegateKeyedFactory<TKey, T> : KeyedFactoryBase<TKey, T>
        where TKey : notnull
        where T : class
    {
        private Func<IServiceProvider, TKey, T> CreateFunc { get; }

        public DelegateKeyedFactory(IServiceProvider serviceProvider, Func<IServiceProvider, TKey, T> createFunc) : base(serviceProvider)
        {
            CreateFunc = createFunc;
        }

        protected override T Create(IServiceProvider serviceProvider, TKey key)
        {
            return CreateFunc(serviceProvider, key);
        }
    }
}
