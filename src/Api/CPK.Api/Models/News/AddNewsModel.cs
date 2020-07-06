using System;

namespace CPK.Api.Models.News
{
    public sealed class AddNewsModel
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        public Guid ImageId { get; set; }
    }
}
