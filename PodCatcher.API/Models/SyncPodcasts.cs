using System.Collections.Generic;
using System.Linq;
using PodCatcher.API.Models.Episodes;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public class SyncPodcasts
    {
        private static IPodcastRepository podcastTableRepository;
        private static IEpisodeRepository episodeTableRepository;
        private static IPodcastBuilder podcastBuilder;
        private static ILogger logger; 

        public SyncPodcasts()
        {
            podcastTableRepository = PodcastTableRepositoryFactory.Create();
            episodeTableRepository = EpisodeTableRepositoryFactory.Create();
            podcastBuilder = PodcastBuilderFactory.Create();
            logger = LoggerFactory.Create();
        }

        public void Sync()
        {
            logger.Debug("Start syncing podcasts");
            IEnumerable<Podcast> podcasts = GetStoredPodcasts();

            foreach (var podcast in podcasts)
            {
                logger.Debug("Syncing podcast: " + podcast.Title);
                IEnumerable<Episode> episodes = GetStoredEpisodes(podcast);
                Episode lastEpisode = GetLatestStoredEpisode(episodes);
                logger.Debug("Fetch podcast feed");
                PodcastFeed updatedPodcastFeed = GetFeed(podcast);
                logger.Debug("Fetched podcast feed");
                logger.Debug("Number of episodes to process: " + updatedPodcastFeed.Episodes.Count());
                if (updatedPodcastFeed != null && updatedPodcastFeed.Episodes.Any())
                {
                    List<Episode> newEpisodes = GetDelta(lastEpisode, updatedPodcastFeed.Episodes.ToList());
                    logger.Debug("Number of episodes to store: " + newEpisodes.Count());
                    if (newEpisodes.Any())
                    {
                        UpdateEpisodes(podcast, newEpisodes);
                    }
                }
                
            }
            logger.Debug("End syncing podcasts");
        }

        public virtual void UpdateEpisodes(Podcast podcast, List<Episode> newEpisodes)
        {
            episodeTableRepository.Add(podcast, newEpisodes);
        }

        public virtual List<Episode> GetDelta(Episode podcastEpisode, List<Episode> podcastEpisodes)
        {
            if (podcastEpisode != null)
            {
                var delta = new List<Episode>();
                delta.AddRange(podcastEpisodes.Where(episode => episode.PublicationDate > podcastEpisode.PublicationDate));

                return delta;    
            }

            return podcastEpisodes;

        }

        public virtual IEnumerable<Podcast> GetStoredPodcasts()
        {
            return podcastTableRepository.GetAll();
        }

        public virtual IEnumerable<Episode> GetStoredEpisodes(Podcast podcast)
        {
            return episodeTableRepository.GetAll(podcast.Id);
        }

        public virtual Episode GetLatestStoredEpisode(IEnumerable<Episode> episodes)
        {
            if (episodes.Any()) { 
                return episodes.ElementAt(0);
            }

            return null;
        }

        public virtual PodcastFeed GetFeed(Podcast podcast)
        {
            return podcastBuilder.Build(podcast);  
        }
    }
}