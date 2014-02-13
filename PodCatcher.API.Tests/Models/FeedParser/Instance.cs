using System;
using System.IO;
using System.Linq;
using System.Net;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Podcasts;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    public class FeedParserInstanceTest
    {
        private Podcast podcast = null;
        private PodcastFeed podcastFeed = null;
        private string xml = null;
        private FeedParser feedParser = null;

        [SetUp]
        public void Init()
        {
            podcast = new Podcast { Uri = "http://something.com" };
            podcastFeed = new PodcastFeed { Podcast = podcast };

//            xml = File.ReadAllText(@"..\..\Fixtures\instance-rss.xml");
            xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><rss xmlns:atom=\"http://www.w3.org/2005/Atom\" xmlns:itunes=\"http://www.itunes.com/dtds/PodcastFeed-1.0.dtd\" version=\"2.0\">    <channel>        <title>The Instance: World of Warcraft PodcastFeed!</title>        <link>http://myextralife.com/wow</link>        <description>The Instance: Weekly radio for fans and lovers of World of Warcraft. We don't take sides, we don't whine, we just give you the facts, news and tips that you want and need for your favorite online addiction. Come meet us at the stone for another Instance!</description>        <generator>Feeder 2.3.9(1722); Mac OS X Version 10.6.8 (Build 10K549) http://reinventedsoftware.com/feeder/</generator>        <docs>http://blogs.law.harvard.edu/tech/rss</docs>        <language>en-us</language>        <copyright>2005 Scott Johnson</copyright>        <managingEditor>theinstance@gmail.com</managingEditor>        <webMaster>theinstance@gmail.com</webMaster>        <pubDate>Fri, 17 Jan 2014 12:19:08 -0700</pubDate>        <lastBuildDate>Fri, 17 Jan 2014 12:19:08 -0700</lastBuildDate>        <category>Technology</category>        <category>Gaming</category>        <image>            <url>http://www.myextralife.com/ftp/radio/instance_01.jpg</url>            <title>The Instance</title>            <link>http://myextralife.com/wow</link>            <width>300</width>            <height>300</height>        </image>        <atom:link href=\"http://myextralife.com/ftp/radio/instance_rss.xml\" rel=\"self\" type=\"application/rss+xml\"/>        <itunes:author>Scott Johnson</itunes:author>        <itunes:subtitle>The Instance: Weekly radio for fans and lovers of World of Warcraft.</itunes:subtitle>        <itunes:summary>The Instance: Weekly radio for fans and lovers of World of Warcraft. We don't take sides, we don't whine, we just give you the facts, news and tips that you want and need for your favorite online addiction. Come meet us at the stone for another Instance!</itunes:summary>        <itunes:image href=\"http://www.myextralife.com/ftp/radio/instance_01.jpg\"/>        <itunes:explicit>no</itunes:explicit>        <itunes:owner>            <itunes:name>Scott Johnson</itunes:name>            <itunes:email>theinstance@gmail.com</itunes:email>        </itunes:owner>        <itunes:block>no</itunes:block>        <itunes:category text=\"Games &amp; Hobbies\">            <itunes:category text=\"Video Games\"/>        </itunes:category>        <itunes:category text=\"Technology\">            <itunes:category text=\"Tech News\"/>        </itunes:category>        <itunes:category text=\"Arts\"/>        <item>            <title>354 - The Instance: Boosting Early</title>            <link>http://www.podtrac.com/pts/redirect.mp3/feeds.soundcloud.com/stream/130013461-scott-johnson-27-instance-354.mp3</link>            <description><![CDATA[On this episode of The Instance, picking that boost character, rusty high dps, pre-pay for WOD and get early rewards, Greg Street is a real Riot at parties, HS balance patch is here, is grinding fun, why you left the community, new talents in retro, Silvermoon is doomed, WoW cards in HS, blizzard says NO to keys, boosting ideas, Paula Patton center stage, and more!]]></description>            <pubDate>Fri, 17 Jan 2014 12:19:02 -0700</pubDate>            <enclosure url=\"http://www.podtrac.com/pts/redirect.mp3/feeds.soundcloud.com/stream/130013461-scott-johnson-27-instance-354.mp3\" length=\"0\" type=\"audio/mpeg\"/>            <guid isPermaLink=\"false\">7D39436C-0C13-47E4-BA69-2C6D2432E96C</guid>            <itunes:author>Scott Johnson</itunes:author>            <itunes:subtitle>On this episode of The Instance, picking that boost character, rusty high dps, pre-pay for WOD and get early rewards, Greg Street is a real Riot at parties, HS balance patch is here, is grinding fun, why you left the c...</itunes:subtitle>            <itunes:summary>On this episode of The Instance, picking that boost character, rusty high dps, pre-pay for WOD and get early rewards, Greg Street is a real Riot at parties, HS balance patch is here, is grinding fun, why you left the community, new talents in retro, Silvermoon is doomed, WoW cards in HS, blizzard says NO to keys, boosting ideas, Paula Patton center stage, and more!</itunes:summary>            <itunes:explicit>no</itunes:explicit>            <itunes:duration>1:28:00</itunes:duration>        </item>    </channel></rss>";
            feedParser = new FeedParser();
        }

        [Test]
        public void FeedParserReturnsPopulatedPodcast()
        {

            podcastFeed = feedParser.Parse(podcastFeed, xml);

            Assert.AreEqual(podcastFeed.Podcast.Title, "The Instance: World of Warcraft PodcastFeed!");
            Assert.AreEqual(podcastFeed.Podcast.Summary, "The Instance: Weekly radio for fans and lovers of World of Warcraft. We don't take sides, we don't whine, we just give you the facts, news and tips that you want and need for your favorite online addiction. Come meet us at the stone for another Instance!");
            Assert.AreEqual(podcastFeed.Podcast.Image, "http://www.myextralife.com/ftp/radio/instance_01.jpg");
        }

        [Test]
        public void FeedParserReturnsEpisodeCollection()
        {
            podcastFeed = feedParser.Parse(podcastFeed, xml);

            Assert.True(podcastFeed.Episodes.Any());
        }

        [Test]
        public void FeedParserReturnsValidEpisode()
        {
            podcastFeed = feedParser.Parse(podcastFeed, xml);
            Episode episode = podcastFeed.Episodes.First();

            Assert.AreEqual(episode.Title, "354 - The Instance: Boosting Early");
            Assert.AreEqual(episode.PublicationDate, Convert.ToDateTime("Fri, 17 Jan 2014 12:19:02 -0700"));
            Assert.AreEqual(episode.Link, "http://www.podtrac.com/pts/redirect.mp3/feeds.soundcloud.com/stream/130013461-scott-johnson-27-instance-354.mp3");
            Assert.AreEqual(episode.MediaLink, "http://www.podtrac.com/pts/redirect.mp3/feeds.soundcloud.com/stream/130013461-scott-johnson-27-instance-354.mp3");
            Assert.AreEqual(episode.MediaDuration, 0);
            Assert.AreEqual(episode.MediaType, "audio/mpeg");
        }
    }
}
