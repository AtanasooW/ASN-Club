using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.ViewModels.ShoppingCart
{
    public class ShoppingCartViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public ICollection<ShoppingCartItemViewModel> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItemViewModel>();
    }
}
