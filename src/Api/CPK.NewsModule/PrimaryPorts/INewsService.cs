using System.Threading.Tasks;
using CPK.NewsModule.Dto;
using CPK.NewsModule.Entities;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.NewsModule.PrimaryPorts
{
    public interface INewsService
    {
        Task<int> Add(News request);
        Task<PageResult<ConcurrencyToken<News>>> Get(NewsFilter request);
        Task<int> Remove(ConcurrencyToken<Id> request);
        Task<int> Update(ConcurrencyToken<News> request);
    }
}