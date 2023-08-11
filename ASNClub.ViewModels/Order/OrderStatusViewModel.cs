using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.ViewModels.Order
{
    public class OrderStatusViewModel
    {
        public int? OrderStatusId { get; set; }

        public ICollection<OrderStatusModel> OrderStatuses { get; set; } = new HashSet<OrderStatusModel>();
        public Guid OrderId { get; set; }
    }
}
