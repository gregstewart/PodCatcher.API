using NLog;

namespace PodCatcher.API.Models
{
    public class PodcastLogger : ILogger
    {
        private static Logger _log;

        public PodcastLogger()
        {
            _log = LogManager.GetCurrentClassLogger();
        }
        
        public void Debug(string message)
        {
            _log.Debug(message);
        }

        public void Info(string message)
        {
            _log.Info(message);
        }

        public void Warn(string message)
        {
            _log.Warn(message);
        }

        public void Error(string message)
        {
            _log.Error(message);
        }

        public void Fatal(string message)
        {
            _log.Fatal(message);
        }
    }
}