using System;
using System.Collections.Generic;

using CPK.BasketModule.Entities;
using CPK.NewsModule.Entities;
using CPK.OrdersModule.Entities;
using CPK.ProductCategoriesModule.Entities;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.Api.SecondaryAdapters.Dto
{
    public sealed class NewsDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        public Guid? ImageId { get; set; }

        public FileDto Image { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public NewsDto()
        {

        }
        public NewsDto(Guid id, string token, string title, string shortDescription, string text, Guid? imageId)
        {
            Id = id;
            ConcurrencyToken = token;
            Title = title;
            ShortDescription = shortDescription;
            ImageId = imageId;
            Text = text;
        }

        public ConcurrencyToken<News> ToNews() =>
            new ConcurrencyToken<News>(ConcurrencyToken, new News(new Id(Id), new Title(Title), new ShortDescription(ShortDescription), new Text(Text), new Image(ImageId)));

        public NewsDto(News news, string version) :
            this(news.Id.Value, version, news.Title.Value, news.ShortDescription.Value, news.Text.Value, news.Image.Value)
        {

        }
    }
}
