using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodCatcher.API.Models;

namespace PodCatcher.API.Tests.Stubs
{
    public class PodcastRepositoryStub : IPodcastRepository
    {
        public Podcast PodcastToBeReturned;

        public IEnumerable<Podcast> GetAll()
        {
            throw new NotImplementedException();
        }

        public Podcast Get(Guid Id)
        {
            return PodcastToBeReturned;
        }

        public Podcast Add(Podcast item)
        {
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
