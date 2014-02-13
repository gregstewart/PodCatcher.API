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
using PodCatcher.API.Models.Podcasts;

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

        public IEnumerable<Podcast> GetAll(IEnumerable<Podcast> podcasts)
        {
            throw new NotImplementedException();
        }

        public Podcast Get(Podcast podcast)
        {
            throw new NotImplementedException();
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

        public Podcast Add(Podcast podcast)
        {
            PodcastEntity podcastEntity = new PodcastEntity(podcast);

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(podcastEntity);

            // Execute the insert operation.
            table.Execute(insertOperation);

            return podcast;
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