using System;
using System.IO;
using System.Linq;
using System.Net;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    public class FeedParserInstanceTest
    {
        private Podcast podcast = null;
        private string xml = null;
        private FeedParser feedParser = null;

        [SetUp]
        public void Init()
        {
            podcast = new Podcast { Uri = "http://something.com" };
            xml = File.ReadAllText(@"../../Fixtures/instance-rss.xml");
            feedParser = new FeedParser();
        }

        [Test]
        public void FeedParserReturnsPopulatedPodcast()
        {

            podcast = feedParser.Parse(podcast, xml);

            Assert.AreEqual(podcast.Title, "The Instance: World of Warcraft Podcast!");
            Assert.AreEqual(podcast.Summary, "The Instance: Weekly radio for fans and lovers of World of Warcraft. We don't take sides, we don't whine, we just give you the facts, news and tips that you want and need for your favorite online addiction. Come meet us at the stone for another Instance!");
            Assert.AreEqual(podcast.Image, "http://www.myextralife.com/ftp/radio/instance_01.jpg");
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

            Assert.AreEqual(episode.Title, "354 - The Instance: Boosting Early");
            Assert.AreEqual(episode.PublicationDate, Convert.ToDateTime("Fri, 17 Jan 2014 12:19:02 -0700"));
            Assert.AreEqual(episode.Link, "http://www.podtrac.com/pts/redirect.mp3/feeds.soundcloud.com/stream/130013461-scott-johnson-27-instance-354.mp3");
            Assert.AreEqual(episode.MediaLink, "http://www.podtrac.com/pts/redirect.mp3/feeds.soundcloud.com/stream/130013461-scott-johnson-27-instance-354.mp3");
            Assert.AreEqual(episode.MediaDuration, 0);
            Assert.AreEqual(episode.MediaType, "audio/mpeg");
        }
    }
}
