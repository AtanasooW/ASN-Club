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
        public Task<ICollection<MyOrderViewModel>> GetMyOrdersByIdAsync(Guid userId);
        public Task<ICollection<MyOrderViewModel>> GetAllOrdersAsync();
        public Task<MyOrderDetailsViewModel> GetMyOrderDetailsByIdAsync(Guid userId, Guid id);
        public Task<MyOrderDetailsViewModel> GetOrderDetailByIdAsync(Guid id);
        public Task<OrderStatusViewModel> GetOrderStatusAsync(Guid id);
        Task EditOrderStatusAsync(OrderStatusViewModel model);
    }
}
