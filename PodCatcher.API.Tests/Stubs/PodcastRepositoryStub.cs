using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Tests.Stubs
{
    public class PodcastRepositoryStub : IPodcastRepository
    {
        public Podcast PodcastToBeReturned;
        public Exception ToBeThrown;

        public IEnumerable<Podcast> GetAll()
        {
            throw new NotImplementedException();
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
            return PodcastToBeReturned;
        }

        public Podcast Add(Podcast podcast)
        {
            if (ToBeThrown != null)
            {
                throw ToBeThrown;
            }
            
            return PodcastToBeReturned;
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
