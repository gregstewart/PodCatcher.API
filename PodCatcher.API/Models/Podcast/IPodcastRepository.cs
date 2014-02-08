using System;
using System.Collections.Generic;

namespace PodCatcher.API.Models
{
    public interface IPodcastRepository
    {
        IEnumerable<Podcast> GetAll();
        IEnumerable<Podcast> GetAll(IEnumerable<Podcast> podcasts);
        Podcast Get(Podcast podcast);
        Podcast Get(Guid Id);
        Podcast Add(Podcast item);
        void Remove(Guid Id);
        bool Update(Podcast item);
    }
}
