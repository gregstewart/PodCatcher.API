using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public interface IPodcastBuilder
    {
        PodcastFeed Build(Podcast podcast);
    }
}
