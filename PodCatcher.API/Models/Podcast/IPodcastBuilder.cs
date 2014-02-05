namespace PodCatcher.API.Models
{
    public interface IPodcastBuilder
    {
        Podcast Build(string Uri);
    }
}
