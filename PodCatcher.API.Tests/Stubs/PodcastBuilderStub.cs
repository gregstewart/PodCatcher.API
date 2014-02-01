using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodCatcher.API.Models;

namespace PodCatcher.API.Tests.Stubs
{
    class PodcastBuilderStub : IPodcastBuilder
    {
        public Podcast ToReturn;
        public Podcast Build(string Uri)
        {
            if (ToReturn != null)
            {
                return ToReturn;
            }
            throw new NotImplementedException();
        }
    }
}
