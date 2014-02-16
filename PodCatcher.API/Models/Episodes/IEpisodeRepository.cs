using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models.Episodes
{
    public interface IEpisodeRepository
    {
        IEnumerable<Episode> GetAll();
        IEnumerable<Episode> GetAll(string podcastTitle);
        IEnumerable<Episode> GetAll(Guid podcastGuid);
        void Add(Episode episode);
        void Add(Podcast podcast, Episode episode);
        void Add(Podcast podcast, IEnumerable<Episode> episodes);
    }
}
