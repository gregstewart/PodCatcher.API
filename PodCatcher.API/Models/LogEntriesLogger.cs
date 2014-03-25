using log4net;
using log4net.Repository.Hierarchy;
using PodCatcher.API.Controllers;

namespace PodCatcher.API.Models
{
    public class LogEntriesLogger : ILogger
    {
        private static ILog log = LogManager.GetLogger(typeof(PodcastsController));

        public void Debug(string message)
        {
            log.Debug(message);
        }

        public void Info(string message)
        {
            log.Info(message);
        }

        public void Warn(string message)
        {
            log.Warn(message);
        }

        public void Error(string message)
        {
            log.Error(message);
        }

        public void Fatal(string message)
        {
            log.Fatal(message);
        }
    }
}