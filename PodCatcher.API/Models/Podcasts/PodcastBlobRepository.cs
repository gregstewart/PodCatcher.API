using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public class PodcastBlobRepository : IPodcastRepository
    {
        private CloudStorageAccount storageAccount;
        private CloudBlobContainer blob;

        public PodcastBlobRepository()
        {
            // Retrieve the storage account from the connection string.
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
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

        public Podcast Get(Podcast podcast)
        {
            if (podcast != null)
            {
                var blobName = string.Format(@"Podcast\{0}\{1}.json", podcast.Title, podcast.Id.ToString());
                var document = this.DownloadDocument(blobName);
                return JsonConvert.DeserializeObject<Podcast>(document);
            }

            return null;
        }

        public Podcast Get(Guid Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Podcast> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Podcast> GetAll(IEnumerable<Podcast> podcasts)
        {
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            
            foreach (var podcast in podcasts)
            {
                var blobName = string.Format(@"PodcastFeed\{0}\{1}.json", podcast.Title, podcast.Id.ToString());
                var document = this.DownloadDocument(blobName);
                yield return JsonConvert.DeserializeObject<Podcast>(document);
            }
        }

        public Podcast Add(Podcast podcast)
        {
            var document = JsonConvert.SerializeObject(podcast, Newtonsoft.Json.Formatting.Indented);

            UploadDocument(podcast.Title, podcast.Id.ToString(), document);

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

        private void UploadDocument(string partitionKey, string rowKey, string document)
        {
            var filename = string.Format(@"Podcast\{0}\{1}.json", partitionKey, rowKey);
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