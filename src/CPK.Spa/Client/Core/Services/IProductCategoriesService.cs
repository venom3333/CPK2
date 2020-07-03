using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CPK.Spa.Client.Core.Models;

namespace CPK.Spa.Client.Core.Services
{
    public interface IProductCategoriesService
    {
        string Error { get; }
        IReadOnlyList<ProductCategoryModel> List { get; }
        Task Create(ProductCategoryModel model);
        Task Update(ProductCategoryModel model);
        Task Delete(ProductCategoryModel model);
        int TotalCount { get; }
        Task Load(ProductCategoriesFilterModel filter);
        string ImageUri(Guid imageId);
    }
}