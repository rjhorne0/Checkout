using System;

namespace Checkout.Models
{
    public class SaleItemPricing
    {
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}