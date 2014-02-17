using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models.Episodes
{
    public class EpisodeTableRepository : IEpisodeRepository
    {
        private CloudTable _cloudTable;
        private TableFactory tableFactory = new TableFactory();

        public EpisodeTableRepository()
        {
            _cloudTable = tableFactory.GetTable("episodes"); ;
        }

        public IEnumerable<Episode> GetAll()
        {
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<EpisodeEntity> query = new TableQuery<EpisodeEntity>();

            var episodeEntities = _cloudTable.ExecuteQuery(query).ToList();

            foreach (var episodeEntity in episodeEntities)
            {
                yield return new Episode
                {
                    Id = episodeEntity.Id,
                    Title = episodeEntity.Title,
                    Link = episodeEntity.Link,
                    Comments = episodeEntity.Comments,
                    PublicationDate = episodeEntity.PublicationDate,
                    PermaLink = episodeEntity.PermaLink,
                    Description = episodeEntity.Description,
                    Subtitle = episodeEntity.Subtitle,
                    Summary = episodeEntity.Summary,
                    Author = episodeEntity.Author,
                    Explicit = episodeEntity.Explicit,
                    Duration = episodeEntity.Duration,
                    MediaLink = episodeEntity.MediaLink,
                    MediaDuration = episodeEntity.MediaDuration,
                    MediaType = episodeEntity.MediaType,
                };
            }
 
        }

        public IEnumerable<Episode> GetAll(string podcastTitle)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Episode> GetAll(Guid podcastGuid)
        {
            throw new NotImplementedException();
        }

        public void Add(Episode episode)
        {
            throw new NotImplementedException();
        }

        public void Add(Podcast podcast, Episode episode)
        {
            EpisodeEntity episodeEntity = new EpisodeEntity(podcast, episode);

            // Create the TableOperation that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(episodeEntity);

            // Execute the insert operation.
            _cloudTable.Execute(insertOperation);

        }

        public void Add(Podcast podcast, IEnumerable<Episode> episodes)
        {
            if (episodes.Count() > 1)
            {
                foreach (var episode in episodes)
                {
                    this.Add(podcast, episode);
                }
            }
        }
    }

    
}