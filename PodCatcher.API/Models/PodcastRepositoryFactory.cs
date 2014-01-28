using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodCatcher.API.Models
{
    public class PodcastRepositoryFactory
    {
        private static IPodcastRepository _podcastRepository = null;

        public static IPodcastRepository Create()
        {
            if (_podcastRepository != null)
                return _podcastRepository;

            return new PodcastRepository();
        }

        public static void SetPodcastRepository(IPodcastRepository podcastRepository)
        {
            _podcastRepository = podcastRepository;
        }

    }
}