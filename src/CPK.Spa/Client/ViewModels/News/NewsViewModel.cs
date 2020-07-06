﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlazorInputFile;
using CPK.Spa.Client.Core.Models;
using CPK.Spa.Client.Core.Models.News;
using CPK.Spa.Client.Core.Services;
using CPK.Spa.Client.Core.Services.News;
using CPK.Spa.Client.Models;
using CPK.Spa.Client.ViewModels.News;
using MatBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace CPK.Spa.Client.ViewModels.News
{
    public class NewsViewModel
    {
        private readonly IJSRuntime _js;
        private readonly INewsService _service;
        private readonly IFileService _fileService;
        private readonly AuthenticationStateProvider _auth;

        public NewsViewModel(IJSRuntime js, INewsService service, IFileService fileService, AuthenticationStateProvider auth)
        {
            _js = js;
            _service = service;
            _fileService = fileService;
            _auth = auth;
            FilterFormEditContext = new EditContext(this);
        }

        public async Task OnInitializedAsync()
        {
            FilterFormEditContext = new EditContext(this);
            await LoadFromServerAsync();
        }

        public PaginatorModel Paginator { get; } = new PaginatorModel()
        {
            Size = 5,
            CurrentPage = 0,
            ItemsPerPage = 10,
            ItemsTotalCount = 0
        };

        public string Id { get; set; }

        [Required(ErrorMessage = "Введите наименование")]
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        // [Required(ErrorMessage = "Прикрепите изображение")]
        public Guid? ImageId { get; set; }
        public int Skip => Paginator.ItemsPerPage * Paginator.CurrentPage;
        public int Take => Paginator.ItemsPerPage;
        public NewsOrderBy Current { get; set; } = NewsOrderBy.Id;
        public EditContext FilterFormEditContext { get; private set; }
        public NewsFilterModel Filter { get; set; } = new NewsFilterModel()
        {
            OrderBy = NewsOrderBy.Id
        };
        public Dictionary<NewsOrderBy, SortableTableHeaderModel> TableHeaderModel { get; } = new Dictionary<NewsOrderBy, SortableTableHeaderModel>()
        {
            [NewsOrderBy.Title] = new SortableTableHeaderModel()
        };
        public string Error => _service.Error;
        public IReadOnlyList<NewsModel> List => _service.List;

        public NewsViewModelState State;

        public void SetError(string error)
        {
            _service.SetError(error);
        }

        public void ChangeState(string value)
        {
            State = NewsViewModelState.View;
            if (string.IsNullOrWhiteSpace(value))
                return;
            if (Enum.TryParse(value, true, out NewsViewModelState state))
                State = state;
        }

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

        public async Task Create(NewsModel model)
        {
            await _service.Create(model);
            return;
        }

        public async Task Update(NewsModel model)
        {
            await _service.Update(model);
            return;
        }

        public async Task Delete(NewsModel model)
        {
            await _service.Delete(model);
            return;
        }

        public async Task UploadImage(IMatFileUploadEntry entry)
        {
            using (var stream = new MemoryStream())
            {
                await entry.WriteToStreamAsync(stream);

                FileModel model = new FileModel
                {
                    ContentType = entry.Type,
                    Content = stream,
                    FileName = entry.Name,
                    Size = entry.Size
                };

                var result = await _fileService.Upload(entry);
                ImageId = result;
            }
            return;
        }

        public async Task<Guid> UploadImage(IFileListEntry entry)
        {
            FileModel model = new FileModel
            {
                ContentType = entry.Type,
                Content = entry.Data,
                FileName = entry.Name,
                Size = entry.Size
            };

            var result = await _fileService.Upload(model);
            return result;
        }

        public async Task HandleSort(NewsOrderBy order)
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

        public string ImageUrl(Guid? imageId) => _service.ImageUri(imageId);

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

        public void ClearViewModelFields()
        {
            Id = default;
            Title = default;
            ShortDescription = default;
            ImageId = default;
            Text = default;
        }
    }
}
