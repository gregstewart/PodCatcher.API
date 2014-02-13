using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebPages;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public class PodcastBuilder : IPodcastBuilder
    {
        private readonly IFeedFetcher _feedFetcher;
        private readonly IFeedParser _feedParser;

        public PodcastBuilder()
        {
            _feedFetcher = FeedFetcherFactory.Create();
            _feedParser = FeedParserFactory.Create();
        }

        public PodcastFeed Build(Podcast podcast)
        {
            if (podcast == null || podcast.Uri.IsEmpty())
            {
                throw new ArgumentNullException("item");
            }

            PodcastFeed podcastFeed = new PodcastFeed {Podcast = podcast};

            Feed feed = _feedFetcher.GetFeed(podcast.Uri);
            if (feed.StatusCode == HttpStatusCode.OK)
            {
                var xml = feed.Content ?? "";
                podcastFeed = _feedParser.Parse(podcastFeed, xml);
                podcastFeed.Podcast.Id = Guid.NewGuid();
                return podcastFeed;
            }
            else
            {
                throw new HttpResponseException(feed.StatusCode);
            }
            
        }
    }
}