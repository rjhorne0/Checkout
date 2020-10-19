using System;
using System.IO;
using Checkout.Models;

namespace Checkout.PricingRules
{
    class AdditionalItemPricingRules : IGroceryItemPricingRules
    {
        public int Priority { get; set; }
        public PricingRulesType Type { get; set; }

        public AdditionalItemPricingRules()
        {
            Priority = 0;
            Type = PricingRulesType.SpecialRule;
        }

        public int CalculateTotal(GroceryItem item, int quantity, ref PricingSubtotal pricingSubtotal)
        {
            if (item.AdditionalItemPricing != null)
            {
                if (HasInvalidInput(item, quantity, ref pricingSubtotal))
                {
                    return -1;
                }

                int numGroups = quantity / 2;
                int numRemainderItems = quantity % 2;

                pricingSubtotal.AdditionalItemPricing = true;
                pricingSubtotal.AdditionalItemPrice = item.AdditionalItemPricing.Price;

                if (pricingSubtotal.OnSale != null && pricingSubtotal.OnSale == true)
                {
                    decimal groupPrice = pricingSubtotal.SalePrice + item.AdditionalItemPricing.Price;
                    pricingSubtotal.SubTotal = groupPrice * numGroups;
                    pricingSubtotal.SubTotal += pricingSubtotal.SalePrice * numRemainderItems;
                }
                else
                {
                    decimal groupPrice = pricingSubtotal.RegularPrice + item.AdditionalItemPricing.Price;
                    pricingSubtotal.SubTotal = groupPrice * numGroups;
                    pricingSubtotal.SubTotal += pricingSubtotal.RegularPrice * numRemainderItems;
                }

                return 0;
            }
            else
            {
                pricingSubtotal.AdditionalItemPricing = false;
            }

            return -1;
        }

        private bool HasInvalidInput(GroceryItem item, int quantity, ref PricingSubtotal pricingSubtotal)
        {
            TextWriter errorWriter = Console.Error;
            var errorString = "Error: Could not apply additional item pricing for {0}. ";

            if (quantity < 0)
            {
                errorWriter.WriteLine(errorString + "Invalid quantity: {1}", item.Name, quantity);
                return true;
            }

            if (item.AdditionalItemPricing.Price < 0)
            {
                errorWriter.WriteLine(errorString + "Invalid additional item price: {1}", item.Name, item.AdditionalItemPricing.Price);
                return true;
            }

            if (pricingSubtotal.RegularPrice < 0)
            {
                errorWriter.WriteLine(errorString + "Invalid regular price: {1}", item.Name, pricingSubtotal.RegularPrice);
                return true;
            }

            if (pricingSubtotal.OnSale != null && pricingSubtotal.OnSale == true && pricingSubtotal.SalePrice < 0)
            {
                errorWriter.WriteLine(errorString + "Invalid sale price: {1}", item.Name, pricingSubtotal.SalePrice);
                return true;
            }

            return false;
        }
    }
}
