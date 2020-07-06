using System;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.NewsModule.Entities
{
    public class News
    {
        public Id Id { get; }
        public Title Title { get; }
        public ShortDescription ShortDescription { get; }
        
        public Text Text { get; }
        public Image Image { get; }
        
        public News(Title title, ShortDescription shortDescription, Text text, Image image) : this(new Id(Guid.NewGuid()), title, shortDescription, text, image) { }
        
        public News(Id id, Title title, ShortDescription shortDescription, Text text, Image image)
        {
            Id = id;
            Title = title;
            ShortDescription = shortDescription;
            Image = image;
            Text = text;
            Validator
                .Begin(id, nameof(id))
                .NotDefault()
                .Map(title, nameof(title))
                .NotDefault()
                .ThrowApiException(nameof(News), nameof(News));
        }
    }
}
