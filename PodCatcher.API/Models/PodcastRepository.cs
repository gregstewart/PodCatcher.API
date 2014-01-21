using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodCatcher.API.Models
{
    public class PodcastRepository : IPodcastRepository
    {
        private List<Podcast> feeds = new List<Podcast>();

        public PodcastRepository()
        {
            Add(new Podcast { Uri = "http://rubyrogues.com/feed/"});
            Add(new Podcast { Uri = "http://wow.joystiq.com/category/wow-insider-show/rss.xml" });
            Add(new Podcast { Uri = "http://pwop.com/feed.aspx?show=hanselminutes&filetype=master" });
            Add(new Podcast { Uri = "http://feeds.feedburner.com/JavascriptJabber" });
            Add(new Podcast { Uri = "http://converttoraid.libsyn.com/rss" });
            Add(new Podcast { Uri = "http://www.myextralife.com/ftp/radio/instance_rss.xml" });
        }

        public IEnumerable<Podcast> GetAll()
        {
            return feeds;
        }

        public Podcast Get(Guid id)
        {
            return feeds.Find(p => p.Id == id);
        }

        public Podcast Add(Podcast item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item.Id = Guid.NewGuid();
            feeds.Add(item);

            return item;
        }

        public void Remove(Guid id)
        {
            feeds.RemoveAll(p => p.Id == id);
        }

        public bool Update(Podcast item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = feeds.FindIndex(p => p.Id == item.Id);
            if (index == -1)
            {
                return false;
            }
            feeds.RemoveAt(index);
            feeds.Add(item);
            return true;
        }
    }
}