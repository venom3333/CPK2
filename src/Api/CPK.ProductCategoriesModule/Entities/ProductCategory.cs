﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using CPK.SharedModule.Entities;
using FluentValidationGuard;

namespace CPK.ProductCategoriesModule.Entities
{
    public class ProductCategory
    {
        public Id Id { get; }
        public Title Title { get; }
        public ShortDescription ShortDescription { get; }
        public Image Image { get; }
        
        public ProductCategory(Title title, ShortDescription shortDescription, Image image) : this(new Id(Guid.NewGuid()), title, shortDescription, image) { }
        
        public ProductCategory(Id id, Title title, ShortDescription shortDescription, Image image)
        {
            Id = id;
            Title = title;
            ShortDescription = shortDescription;
            Image = image;
            Validator
                .Begin(id, nameof(id))
                .NotDefault()
                .Map(title, nameof(title))
                .NotDefault()
                .ThrowApiException(nameof(ProductCategory), nameof(ProductCategory));
        }
    }
}
