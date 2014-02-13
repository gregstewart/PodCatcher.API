using System;
using System.Collections.Generic;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public interface IPodcastRepository
    {
        IEnumerable<Podcast> GetAll();
        IEnumerable<Podcast> GetAll(IEnumerable<Podcast> podcasts);
        Podcast Get(Podcast podcast);
        Podcast Get(Guid Id);
        Podcast Add(Podcast podcast);
        void Remove(Guid Id);
        bool Update(Podcast item);
    }
}
