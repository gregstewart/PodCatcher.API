using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodCatcher.API.Models
{
    public class FeedParserFactory
    {
        private static IFeedParser _feedParser = null;

        public static IFeedParser Create()
        {
            if (_feedParser != null)
                return _feedParser;

            return new FeedParser();
        }

        public static void SetFeedParser(IFeedParser feedParser)
        {
            _feedParser = feedParser;
        }
    }
}