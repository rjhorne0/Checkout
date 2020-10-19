using System;
using System.IO;
using Checkout.Models;

namespace Checkout.Receipt
{
    public static class ReceiptWriter
    {
        public static void WriteReceipt(PricingTotal pricingTotal)
        {
            if (pricingTotal.Items.Count <= 0)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine("No items to display");
                return;
            }

            foreach (var item in pricingTotal.Items)
            {
                bool dealsMsgDisplayed = false;
                    
                decimal savings = (item.RegularPrice * item.Quantity) - item.SubTotal;

                Console.Write("{0}x {1,-25}{2,20:C}", item.Quantity, item.Name, item.SubTotal);
                Console.WriteLine();
                Console.Write(" - Regular price (each) {0,24:C} ", item.RegularPrice);

                if (item.AdditionalItemPricing != null && item.AdditionalItemPricing == true)
                {
                    if (!dealsMsgDisplayed)
                    {
                        dealsMsgDisplayed = displayDealsMsg(savings);
                    }

                    Console.WriteLine();
                    if (item.AdditionalItemPrice > 0)
                    {
                        Console.Write(" - Discount: Buy one, get the next for {0:C}", item.AdditionalItemPrice);
                    }
                    else
                    {
                        Console.Write(" - Discount: Buy one, get the next for free");
                    }
                }

                if (item.GroupPricing != null && item.GroupPricing == true)
                {
                    if (!dealsMsgDisplayed)
                    {
                        dealsMsgDisplayed = displayDealsMsg(savings);
                    }

                    Console.WriteLine();
                    Console.Write(" - Discount: Buy {0} for {1:C}", item.GroupSize, item.GroupPrice);
                }

                if (item.OnSale != null && item.OnSale == true)
                {
                    if (!dealsMsgDisplayed)
                    {
                        dealsMsgDisplayed = displayDealsMsg(savings);
                    }
                    Console.WriteLine();
                    Console.Write(" - Discount: On sale for {0:C} until {1:d}", item.SalePrice, item.SaleEnd);
                }

                Console.WriteLine();
            }
                
            Console.WriteLine(("").PadRight(48, '-'));
            Console.WriteLine("Total:{0,42:C}\n", pricingTotal.Total);
        }

        public static bool displayDealsMsg(decimal savings)
        {
            string dealsMsg = " - You saved {0,35:C}";

            Console.WriteLine();
            Console.Write(dealsMsg, savings);

            return true;
        }
    }
}
