namespace PodCatcher.API.Models
{
    public interface IFeedParser
    {
        Podcast Parse(Podcast podcast, string xml);
    }
}
