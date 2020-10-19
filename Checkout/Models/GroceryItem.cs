namespace Checkout.Models
{
    public class GroceryItem
    {
        public string Name { get; set; }

        public RegularItemPricing RegularPricing { get; set; }
        public SaleItemPricing SalePricing { get; set; }
        public GroupItemPricing GroupPricing { get; set; }
        public AdditionalItemPricing AdditionalItemPricing { get; set; }
    }
}