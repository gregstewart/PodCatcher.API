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
    public class PodcastRepository : IPodcastRepository
    {
        private CloudStorageAccount storageAccount;
        private CloudTable table;
        private CloudBlobContainer blob;

        public PodcastRepository()
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
//            storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            table = GetTable();
            blob = GetBlob();
        }

        private CloudBlobContainer GetBlob()
        {
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Create the blob if it doesn't exist.
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("podcasts");
            blobContainer.CreateIfNotExists();
            return blobContainer;
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
                var blobName = string.Format(@"podcast\{0}\{1}.json", podcastEntity.PartitionKey, podcastEntity.RowKey);
                var document = this.DownloadDocument(blobName);
                yield return JsonConvert.DeserializeObject<Podcast>(document);
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
                var blobName = string.Format(@"podcast\{0}\{1}.json", podcastEntity.PartitionKey, podcastEntity.RowKey);
                var document = this.DownloadDocument(blobName);
                return JsonConvert.DeserializeObject<Podcast>(document);
            }

            return null;
        }

        public Podcast Add(Podcast item)
        {
            PodcastEntity podcastEntity = new PodcastEntity(item.Title, item.Id);

            var document = JsonConvert.SerializeObject(item, Newtonsoft.Json.Formatting.Indented);

            UploadDocument(item.Title, item.Id.ToString(), document);
            
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

        private void UploadDocument(string partitionKey, string rowKey, string document)
        {
            var filename = string.Format(@"podcast\{0}\{1}.json", partitionKey, rowKey);
            var blockBlob = blob.GetBlockBlobReference(filename);

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(document)))
            {
                blockBlob.UploadFromStream(ms);
            }

            blockBlob.Properties.ContentType = "application/json";
            blockBlob.SetProperties();
        }

        private string DownloadDocument(string blobName)
        {
            var blockBlob = this.blob.GetBlockBlobReference(blobName);

            using (var memory = new MemoryStream())
            using (var reader = new StreamReader(memory))
            {
                blockBlob.DownloadToStream(memory);
                memory.Seek(0, SeekOrigin.Begin);

                return reader.ReadToEnd();
            }
        }
    }
}