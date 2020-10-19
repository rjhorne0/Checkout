using System;
using System.IO;
using Checkout.Models;

namespace Checkout.PricingRules
{
    class GroupItemPricingRules : IGroceryItemPricingRules
    {
        public int Priority { get; set; }
        public PricingRulesType Type { get; set; }

        public GroupItemPricingRules()
        {
            Priority = 1;
            Type = PricingRulesType.SpecialRule;
        }

        public int CalculateTotal(GroceryItem item, int quantity, ref PricingSubtotal pricingSubtotal)
        {
            if (item.GroupPricing != null)
            {
                if (HasInvalidInput(item, quantity, ref pricingSubtotal))
                {
                    return -1;
                }

                int numGroups = quantity / item.GroupPricing.GroupSize;
                int numRemainderItems = quantity % item.GroupPricing.GroupSize;

                pricingSubtotal.GroupPricing = true;
                pricingSubtotal.GroupSize = item.GroupPricing.GroupSize;
                pricingSubtotal.GroupPrice = item.GroupPricing.Price;

                if (pricingSubtotal.OnSale != null && pricingSubtotal.OnSale == true)
                {
                    pricingSubtotal.SubTotal = item.GroupPricing.Price * numGroups;
                    pricingSubtotal.SubTotal += pricingSubtotal.SalePrice * numRemainderItems;
                }
                else
                {
                    pricingSubtotal.SubTotal = item.GroupPricing.Price * numGroups;
                    pricingSubtotal.SubTotal += pricingSubtotal.RegularPrice * numRemainderItems;
                }

                return 0;
            }
            else
            {
                pricingSubtotal.GroupPricing = false;
            }

            return -1;
        }

        private bool HasInvalidInput(GroceryItem item, int quantity, ref PricingSubtotal pricingSubtotal)
        {
            TextWriter errorWriter = Console.Error;
            var errorString = "Error: Could not apply group pricing for {0}. ";

            if (quantity < 0)
            {
                errorWriter.WriteLine(errorString + "Invalid quantity: {1}.", item.Name, quantity);
                return true;
            }

            if (item.GroupPricing.GroupSize <= 0)
            {
                errorWriter.WriteLine(errorString + "Invalid group size: {1}", item.Name, item.GroupPricing.GroupSize);
                return true;
            }

            if (item.GroupPricing.Price < 0)
            {
                errorWriter.WriteLine(errorString + "Invalid group price: {1}", item.Name, item.GroupPricing.Price);
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
