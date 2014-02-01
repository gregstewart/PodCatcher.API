using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using PodCatcher.API.Models;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    public class FeedFetcherTest
    {
        private FeedFetcher _mFeedFetcher = null;
        [SetUp]
        public void Init()
        {
            _mFeedFetcher = new FeedFetcher();
        }

        [Test]
        public void FetcheedReturnsOk()
        {
            HttpResponseMessage response = _mFeedFetcher.GetFeed("http://gregs.tcias.co.uk/atom.xml");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void FetcheedReturnsNotFound()
        {
            HttpResponseMessage response = _mFeedFetcher.GetFeed("http://www.tcias.co.uk/1");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test, Ignore]
        public void FetcheedReturnsServerError()
        {
            HttpResponseMessage response = _mFeedFetcher.GetFeed("http://someurl.com");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [Test]
        public void FetchFeedWithNullUriReturnsBadRequest()
        {
            HttpResponseMessage response = _mFeedFetcher.GetFeed(null);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }
        [Test]
        public void FetchFeedWithEmptyUriReturnsBadRequest()
        {
            HttpResponseMessage response = _mFeedFetcher.GetFeed("");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }
    }
}
