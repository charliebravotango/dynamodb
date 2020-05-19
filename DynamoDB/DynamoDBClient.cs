using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;

namespace DynamoDB
{
    internal class DynamoDBClient
    {
        private static AmazonDynamoDBClient client = null;

        internal static Amazon.DynamoDBv2.AmazonDynamoDBClient CreateClient(Amazon.RegionEndpoint region)
        {
            if (client == null)
            {
                client = new AmazonDynamoDBClient(region);
            }

            return client;
        } // CreateClient method

        internal static TableDescription CreateTable(string tableName, List<AttributeDefinition> tableAttributes, List<KeySchemaElement> schemaElements, ProvisionedThroughput provisionedThroughput)
        {
            TableDescription tableDescription = null;
            string partition_key_name = "ProductID";
            string sort_key_name = "Colour";


            // WARNING: only returns 100 tables maximum
            ListTablesResponse listTables = client.ListTables();
            if (!listTables.TableNames.Contains(tableName))
            {
                try
                {
                    CreateTableRequest request = new CreateTableRequest()
                    {
                        TableName = tableName,
                        AttributeDefinitions = tableAttributes,
                        KeySchema = schemaElements,
                        // Provisioned-throughput settings are always required,
                        // although the local test version of DynamoDB ignores them.
                        ProvisionedThroughput = provisionedThroughput
                    };


                    //////////////////////////////////////////////////////////////////////////////////////
                    //List<AttributeDefinition> lsiAttributes = new List<AttributeDefinition>
                    // { new AttributeDefinition { AttributeName = partition_key_name, AttributeType = "N" },
                    //   new AttributeDefinition{ AttributeName=sort_key_name, AttributeType="S" } };
                    //List<KeySchemaElement> lsiSchemaElements = new List<KeySchemaElement>
                    // { new KeySchemaElement { AttributeName = partition_key_name, KeyType = "HASH" },
                    //   new KeySchemaElement { AttributeName=sort_key_name, KeyType="RANGE" } };
                    //Projection projection = new Projection() { ProjectionType = "INCLUDE" };

                    //List<string> nonKeyAttributes = new List<string>();
                    //nonKeyAttributes.Add("Name");
                    //nonKeyAttributes.Add("Dscription");
                    //nonKeyAttributes.Add("Price");
                    //projection.NonKeyAttributes = nonKeyAttributes;

                    //LocalSecondaryIndex localSecondaryIndex = new LocalSecondaryIndex()
                    //{
                    //    IndexName = "ProductIDColourIndex",
                    //    KeySchema = lsiSchemaElements,
                    //    Projection = projection
                    //};

                    //List<LocalSecondaryIndex> localSecondaryIndexes = new List<LocalSecondaryIndex>();
                    //localSecondaryIndexes.Add(localSecondaryIndex);
                    //request.LocalSecondaryIndexes.Add(localSecondaryIndex);
                    //////////////////////////////////////////////////////////////////////////////////////

                    CreateTableResponse response = client.CreateTable(request);
                    tableDescription = response.TableDescription;
                } // try
                catch (Exception ex)
                {
                    // TODO Log the exception
                    Console.WriteLine(ex.Message);
                } // catch
            } // if
            else
            {
                DescribeTableResponse describeTable = client.DescribeTable(tableName);
                tableDescription = describeTable.Table;
            } // else
            return tableDescription;
        } // CreateTable method
    } // CynamoDBClient class
} // namespace
