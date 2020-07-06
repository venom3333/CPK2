using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models.News;

namespace CPK.Spa.Client.Core.Services.News
{
    public interface INewsService
    {
        string Error { get; }
        IReadOnlyList<NewsModel> List { get; }
        Task Create(NewsModel model);
        Task Update(NewsModel model);
        Task Delete(NewsModel model);
        int TotalCount { get; }
        Task Load(NewsFilterModel filter);
        string ImageUri(Guid? imageId);
        void SetError(string error);
    }
}