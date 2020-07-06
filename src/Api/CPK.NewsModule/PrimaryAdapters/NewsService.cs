using System;
using System.Threading.Tasks;
using CPK.NewsModule.Dto;
using CPK.NewsModule.Entities;
using CPK.NewsModule.PrimaryPorts;
using CPK.NewsModule.SecondaryPorts;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.NewsModule.PrimaryAdapters
{
    public sealed class NewsService : INewsService
    {
        private readonly INewsUow _uow;

        public NewsService(INewsUow uow)
        {
            _uow = uow;
        }

        public async Task<int> Add(News request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            await _uow.Repository.Add(request);
            return await _uow.SaveAsync();
        }

        public async Task<PageResult<ConcurrencyToken<News>>> Get(NewsFilter request)
        {
            if (request == default)
                throw new ArgumentOutOfRangeException(nameof(request));
            var news = await _uow.Repository.Get(request);
            var total = await _uow.Repository.Count(request);
            return new PageResult<ConcurrencyToken<News>>(request.PageFilter, news, (uint)total);
        }

        public async Task<int> Remove(ConcurrencyToken<Id> request)
        {
            if (request.Entity == default || string.IsNullOrWhiteSpace(request.Token))
                throw new ArgumentNullException(nameof(request));
            await _uow.Repository.Remove(request);
            var count = await _uow.SaveAsync();
            return count;
        }

        public async Task<int> Update(ConcurrencyToken<News> request)
        {
            if (request.Entity == null || string.IsNullOrWhiteSpace(request.Token))
                throw new ArgumentNullException(nameof(request));
            await _uow.Repository.Update(request);
            var count = await _uow.SaveAsync();
            return count;
        }
    }
}
