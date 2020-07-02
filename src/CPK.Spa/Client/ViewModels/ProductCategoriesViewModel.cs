using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using CPK.Spa.Client.Attributes;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Services;
using CPK.Spa.Client.Models;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace CPK.Spa.Client.ViewModels
{
    public class ProductCategoriesViewModel
    {
        private readonly IJSRuntime _js;
        private readonly IProductCategoriesService _service;
        private readonly AuthenticationStateProvider _auth;

        public ProductCategoriesViewModel(IJSRuntime js, IProductCategoriesService service, AuthenticationStateProvider auth)
        {
            _js = js;
            _service = service;
            _auth = auth;
            FilterFormEditContext = new EditContext(this);
        }

        public async Task OnInitializedAsync()
        {
              await LoadFromServerAsync();
        }

        public PaginatorModel Paginator { get; } = new PaginatorModel()
        {
            Size = 5,
            CurrentPage = 0,
            ItemsPerPage = 10,
            ItemsTotalCount = 0
        };

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public int Skip => Paginator.ItemsPerPage * Paginator.CurrentPage;
        public int Take => Paginator.ItemsPerPage;
        public ProductCategoryOrderBy Current { get; set; } = ProductCategoryOrderBy.Id;
        public EditContext FilterFormEditContext { get; }
        public ProductCategoriesFilterModel Filter { get; set; } = new ProductCategoriesFilterModel()
        {
            OrderBy = ProductCategoryOrderBy.Id
        };
        public Dictionary<ProductCategoryOrderBy, SortableTableHeaderModel> TableHeaderModel { get; } = new Dictionary<ProductCategoryOrderBy, SortableTableHeaderModel>()
        {
            [ProductCategoryOrderBy.Title] = new SortableTableHeaderModel()
        };
        public string Error => _service.Error;
        public IReadOnlyList<ProductCategoryModel> Model => _service.Model;

        public async Task HandleValidSubmit()
        {
            Paginator.CurrentPage = 0;
            Filter.Title = Title;
            await LoadFromServerAsync();
        }

        public async Task LoadPage()
        {
            await LoadFromServerAsync();
        }

        public async Task Edit(ProductCategoryModel model, MouseEventArgs mouseEventArgs = null)
        {
            Console.WriteLine(model.Title);
            await Task.CompletedTask;
            return;
        }

        public async Task HandleSort(ProductCategoryOrderBy order)
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

        public string ImageUrl(ProductCategoryModel model) => _service.ImageUri(model);

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
            await _service.Load(Filter);
            Paginator.ItemsTotalCount = _service.TotalCount;
        }
    }
}
