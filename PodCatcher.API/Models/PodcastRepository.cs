using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI.WebControls;

namespace PodCatcher.API.Models
{
    public class PodcastRepository : IPodcastRepository
    {
        private readonly List<Podcast> podcasts = new List<Podcast>();
        private readonly IFeedFetcher _feedFetcher;
        private readonly IFeedParser _feedParser;

        public PodcastRepository()
        {
            _feedFetcher = FeedFetcherFactory.Create();
            _feedParser = FeedParserFactory.Create();

//            Add(new Podcast { Uri = "http://rubyrogues.com/feed/"});
//            Add(new Podcast { Uri = "http://wow.joystiq.com/category/wow-insider-show/rss.xml" });
//            Add(new Podcast { Uri = "http://pwop.com/feed.aspx?show=hanselminutes&filetype=master" });
//            Add(new Podcast { Uri = "http://feeds.feedburner.com/JavascriptJabber" });
//            Add(new Podcast { Uri = "http://converttoraid.libsyn.com/rss" });
//            Add(new Podcast { Uri = "http://www.myextralife.com/ftp/radio/instance_rss.xml" });
        }

        public IEnumerable<Podcast> GetAll()
        {
            return podcasts;
        }

        public Podcast Get(Guid id)
        {
            return podcasts.Find(p => p.Id == id);
        }

        public Podcast Add(Podcast item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            HttpResponseMessage response = _feedFetcher.GetFeed(item.Uri);
            var xml = response.Content == null ? "" : response.Content.ToString();
            item = _feedParser.Parse(item, xml);
            item.Id = Guid.NewGuid();
            podcasts.Add(item);

            return item;
        }

        public void Remove(Guid id)
        {
            podcasts.RemoveAll(p => p.Id == id);
        }

        public bool Update(Podcast item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = podcasts.FindIndex(p => p.Id == item.Id);
            if (index == -1)
            {
                return false;
            }
            podcasts.RemoveAt(index);
            podcasts.Add(item);
            return true;
        }
    }
}