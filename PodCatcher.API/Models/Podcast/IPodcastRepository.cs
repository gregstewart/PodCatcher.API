using System;
using System.Collections.Generic;

namespace PodCatcher.API.Models
{
    public interface IPodcastRepository
    {
        IEnumerable<Podcast> GetAll();
        Podcast Get(Guid Id);
        Podcast Add(Podcast item);
        void Remove(Guid Id);
        bool Update(Podcast item);
    }
}
