using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Checkout.Models;
using Checkout.FileReader;
using Checkout.PricingRules;

namespace Checkout.PricingCalculator
{
    class PriceCalculator
    {
        public Dictionary<string, GroceryItem> pricingModel = new Dictionary<string, GroceryItem>();
        public List<IGroceryItemPricingRules> pricingRuleSet = new List<IGroceryItemPricingRules>();
        public RegularItemPricingRules regularPricingRules = new RegularItemPricingRules();
        public SaleItemPricingRules salePricingRules = new SaleItemPricingRules();
        public GroupItemPricingRules groupPricingRules = new GroupItemPricingRules();
        public AdditionalItemPricingRules additionalItemPricingRules = new AdditionalItemPricingRules();

        public PriceCalculator()
        {
            ConfigurePricingRules();
            UpdatePricingModel();
        }

        public PriceCalculator(Dictionary<string, GroceryItem> pricingRules)
        {
            ConfigurePricingRules();
            UpdatePricingModel(pricingRules);
        }

        public PricingTotal GetPriceOfItems(Dictionary<string, int> checkoutItems, bool refreshPrices = true)
        {
            if (refreshPrices)
            {
                ConfigurePricingRules();
                UpdatePricingModel();
            }

            PricingTotal priceTotal = new PricingTotal()
            {
                Total = 0m,
                Items = new List<PricingSubtotal>()
            };

            if (checkoutItems != null && checkoutItems.Count > 0 &&
                pricingModel != null && pricingModel.Count > 0)
            {
                foreach (var item in checkoutItems)
                {
                    if (pricingModel.ContainsKey(item.Key))
                    {
                        var calculatedPrice = CalculatePrice(pricingModel[item.Key], item.Value);
                        priceTotal.Items.Add(calculatedPrice);
                        priceTotal.Total += calculatedPrice.SubTotal;
                    }
                }
            }

            return priceTotal;
        }

        private PricingSubtotal CalculatePrice(GroceryItem item, int quantity)
        {
            PricingSubtotal priceView = new PricingSubtotal() { Name = item.Name, RegularPrice = 0m, Quantity = quantity, SubTotal = 0m };

            foreach (var rule in pricingRuleSet)
            {
                var error = rule.CalculateTotal(item, quantity, ref priceView);

                if (rule.Type == PricingRulesType.SpecialRule && error == 0)
                {
                    break;
                }
            }

            return priceView;
        }

        private void ConfigurePricingRules()
        {
            pricingRuleSet = new List<IGroceryItemPricingRules>
            {
                regularPricingRules,
                salePricingRules,
                groupPricingRules,
                additionalItemPricingRules
            };

            // Order rules by priority, standard (price per item) rules first, then special (additional item, group prices) rules.
            // Only the highest priority special rule will apply for each item.

            var regularPricingRuleSet = pricingRuleSet.Where(x => x.Type == PricingRulesType.StandardRule).OrderByDescending(x => x.Priority);
            var specialPricingRuleSet = pricingRuleSet.Where(x => x.Type == PricingRulesType.SpecialRule).OrderByDescending(x => x.Priority);

            pricingRuleSet = regularPricingRuleSet.ToList();
            pricingRuleSet = pricingRuleSet.Concat(specialPricingRuleSet.ToList()).ToList();
        }

        private void UpdatePricingModel()
        {
            pricingModel = RemoveInvalidItemPricing(JSONandTXTFileReader.LoadPricing());
        }

        private void UpdatePricingModel(Dictionary<string, GroceryItem> pricingRules)
        {
            pricingModel = RemoveInvalidItemPricing(pricingRules);
        }

        private Dictionary<string, GroceryItem> RemoveInvalidItemPricing(Dictionary<string, GroceryItem> pm)
        {
            // Item pricing needs at least a name and a standard price to be defined

            if (pm != null && pm.Count > 0)
            {
                List<string> invalid = new List<string>();

                foreach (var item in pm)
                {
                    if (item.Value.Name == null)
                    {
                        invalid.Add(item.Key);
                    }
                    else if (item.Value.RegularPricing == null)
                    {
                        invalid.Add(item.Key);
                    }
                }

                foreach (var item in invalid)
                {
                    pm.Remove(item);
                }
            }

            return pm;
        }
    }
}
