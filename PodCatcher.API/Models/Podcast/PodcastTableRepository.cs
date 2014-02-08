using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace PodCatcher.API.Models
{
    public class PodcastTableRepository : IPodcastRepository
    {
        private CloudStorageAccount storageAccount;
        private CloudTable table;

        public PodcastTableRepository()
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
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<PodcastEntity> query = new TableQuery<PodcastEntity>();

            var podcastEntities = table.ExecuteQuery(query).ToList();

            foreach (var podcastEntity in podcastEntities)
            {
//                var blobName = string.Format(@"podcast\{0}\{1}.json", podcastEntity.PartitionKey, podcastEntity.RowKey);
//                var document = this.DownloadDocument(blobName);
//                yield return JsonConvert.DeserializeObject<Podcast>(document);
                yield return new Podcast
                {
                    Id = podcastEntity.Id,
                    Title = podcastEntity.Title,
                    Image = podcastEntity.Image,
                    Summary = podcastEntity.Summary,
                    Uri = podcastEntity.Uri
                };
            }
        }

        public Podcast Get(Guid Id)
        {
            // Create a retrieve operation that takes a customer entity.
            TableQuery<PodcastEntity> query = new TableQuery<PodcastEntity>().Where(
                TableQuery.GenerateFilterCondition("Id", QueryComparisons.Equal, Id.ToString()));

            var podcastEntity = table.ExecuteQuery(query).SingleOrDefault();
            // handle null scenario document

            if (podcastEntity != null)
            {
                return new Podcast
                {
                    Id = podcastEntity.Id,
                    Title = podcastEntity.Title,
                    Image = podcastEntity.Image,
                    Summary = podcastEntity.Summary,
                    Uri = podcastEntity.Uri
                };
            }
            else
            {
                return null;
            }
            
        }

        public Podcast Add(Podcast item)
        {
            PodcastEntity podcastEntity = new PodcastEntity(item.Title, item.Id);

            var document = JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented);

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(podcastEntity);

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