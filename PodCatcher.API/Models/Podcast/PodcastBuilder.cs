using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebPages;

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

        public Podcast Build(string Uri)
        {
            if (Uri.IsEmpty())
            {
                throw new ArgumentNullException("item");
            }

            Podcast podcast = new Podcast {Uri = Uri};

            Feed feed = _feedFetcher.GetFeed(podcast.Uri);
            if (feed.StatusCode == HttpStatusCode.OK)
            {
                var xml = feed.Content ?? "";
                podcast = _feedParser.Parse(podcast, xml);
                podcast.Id = Guid.NewGuid();
                return podcast;
            }
            else
            {
                throw new HttpResponseException(feed.StatusCode);
            }
            
        }
    }
}