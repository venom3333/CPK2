using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPK.Api.SecondaryAdapters.Dto;
using CPK.FilesModule.SecondaryPorts;
using CPK.NewsModule.Dto;
using CPK.NewsModule.Entities;
using CPK.NewsModule.SecondaryPorts;
using CPK.SharedModule;
using CPK.SharedModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace CPK.Api.SecondaryAdapters.Repositories
{
    internal sealed class NewsRepository : INewsRepository
    {
        private readonly CpkContext _context;
        private readonly IFilesRepository _files;

        public NewsRepository(CpkContext context, IFilesRepository files)
        {
            _context = context;
            _files = files;
        }

        public async Task<List<ConcurrencyToken<News>>> Get(NewsFilter newsFilter)
        {
            var query = Filter(newsFilter);
            query = Order(newsFilter, query);
            var news = await query
                .Skip((int) newsFilter.PageFilter.Skip)
                .Take((int) newsFilter.PageFilter.Take)
                .ToListAsync();
            return news.Select(item => item.ToNews()).ToList();
        }

        public async Task Add(News news)
        {
            await CheckForDoublesOrThrow(news);

            var p = new NewsDto(news, Guid.NewGuid().ToString());
            await _context.News.AddAsync(p);
        }

        private async Task CheckForDoublesOrThrow(News news)
        {
            if (await _context.News.AnyAsync(entity =>
                entity.Id != news.Id.Value && entity.Title == news.Title.Value))
            {
                throw new ApiException(ApiExceptionCode.EntityAlreadyExists, null,
                    "Категория с таким наименованием уже существует!");
            }
        }

        public async Task Update(ConcurrencyToken<News> news)
        {
            await CheckForDoublesOrThrow(news.Entity);
            var original = await _context.News.SingleAsync(p => p.Id == news.Entity.Id.Value);
            original.ShortDescription = news.Entity.ShortDescription.Value;
            original.Title = news.Entity.Title.Value;
            original.Text = news.Entity.Text.Value;

            var originalImageId = original.ImageId;

            original.ImageId = news.Entity.Image.Value;
            original.Updated = DateTime.Now;
            _context.UpdateWithToken<News, NewsDto, Guid>(news, original);

            if (news.Entity.Image.Value != originalImageId && originalImageId != null)
            {
                await _files.Remove(originalImageId.Value, original.Id);
            }
        }

        public async Task Remove(ConcurrencyToken<Id> id)
        {
            var original = await _context.News
                .SingleAsync(p => p.Id == id.Entity.Value);
            _context.DeleteWithToken<Id, NewsDto, Guid>(id, original);
            if (original.ImageId != null)
            {
                await _files.Remove(original.ImageId.Value, original.Id);
            }
        }

        public async Task<ConcurrencyToken<News>> Get(Id id)
        {
            var news = await _context.News.FindAsync(id.Value);
            if (news == null)
                return default;
            return news.ToNews();
        }

        public Task<int> Count(NewsFilter filter) => Filter(filter).CountAsync();

        private IQueryable<NewsDto> Filter(NewsFilter filter)
        {
            var query = _context.News.AsQueryable();
            if (filter.Id != default)
            {
                query = query.Where(x => x.Id == filter.Id);
            }
            else
            {
                query = string.IsNullOrWhiteSpace(filter.Title)
                    ? query
                    : query.Where(x => x.Title.Contains(filter.Title));
            }

            return query;
        }

        private IOrderedQueryable<NewsDto> Order(NewsFilter filter,
            IQueryable<NewsDto> query)
        {
            return filter.OrderBy switch
            {
                NewsOrderBy.Id => filter.Descending
                    ? query.OrderByDescending(x => x.Id)
                    : query.OrderBy(x => x.Id),
                NewsOrderBy.Title => filter.Descending
                    ? query.OrderByDescending(x => x.Title)
                    : query.OrderBy(x => x.Title),
                _ => throw new NotImplementedException()
            };
        }
    }
}