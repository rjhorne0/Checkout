using Checkout.Models;

namespace Checkout.PricingRules
{
    interface IGroceryItemPricingRules
    {
        PricingRulesType Type { get; set; }
        int Priority { get; set; }
        
        // Each rule implements CalculateTotal which checks if the rule applies, calculates the subtotal for the item and updates a pricing subtotal view model
        int CalculateTotal(GroceryItem item, int quantity, ref PricingSubtotal pricingSubtotal);
    }

    enum PricingRulesType
    {
        StandardRule,  // Standard rules define the price per item
        SpecialRule    // Special rules define group pricing, buy one get next for x pricing, etc.  Only the highest priority special rule will apply for each item.
    }
}
