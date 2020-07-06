using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CPK.Spa.Client.Attributes;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Models.Products;
using CPK.Spa.Client.Core.Services;
using CPK.Spa.Client.Core.Services.Products;
using CPK.Spa.Client.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace CPK.Spa.Client.ViewModels.Products
{
    public class ProductsViewModel
    {
        private readonly IJSRuntime _js;
        private readonly IProductsService _products;
        private readonly IBasketService _basket;
        private readonly AuthenticationStateProvider _auth;

        public ProductsViewModel(IJSRuntime js, IProductsService products, IBasketService basket, AuthenticationStateProvider auth)
        {
            _js = js;
            _products = products;
            _basket = basket;
            _auth = auth;
            FilterFormEditContext = new EditContext(this);
        }

        public async Task OnInitializedAsync()
        {
            await LoadFromServerAsync();
            if (await IsAuth())
                await _basket.Load();
        }

        public PaginatorModel Paginator { get; } = new PaginatorModel()
        {
            Size = 5,
            CurrentPage = 0,
            ItemsPerPage = 10,
            ItemsTotalCount = 0
        };
        
        [StringLength(30, ErrorMessage = "Название продукта должно быть короче 30 символов")]
        public string Title { get; set; }
        [GreaterOrEqualTo(nameof(Min), "0")]
        public decimal MinPrice { get; set; } = 0m;
        public decimal Min => 0m;
        [GreaterOrEqualTo(nameof(MinPrice), "Min Price")]
        public decimal? MaxPrice { get; set; }
        public int Skip => Paginator.ItemsPerPage * Paginator.CurrentPage;
        public int Take => Paginator.ItemsPerPage;
        public ProductOrderBy Current { get; set; } = ProductOrderBy.Id;
        public EditContext FilterFormEditContext { get; }
        public ProductsFilterModel Filter { get; set; } = new ProductsFilterModel()
        {
            OrderBy = ProductOrderBy.Id,
            MaxPrice = decimal.MaxValue
        };
        public Dictionary<ProductOrderBy, SortableTableHeaderModel> TableHeaderModel { get; } = new Dictionary<ProductOrderBy, SortableTableHeaderModel>()
        {
            [ProductOrderBy.Title] = new SortableTableHeaderModel(),
            [ProductOrderBy.Price] = new SortableTableHeaderModel(),
        };
        public string Error => _products.Error + _basket.Error;
        public IReadOnlyList<ProductModel> Model => _products.Model;

        public async Task AddToBasket(ProductModel product)
        {
            if (!await IsAuth())
            {
                await _js.InvokeVoidAsync(
                     "alert",
                     "Пожалуйста авторизутесь в системе для добавления товаров в корзину. Кнопка для входа в верхнем правом углу."
                     );
                return;
            }
            await _basket.Add(product);
        }

        public async Task HandleValidSubmit()
        {
            Paginator.CurrentPage = 0;
            Filter.MinPrice = MinPrice;
            Filter.MaxPrice = MaxPrice ?? decimal.MaxValue;
            Filter.Title = Title;
            await LoadFromServerAsync();
        }

        public async Task LoadPage()
        {
            await LoadFromServerAsync();
        }

        public async Task HandleSort(ProductOrderBy order)
        {
            foreach (var kv in TableHeaderModel)
            {
                if (kv.Key == order)
                {
                    kv.Value.IsActive = true;
                }
                else
                {
                    kv.Value.IsActive = false;
                }
            }
            Current = order;
            await LoadFromServerAsync();
        }

        public string ImageUrl(ProductModel product) => _products.ImageUri(product);

        private async Task<bool> IsAuth()
        {
            var state = await _auth.GetAuthenticationStateAsync();
            return state?.User?.Identity?.IsAuthenticated ?? false;
        }

        private async Task LoadFromServerAsync()
        {
            Filter.Descending = TableHeaderModel.Any(x => x.Value.IsActive && x.Value.Descending);
            Filter.OrderBy = Current;
            Filter.Skip = Skip;
            Filter.Take = Take;
            await _products.Load(Filter);
            Paginator.ItemsTotalCount = _products.TotalCount;
        }
    }
}
