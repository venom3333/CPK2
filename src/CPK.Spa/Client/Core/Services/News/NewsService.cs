using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Models.News;
using CPK.Spa.Client.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace CPK.Spa.Client.Core.Services.News
{
    public class NewsService : INewsService
    {
        private readonly IApiRepository _repository;
        private readonly ILogger<NewsService> _logger;
        private PageResultModel<NewsModel> _page;

        public NewsService(IApiRepository repository, ILogger<NewsService> logger)
        {
            _repository = repository;
            _logger = logger;
            _page = new PageResultModel<NewsModel>();
        }
        public string Error { get; private set; }
        public IReadOnlyList<NewsModel> List => _page?.Value?.AsReadOnly();
        public int TotalCount => _page?.TotalCount ?? 0;
        public async Task Load(NewsFilterModel filter)
        {
            _logger.LogDebug("LOAD CATEGORIES!");
            var (r, e) = await _repository.GetFilteredNews(filter);
            _page = r;
            Error = e;
        }

        public string ImageUri(Guid? id) {
            if (id == null) return "";
            return _repository.GetFullUrl($"files/{id}");
        }

        public async Task Create(NewsModel model)
        {
            var(result, error) = await _repository.CreateNews(model);
            ShowErrorIfNecessary(error);
        }

        public async Task Update(NewsModel model)
        {
            var(result, error) = await _repository.UpdateNews(model);
            ShowErrorIfNecessary(error);
        }

        public async Task Delete(NewsModel model)
        {
            var(result, error) = await _repository.RemoveNews(model.Id, model.Version);
            ShowErrorIfNecessary(error);
        }

        private void ShowErrorIfNecessary(string error) {
            if (!string.IsNullOrWhiteSpace(error)) Error = error;
        }

        public void SetError(string error)
        {
            Error = error;
        }
    }
}
