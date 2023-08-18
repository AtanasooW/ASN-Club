using ASNClub.ViewModels.Product.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Infrastructure.Extensions
{
    public static class ModelExtensions
    {
        public static string GetInformation(this IProductModel product)
        {
            return  product.Make.Replace(" ", "-") + "-" + product.Model.Replace(" ", "-");
        }
    }
}