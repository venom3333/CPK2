using System;
using CPK.SharedModule;
using CPK.SharedModule.Entities;

namespace CPK.Api.Models.News
{
    public sealed class NewsModel
    {
        public Guid Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        public Guid? ImageId { get; set; }

        public NewsModel()
        {

        }

        public NewsModel(ConcurrencyToken<NewsModule.Entities.News> news)
        {
            Id = news.Entity.Id.Value;
            Version = news.Token;
            Title = news.Entity.Title.Value;
            ShortDescription = news.Entity.ShortDescription.Value;
            ImageId = news.Entity.Image.Value;
            Text = news.Entity.Text.Value;
        }

        public ConcurrencyToken<NewsModule.Entities.News> ToNews() =>
            new ConcurrencyToken<NewsModule.Entities.News>(
                Version,
                new NewsModule.Entities.News(
                    new Id(Id),
                    new Title(Title),
                    new ShortDescription(ShortDescription),
                    new Text(Text), 
                    new Image(ImageId)));
    }
}
