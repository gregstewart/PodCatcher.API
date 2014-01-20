using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodCatcher.API.Models
{
    public class FeedRepository : IFeedRepository
    {
        private List<Feed> feeds = new List<Feed>();

        public FeedRepository()
        {
            Add(new Feed { Uri = "http://rubyrogues.com/feed/"});
            Add(new Feed { Uri = "http://wow.joystiq.com/category/wow-insider-show/rss.xml" });
            Add(new Feed { Uri = "http://pwop.com/feed.aspx?show=hanselminutes&filetype=master" });
            Add(new Feed { Uri = "http://feeds.feedburner.com/JavascriptJabber" });
            Add(new Feed { Uri = "http://converttoraid.libsyn.com/rss" });
            Add(new Feed { Uri = "http://www.myextralife.com/ftp/radio/instance_rss.xml" });
        }

        public IEnumerable<Feed> GetAll()
        {
            return feeds;
        }

        public Feed Get(Guid id)
        {
            return feeds.Find(p => p.Id == id);
        }

        public Feed Add(Feed item)
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

        public bool Update(Feed item)
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