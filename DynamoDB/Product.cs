using System;
using System.Collections.Generic;

namespace DynamoDB
{
    [Serializable]
    public class Product
    {
        // backing variable for the product id
        protected int mProductID;
        private static Random random = new Random();

        public Product()
        {

            mProductID = random.Next(1000, Int32.MaxValue);
        } // constructor

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Colour { get; set; }
        public int ProductID => mProductID;

        internal static List<Product> GenerateSomeProducts(int quantity)
        {
            List<Product> products = new List<Product>(quantity);
            string[] colours = { "Red", "Yellow", "Pink", "Green", "Orange", "Purple", "Blue", "Black", "White", "Magenta" };
            string[] names = { "Widget", "Sprocket", "Bracket", "Flange", "Hinge", "Gudgeon", "Screw", "Cog", "Rivet", "Bolt" };
            for (int i = 0; i < quantity; i++)
            {
                int index = random.Next(0, 9);
                int index2 = random.Next(0, 9);
                string colour = colours[index];
                string name = $"{colour} {names[index]}-{names[index2]}";
                string description = $"Description for the {name} product.";
                decimal price = (decimal)random.NextDouble() * 1000M;
                Product p = new Product() { Name = name, Colour = colour, Description = description, Price = price };
                products.Add(p);
            } // for
            return products;
        } // GenerateSomeProducts method
    } // Product class
} // namespace
