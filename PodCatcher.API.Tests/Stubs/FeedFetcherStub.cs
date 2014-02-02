using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using PodCatcher.API.Models;

namespace PodCatcher.API.Tests.Stubs
{
    public class FeedFetcherStub : IFeedFetcher
    {
        public HttpStatusCode ToReturn = HttpStatusCode.OK;
        public Feed GetFeed(string Uri)
        {
            Feed feed = new Feed();
            
            feed.StatusCode = ToReturn;
            
            return feed;
            
        }
    }
}
