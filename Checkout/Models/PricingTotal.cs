using System.Collections.Generic;

namespace Checkout.Models
{
    public class PricingTotal
    {
        public decimal Total { get; set; }
        public List<PricingSubtotal> Items { get; set; }
    }
}