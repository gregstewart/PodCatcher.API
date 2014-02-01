using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NUnit.Framework;
using PodCatcher.API.Models;
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
            
            PodcastBuilder podcastBuilder = new PodcastBuilder();
            Podcast podcast = podcastBuilder.Build(httpWwwTestComFeedXml);
            
            Assert.IsInstanceOf(typeof (Podcast), podcast);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Build_WithNullURI_ThrowsArgumentNullException()
        {
            PodcastBuilder podcastBuilder = new PodcastBuilder();
            Podcast podcast = podcastBuilder.Build(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Build_WithEmptyURI_ThrowsArgumentNullException()
        {
            PodcastBuilder podcastBuilder = new PodcastBuilder();
            Podcast podcast = podcastBuilder.Build("");
        }

        [Test]
        [ExpectedException(typeof(HttpResponseException))]
        public void Build_WithValidUrlIsNotFound_IsNull()
        {
            _mFeedFetcher.ToReturn = new HttpResponseMessage(HttpStatusCode.NotFound);
            PodcastBuilder podcastBuilder = new PodcastBuilder();
            Podcast podcast = podcastBuilder.Build("http://rubyrogues.com/feed/");
        }
    }
}
