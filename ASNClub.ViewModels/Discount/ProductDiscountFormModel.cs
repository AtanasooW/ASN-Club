using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.ViewModels.Discount
{
    public class ProductDiscountFormModel
    {
      
        public bool IsDiscount { get; set; } = false;

        public double? DiscountRate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
