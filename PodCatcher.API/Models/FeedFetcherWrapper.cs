using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Services.Description;

namespace PodCatcher.API.Models
{
    public class FeedFetcherWrapper : IFeedFetcherWrapper
    {
        private FeedFetcher _feedFetcher;

        public FeedFetcherWrapper(FeedFetcher feedFecther)
        {
            _feedFetcher = feedFecther;
        }

        public HttpResponseMessage GetFeed(string Uri)
        {
            return _feedFetcher.Get(Uri);
        }
    }

    public class FeedFetcher
    {
        public HttpResponseMessage Get(string Uri)
        {
            throw new NotImplementedException();
        }
    }
}