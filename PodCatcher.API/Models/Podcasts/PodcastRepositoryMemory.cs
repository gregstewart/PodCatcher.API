using System;
using System.Collections.Generic;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public class PodcastRepositoryMemory : IPodcastRepository
    {
        private readonly List<Podcast> podcasts = new List<Podcast>();
        
        public PodcastRepositoryMemory()
        {

//            Add(new PodcastFeed { Uri = "http://rubyrogues.com/feed/"});
//            Add(new PodcastFeed { Uri = "http://wow.joystiq.com/category/wow-insider-show/rss.xml" });
//            Add(new PodcastFeed { Uri = "http://pwop.com/feed.aspx?show=hanselminutes&filetype=master" });
//            Add(new PodcastFeed { Uri = "http://feeds.feedburner.com/JavascriptJabber" });
//            Add(new PodcastFeed { Uri = "http://converttoraid.libsyn.com/rss" });
//            Add(new PodcastFeed { Uri = "http://www.myextralife.com/ftp/radio/instance_rss.xml" });
        }

        public IEnumerable<Podcast> GetAll()
        {
            return podcasts;
        }

        public IEnumerable<Podcast> GetAll(IEnumerable<Podcast> podcasts)
        {
            throw new NotImplementedException();
        }

        public Podcast Get(Podcast podcast)
        {
            throw new NotImplementedException();
        }

        public Podcast Get(Guid id)
        {
            return podcasts.Find(p => p.Id == id);
        }

        public Podcast Get(string title)
        {
            throw new NotImplementedException();
        }

        public Podcast Add(Podcast podcast)
        {
            if (podcast == null)
            {
                throw new ArgumentNullException("podcastFeed");
            }
            podcasts.Add(podcast);
            return podcast;
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