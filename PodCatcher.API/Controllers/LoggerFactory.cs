namespace PodCatcher.API.Controllers
{
    public class LoggerFactory
    {
        private static ILogger _logger;
        public static ILogger Create()
        {
            if (_logger != null)
            {
                return _logger;
            }

            return new PodcastLogger();
        }

        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }
    }
}