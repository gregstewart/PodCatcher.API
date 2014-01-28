using System;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    public class FeedFetcher
    {
        private FeedFetcherStub _mFeedFetcherWrapper = null;
        [SetUp]
        public void Init()
        {
            _mFeedFetcherWrapper = new FeedFetcherStub();
        }

        [Test]
        public void FetcheedReturnsOk()
        {
            _mFeedFetcherWrapper.ToReturn = new HttpResponseMessage(HttpStatusCode.OK);

            HttpResponseMessage response = _mFeedFetcherWrapper.GetFeed("http://someurl.com");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public void FetcheedReturnsNotFound()
        {
            _mFeedFetcherWrapper.ToReturn = new HttpResponseMessage(HttpStatusCode.NotFound);

            HttpResponseMessage response = _mFeedFetcherWrapper.GetFeed("http://someurl.com");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void FetcheedReturnsServerError()
        {
            _mFeedFetcherWrapper.ToReturn = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            HttpResponseMessage response = _mFeedFetcherWrapper.GetFeed("http://someurl.com");

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [Test]
        public void FetchFeedWithNullUrieturnBadRequest()
        {
            _mFeedFetcherWrapper.ToReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);

            HttpResponseMessage response = _mFeedFetcherWrapper.GetFeed(null);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }
    }
}
