using System;
using System.Collections.Generic;
using Checkout.Models;
using Checkout.FileReader;
using Checkout.PricingCalculator;
using Checkout.Receipt;

namespace Checkout
{
    class Program
    {
        static void Main(string[] args)
        {
            PricingTotal itemPrices;
            Dictionary<string, int> checkoutItems;
            var priceCalc = new PriceCalculator();

            checkoutItems = JSONandTXTFileReader.LoadItems();
            itemPrices = priceCalc.GetPriceOfItems(checkoutItems);

            ReceiptWriter.WriteReceipt(itemPrices);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}