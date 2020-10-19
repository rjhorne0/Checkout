using System;

namespace Checkout.Models
{
    public class PricingSubtotal
    {
        public string Name { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime SaleStart { get; set; }
        public DateTime SaleEnd { get; set; }
        public bool? OnSale { get; set; }
        public decimal AdditionalItemPrice { get; set; }
        public bool? AdditionalItemPricing { get; set; }
        public int GroupSize { get; set; }
        public decimal GroupPrice { get; set; }
        public bool? GroupPricing { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}