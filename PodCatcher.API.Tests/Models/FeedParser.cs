using System;
using System.IO;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    public class FeedParserTest
    {
        private FeedFetcherWrapperStub _mFeedFetcherWrapper = null;
        
        [SetUp]
        public void Init()
        {
//            _mFeedFetcherWrapper = new FeedFetcherWrapperStub();
        }

        [Test]
        public void FeedParserReturnsPopulatedPodcast()
        {
            Podcast podcast = new Podcast {Uri = "http://something.com"};
            string xml = File.ReadAllText(@"../../Fixtures/ruby-rogues.xml");

//            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
//            httpResponseMessage.Content = new StringContent(xml);
//            _mFeedFetcherWrapper.ToReturn = httpResponseMessage;
//            
            FeedParser feedParser = new FeedParser();
            podcast = feedParser.Parse(podcast, xml);

            Assert.AreEqual(podcast.Title, "Ruby Rogues");
            Assert.AreEqual(podcast.Summary, "Rubyist.where(:rogue => true).limit(6).all.talk(:about => Topics.where(:awesome => true))");
            Assert.AreEqual(podcast.Image, "http://rubyrogues.com/wp-content/uploads/2013/05/RubyRogues_iTunes.jpg");
        }
    }
}
