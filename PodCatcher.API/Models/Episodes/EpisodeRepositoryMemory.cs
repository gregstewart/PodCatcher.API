using System;
using System.Collections.Generic;
using System.Linq;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models.Episodes
{
    public class EpisodeRepositoryMemory : IEpisodeRepository
    {
        private readonly List<Episode> _episodes = new List<Episode>();

        public IEnumerable<Episode> GetAll()
        {
            return _episodes;
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
            if (episode == null)
            {
                throw new ArgumentNullException("episode");
            }
            _episodes.Add(episode);
        }

        public void Add(Podcast podcast, Episode episode)
        {
            throw new NotImplementedException();
        }

        public void Add(Podcast podcast, IEnumerable<Episode> episodes)
        {
            if (episodes.Count() > 1)
            {
                foreach (var episode in episodes)
                {
                    _episodes.Add(episode);
                }
            }
        }
    }
}