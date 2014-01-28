using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    public class PodcastRepositoryTest
    {
        private FeedFetcherStub _mFeedFetcher = null;
        private FeedParserStub _mFeedParser;
        private PodcastRepository _mPodcastRepository;

        [SetUp]
        public void Init()
        {
            _mFeedFetcher = new FeedFetcherStub();
            _mFeedParser = new FeedParserStub();
            FeedParserFactory.SetFeedParser(_mFeedParser);
            FeedFetcherFactory.SetFeedParser(_mFeedFetcher);
            _mPodcastRepository = new PodcastRepository();
        }

        [Test]
        public void AddPodcast_WithValidUrl_IsSuccessful()
        {
            _mFeedFetcher.ToReturn = new HttpResponseMessage(HttpStatusCode.OK);

            HttpResponseMessage response = _mFeedFetcher.GetFeed("http://someurl.com");

            _mPodcastRepository.Add(new Podcast { Uri = "http://rubyrogues.com/feed/" });

            var enumerable = _mPodcastRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 1, "Failed to add new Podcast");
        }

        public void AddPodcast_WithValidUrl_IsNotFoundShouldNotAddToCollection()
        {
            _mFeedFetcher.ToReturn = new HttpResponseMessage(HttpStatusCode.NotFound);

            HttpResponseMessage response = _mFeedFetcher.GetFeed("http://someurl.com");

            _mPodcastRepository.Add(new Podcast { Uri = "http://rubyrogues.com/feed/" });

            var enumerable = _mPodcastRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 0, "Failed to add new Podcast");
        }
    }

}
