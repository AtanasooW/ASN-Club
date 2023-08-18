using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.ViewModels.Product.Contracts
{
    public interface IProductModel
    {
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
