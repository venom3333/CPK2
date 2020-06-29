﻿using System;

namespace CPK.Api.Models
{
    public sealed class AddProductModel
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Guid ImageId { get; set; }
    }
}
