using System;
using System.IO;
using Checkout.Models;

namespace Checkout.PricingRules
{
    class RegularItemPricingRules : IGroceryItemPricingRules
    {
        public int Priority { get; set; }
        public PricingRulesType Type { get; set; }

        public RegularItemPricingRules()
        {
            Priority = 0;
            Type = PricingRulesType.StandardRule;
        }

        public int CalculateTotal(GroceryItem item, int quantity, ref PricingSubtotal pricingSubtotal)
        {
            if (item.RegularPricing != null)
            {
                if (HasInvalidInput(item, quantity, ref pricingSubtotal))
                {
                    return -1;
                }

                if (pricingSubtotal.OnSale == null || pricingSubtotal.OnSale == false)
                {
                    pricingSubtotal.SubTotal = item.RegularPricing.Price * quantity;
                }

                pricingSubtotal.RegularPrice = item.RegularPricing.Price;

                return 0;
            }

            return -1;
        }

        private bool HasInvalidInput(GroceryItem item, int quantity, ref PricingSubtotal pricingSubtotal)
        {
            TextWriter errorWriter = Console.Error;
            var errorString = "Error: Could not apply regular pricing for {0}. ";

            if (quantity < 0)
            {
                errorWriter.WriteLine(errorString + "Invalid quantity: {1}.", item.Name, quantity);
                return true;
            }

            return false;
        }
    }
}