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
            Podcast podcast = null;
            if (Uri.IsEmpty())
            {
                throw new ArgumentNullException("item");
            }

            podcast = new Podcast {Uri = Uri};

            HttpResponseMessage response = _feedFetcher.GetFeed(podcast.Uri);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var xml = response.Content == null ? "" : response.Content.ToString();
                podcast = _feedParser.Parse(podcast, xml);
                podcast.Id = Guid.NewGuid();
                return podcast;
            }
            else
            {
                throw new HttpResponseException(response.StatusCode);
            }
            
        }
    }
}