using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    public class FeedParserRoguesTest
    {
        private Podcast podcast = null;
        private string xml = null;
        private FeedParser feedParser = null;
        
        [SetUp]
        public void Init()
        {
            podcast = new Podcast {Uri = "http://something.com"};
            xml = File.ReadAllText(@"../../Fixtures/ruby-rogues.xml");
            feedParser = new FeedParser();
        }

        [Test]
        public void FeedParserReturnsPopulatedPodcast()
        {

            podcast = feedParser.Parse(podcast, xml);

            Assert.AreEqual(podcast.Title, "Ruby Rogues");
            Assert.AreEqual(podcast.Summary, "Rubyist.where(:rogue => true).limit(6).all.talk(:about => Topics.where(:awesome => true))");
            Assert.AreEqual(podcast.Image, "http://rubyrogues.com/wp-content/uploads/2013/05/RubyRogues_iTunes.jpg");
        }

        [Test]
        public void FeedParserReturnsEpisodeCollection()
        {
            podcast = feedParser.Parse(podcast, xml);
            
            Assert.True(podcast.Episodes.Any());
        }

        [Test]
        public void FeedParserReturnsValidEpisode()
        {
            podcast = feedParser.Parse(podcast, xml);
            Episode episode = podcast.Episodes.First();

            Assert.AreEqual(episode.Title, "139 RR Riak with Sean Cribbs and Bryce Kerley");
            Assert.AreEqual(episode.PublicationDate, Convert.ToDateTime("Wed, 15 Jan 2014 12:00:57 +0000"));
            Assert.AreEqual(episode.Link, "http://rubyrogues.com/139-rr-riak-with-sean-cribbs-and-bryce-kerley/");
            Assert.AreEqual(episode.MediaLink, "http://traffic.libsyn.com/rubyrogues/RR139Riak.mp3");
            Assert.AreEqual(episode.MediaDuration, 69286507);
            Assert.AreEqual(episode.MediaType, "audio/mpeg");
        }
    }
}
