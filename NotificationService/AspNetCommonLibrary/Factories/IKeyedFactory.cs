namespace AspNetCommonLibrary.Factories
{
    public interface IKeyedFactory<TKey, T>
        where TKey : notnull
        where T : class
    {
        T Get(TKey key);
    }
}
