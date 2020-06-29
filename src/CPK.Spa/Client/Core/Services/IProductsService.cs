using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;

namespace CPK.Spa.Client.Core.Services
{
    public interface IProductsService
    {
        string Error { get; }
        IReadOnlyList<ProductModel> Model { get; }
        int TotalCount { get; }
        Task Load(ProductsFilterModel filter);
        string ImageUri(ProductModel product);
    }
}