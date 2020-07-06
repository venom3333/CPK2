using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.NewsModule.Dto;
using CPK.NewsModule.Entities;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.NewsModule.SecondaryPorts
{
    public interface INewsRepository
    {
        Task<List<ConcurrencyToken<News>>> Get(NewsFilter newsFilter);
        Task Add(News news);
        Task Update(ConcurrencyToken<News> news);
        Task Remove(ConcurrencyToken<Id> id);
        Task<ConcurrencyToken<News>> Get(Id id);
        Task<int> Count(NewsFilter newsFilter);
    }
}