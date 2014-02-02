using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace PodCatcher.API.Models
{
    public class PodcastRepository : IPodcastRepository
    {
        private CloudStorageAccount storageAccount;
        private CloudTable table;
        public PodcastRepository()
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            table = GetTable();
        }

        private CloudTable GetTable()
        {
// Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference("podcasts");
            table.CreateIfNotExists();
            return table;
        }

        public IEnumerable<Podcast> GetAll()
        {
            table = GetTable();
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<Podcast> query = new TableQuery<Podcast>();

            var podcasts = table.ExecuteQuery(query);
            return podcasts;
        }

        public Podcast Get(Guid Id)
        {
            // Create a retrieve operation that takes a customer entity.
            TableQuery<Podcast> query = new TableQuery<Podcast>().Where(
                TableQuery.GenerateFilterCondition("Id", QueryComparisons.Equal, Id.ToString()));

            var podcast = table.ExecuteQuery(query);
            
            return podcast.First();
        }

        public Podcast Add(Podcast item)
        {
            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(item);

            // Execute the insert operation.
            table.Execute(insertOperation);

            return item;
        }

        public void Remove(Guid Id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Podcast item)
        {
            throw new NotImplementedException();
        }
    }
}