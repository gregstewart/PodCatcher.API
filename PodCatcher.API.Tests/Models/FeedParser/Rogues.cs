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
    public class FeedParserRoguesTest
    {
        private Podcast podcast = null;
        private PodcastFeed podcastFeed = null;
        private string xml = null;
        private FeedParser feedParser = null;
        
        [SetUp]
        public void Init()
        {
            podcast = new Podcast {Uri = "http://something.com"};
            podcastFeed = new PodcastFeed {Podcast = podcast};
//            xml = File.ReadAllText(@"../../Fixtures/ruby-rogues.xml");
            xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><rss version=\"2.0\" xmlns:content=\"http://purl.org/rss/1.0/modules/content/\" xmlns:wfw=\"http://wellformedweb.org/CommentAPI/\"	xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:atom=\"http://www.w3.org/2005/Atom\" xmlns:sy=\"http://purl.org/rss/1.0/modules/syndication/\" xmlns:slash=\"http://purl.org/rss/1.0/modules/slash/\" xmlns:itunes=\"http://www.itunes.com/dtds/PodcastFeed-1.0.dtd\" xmlns:rawvoice=\"http://www.rawvoice.com/rawvoiceRssModule/\"><channel>	<title>Ruby Rogues</title>	<atom:link href=\"http://rubyrogues.com/feed/\" rel=\"self\" type=\"application/rss+xml\" />	<link>http://rubyrogues.com</link>	<description>Rubyist.where(:rogue =&#62; true).limit(6).all.talk(:about =&#62; Topics.where(:awesome =&#62; true))</description>	<lastBuildDate>Thu, 16 Jan 2014 19:39:52 +0000</lastBuildDate>	<language>en-US</language>		<sy:updatePeriod>hourly</sy:updatePeriod>		<sy:updateFrequency>1</sy:updateFrequency>	<generator>http://wordpress.org/?v=3.7.1</generator><!-- podcast_generator=\"Blubrry PowerPress/5.0.2\" mode=\"advanced\" -->	<itunes:new-feed-url>http://rubyrogues.com/PodcastFeed.rss</itunes:new-feed-url>	<itunes:summary>Rubyist.where(:rogue =&gt; true).limit(6).all.talk(:about =&gt; Topics.where(:awesome =&gt; true))</itunes:summary>	<itunes:author>Charles Max Wood, James Edward Gray II, David Brady, Avdi Grimm, Josh Susser, Katrina Owen</itunes:author>	<itunes:explicit>no</itunes:explicit>	<itunes:image href=\"http://rubyrogues.com/wp-content/uploads/2013/05/RubyRogues_iTunes.jpg\" />	<itunes:owner>		<itunes:name>Charles Max Wood, James Edward Gray II, David Brady, Avdi Grimm, Josh Susser, Katrina Owen</itunes:name>		<itunes:email>chuck@devchat.tv</itunes:email>	</itunes:owner>	<managingEditor>chuck@devchat.tv (Charles Max Wood, James Edward Gray II, David Brady, Avdi Grimm, Josh Susser, Katrina Owen)</managingEditor>	<itunes:subtitle>Rubyist.where(:rogue =&gt; true).limit(6).all.talk(:about =&gt; Topics.where(:awesome =&gt; true))</itunes:subtitle>	<itunes:keywords>ruby, rails, programmer, programming, technology, learn, code, coder, tools, computers</itunes:keywords>	<image>		<title>Ruby Rogues</title>		<url>http://rubyrogues.com/wp-content/uploads/2011/09/iTunesLogo.png</url>		<link>http://rubyrogues.com</link>	</image>	<itunes:category text=\"Technology\">		<itunes:category text=\"Software How-To\" />	</itunes:category>	<item>		<title>139 RR Riak with Sean Cribbs and Bryce Kerley</title>		<link>http://rubyrogues.com/139-rr-riak-with-sean-cribbs-and-bryce-kerley/</link>		<comments>http://rubyrogues.com/139-rr-riak-with-sean-cribbs-and-bryce-kerley/#comments</comments>		<pubDate>Wed, 15 Jan 2014 12:00:57 +0000</pubDate>		<dc:creator><![CDATA[Charles Max Wood]]></dc:creator>				<category><![CDATA[Avdi Grimm]]></category>		<category><![CDATA[Bryce Kerley]]></category>		<category><![CDATA[Charles Max Wood]]></category>		<category><![CDATA[David Brady]]></category>		<category><![CDATA[James Edward Gray II]]></category>		<category><![CDATA[Sean Cribbs]]></category>		<guid isPermaLink=\"false\">http://rubyrogues.com/?p=1673</guid>		<description><![CDATA[Get your Ruby Rogues T-Shirt or hoodie!! Ladies&#8217; sizes available as well! Panel Sean Cribbs (twitter github blog) Bryce Kerley (twitter github blog) Avdi Grimm (twitter github blog book) James Edward Gray (twitter github blog) David Brady (twitter github blog ADDcasts) Charles Max Wood (twitter github Teach Me To Code Rails Ramp Up) Discussion 01:32 [&#8230;]]]></description>		<wfw:commentRss>http://rubyrogues.com/139-rr-riak-with-sean-cribbs-and-bryce-kerley/feed/</wfw:commentRss>		<slash:comments>1</slash:comments><enclosure url=\"http://traffic.libsyn.com/rubyrogues/RR139Riak.mp3\" length=\"69286507\" type=\"audio/mpeg\" />		<itunes:subtitle>Get your Ruby Rogues T-Shirt or hoodie!! Ladies&#039; sizes available as well! Panel  Sean Cribbs (twitter github blog)   Bryce Kerley (twitter github blog)   Avdi Grimm (twitter github blog book)   James Edward Gray (twitter github blog) </itunes:subtitle>		<itunes:summary>Get your Ruby Rogues T-Shirt or hoodie!! Ladies&#039; sizes available as well!Panel	Sean Cribbs (twitter github blog)	Bryce Kerley (twitter github blog)	Avdi Grimm (twitter github blog book)	James Edward Gray (twitter github blog)	David Brady (twitter github blog ADDcasts)	Charles Max Wood (twitter github Teach Me To Code Rails Ramp Up)Discussion01:32 - Job Replacement Guide by David Brady03:28 - Sean Cribbs Introduction04:31 - Bryce Kerley Introduction04:45 - Riak and Advantages	Dynamo: Amazon’s Highly Available Key-value Store08:51 - The CAP Theorem	Code Hale: You Can’t Sacrifice Partition Tolerance10:27 - What is Riak?	The Ring14:07 - Introducing Riak 2.0: Data Types, Strong Consistency, Full-Text Search, and Much More	Sean Cribbs - Eventually Consistent Data Structures16:05 - Autocomplete27:50 - Scaling30:02 - Guidelines for Designing Code37:39 - HTTP 2.0 Support41:40 - MapReduce46:24 - Full-Text Search	yokozuna	riak-yz-query49:50 - Primary Data Store	Datomic52:27 - Programming RiakPicks	IRCCloud (Avdi)	One Today (Avdi)	Code Climate: When Is It Time to Refactor? (James)	sandi_meter (James)	Gone Home (James)	MailChimp (David)	The Gentle Art of Verbal Self-Defense at Work by Suzette H. Elgin (David)	Metro Franchise Pack (David)	Charity Navigator (David)	Disneyland (Chuck)	New Media Expo (Chuck)	Om (Sean)	QuickCheck (Erlang): QuivQ (Sean)	proper (Sean)	triq (Sean)	(Clojure): simple-check (Sean)	(Ruby): rantly (Sean)	Peter Bailis (@pbailis)(Sean)	Bastion Soundtrack (Sean)	Kennedy Space Center (Bryce)	Project Kenai (Bryce)	Atlantis (Bryce)Book ClubRuby Under a Microscope by Pat Shaughnessy! We will be interviewing Pat on February 27, 2014. The episode will air on March 6th, 2014. No Starch was kind enough to provide this coupon code your listeners can use to get a discount for Ruby Under a Microscope. Use the coupon code ROGUE for 40% off! (Coupon expires April 1, 2014.)Next WeekHeroku with Richard SchneemanTranscriptDAVID:  If Liz was up and dressed, I’d send her to the mailbox and then I could squee in the middle of the show, “My shirt came! My shirt came!”[Laughter][Hosting and bandwidth provided by the Blue Box Group. Check them out at BlueBox.net.] [This PodcastFeed is sponsored by New Relic. To track and optimize your application performance, go to RubyRogues.com/NewRelic.][This episode is sponsored by Code Climate. Code Climate automated code reviews ensure that your projects stay on track. Fix and find quality and security issues in your Ruby code sooner. Try it free at RubyRogues.com/CodeClimate.][This episode is sponsored by SendGrid, the leader in transactional email and email deliverability. SendGrid helps eliminate the cost and complexity of owning and maintaining your own email infrastructure by handling ISP monitoring, DKIM, SPF, feedback loops, whitelabeling, link customization and more. If you’d rather focus on your business than on scaling your email infrastructure, then visit www.SendGrid.com.]CHUCK:  Hey everybody and welcome to episode 139 of the Ruby Rogues PodcastFeed. This week on our panel, we have Avdi Grimm.AVDI:  Hello.CHUCK:  James Edward Gray.JAMES:  I’m Batman.CHUCK:  David Brady.DAVID:  Wait, I thought I was Batman.CHUCK:  I’m Charles Max Wood from DevChat.TV. We also have two special guests this week, Sean Cribbs.SEAN:  Howdy.CHUCK:  And Bryce Kerley.BRYCE:  Good afternoon.CHUCK:  Before we get started, I’m hearing that Dave has an announcement he’d like to make. So, I’m going to let him go and then we’ll have you guys introduce yourselves.DAVID:  Thanks, Chuck. So, we always do introductions and then Chuck tells us what course he’s working on. And I wanted to do that today. I’m putting together an emergency job replacement guide. Basically,</itunes:summary>		<itunes:author>Charles Max Wood, James Edward Gray II, David Brady, Avdi Grimm, Josh Susser, Katrina Owen</itunes:author>		<itunes:explicit>no</itunes:explicit>		<itunes:duration>1:12:10</itunes:duration>	</item>	</channel></rss>";
            feedParser = new FeedParser();
        }

        [Test]
        public void FeedParserReturnsPopulatedPodcast()
        {

            podcastFeed = feedParser.Parse(podcastFeed, xml);

            Assert.AreEqual(podcastFeed.Podcast.Title, "Ruby Rogues");
            Assert.AreEqual(podcastFeed.Podcast.Summary, "Rubyist.where(:rogue => true).limit(6).all.talk(:about => Topics.where(:awesome => true))");
            Assert.AreEqual(podcastFeed.Podcast.Image, "http://rubyrogues.com/wp-content/uploads/2013/05/RubyRogues_iTunes.jpg");
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

            Assert.AreEqual(episode.Title, "139 RR Riak with Sean Cribbs and Bryce Kerley");
            Assert.AreEqual(episode.PublicationDate, Convert.ToDateTime("Wed, 15 Jan 2014 12:00:57 +0000"));
            Assert.AreEqual(episode.Link, "http://rubyrogues.com/139-rr-riak-with-sean-cribbs-and-bryce-kerley/");
            Assert.AreEqual(episode.MediaLink, "http://traffic.libsyn.com/rubyrogues/RR139Riak.mp3");
            Assert.AreEqual(episode.MediaDuration, 69286507);
            Assert.AreEqual(episode.MediaType, "audio/mpeg");
        }
    }
}
