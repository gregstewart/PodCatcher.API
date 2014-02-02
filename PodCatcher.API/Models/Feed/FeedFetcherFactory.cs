namespace PodCatcher.API.Models
{
    public class FeedFetcherFactory
    {
        private static IFeedFetcher _feedFetcher = null;

        public static IFeedFetcher Create()
        {
            if (_feedFetcher != null)
                return _feedFetcher;

            return new FeedFetcher();
        }

        public static void SetFeedParser(IFeedFetcher feedParser)
        {
            _feedFetcher = feedParser;
        }
    }
}