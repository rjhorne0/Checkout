using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Checkout.Models;
using Checkout.PricingCalculator;

namespace Checkout.Testing
{
    [TestFixture]
    public class PriceCalculatorTest
    {
        // Models for CalculateRegularPrices()

        private Dictionary<string, GroceryItem> regularPricingModel;
        private PriceCalculator RegularPriceCalc;

        private Dictionary<string, int> standardCheckoutItems;
        private PricingTotal standardItemPrices;

        // Models for CalculateSalePrices()

        private Dictionary<string, GroceryItem> salePricingModel;
        private PriceCalculator salePriceCalc;

        private Dictionary<string, int> saleCheckoutItems;
        private PricingTotal saleItemPrices;

        // Models for CalculateGroupPrices()

        private Dictionary<string, GroceryItem> groupPricingModel;
        private PriceCalculator groupPriceCalc;

        private Dictionary<string, int> groupCheckoutItems;
        private PricingTotal groupItemPrices;

        // Models for CalculateAdditionalItemPrices()

        private Dictionary<string, GroceryItem> additionalItemPricingModel;
        private PriceCalculator additionalItemPriceCalc;

        private Dictionary<string, int> additionalItemCheckoutItems;
        private PricingTotal additionalItemPrices;

        [SetUp]
        protected void SetUp()
        {
            // Setup for CalculateRegularPrices()

            regularPricingModel = new Dictionary<string, GroceryItem>()
            {
                { "Apple", new GroceryItem() { Name = "Apple",
                    RegularPricing = new RegularItemPricing() { Price = 1.00m }
                }},
                { "Banana", new GroceryItem() { Name = "Banana",
                    RegularPricing = new RegularItemPricing() { Price = 2.00m }
                }}
            };
            RegularPriceCalc = new PriceCalculator(regularPricingModel);
    
            standardCheckoutItems = new Dictionary<string, int>()
            {
                { "Apple", 3 },
                { "Banana", 2 }
            };
            standardItemPrices = new PricingTotal();

            // Setup for CalculateSalePrices()

            salePricingModel = new Dictionary<string, GroceryItem>()
            {
                { "Apple", new GroceryItem() { Name = "Apple",
                    RegularPricing = new RegularItemPricing() { Price = 1.00m },
                    SalePricing = new SaleItemPricing() { Price = 0.75m, StartDate = DateTime.Parse("2020/10/01 00:00:00"), EndDate = DateTime.Parse("2020/10/31 00:00:00") }
                }},
                { "Banana", new GroceryItem() { Name = "Banana",
                    RegularPricing = new RegularItemPricing() { Price = 2.00m },
                    SalePricing = new SaleItemPricing() { Price = 1.5m, StartDate = DateTime.Parse("2020/10/07 00:00:00"), EndDate = DateTime.Parse("2020/10/30 00:00:00") }
                }}
            };
            salePriceCalc = new PriceCalculator(salePricingModel);

            saleCheckoutItems = new Dictionary<string, int>()
            {
                { "Apple", 3 },
                { "Banana", 2 }
            };
            saleItemPrices = new PricingTotal();

            // Setup for CalculateGroupPrices()

            groupPricingModel = new Dictionary<string, GroceryItem>()
            {
                { "Apple", new GroceryItem() { Name = "Apple",
                    RegularPricing = new RegularItemPricing() { Price = 1.00m },
                    GroupPricing = new GroupItemPricing() { GroupSize = 3, Price = 2.00m }
                }},
                { "Banana", new GroceryItem() { Name = "Banana",
                    RegularPricing = new RegularItemPricing() { Price = 2.00m },
                    GroupPricing = new GroupItemPricing() { GroupSize = 2, Price = 3.00m }
                }}
            };
            groupPriceCalc = new PriceCalculator(groupPricingModel);

            groupCheckoutItems = new Dictionary<string, int>()
            {
                { "Apple", 4 },
                { "Banana", 4 }
            };
            groupItemPrices = new PricingTotal();

            // Setup for CalculateAdditionalItemPrices()

            additionalItemPricingModel = new Dictionary<string, GroceryItem>()
            {
                { "Apple", new GroceryItem() { Name = "Apple",
                    RegularPricing = new RegularItemPricing() { Price = 1.00m },
                    AdditionalItemPricing = new AdditionalItemPricing() { Price = 0m }
                }},
                { "Banana", new GroceryItem() { Name = "Banana",
                    RegularPricing = new RegularItemPricing() { Price = 2.00m },
                    AdditionalItemPricing = new AdditionalItemPricing() { Price = 1.00m }
                }}
            };
            additionalItemPriceCalc = new PriceCalculator(additionalItemPricingModel);

            additionalItemCheckoutItems = new Dictionary<string, int>()
            {
                { "Apple", 4 },
                { "Banana", 3 }
            };
            additionalItemPrices = new PricingTotal();

        }

        [Test]
        public void CalculateRegularPrices()
        {
            decimal expectedTotal = 7m; // 3 Apples at $1, 2 Bananas at $2 totals to $7
            standardItemPrices = RegularPriceCalc.GetPriceOfItems(standardCheckoutItems, false);
            Assert.AreEqual(expectedTotal, standardItemPrices.Total);
        }

        [Test]
        public void CalculateSalePrices()
        {
            decimal expectedTotal = 5.25m; // 3 Apples on sale at $0.75, 2 Bananas on sale at $1.50 totals to $5.25
            saleItemPrices = salePriceCalc.GetPriceOfItems(saleCheckoutItems, false);
            Assert.AreEqual(expectedTotal, saleItemPrices.Total);
        }

        [Test]
        public void CalculateGroupPrices()
        {
            decimal expectedTotal = 9m; // 4 Apples on sale at 3 for $2, $1 per additional.  4 Bananas on sale at 2 for $3, $2 per additional.  Totals to $9
            groupItemPrices = groupPriceCalc.GetPriceOfItems(groupCheckoutItems, false);
            Assert.AreEqual(expectedTotal, groupItemPrices.Total);
        }

        [Test]
        public void CalculateAdditionalItemPrices()
        {
            decimal expectedTotal = 7m; // 4 Apples on sale at $1 each, BOGO.  3 Bananas on sale $2 each, buy one get the next for $1.  Totals to $7
            additionalItemPrices = additionalItemPriceCalc.GetPriceOfItems(additionalItemCheckoutItems, false);
            Assert.AreEqual(expectedTotal, additionalItemPrices.Total);
        }
    }
}
