using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.ViewModels.Order
{
    public class MyOrderViewModel
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal OrderTotal { get; set; }

        public string OrderStatus{ get; set; }
    }
}
