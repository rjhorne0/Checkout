# Checkout
A simple check out and receipt system

Should work out of the box, update the input files as desired.

InputFiles/Pricing/pricing.json defines the pricing model in simple json and is read in via json.NET.  Each item needs at least a Name and a RegularPricing to be valid, but can also define a SalePricing, GroupPricing and AdditionalItemPricing as desired (the fields for each match the classes in Models).  I assume the store would provide the pricing model as an excel sheet with an item on each row and regular price, sale price, etc. in columns which could be converted via .csv to .json to create the pricing.json file.

InputFiles/Items/items.txt defines the simple list of items to total up.  Currently it will only load in the one list and total it up, but it should be easy to modify to continue to run and scan in multiple item lists.

The FileReader will read in the list of items and pass them to the PriceCalculator which will refresh the pricing model before each transaction and then total up the items.

The pricing rules are defined in the PriceCalculators pricingRuleSet, with regular, sale, group and additional item currently enabled.

Each pricing rule implements IGroceryItemPricingRules which allows for easier addition of new rules.  Each rule sets a type - standard (price per item) or special (group prices, additional item) and a priority number.  I limited the calculator to only apply the highest priority special rule defined for each item as it would get complex trying to calculate multiple special rules for one item and doesn't seem like something a store would do.

Each pricing rule also implements its own CalculateTotal method which checks if the rule applies, calculates the subtotal and updates a pricing subtotal view model for that item.

Once each item has been subtotalled and the pricing total view model has been built, it is written to the console via the ReceiptWriter, which includes subtotal, regular price and a breakdown of any savings and discounts for each item, and a final total.

Unit testing is done via NUnit in PriceCalculatorTest which verifies the price calculator and each pricing rule is working as expected.  Any errors will be output to the console error log.
