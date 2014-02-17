using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Tests.Models
{
    class DateParsingFromFeed
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

            xml = "<rss xmlns:itunes=\"http://www.itunes.com/dtds/podcast-1.0.dtd\" xmlns:atom=\"http://www.w3.org/2005/Atom\" version=\"2.0\"><channel><ttl>30</ttl><title>Hanselminutes</title><link>http://www.hanselminutes.com</link><language>en-us</language><description>Hanselminutes is a weekly audio talk show with noted web developer and technologist Scott Hanselman and hosted by Carl Franklin. Scott discusses utilities and tools, gives practical how-to advice, and discusses ASP.NET or Windows issues and workarounds.</description><copyright>Copyright © 2006-2014 by Pwop Productions</copyright><managingEditor>scott@hanselman.com (Scott Hanselman)</managingEditor><webMaster>scott@hanselman.com (Scott Hanselman)</webMaster><rating>G</rating><pubDate>Mon, 17 Feb 2014 21:35:11 EDT</pubDate><lastBuildDate>Mon, 17 Feb 2014 21:35:11 EDT</lastBuildDate><cloud domain=\"http://www.hanselminutes.com\" port=\"80\" path=\"/RPC2\" registerProcedure=\"pingMe\" protocol=\"soap\"></cloud><atom:link href=\"http://www.pwop.com%2ffeed.aspx%3fshow%3dhanselminutes%26filetype%3dmaster\" rel=\"self\" type=\"application/rss+xml\"/><image><url>http://www.pwop.com/itunes_hanselminutes.jpg</url><title>Hanselminutes</title><link>http://www.hanselminutes.com</link><width>144</width><height>300</height><description>Hanselminutes is a weekly audio talk show with noted web developer and technologist Scott Hanselman and hosted by Carl Franklin. Scott discusses utilities and tools, gives practical how-to advice, and discusses ASP.NET or Windows issues and workarounds.</description></image><category>Programming</category><category>Talk</category><category>ASP.NET</category><category>Podcast</category><itunes:subtitle>Hanselminutes is a weekly audio talk show with noted web developer and technologist Scott Hanselman and hosted by Carl Franklin. Scott discusses utilities and tools, gives practical how-to advice, and discusses ASP.NET or Windows issues and workarounds.</itunes:subtitle><itunes:explicit>no</itunes:explicit><itunes:author>Scott Hanselman</itunes:author><itunes:summary>Hanselminutes is a weekly audio talk show with noted web developer and technologist Scott Hanselman and hosted by Carl Franklin. Scott discusses utilities and tools, gives practical how-to advice, and discusses ASP.NET or Windows issues and workarounds.</itunes:summary><itunes:owner><itunes:name>Scott Hanselman</itunes:name><itunes:email>scott@hanselman.com (Scott Hanselman)</itunes:email></itunes:owner><itunes:image href=\"http://www.pwop.com/itunes_hanselminutes.jpg\"/><itunes:category text=\"Technology\"><itunes:category text=\"Software How-To\"/></itunes:category><itunes:category text=\"Technology\"><itunes:category text=\"Gadgets\"/></itunes:category><itunes:category text=\"Technology\"><itunes:category text=\"Software How-To\"/></itunes:category><item><title>Xbox One Developer with Dave Voyles, formerly of Comcast</title><link>http://www.hanselminutes.com/default.aspx?ShowID=5424</link><pubDate>Fri, 14 Feb 2014 00:00:00 EDT</pubDate><description>Scott talks with Dave Voyles who worked on the Comcast Xfinity application for Xbox. What's it take to write an application for an Xbox One? Will your HTML and JavaScript skills translate? All this, plus discussion of SmartGlass.</description><source url=\"http://www.hanselminutes.com/default.aspx?ShowID=5424\">Hanselminutes</source><guid isPermaLink=\"true\">http://www.hanselminutes.com/default.aspx?ShowID=5424</guid><itunes:author>Scott Hanselman</itunes:author><itunes:subtitle>Hanselminutes is a weekly audio talk show with noted web developer and technologist Scott Hanselman and hosted by Carl Franklin. Scott discusses utilities and tools, gives practical how-to advice, and discusses ASP.NET or Windows issues and workarounds.</itunes:subtitle><itunes:summary>Scott talks with Dave Voyles who worked on the Comcast Xfinity application for Xbox. What's it take to write an application for an Xbox One? Will your HTML and JavaScript skills translate? All this, plus discussion of SmartGlass.</itunes:summary><enclosure url=\"http://s3.amazonaws.com/hanselminutes/hanselminutes_0410.mp3\" length=\"35630980\" type=\"audio/mp3\"/><itunes:duration>00:32:00</itunes:duration><itunes:keywords/></item></channel></rss>";
            feedParser = new FeedParser();
        }

        [Test]
        public void FeedParser_WithEDTDate_ReturnsValidPodcast()
        {
            podcastFeed = feedParser.Parse(podcastFeed, xml);
            Episode episode = podcastFeed.Episodes.First();

            Assert.AreEqual(episode.PublicationDate, Convert.ToDateTime("2014-02-14 04:00:00.000"));
        }
    }
}

