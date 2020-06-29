namespace CPK.Api.SecondaryAdapters.Dto
{
    public interface IConfig
    {
        string ApiName { get; }
        string Authority { get; }
        string ConnectionString { get; }
        int MaxBasketSize { get; }
        string IdentityUrlExternal { get; }
        string FilesDir { get; }
    }
}
