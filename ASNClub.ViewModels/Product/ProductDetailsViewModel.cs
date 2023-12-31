﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASNClub.ViewModels.Discount;
using ASNClub.ViewModels.Comment;
using ASNClub.ViewModels.Color;
using ASNClub.ViewModels.Product.Contracts;

namespace ASNClub.ViewModels.Product
{
    public class ProductDetailsViewModel : IProductModel
    {
        public ProductDetailsViewModel()
        {
            this.ImgUrls = new List<string>();
        }
        public int Id { get; set; }
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int? TypeId { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; } = 0.0;
        public ProductDiscountFormModel Discount { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public string Material { get; set; } = null!;
        public List<string> ImgUrls { get; set; }
        public string? Color { get; set; }
        public List<ColorProductIdViewModel>? Colors { get; set; }
        public List<CommentViewModel> Comments { get; set; } = null!;
    }
}
