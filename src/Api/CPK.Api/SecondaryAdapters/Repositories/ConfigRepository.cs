using CPK.Api.SecondaryAdapters.Dto;
using CPK.BasketModule.SecondaryPorts;

namespace CPK.Api.SecondaryAdapters.Repositories
{
    internal sealed class ConfigRepository : IConfigRepository
    {
        private readonly IConfig _config;

        public ConfigRepository(IConfig config)
        {
            _config = config;
        }

        public int GetMaxBasketSize() => _config.MaxBasketSize;
    }
}
