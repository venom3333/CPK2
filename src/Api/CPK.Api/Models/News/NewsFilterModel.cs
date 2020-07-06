using System;
using CPK.NewsModule.Dto;
using CPK.ProductCategoriesModule.Dto;

namespace CPK.Api.Models.News
{
    public sealed class NewsFilterModel
    {
        public Guid Id { get; set; }
        public uint Take { get; set; }
        public uint Skip { get; set; }
        public string Title { get; set; }
        
        public bool Descending { get; set; }
        public NewsOrderBy OrderBy { get; set; }
    }
}
