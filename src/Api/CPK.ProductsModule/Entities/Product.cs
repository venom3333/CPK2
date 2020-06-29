﻿using System;
using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.ProductsModule.Entities
{
    public class Product
    {
        public Product(Title title, Money price, Image image) : this(new ProductId(Guid.NewGuid()), title, price, image) { }

        public Product(ProductId id, Title title, Money price, Image image)
        {
            Id = id;
            Title = title;
            Price = price;
            Image = image;
            Validator
                .Begin(id, nameof(id))
                .NotDefault()
                .Map(title, nameof(title))
                .NotDefault()
                .Map(price, nameof(price))
                .NotDefault()
                .Map(image, nameof(image))
                .NotDefault()
                .ThrowApiException(nameof(Product), nameof(Product));
        }

        public ProductId Id { get; }
        public Title Title { get; }
        public Money Price { get; }
        public Image Image { get; }
    }
}
