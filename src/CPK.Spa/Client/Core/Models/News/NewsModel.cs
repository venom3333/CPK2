using System;

namespace CPK.Spa.Client.Core.Models.News
{
    public sealed class NewsModel
    {
        public Guid Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        public Guid? ImageId { get; set; }
    }
}