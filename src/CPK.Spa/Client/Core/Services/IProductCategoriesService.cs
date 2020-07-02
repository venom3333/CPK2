using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;

namespace CPK.Spa.Client.Core.Services
{
    public interface IProductCategoriesService
    {
        string Error { get; }
        IReadOnlyList<ProductCategoryModel> Model { get; }
        int TotalCount { get; }
        Task Load(ProductCategoriesFilterModel filter);
        string ImageUri(ProductCategoryModel model);
    }
}