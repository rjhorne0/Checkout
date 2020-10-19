using System;
using System.IO;
using Checkout.Models;

namespace Checkout.PricingRules
{
    class SaleItemPricingRules : IGroceryItemPricingRules
    {
        public int Priority { get; set; }
        public PricingRulesType Type { get; set; }

        public SaleItemPricingRules()
        {
            Priority = 1;
            Type = PricingRulesType.StandardRule;
        }

        public int CalculateTotal(GroceryItem item, int quantity, ref PricingSubtotal pricingSubtotal)
        {
            if (item.SalePricing != null)
            {
                if (HasInvalidInput(item, quantity, ref pricingSubtotal))
                {
                    return -1;
                }

                var now = DateTime.Now;
                if (item.SalePricing.StartDate <= now && item.SalePricing.EndDate >= now)
                {
                    pricingSubtotal.OnSale = true;
                    pricingSubtotal.SubTotal = item.SalePricing.Price * quantity;
                    pricingSubtotal.SalePrice = item.SalePricing.Price;
                    pricingSubtotal.SaleStart = item.SalePricing.StartDate;
                    pricingSubtotal.SaleEnd = item.SalePricing.EndDate;
                }
                else
                {
                    pricingSubtotal.OnSale = false;
                }

                return 0;
            }

            return -1;
        }

        private bool HasInvalidInput(GroceryItem item, int quantity, ref PricingSubtotal pricingSubtotal)
        {
            TextWriter errorWriter = Console.Error;
            var errorString = "Error: Could not apply sale pricing for {0}. ";

            if (quantity < 0)
            {
                errorWriter.WriteLine(errorString + "Invalid quantity: {1}.", item.Name, quantity);
                return true;
            }

            if (item.SalePricing.Price < 0)
            {
                errorWriter.WriteLine(errorString + "Invalid sale price: {1}", item.Name, item.SalePricing.Price);
                return true;
            }

            return false;
        }
    }
}