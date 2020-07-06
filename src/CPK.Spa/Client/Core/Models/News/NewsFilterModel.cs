using System;

namespace CPK.Spa.Client.Core.Models.News
{
    public sealed class NewsFilterModel
    {
        public Guid Id { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public string Title { get; set; }

        public bool Descending { get; set; }
        public NewsOrderBy OrderBy { get; set; }
    }
}