using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Models.Products;

namespace CPK.Spa.Client.Core.Services
{
    public interface IBasketService
    {
        string Error { get; }
        IReadOnlyList<LineModel> Model { get; }
        long ItemsCount { get; }
        event EventHandler OnBasketItemsCountChanged;
        Task Load();
        Task Add(ProductModel product);
        Task Remove(ProductModel product);
        Task Clear();
    }
}