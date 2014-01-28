using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
