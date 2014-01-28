using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace PodCatcher.API.Models
{
    public class FeedFetcher : IFeedFetcher
    {
        
        public HttpResponseMessage GetFeed(string Uri)
        {
            throw new NotImplementedException();
        }
    }
}