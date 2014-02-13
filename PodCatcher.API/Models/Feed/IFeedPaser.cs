namespace PodCatcher.API.Models
{
    public interface IFeedParser
    {
        PodcastFeed Parse(PodcastFeed podcastFeed, string xml);
    }
}
