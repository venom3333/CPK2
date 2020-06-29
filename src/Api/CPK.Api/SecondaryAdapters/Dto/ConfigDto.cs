namespace CPK.Api.SecondaryAdapters.Dto
{
    public sealed class ConfigDto : IConfig
    {
        public string ApiName { get; set; }
        public string Authority { get; set; }
        public int MaxBasketSize { get; set; }
        public string ConnectionString { get; set; }
        public string IdentityUrlExternal { get; set; }
        public string FilesDir { get; set; }
    }
}
