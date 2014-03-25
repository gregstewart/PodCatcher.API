namespace PodCatcher.API.Models
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

            return new LogEntriesLogger();
        }

        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }
    }
}