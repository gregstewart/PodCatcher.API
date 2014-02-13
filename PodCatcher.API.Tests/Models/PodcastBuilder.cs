using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Podcasts;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    public class PodcastBuilderTest
    {
        private FeedFetcherStub _mFeedFetcher;
        private FeedParserStub _mFeedParser;
        
        [SetUp]
        public void Init()
        {
            _mFeedFetcher = new FeedFetcherStub();
            _mFeedParser = new FeedParserStub();
            FeedParserFactory.SetFeedParser(_mFeedParser);
            FeedFetcherFactory.SetFeedParser(_mFeedFetcher);
        }

        [Test]
        public void Build_WithValidURI_IsSuccessful()
        {
            var httpWwwTestComFeedXml = "http://www.test.com/feed.xml";
            Podcast podcast = new Podcast {Uri = httpWwwTestComFeedXml};
            
            PodcastBuilder podcastBuilder = new PodcastBuilder();
            PodcastFeed podcastFeed = podcastBuilder.Build(podcast);
            
            Assert.IsInstanceOf(typeof (PodcastFeed), podcastFeed);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Build_WithNullURI_ThrowsArgumentNullException()
        {
            PodcastBuilder podcastBuilder = new PodcastBuilder();
            PodcastFeed podcastFeed = podcastBuilder.Build(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Build_WithEmptyPodcast_ThrowsArgumentNullException()
        {
            Podcast podcast = new Podcast();
            PodcastBuilder podcastBuilder = new PodcastBuilder();
            PodcastFeed podcastFeed = podcastBuilder.Build(podcast);
        }

        [Test]
        [ExpectedException(typeof(HttpResponseException))]
        public void Build_WithValidUrlIsNotFound_IsNull()
        {
            _mFeedFetcher.ToReturn = HttpStatusCode.NotFound;
            var httpWwwTestComFeedXml = "http://rubyrogues.com/feed/";
            Podcast podcast = new Podcast { Uri = httpWwwTestComFeedXml };
            PodcastBuilder podcastBuilder = new PodcastBuilder();
            PodcastFeed podcastFeed = podcastBuilder.Build(podcast);
        }
    }
}
