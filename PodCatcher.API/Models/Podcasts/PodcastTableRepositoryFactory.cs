namespace PodCatcher.API.Models
{
    public class PodcastTableRepositoryFactory
    {
        private static IPodcastRepository _podcastRepository = null;

        public static IPodcastRepository Create()
        {
            if (_podcastRepository != null)
            {
                return _podcastRepository;
            }

            return new PodcastTableRepository();
        }

        public static void SetPodcastRepository(IPodcastRepository podcastRepository)
        {
            _podcastRepository = podcastRepository;
        }

    }
}