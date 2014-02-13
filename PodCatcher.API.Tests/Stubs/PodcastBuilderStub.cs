using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Tests.Stubs
{
    class PodcastBuilderStub : IPodcastBuilder
    {
        public PodcastFeed ToReturn;
        public PodcastFeed Build(Podcast podcast)
        {
            if (ToReturn != null)
            {
                return ToReturn;
            }
            throw new NotImplementedException();
        }
    }
}
