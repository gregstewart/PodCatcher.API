using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Episodes;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Tests.Stubs
{
    class EpisodeRepositoryStub : IEpisodeRepository
    {
        public Episode EpisodeToBeReturned;
        public Exception ToBeThrown;

        public IEnumerable<Episode> GetAll()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Add(Podcast podcast, IEnumerable<Episode> episodes)
        {
            throw new NotImplementedException();
        }
    }
}
