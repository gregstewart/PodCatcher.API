using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public class PodcastTableRepository : IPodcastRepository
    {
        private CloudTable _cloudTable;
        TableFactory tableFactory = new TableFactory();
            
        public PodcastTableRepository()
        {
            _cloudTable = tableFactory.GetTable("podcasts"); ;
        }

        public IEnumerable<Podcast> GetAll()
        {
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<PodcastEntity> query = new TableQuery<PodcastEntity>();

            var podcastEntities = _cloudTable.ExecuteQuery(query).ToList();

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
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Id.ToString()));

            var podcastEntity = _cloudTable.ExecuteQuery(query).SingleOrDefault();
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
            _cloudTable.Execute(insertOperation);

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