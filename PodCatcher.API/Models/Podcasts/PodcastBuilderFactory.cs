namespace PodCatcher.API.Models
{
    public class PodcastBuilderFactory
    {
        private static IPodcastBuilder _podcastBuilder = null;

        public static IPodcastBuilder Create()
        {
            if (_podcastBuilder != null)
                return _podcastBuilder;

            return new PodcastBuilder();
        }

        public static void SetPodcastBuilder(IPodcastBuilder podcastBuilder)
        {
            _podcastBuilder = podcastBuilder;
        }
    }
}