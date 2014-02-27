using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Tests.Models
{
    class ConvertToRaid
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

//            xml = File.ReadAllText(@"..\..\Fixtures\convert-to-raid.xml");
            xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><rss version=\"2.0\" xmlns:atom=\"http://www.w3.org/2005/Atom\" xmlns:cc=\"http://web.resource.org/cc/\" xmlns:itunes=\"http://www.itunes.com/dtds/podcast-1.0.dtd\" xmlns:media=\"http://search.yahoo.com/mrss/\" xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\">  <channel>    <atom:link href=\"http://converttoraid.libsyn.com/rss\" rel=\"self\" type=\"application/rss+xml\"/>    <title>Convert to Raid: The podcast for raiders in World of Warcraft</title>    <pubDate>Mon, 17 Feb 2014 16:59:59 +0000</pubDate>    <lastBuildDate>Mon, 17 Feb 2014 18:14:13 +0000</lastBuildDate>    <generator>Libsyn WebEngine 2.0</generator>    <link>http://converttoraid.libsyn.com</link>    <language>en</language>    <copyright><![CDATA[Signals Media]]></copyright>    <docs>http://converttoraid.libsyn.com</docs>    <managingEditor>converttoraid@gmail.com (converttoraid@gmail.com)</managingEditor>    <description><![CDATA[Convert to Raid examines everything about the end game in World of Warcraft.  Join our panel of avid raiders each episode and keep up to date on the latest buzz from Azeroth.  From noob to pro, we'll keep you in the loop on all the latest additions and changes that affect you most!]]></description>    <image>      <url>http://assets.libsyn.com/content/5472375.jpg</url>      <title>Convert to Raid: The podcast for raiders in World of Warcraft</title>      <link><![CDATA[http://converttoraid.libsyn.com]]></link>    </image>    <itunes:author>Pat Krane</itunes:author>    <itunes:keywords>alliance,games,gaming,horde,of,raiding,video,warcraft,world</itunes:keywords>    <itunes:category text=\"Games &amp; Hobbies\">      <itunes:category text=\"Video Games\"/>    </itunes:category>    <itunes:image href=\"http://assets.libsyn.com/content/5472375.jpg\" />    <itunes:explicit>clean</itunes:explicit>    <itunes:owner>      <itunes:name><![CDATA[Pat Krane]]></itunes:name>      <itunes:email>converttoraid@gmail.com</itunes:email>    </itunes:owner>    <itunes:summary><![CDATA[Convert to Raid is the podcast for raiders in World of Warcraft!  Our panel of avid players will talk about the latest buzz in the game and how it affects the end game.]]></itunes:summary>    <itunes:subtitle><![CDATA[Convert to Raid]]></itunes:subtitle>    <item>      <title>#131 - Convert to Raid: The End is Nigh!</title>      <pubDate>Mon, 17 Feb 2014 16:59:59 +0000</pubDate>      <guid isPermaLink=\"false\"><![CDATA[19b3975cc2cd6029e3444a667018c913]]></guid>      <link><![CDATA[http://converttoraid.libsyn.com/131-convert-to-raid-the-end-is-nigh]]></link>      <itunes:image href=\"http://assets.libsyn.com/content/6851196\" />      <description>        <![CDATA[<p dir=\"ltr\" style=\"line-height:1.5;margin-top:0pt;margin-bottom:11pt;\">&nbsp;</p><p dir=\"ltr\" style=\"line-height:1.5;margin-top:0pt;margin-bottom:11pt;\"><span style=\"font-size:15px;font-family:Arial;color:#000000;background-color:transparent;font-weight:normal;font-style:normal;font-variant:normal;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;\">The Convert to Raid crew are back in action to give you the hottest news for raiders in World of Warcraft! </span></p><p dir=\"ltr\" style=\"line-height:1.5;margin-top:0pt;margin-bottom:11pt;\"><span style=\"font-size:15px;font-family:Arial;color:#000000;background-color:transparent;font-weight:normal;font-style:normal;font-variant:normal;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;\">This week, Lead Encounter Designer Ion Hazzikostas dropped a ton of news, including some minor nerfs to the 10 man heroic version of the Garrosh encounter. While this has been an issue for awhile, now is the time to make some adjustments to ensure that the difficulty between 10 and 25 is better aligned. </span></p><p dir=\"ltr\" style=\"line-height:1.5;margin-top:0pt;margin-bottom:11pt;\"><span style=\"font-size:15px;font-family:Arial;color:#000000;background-color:transparent;font-weight:normal;font-style:normal;font-variant:normal;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;\">The Challenge Mode dungeon &ldquo;season&rdquo; is ending soon, according to Blizzard. But did anyone ever tell the general public that this was a &ldquo;season&rdquo; in the first place? Why did Blizzard pull the plug? How have players been reacting to the news, and how can Blizzard improve on this scaled 5 man content in Warlords?</span></p><p dir=\"ltr\" style=\"line-height:1.5;margin-top:0pt;margin-bottom:11pt;\"><span style=\"font-size:15px;font-family:Arial;color:#000000;background-color:transparent;font-weight:normal;font-style:normal;font-variant:normal;text-decoration:none;vertical-align:baseline;white-space:pre-wrap;\">And finally, reported just before this show recorded, patch 5.4.7 should be landing this week in the World of Warcraft! This new patch brings the start of the next PvP season, several class changes and preparation for the Level 90 Boosts and Pre-Order for Warlords of Draenor. Not everything may be available right away, but we expect to hear more news about the Pre-Order soon&trade;.</span></p><p><span id=\"docs-internal-guid-114b57d5-40c6-753c-33cf-21616de8c172\"><span style=\"font-size: 15px; font-family: Arial; background-color: transparent; vertical-align: baseline; white-space: pre-wrap;\">Also, The Scroll of Resurrection Retires, garrison art is revealed by Blizzard, casters may have fewer instants in WoD, and more!</span></span></p>]]>      </description>      <enclosure length=\"104221432\" type=\"audio/mpeg\" url=\"http://traffic.libsyn.com/converttoraid/ctr_131.mp3\" />      <itunes:duration>01:47:31</itunes:duration>      <itunes:explicit>clean</itunes:explicit>    <itunes:keywords />    <itunes:subtitle><![CDATA[Challenge Modes, Scroll of Resurrection End, Patch 5.4.7 to Start This Week]]></itunes:subtitle>    </item></channel></rss>";
            feedParser = new FeedParser();
        }

        [Test]
        public void FeedParserReturnsPopulatedPodcast()
        {

            podcastFeed = feedParser.Parse(podcastFeed, xml);

            Assert.AreEqual(podcastFeed.Podcast.Title, "Convert to Raid: The podcast for raiders in World of Warcraft");
            Assert.AreEqual(podcastFeed.Podcast.Summary, "Convert to Raid examines everything about the end game in World of Warcraft.  Join our panel of avid raiders each episode and keep up to date on the latest buzz from Azeroth.  From noob to pro, we'll keep you in the loop on all the latest additions and changes that affect you most!");
            Assert.AreEqual(podcastFeed.Podcast.Image, "http://assets.libsyn.com/content/5472375.jpg");
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

            Assert.AreEqual(episode.Title, "#131 - Convert to Raid: The End is Nigh!");
            Assert.AreEqual(episode.PublicationDate, Convert.ToDateTime("Mon, 17 Feb 2014 16:59:59 +0000"));
            Assert.AreEqual(episode.Link, "http://converttoraid.libsyn.com/131-convert-to-raid-the-end-is-nigh");
            Assert.AreEqual(episode.MediaLink, "http://traffic.libsyn.com/converttoraid/ctr_131.mp3");
            Assert.AreEqual(episode.MediaDuration, 104221432);
            Assert.AreEqual(episode.MediaType, "audio/mpeg");
            Assert.AreEqual(episode.Subtitle, "Challenge Modes, Scroll of Resurrection End, Patch 5.4.7 to Start This Week");
            Assert.AreEqual(episode.Summary, null);
            Assert.AreEqual(episode.Author, null);
            Assert.AreEqual(episode.Duration, "01:47:31");
        }
    }
}
