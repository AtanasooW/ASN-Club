using ASNClub.Data.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Data.Configurations
{
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.HasData(this.GenerateOrderStatus());
        }
        private OrderStatus[] GenerateOrderStatus()
        {
            ICollection<OrderStatus> orderStatuses = new HashSet<OrderStatus>();

            OrderStatus orderstatus;

            orderstatus = new OrderStatus()
            {
                Id = 1,
                Status = "Awaiting approval"
            };
            orderStatuses.Add(orderstatus);

            orderstatus = new OrderStatus()
            {
                Id = 2,
                Status = "Confirmed"
            };
            orderStatuses.Add(orderstatus);

            orderstatus = new OrderStatus()
            {
                Id = 3,
                Status = "The order is packaged"
            };
            orderStatuses.Add(orderstatus);
            orderstatus = new OrderStatus()
            {
                Id = 4,
                Status = "The order has been delivered to a courier"
            };
            orderStatuses.Add(orderstatus);

            return orderStatuses.ToArray();
        }
    }
}
