using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace DynamoDB
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region Program-wide variables
            RegionEndpoint region = RegionEndpoint.APSoutheast2;
            AmazonDynamoDBClient dBClient = null;

            // table-related variables
            string tableName = "Product";
            TableDescription tableDescription = null;
            string partition_key_name = "ProductID";
            List<AttributeDefinition> tableAttributes = new List<AttributeDefinition> { new AttributeDefinition { AttributeName = partition_key_name, AttributeType = "N" } };
            List<KeySchemaElement> schemaElements = new List<KeySchemaElement> { new KeySchemaElement { AttributeName = partition_key_name, KeyType = "HASH" } };

            // Limit provisioned throughput to save costs (for demonstration)
            ProvisionedThroughput provisionedThroughput = new ProvisionedThroughput(1, 1);
            #endregion

            #region Create DynamoDB client
            Console.Write("Creating a DynamoDB client...");
            try
            {
                dBClient = DynamoDBClient.CreateClient(region);
                Console.WriteLine("done!");
            } // try
            catch (AmazonDynamoDBException ex)
            {
                Console.WriteLine($"!!!!!!!\nEXCEPTION thrown: {ex.Message}");
            } // catch 
            #endregion

            #region Create the table
            Console.Write("Creating the table...");
            try
            {
                tableDescription = DynamoDBClient.CreateTable(tableName, tableAttributes, schemaElements, provisionedThroughput);
                Console.WriteLine("done!");
            } // try
            catch (Exception ex)
            {
                Console.WriteLine($"!!!!!!!\nEXCEPTION thrown: {ex.Message}");
            }// catch
            HitENTERtoContinue();
            #endregion

            #region Generate some random products for the table
            Console.Write("Generating some randome products...");
            List<Product> products = Product.GenerateSomeProducts(100);
            Console.WriteLine("done!");
            #endregion

            #region Write random products to database
            Console.WriteLine("Writing products to the table...");
            Table table = Table.LoadTable(dBClient, tableName);
            if (tableDescription.ItemCount < 100)
            {
                foreach (Product product in products)
                {
                    string json = JsonConvert.SerializeObject(product);
                    Document doc = Document.FromJson(json);
                    table.PutItem(doc);
                    Console.Write(".");
                } // foreach

            }
            Console.WriteLine();
            HitENTERtoContinue();
            #endregion

            #region Find one product, based on the ProductID
            int productID = products[1].ProductID;
            Console.WriteLine($"Finding productID {productID}...");
            Primitive hash = new Primitive(productID.ToString(), true);
            Document found1 = table.GetItem(hash);
            if (found1 != null)
            {
                Console.WriteLine(found1.ToJsonPretty());
            } // if
            HitENTERtoContinue();
            #endregion

            #region Find the blue products
            string colour = "Blue";
            Console.Write($"Scan for the '{colour}' products...");
            ComparisonOperator @operator = ComparisonOperator.EQ;
            ScanFilter filter = new ScanFilter();
            filter.AddCondition("Colour", ScanOperator.Equal, new DynamoDBEntry[] { colour });
            ScanOperationConfig scanConfig = new ScanOperationConfig
            {
                Filter = filter
            };
            Search search = table.Scan(scanConfig);
            List<Document> docList = new List<Document>();
            docList = search.GetNextSet();
            Console.WriteLine("done!");

            if (docList.Count > 0)
            {
                foreach (Document item in docList)
                {
                    Console.WriteLine(item.ToJsonPretty());
                } // foreach
            } // if
            HitENTERtoContinue();
            #endregion

        } // Main method

        private static void HitENTERtoContinue()
        {
            Console.WriteLine("Hit ENTER to continue");
            Console.ReadLine();
        } // HitENTERtoContinue method
    } // Program class
} // namespace
