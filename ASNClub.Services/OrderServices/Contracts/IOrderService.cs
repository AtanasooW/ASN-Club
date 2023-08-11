using ASNClub.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.OrderServices.Contracts
{
    public interface IOrderService
    {
        public Task<OrderViewModel> GetOrderByUserIdAsync(Guid userId);
        public Task PlaceOrderAsync(OrderViewModel model);
    }
}
