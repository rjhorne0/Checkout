using System;
using System.Collections.Generic;
using System.IO;
using Checkout.Models;
using Newtonsoft.Json;

namespace Checkout.FileReader
{
    public static class JSONandTXTFileReader
    {
        const string baseFilename = @"..\..\InputFiles\";
        const string pricingFilename = @"Pricing/pricing.json";
        const string itemsFilename = @"Items/items.txt";

        public static Dictionary<string, GroceryItem> LoadPricing()
        {
            List<GroceryItem> items = new List<GroceryItem>();
            Dictionary<string, GroceryItem> itemDict = new Dictionary<string, GroceryItem>(
                StringComparer.InvariantCultureIgnoreCase);

            try
            {
                using (StreamReader r = new StreamReader(baseFilename + pricingFilename))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<List<GroceryItem>>(json);
                }
            }
            catch (Exception ex)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine("Error trying to read input files: {0}", ex.Message);
            }

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (item != null && item.Name != null)
                    {
                        itemDict.Add(item.Name, item);
                    }
                }

                return itemDict;
            }

            return null;
        }

        public static Dictionary<string, int> LoadItems()
        {
            Dictionary<string, int> itemDict = new Dictionary<string, int>(
                StringComparer.InvariantCultureIgnoreCase);

            string[] lines = null;

            try
            {
                lines = File.ReadAllLines(baseFilename + itemsFilename);
            }
            catch (Exception ex)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine("Error trying to read input files: {0}", ex.Message);
            }

            if (lines != null && lines.Length > 0)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] != null && lines[i].Length > 0)
                    {
                        if (itemDict.ContainsKey(lines[i]))
                        {
                            itemDict[lines[i]]++;
                        }
                        else
                        {
                            itemDict[lines[i]] = 1;
                        }
                    }
                }

                return itemDict;
            }

            return null;
        }
    }
}
